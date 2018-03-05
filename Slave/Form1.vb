Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Text
Imports Interfaces
Public Class Form1
    Dim Listener As New TcpListener(IPAddress.Any, 11000)
    Dim LPlugin As New List(Of IServerPlugin)
    Dim LClient As New List(Of Link)
    Dim MyMachine As Machine

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim files() As String = Directory.GetFiles("C:\Users\Davide\Desktop\Plugin dlls", "*.dll", SearchOption.TopDirectoryOnly)
            For Each FileName As String In files
                Dim asm As Assembly = Assembly.LoadFile(FileName)
                Dim myType As Type = asm.GetType(asm.GetName.Name + ".ServerPlugin")
                If GetType(IServerPlugin).IsAssignableFrom(myType) Then
                    Dim plugin As IServerPlugin = CType(Activator.CreateInstance(myType), IServerPlugin)
                    Try
                        AddHandler plugin.SendData, AddressOf BroadcastPluginData
                        plugin.ServerStarted()
                        LPlugin.Add(plugin)
                        ListBox2.Items.Add(plugin.Name & " - " & plugin.Version)
                    Catch ex As Exception
                        RichTextBox1.AppendText("Failed loading " & plugin.Name & vbNewLine)
                    End Try
                End If
            Next
        Catch ex As Exception
            RichTextBox1.AppendText("Loading plugins failed - " & ex.Message & vbNewLine)
        End Try
        CheckForIllegalCrossThreadCalls = False 'Necessario solo per i cross-thread con gli elementi della GUI
        Listener.Start()
        Listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptCallback), Listener)
        RichTextBox1.AppendText("Listening on 11000..." & vbNewLine)
        Dim LPluginInfo As New List(Of PluginInfo)
        For Each Plugin As IServerPlugin In LPlugin
            LPluginInfo.Add(New PluginInfo With {.Name = Plugin.Name, .Version = Plugin.Version})
        Next
        MyMachine = New Machine(LPluginInfo.ToArray)
    End Sub
    Public Sub AcceptCallback(ByVal ar As IAsyncResult)
        Dim Client As New Link(Listener.EndAcceptTcpClient(ar))
        Listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptCallback), Listener)
        AddHandler Client.IncomingData, AddressOf IncomingData
        AddHandler Client.ConnectionFailed, AddressOf FailedConnection
        ListBox1.Items.Add(Client)
        LClient.Add(Client)
        RichTextBox1.AppendText(Client.RemoteEndPoint.Address.ToString & " - " & "Connection Established" & vbNewLine)

        Client.Send(XMLLibrary.ToText(MyMachine))
        RichTextBox1.AppendText(Client.RemoteEndPoint.Address.ToString & " - " & "Sent MachineInfo" & vbNewLine)
    End Sub
    Private Sub FailedConnection(ByVal sender As Link, ByVal ex As Exception)
        RichTextBox1.AppendText(sender.RemoteEndPoint.Address.ToString & " - " & "Connection Failed" & vbNewLine)
        ListBox1.Items.Remove(sender)
        LClient.Remove(sender)
    End Sub
    Public Sub BroadcastPluginData(ByVal sender As IServerPlugin, ByVal data As String)
        For Each Client As Link In LClient
            Client.Send(XMLLibrary.ToText(New PluginPacket With {.Name = sender.Name, .Version = sender.Version, .Data = data}))
        Next
    End Sub
    Private Sub IncomingData(ByVal sender As Link, ByVal data As String)
        Dim Packet As PluginPacket = XMLLibrary.ToObject(data, GetType(PluginPacket))
        For Each plugin As IServerPlugin In LPlugin
            If plugin.Name = Packet.Name And plugin.Version = Packet.Version Then
                plugin.IncomingData(sender, Packet.Data)
                Exit Sub
            End If
        Next
        RichTextBox1.AppendText(sender.RemoteEndPoint.Address.ToString & " - " & data & vbNewLine)
    End Sub
#Region "GUI"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim listb As ArrayList = ArrayList.Adapter(ListBox1.Items)
        For Each i As Link In listb.ToArray
            i.Send(TextBox1.Text)

        Next
        TextBox1.ResetText()

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim listb As ArrayList = ArrayList.Adapter(ListBox1.SelectedItems)
        For Each i As Link In listb.ToArray
            i.Send(TextBox1.Text)

        Next
        TextBox1.ResetText()
    End Sub
#End Region
End Class









