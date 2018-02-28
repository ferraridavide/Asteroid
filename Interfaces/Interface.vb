Imports System.Drawing
Imports System.Windows.Forms



Public Interface IClientPlugin
    ReadOnly Property Name As String
    ReadOnly Property Version As String

    ReadOnly Property Icon As Bitmap
    ReadOnly Property UI As Control
    ReadOnly Property QuickActions As List(Of IQuickAction)
    Delegate Sub ActionDelegate(destination As List(Of Link))

    Sub ClientStarted()
    Sub IncomingData(ByVal sender As Link, ByVal data As String)
    Event SendData(ByVal sender As IClientPlugin, ByVal data As String, ByVal destination As List(Of Link))

    Structure IQuickAction
        Property Name As String
        Property Icon As Bitmap
        Property Action As [Delegate]
    End Structure
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
    Public Property Plugins As PluginInfo()
    Sub New()
        'Necessario per la serializzazione XML
    End Sub
    Sub New(ByVal LPlugin As PluginInfo())
        LoggedUser = "Ciao"
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