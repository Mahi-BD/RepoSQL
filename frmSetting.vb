Imports System.IO
Imports System.Windows.Forms

Public Class frmSetting
    Private iniHelper As IniHelper
    Private ReadOnly iniFilePath As String = Path.Combine(Application.StartupPath, "sconfig.ini")

    ' Settings properties
    Public Property GenerateDropStatements As Boolean = False
    Public Property OverwriteExistingFiles As Boolean = True
    Public Property OpenOutputFolderAfterOperation As Boolean = False ' New property

#Region "Form Events"

    Private Sub frmSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeForm()
        LoadSettings()
    End Sub

#End Region

#Region "Initialization"

    Private Sub InitializeForm()
        ' Initialize INI helper
        iniHelper = New IniHelper(iniFilePath)

        ' Set form properties
        Me.Text = "RepoSQL Settings"

        ' Set default values
        GenerateDropStatements = False
        OverwriteExistingFiles = True
        OpenOutputFolderAfterOperation = False ' Default to false
    End Sub

#End Region

#Region "Settings Management"

    ''' <summary>
    ''' Load settings from sconfig.ini file
    ''' </summary>
    Private Sub LoadSettings()
        Try
            ' Load settings from INI file
            GenerateDropStatements = iniHelper.ReadBoolean("GenerationSettings", "GenerateDropStatements", False)
            OverwriteExistingFiles = iniHelper.ReadBoolean("GenerationSettings", "OverwriteExistingFiles", True)
            OpenOutputFolderAfterOperation = iniHelper.ReadBoolean("GenerationSettings", "OpenOutputFolderAfterOperation", False)

            ' Update UI controls
            chkGenerateDropStatements.Checked = GenerateDropStatements
            chkOverwriteExistingFiles.Checked = OverwriteExistingFiles
            chkOpenOutputFolder.Checked = OpenOutputFolderAfterOperation

        Catch ex As Exception
            UpdateButtonStatus(btnApply, "error", "Error loading settings")
        End Try
    End Sub

    ''' <summary>
    ''' Save settings to sconfig.ini file
    ''' </summary>
    Private Sub SaveSettings()
        Try
            ' Get values from UI controls
            GenerateDropStatements = chkGenerateDropStatements.Checked
            OverwriteExistingFiles = chkOverwriteExistingFiles.Checked
            OpenOutputFolderAfterOperation = chkOpenOutputFolder.Checked

            ' Save to INI file
            iniHelper.WriteBoolean("GenerationSettings", "GenerateDropStatements", GenerateDropStatements)
            iniHelper.WriteBoolean("GenerationSettings", "OverwriteExistingFiles", OverwriteExistingFiles)
            iniHelper.WriteBoolean("GenerationSettings", "OpenOutputFolderAfterOperation", OpenOutputFolderAfterOperation)

            ' Write timestamp
            iniHelper.WriteValue("GenerationSettings", "LastUpdated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

        Catch ex As Exception
            UpdateButtonStatus(btnApply, "error", "Error saving settings")
        End Try
    End Sub

    ''' <summary>
    ''' Reset settings to default values
    ''' </summary>
    Private Sub ResetSettings()
        GenerateDropStatements = False
        OverwriteExistingFiles = True
        OpenOutputFolderAfterOperation = False

        ' Update UI controls
        chkGenerateDropStatements.Checked = GenerateDropStatements
        chkOverwriteExistingFiles.Checked = OverwriteExistingFiles
        chkOpenOutputFolder.Checked = OpenOutputFolderAfterOperation
    End Sub

    ''' <summary>
    ''' Update button status with visual feedback
    ''' </summary>
    ''' <param name="button">Button to update</param>
    ''' <param name="status">Status type (success, error, warning, info)</param>
    ''' <param name="message">Status message</param>
    ''' <param name="autoRevert">Whether to auto-revert after 3 seconds</param>
    Private Sub UpdateButtonStatus(button As Button, status As String, message As String, Optional autoRevert As Boolean = True)
        Dim originalText As String = button.Tag?.ToString() ' Store original text in Tag
        If String.IsNullOrEmpty(originalText) Then
            button.Tag = button.Text
            originalText = button.Text
        End If

        Dim originalColor As Color = button.BackColor

        Select Case status.ToLower()
            Case "success"
                button.BackColor = Color.FromArgb(40, 167, 69)
                button.Text = "✓ " & message
            Case "error"
                button.BackColor = Color.FromArgb(220, 53, 69)
                button.Text = "✗ " & message
            Case "warning"
                button.BackColor = Color.FromArgb(255, 193, 7)
                button.ForeColor = Color.Black
                button.Text = "⚠ " & message
            Case "info"
                button.BackColor = Color.FromArgb(0, 120, 215)
                button.Text = "ℹ " & message
            Case "loading"
                button.BackColor = Color.FromArgb(108, 117, 125)
                button.Text = "⏳ " & message
        End Select

        Application.DoEvents()

        If autoRevert Then
            ' Auto-revert after 3 seconds using a Timer instead of Task.Delay
            Dim revertTimer As New Timer()
            revertTimer.Interval = 3000 ' 3 seconds
            AddHandler revertTimer.Tick, Sub()
                                             revertTimer.Stop()
                                             revertTimer.Dispose()
                                             If Not Me.IsDisposed AndAlso Me.IsHandleCreated Then
                                                 button.Text = originalText
                                                 button.BackColor = originalColor
                                                 button.ForeColor = Color.White
                                             End If
                                         End Sub
            revertTimer.Start()
        End If
    End Sub

#End Region

#Region "Button Event Handlers"

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        SaveSettings()
        UpdateButtonStatus(btnOK, "success", "Settings saved")

        ' Delay close to show status using Timer instead of Task.Delay
        Dim closeTimer As New Timer()
        closeTimer.Interval = 1000 ' 1 second
        AddHandler closeTimer.Tick, Sub()
                                        closeTimer.Stop()
                                        closeTimer.Dispose()
                                        If Not Me.IsDisposed AndAlso Me.IsHandleCreated Then
                                            Me.DialogResult = DialogResult.OK
                                            Me.Close()
                                        End If
                                    End Sub
        closeTimer.Start()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        SaveSettings()
        UpdateButtonStatus(btnApply, "success", "Applied successfully")
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ' Create custom confirmation dialog instead of MessageBox
        Using confirmDialog As New Form()
            confirmDialog.Text = "Reset Settings"
            confirmDialog.Size = New Size(400, 200)
            confirmDialog.StartPosition = FormStartPosition.CenterParent
            confirmDialog.FormBorderStyle = FormBorderStyle.FixedDialog
            confirmDialog.MaximizeBox = False
            confirmDialog.MinimizeBox = False
            confirmDialog.ShowIcon = False

            Dim lblMessage As New Label()
            lblMessage.Text = "Reset all settings to default values?"
            lblMessage.Font = New Font("Segoe UI", 10)
            lblMessage.Location = New Point(20, 30)
            lblMessage.Size = New Size(360, 50)
            lblMessage.TextAlign = ContentAlignment.MiddleCenter

            Dim btnYes As New Button()
            btnYes.Text = "Yes"
            btnYes.Location = New Point(220, 100)
            btnYes.Size = New Size(75, 30)
            btnYes.BackColor = Color.FromArgb(40, 167, 69)
            btnYes.ForeColor = Color.White
            btnYes.FlatStyle = FlatStyle.Flat
            btnYes.DialogResult = DialogResult.Yes
            AddHandler btnYes.Click, Sub() confirmDialog.Close()

            Dim btnNo As New Button()
            btnNo.Text = "No"
            btnNo.Location = New Point(305, 100)
            btnNo.Size = New Size(75, 30)
            btnNo.BackColor = Color.FromArgb(108, 117, 125)
            btnNo.ForeColor = Color.White
            btnNo.FlatStyle = FlatStyle.Flat
            btnNo.DialogResult = DialogResult.No
            AddHandler btnNo.Click, Sub() confirmDialog.Close()

            confirmDialog.Controls.AddRange({lblMessage, btnYes, btnNo})
            confirmDialog.AcceptButton = btnYes
            confirmDialog.CancelButton = btnNo

            If confirmDialog.ShowDialog(Me) = DialogResult.Yes Then
                ResetSettings()
                UpdateButtonStatus(btnReset, "success", "Settings reset")
            End If
        End Using
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Get current settings from the form
    ''' </summary>
    ''' <returns>Dictionary containing current settings</returns>
    Public Function GetCurrentSettings() As Dictionary(Of String, Object)
        Dim settings As New Dictionary(Of String, Object)
        settings("GenerateDropStatements") = chkGenerateDropStatements.Checked
        settings("OverwriteExistingFiles") = chkOverwriteExistingFiles.Checked
        settings("OpenOutputFolderAfterOperation") = chkOpenOutputFolder.Checked
        Return settings
    End Function

    ''' <summary>
    ''' Apply settings to the form
    ''' </summary>
    ''' <param name="settings">Dictionary containing settings to apply</param>
    Public Sub ApplySettings(settings As Dictionary(Of String, Object))
        If settings.ContainsKey("GenerateDropStatements") Then
            chkGenerateDropStatements.Checked = CBool(settings("GenerateDropStatements"))
        End If

        If settings.ContainsKey("OverwriteExistingFiles") Then
            chkOverwriteExistingFiles.Checked = CBool(settings("OverwriteExistingFiles"))
        End If

        If settings.ContainsKey("OpenOutputFolderAfterOperation") Then
            chkOpenOutputFolder.Checked = CBool(settings("OpenOutputFolderAfterOperation"))
        End If
    End Sub

#End Region

End Class