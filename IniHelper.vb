Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices

''' <summary>
''' Helper class for reading and writing INI files using Windows API
''' </summary>
Public Class IniHelper
    Private ReadOnly _filePath As String

    ' Windows API declarations for INI file operations
    <DllImport("kernel32", CharSet:=CharSet.Unicode)>
    Private Shared Function WritePrivateProfileString(ByVal section As String, ByVal key As String, ByVal val As String, ByVal filePath As String) As Long
    End Function

    <DllImport("kernel32", CharSet:=CharSet.Unicode)>
    Private Shared Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
    End Function

    ''' <summary>
    ''' Initialize the INI helper with the specified file path
    ''' </summary>
    ''' <param name="filePath">Full path to the INI file</param>
    Public Sub New(filePath As String)
        _filePath = filePath

        ' Ensure the directory exists
        Dim directoryPath As String = Path.GetDirectoryName(_filePath)
        If Not String.IsNullOrEmpty(directoryPath) AndAlso Not Directory.Exists(directoryPath) Then
            Directory.CreateDirectory(directoryPath)
        End If
    End Sub

    ''' <summary>
    ''' Read a string value from the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    ''' <param name="defaultValue">Default value if key not found</param>
    ''' <returns>The value from the INI file or default value</returns>
    Public Function ReadValue(section As String, key As String, defaultValue As String) As String
        Dim temp As New StringBuilder(255)
        Dim result As Integer = GetPrivateProfileString(section, key, defaultValue, temp, 255, _filePath)
        Return temp.ToString()
    End Function

    ''' <summary>
    ''' Read an integer value from the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    ''' <param name="defaultValue">Default value if key not found</param>
    ''' <returns>The integer value from the INI file or default value</returns>
    Public Function ReadInteger(section As String, key As String, defaultValue As Integer) As Integer
        Dim value As String = ReadValue(section, key, defaultValue.ToString())
        Dim result As Integer
        If Integer.TryParse(value, result) Then
            Return result
        Else
            Return defaultValue
        End If
    End Function

    ''' <summary>
    ''' Read a boolean value from the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    ''' <param name="defaultValue">Default value if key not found</param>
    ''' <returns>The boolean value from the INI file or default value</returns>
    Public Function ReadBoolean(section As String, key As String, defaultValue As Boolean) As Boolean
        Dim value As String = ReadValue(section, key, defaultValue.ToString())
        Dim result As Boolean
        If Boolean.TryParse(value, result) Then
            Return result
        Else
            Return defaultValue
        End If
    End Function

    ''' <summary>
    ''' Write a string value to the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    ''' <param name="value">Value to write</param>
    Public Sub WriteValue(section As String, key As String, value As String)
        WritePrivateProfileString(section, key, value, _filePath)
    End Sub

    ''' <summary>
    ''' Write an integer value to the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    ''' <param name="value">Value to write</param>
    Public Sub WriteInteger(section As String, key As String, value As Integer)
        WriteValue(section, key, value.ToString())
    End Sub

    ''' <summary>
    ''' Write a boolean value to the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    ''' <param name="value">Value to write</param>
    Public Sub WriteBoolean(section As String, key As String, value As Boolean)
        WriteValue(section, key, value.ToString())
    End Sub

    ''' <summary>
    ''' Delete a key from the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="key">Key name</param>
    Public Sub DeleteKey(section As String, key As String)
        WritePrivateProfileString(section, key, Nothing, _filePath)
    End Sub

    ''' <summary>
    ''' Delete an entire section from the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    Public Sub DeleteSection(section As String)
        WritePrivateProfileString(section, Nothing, Nothing, _filePath)
    End Sub

    ''' <summary>
    ''' Check if the INI file exists
    ''' </summary>
    ''' <returns>True if file exists, False otherwise</returns>
    Public Function FileExists() As Boolean
        Return File.Exists(_filePath)
    End Function

    ''' <summary>
    ''' Get the full path of the INI file
    ''' </summary>
    ''' <returns>Full file path</returns>
    Public ReadOnly Property FilePath As String
        Get
            Return _filePath
        End Get
    End Property

    ''' <summary>
    ''' Read a list of string values from the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="keyPrefix">Prefix for the keys (e.g., "Table" for Table0, Table1, etc.)</param>
    ''' <param name="count">Number of items to read</param>
    ''' <returns>List of string values</returns>
    Public Function ReadList(section As String, keyPrefix As String, count As Integer) As List(Of String)
        Dim result As New List(Of String)

        For i As Integer = 0 To count - 1
            Dim value As String = ReadValue(section, $"{keyPrefix}{i}", "")
            If Not String.IsNullOrEmpty(value) Then
                result.Add(value)
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' Write a list of string values to the INI file
    ''' </summary>
    ''' <param name="section">Section name</param>
    ''' <param name="keyPrefix">Prefix for the keys (e.g., "Table" for Table0, Table1, etc.)</param>
    ''' <param name="values">List of values to write</param>
    Public Sub WriteList(section As String, keyPrefix As String, values As List(Of String))
        ' Clear existing entries by deleting the section first
        DeleteSection(section)

        ' Write count
        WriteInteger(section, "Count", values.Count)

        ' Write each value
        For i As Integer = 0 To values.Count - 1
            WriteValue(section, $"{keyPrefix}{i}", values(i))
        Next
    End Sub

End Class