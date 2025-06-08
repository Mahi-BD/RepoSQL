Imports Microsoft.Data.SqlClient
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Windows.Forms
Imports System.Threading.Tasks
Imports System.Diagnostics
Imports System.Security.Principal

Public Class frmMain
    ' INI file configuration variables
    Private ReadOnly iniFilePath As String = Path.Combine(Application.StartupPath, "sconfig.ini")
    Private iniHelper As IniHelper
    Private isLoadingConfig As Boolean = False
    Private saveTimer As Timer

    ' Existing variables
    Private selectedTables As New List(Of String)
    Private connectionString As String = String.Empty
    Private allTables As New List(Of String)
    Private isConnected As Boolean = False

#Region "Form Events"

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "RepoSQL - SQL Script Generator v2.0"
        InitializeFormWithIniConfigUpdated()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveIniConfig()
    End Sub

    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        ' Handle form resize if needed
        If Me.WindowState <> FormWindowState.Minimized Then
            ' Adjust layout if necessary
        End If
    End Sub

#End Region

#Region "Initialization"

    Private Sub InitializeFormWithIniConfigUpdated()
        ' Initialize INI helper
        iniHelper = New IniHelper(iniFilePath)

        ' Set initial states
        pnlProgress.Visible = False
        toolStripProgressBar.Visible = False
        btnGenerate.Enabled = False
        btnLoadTables.Enabled = False
        btnBackupDatabase.Enabled = False

        ' Initialize connection status
        UpdateConnectionStatus(False, "Not connected")

        ' Set up table search
        AddHandler txtTableSearch.TextChanged, AddressOf txtTableSearch_TextChanged

        ' Set up integrated security checkbox
        AddHandler chkIntegratedSecurity.CheckedChanged, AddressOf chkIntegratedSecurity_CheckedChanged

        ' Initialize auto-save event handlers
        InitializeAutoSaveEventHandlers()

        ' Load configuration from INI file
        LoadIniConfig()

        ' Load generation settings
        LoadGenerationSettings()

        ' Initial UI state after loading config
        chkIntegratedSecurity_CheckedChanged(Nothing, Nothing)

        ' Set default output path if still empty after loading
        If String.IsNullOrEmpty(txtOutput.Text) Then
            txtOutput.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RepoSQL_Output")
        End If
    End Sub

#End Region

#Region "Settings Integration"

    ''' <summary>
    ''' Load generation settings from INI file
    ''' </summary>
    Private Sub LoadGenerationSettings()
        Try
            ' Load generation settings that affect script creation
            Dim generateDropStatements As Boolean = iniHelper.ReadBoolean("GenerationSettings", "GenerateDropStatements", False)
            Dim overwriteExistingFiles As Boolean = iniHelper.ReadBoolean("GenerationSettings", "OverwriteExistingFiles", True)

            ' Store settings for use during generation
            Me.Tag = New Dictionary(Of String, Object) From {
                {"GenerateDropStatements", generateDropStatements},
                {"OverwriteExistingFiles", overwriteExistingFiles}
            }

        Catch ex As Exception
            ' Use default settings if error loading
            Me.Tag = New Dictionary(Of String, Object) From {
                {"GenerateDropStatements", False},
                {"OverwriteExistingFiles", True}
            }
        End Try
    End Sub

    ''' <summary>
    ''' Get current generation settings
    ''' </summary>
    ''' <returns>Dictionary with current settings</returns>
    Private Function GetGenerationSettings() As Dictionary(Of String, Object)
        If Me.Tag Is Nothing Then
            LoadGenerationSettings()
        End If
        Return DirectCast(Me.Tag, Dictionary(Of String, Object))
    End Function

#End Region

#Region "INI Configuration Management"

    ''' <summary>
    ''' Loads configuration from sconfig.ini file
    ''' </summary>
    Private Sub LoadIniConfig()
        If Not iniHelper.FileExists() Then
            ' Create default INI file if it doesn't exist
            SaveIniConfig()
            Return
        End If

        Try
            isLoadingConfig = True ' Prevent auto-save during loading

            ' Load Server Configuration
            txtServer.Text = iniHelper.ReadValue("ServerConfig", "Server", "LSERVER")
            txtDatabase.Text = iniHelper.ReadValue("ServerConfig", "Database", "BinBookLiteDB")
            txtUsername.Text = iniHelper.ReadValue("ServerConfig", "Username", "as")
            txtPassword.Text = iniHelper.ReadValue("ServerConfig", "Password", "fobian")
            chkIntegratedSecurity.Checked = iniHelper.ReadBoolean("ServerConfig", "IntegratedSecurity", False)

            ' Load Output Configuration
            txtOutput.Text = iniHelper.ReadValue("OutputConfig", "OutputPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RepoSQL_Output"))

            ' Load Script Options
            chkTables.Checked = iniHelper.ReadBoolean("ScriptOptions", "Tables", True)
            chkViews.Checked = iniHelper.ReadBoolean("ScriptOptions", "Views", False)
            chkStoredProcedures.Checked = iniHelper.ReadBoolean("ScriptOptions", "StoredProcedures", False)
            chkFunctions.Checked = iniHelper.ReadBoolean("ScriptOptions", "Functions", False)
            chkIndexes.Checked = iniHelper.ReadBoolean("ScriptOptions", "Indexes", False)
            chkTriggers.Checked = iniHelper.ReadBoolean("ScriptOptions", "Triggers", False)
            chkPermissions.Checked = iniHelper.ReadBoolean("ScriptOptions", "Permissions", False)
            chkIncludeData.Checked = iniHelper.ReadBoolean("ScriptOptions", "IncludeData", False)

            ' Load Selected Tables
            LoadSelectedTablesFromIni()

            UpdateStatus("Configuration loaded from sconfig.ini")

        Catch ex As Exception
            MessageBox.Show($"Error loading configuration: {ex.Message}", "Configuration Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            isLoadingConfig = False
        End Try
    End Sub

    ''' <summary>
    ''' Saves current configuration to sconfig.ini file
    ''' </summary>
    Private Sub SaveIniConfig()
        If isLoadingConfig Then Return ' Don't save while loading

        Try
            ' Save Server Configuration
            iniHelper.WriteValue("ServerConfig", "Server", txtServer.Text.Trim())
            iniHelper.WriteValue("ServerConfig", "Database", txtDatabase.Text.Trim())
            iniHelper.WriteValue("ServerConfig", "Username", txtUsername.Text.Trim())
            iniHelper.WriteValue("ServerConfig", "Password", txtPassword.Text.Trim())
            iniHelper.WriteBoolean("ServerConfig", "IntegratedSecurity", chkIntegratedSecurity.Checked)

            ' Save Output Configuration
            iniHelper.WriteValue("OutputConfig", "OutputPath", txtOutput.Text.Trim())

            ' Save Script Options
            iniHelper.WriteBoolean("ScriptOptions", "Tables", chkTables.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "Views", chkViews.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "StoredProcedures", chkStoredProcedures.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "Functions", chkFunctions.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "Indexes", chkIndexes.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "Triggers", chkTriggers.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "Permissions", chkPermissions.Checked)
            iniHelper.WriteBoolean("ScriptOptions", "IncludeData", chkIncludeData.Checked)

            ' Save Selected Tables
            SaveSelectedTablesToIni()

            ' Write timestamp and version
            iniHelper.WriteValue("General", "LastSaved", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            iniHelper.WriteValue("General", "Version", "2.0")

        Catch ex As Exception
            MessageBox.Show($"Error saving configuration: {ex.Message}", "Configuration Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    ''' <summary>
    ''' Loads selected tables from INI file
    ''' </summary>
    Private Sub LoadSelectedTablesFromIni()
        selectedTables.Clear()

        Dim tableCount As Integer = iniHelper.ReadInteger("SelectedTables", "Count", 0)
        selectedTables = iniHelper.ReadList("SelectedTables", "Table", tableCount)

        ' Update UI if tables are already loaded
        If lstTables.Items.Count > 0 Then
            For i As Integer = 0 To lstTables.Items.Count - 1
                Dim tableName As String = lstTables.Items(i).ToString()
                lstTables.SetItemChecked(i, selectedTables.Contains(tableName))
            Next
            UpdateTableCount()
        End If
    End Sub

    ''' <summary>
    ''' Saves selected tables to INI file
    ''' </summary>
    Private Sub SaveSelectedTablesToIni()
        iniHelper.WriteList("SelectedTables", "Table", selectedTables)
    End Sub

#End Region

#Region "Auto-Save Event Handlers"

    ''' <summary>
    ''' Initialize auto-save event handlers
    ''' </summary>
    Private Sub InitializeAutoSaveEventHandlers()
        ' Server configuration change events
        AddHandler txtServer.TextChanged, AddressOf OnConfigurationChanged
        AddHandler txtDatabase.TextChanged, AddressOf OnConfigurationChanged
        AddHandler txtUsername.TextChanged, AddressOf OnConfigurationChanged
        AddHandler txtPassword.TextChanged, AddressOf OnConfigurationChanged
        AddHandler chkIntegratedSecurity.CheckedChanged, AddressOf OnConfigurationChanged

        ' Output configuration change events
        AddHandler txtOutput.TextChanged, AddressOf OnConfigurationChanged

        ' Script options change events
        AddHandler chkTables.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkViews.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkStoredProcedures.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkFunctions.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkIndexes.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkTriggers.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkPermissions.CheckedChanged, AddressOf OnConfigurationChanged
        AddHandler chkIncludeData.CheckedChanged, AddressOf OnConfigurationChanged
    End Sub

    ''' <summary>
    ''' Called when any configuration parameter changes
    ''' </summary>
    Private Sub OnConfigurationChanged(sender As Object, e As EventArgs)
        If Not isLoadingConfig Then
            ' Add a small delay to prevent too frequent saves during typing
            SaveConfigurationDelayed()
        End If
    End Sub

    ''' <summary>
    ''' Saves configuration with a small delay to prevent excessive saves during typing
    ''' </summary>
    Private Sub SaveConfigurationDelayed()
        If saveTimer IsNot Nothing Then
            saveTimer.Stop()
            saveTimer.Dispose()
        End If

        saveTimer = New Timer()
        saveTimer.Interval = 1000 ' 1 second delay
        AddHandler saveTimer.Tick, Sub()
                                       saveTimer.Stop()
                                       saveTimer.Dispose()
                                       saveTimer = Nothing
                                       SaveIniConfig()
                                   End Sub
        saveTimer.Start()
    End Sub

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
        btnBackupDatabase.Enabled = connected
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

            ' Save updated configuration
            SaveIniConfig()
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

                           ' Auto-save configuration
                           If Not isLoadingConfig Then
                               SaveIniConfig()
                           End If
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
        SaveIniConfig() ' Auto-save
    End Sub

    Private Sub btnSelectNone_Click(sender As Object, e As EventArgs) Handles btnSelectNone.Click
        For i As Integer = 0 To lstTables.Items.Count - 1
            lstTables.SetItemChecked(i, False)
        Next
        selectedTables.Clear()
        UpdateTableCount()
        SaveIniConfig() ' Auto-save
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
        SaveIniConfig() ' Auto-save
    End Sub

#End Region

#Region "Option Events"

    Private Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles chkTables.CheckedChanged,
        chkViews.CheckedChanged, chkStoredProcedures.CheckedChanged, chkFunctions.CheckedChanged,
        chkIndexes.CheckedChanged, chkTriggers.CheckedChanged, chkPermissions.CheckedChanged, chkIncludeData.CheckedChanged
        ' Auto-save is now handled by OnConfigurationChanged
    End Sub

#End Region

#Region "Output Management"

    Private Sub btnSelectOutput_Click(sender As Object, e As EventArgs) Handles btnSelectOutput.Click
        Using folderDialog As New FolderBrowserDialog()
            folderDialog.Description = "Select output folder for SQL scripts"
            folderDialog.SelectedPath = If(Directory.Exists(txtOutput.Text), txtOutput.Text, Environment.GetFolderPath(Environment.SpecialFolder.Desktop))

            If folderDialog.ShowDialog() = DialogResult.OK Then
                txtOutput.Text = folderDialog.SelectedPath
                btnBackupDatabase.Enabled = isConnected
                SaveIniConfig()
            End If
        End Using
    End Sub

#End Region

#Region "Enhanced Database Backup Integration"

    ''' <summary>
    ''' Event handler for the backup database button with enhanced error handling
    ''' </summary>
    Private Sub btnBackupDatabase_Click(sender As Object, e As EventArgs) Handles btnBackupDatabase.Click
        ' Show backup method selection dialog
        ShowBackupMethodDialog()
    End Sub

    ''' <summary>
    ''' Show dialog to select backup method
    ''' </summary>
    Private Sub ShowBackupMethodDialog()
        Dim methodDialog As New Form()
        methodDialog.Text = "Select Backup Method"
        methodDialog.Size = New Size(500, 300)
        methodDialog.StartPosition = FormStartPosition.CenterParent
        methodDialog.FormBorderStyle = FormBorderStyle.FixedDialog
        methodDialog.MaximizeBox = False
        methodDialog.MinimizeBox = False
        methodDialog.ShowIcon = False

        Dim lblTitle As New Label()
        lblTitle.Text = "Choose Backup Method"
        lblTitle.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblTitle.Location = New Point(20, 20)
        lblTitle.Size = New Size(460, 25)

        Dim lblDesc As New Label()
        lblDesc.Text = "Select the backup method that works best for your system configuration:"
        lblDesc.Location = New Point(20, 50)
        lblDesc.Size = New Size(460, 20)

        Dim btnStandard As New Button()
        btnStandard.Text = "🚀 Standard Method (Recommended)"
        btnStandard.Location = New Point(20, 90)
        btnStandard.Size = New Size(460, 35)
        btnStandard.BackColor = Color.FromArgb(40, 167, 69)
        btnStandard.ForeColor = Color.White
        btnStandard.FlatStyle = FlatStyle.Flat
        AddHandler btnStandard.Click, Sub()
                                          methodDialog.DialogResult = DialogResult.OK
                                          methodDialog.Tag = "standard"
                                          methodDialog.Close()
                                      End Sub

        Dim lblStandardDesc As New Label()
        lblStandardDesc.Text = "Uses enhanced backup manager with automatic directory detection and permission handling."
        lblStandardDesc.Location = New Point(40, 130)
        lblStandardDesc.Size = New Size(440, 20)
        lblStandardDesc.Font = New Font("Segoe UI", 8, FontStyle.Italic)
        lblStandardDesc.ForeColor = Color.Gray

        Dim btnAlternative As New Button()
        btnAlternative.Text = "⚡ Alternative Method (SQLCMD)"
        btnAlternative.Location = New Point(20, 160)
        btnAlternative.Size = New Size(460, 35)
        btnAlternative.BackColor = Color.FromArgb(0, 120, 215)
        btnAlternative.ForeColor = Color.White
        btnAlternative.FlatStyle = FlatStyle.Flat
        AddHandler btnAlternative.Click, Sub()
                                             methodDialog.DialogResult = DialogResult.OK
                                             methodDialog.Tag = "alternative"
                                             methodDialog.Close()
                                         End Sub

        Dim lblAltDesc As New Label()
        lblAltDesc.Text = "Uses SQLCMD for backup creation. Try this if the standard method fails with permission errors."
        lblAltDesc.Location = New Point(40, 200)
        lblAltDesc.Size = New Size(440, 20)
        lblAltDesc.Font = New Font("Segoe UI", 8, FontStyle.Italic)
        lblAltDesc.ForeColor = Color.Gray

        Dim btnCancel As New Button()
        btnCancel.Text = "Cancel"
        btnCancel.Location = New Point(405, 230)
        btnCancel.Size = New Size(75, 25)
        btnCancel.DialogResult = DialogResult.Cancel
        AddHandler btnCancel.Click, Sub() methodDialog.Close()

        methodDialog.Controls.AddRange({lblTitle, lblDesc, btnStandard, lblStandardDesc, btnAlternative, lblAltDesc, btnCancel})

        If methodDialog.ShowDialog(Me) = DialogResult.OK Then
            Dim method As String = If(methodDialog.Tag?.ToString(), "standard")
            If method = "alternative" Then
                BackupDatabaseUsingSqlCmd()
            Else
                BackupDatabaseAndZipEnhanced()
            End If
        End If

        methodDialog.Dispose()
    End Sub

    ''' <summary>
    ''' Create database backup using the enhanced DatabaseBackupManager class
    ''' </summary>
    Private Async Sub BackupDatabaseAndZipEnhanced()
        ' Validate connection
        If Not isConnected Then
            MessageBox.Show("Please test the database connection first.", "Not Connected",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validate output directory
        If String.IsNullOrEmpty(txtOutput.Text) OrElse Not Directory.Exists(txtOutput.Text) Then
            MessageBox.Show("Please select a valid output directory first.", "Invalid Output Directory",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Check if we're running with sufficient privileges
        Dim privilegeWarning As String = CheckBackupPrivileges()
        If Not String.IsNullOrEmpty(privilegeWarning) Then
            Dim result As DialogResult = MessageBox.Show(
                privilegeWarning & vbCrLf & vbCrLf & "Do you want to continue anyway?",
                "Privilege Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning)

            If result <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        ' Get database size estimate for user confirmation
        Try
            Dim dbSize As Double = Await GetDatabaseSizeAsync()
            Dim sizeMessage As String = If(dbSize > 0, $" (Current size: ~{dbSize:F1} MB)", "")

            ' Enhanced confirmation dialog with more information
            Dim confirmationMessage As String =
                $"Database Backup Confirmation" & vbCrLf & vbCrLf &
                $"Database: {txtDatabase.Text}{sizeMessage}" & vbCrLf &
                $"Output Location: {txtOutput.Text}" & vbCrLf & vbCrLf &
                "The backup process will:" & vbCrLf &
                "• Create a full database backup" & vbCrLf &
                "• Compress it to a ZIP file with timestamp" & vbCrLf &
                "• Use SQL Server's optimal backup location" & vbCrLf &
                "• Clean up temporary files automatically" & vbCrLf & vbCrLf &
                "This operation may take several minutes depending on database size." & vbCrLf & vbCrLf &
                "Continue with backup?"

            Dim result As DialogResult = MessageBox.Show(confirmationMessage,
                "Confirm Database Backup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result <> DialogResult.Yes Then
                Exit Sub
            End If
        Catch ex As Exception
            ' Continue without size info if we can't get it
            Dim result As DialogResult = MessageBox.Show(
                $"Create backup of database '{txtDatabase.Text}'?" & vbCrLf & vbCrLf &
                "This operation may take several minutes depending on database size." & vbCrLf & vbCrLf &
                "Continue?",
                "Confirm Database Backup",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If result <> DialogResult.Yes Then
                Exit Sub
            End If
        End Try

        ' Initialize enhanced backup manager
        Dim backupManager As New DatabaseBackupManager()

        ' Configure backup manager with enhanced settings
        backupManager.ConnectionString = BuildConnectionString()
        backupManager.DatabaseName = txtDatabase.Text.Trim()
        backupManager.OutputDirectory = txtOutput.Text.Trim()
        backupManager.BackupTimeoutSeconds = 1800 ' 30 minutes for large databases

        ' Get overwrite setting from configuration
        Dim settings As Dictionary(Of String, Object) = GetGenerationSettings()
        backupManager.OverwriteExistingFiles = CBool(settings("OverwriteExistingFiles"))

        ' Set up event handlers for progress updates
        AddHandler backupManager.ProgressUpdated, AddressOf OnBackupProgressUpdated
        AddHandler backupManager.StatusUpdated, AddressOf OnBackupStatusUpdated

        ' Prepare UI for backup operation
        SetControlsEnabled(False)
        pnlProgress.Visible = True
        toolStripProgressBar.Visible = True
        progressBar.Style = ProgressBarStyle.Continuous
        toolStripProgressBar.Style = ProgressBarStyle.Continuous
        progressBar.Value = 0
        toolStripProgressBar.Value = 0

        Try
            UpdateStatus("Initializing enhanced database backup...")
            lblProgress.Text = "Determining optimal backup strategy..."

            ' Perform the backup with enhanced error handling
            Dim backupResult As DatabaseBackupManager.BackupResult = Await backupManager.CreateBackupAsync()

            ' Handle the result with enhanced feedback
            If backupResult.Success Then
                ' Update progress to 100%
                progressBar.Value = progressBar.Maximum
                toolStripProgressBar.Value = toolStripProgressBar.Maximum

                ' Show detailed success message
                Dim successMessage As String = BuildSuccessMessage(backupResult)

                Dim openFolderResult As DialogResult = MessageBox.Show(successMessage,
                                                                      "Backup Completed Successfully",
                                                                      MessageBoxButtons.YesNo,
                                                                      MessageBoxIcon.Information)

                If openFolderResult = DialogResult.Yes Then
                    OpenOutputFolder()
                End If

                UpdateStatus($"Database backup completed: {backupResult.FileName}")

            Else
                ' Show enhanced error message with troubleshooting tips
                ShowEnhancedErrorDialog(backupResult.ErrorMessage)
                UpdateStatus("Database backup failed.")
            End If

        Catch ex As Exception
            ' Handle unexpected errors with guidance
            UpdateStatus("Database backup failed.")
            ShowEnhancedErrorDialog($"Unexpected error during backup: {ex.Message}")
        Finally
            ' Clean up event handlers
            RemoveHandler backupManager.ProgressUpdated, AddressOf OnBackupProgressUpdated
            RemoveHandler backupManager.StatusUpdated, AddressOf OnBackupStatusUpdated

            ' Re-enable controls
            SetControlsEnabled(True)
            pnlProgress.Visible = False
            toolStripProgressBar.Visible = False
            progressBar.Style = ProgressBarStyle.Continuous
            toolStripProgressBar.Style = ProgressBarStyle.Continuous
        End Try
    End Sub

    ''' <summary>
    ''' Alternative backup method using SQLCMD - works around permission issues
    ''' </summary>
    Private Async Sub BackupDatabaseUsingSqlCmd()
        ' Validate connection
        If Not isConnected Then
            MessageBox.Show("Please test the database connection first.", "Not Connected",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validate output directory
        If String.IsNullOrEmpty(txtOutput.Text) OrElse Not Directory.Exists(txtOutput.Text) Then
            MessageBox.Show("Please select a valid output directory first.", "Invalid Output Directory",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Confirm backup operation
        Dim result As DialogResult = MessageBox.Show(
            $"Create backup of database '{txtDatabase.Text}' using SQLCMD method?" & vbCrLf & vbCrLf &
            "This method uses SQLCMD and may work better with permission issues." & vbCrLf & vbCrLf &
            "Continue?",
            "Alternative Database Backup",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result <> DialogResult.Yes Then
            Exit Sub
        End If

        ' Prepare UI
        SetControlsEnabled(False)
        pnlProgress.Visible = True
        toolStripProgressBar.Visible = True
        progressBar.Style = ProgressBarStyle.Marquee
        toolStripProgressBar.Style = ProgressBarStyle.Marquee

        Try
            UpdateStatus("Creating backup using SQLCMD method...")
            lblProgress.Text = "Preparing backup command..."

            ' Create timestamp and file names
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim backupFileName As String = $"{txtDatabase.Text}_{timestamp}.bak"
            Dim zipFileName As String = $"{txtDatabase.Text}_Backup_{timestamp}.zip"

            ' Use a simple backup location that usually works
            Dim backupPath As String = Path.Combine("C:\Temp", backupFileName)

            ' Ensure C:\Temp exists
            If Not Directory.Exists("C:\Temp") Then
                Directory.CreateDirectory("C:\Temp")
            End If

            ' Build SQLCMD backup command
            Dim sqlCommand As String = $"BACKUP DATABASE [{txtDatabase.Text}] TO DISK = N'{backupPath}' WITH FORMAT, INIT, NAME = N'RepoSQL Backup - {DateTime.Now:yyyy-MM-dd HH:mm:ss}', SKIP, NOREWIND, NOUNLOAD, STATS = 10;"

            ' Create temporary SQL file
            Dim tempSqlFile As String = Path.Combine(Path.GetTempPath(), $"backup_{Guid.NewGuid().ToString("N").Substring(0, 8)}.sql")
            Await File.WriteAllTextAsync(tempSqlFile, sqlCommand)

            lblProgress.Text = "Executing backup command via SQLCMD..."

            ' Build SQLCMD command arguments
            Dim sqlcmdArgs As String
            If chkIntegratedSecurity.Checked Then
                sqlcmdArgs = $"-S ""{txtServer.Text}"" -E -i ""{tempSqlFile}"""
            Else
                sqlcmdArgs = $"-S ""{txtServer.Text}"" -U ""{txtUsername.Text}"" -P ""{txtPassword.Text}"" -i ""{tempSqlFile}"""
            End If

            ' Execute SQLCMD
            Dim processInfo As New ProcessStartInfo() With {
                .FileName = "sqlcmd.exe",
                .Arguments = sqlcmdArgs,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True,
                .CreateNoWindow = True
            }

            Using process As Process = Process.Start(processInfo)
                Dim output As String = Await process.StandardOutput.ReadToEndAsync()
                Dim errorOutput As String = Await process.StandardError.ReadToEndAsync()
                Await Task.Run(Sub() process.WaitForExit())

                If process.ExitCode <> 0 Then
                    Throw New Exception($"SQLCMD backup failed: {errorOutput}")
                End If
            End Using

            ' Clean up temp SQL file
            If File.Exists(tempSqlFile) Then
                File.Delete(tempSqlFile)
            End If

            ' Verify backup file was created
            If Not File.Exists(backupPath) Then
                Throw New Exception("Backup file was not created.")
            End If

            lblProgress.Text = "Compressing backup file..."

            ' Get backup file size
            Dim backupFileInfo As New FileInfo(backupPath)
            Dim backupSizeMB As Double = Math.Round(backupFileInfo.Length / (1024.0 * 1024.0), 2)

            ' Create zip file
            Dim finalZipPath As String = Path.Combine(txtOutput.Text, zipFileName)

            ' Delete existing zip if present
            If File.Exists(finalZipPath) Then
                File.Delete(finalZipPath)
            End If

            ' Create zip archive
            Using archive As ZipArchive = ZipFile.Open(finalZipPath, ZipArchiveMode.Create)
                archive.CreateEntryFromFile(backupPath, backupFileName, CompressionLevel.Optimal)
            End Using

            ' Get compressed size
            Dim zipFileInfo As New FileInfo(finalZipPath)
            Dim compressedSizeMB As Double = Math.Round(zipFileInfo.Length / (1024.0 * 1024.0), 2)
            Dim compressionRatio As Double = Math.Round((1 - (zipFileInfo.Length / CDbl(backupFileInfo.Length))) * 100, 1)

            ' Clean up backup file
            File.Delete(backupPath)

            ' Show success message
            Dim successMessage As String =
                $"✅ Database backup completed successfully using SQLCMD method!" & vbCrLf & vbCrLf &
                $"Database: {txtDatabase.Text}" & vbCrLf &
                $"Original Size: {backupSizeMB:F2} MB" & vbCrLf &
                $"Compressed Size: {compressedSizeMB:F2} MB" & vbCrLf &
                $"Compression: {compressionRatio:F1}% size reduction" & vbCrLf & vbCrLf &
                $"File: {zipFileName}" & vbCrLf & vbCrLf &
                "Would you like to open the output folder?"

            Dim openFolderResult As DialogResult = MessageBox.Show(successMessage,
                                                                  "Backup Complete",
                                                                  MessageBoxButtons.YesNo,
                                                                  MessageBoxIcon.Information)

            If openFolderResult = DialogResult.Yes Then
                OpenOutputFolder()
            End If

            UpdateStatus($"SQLCMD backup completed: {zipFileName}")

        Catch ex As Exception
            UpdateStatus("SQLCMD backup failed.")
            MessageBox.Show($"SQLCMD backup method failed:" & vbCrLf & vbCrLf & ex.Message & vbCrLf & vbCrLf &
                           "Try these solutions:" & vbCrLf &
                           "1. Run this application as Administrator" & vbCrLf &
                           "2. Ensure SQLCMD is installed and in PATH" & vbCrLf &
                           "3. Use SQL Server Management Studio to create the backup manually" & vbCrLf &
                           "4. Ask your database administrator for help",
                           "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetControlsEnabled(True)
            pnlProgress.Visible = False
            toolStripProgressBar.Visible = False
            progressBar.Style = ProgressBarStyle.Continuous
            toolStripProgressBar.Style = ProgressBarStyle.Continuous
        End Try
    End Sub

    ''' <summary>
    ''' Check for backup privileges and provide warnings
    ''' </summary>
    Private Function CheckBackupPrivileges() As String
        Try
            ' Check if running as administrator
            Dim identity As WindowsIdentity = WindowsIdentity.GetCurrent()
            Dim principal As WindowsPrincipal = New WindowsPrincipal(identity)
            Dim isAdmin As Boolean = principal.IsInRole(WindowsBuiltInRole.Administrator)

            If Not isAdmin Then
                Return "Warning: You are not running as Administrator. " &
                       "This may cause permission issues when SQL Server tries to create backup files. " &
                       "For best results, try running this application as Administrator."
            End If

        Catch ex As Exception
            Return $"Warning: Could not determine privilege level: {ex.Message}"
        End Try

        Return String.Empty
    End Function

    ''' <summary>
    ''' Build detailed success message for backup completion
    ''' </summary>
    Private Function BuildSuccessMessage(backupResult As DatabaseBackupManager.BackupResult) As String
        Dim message As New StringBuilder()

        message.AppendLine("✅ Database backup completed successfully!")
        message.AppendLine()
        message.AppendLine("📋 Backup Details:")
        message.AppendLine($"   Database: {txtDatabase.Text}")
        message.AppendLine($"   Original Size: {backupResult.BackupSizeMB:F2} MB")
        message.AppendLine($"   Compressed Size: {backupResult.CompressedSizeMB:F2} MB")
        message.AppendLine($"   Compression Ratio: {backupResult.CompressionRatio:F1}% size reduction")
        message.AppendLine($"   Time Taken: {backupResult.TimeTaken.ToString("mm\:ss")} (mm:ss)")
        message.AppendLine()
        message.AppendLine("📁 Files Created:")
        message.AppendLine($"   ZIP File: {backupResult.FileName}")
        message.AppendLine($"   Location: {Path.GetDirectoryName(backupResult.FinalFilePath)}")

        If Not String.IsNullOrEmpty(backupResult.BackupDirectory) Then
            message.AppendLine($"   Backup Directory Used: {backupResult.BackupDirectory}")
        End If

        message.AppendLine()
        message.AppendLine("🎉 Your database backup is ready!")
        message.AppendLine()
        message.AppendLine("Would you like to open the output folder?")

        Return message.ToString()
    End Function

    ''' <summary>
    ''' Show enhanced error dialog with troubleshooting guidance
    ''' </summary>
    Private Sub ShowEnhancedErrorDialog(errorMessage As String)
        Dim enhancedMessage As New StringBuilder()

        enhancedMessage.AppendLine("❌ Database Backup Failed")
        enhancedMessage.AppendLine()
        enhancedMessage.AppendLine("Error Details:")
        enhancedMessage.AppendLine(errorMessage)
        enhancedMessage.AppendLine()
        enhancedMessage.AppendLine("🔧 Troubleshooting Steps:")
        enhancedMessage.AppendLine()
        enhancedMessage.AppendLine("1. Permission Issues:")
        enhancedMessage.AppendLine("   • Run this application as Administrator")
        enhancedMessage.AppendLine("   • Ensure SQL Server service has write permissions")
        enhancedMessage.AppendLine("   • Check that the output directory is accessible")
        enhancedMessage.AppendLine()
        enhancedMessage.AppendLine("2. SQL Server Issues:")
        enhancedMessage.AppendLine("   • Verify SQL Server is running and accessible")
        enhancedMessage.AppendLine("   • Check database connection settings")
        enhancedMessage.AppendLine("   • Ensure sufficient disk space is available")
        enhancedMessage.AppendLine()
        enhancedMessage.AppendLine("3. Alternative Solutions:")
        enhancedMessage.AppendLine("   • Try the SQLCMD backup method")
        enhancedMessage.AppendLine("   • Use SQL Server Management Studio")
        enhancedMessage.AppendLine("   • Use a different output directory")
        enhancedMessage.AppendLine("   • Contact your database administrator")

        ' Create custom error dialog
        Using errorForm As New Form()
            errorForm.Text = "Database Backup Error"
            errorForm.Size = New Size(600, 500)
            errorForm.StartPosition = FormStartPosition.CenterParent
            errorForm.FormBorderStyle = FormBorderStyle.FixedDialog
            errorForm.MaximizeBox = False
            errorForm.MinimizeBox = False
            errorForm.ShowIcon = False

            Dim textBox As New TextBox()
            textBox.Multiline = True
            textBox.ScrollBars = ScrollBars.Vertical
            textBox.ReadOnly = True
            textBox.Text = enhancedMessage.ToString()
            textBox.Font = New Font("Consolas", 9)
            textBox.Dock = DockStyle.Fill
            textBox.BackColor = Color.White
            textBox.Margin = New Padding(10)

            Dim panel As New Panel()
            panel.Dock = DockStyle.Fill
            panel.Padding = New Padding(10)
            panel.Controls.Add(textBox)

            Dim buttonPanel As New Panel()
            buttonPanel.Dock = DockStyle.Bottom
            buttonPanel.Height = 50
            buttonPanel.Padding = New Padding(10)

            Dim okButton As New Button()
            okButton.Text = "OK"
            okButton.DialogResult = DialogResult.OK
            okButton.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
            okButton.Location = New Point(buttonPanel.Width - 90, 15)
            okButton.Size = New Size(75, 25)

            buttonPanel.Controls.Add(okButton)
            errorForm.Controls.Add(panel)
            errorForm.Controls.Add(buttonPanel)
            errorForm.AcceptButton = okButton

            errorForm.ShowDialog(Me)
        End Using
    End Sub

    ''' <summary>
    ''' Handle backup progress updates with enhanced feedback
    ''' </summary>
    ''' <param name="message">Progress message</param>
    ''' <param name="percentage">Progress percentage (-1 if unknown)</param>
    Private Sub OnBackupProgressUpdated(message As String, percentage As Integer)
        If Me.InvokeRequired Then
            Me.BeginInvoke(Sub() OnBackupProgressUpdated(message, percentage))
            Return
        End If

        ' Enhanced progress display
        lblProgress.Text = $"🔄 {message}"

        If percentage >= 0 AndAlso percentage <= 100 Then
            progressBar.Value = Math.Min(percentage, progressBar.Maximum)
            toolStripProgressBar.Value = Math.Min(percentage, toolStripProgressBar.Maximum)

            ' Add percentage to status if available
            If percentage > 0 Then
                lblProgress.Text = $"🔄 {message} ({percentage}%)"
            End If
        End If

        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Handle status updates with enhanced formatting
    ''' </summary>
    ''' <param name="message">Status message</param>
    Private Sub OnBackupStatusUpdated(message As String)
        If Me.InvokeRequired Then
            Me.BeginInvoke(Sub() OnBackupStatusUpdated(message))
            Return
        End If

        UpdateStatus($"🔄 {message}")
    End Sub

    ''' <summary>
    ''' Get database size information for user confirmation
    ''' </summary>
    ''' <returns>Database size in MB</returns>
    Private Async Function GetDatabaseSizeAsync() As Task(Of Double)
        Try
            connectionString = BuildConnectionString()

            Using connection As New SqlConnection(connectionString)
                Await connection.OpenAsync()

                Dim sizeSql As String = $"
                    SELECT 
                        SUM(CAST(FILEPROPERTY(name, 'SpaceUsed') AS bigint) * 8.0 / 1024) as SizeMB
                    FROM sys.database_files 
                    WHERE type_desc = 'ROWS'"

                Using command As New SqlCommand(sizeSql, connection)
                    command.CommandTimeout = 30
                    Dim result As Object = Await command.ExecuteScalarAsync()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        Return Convert.ToDouble(result)
                    End If
                End Using
            End Using

        Catch ex As Exception
            Debug.WriteLine($"Error getting database size: {ex.Message}")
        End Try

        Return 0.0
    End Function

    ''' <summary>
    ''' Open the output folder in Windows Explorer with error handling
    ''' </summary>
    Private Sub OpenOutputFolder()
        If Directory.Exists(txtOutput.Text) Then
            Try
                Process.Start(New ProcessStartInfo() With {
                    .FileName = "explorer.exe",
                    .Arguments = txtOutput.Text,
                    .UseShellExecute = True
                })
            Catch ex As Exception
                MessageBox.Show($"Could not open folder: {ex.Message}" & vbCrLf & vbCrLf &
                              $"Please navigate manually to: {txtOutput.Text}",
                              "Cannot Open Folder",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

        If String.IsNullOrEmpty(txtOutput.Text) Then
            MessageBox.Show("Please select an output folder.", "No Output Folder",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Ensure output directory exists
        Try
            If Not Directory.Exists(txtOutput.Text) Then
                Directory.CreateDirectory(txtOutput.Text)
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

            Dim outputFiles As List(Of String) = Await GenerateScriptsWithSettings()

            progressBar.Value = progressBar.Maximum
            toolStripProgressBar.Value = toolStripProgressBar.Maximum

            Dim message As String = "SQL scripts generated successfully!" & vbCrLf & vbCrLf &
                                  "Generated files:" & vbCrLf &
                                  String.Join(vbCrLf, outputFiles.Select(Function(f) "• " & Path.GetFileName(f)))

            Dim result As DialogResult = MessageBox.Show(message & vbCrLf & vbCrLf & "Would you like to open the output folder?",
                                                       "Generation Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

            If result = DialogResult.Yes Then
                OpenOutputFolder()
            End If

            UpdateStatus("Generation completed successfully.")

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
        btnBackupDatabase.Enabled = enabled AndAlso isConnected
        grpConnection.Enabled = enabled
        grpOptions.Enabled = enabled
        pnlTableActions.Enabled = enabled
        lstTables.Enabled = enabled
    End Sub

    Private Async Function GenerateScriptsWithSettings() As Task(Of List(Of String))
        Dim outputFiles As New List(Of String)

        Using connection As New SqlConnection(connectionString)
            Await connection.OpenAsync()

            ' Calculate total operations for progress
            Dim totalOps As Integer = 0
            If chkTables.Checked Then totalOps += 1
            If chkIncludeData.Checked AndAlso selectedTables.Count > 0 Then totalOps += 1
            If chkViews.Checked Then totalOps += 1
            If chkStoredProcedures.Checked Then totalOps += 1
            If chkFunctions.Checked Then totalOps += 1
            If chkIndexes.Checked Then totalOps += 1
            If chkTriggers.Checked Then totalOps += 1
            If chkPermissions.Checked Then totalOps += 1

            progressBar.Maximum = totalOps
            toolStripProgressBar.Maximum = totalOps
            Dim currentOp As Integer = 0

            ' Generate database header
            Dim dbHeader As String = GenerateDatabaseHeader(txtDatabase.Text)

            ' Generate scripts using settings-aware methods
            If chkTables.Checked Then
                lblProgress.Text = "Generating table creation scripts..."
                Dim tableScript As String = Await GenerateTableCreationScriptWithSettings(connection, dbHeader)
                If Not String.IsNullOrEmpty(tableScript) Then
                    Dim tablePath As String = Path.Combine(txtOutput.Text, "01_TableCreate.sql")
                    If Await WriteScriptFileWithSettings(tablePath, tableScript) Then
                        outputFiles.Add(tablePath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            ' Table Insert remains the same as it doesn't need DROP statements
            If chkIncludeData.Checked AndAlso selectedTables.Count > 0 Then
                lblProgress.Text = "Generating table insert scripts..."
                Dim insertScript As String = Await GenerateTableInsertScript(connection, dbHeader)
                If Not String.IsNullOrEmpty(insertScript) Then
                    Dim insertPath As String = Path.Combine(txtOutput.Text, "02_TableInsert.sql")
                    If Await WriteScriptFileWithSettings(insertPath, insertScript) Then
                        outputFiles.Add(insertPath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If chkViews.Checked Then
                lblProgress.Text = "Generating views scripts..."
                Dim viewScript As String = Await GenerateViewsScriptWithSettings(connection, dbHeader)
                If Not String.IsNullOrEmpty(viewScript) Then
                    Dim viewPath As String = Path.Combine(txtOutput.Text, "03_Views.sql")
                    If Await WriteScriptFileWithSettings(viewPath, viewScript) Then
                        outputFiles.Add(viewPath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If chkStoredProcedures.Checked Then
                lblProgress.Text = "Generating stored procedures scripts..."
                Dim procScript As String = Await GenerateStoredProceduresScriptWithSettings(connection, dbHeader)
                If Not String.IsNullOrEmpty(procScript) Then
                    Dim procPath As String = Path.Combine(txtOutput.Text, "04_StoredProcedures.sql")
                    If Await WriteScriptFileWithSettings(procPath, procScript) Then
                        outputFiles.Add(procPath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If chkFunctions.Checked Then
                lblProgress.Text = "Generating functions scripts..."
                Dim funcScript As String = Await GenerateFunctionsScriptWithSettings(connection, dbHeader)
                If Not String.IsNullOrEmpty(funcScript) Then
                    Dim funcPath As String = Path.Combine(txtOutput.Text, "05_Functions.sql")
                    If Await WriteScriptFileWithSettings(funcPath, funcScript) Then
                        outputFiles.Add(funcPath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If chkIndexes.Checked Then
                lblProgress.Text = "Generating indexes scripts..."
                Dim indexScript As String = Await GenerateIndexesScript(connection, dbHeader)
                If Not String.IsNullOrEmpty(indexScript) Then
                    Dim indexPath As String = Path.Combine(txtOutput.Text, "06_Indexes.sql")
                    If Await WriteScriptFileWithSettings(indexPath, indexScript) Then
                        outputFiles.Add(indexPath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If chkTriggers.Checked Then
                lblProgress.Text = "Generating triggers scripts..."
                Dim triggerScript As String = Await GenerateTriggersScript(connection, dbHeader)
                If Not String.IsNullOrEmpty(triggerScript) Then
                    Dim triggerPath As String = Path.Combine(txtOutput.Text, "07_Triggers.sql")
                    If Await WriteScriptFileWithSettings(triggerPath, triggerScript) Then
                        outputFiles.Add(triggerPath)
                    End If
                End If
                currentOp += 1
                UpdateProgress(currentOp)
            End If

            If chkPermissions.Checked Then
                lblProgress.Text = "Generating permissions scripts..."
                Dim permScript As String = Await GeneratePermissionsScript(connection, dbHeader)
                If Not String.IsNullOrEmpty(permScript) Then
                    Dim permPath As String = Path.Combine(txtOutput.Text, "08_Permissions.sql")
                    If Await WriteScriptFileWithSettings(permPath, permScript) Then
                        outputFiles.Add(permPath)
                    End If
                End If
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

    ''' <summary>
    ''' Modified file writing with overwrite settings support
    ''' </summary>
    Private Async Function WriteScriptFileWithSettings(filePath As String, content As String) As Task(Of Boolean)
        Try
            Dim settings As Dictionary(Of String, Object) = GetGenerationSettings()
            Dim overwriteExistingFiles As Boolean = CBool(settings("OverwriteExistingFiles"))

            ' Check if file exists and handle overwrite setting
            If File.Exists(filePath) AndAlso Not overwriteExistingFiles Then
                Dim result As DialogResult = MessageBox.Show(
                    $"File '{Path.GetFileName(filePath)}' already exists. Do you want to overwrite it?",
                    "File Exists",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)

                If result <> DialogResult.Yes Then
                    Return False
                End If
            End If

            ' Write the file
            Await File.WriteAllTextAsync(filePath, content)
            Return True

        Catch ex As Exception
            MessageBox.Show($"Error writing file '{Path.GetFileName(filePath)}': {ex.Message}",
                          "File Write Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Modified table creation script generation with DROP statements support
    ''' </summary>
    Private Async Function GenerateTableCreationScriptWithSettings(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        Dim settings As Dictionary(Of String, Object) = GetGenerationSettings()
        Dim generateDropStatements As Boolean = CBool(settings("GenerateDropStatements"))

        script.AppendLine("-- ====================================")
        script.AppendLine("-- TABLE CREATION SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        For Each tableName As String In selectedTables
            script.AppendLine($"-- Table: {tableName}")

            If generateDropStatements Then
                script.AppendLine($"IF EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')")
                script.AppendLine($"    DROP TABLE [dbo].[{tableName}];")
                script.AppendLine("GO")
                script.AppendLine()
            End If

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

    Private Async Function GenerateTableInsertScript(connection As SqlConnection, dbHeader As String) As Task(Of String)
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

    ''' <summary>
    ''' Enhanced views script generation with actual SQL definitions
    ''' </summary>
    Private Async Function GenerateViewsScriptWithSettings(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        Dim settings As Dictionary(Of String, Object) = GetGenerationSettings()
        Dim generateDropStatements As Boolean = CBool(settings("GenerateDropStatements"))

        script.AppendLine("-- ====================================")
        script.AppendLine("-- VIEWS SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get all views in the database
        Dim viewQuery As String = "
            SELECT 
                v.TABLE_NAME as ViewName,
                v.VIEW_DEFINITION as ViewDefinition
            FROM INFORMATION_SCHEMA.VIEWS v
            WHERE v.TABLE_CATALOG = DB_NAME()
            ORDER BY v.TABLE_NAME"

        Try
            Using command As New SqlCommand(viewQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim viewCount As Integer = 0
                    While Await reader.ReadAsync()
                        Dim viewName As String = reader.GetString("ViewName")
                        Dim viewDefinition As String = If(reader.IsDBNull("ViewDefinition"), "", reader.GetString("ViewDefinition"))

                        script.AppendLine($"-- ==========================================")
                        script.AppendLine($"-- View: {viewName}")
                        script.AppendLine($"-- ==========================================")

                        If generateDropStatements Then
                            script.AppendLine($"IF EXISTS (SELECT * FROM sys.views WHERE name = '{viewName}' AND schema_id = SCHEMA_ID('dbo'))")
                            script.AppendLine($"    DROP VIEW [dbo].[{viewName}];")
                            script.AppendLine("GO")
                            script.AppendLine()
                        End If

                        ' Clean and format the view definition
                        If Not String.IsNullOrEmpty(viewDefinition) Then
                            ' Remove any CREATE VIEW part if it exists and add our own
                            Dim cleanDefinition As String = CleanViewDefinition(viewDefinition)
                            script.AppendLine($"CREATE VIEW [dbo].[{viewName}] AS")
                            script.AppendLine(cleanDefinition)
                        Else
                            script.AppendLine($"-- CREATE VIEW [dbo].[{viewName}] AS")
                            script.AppendLine($"-- TODO: Add view definition for {viewName}")
                        End If

                        script.AppendLine("GO")
                        script.AppendLine()
                        viewCount += 1
                    End While

                    If viewCount = 0 Then
                        script.AppendLine("-- No views found in the database")
                        script.AppendLine()
                    End If
                End Using
            End Using
        Catch ex As Exception
            script.AppendLine($"-- Error retrieving views: {ex.Message}")
            script.AppendLine()
        End Try

        Return script.ToString()
    End Function

    ''' <summary>
    ''' Enhanced stored procedures script generation with actual SQL definitions
    ''' </summary>
    Private Async Function GenerateStoredProceduresScriptWithSettings(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        Dim settings As Dictionary(Of String, Object) = GetGenerationSettings()
        Dim generateDropStatements As Boolean = CBool(settings("GenerateDropStatements"))

        script.AppendLine("-- ====================================")
        script.AppendLine("-- STORED PROCEDURES SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get stored procedures with their definitions
        Dim procQuery As String = "
            SELECT 
                r.ROUTINE_NAME as ProcedureName,
                r.ROUTINE_DEFINITION as ProcedureDefinition
            FROM INFORMATION_SCHEMA.ROUTINES r
            WHERE r.ROUTINE_TYPE = 'PROCEDURE' 
            AND r.ROUTINE_CATALOG = DB_NAME()
            AND r.ROUTINE_SCHEMA = 'dbo'
            ORDER BY r.ROUTINE_NAME"

        Try
            Using command As New SqlCommand(procQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim procCount As Integer = 0
                    While Await reader.ReadAsync()
                        Dim procName As String = reader.GetString("ProcedureName")
                        Dim procDefinition As String = If(reader.IsDBNull("ProcedureDefinition"), "", reader.GetString("ProcedureDefinition"))

                        script.AppendLine($"-- ==========================================")
                        script.AppendLine($"-- Stored Procedure: {procName}")
                        script.AppendLine($"-- ==========================================")

                        If generateDropStatements Then
                            script.AppendLine($"IF EXISTS (SELECT * FROM sys.procedures WHERE name = '{procName}' AND schema_id = SCHEMA_ID('dbo'))")
                            script.AppendLine($"    DROP PROCEDURE [dbo].[{procName}];")
                            script.AppendLine("GO")
                            script.AppendLine()
                        End If

                        ' Clean and format the procedure definition
                        If Not String.IsNullOrEmpty(procDefinition) Then
                            Dim cleanDefinition As String = CleanProcedureDefinition(procDefinition, procName)
                            script.AppendLine(cleanDefinition)
                        Else
                            script.AppendLine($"CREATE PROCEDURE [dbo].[{procName}]")
                            script.AppendLine("AS")
                            script.AppendLine("BEGIN")
                            script.AppendLine($"    -- TODO: Add procedure implementation for {procName}")
                            script.AppendLine("END")
                        End If

                        script.AppendLine("GO")
                        script.AppendLine()
                        procCount += 1
                    End While

                    If procCount = 0 Then
                        script.AppendLine("-- No stored procedures found in the database")
                        script.AppendLine()
                    End If
                End Using
            End Using
        Catch ex As Exception
            script.AppendLine($"-- Error retrieving stored procedures: {ex.Message}")
            script.AppendLine()
        End Try

        Return script.ToString()
    End Function

    ''' <summary>
    ''' Enhanced functions script generation with actual SQL definitions
    ''' </summary>
    Private Async Function GenerateFunctionsScriptWithSettings(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        Dim settings As Dictionary(Of String, Object) = GetGenerationSettings()
        Dim generateDropStatements As Boolean = CBool(settings("GenerateDropStatements"))

        script.AppendLine("-- ====================================")
        script.AppendLine("-- FUNCTIONS SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get functions with their definitions
        Dim funcQuery As String = "
            SELECT 
                r.ROUTINE_NAME as FunctionName,
                r.ROUTINE_DEFINITION as FunctionDefinition,
                r.DATA_TYPE as ReturnType
            FROM INFORMATION_SCHEMA.ROUTINES r
            WHERE r.ROUTINE_TYPE = 'FUNCTION' 
            AND r.ROUTINE_CATALOG = DB_NAME()
            AND r.ROUTINE_SCHEMA = 'dbo'
            ORDER BY r.ROUTINE_NAME"

        Try
            Using command As New SqlCommand(funcQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim funcCount As Integer = 0
                    While Await reader.ReadAsync()
                        Dim funcName As String = reader.GetString("FunctionName")
                        Dim funcDefinition As String = If(reader.IsDBNull("FunctionDefinition"), "", reader.GetString("FunctionDefinition"))
                        Dim returnType As String = If(reader.IsDBNull("ReturnType"), "INT", reader.GetString("ReturnType"))

                        script.AppendLine($"-- ==========================================")
                        script.AppendLine($"-- Function: {funcName}")
                        script.AppendLine($"-- ==========================================")

                        If generateDropStatements Then
                            script.AppendLine($"IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('[dbo].[{funcName}]') AND type IN ('FN', 'IF', 'TF'))")
                            script.AppendLine($"    DROP FUNCTION [dbo].[{funcName}];")
                            script.AppendLine("GO")
                            script.AppendLine()
                        End If

                        ' Clean and format the function definition
                        If Not String.IsNullOrEmpty(funcDefinition) Then
                            Dim cleanDefinition As String = CleanFunctionDefinition(funcDefinition, funcName)
                            script.AppendLine(cleanDefinition)
                        Else
                            script.AppendLine($"CREATE FUNCTION [dbo].[{funcName}]()")
                            script.AppendLine($"RETURNS {returnType}")
                            script.AppendLine("AS")
                            script.AppendLine("BEGIN")
                            script.AppendLine($"    -- TODO: Add function implementation for {funcName}")
                            script.AppendLine($"    RETURN NULL")
                            script.AppendLine("END")
                        End If

                        script.AppendLine("GO")
                        script.AppendLine()
                        funcCount += 1
                    End While

                    If funcCount = 0 Then
                        script.AppendLine("-- No functions found in the database")
                        script.AppendLine()
                    End If
                End Using
            End Using
        Catch ex As Exception
            script.AppendLine($"-- Error retrieving functions: {ex.Message}")
            script.AppendLine()
        End Try

        Return script.ToString()
    End Function

    ''' <summary>
    ''' Enhanced indexes script generation with actual index definitions
    ''' </summary>
    Private Async Function GenerateIndexesScript(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- INDEXES SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get indexes for selected tables only
        Dim indexQuery As String = "
            SELECT 
                t.name AS TableName,
                i.name AS IndexName,
                i.type_desc AS IndexType,
                i.is_unique,
                i.is_primary_key,
                STUFF((
                    SELECT ', [' + c.name + ']' + 
                           CASE WHEN ic.is_descending_key = 1 THEN ' DESC' ELSE ' ASC' END
                    FROM sys.index_columns ic
                    INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                    WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id
                    ORDER BY ic.key_ordinal
                    FOR XML PATH('')
                ), 1, 2, '') AS IndexColumns
            FROM sys.indexes i
            INNER JOIN sys.tables t ON i.object_id = t.object_id
            WHERE t.name IN ('" & String.Join("','", selectedTables) & "')
            AND i.type > 0  -- Exclude heaps
            AND i.is_primary_key = 0  -- Exclude primary keys (handled with table creation)
            ORDER BY t.name, i.name"

        Try
            Using command As New SqlCommand(indexQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim indexCount As Integer = 0
                    While Await reader.ReadAsync()
                        Dim tableName As String = reader.GetString("TableName")
                        Dim indexName As String = reader.GetString("IndexName")
                        Dim indexType As String = reader.GetString("IndexType")
                        Dim isUnique As Boolean = reader.GetBoolean("is_unique")
                        Dim indexColumns As String = If(reader.IsDBNull("IndexColumns"), "", reader.GetString("IndexColumns"))

                        If Not String.IsNullOrEmpty(indexColumns) Then
                            script.AppendLine($"-- Index: {indexName} on table {tableName}")

                            Dim uniqueKeyword As String = If(isUnique, "UNIQUE ", "")
                            script.AppendLine($"CREATE {uniqueKeyword}INDEX [{indexName}] ON [dbo].[{tableName}]")
                            script.AppendLine($"({indexColumns});")
                            script.AppendLine()
                            indexCount += 1
                        End If
                    End While

                    If indexCount = 0 Then
                        script.AppendLine("-- No custom indexes found for selected tables")
                        script.AppendLine()
                    End If
                End Using
            End Using
        Catch ex As Exception
            script.AppendLine($"-- Error retrieving indexes: {ex.Message}")
            script.AppendLine()
        End Try

        script.AppendLine("GO")
        Return script.ToString()
    End Function

    ''' <summary>
    ''' Enhanced triggers script generation with actual trigger definitions
    ''' </summary>
    Private Async Function GenerateTriggersScript(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- TRIGGERS SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get triggers for selected tables
        Dim triggerQuery As String = "
            SELECT 
                t.name AS TriggerName,
                tab.name AS TableName,
                OBJECT_DEFINITION(t.object_id) AS TriggerDefinition
            FROM sys.triggers t
            INNER JOIN sys.tables tab ON t.parent_id = tab.object_id
            WHERE tab.name IN ('" & String.Join("','", selectedTables) & "')
            ORDER BY tab.name, t.name"

        Try
            Using command As New SqlCommand(triggerQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim triggerCount As Integer = 0
                    While Await reader.ReadAsync()
                        Dim triggerName As String = reader.GetString("TriggerName")
                        Dim tableName As String = reader.GetString("TableName")
                        Dim triggerDefinition As String = If(reader.IsDBNull("TriggerDefinition"), "", reader.GetString("TriggerDefinition"))

                        script.AppendLine($"-- ==========================================")
                        script.AppendLine($"-- Trigger: {triggerName} on table {tableName}")
                        script.AppendLine($"-- ==========================================")

                        If Not String.IsNullOrEmpty(triggerDefinition) Then
                            script.AppendLine(triggerDefinition)
                        Else
                            script.AppendLine($"-- CREATE TRIGGER [dbo].[{triggerName}] ON [dbo].[{tableName}]")
                            script.AppendLine($"-- TODO: Add trigger definition for {triggerName}")
                        End If

                        script.AppendLine("GO")
                        script.AppendLine()
                        triggerCount += 1
                    End While

                    If triggerCount = 0 Then
                        script.AppendLine("-- No triggers found for selected tables")
                        script.AppendLine()
                    End If
                End Using
            End Using
        Catch ex As Exception
            script.AppendLine($"-- Error retrieving triggers: {ex.Message}")
            script.AppendLine()
        End Try

        Return script.ToString()
    End Function

    ''' <summary>
    ''' Enhanced permissions script generation with actual permission definitions
    ''' </summary>
    Private Async Function GeneratePermissionsScript(connection As SqlConnection, dbHeader As String) As Task(Of String)
        Dim script As New StringBuilder(dbHeader)
        script.AppendLine("-- ====================================")
        script.AppendLine("-- PERMISSIONS SCRIPTS")
        script.AppendLine("-- ====================================")
        script.AppendLine()

        ' Get permissions for selected tables
        Dim permQuery As String = "
            SELECT 
                p.state_desc AS PermissionState,
                p.permission_name AS PermissionName,
                s.name AS PrincipalName,
                o.name AS ObjectName,
                o.type_desc AS ObjectType
            FROM sys.database_permissions p
            LEFT JOIN sys.objects o ON p.major_id = o.object_id
            LEFT JOIN sys.database_principals s ON p.grantee_principal_id = s.principal_id
            WHERE o.name IN ('" & String.Join("','", selectedTables) & "')
            ORDER BY o.name, s.name, p.permission_name"

        Try
            Using command As New SqlCommand(permQuery, connection)
                Using reader As SqlDataReader = Await command.ExecuteReaderAsync()
                    Dim permCount As Integer = 0
                    While Await reader.ReadAsync()
                        Dim permissionState As String = reader.GetString("PermissionState")
                        Dim permissionName As String = reader.GetString("PermissionName")
                        Dim principalName As String = reader.GetString("PrincipalName")
                        Dim objectName As String = reader.GetString("ObjectName")

                        script.AppendLine($"-- Permission: {permissionName} on {objectName} for {principalName}")
                        script.AppendLine($"{permissionState} {permissionName} ON [dbo].[{objectName}] TO [{principalName}];")
                        script.AppendLine()
                        permCount += 1
                    End While

                    If permCount = 0 Then
                        script.AppendLine("-- No explicit permissions found for selected tables")
                        script.AppendLine()
                    End If
                End Using
            End Using
        Catch ex As Exception
            script.AppendLine($"-- Error retrieving permissions: {ex.Message}")
            script.AppendLine()
        End Try

        script.AppendLine("GO")
        Return script.ToString()
    End Function

    ''' <summary>
    ''' Clean view definition for proper formatting
    ''' </summary>
    Private Function CleanViewDefinition(definition As String) As String
        If String.IsNullOrEmpty(definition) Then Return ""

        ' Remove any existing CREATE VIEW statements
        definition = System.Text.RegularExpressions.Regex.Replace(definition, "^\s*CREATE\s+VIEW\s+\S+\s+AS\s*", "",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase Or System.Text.RegularExpressions.RegexOptions.Multiline)

        Return definition.Trim()
    End Function

    ''' <summary>
    ''' Clean procedure definition for proper formatting
    ''' </summary>
    Private Function CleanProcedureDefinition(definition As String, procName As String) As String
        If String.IsNullOrEmpty(definition) Then
            Return $"CREATE PROCEDURE [dbo].[{procName}]" & vbCrLf & "AS" & vbCrLf & "BEGIN" & vbCrLf & $"    -- TODO: Add implementation for {procName}" & vbCrLf & "END"
        End If

        ' Clean up the definition
        definition = definition.Trim()

        ' If it doesn't start with CREATE, add it
        If Not definition.ToUpper().StartsWith("CREATE") Then
            definition = $"CREATE PROCEDURE [dbo].[{procName}]" & vbCrLf & definition
        End If

        Return definition
    End Function

    ''' <summary>
    ''' Clean function definition for proper formatting
    ''' </summary>
    Private Function CleanFunctionDefinition(definition As String, funcName As String) As String
        If String.IsNullOrEmpty(definition) Then
            Return $"CREATE FUNCTION [dbo].[{funcName}]()" & vbCrLf & "RETURNS INT" & vbCrLf & "AS" & vbCrLf & "BEGIN" & vbCrLf & $"    -- TODO: Add implementation for {funcName}" & vbCrLf & "    RETURN 0" & vbCrLf & "END"
        End If

        ' Clean up the definition
        definition = definition.Trim()

        ' If it doesn't start with CREATE, add it
        If Not definition.ToUpper().StartsWith("CREATE") Then
            definition = $"CREATE FUNCTION [dbo].[{funcName}]" & vbCrLf & definition
        End If

        Return definition
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
            LoadIniConfig() ' This will load defaults
        End If
    End Sub

    Private Sub loadProfileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles loadProfileToolStripMenuItem.Click
        Using openDialog As New OpenFileDialog()
            openDialog.Filter = "INI Files (*.ini)|*.ini|All Files (*.*)|*.*"
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

            If openDialog.ShowDialog() = DialogResult.OK Then
                Try
                    File.Copy(openDialog.FileName, iniFilePath, True)
                    LoadIniConfig()
                    LoadGenerationSettings() ' Reload generation settings too
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
            saveDialog.Filter = "INI Files (*.ini)|*.ini|All Files (*.*)|*.*"
            saveDialog.DefaultExt = "ini"
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            saveDialog.FileName = $"RepoSQL_Profile_{DateTime.Now:yyyyMMdd_HHmmss}.ini"

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Try
                    File.Copy(iniFilePath, saveDialog.FileName, True)
                    MessageBox.Show($"Profile saved successfully to: {saveDialog.FileName}", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show($"Error saving profile: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Updated settings menu item click handler
    ''' </summary>
    Private Sub settingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles settingsToolStripMenuItem.Click
        Using settingsForm As New frmSetting()
            If settingsForm.ShowDialog(Me) = DialogResult.OK Then
                ' Reload generation settings after changes
                LoadGenerationSettings()
                UpdateStatus("Settings updated successfully.")
            End If
        End Using
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
                                 "• INI-based configuration with auto-save" & vbCrLf &
                                 "• Settings for DROP statements and file overwrite" & vbCrLf &
                                 "• Fixed filename SQL script generation" & vbCrLf &
                                 "• Database backup and compression" & vbCrLf &
                                 "• Comprehensive script generation" & vbCrLf &
                                 "• Table search and filtering" & vbCrLf &
                                 "• Windows and SQL authentication" & vbCrLf & vbCrLf &
                                 "Generate professional SQL scripts and database backups with ease!"

        MessageBox.Show(aboutText, "About RepoSQL", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

#Region "Manual Save/Load Configuration Methods"

    ''' <summary>
    ''' Manually save current configuration
    ''' </summary>
    Private Sub ManualSaveConfig()
        SaveIniConfig()
        MessageBox.Show("Configuration saved to sconfig.ini", "Configuration Saved",
                       MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' Manually reload configuration
    ''' </summary>
    Private Sub ManualLoadConfig()
        If MessageBox.Show("Reload configuration from sconfig.ini? This will overwrite current settings.",
                          "Reload Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            LoadIniConfig()
            LoadGenerationSettings()
            MessageBox.Show("Configuration reloaded from sconfig.ini", "Configuration Loaded",
                           MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

End Class