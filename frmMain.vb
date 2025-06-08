Imports Microsoft.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms
Imports System.Threading.Tasks
Imports System.Diagnostics

Public Class frmMain
    Private ReadOnly profileFilePath As String = Path.Combine(Application.StartupPath, "ProfileConfig.json")
    Private selectedTables As New List(Of String)
    Private connectionString As String = String.Empty
    Private allTables As New List(Of String)
    Private isConnected As Boolean = False

    ' Configuration class for JSON serialization
    Private Class ProfileConfig
        Public Property Server As String = "LSERVER"
        Public Property Username As String = "as"
        Public Property Password As String = "fobian"
        Public Property Database As String = "BinBookLiteDB"
        Public Property OutputFilePath As String = ""
        Public Property SelectedTables As List(Of String) = New List(Of String)
        Public Property Tables As Boolean = True
        Public Property Views As Boolean = True
        Public Property StoredProcedures As Boolean = True
        Public Property Functions As Boolean = True
        Public Property Indexes As Boolean = False
        Public Property Triggers As Boolean = False
        Public Property Permissions As Boolean = False
        Public Property IncludeData As Boolean = False
        Public Property IntegratedSecurity As Boolean = False
    End Class

#Region "Form Events"

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "RepoSQL - SQL Script Generator v2.0"
        InitializeForm()
        LoadProfile()
    End Sub

    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' Handle form resize if needed
        If Me.WindowState <> FormWindowState.Minimized Then
            ' Adjust layout if necessary
        End If
    End Sub

#End Region

#Region "Initialization"

    Private Sub InitializeForm()
        ' Set initial states
        pnlProgress.Visible = False
        toolStripProgressBar.Visible = False
        btnGenerate.Enabled = False
        btnLoadTables.Enabled = False
        btnOpenOutput.Enabled = False

        ' Set default output path if empty
        If String.IsNullOrEmpty(txtOutput.Text) Then
            txtOutput.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RepoSQL_Output")
        End If

        ' Initialize connection status
        UpdateConnectionStatus(False, "Not connected")

        ' Set up table search
        AddHandler txtTableSearch.TextChanged, AddressOf txtTableSearch_TextChanged

        ' Set up integrated security checkbox
        AddHandler chkIntegratedSecurity.CheckedChanged, AddressOf chkIntegratedSecurity_CheckedChanged

        ' Initial UI state
        chkIntegratedSecurity_CheckedChanged(Nothing, Nothing)
    End Sub

#End Region

#Region "Profile Management"

    Private Sub LoadProfile()
        Dim config As New ProfileConfig()

        ' Read from profile file if it exists
        If File.Exists(profileFilePath) Then
            Try
                Dim jsonString As String = File.ReadAllText(profileFilePath)
                config = JsonSerializer.Deserialize(Of ProfileConfig)(jsonString)
            Catch ex As Exception
                MessageBox.Show($"Error reading profile: {ex.Message}", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ' Use default config if error
            End Try
        Else
            ' Create new profile file with defaults
            SaveProfile(config)
        End If

        ' Apply values to controls
        txtServer.Text = config.Server
        txtUsername.Text = config.Username
        txtPassword.Text = config.Password
        txtDatabase.Text = config.Database
        txtOutput.Text = If(String.IsNullOrEmpty(config.OutputFilePath), txtOutput.Text, config.OutputFilePath)
        selectedTables = config.SelectedTables
        chkTables.Checked = config.Tables
        chkViews.Checked = config.Views
        chkStoredProcedures.Checked = config.StoredProcedures
        chkFunctions.Checked = config.Functions
        chkIndexes.Checked = config.Indexes
        chkTriggers.Checked = config.Triggers
        chkPermissions.Checked = config.Permissions
        chkIncludeData.Checked = config.IncludeData
        chkIntegratedSecurity.Checked = config.IntegratedSecurity

        UpdateStatus("Profile loaded successfully.")
    End Sub

    Private Sub SaveProfile(config As ProfileConfig)
        Try
            Dim jsonString As String = JsonSerializer.Serialize(config, New JsonSerializerOptions With {
                .WriteIndented = True
            })
            File.WriteAllText(profileFilePath, jsonString)
        Catch ex As Exception
            MessageBox.Show($"Error saving profile: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Function GetCurrentConfig() As ProfileConfig
        Return New ProfileConfig With {
            .Server = txtServer.Text.Trim(),
            .Username = txtUsername.Text.Trim(),
            .Password = txtPassword.Text.Trim(),
            .Database = txtDatabase.Text.Trim(),
            .OutputFilePath = txtOutput.Text.Trim(),
            .SelectedTables = selectedTables,
            .Tables = chkTables.Checked,
            .Views = chkViews.Checked,
            .StoredProcedures = chkStoredProcedures.Checked,
            .Functions = chkFunctions.Checked,
            .Indexes = chkIndexes.Checked,
            .Triggers = chkTriggers.Checked,
            .Permissions = chkPermissions.Checked,
            .IncludeData = chkIncludeData.Checked,
            .IntegratedSecurity = chkIntegratedSecurity.Checked
        }
    End Function

#End Region

#Region "UI Updates"

    Private Sub UpdateStatus(message As String)
        toolStripStatusLabel.Text = message
        Application.DoEvents()
    End Sub

    Private Sub UpdateConnectionStatus(connected As Boolean, message As String)
        isConnected = connected
        lblConnectionStatus.Text = message
        toolStripConnectionStatus.Text = message

        If connected Then
            lblConnectionStatus.ForeColor = Color.FromArgb(40, 167, 69) ' Success green
            picConnectionStatus.BackColor = Color.FromArgb(40, 167, 69)
            toolStripConnectionStatus.Text = "Connected"
        Else
            lblConnectionStatus.ForeColor = Color.FromArgb(220, 53, 69) ' Danger red
            picConnectionStatus.BackColor = Color.FromArgb(220, 53, 69)
            toolStripConnectionStatus.Text = "Disconnected"
        End If

        btnLoadTables.Enabled = connected
        btnGenerate.Enabled = connected AndAlso selectedTables.Count > 0
    End Sub

    Private Sub UpdateTableCount()
        Dim checkedCount As Integer = lstTables.CheckedItems.Count
        Dim totalCount As Integer = lstTables.Items.Count
        lblTableCount.Text = $"{checkedCount} of {totalCount} tables selected"

        btnGenerate.Enabled = isConnected AndAlso checkedCount > 0
    End Sub

#End Region

#Region "Connection Management"

    Private Sub chkIntegratedSecurity_CheckedChanged(sender As Object, e As EventArgs)
        Dim useIntegrated As Boolean = chkIntegratedSecurity.Checked
        txtUsername.Enabled = Not useIntegrated
        txtPassword.Enabled = Not useIntegrated
        lblUsername.Enabled = Not useIntegrated
        lblPassword.Enabled = Not useIntegrated

        If useIntegrated Then
            txtUsername.BackColor = Color.FromArgb(248, 249, 250)
            txtPassword.BackColor = Color.FromArgb(248, 249, 250)
        Else
            txtUsername.BackColor = Color.White
            txtPassword.BackColor = Color.White
        End If

        SaveProfile(GetCurrentConfig())
    End Sub

    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        TestDatabaseConnection()
    End Sub

    Private Async Sub TestDatabaseConnection()
        Dim serverName As String = txtServer.Text.Trim()
        Dim databaseName As String = txtDatabase.Text.Trim()

        If String.IsNullOrEmpty(serverName) OrElse String.IsNullOrEmpty(databaseName) Then
            MessageBox.Show("Please enter server and database information.", "Missing Information",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not chkIntegratedSecurity.Checked AndAlso
           (String.IsNullOrEmpty(txtUsername.Text.Trim()) OrElse String.IsNullOrEmpty(txtPassword.Text.Trim())) Then
            MessageBox.Show("Please enter username and password for SQL authentication.", "Missing Credentials",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        btnTestConnection.Enabled = False
        UpdateStatus("Testing connection...")
        UpdateConnectionStatus(False, "Connecting...")

        Try
            connectionString = BuildConnectionString()

            Using connection As New SqlConnection(connectionString)
                Await connection.OpenAsync()
                UpdateConnectionStatus(True, $"Connected to {databaseName}")
                UpdateStatus("Connection successful!")
                MessageBox.Show("Database connection successful!", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            UpdateConnectionStatus(False, "Connection failed")
            UpdateStatus("Connection failed.")
            MessageBox.Show($"Connection failed: {ex.Message}", "Connection Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnTestConnection.Enabled = True
        End Try
    End Sub

    Private Function BuildConnectionString() As String
        Dim builder As New SqlConnectionStringBuilder()
        builder.DataSource = txtServer.Text.Trim()
        builder.InitialCatalog = txtDatabase.Text.Trim()
        builder.ConnectTimeout = 30
        builder.TrustServerCertificate = True

        If chkIntegratedSecurity.Checked Then
            builder.IntegratedSecurity = True
        Else
            builder.UserID = txtUsername.Text.Trim()
            builder.Password = txtPassword.Text.Trim()
        End If

        Return builder.ConnectionString
    End Function

#End Region

#Region "Table Management"

    Private Sub btnLoadTables_Click(sender As Object, e As EventArgs) Handles btnLoadTables.Click
        LoadTables()
    End Sub

    Private Async Sub LoadTables()
        If Not isConnected Then
            MessageBox.Show("Please test the connection first.", "Not Connected",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        btnLoadTables.Enabled = False
        UpdateStatus("Loading tables...")

        Try
            connectionString = BuildConnectionString()

            Using connection As New SqlConnection(connectionString)
                Await connection.OpenAsync()

                ' Clear existing items
                lstTables.Items.Clear()
                allTables.Clear()

                ' Load tables
                Dim query As String = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME"
                Using command As New SqlCommand(query, connection)
                    Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                        While Await reader.ReadAsync()
                            Dim tableName As String = reader.GetString(0)
                            allTables.Add(tableName)
                            lstTables.Items.Add(tableName, selectedTables.Contains(tableName))
                        End While
                    End Using
                End Using
            End Using

            ' Remove any selected tables that no longer exist
            Dim existingTables As New HashSet(Of String)(allTables)
            selectedTables.RemoveAll(Function(tableName) Not existingTables.Contains(tableName))

            ' Update UI
            UpdateTableCount()
            FilterTables()

            ' Save updated profile
            SaveProfile(GetCurrentConfig())
            UpdateStatus($"Loaded {allTables.Count} tables successfully.")

        Catch ex As Exception
            UpdateStatus("Error loading tables.")
            MessageBox.Show($"Error loading tables: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnLoadTables.Enabled = True
        End Try
    End Sub

    Private Sub txtTableSearch_TextChanged(sender As Object, e As EventArgs)
        FilterTables()
    End Sub

    Private Sub FilterTables()
        Dim searchText As String = txtTableSearch.Text.Trim().ToLower()

        lstTables.Items.Clear()

        For Each tableName As String In allTables
            If String.IsNullOrEmpty(searchText) OrElse tableName.ToLower().Contains(searchText) Then
                lstTables.Items.Add(tableName, selectedTables.Contains(tableName))
            End If
        Next

        UpdateTableCount()
    End Sub

    Private Sub lstTables_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles lstTables.ItemCheck
        ' Use BeginInvoke to ensure the check state is updated before processing
        Me.BeginInvoke(Sub()
                           Dim tableName As String = lstTables.Items(e.Index).ToString()
                           If lstTables.GetItemChecked(e.Index) Then
                               If Not selectedTables.Contains(tableName) Then
                                   selectedTables.Add(tableName)
                               End If
                           Else
                               selectedTables.Remove(tableName)
                           End If
                           UpdateTableCount()
                           SaveProfile(GetCurrentConfig())
                       End Sub)
    End Sub

    Private Sub btnSelectAll_Click(sender As Object, e As EventArgs) Handles btnSelectAll.Click
        For i As Integer = 0 To lstTables.Items.Count - 1
            lstTables.SetItemChecked(i, True)
            Dim tableName As String = lstTables.Items(i).ToString()
            If Not selectedTables.Contains(tableName) Then
                selectedTables.Add(tableName)
            End If
        Next
        UpdateTableCount()
        SaveProfile(GetCurrentConfig())
    End Sub

    Private Sub btnSelectNone_Click(sender As Object, e As EventArgs) Handles btnSelectNone.Click
        For i As Integer = 0 To lstTables.Items.Count - 1
            lstTables.SetItemChecked(i, False)
        Next
        selectedTables.Clear()
        UpdateTableCount()
        SaveProfile(GetCurrentConfig())
    End Sub

    Private Sub btnInvertSelection_Click(sender As Object, e As EventArgs) Handles btnInvertSelection.Click
        For i As Integer = 0 To lstTables.Items.Count - 1
            Dim isChecked As Boolean = lstTables.GetItemChecked(i)
            lstTables.SetItemChecked(i, Not isChecked)

            Dim tableName As String = lstTables.Items(i).ToString()
            If Not isChecked Then
                If Not selectedTables.Contains(tableName) Then
                    selectedTables.Add(tableName)
                End If
            Else
                selectedTables.Remove(tableName)
            End If
        Next
        UpdateTableCount()
        SaveProfile(GetCurrentConfig())
    End Sub

#End Region

#Region "Option Events"

    Private Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles chkTables.CheckedChanged,
        chkViews.CheckedChanged, chkStoredProcedures.CheckedChanged, chkFunctions.CheckedChanged,
        chkIndexes.CheckedChanged, chkTriggers.CheckedChanged, chkPermissions.CheckedChanged, chkIncludeData.CheckedChanged
        SaveProfile(GetCurrentConfig())
    End Sub

#End Region

#Region "Output Management"

    Private Sub btnSelectOutput_Click(sender As Object, e As EventArgs) Handles btnSelectOutput.Click
        Using folderDialog As New FolderBrowserDialog()
            folderDialog.Description = "Select output folder for SQL scripts"
            folderDialog.SelectedPath = If(Directory.Exists(txtOutput.Text), txtOutput.Text, Environment.GetFolderPath(Environment.SpecialFolder.Desktop))

            If folderDialog.ShowDialog() = DialogResult.OK Then
                txtOutput.Text = folderDialog.SelectedPath
                btnOpenOutput.Enabled = True
                SaveProfile(GetCurrentConfig())
            End If
        End Using
    End Sub

    Private Sub btnOpenOutput_Click(sender As Object, e As EventArgs) Handles btnOpenOutput.Click
        If Directory.Exists(txtOutput.Text) Then
            Try
                Process.Start("explorer.exe", txtOutput.Text)
            Catch ex As Exception
                MessageBox.Show($"Error opening folder: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Output folder does not exist.", "Folder Not Found",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

#End Region

#Region "Script Generation"

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        GenerateSqlScripts()
    End Sub

    Private Async Sub GenerateSqlScripts()
        Dim config As ProfileConfig = GetCurrentConfig()

        If Not isConnected Then
            MessageBox.Show("Please test the connection first.", "Not Connected",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If selectedTables.Count = 0 Then
            MessageBox.Show("Please select at least one table.", "No Tables Selected",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If String.IsNullOrEmpty(config.OutputFilePath) Then
            MessageBox.Show("Please select an output folder.", "No Output Folder",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Ensure output directory exists
        Try
            If Not Directory.Exists(config.OutputFilePath) Then
                Directory.CreateDirectory(config.OutputFilePath)
            End If
        Catch ex As Exception
            MessageBox.Show($"Error creating output directory: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        ' Disable controls during generation
        SetControlsEnabled(False)
        pnlProgress.Visible = True
        toolStripProgressBar.Visible = True
        progressBar.Value = 0
        toolStripProgressBar.Value = 0

        Try
            UpdateStatus("Generating SQL scripts...")
            lblProgress.Text = "Initializing script generation..."

            connectionString = BuildConnectionString()

            Dim outputFiles As List(Of String) = Await GenerateScripts(config)

            progressBar.Value = progressBar.Maximum
            toolStripProgressBar.Value = toolStripProgressBar.Maximum

            Dim message As String = "SQL scripts generated successfully!" & vbCrLf & vbCrLf &
                                  "Generated files:" & vbCrLf &
                                  String.Join(vbCrLf, outputFiles.Select(Function(f) "• " & Path.GetFileName(f)))

            Dim result As DialogResult = MessageBox.Show(message & vbCrLf & vbCrLf & "Would you like to open the output folder?",
                                                       "Generation Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

            If result = DialogResult.Yes Then
                btnOpenOutput_Click(Nothing, Nothing)
            End If

            UpdateStatus("Generation completed successfully.")
            SaveProfile(config)

        Catch ex As Exception
            UpdateStatus("Generation failed.")
            MessageBox.Show($"Error generating scripts: {ex.Message}", "Generation Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetControlsEnabled(True)
            pnlProgress.Visible = False
            toolStripProgressBar.Visible = False
        End Try
    End Sub

    Private Sub SetControlsEnabled(enabled As Boolean)
        btnGenerate.Enabled = enabled AndAlso isConnected AndAlso selectedTables.Count > 0
        btnLoadTables.Enabled = enabled AndAlso isConnected
        btnSelectOutput.Enabled = enabled
        btnTestConnection.Enabled = enabled
        btnSaveProfile.Enabled = enabled
        grpConnection.Enabled = enabled
        grpOptions.Enabled = enabled
        pnlTableActions.Enabled = enabled
        lstTables.Enabled = enabled
    End Sub

    Private Async Function GenerateScripts(config As ProfileConfig) As Task(Of List(Of String))
        Dim outputFiles As New List(Of String)
        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")

        Using connection As New SqlConnection(connectionString)
            Await connection.OpenAsync()

            ' Calculate total operations for progress
            Dim totalOps As Integer = 0
            If config.Tables Then totalOps += 1
            If config.IncludeData AndAlso selectedTables.Count > 0 Then totalOps += 1
            If config.Views Then totalOps += 1
            If config.StoredProcedures Then totalOps += 1
            If config.Functions Then totalOps += 1
            If config.Indexes Then totalOps += 1
            If config.Triggers Then totalOps += 1
            If config.Permissions Then totalOps += 1

            progressBar.Maximum = totalOps
            toolStripProgressBar.Maximum = totalOps
            Dim currentOp As Integer = 0

            ' Generate database header
            Dim dbHeader As String = GenerateDatabaseHeader(config.Database)

            ' Generate Table Creation Script
            If config.Tables Then
                lblProgress.Text = "Generating table creation scripts..."
                Dim tableScript As String = Await GenerateTableCreationScript(connection, config, dbHeader)
                If Not String.IsNullOrEmpty(tableScript) Then
                    Dim tablePath As String = Path.Combine(config.OutputFilePath, $"01_TableCreate_{timestamp}.sql")
                    Await File.WriteAllTextAsync(tablePath, tableScript)
                    outputFiles.Add(tablePath)
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            ' Generate Table Insert Script
            If config.IncludeData AndAlso selectedTables.Count > 0 Then
                lblProgress.Text = "Generating table insert scripts..."
                Dim insertScript As String = Await GenerateTableInsertScript(connection, config, dbHeader)
                If Not String.IsNullOrEmpty(insertScript) Then
                    Dim insertPath As String = Path.Combine(config.OutputFilePath, $"02_TableInsert_{timestamp}.sql")
                    Await File.WriteAllTextAsync(insertPath, insertScript)
                    outputFiles.Add(insertPath)
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            ' Generate Views Script
            If config.Views Then
                lblProgress.Text = "Generating views scripts..."
                Dim viewScript As String = Await GenerateViewsScript(connection, config, dbHeader)
                If Not String.IsNullOrEmpty(viewScript) Then
                    Dim viewPath As String = Path.Combine(config.OutputFilePath, $"03_Views_{timestamp}.sql")
                    Await File.WriteAllTextAsync(viewPath, viewScript)
                    outputFiles.Add(viewPath)
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            ' Generate Stored Procedures Script
            If config.StoredProcedures Then
                lblProgress.Text = "Generating stored procedures scripts..."
                Dim procScript As String = Await GenerateStoredProceduresScript(connection, config, dbHeader)
                If Not String.IsNullOrEmpty(procScript) Then
                    Dim procPath As String = Path.Combine(config.OutputFilePath, $"04_StoredProcedures_{timestamp}.sql")
                    Await File.WriteAllTextAsync(procPath, procScript)
                    outputFiles.Add(procPath)
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            ' Generate Functions Script
            If config.Functions Then
                lblProgress.Text = "Generating functions scripts..."
                Dim funcScript As String = Await GenerateFunctionsScript(connection, config, dbHeader)
                If Not String.IsNullOrEmpty(funcScript) Then
                    Dim funcPath As String = Path.Combine(config.OutputFilePath, $"05_Functions_{timestamp}.sql")
                    Await File.WriteAllTextAsync(funcPath, funcScript)
                    outputFiles.Add(funcPath)
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            ' Generate additional scripts if selected
            If config.Indexes Then
                lblProgress.Text = "Generating indexes scripts..."
                ' TODO: Implement index generation
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If config.Triggers Then
                lblProgress.Text = "Generating triggers scripts..."
                ' TODO: Implement trigger generation
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If config.Permissions Then
                lblProgress.Text = "Generating permissions scripts..."
                ' TODO: Implement permissions generation
                currentOp += 1
                UpdateProgress(currentOp)
            End If
        End Using

        Return outputFiles
    End Function

    Private Sub UpdateProgress(value As Integer)
        progressBar.Value = Math.Min(value, progressBar.Maximum)
        toolStripProgressBar.Value = Math.Min(value, toolStripProgressBar.Maximum)
        Application.DoEvents()
    End Sub

    Private Function GenerateDatabaseHeader(databaseName As String) As String
        Return $"-- Generated by RepoSQL v2.0 on {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}" &
               $"-- Database: {databaseName}{Environment.NewLine}" &
               $"-- Generated by: {Environment.UserName}{Environment.NewLine}{Environment.NewLine}" &
               $"USE [{databaseName}];{Environment.NewLine}" &
               $"GO{Environment.NewLine}{Environment.NewLine}"
    End Function

    Private Async Function GenerateTableCreationScript(connection As SqlConnection, config As ProfileConfig, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- TABLE CREATION SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get detailed table information including columns, data types, etc.
        For Each tableName As String In selectedTables
            script.AppendLine($"-- Table: {tableName}")
            script.AppendLine($"IF OBJECT_ID(N'[dbo].[{tableName}]', N'U') IS NOT NULL")
            script.AppendLine($"    DROP TABLE [dbo].[{tableName}];")
            script.AppendLine("GO")
            script.AppendLine()

            ' Get table structure
            Dim tableStructure As String = Await GetTableCreateScript(connection, tableName)
            script.AppendLine(tableStructure)
            script.AppendLine("GO")
            script.AppendLine()
        Next

        Return script.ToString()
    End Function

    Private Async Function GetTableCreateScript(connection As SqlConnection, tableName As String) As Task(Of String)
        Dim script As New StringBuilder()
        script.AppendLine($"CREATE TABLE [dbo].[{tableName}] (")

        ' Get column information
        Dim columnQuery As String = $"
            SELECT 
                c.COLUMN_NAME,
                c.DATA_TYPE,
                c.CHARACTER_MAXIMUM_LENGTH,
                c.NUMERIC_PRECISION,
                c.NUMERIC_SCALE,
                c.IS_NULLABLE,
                c.COLUMN_DEFAULT,
                COLUMNPROPERTY(OBJECT_ID(c.TABLE_SCHEMA + '.' + c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') AS IsIdentity
            FROM INFORMATION_SCHEMA.COLUMNS c
            WHERE c.TABLE_NAME = '{tableName}'
            ORDER BY c.ORDINAL_POSITION"

        Dim columns As New List(Of String)
        Using command As New SqlCommand(columnQuery, connection)
            Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim columnDef As String = $"    [{reader("COLUMN_NAME")}] {FormatDataType(reader)}"

                    If Not reader.IsDBNull("IsIdentity") AndAlso CBool(reader("IsIdentity")) Then
                        columnDef &= " IDENTITY(1,1)"
                    End If

                    columnDef &= If(reader("IS_NULLABLE").ToString() = "YES", " NULL", " NOT NULL")

                    If Not reader.IsDBNull("COLUMN_DEFAULT") Then
                        columnDef &= $" DEFAULT {reader("COLUMN_DEFAULT")}"
                    End If

                    columns.Add(columnDef)
                End While
            End Using
        End Using

        script.AppendLine(String.Join("," & Environment.NewLine, columns))
        script.AppendLine(")")

        Return script.ToString()
    End Function

    Private Function FormatDataType(reader As SqlDataReader) As String
        Dim dataType As String = reader("DATA_TYPE").ToString().ToUpper()

        Select Case dataType
            Case "VARCHAR", "NVARCHAR", "CHAR", "NCHAR"
                Dim length As Object = reader("CHARACTER_MAXIMUM_LENGTH")
                If length Is DBNull.Value OrElse CInt(length) = -1 Then
                    Return $"{dataType}(MAX)"
                Else
                    Return $"{dataType}({length})"
                End If
            Case "DECIMAL", "NUMERIC"
                Return $"{dataType}({reader("NUMERIC_PRECISION")},{reader("NUMERIC_SCALE")})"
            Case Else
                Return dataType
        End Select
    End Function

    Private Async Function GenerateTableInsertScript(connection As SqlConnection, config As ProfileConfig, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- TABLE INSERT SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        For Each tableName As String In selectedTables
            script.AppendLine($"-- Insert data for table [{tableName}]")

            Dim query As String = $"SELECT * FROM [{tableName}]"
            Using command As New SqlCommand(query, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim rowCount As Integer = 0
                    While Await reader.ReadAsync() AndAlso rowCount < 1000 ' Limit to prevent huge scripts
                        Dim columns As New List(Of String)
                        Dim values As New List(Of String)

                        For i As Integer = 0 To reader.FieldCount - 1
                            If Not reader.IsDBNull(i) Then
                                columns.Add($"[{reader.GetName(i)}]")
                                values.Add(FormatSqlValue(reader.GetValue(i)))
                            End If
                        Next

                        If columns.Count > 0 Then
                            script.AppendLine($"INSERT INTO [{tableName}] ({String.Join(", ", columns)}) VALUES ({String.Join(", ", values)});")
                        End If
                        rowCount += 1
                    End While
                End Using
            End Using
            script.AppendLine("GO")
            script.AppendLine()
        Next

        Return script.ToString()
    End Function

    Private Async Function GenerateViewsScript(connection As SqlConnection, config As ProfileConfig, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- VIEWS SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        Dim query As String = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS ORDER BY TABLE_NAME"
        Using command As New SqlCommand(query, connection)
            Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim viewName As String = reader.GetString(0)
                    script.AppendLine($"-- View: {viewName}")
                    script.AppendLine($"-- TODO: Add view definition for {viewName}")
                    script.AppendLine("GO")
                    script.AppendLine()
                End While
            End Using
        End Using

        Return script.ToString()
    End Function

    Private Async Function GenerateStoredProceduresScript(connection As SqlConnection, config As ProfileConfig, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- STORED PROCEDURES SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        Dim query As String = "SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'PROCEDURE' ORDER BY ROUTINE_NAME"
        Using command As New SqlCommand(query, connection)
            Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim procName As String = reader.GetString(0)
                    script.AppendLine($"-- Stored Procedure: {procName}")
                    script.AppendLine($"-- TODO: Add procedure definition for {procName}")
                    script.AppendLine("GO")
                    script.AppendLine()
                End While
            End Using
        End Using

        Return script.ToString()
    End Function

    Private Async Function GenerateFunctionsScript(connection As SqlConnection, config As ProfileConfig, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- FUNCTIONS SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        Dim query As String = "SELECT ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_TYPE = 'FUNCTION' ORDER BY ROUTINE_NAME"
        Using command As New SqlCommand(query, connection)
            Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim funcName As String = reader.GetString(0)
                    script.AppendLine($"-- Function: {funcName}")
                    script.AppendLine($"-- TODO: Add function definition for {funcName}")
                    script.AppendLine("GO")
                    script.AppendLine()
                End While
            End Using
        End Using

        Return script.ToString()
    End Function

    Private Function FormatSqlValue(value As Object) As String
        If value Is DBNull.Value OrElse value Is Nothing Then Return "NULL"

        Select Case value.GetType().Name
            Case "String"
                Return "'" & value.ToString().Replace("'", "''") & "'"
            Case "DateTime"
                Return "'" & Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss") & "'"
            Case "Boolean"
                Return If(CBool(value), "1", "0")
            Case "Byte", "SByte", "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64", "Decimal", "Double", "Single"
                Return value.ToString()
            Case Else
                Return "'" & value.ToString().Replace("'", "''") & "'"
        End Select
    End Function

#End Region

#Region "Menu Event Handlers"

    Private Sub newProfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles newProfileToolStripMenuItem.Click
        If MessageBox.Show("Create a new profile? This will clear current settings.", "New Profile",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            LoadProfile() ' This will load defaults
        End If
    End Sub

    Private Sub loadProfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles loadProfileToolStripMenuItem.Click
        Using openDialog As New OpenFileDialog()
            openDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            If openDialog.ShowDialog() = DialogResult.OK Then
                Try
                    File.Copy(openDialog.FileName, profileFilePath, True)
                    LoadProfile()
                    MessageBox.Show("Profile loaded successfully!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"Error loading profile: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub saveProfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles saveProfileToolStripMenuItem.Click
        btnSaveProfile_Click(sender, e)
    End Sub

    Private Sub btnSaveProfile_Click(sender As Object, e As EventArgs) Handles btnSaveProfile.Click
        Using saveDialog As New SaveFileDialog()
            saveDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            saveDialog.DefaultExt = "json"
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            saveDialog.FileName = $"RepoSQL_Profile_{DateTime.Now:yyyyMMdd_HHmmss}.json"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Try
                    File.Copy(profileFilePath, saveDialog.FileName, True)
                    MessageBox.Show($"Profile saved successfully to: {saveDialog.FileName}", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"Error saving profile: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub settingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles settingsToolStripMenuItem.Click
        MessageBox.Show("Settings dialog coming soon!", "Settings",
                       MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub exitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles exitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub aboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles aboutToolStripMenuItem.Click
        Dim aboutText As String = "RepoSQL - Professional SQL Script Generator" & vbCrLf & vbCrLf &
                                 "Version: 2.0" & vbCrLf &
                                 "Framework: .NET 8.0" & vbCrLf &
                                 "Database: SQL Server 2012+" & vbCrLf & vbCrLf &
                                 "Features:" & vbCrLf &
                                 "• Modern, professional UI/UX" & vbCrLf &
                                 "• Async database operations" & vbCrLf &
                                 "• JSON-based configuration" & vbCrLf &
                                 "• Comprehensive script generation" & vbCrLf &
                                 "• Table search and filtering" & vbCrLf &
                                 "• Windows and SQL authentication" & vbCrLf & vbCrLf &
                                 "Generate professional SQL scripts from your databases with ease!"

        MessageBox.Show(aboutText, "About RepoSQL", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

End Class