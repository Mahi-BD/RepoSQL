<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        toolTip = New ToolTip(components)
        btnSelectAll = New Button()
        btnSelectNone = New Button()
        btnInvertSelection = New Button()
        txtTableSearch = New TextBox()
        lstTables = New CheckedListBox()
        btnGenerate = New Button()
        btnSelectOutput = New Button()
        btnBackupDatabase = New Button()
        txtOutput = New TextBox()
        chkIncludeData = New CheckBox()
        chkTables = New CheckBox()
        chkViews = New CheckBox()
        chkStoredProcedures = New CheckBox()
        chkFunctions = New CheckBox()
        chkIndexes = New CheckBox()
        chkTriggers = New CheckBox()
        chkPermissions = New CheckBox()
        btnTestConnection = New Button()
        btnLoadTables = New Button()
        btnSaveProfile = New Button()
        txtServer = New TextBox()
        txtDatabase = New TextBox()
        txtUsername = New TextBox()
        txtPassword = New TextBox()
        chkIntegratedSecurity = New CheckBox()
        menuStrip = New MenuStrip()
        fileToolStripMenuItem = New ToolStripMenuItem()
        newProfileToolStripMenuItem = New ToolStripMenuItem()
        loadProfileToolStripMenuItem = New ToolStripMenuItem()
        saveProfileToolStripMenuItem = New ToolStripMenuItem()
        toolStripSeparator1 = New ToolStripSeparator()
        exitToolStripMenuItem = New ToolStripMenuItem()
        toolsToolStripMenuItem = New ToolStripMenuItem()
        settingsToolStripMenuItem = New ToolStripMenuItem()
        helpToolStripMenuItem = New ToolStripMenuItem()
        aboutToolStripMenuItem = New ToolStripMenuItem()
        statusStrip = New StatusStrip()
        toolStripStatusLabel = New ToolStripStatusLabel()
        toolStripProgressBar = New ToolStripProgressBar()
        toolStripSpring = New ToolStripStatusLabel()
        toolStripConnectionStatus = New ToolStripStatusLabel()
        pnlMain = New Panel()
        pnlRight = New Panel()
        grpTables = New GroupBox()
        pnlTableActions = New Panel()
        lblTableCount = New Label()
        lblTableSearch = New Label()
        pnlLeft = New Panel()
        grpGeneration = New GroupBox()
        lblProgress = New Label()
        pnlProgress = New Panel()
        progressBar = New ProgressBar()
        pnlGenerationActions = New Panel()
        lblOutput = New Label()
        grpOptions = New GroupBox()
        pnlOptions = New Panel()
        grpConnection = New GroupBox()
        pnlConnectionStatus = New Panel()
        lblConnectionStatus = New Label()
        picConnectionStatus = New PictureBox()
        pnlConnectionActions = New Panel()
        tblConnection = New TableLayoutPanel()
        lblServer = New Label()
        lblDatabase = New Label()
        lblUsername = New Label()
        lblPassword = New Label()
        menuStrip.SuspendLayout()
        statusStrip.SuspendLayout()
        pnlMain.SuspendLayout()
        pnlRight.SuspendLayout()
        grpTables.SuspendLayout()
        pnlTableActions.SuspendLayout()
        pnlLeft.SuspendLayout()
        grpGeneration.SuspendLayout()
        pnlProgress.SuspendLayout()
        grpOptions.SuspendLayout()
        pnlOptions.SuspendLayout()
        grpConnection.SuspendLayout()
        pnlConnectionStatus.SuspendLayout()
        CType(picConnectionStatus, ComponentModel.ISupportInitialize).BeginInit()
        pnlConnectionActions.SuspendLayout()
        tblConnection.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnSelectAll
        ' 
        btnSelectAll.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnSelectAll.FlatAppearance.BorderSize = 0
        btnSelectAll.FlatStyle = FlatStyle.Flat
        btnSelectAll.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnSelectAll.ForeColor = Color.White
        btnSelectAll.Location = New Point(0, 8)
        btnSelectAll.Margin = New Padding(2)
        btnSelectAll.Name = "btnSelectAll"
        btnSelectAll.Size = New Size(112, 28)
        btnSelectAll.TabIndex = 0
        btnSelectAll.Text = "Select All"
        toolTip.SetToolTip(btnSelectAll, "Select all tables in the list")
        btnSelectAll.UseVisualStyleBackColor = False
        ' 
        ' btnSelectNone
        ' 
        btnSelectNone.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnSelectNone.FlatAppearance.BorderSize = 0
        btnSelectNone.FlatStyle = FlatStyle.Flat
        btnSelectNone.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnSelectNone.ForeColor = Color.White
        btnSelectNone.Location = New Point(124, 8)
        btnSelectNone.Margin = New Padding(2)
        btnSelectNone.Name = "btnSelectNone"
        btnSelectNone.Size = New Size(112, 28)
        btnSelectNone.TabIndex = 1
        btnSelectNone.Text = "Select None"
        toolTip.SetToolTip(btnSelectNone, "Deselect all tables in the list")
        btnSelectNone.UseVisualStyleBackColor = False
        ' 
        ' btnInvertSelection
        ' 
        btnInvertSelection.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnInvertSelection.FlatAppearance.BorderSize = 0
        btnInvertSelection.FlatStyle = FlatStyle.Flat
        btnInvertSelection.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnInvertSelection.ForeColor = Color.White
        btnInvertSelection.Location = New Point(248, 8)
        btnInvertSelection.Margin = New Padding(2)
        btnInvertSelection.Name = "btnInvertSelection"
        btnInvertSelection.Size = New Size(112, 28)
        btnInvertSelection.TabIndex = 2
        btnInvertSelection.Text = "Invert Selection"
        toolTip.SetToolTip(btnInvertSelection, "Invert the current selection")
        btnInvertSelection.UseVisualStyleBackColor = False
        ' 
        ' txtTableSearch
        ' 
        txtTableSearch.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtTableSearch.Font = New Font("Segoe UI", 10F)
        txtTableSearch.Location = New Point(13, 46)
        txtTableSearch.Margin = New Padding(2)
        txtTableSearch.Name = "txtTableSearch"
        txtTableSearch.Size = New Size(510, 25)
        txtTableSearch.TabIndex = 1
        toolTip.SetToolTip(txtTableSearch, "Search tables by name")
        ' 
        ' lstTables
        ' 
        lstTables.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        lstTables.BackColor = Color.White
        lstTables.BorderStyle = BorderStyle.FixedSingle
        lstTables.CheckOnClick = True
        lstTables.Font = New Font("Segoe UI", 9F)
        lstTables.IntegralHeight = False
        lstTables.Location = New Point(13, 75)
        lstTables.Margin = New Padding(2)
        lstTables.Name = "lstTables"
        lstTables.Size = New Size(509, 427)
        lstTables.TabIndex = 2
        toolTip.SetToolTip(lstTables, "Select tables to include in the script generation")
        ' 
        ' btnGenerate
        ' 
        btnGenerate.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnGenerate.FlatAppearance.BorderSize = 0
        btnGenerate.FlatStyle = FlatStyle.Flat
        btnGenerate.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnGenerate.ForeColor = Color.White
        btnGenerate.Location = New Point(15, 30)
        btnGenerate.Margin = New Padding(2)
        btnGenerate.Name = "btnGenerate"
        btnGenerate.Size = New Size(116, 32)
        btnGenerate.TabIndex = 0
        btnGenerate.Text = "🚀 Generate SQL"
        toolTip.SetToolTip(btnGenerate, "Generate SQL scripts for selected objects")
        btnGenerate.UseVisualStyleBackColor = False
        ' 
        ' btnSelectOutput
        ' 
        btnSelectOutput.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnSelectOutput.FlatAppearance.BorderSize = 0
        btnSelectOutput.FlatStyle = FlatStyle.Flat
        btnSelectOutput.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnSelectOutput.ForeColor = Color.White
        btnSelectOutput.Location = New Point(143, 30)
        btnSelectOutput.Margin = New Padding(2)
        btnSelectOutput.Name = "btnSelectOutput"
        btnSelectOutput.Size = New Size(116, 32)
        btnSelectOutput.TabIndex = 1
        btnSelectOutput.Text = "📁 Select Output"
        toolTip.SetToolTip(btnSelectOutput, "Choose the output directory for SQL scripts")
        btnSelectOutput.UseVisualStyleBackColor = False
        ' 
        ' btnBackupDatabase
        ' 
        btnBackupDatabase.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnBackupDatabase.FlatAppearance.BorderSize = 0
        btnBackupDatabase.FlatStyle = FlatStyle.Flat
        btnBackupDatabase.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnBackupDatabase.ForeColor = Color.White
        btnBackupDatabase.Location = New Point(271, 30)
        btnBackupDatabase.Margin = New Padding(2)
        btnBackupDatabase.Name = "btnBackupDatabase"
        btnBackupDatabase.Size = New Size(116, 32)
        btnBackupDatabase.TabIndex = 2
        btnBackupDatabase.Text = "💾 Backup DB"
        toolTip.SetToolTip(btnBackupDatabase, "Create database backup and zip file with timestamp")
        btnBackupDatabase.UseVisualStyleBackColor = False
        ' 
        ' txtOutput
        ' 
        txtOutput.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtOutput.Font = New Font("Segoe UI", 9F)
        txtOutput.Location = New Point(15, 90)
        txtOutput.Margin = New Padding(2)
        txtOutput.Name = "txtOutput"
        txtOutput.ReadOnly = True
        txtOutput.Size = New Size(371, 23)
        txtOutput.TabIndex = 1
        toolTip.SetToolTip(txtOutput, "Output directory path")
        ' 
        ' chkIncludeData
        ' 
        chkIncludeData.AutoSize = True
        chkIncludeData.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        chkIncludeData.ForeColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        chkIncludeData.Location = New Point(192, 45)
        chkIncludeData.Margin = New Padding(2)
        chkIncludeData.Name = "chkIncludeData"
        chkIncludeData.Size = New Size(144, 19)
        chkIncludeData.TabIndex = 7
        chkIncludeData.Text = "📊 Include Table Data"
        toolTip.SetToolTip(chkIncludeData, "Include INSERT statements for table data (selected tables only)")
        chkIncludeData.UseVisualStyleBackColor = True
        ' 
        ' chkTables
        ' 
        chkTables.AutoSize = True
        chkTables.Checked = True
        chkTables.CheckState = CheckState.Checked
        chkTables.Location = New Point(0, 0)
        chkTables.Margin = New Padding(2)
        chkTables.Name = "chkTables"
        chkTables.Size = New Size(71, 19)
        chkTables.TabIndex = 0
        chkTables.Text = "📋 Tables"
        toolTip.SetToolTip(chkTables, "Include table creation scripts")
        chkTables.UseVisualStyleBackColor = True
        ' 
        ' chkViews
        ' 
        chkViews.AutoSize = True
        chkViews.Location = New Point(96, 0)
        chkViews.Margin = New Padding(2)
        chkViews.Name = "chkViews"
        chkViews.Size = New Size(71, 19)
        chkViews.TabIndex = 1
        chkViews.Text = "👁️ Views"
        toolTip.SetToolTip(chkViews, "Include view creation scripts")
        chkViews.UseVisualStyleBackColor = True
        ' 
        ' chkStoredProcedures
        ' 
        chkStoredProcedures.AutoSize = True
        chkStoredProcedures.Location = New Point(192, 0)
        chkStoredProcedures.Margin = New Padding(2)
        chkStoredProcedures.Name = "chkStoredProcedures"
        chkStoredProcedures.Size = New Size(137, 19)
        chkStoredProcedures.TabIndex = 2
        chkStoredProcedures.Text = "⚡ Stored Procedures"
        toolTip.SetToolTip(chkStoredProcedures, "Include stored procedure scripts")
        chkStoredProcedures.UseVisualStyleBackColor = True
        ' 
        ' chkFunctions
        ' 
        chkFunctions.AutoSize = True
        chkFunctions.Location = New Point(0, 22)
        chkFunctions.Margin = New Padding(2)
        chkFunctions.Name = "chkFunctions"
        chkFunctions.Size = New Size(93, 19)
        chkFunctions.TabIndex = 3
        chkFunctions.Text = "🔧 Functions"
        toolTip.SetToolTip(chkFunctions, "Include user-defined function scripts")
        chkFunctions.UseVisualStyleBackColor = True
        ' 
        ' chkIndexes
        ' 
        chkIndexes.AutoSize = True
        chkIndexes.Location = New Point(96, 22)
        chkIndexes.Margin = New Padding(2)
        chkIndexes.Name = "chkIndexes"
        chkIndexes.Size = New Size(81, 19)
        chkIndexes.TabIndex = 4
        chkIndexes.Text = "🔍 Indexes"
        toolTip.SetToolTip(chkIndexes, "Include index creation scripts")
        chkIndexes.UseVisualStyleBackColor = True
        ' 
        ' chkTriggers
        ' 
        chkTriggers.AutoSize = True
        chkTriggers.Location = New Point(192, 22)
        chkTriggers.Margin = New Padding(2)
        chkTriggers.Name = "chkTriggers"
        chkTriggers.Size = New Size(82, 19)
        chkTriggers.TabIndex = 5
        chkTriggers.Text = "🎯 Triggers"
        toolTip.SetToolTip(chkTriggers, "Include trigger scripts")
        chkTriggers.UseVisualStyleBackColor = True
        ' 
        ' chkPermissions
        ' 
        chkPermissions.AutoSize = True
        chkPermissions.Location = New Point(0, 45)
        chkPermissions.Margin = New Padding(2)
        chkPermissions.Name = "chkPermissions"
        chkPermissions.Size = New Size(104, 19)
        chkPermissions.TabIndex = 6
        chkPermissions.Text = "🔐 Permissions"
        toolTip.SetToolTip(chkPermissions, "Include permission and security scripts")
        chkPermissions.UseVisualStyleBackColor = True
        ' 
        ' btnTestConnection
        ' 
        btnTestConnection.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnTestConnection.FlatAppearance.BorderSize = 0
        btnTestConnection.FlatStyle = FlatStyle.Flat
        btnTestConnection.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnTestConnection.ForeColor = Color.White
        btnTestConnection.Location = New Point(0, 4)
        btnTestConnection.Margin = New Padding(2)
        btnTestConnection.Name = "btnTestConnection"
        btnTestConnection.Size = New Size(114, 26)
        btnTestConnection.TabIndex = 0
        btnTestConnection.Text = "🔌 Connect"
        toolTip.SetToolTip(btnTestConnection, "Test the database connection with current settings")
        btnTestConnection.UseVisualStyleBackColor = False
        ' 
        ' btnLoadTables
        ' 
        btnLoadTables.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnLoadTables.FlatAppearance.BorderSize = 0
        btnLoadTables.FlatStyle = FlatStyle.Flat
        btnLoadTables.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnLoadTables.ForeColor = Color.White
        btnLoadTables.Location = New Point(129, 4)
        btnLoadTables.Margin = New Padding(2)
        btnLoadTables.Name = "btnLoadTables"
        btnLoadTables.Size = New Size(114, 26)
        btnLoadTables.TabIndex = 1
        btnLoadTables.Text = "📋 Load Tables"
        toolTip.SetToolTip(btnLoadTables, "Load tables from the database")
        btnLoadTables.UseVisualStyleBackColor = False
        ' 
        ' btnSaveProfile
        ' 
        btnSaveProfile.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnSaveProfile.FlatAppearance.BorderSize = 0
        btnSaveProfile.FlatStyle = FlatStyle.Flat
        btnSaveProfile.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnSaveProfile.ForeColor = Color.White
        btnSaveProfile.Location = New Point(258, 4)
        btnSaveProfile.Margin = New Padding(2)
        btnSaveProfile.Name = "btnSaveProfile"
        btnSaveProfile.Size = New Size(114, 26)
        btnSaveProfile.TabIndex = 2
        btnSaveProfile.Text = "💾 Save Profile"
        toolTip.SetToolTip(btnSaveProfile, "Save current connection settings as profile")
        btnSaveProfile.UseVisualStyleBackColor = False
        ' 
        ' txtServer
        ' 
        txtServer.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtServer.Font = New Font("Segoe UI", 10F)
        txtServer.Location = New Point(98, 5)
        txtServer.Margin = New Padding(2, 5, 2, 2)
        txtServer.Name = "txtServer"
        txtServer.Size = New Size(274, 25)
        txtServer.TabIndex = 1
        toolTip.SetToolTip(txtServer, "Enter the SQL Server instance name or IP address")
        ' 
        ' txtDatabase
        ' 
        txtDatabase.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtDatabase.Font = New Font("Segoe UI", 10F)
        txtDatabase.Location = New Point(98, 37)
        txtDatabase.Margin = New Padding(2, 5, 2, 2)
        txtDatabase.Name = "txtDatabase"
        txtDatabase.Size = New Size(274, 25)
        txtDatabase.TabIndex = 3
        toolTip.SetToolTip(txtDatabase, "Enter the database name to script")
        ' 
        ' txtUsername
        ' 
        txtUsername.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtUsername.Font = New Font("Segoe UI", 10F)
        txtUsername.Location = New Point(98, 69)
        txtUsername.Margin = New Padding(2, 5, 2, 2)
        txtUsername.Name = "txtUsername"
        txtUsername.Size = New Size(274, 25)
        txtUsername.TabIndex = 5
        toolTip.SetToolTip(txtUsername, "Enter the SQL Server username")
        ' 
        ' txtPassword
        ' 
        txtPassword.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtPassword.Font = New Font("Segoe UI", 10F)
        txtPassword.Location = New Point(98, 101)
        txtPassword.Margin = New Padding(2, 5, 2, 2)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordChar = "•"c
        txtPassword.Size = New Size(274, 25)
        txtPassword.TabIndex = 7
        toolTip.SetToolTip(txtPassword, "Enter the SQL Server password")
        ' 
        ' chkIntegratedSecurity
        ' 
        chkIntegratedSecurity.AutoSize = True
        chkIntegratedSecurity.Dock = DockStyle.Fill
        chkIntegratedSecurity.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        chkIntegratedSecurity.ForeColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        chkIntegratedSecurity.Location = New Point(98, 130)
        chkIntegratedSecurity.Margin = New Padding(2)
        chkIntegratedSecurity.Name = "chkIntegratedSecurity"
        chkIntegratedSecurity.Size = New Size(274, 28)
        chkIntegratedSecurity.TabIndex = 8
        chkIntegratedSecurity.Text = "🔐 Use Windows Authentication"
        toolTip.SetToolTip(chkIntegratedSecurity, "Use Windows Authentication instead of SQL Server Authentication")
        chkIntegratedSecurity.UseVisualStyleBackColor = True
        ' 
        ' menuStrip
        ' 
        menuStrip.BackColor = Color.White
        menuStrip.Font = New Font("Segoe UI", 9F)
        menuStrip.ImageScalingSize = New Size(20, 20)
        menuStrip.Items.AddRange(New ToolStripItem() {fileToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem})
        menuStrip.Location = New Point(0, 0)
        menuStrip.Name = "menuStrip"
        menuStrip.Size = New Size(967, 24)
        menuStrip.TabIndex = 0
        ' 
        ' fileToolStripMenuItem
        ' 
        fileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {newProfileToolStripMenuItem, loadProfileToolStripMenuItem, saveProfileToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem})
        fileToolStripMenuItem.Name = "fileToolStripMenuItem"
        fileToolStripMenuItem.Size = New Size(37, 20)
        fileToolStripMenuItem.Text = "&File"
        ' 
        ' newProfileToolStripMenuItem
        ' 
        newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem"
        newProfileToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.N
        newProfileToolStripMenuItem.Size = New Size(200, 22)
        newProfileToolStripMenuItem.Text = "&New Profile"
        ' 
        ' loadProfileToolStripMenuItem
        ' 
        loadProfileToolStripMenuItem.Name = "loadProfileToolStripMenuItem"
        loadProfileToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.O
        loadProfileToolStripMenuItem.Size = New Size(200, 22)
        loadProfileToolStripMenuItem.Text = "&Load Profile..."
        ' 
        ' saveProfileToolStripMenuItem
        ' 
        saveProfileToolStripMenuItem.Name = "saveProfileToolStripMenuItem"
        saveProfileToolStripMenuItem.ShortcutKeys = Keys.Control Or Keys.S
        saveProfileToolStripMenuItem.Size = New Size(200, 22)
        saveProfileToolStripMenuItem.Text = "&Save Profile As..."
        ' 
        ' toolStripSeparator1
        ' 
        toolStripSeparator1.Name = "toolStripSeparator1"
        toolStripSeparator1.Size = New Size(197, 6)
        ' 
        ' exitToolStripMenuItem
        ' 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        exitToolStripMenuItem.ShortcutKeys = Keys.Alt Or Keys.F4
        exitToolStripMenuItem.Size = New Size(200, 22)
        exitToolStripMenuItem.Text = "E&xit"
        ' 
        ' toolsToolStripMenuItem
        ' 
        toolsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {settingsToolStripMenuItem})
        toolsToolStripMenuItem.Name = "toolsToolStripMenuItem"
        toolsToolStripMenuItem.Size = New Size(46, 20)
        toolsToolStripMenuItem.Text = "&Tools"
        ' 
        ' settingsToolStripMenuItem
        ' 
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem"
        settingsToolStripMenuItem.Size = New Size(125, 22)
        settingsToolStripMenuItem.Text = "&Settings..."
        ' 
        ' helpToolStripMenuItem
        ' 
        helpToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {aboutToolStripMenuItem})
        helpToolStripMenuItem.Name = "helpToolStripMenuItem"
        helpToolStripMenuItem.Size = New Size(44, 20)
        helpToolStripMenuItem.Text = "&Help"
        ' 
        ' aboutToolStripMenuItem
        ' 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem"
        aboutToolStripMenuItem.Size = New Size(116, 22)
        aboutToolStripMenuItem.Text = "&About..."
        ' 
        ' statusStrip
        ' 
        statusStrip.BackColor = Color.FromArgb(CByte(240), CByte(240), CByte(240))
        statusStrip.Font = New Font("Segoe UI", 9F)
        statusStrip.ImageScalingSize = New Size(20, 20)
        statusStrip.Items.AddRange(New ToolStripItem() {toolStripStatusLabel, toolStripProgressBar, toolStripSpring, toolStripConnectionStatus})
        statusStrip.Location = New Point(0, 597)
        statusStrip.Name = "statusStrip"
        statusStrip.Padding = New Padding(1, 0, 15, 0)
        statusStrip.Size = New Size(967, 22)
        statusStrip.TabIndex = 1
        ' 
        ' toolStripStatusLabel
        ' 
        toolStripStatusLabel.Name = "toolStripStatusLabel"
        toolStripStatusLabel.Size = New Size(39, 17)
        toolStripStatusLabel.Text = "Ready"
        ' 
        ' toolStripProgressBar
        ' 
        toolStripProgressBar.Name = "toolStripProgressBar"
        toolStripProgressBar.Size = New Size(120, 16)
        toolStripProgressBar.Style = ProgressBarStyle.Continuous
        toolStripProgressBar.Visible = False
        ' 
        ' toolStripSpring
        ' 
        toolStripSpring.Name = "toolStripSpring"
        toolStripSpring.Size = New Size(833, 17)
        toolStripSpring.Spring = True
        ' 
        ' toolStripConnectionStatus
        ' 
        toolStripConnectionStatus.Name = "toolStripConnectionStatus"
        toolStripConnectionStatus.Size = New Size(79, 17)
        toolStripConnectionStatus.Text = "Disconnected"
        ' 
        ' pnlMain
        ' 
        pnlMain.Controls.Add(pnlRight)
        pnlMain.Controls.Add(pnlLeft)
        pnlMain.Dock = DockStyle.Fill
        pnlMain.Location = New Point(0, 24)
        pnlMain.Margin = New Padding(2)
        pnlMain.Name = "pnlMain"
        pnlMain.Padding = New Padding(10)
        pnlMain.Size = New Size(967, 573)
        pnlMain.TabIndex = 2
        ' 
        ' pnlRight
        ' 
        pnlRight.Controls.Add(grpTables)
        pnlRight.Location = New Point(422, 10)
        pnlRight.Margin = New Padding(2)
        pnlRight.Name = "pnlRight"
        pnlRight.Size = New Size(534, 555)
        pnlRight.TabIndex = 2
        ' 
        ' grpTables
        ' 
        grpTables.Controls.Add(pnlTableActions)
        grpTables.Controls.Add(txtTableSearch)
        grpTables.Controls.Add(lblTableSearch)
        grpTables.Controls.Add(lstTables)
        grpTables.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        grpTables.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        grpTables.Location = New Point(0, 0)
        grpTables.Margin = New Padding(2)
        grpTables.Name = "grpTables"
        grpTables.Padding = New Padding(13, 10, 13, 13)
        grpTables.Size = New Size(534, 555)
        grpTables.TabIndex = 0
        grpTables.TabStop = False
        grpTables.Text = "📋 Database Tables & Objects"
        ' 
        ' pnlTableActions
        ' 
        pnlTableActions.Controls.Add(btnSelectAll)
        pnlTableActions.Controls.Add(btnSelectNone)
        pnlTableActions.Controls.Add(btnInvertSelection)
        pnlTableActions.Controls.Add(lblTableCount)
        pnlTableActions.Font = New Font("Segoe UI", 9F)
        pnlTableActions.Location = New Point(13, 509)
        pnlTableActions.Margin = New Padding(2)
        pnlTableActions.Name = "pnlTableActions"
        pnlTableActions.Size = New Size(508, 44)
        pnlTableActions.TabIndex = 3
        ' 
        ' lblTableCount
        ' 
        lblTableCount.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblTableCount.Font = New Font("Segoe UI", 9F, FontStyle.Italic)
        lblTableCount.ForeColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        lblTableCount.Location = New Point(385, 17)
        lblTableCount.Margin = New Padding(2, 0, 2, 0)
        lblTableCount.Name = "lblTableCount"
        lblTableCount.Size = New Size(120, 16)
        lblTableCount.TabIndex = 3
        lblTableCount.Text = "0 tables loaded"
        lblTableCount.TextAlign = ContentAlignment.TopRight
        ' 
        ' lblTableSearch
        ' 
        lblTableSearch.AutoSize = True
        lblTableSearch.Font = New Font("Segoe UI", 9F)
        lblTableSearch.ForeColor = Color.FromArgb(CByte(73), CByte(80), CByte(87))
        lblTableSearch.Location = New Point(13, 28)
        lblTableSearch.Margin = New Padding(2, 0, 2, 0)
        lblTableSearch.Name = "lblTableSearch"
        lblTableSearch.Size = New Size(95, 15)
        lblTableSearch.TabIndex = 0
        lblTableSearch.Text = "🔍 Search Tables:"
        ' 
        ' pnlLeft
        ' 
        pnlLeft.Controls.Add(grpGeneration)
        pnlLeft.Controls.Add(grpOptions)
        pnlLeft.Controls.Add(grpConnection)
        pnlLeft.Location = New Point(10, 10)
        pnlLeft.Margin = New Padding(2)
        pnlLeft.Name = "pnlLeft"
        pnlLeft.Size = New Size(400, 554)
        pnlLeft.TabIndex = 0
        ' 
        ' grpGeneration
        ' 
        grpGeneration.Controls.Add(lblProgress)
        grpGeneration.Controls.Add(btnGenerate)
        grpGeneration.Controls.Add(btnSelectOutput)
        grpGeneration.Controls.Add(pnlProgress)
        grpGeneration.Controls.Add(btnBackupDatabase)
        grpGeneration.Controls.Add(pnlGenerationActions)
        grpGeneration.Controls.Add(txtOutput)
        grpGeneration.Controls.Add(lblOutput)
        grpGeneration.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        grpGeneration.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        grpGeneration.Location = New Point(0, 389)
        grpGeneration.Margin = New Padding(2)
        grpGeneration.Name = "grpGeneration"
        grpGeneration.Padding = New Padding(13, 10, 13, 13)
        grpGeneration.Size = New Size(400, 166)
        grpGeneration.TabIndex = 2
        grpGeneration.TabStop = False
        grpGeneration.Text = "⚡ Script Generation"
        ' 
        ' lblProgress
        ' 
        lblProgress.Font = New Font("Segoe UI", 9F, FontStyle.Italic)
        lblProgress.ForeColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        lblProgress.Location = New Point(12, 118)
        lblProgress.Margin = New Padding(2, 0, 2, 0)
        lblProgress.Name = "lblProgress"
        lblProgress.Size = New Size(374, 15)
        lblProgress.TabIndex = 4
        lblProgress.Text = "Generating scripts..."
        ' 
        ' pnlProgress
        ' 
        pnlProgress.Controls.Add(progressBar)
        pnlProgress.Font = New Font("Segoe UI", 9F)
        pnlProgress.Location = New Point(15, 137)
        pnlProgress.Margin = New Padding(2)
        pnlProgress.Name = "pnlProgress"
        pnlProgress.Size = New Size(370, 15)
        pnlProgress.TabIndex = 3
        pnlProgress.Visible = False
        ' 
        ' progressBar
        ' 
        progressBar.Location = New Point(0, 0)
        progressBar.Margin = New Padding(2)
        progressBar.Name = "progressBar"
        progressBar.Size = New Size(373, 13)
        progressBar.Style = ProgressBarStyle.Continuous
        progressBar.TabIndex = 1
        ' 
        ' pnlGenerationActions
        ' 
        pnlGenerationActions.Font = New Font("Segoe UI", 9F)
        pnlGenerationActions.Location = New Point(15, 29)
        pnlGenerationActions.Margin = New Padding(2)
        pnlGenerationActions.Name = "pnlGenerationActions"
        pnlGenerationActions.Size = New Size(374, 32)
        pnlGenerationActions.TabIndex = 2
        ' 
        ' lblOutput
        ' 
        lblOutput.AutoSize = True
        lblOutput.Font = New Font("Segoe UI", 9F)
        lblOutput.ForeColor = Color.FromArgb(CByte(73), CByte(80), CByte(87))
        lblOutput.Location = New Point(12, 69)
        lblOutput.Margin = New Padding(2, 0, 2, 0)
        lblOutput.Name = "lblOutput"
        lblOutput.Size = New Size(97, 15)
        lblOutput.TabIndex = 0
        lblOutput.Text = "Output Location:"
        ' 
        ' grpOptions
        ' 
        grpOptions.Controls.Add(pnlOptions)
        grpOptions.Dock = DockStyle.Top
        grpOptions.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        grpOptions.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        grpOptions.Location = New Point(0, 278)
        grpOptions.Margin = New Padding(2)
        grpOptions.Name = "grpOptions"
        grpOptions.Padding = New Padding(13, 10, 13, 13)
        grpOptions.Size = New Size(400, 111)
        grpOptions.TabIndex = 1
        grpOptions.TabStop = False
        grpOptions.Text = "⚙️ Script Generation Options"
        ' 
        ' pnlOptions
        ' 
        pnlOptions.Controls.Add(chkIncludeData)
        pnlOptions.Controls.Add(chkTables)
        pnlOptions.Controls.Add(chkViews)
        pnlOptions.Controls.Add(chkStoredProcedures)
        pnlOptions.Controls.Add(chkFunctions)
        pnlOptions.Controls.Add(chkIndexes)
        pnlOptions.Controls.Add(chkTriggers)
        pnlOptions.Controls.Add(chkPermissions)
        pnlOptions.Dock = DockStyle.Fill
        pnlOptions.Font = New Font("Segoe UI", 9F)
        pnlOptions.Location = New Point(13, 28)
        pnlOptions.Margin = New Padding(2)
        pnlOptions.Name = "pnlOptions"
        pnlOptions.Size = New Size(374, 70)
        pnlOptions.TabIndex = 0
        ' 
        ' grpConnection
        ' 
        grpConnection.Controls.Add(pnlConnectionStatus)
        grpConnection.Controls.Add(pnlConnectionActions)
        grpConnection.Controls.Add(tblConnection)
        grpConnection.Dock = DockStyle.Top
        grpConnection.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        grpConnection.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        grpConnection.Location = New Point(0, 0)
        grpConnection.Margin = New Padding(2)
        grpConnection.Name = "grpConnection"
        grpConnection.Padding = New Padding(13, 10, 13, 13)
        grpConnection.Size = New Size(400, 278)
        grpConnection.TabIndex = 0
        grpConnection.TabStop = False
        grpConnection.Text = "🔗 Database Connection"
        ' 
        ' pnlConnectionStatus
        ' 
        pnlConnectionStatus.Controls.Add(lblConnectionStatus)
        pnlConnectionStatus.Controls.Add(picConnectionStatus)
        pnlConnectionStatus.Dock = DockStyle.Bottom
        pnlConnectionStatus.Font = New Font("Segoe UI", 9F)
        pnlConnectionStatus.Location = New Point(13, 207)
        pnlConnectionStatus.Margin = New Padding(2)
        pnlConnectionStatus.Name = "pnlConnectionStatus"
        pnlConnectionStatus.Size = New Size(374, 26)
        pnlConnectionStatus.TabIndex = 2
        ' 
        ' lblConnectionStatus
        ' 
        lblConnectionStatus.AutoSize = True
        lblConnectionStatus.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        lblConnectionStatus.ForeColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        lblConnectionStatus.Location = New Point(19, 5)
        lblConnectionStatus.Margin = New Padding(2, 0, 2, 0)
        lblConnectionStatus.Name = "lblConnectionStatus"
        lblConnectionStatus.Size = New Size(83, 15)
        lblConnectionStatus.TabIndex = 1
        lblConnectionStatus.Text = "Disconnected"
        ' 
        ' picConnectionStatus
        ' 
        picConnectionStatus.Location = New Point(0, 5)
        picConnectionStatus.Margin = New Padding(2)
        picConnectionStatus.Name = "picConnectionStatus"
        picConnectionStatus.Size = New Size(16, 16)
        picConnectionStatus.SizeMode = PictureBoxSizeMode.StretchImage
        picConnectionStatus.TabIndex = 0
        picConnectionStatus.TabStop = False
        ' 
        ' pnlConnectionActions
        ' 
        pnlConnectionActions.Controls.Add(btnTestConnection)
        pnlConnectionActions.Controls.Add(btnLoadTables)
        pnlConnectionActions.Controls.Add(btnSaveProfile)
        pnlConnectionActions.Dock = DockStyle.Bottom
        pnlConnectionActions.Font = New Font("Segoe UI", 9F)
        pnlConnectionActions.Location = New Point(13, 233)
        pnlConnectionActions.Margin = New Padding(2)
        pnlConnectionActions.Name = "pnlConnectionActions"
        pnlConnectionActions.Size = New Size(374, 32)
        pnlConnectionActions.TabIndex = 3
        ' 
        ' tblConnection
        ' 
        tblConnection.ColumnCount = 2
        tblConnection.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 96F))
        tblConnection.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        tblConnection.Controls.Add(lblServer, 0, 0)
        tblConnection.Controls.Add(txtServer, 1, 0)
        tblConnection.Controls.Add(lblDatabase, 0, 1)
        tblConnection.Controls.Add(txtDatabase, 1, 1)
        tblConnection.Controls.Add(lblUsername, 0, 2)
        tblConnection.Controls.Add(txtUsername, 1, 2)
        tblConnection.Controls.Add(lblPassword, 0, 3)
        tblConnection.Controls.Add(txtPassword, 1, 3)
        tblConnection.Controls.Add(chkIntegratedSecurity, 1, 4)
        tblConnection.Dock = DockStyle.Fill
        tblConnection.Font = New Font("Segoe UI", 9F)
        tblConnection.Location = New Point(13, 28)
        tblConnection.Margin = New Padding(2)
        tblConnection.Name = "tblConnection"
        tblConnection.RowCount = 6
        tblConnection.RowStyles.Add(New RowStyle(SizeType.Absolute, 32F))
        tblConnection.RowStyles.Add(New RowStyle(SizeType.Absolute, 32F))
        tblConnection.RowStyles.Add(New RowStyle(SizeType.Absolute, 32F))
        tblConnection.RowStyles.Add(New RowStyle(SizeType.Absolute, 32F))
        tblConnection.RowStyles.Add(New RowStyle(SizeType.Absolute, 32F))
        tblConnection.RowStyles.Add(New RowStyle(SizeType.Percent, 100F))
        tblConnection.Size = New Size(374, 237)
        tblConnection.TabIndex = 1
        ' 
        ' lblServer
        ' 
        lblServer.AutoSize = True
        lblServer.Dock = DockStyle.Fill
        lblServer.ForeColor = Color.FromArgb(CByte(73), CByte(80), CByte(87))
        lblServer.Location = New Point(2, 0)
        lblServer.Margin = New Padding(2, 0, 2, 0)
        lblServer.Name = "lblServer"
        lblServer.Size = New Size(92, 32)
        lblServer.TabIndex = 0
        lblServer.Text = "Server:"
        lblServer.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' lblDatabase
        ' 
        lblDatabase.AutoSize = True
        lblDatabase.Dock = DockStyle.Fill
        lblDatabase.ForeColor = Color.FromArgb(CByte(73), CByte(80), CByte(87))
        lblDatabase.Location = New Point(2, 32)
        lblDatabase.Margin = New Padding(2, 0, 2, 0)
        lblDatabase.Name = "lblDatabase"
        lblDatabase.Size = New Size(92, 32)
        lblDatabase.TabIndex = 2
        lblDatabase.Text = "Database:"
        lblDatabase.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' lblUsername
        ' 
        lblUsername.AutoSize = True
        lblUsername.Dock = DockStyle.Fill
        lblUsername.ForeColor = Color.FromArgb(CByte(73), CByte(80), CByte(87))
        lblUsername.Location = New Point(2, 64)
        lblUsername.Margin = New Padding(2, 0, 2, 0)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New Size(92, 32)
        lblUsername.TabIndex = 4
        lblUsername.Text = "Username:"
        lblUsername.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' lblPassword
        ' 
        lblPassword.AutoSize = True
        lblPassword.Dock = DockStyle.Fill
        lblPassword.ForeColor = Color.FromArgb(CByte(73), CByte(80), CByte(87))
        lblPassword.Location = New Point(2, 96)
        lblPassword.Margin = New Padding(2, 0, 2, 0)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(92, 32)
        lblPassword.TabIndex = 6
        lblPassword.Text = "Password:"
        lblPassword.TextAlign = ContentAlignment.MiddleLeft
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(96F, 96F)
        AutoScaleMode = AutoScaleMode.Dpi
        BackColor = Color.White
        ClientSize = New Size(967, 619)
        Controls.Add(pnlMain)
        Controls.Add(statusStrip)
        Controls.Add(menuStrip)
        Font = New Font("Segoe UI", 9F)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MainMenuStrip = menuStrip
        Margin = New Padding(2)
        MaximizeBox = False
        MinimumSize = New Size(803, 568)
        Name = "frmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "RepoSQL - SQL Script Generator"
        menuStrip.ResumeLayout(False)
        menuStrip.PerformLayout()
        statusStrip.ResumeLayout(False)
        statusStrip.PerformLayout()
        pnlMain.ResumeLayout(False)
        pnlRight.ResumeLayout(False)
        grpTables.ResumeLayout(False)
        grpTables.PerformLayout()
        pnlTableActions.ResumeLayout(False)
        pnlLeft.ResumeLayout(False)
        grpGeneration.ResumeLayout(False)
        grpGeneration.PerformLayout()
        pnlProgress.ResumeLayout(False)
        grpOptions.ResumeLayout(False)
        pnlOptions.ResumeLayout(False)
        pnlOptions.PerformLayout()
        grpConnection.ResumeLayout(False)
        pnlConnectionStatus.ResumeLayout(False)
        pnlConnectionStatus.PerformLayout()
        CType(picConnectionStatus, ComponentModel.ISupportInitialize).EndInit()
        pnlConnectionActions.ResumeLayout(False)
        tblConnection.ResumeLayout(False)
        tblConnection.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents toolTip As System.Windows.Forms.ToolTip
    Friend WithEvents menuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents newProfileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents loadProfileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents saveProfileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents toolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents settingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents helpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents aboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents statusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents toolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents toolStripProgressBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents toolStripSpring As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents toolStripConnectionStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents pnlRight As System.Windows.Forms.Panel
    Friend WithEvents grpTables As System.Windows.Forms.GroupBox
    Friend WithEvents pnlTableActions As System.Windows.Forms.Panel
    Friend WithEvents btnSelectAll As System.Windows.Forms.Button
    Friend WithEvents btnSelectNone As System.Windows.Forms.Button
    Friend WithEvents btnInvertSelection As System.Windows.Forms.Button
    Friend WithEvents lblTableCount As System.Windows.Forms.Label
    Friend WithEvents txtTableSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblTableSearch As System.Windows.Forms.Label
    Friend WithEvents lstTables As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlLeft As System.Windows.Forms.Panel
    Friend WithEvents grpGeneration As System.Windows.Forms.GroupBox
    Friend WithEvents pnlProgress As System.Windows.Forms.Panel
    Friend WithEvents progressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents pnlGenerationActions As System.Windows.Forms.Panel
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents btnSelectOutput As System.Windows.Forms.Button
    Friend WithEvents btnBackupDatabase As System.Windows.Forms.Button
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents lblOutput As System.Windows.Forms.Label
    Friend WithEvents grpOptions As System.Windows.Forms.GroupBox
    Friend WithEvents pnlOptions As System.Windows.Forms.Panel
    Friend WithEvents chkIncludeData As System.Windows.Forms.CheckBox
    Friend WithEvents chkTables As System.Windows.Forms.CheckBox
    Friend WithEvents chkViews As System.Windows.Forms.CheckBox
    Friend WithEvents chkStoredProcedures As System.Windows.Forms.CheckBox
    Friend WithEvents chkFunctions As System.Windows.Forms.CheckBox
    Friend WithEvents chkIndexes As System.Windows.Forms.CheckBox
    Friend WithEvents chkTriggers As System.Windows.Forms.CheckBox
    Friend WithEvents chkPermissions As System.Windows.Forms.CheckBox
    Friend WithEvents grpConnection As System.Windows.Forms.GroupBox
    Friend WithEvents pnlConnectionStatus As System.Windows.Forms.Panel
    Friend WithEvents lblConnectionStatus As System.Windows.Forms.Label
    Friend WithEvents picConnectionStatus As System.Windows.Forms.PictureBox
    Friend WithEvents pnlConnectionActions As System.Windows.Forms.Panel
    Friend WithEvents btnTestConnection As System.Windows.Forms.Button
    Friend WithEvents btnLoadTables As System.Windows.Forms.Button
    Friend WithEvents btnSaveProfile As System.Windows.Forms.Button
    Friend WithEvents tblConnection As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblServer As System.Windows.Forms.Label
    Friend WithEvents txtServer As System.Windows.Forms.TextBox
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblPassword As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents chkIntegratedSecurity As System.Windows.Forms.CheckBox
    Friend WithEvents lblProgress As Label
End Class