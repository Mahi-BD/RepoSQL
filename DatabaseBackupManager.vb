Imports Microsoft.Data.SqlClient
Imports System.IO
Imports System.IO.Compression
Imports System.Threading.Tasks
Imports System.Diagnostics
Imports System.Security.Principal
Imports System.Environment

''' <summary>
''' Fixed Database backup manager that resolves SQL Server permission issues
''' Uses SQL Server accessible directories and proper permission handling
''' </summary>
Public Class DatabaseBackupManager

#Region "Events and Delegates"

    ''' <summary>
    ''' Event raised when backup progress is updated
    ''' </summary>
    Public Event ProgressUpdated(message As String, percentage As Integer)

    ''' <summary>
    ''' Event raised when status message should be displayed
    ''' </summary>
    Public Event StatusUpdated(message As String)

#End Region

#Region "Properties"

    ''' <summary>
    ''' Connection string for the database
    ''' </summary>
    Public Property ConnectionString As String

    ''' <summary>
    ''' Database name to backup
    ''' </summary>
    Public Property DatabaseName As String

    ''' <summary>
    ''' Output directory for the final zip file
    ''' </summary>
    Public Property OutputDirectory As String

    ''' <summary>
    ''' Whether to overwrite existing files without prompting
    ''' </summary>
    Public Property OverwriteExistingFiles As Boolean = True

    ''' <summary>
    ''' Backup timeout in seconds (default: 15 minutes)
    ''' </summary>
    Public Property BackupTimeoutSeconds As Integer = 900

#End Region

#Region "Structures"

    ''' <summary>
    ''' SQL Server version information structure
    ''' </summary>
    Private Structure SqlServerVersionInfo
        Public MajorVersion As Integer
        Public Edition As String
        Public EngineEdition As Integer
        Public SupportsCompression As Boolean
        Public SupportsChecksum As Boolean
        Public SupportsStats As Boolean
    End Structure

    ''' <summary>
    ''' Backup result information
    ''' </summary>
    Public Structure BackupResult
        Public Success As Boolean
        Public ErrorMessage As String
        Public BackupSizeMB As Double
        Public CompressedSizeMB As Double
        Public CompressionRatio As Double
        Public FinalFilePath As String
        Public FileName As String
        Public TimeTaken As TimeSpan
        Public BackupDirectory As String
    End Structure

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Create database backup using SQL Server accessible directories
    ''' </summary>
    ''' <returns>Backup result information</returns>
    Public Async Function CreateBackupAsync() As Task(Of BackupResult)
        Dim result As New BackupResult()
        Dim startTime As DateTime = DateTime.Now
        Dim sqlBackupPath As String = ""
        Dim sqlBackupDirectory As String = ""

        Try
            ' Validate inputs
            ValidateInputs()

            ' Initialize result
            result.Success = False

            ' Create timestamp and file names
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim backupFileName As String = $"{DatabaseName}_{timestamp}.bak"
            Dim zipFileName As String = $"{DatabaseName}_Backup_{timestamp}.zip"

            result.FileName = zipFileName

            ' Get SQL Server accessible backup directory
            sqlBackupDirectory = Await GetSqlServerAccessibleDirectoryAsync()
            sqlBackupPath = Path.Combine(sqlBackupDirectory, backupFileName)
            result.BackupDirectory = sqlBackupDirectory

            RaiseEvent StatusUpdated($"Using SQL Server backup directory: {sqlBackupDirectory}")

            ' Define final zip path
            Dim finalZipPath As String = Path.Combine(OutputDirectory, zipFileName)
            result.FinalFilePath = finalZipPath

            ' Step 1: Create database backup directly in SQL Server accessible location
            RaiseEvent ProgressUpdated("Creating database backup...", 10)
            RaiseEvent StatusUpdated("Creating database backup in SQL Server accessible directory...")

            Await CreateDatabaseBackupAsync(sqlBackupPath)

            ' Step 2: Verify backup and get size
            If Not File.Exists(sqlBackupPath) Then
                Throw New Exception("Backup file was not created successfully.")
            End If

            Dim backupFileInfo As New FileInfo(sqlBackupPath)
            result.BackupSizeMB = Math.Round(backupFileInfo.Length / (1024.0 * 1024.0), 2)

            RaiseEvent ProgressUpdated("Backup created successfully", 40)
            RaiseEvent StatusUpdated($"Backup file created: {backupFileInfo.Length:N0} bytes")

            ' Step 3: Create zip file directly in output directory
            RaiseEvent ProgressUpdated("Compressing backup file...", 50)
            RaiseEvent StatusUpdated("Compressing backup file to zip archive...")

            Await CreateZipFileAsync(sqlBackupPath, finalZipPath)

            ' Step 4: Verify zip and calculate compression
            If Not File.Exists(finalZipPath) Then
                Throw New Exception("Zip file was not created successfully.")
            End If

            Dim zipFileInfo As New FileInfo(finalZipPath)
            result.CompressedSizeMB = Math.Round(zipFileInfo.Length / (1024.0 * 1024.0), 2)
            result.CompressionRatio = Math.Round((1 - (zipFileInfo.Length / CDbl(backupFileInfo.Length))) * 100, 1)

            RaiseEvent ProgressUpdated("Compression completed", 80)

            ' Step 5: Clean up the SQL backup file
            RaiseEvent ProgressUpdated("Cleaning up temporary files...", 90)
            RaiseEvent StatusUpdated("Cleaning up temporary backup file...")

            Try
                File.Delete(sqlBackupPath)
                RaiseEvent StatusUpdated("Temporary backup file cleaned up successfully")
            Catch ex As Exception
                RaiseEvent StatusUpdated($"Warning: Could not delete temporary backup file: {ex.Message}")
                ' Don't fail the operation for cleanup issues
            End Try

            RaiseEvent ProgressUpdated("Backup completed successfully", 100)

            ' Calculate time taken
            result.TimeTaken = DateTime.Now - startTime
            result.Success = True

            RaiseEvent StatusUpdated($"Database backup completed successfully: {zipFileName}")

        Catch ex As SqlException
            result.Success = False
            result.ErrorMessage = GetEnhancedSqlErrorMessage(ex)
            RaiseEvent StatusUpdated("Database backup failed due to SQL Server error.")
        Catch ex As UnauthorizedAccessException
            result.Success = False
            result.ErrorMessage = $"Access denied: {ex.Message}" & vbCrLf & vbCrLf &
                                 "Solution: Run this application as Administrator or ensure SQL Server service has proper permissions."
            RaiseEvent StatusUpdated("Database backup failed due to permission error.")
        Catch ex As Exception
            result.Success = False
            result.ErrorMessage = ex.Message
            RaiseEvent StatusUpdated("Database backup failed.")
        Finally
            ' Additional cleanup - try to remove any leftover backup files
            If Not String.IsNullOrEmpty(sqlBackupPath) AndAlso File.Exists(sqlBackupPath) Then
                Try
                    File.Delete(sqlBackupPath)
                Catch
                    ' Ignore cleanup errors
                End Try
            End If
        End Try

        Return result
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Get a directory that SQL Server can definitely write to
    ''' </summary>
    Private Async Function GetSqlServerAccessibleDirectoryAsync() As Task(Of String)
        Try
            Using connection As New SqlConnection(ConnectionString)
                Await connection.OpenAsync()

                ' Try method 1: Get SQL Server default backup directory
                Dim defaultBackupDir As String = Await GetSqlServerDefaultBackupDirectoryAsync(connection)
                If Not String.IsNullOrEmpty(defaultBackupDir) AndAlso Directory.Exists(defaultBackupDir) Then
                    RaiseEvent StatusUpdated($"Found SQL Server default backup directory: {defaultBackupDir}")
                    Return defaultBackupDir
                End If

                ' Try method 2: Get SQL Server data directory
                Dim dataDirectory As String = Await GetSqlServerDataDirectoryAsync(connection)
                If Not String.IsNullOrEmpty(dataDirectory) AndAlso Directory.Exists(dataDirectory) Then
                    RaiseEvent StatusUpdated($"Using SQL Server data directory: {dataDirectory}")
                    Return dataDirectory
                End If

                ' Try method 3: Get SQL Server installation directory
                Dim installDir As String = Await GetSqlServerInstallDirectoryAsync(connection)
                If Not String.IsNullOrEmpty(installDir) Then
                    Dim backupDir As String = Path.Combine(installDir, "Backup")
                    If Directory.Exists(backupDir) Then
                        RaiseEvent StatusUpdated($"Using SQL Server installation backup directory: {backupDir}")
                        Return backupDir
                    End If
                End If

            End Using

        Catch ex As Exception
            RaiseEvent StatusUpdated($"Warning: Could not determine SQL Server directories: {ex.Message}")
        End Try

        ' Fallback: Use a common directory that usually works
        Dim fallbackDirectories() As String = {
            "C:\Program Files\Microsoft SQL Server\MSSQL\Backup",
            "C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Backup",
            "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\Backup",
            "C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\Backup",
            "C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\Backup",
            "C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Backup",
            "C:\Temp",
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        }

        For Each dir As String In fallbackDirectories
            If Directory.Exists(dir) Then
                Try
                    ' Test write access
                    Dim testFile As String = Path.Combine(dir, $"test_{Guid.NewGuid().ToString("N").Substring(0, 8)}.tmp")
                    File.WriteAllText(testFile, "test")
                    File.Delete(testFile)
                    RaiseEvent StatusUpdated($"Using fallback directory: {dir}")
                    Return dir
                Catch
                    ' Continue to next directory
                End Try
            End If
        Next

        ' Last resort: Create a directory in ProgramData
        Dim lastResortDir As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "RepoSQL", "Backup")
        Directory.CreateDirectory(lastResortDir)
        RaiseEvent StatusUpdated($"Created backup directory: {lastResortDir}")
        Return lastResortDir
    End Function

    ''' <summary>
    ''' Get SQL Server's default backup directory from registry
    ''' </summary>
    Private Async Function GetSqlServerDefaultBackupDirectoryAsync(connection As SqlConnection) As Task(Of String)
        Try
            Dim query As String = "
                DECLARE @BackupDirectory NVARCHAR(4000)
                EXEC master.dbo.xp_instance_regread 
                    N'HKEY_LOCAL_MACHINE',
                    N'SOFTWARE\Microsoft\MSSQLServer\MSSQLServer',
                    N'BackupDirectory',
                    @BackupDirectory OUTPUT
                SELECT ISNULL(@BackupDirectory, '') AS BackupDirectory"

            Using command As New SqlCommand(query, connection)
                command.CommandTimeout = 30
                Dim result As Object = Await command.ExecuteScalarAsync()
                If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso Not String.IsNullOrEmpty(result.ToString()) Then
                    Return result.ToString()
                End If
            End Using

        Catch ex As Exception
            RaiseEvent StatusUpdated($"Could not get SQL Server default backup directory: {ex.Message}")
        End Try

        Return String.Empty
    End Function

    ''' <summary>
    ''' Get SQL Server's data directory
    ''' </summary>
    Private Async Function GetSqlServerDataDirectoryAsync(connection As SqlConnection) As Task(Of String)
        Try
            Dim query As String = "
                SELECT 
                    SUBSTRING(physical_name, 1, LEN(physical_name) - LEN(REVERSE(SUBSTRING(REVERSE(physical_name), 1, CHARINDEX('\', REVERSE(physical_name)) - 1))) - 1) AS DataDirectory
                FROM sys.master_files 
                WHERE database_id = 1 AND type = 0"

            Using command As New SqlCommand(query, connection)
                command.CommandTimeout = 30
                Dim result As Object = Await command.ExecuteScalarAsync()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    Return result.ToString()
                End If
            End Using

        Catch ex As Exception
            RaiseEvent StatusUpdated($"Could not get SQL Server data directory: {ex.Message}")
        End Try

        Return String.Empty
    End Function

    ''' <summary>
    ''' Get SQL Server installation directory
    ''' </summary>
    Private Async Function GetSqlServerInstallDirectoryAsync(connection As SqlConnection) As Task(Of String)
        Try
            Dim query As String = "
                DECLARE @InstallDir NVARCHAR(4000)
                EXEC master.dbo.xp_instance_regread 
                    N'HKEY_LOCAL_MACHINE',
                    N'SOFTWARE\Microsoft\MSSQLServer\Setup',
                    N'SQLPath',
                    @InstallDir OUTPUT
                SELECT ISNULL(@InstallDir, '') AS InstallDir"

            Using command As New SqlCommand(query, connection)
                command.CommandTimeout = 30
                Dim result As Object = Await command.ExecuteScalarAsync()
                If result IsNot Nothing AndAlso Not IsDBNull(result) AndAlso Not String.IsNullOrEmpty(result.ToString()) Then
                    Return result.ToString()
                End If
            End Using

        Catch ex As Exception
            RaiseEvent StatusUpdated($"Could not get SQL Server installation directory: {ex.Message}")
        End Try

        Return String.Empty
    End Function

    ''' <summary>
    ''' Enhanced error message for SQL exceptions
    ''' </summary>
    Private Function GetEnhancedSqlErrorMessage(ex As SqlException) As String
        Dim message As String = $"SQL Server Error {ex.Number}: {ex.Message}"

        Select Case ex.Number
            Case 3201
                message &= vbCrLf & vbCrLf & "🔧 SOLUTION FOR ERROR 3201:"
                message &= vbCrLf & "This is a permission error. Try these steps:"
                message &= vbCrLf & "1. Run this application as Administrator"
                message &= vbCrLf & "2. Or use SQL Server Management Studio to create the backup"
                message &= vbCrLf & "3. Or ask your database administrator to grant backup permissions"
            Case 3202
                message &= vbCrLf & vbCrLf & "🔧 SOLUTION FOR ERROR 3202:"
                message &= vbCrLf & "Check file permissions and ensure the backup file is not in use."
            Case 5103
                message &= vbCrLf & vbCrLf & "🔧 SOLUTION FOR ERROR 5103:"
                message &= vbCrLf & "Invalid backup device. Try using a different backup location."
        End Select

        Return message
    End Function

    ''' <summary>
    ''' Validate input parameters
    ''' </summary>
    Private Sub ValidateInputs()
        If String.IsNullOrEmpty(ConnectionString) Then
            Throw New ArgumentException("Connection string is required.")
        End If

        If String.IsNullOrEmpty(DatabaseName) Then
            Throw New ArgumentException("Database name is required.")
        End If

        If String.IsNullOrEmpty(OutputDirectory) Then
            Throw New ArgumentException("Output directory is required.")
        End If

        If Not Directory.Exists(OutputDirectory) Then
            Try
                Directory.CreateDirectory(OutputDirectory)
            Catch ex As Exception
                Throw New ArgumentException($"Cannot create output directory: {ex.Message}")
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Create database backup file using SQL Server accessible path
    ''' </summary>
    ''' <param name="backupFilePath">Path where backup will be created</param>
    Private Async Function CreateDatabaseBackupAsync(backupFilePath As String) As Task
        Try
            Using connection As New SqlConnection(ConnectionString)
                Await connection.OpenAsync()

                ' Get SQL Server version information
                Dim sqlVersion As SqlServerVersionInfo = Await GetSqlServerVersionAsync(connection)

                ' Build backup command with proper path escaping
                Dim backupSql As String = BuildBackupCommand(sqlVersion, backupFilePath)

                RaiseEvent StatusUpdated($"Executing backup command...")
                RaiseEvent StatusUpdated($"Backup file path: {backupFilePath}")

                Using command As New SqlCommand(backupSql, connection)
                    command.CommandTimeout = BackupTimeoutSeconds

                    ' Handle SQL Server progress messages
                    AddHandler connection.InfoMessage, AddressOf OnSqlInfoMessage

                    Await command.ExecuteNonQueryAsync()

                    RemoveHandler connection.InfoMessage, AddressOf OnSqlInfoMessage
                End Using

                RaiseEvent StatusUpdated("Database backup completed successfully")
            End Using

        Catch ex As SqlException
            Dim enhancedError As String = GetEnhancedSqlErrorMessage(ex)
            Throw New Exception(enhancedError)
        Catch ex As Exception
            Throw New Exception($"Backup Error: {ex.Message}")
        End Try
    End Function

    ''' <summary>
    ''' Build backup SQL command with proper path escaping
    ''' </summary>
    Private Function BuildBackupCommand(versionInfo As SqlServerVersionInfo, backupFilePath As String) As String
        Dim sqlBuilder As New Text.StringBuilder()

        ' Ensure the path is properly escaped for SQL
        Dim escapedPath As String = backupFilePath.Replace("'", "''")

        sqlBuilder.AppendLine($"BACKUP DATABASE [{DatabaseName}]")
        sqlBuilder.AppendLine($"TO DISK = N'{escapedPath}'")
        sqlBuilder.Append("WITH FORMAT, INIT")

        If versionInfo.SupportsCompression Then
            sqlBuilder.Append(", COMPRESSION")
        End If

        If versionInfo.SupportsChecksum Then
            sqlBuilder.Append(", CHECKSUM")
        End If

        If versionInfo.SupportsStats Then
            sqlBuilder.Append(", STATS = 10")
        End If

        sqlBuilder.Append($", NAME = N'Full Backup of {DatabaseName} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}'")
        sqlBuilder.Append(", SKIP, NOREWIND, NOUNLOAD")

        Return sqlBuilder.ToString()
    End Function

    ''' <summary>
    ''' Get SQL Server version and capabilities
    ''' </summary>
    Private Async Function GetSqlServerVersionAsync(connection As SqlConnection) As Task(Of SqlServerVersionInfo)
        Dim versionInfo As New SqlServerVersionInfo()

        Try
            Dim versionQuery As String = "
                SELECT 
                    SERVERPROPERTY('ProductVersion') AS Version,
                    SERVERPROPERTY('Edition') AS Edition,
                    SERVERPROPERTY('EngineEdition') AS EngineEdition"

            Using command As New SqlCommand(versionQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    If Await reader.ReadAsync() Then
                        Dim version As String = reader.GetString("Version")
                        versionInfo.Edition = reader.GetString("Edition")
                        versionInfo.EngineEdition = reader.GetInt32("EngineEdition")

                        ' Parse major version
                        Dim versionParts() As String = version.Split("."c)
                        versionInfo.MajorVersion = Integer.Parse(versionParts(0))

                        ' Determine feature support
                        DetermineFeatureSupport(versionInfo)
                    End If
                End Using
            End Using

        Catch ex As Exception
            ' Default to SQL Server 2012 capabilities
            versionInfo.MajorVersion = 11
            versionInfo.Edition = "Unknown"
            versionInfo.EngineEdition = 1
            versionInfo.SupportsCompression = True
            versionInfo.SupportsChecksum = True
            versionInfo.SupportsStats = True
        End Try

        Return versionInfo
    End Function

    ''' <summary>
    ''' Determine SQL Server feature support
    ''' </summary>
    Private Sub DetermineFeatureSupport(ByRef versionInfo As SqlServerVersionInfo)
        ' CHECKSUM and STATS: Available since SQL Server 2005
        versionInfo.SupportsChecksum = (versionInfo.MajorVersion >= 9)
        versionInfo.SupportsStats = (versionInfo.MajorVersion >= 9)

        ' COMPRESSION support
        If versionInfo.EngineEdition = 4 Then
            ' Express Edition
            versionInfo.SupportsCompression = (versionInfo.MajorVersion >= 13)
        ElseIf versionInfo.MajorVersion >= 13 Then
            ' SQL Server 2016+
            versionInfo.SupportsCompression = True
        ElseIf versionInfo.MajorVersion >= 10 AndAlso
               (versionInfo.Edition.ToUpper().Contains("ENTERPRISE") OrElse
                versionInfo.Edition.ToUpper().Contains("DEVELOPER") OrElse
                versionInfo.Edition.ToUpper().Contains("EVALUATION")) Then
            versionInfo.SupportsCompression = True
        Else
            versionInfo.SupportsCompression = False
        End If
    End Sub

    ''' <summary>
    ''' Handle SQL Server info messages for progress updates
    ''' </summary>
    Private Sub OnSqlInfoMessage(sender As Object, e As SqlInfoMessageEventArgs)
        For Each info As SqlError In e.Errors
            If info.Message.Contains("percent") Then
                ' Extract percentage
                Dim percentageText As String = info.Message
                Dim percentMatch As System.Text.RegularExpressions.Match =
                    System.Text.RegularExpressions.Regex.Match(percentageText, "(\d+)\s*percent")

                Dim percentage As Integer = -1
                If percentMatch.Success Then
                    Integer.TryParse(percentMatch.Groups(1).Value, percentage)
                    ' Map SQL percentage to our progress range (10-40)
                    percentage = 10 + CInt((percentage / 100.0) * 30)
                End If

                RaiseEvent ProgressUpdated($"Database backup: {info.Message}", percentage)
            Else
                RaiseEvent StatusUpdated($"SQL Server: {info.Message}")
            End If
        Next
    End Sub

    ''' <summary>
    ''' Create zip file from backup file
    ''' </summary>
    Private Async Function CreateZipFileAsync(sourceFilePath As String, zipFilePath As String) As Task
        Await Task.Run(Sub()
                           Try
                               ' Delete existing zip if present
                               If File.Exists(zipFilePath) Then
                                   File.Delete(zipFilePath)
                               End If

                               ' Create zip archive
                               Using archive As ZipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create)
                                   Dim entryName As String = Path.GetFileName(sourceFilePath)
                                   archive.CreateEntryFromFile(sourceFilePath, entryName, CompressionLevel.Optimal)
                               End Using

                           Catch ex As Exception
                               Throw New Exception($"Zip Creation Error: {ex.Message}")
                           End Try
                       End Sub)
    End Function

#End Region

End Class