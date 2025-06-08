Imports System.IO
Imports System.Windows.Forms

Public Class frmSetting
    Private iniHelper As IniHelper
    Private ReadOnly iniFilePath As String = Path.Combine(Application.StartupPath, "sconfig.ini")

    ' Settings properties
    Public Property GenerateDropStatements As Boolean = False
    Public Property OverwriteExistingFiles As Boolean = True

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

            ' Update UI controls
            chkGenerateDropStatements.Checked = GenerateDropStatements
            chkOverwriteExistingFiles.Checked = OverwriteExistingFiles

        Catch ex As Exception
            MessageBox.Show($"Error loading settings: {ex.Message}", "Settings Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

            ' Save to INI file
            iniHelper.WriteBoolean("GenerationSettings", "GenerateDropStatements", GenerateDropStatements)
            iniHelper.WriteBoolean("GenerationSettings", "OverwriteExistingFiles", OverwriteExistingFiles)

            ' Write timestamp
            iniHelper.WriteValue("GenerationSettings", "LastUpdated", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

        Catch ex As Exception
            MessageBox.Show($"Error saving settings: {ex.Message}", "Settings Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Reset settings to default values
    ''' </summary>
    Private Sub ResetSettings()
        GenerateDropStatements = False
        OverwriteExistingFiles = True

        ' Update UI controls
        chkGenerateDropStatements.Checked = GenerateDropStatements
        chkOverwriteExistingFiles.Checked = OverwriteExistingFiles
    End Sub

#End Region

#Region "Button Event Handlers"

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        SaveSettings()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        SaveSettings()
        MessageBox.Show("Settings applied successfully!", "Settings",
                       MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        If MessageBox.Show("Reset all settings to default values?", "Reset Settings",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            ResetSettings()
            MessageBox.Show("Settings reset to default values.", "Settings Reset",
                           MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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
    End Sub

#End Region

End Class