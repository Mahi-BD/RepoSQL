<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSetting
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
        Me.components = New System.ComponentModel.Container()
        Me.toolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.grpScriptGeneration = New System.Windows.Forms.GroupBox()
        Me.pnlScriptOptions = New System.Windows.Forms.Panel()
        Me.chkGenerateDropStatements = New System.Windows.Forms.CheckBox()
        Me.lblDropStatementsDesc = New System.Windows.Forms.Label()
        Me.grpOutputSettings = New System.Windows.Forms.GroupBox()
        Me.pnlOutputOptions = New System.Windows.Forms.Panel()
        Me.chkOverwriteExistingFiles = New System.Windows.Forms.CheckBox()
        Me.lblOverwriteDesc = New System.Windows.Forms.Label()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.picIcon = New System.Windows.Forms.PictureBox()
        Me.pnlMain.SuspendLayout()
        Me.grpScriptGeneration.SuspendLayout()
        Me.pnlScriptOptions.SuspendLayout()
        Me.grpOutputSettings.SuspendLayout()
        Me.pnlOutputOptions.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.grpScriptGeneration)
        Me.pnlMain.Controls.Add(Me.grpOutputSettings)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 80)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Padding = New System.Windows.Forms.Padding(20)
        Me.pnlMain.Size = New System.Drawing.Size(500, 280)
        Me.pnlMain.TabIndex = 1
        '
        'grpScriptGeneration
        '
        Me.grpScriptGeneration.Controls.Add(Me.pnlScriptOptions)
        Me.grpScriptGeneration.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpScriptGeneration.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.grpScriptGeneration.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.grpScriptGeneration.Location = New System.Drawing.Point(20, 20)
        Me.grpScriptGeneration.Name = "grpScriptGeneration"
        Me.grpScriptGeneration.Padding = New System.Windows.Forms.Padding(15, 10, 15, 15)
        Me.grpScriptGeneration.Size = New System.Drawing.Size(460, 100)
        Me.grpScriptGeneration.TabIndex = 0
        Me.grpScriptGeneration.TabStop = False
        Me.grpScriptGeneration.Text = "🔧 Script Generation Options"
        '
        'pnlScriptOptions
        '
        Me.pnlScriptOptions.Controls.Add(Me.chkGenerateDropStatements)
        Me.pnlScriptOptions.Controls.Add(Me.lblDropStatementsDesc)
        Me.pnlScriptOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlScriptOptions.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.pnlScriptOptions.Location = New System.Drawing.Point(15, 28)
        Me.pnlScriptOptions.Name = "pnlScriptOptions"
        Me.pnlScriptOptions.Size = New System.Drawing.Size(430, 57)
        Me.pnlScriptOptions.TabIndex = 0
        '
        'chkGenerateDropStatements
        '
        Me.chkGenerateDropStatements.AutoSize = True
        Me.chkGenerateDropStatements.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkGenerateDropStatements.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.chkGenerateDropStatements.Location = New System.Drawing.Point(10, 10)
        Me.chkGenerateDropStatements.Name = "chkGenerateDropStatements"
        Me.chkGenerateDropStatements.Size = New System.Drawing.Size(340, 19)
        Me.chkGenerateDropStatements.TabIndex = 0
        Me.chkGenerateDropStatements.Text = "🗑️ Generate IF EXISTS DROP statements before CREATE"
        Me.toolTip.SetToolTip(Me.chkGenerateDropStatements, "When checked, adds IF EXISTS DROP TABLE/VIEW/PROCEDURE statements before CREATE statements")
        Me.chkGenerateDropStatements.UseVisualStyleBackColor = True
        '
        'lblDropStatementsDesc
        '
        Me.lblDropStatementsDesc.AutoSize = True
        Me.lblDropStatementsDesc.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblDropStatementsDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblDropStatementsDesc.Location = New System.Drawing.Point(25, 32)
        Me.lblDropStatementsDesc.Name = "lblDropStatementsDesc"
        Me.lblDropStatementsDesc.Size = New System.Drawing.Size(350, 13)
        Me.lblDropStatementsDesc.TabIndex = 1
        Me.lblDropStatementsDesc.Text = "Adds safety checks to drop existing objects before creating new ones"
        '
        'grpOutputSettings
        '
        Me.grpOutputSettings.Controls.Add(Me.pnlOutputOptions)
        Me.grpOutputSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.grpOutputSettings.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.grpOutputSettings.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.grpOutputSettings.Location = New System.Drawing.Point(20, 120)
        Me.grpOutputSettings.Name = "grpOutputSettings"
        Me.grpOutputSettings.Padding = New System.Windows.Forms.Padding(15, 10, 15, 15)
        Me.grpOutputSettings.Size = New System.Drawing.Size(460, 100)
        Me.grpOutputSettings.TabIndex = 1
        Me.grpOutputSettings.TabStop = False
        Me.grpOutputSettings.Text = "📁 Output File Options"
        '
        'pnlOutputOptions
        '
        Me.pnlOutputOptions.Controls.Add(Me.chkOverwriteExistingFiles)
        Me.pnlOutputOptions.Controls.Add(Me.lblOverwriteDesc)
        Me.pnlOutputOptions.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlOutputOptions.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.pnlOutputOptions.Location = New System.Drawing.Point(15, 28)
        Me.pnlOutputOptions.Name = "pnlOutputOptions"
        Me.pnlOutputOptions.Size = New System.Drawing.Size(430, 57)
        Me.pnlOutputOptions.TabIndex = 0
        '
        'chkOverwriteExistingFiles
        '
        Me.chkOverwriteExistingFiles.AutoSize = True
        Me.chkOverwriteExistingFiles.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.chkOverwriteExistingFiles.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.chkOverwriteExistingFiles.Location = New System.Drawing.Point(10, 10)
        Me.chkOverwriteExistingFiles.Name = "chkOverwriteExistingFiles"
        Me.chkOverwriteExistingFiles.Size = New System.Drawing.Size(280, 19)
        Me.chkOverwriteExistingFiles.TabIndex = 0
        Me.chkOverwriteExistingFiles.Text = "💾 Overwrite existing SQL files without prompt"
        Me.toolTip.SetToolTip(Me.chkOverwriteExistingFiles, "When checked, overwrites existing SQL files without asking for confirmation")
        Me.chkOverwriteExistingFiles.UseVisualStyleBackColor = True
        '
        'lblOverwriteDesc
        '
        Me.lblOverwriteDesc.AutoSize = True
        Me.lblOverwriteDesc.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblOverwriteDesc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblOverwriteDesc.Location = New System.Drawing.Point(25, 32)
        Me.lblOverwriteDesc.Name = "lblOverwriteDesc"
        Me.lblOverwriteDesc.Size = New System.Drawing.Size(300, 13)
        Me.lblOverwriteDesc.TabIndex = 1
        Me.lblOverwriteDesc.Text = "Automatically replaces existing files in the output directory"
        '
        'pnlButtons
        '
        Me.pnlButtons.Controls.Add(Me.btnCancel)
        Me.pnlButtons.Controls.Add(Me.btnOK)
        Me.pnlButtons.Controls.Add(Me.btnApply)
        Me.pnlButtons.Controls.Add(Me.btnReset)
        Me.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtons.Location = New System.Drawing.Point(0, 360)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Padding = New System.Windows.Forms.Padding(20, 10, 20, 20)
        Me.pnlButtons.Size = New System.Drawing.Size(500, 60)
        Me.pnlButtons.TabIndex = 2
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(400, 15)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 30)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "Cancel"
        Me.toolTip.SetToolTip(Me.btnCancel, "Cancel and close without saving changes")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.FlatAppearance.BorderSize = 0
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnOK.ForeColor = System.Drawing.Color.White
        Me.btnOK.Location = New System.Drawing.Point(310, 15)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 30)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.toolTip.SetToolTip(Me.btnOK, "Save settings and close")
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.btnApply.FlatAppearance.BorderSize = 0
        Me.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApply.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnApply.ForeColor = System.Drawing.Color.White
        Me.btnApply.Location = New System.Drawing.Point(220, 15)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(80, 30)
        Me.btnApply.TabIndex = 1
        Me.btnApply.Text = "Apply"
        Me.toolTip.SetToolTip(Me.btnApply, "Apply settings without closing")
        Me.btnApply.UseVisualStyleBackColor = False
        '
        'btnReset
        '
        Me.btnReset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReset.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.btnReset.FlatAppearance.BorderSize = 0
        Me.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReset.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnReset.ForeColor = System.Drawing.Color.White
        Me.btnReset.Location = New System.Drawing.Point(20, 15)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(80, 30)
        Me.btnReset.TabIndex = 0
        Me.btnReset.Text = "Reset"
        Me.toolTip.SetToolTip(Me.btnReset, "Reset settings to default values")
        Me.btnReset.UseVisualStyleBackColor = False
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.White
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Controls.Add(Me.lblDescription)
        Me.pnlHeader.Controls.Add(Me.picIcon)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Padding = New System.Windows.Forms.Padding(20, 15, 20, 15)
        Me.pnlHeader.Size = New System.Drawing.Size(500, 80)
        Me.pnlHeader.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(60, 15)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(169, 21)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "RepoSQL Settings"
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblDescription.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblDescription.Location = New System.Drawing.Point(60, 40)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(280, 15)
        Me.lblDescription.TabIndex = 2
        Me.lblDescription.Text = "Configure script generation and output file options"
        '
        'picIcon
        '
        Me.picIcon.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(215, Byte), Integer))
        Me.picIcon.Location = New System.Drawing.Point(20, 20)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(32, 32)
        Me.picIcon.TabIndex = 0
        Me.picIcon.TabStop = False
        '
        'frmSetting
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(500, 420)
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.pnlButtons)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSetting"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "RepoSQL Settings"
        Me.pnlMain.ResumeLayout(False)
        Me.grpScriptGeneration.ResumeLayout(False)
        Me.pnlScriptOptions.ResumeLayout(False)
        Me.pnlScriptOptions.PerformLayout()
        Me.grpOutputSettings.ResumeLayout(False)
        Me.pnlOutputOptions.ResumeLayout(False)
        Me.pnlOutputOptions.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents toolTip As System.Windows.Forms.ToolTip
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents grpScriptGeneration As System.Windows.Forms.GroupBox
    Friend WithEvents pnlScriptOptions As System.Windows.Forms.Panel
    Friend WithEvents chkGenerateDropStatements As System.Windows.Forms.CheckBox
    Friend WithEvents lblDropStatementsDesc As System.Windows.Forms.Label
    Friend WithEvents grpOutputSettings As System.Windows.Forms.GroupBox
    Friend WithEvents pnlOutputOptions As System.Windows.Forms.Panel
    Friend WithEvents chkOverwriteExistingFiles As System.Windows.Forms.CheckBox
    Friend WithEvents lblOverwriteDesc As System.Windows.Forms.Label
    Friend WithEvents pnlButtons As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnReset As System.Windows.Forms.Button
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents picIcon As System.Windows.Forms.PictureBox
End Class