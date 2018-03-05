Imports System.Drawing
Imports System.Management
Imports System.Windows.Forms



Public Interface IClientPlugin
    ReadOnly Property Name As String
    ReadOnly Property Version As String

    ReadOnly Property Icon As Bitmap
    Function UI(ByVal destination As List(Of Link)) As Control
    ReadOnly Property QuickActions As List(Of IQuickAction)
    Delegate Sub ActionDelegate(destination As List(Of Link))

    Structure IQuickAction
        Property Name As String
        Property Icon As Bitmap
        Property Action As [Delegate]
    End Structure

    Sub ClientStarted()
    Sub IncomingData(ByVal sender As Link, ByVal data As String)
    Event SendData(ByVal sender As IClientPlugin, ByVal data As String, ByVal destination As List(Of Link))
End Interface

Public Interface IServerPlugin
    ReadOnly Property Name As String
    ReadOnly Property Version As String

    Sub ServerStarted()
    Sub IncomingData(ByVal sender As Link, ByVal data As String)
    Event SendData(ByVal sender As IServerPlugin, ByVal data As String)
End Interface




Public Class Machine
    Public Property LoggedUser As String
    Public Property ProcessorName As String
    Public Property VideoControllerName As String
    Public Property MoboInfo As String
    Public Property OSName As String
    Public Property OSArchitecture As String
    Public Property DiskDriveInfo As String
    Public Property VideoResolution As String
    Public Property ProcessorCoreNumber As String
    Public Property MachineName As String
    Public Property RAM As String
    Public Property Plugins As PluginInfo()
    Sub New()
        'Necessario per la serializzazione XML
    End Sub
    Sub New(ByVal LPlugin As PluginInfo())
        Using objMgmt As ManagementObject = New ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem").Get()(0)
            LoggedUser = objMgmt("username").ToString()
        End Using
        Using objMgmt As ManagementObject = New ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get()(0)
            ProcessorName = objMgmt("name").ToString()
        End Using
        Using objMgmt As ManagementObject = New ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get()(0)
            VideoControllerName = objMgmt("caption").ToString()
        End Using
        Using objMgmt As ManagementObject = New ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard").Get()(0)
            MoboInfo = objMgmt("manufacturer").ToString() & " - " & objMgmt("product").ToString()
        End Using
        Using objMgmt As ManagementObject = New ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get()(0)
            OSName = objMgmt("caption").ToString()
            OSArchitecture = objMgmt("osarchitecture").ToString()
        End Using
        For Each objMgmt As ManagementObject In New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive").Get()
            DiskDriveInfo = DiskDriveInfo & objMgmt("caption").ToString() & " | " & Utils.BytesToString(Val(objMgmt("size").ToString())) & "&"
        Next
        VideoResolution = Screen.PrimaryScreen.Bounds.Width & "x" & Screen.PrimaryScreen.Bounds.Height
        RAM = Utils.BytesToString(My.Computer.Info.TotalPhysicalMemory)
        MachineName = Environment.MachineName
        ProcessorCoreNumber = Environment.ProcessorCount
        Plugins = LPlugin
    End Sub
End Class
Public Class PluginInfo
    Property Name As String
    Property Version As String
End Class
Public Class PluginPacket
    Inherits PluginInfo
    Property Data As String
End Class