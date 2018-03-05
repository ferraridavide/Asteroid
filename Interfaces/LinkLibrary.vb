Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports Interfaces

Public Enum LinkType
    ToServer = 0
    ToClient = 1
End Enum
Public Enum LinkStatus
    Uninitialized = 0
    Connecting = 1
    Running = 2
    Closed = 3
End Enum



Public Class Link
    Public Event IncomingData(ByVal sender As Link, ByVal data As String)
    Public Event ConnectionFailed(ByVal sender As Link, ByVal ex As Exception)
    Public Event Update(ByVal sender As Link, ByVal status As LinkStatus)

    Private DataStream As Stream
    Private Client As New TcpClient
    Private BufferSize As Integer
    Private Buffer As Byte()
    Public LClientPlugin As New List(Of IClientPlugin)
    Public ReadOnly LinkType As LinkType
    Public LinkStatus As LinkStatus = LinkStatus.Uninitialized
    Public ReadOnly RemoteEndPoint As IPEndPoint
    Public RemoteMachine As Machine

    Public Sub New()

    End Sub
    Public Sub New(ByVal PenFriend As TcpClient)
        Try

            LinkType = LinkType.ToClient
            Client = PenFriend

            DataStream = Client.GetStream
            BufferSize = Client.SendBufferSize
            RemoteEndPoint = Client.Client.RemoteEndPoint

            Buffer = New Byte(BufferSize - 1) {}

            DataStream.BeginRead(Buffer, 0, BufferSize, New AsyncCallback(AddressOf ReadCallback), Nothing)
            LinkStatus = LinkStatus.Running
            'Try
        Catch ex As Exception
            Fail(ex)
        End Try

    End Sub
    Public Sub SendPluginData(ByVal sender As IClientPlugin, ByVal data As String, ByVal destination As List(Of Link))
        For Each PCLink As Link In destination
            PCLink.Send(XMLLibrary.ToText(New PluginPacket With {.Name = sender.Name, .Version = sender.Version, .Data = data}))
        Next
    End Sub
    Public Sub New(ByVal EndPoint As IPEndPoint, LPluginType As List(Of Type))

        For Each pluginType As Type In LPluginType
            Dim plugin As IClientPlugin = CType(Activator.CreateInstance(pluginType), IClientPlugin)
            Try

                plugin.ClientStarted()
                AddHandler plugin.SendData, AddressOf SendPluginData
                LClientPlugin.Add(plugin)
            Catch ex As Exception
                MsgBox("FAIL - " & ex.Message)
            End Try



        Next

        Try

            LinkType = LinkType.ToServer
            RemoteEndPoint = EndPoint
            Client.BeginConnect(EndPoint.Address, EndPoint.Port, New AsyncCallback(AddressOf ConnectCallback), Client)
            LinkStatus = LinkStatus.Connecting
            'Try
        Catch ex As Exception
            Fail(ex)
        End Try

    End Sub

    Private Sub ConnectCallback(ByVal ar As IAsyncResult)
        Try
            Client.EndConnect(ar)
            DataStream = Client.GetStream
            BufferSize = Client.SendBufferSize

            Buffer = New Byte(BufferSize - 1) {}

            DataStream.BeginRead(Buffer, 0, BufferSize, New AsyncCallback(AddressOf ReadCallback), Nothing)
            LinkStatus = LinkStatus.Running
            RaiseEvent Update(Me, LinkStatus)
            'Try

        Catch ex As Exception
            Fail(ex)
        End Try
    End Sub

    Private Sub ReadCallback(ByVal ar As IAsyncResult)
        Try
            Dim bytesRead As Integer = DataStream.EndRead(ar)
            If bytesRead > 0 Then
                If XMLLibrary.TryObjects(Encoding.ASCII.GetString(Buffer, 0, bytesRead), {GetType(Machine)}) IsNot Nothing Then
                    RemoteMachine = XMLLibrary.ToObject(Encoding.ASCII.GetString(Buffer, 0, bytesRead), GetType(Machine))
                Else
                    RaiseEvent IncomingData(Me, Encoding.ASCII.GetString(Buffer, 0, bytesRead))
                End If

                DataStream.BeginRead(Buffer, 0, BufferSize, New AsyncCallback(AddressOf ReadCallback), Nothing)
            ElseIf bytesRead = 0 Then
                Me.Close()
            End If
            'Try
        Catch ex As Exception
            Fail(ex)
        End Try
    End Sub

    Public Sub Send(ByVal data As String)
        Try
            Dim byteData As Byte() = Encoding.ASCII.GetBytes(data)
            DataStream.Write(byteData, 0, byteData.Length)
            'Try
        Catch ex As Exception
            Fail(ex)
        End Try
    End Sub
    Public Sub Close()
        Try
            Client.Client.Disconnect(False)
        Catch ex As Exception
        End Try
        Try

            Client.Client.Shutdown(SocketShutdown.Both)
        Catch ex As Exception
        End Try
        Try
            Client.Close()
        Catch ex As Exception

        End Try
        Try
            DataStream.Close()
        Catch ex As Exception

        End Try
        RaiseEvent ConnectionFailed(Me, New Exception(""))
    End Sub

    Public Sub Fail(ByVal ex As Exception)
        LinkStatus = LinkStatus.Closed
        Try
            DataStream.Dispose()
        Catch exe As Exception

        End Try
        RaiseEvent ConnectionFailed(Me, ex)
    End Sub
    Public Sub CauseUpdate()

        RaiseEvent Update(Me, LinkStatus)
    End Sub
End Class



