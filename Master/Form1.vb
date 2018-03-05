Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Text
Imports Interfaces



Public Class Form1
    Public LPluginTypes As New List(Of Type)


    Public LPlugin As New List(Of IClientPlugin)
    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseUp
        If e.Button = MouseButtons.Right Then
            GroupToolStripMenuItem.DropDownItems.Clear()
            For Each category As ListViewGroup In ListView1.Groups
                GroupToolStripMenuItem.DropDownItems.Add(New ToolStripButton(category.Header, Nothing, Function() CreateNewGroup(category.Header, False)))
            Next
            If GroupToolStripMenuItem.DropDownItems.Count > 0 Then
                GroupToolStripMenuItem.DropDownItems.Add(New ToolStripSeparator)
            End If
            GroupToolStripMenuItem.DropDownItems.Add(New ToolStripButton("New...", Nothing, Function() CreateNewGroup("New...", True)))
            SaveConnectionsToFileToolStripMenuItem.Enabled = Not (ListView1.SelectedItems.Count = 0)
            GroupToolStripMenuItem.Enabled = Not (ListView1.SelectedItems.Count = 0)
            RetryToolStripMenuItem.Enabled = Not (ListView1.SelectedItems.Count = 0)
            QuickActionsToolStripMenuItem.Enabled = Not (ListView1.SelectedItems.Count = 0)
            Dim LList As New List(Of List(Of String))
            Dim LLocalPlugin As New List(Of String)
            For Each Plugin As IClientPlugin In LPlugin
                LLocalPlugin.Add((Plugin.Name & Plugin.Version).GetHashCode)
            Next
            LList.Add(LLocalPlugin)
            Dim LSelectedLink As New List(Of Link)
            For Each SelectedPC As ListViewItem In ListView1.SelectedItems
                If DirectCast(SelectedPC.Tag, Link).LinkStatus = LinkStatus.Running Then
                    LSelectedLink.Add(TryCast(SelectedPC.Tag, Link))
                    RetryToolStripMenuItem.Enabled = False
                    Dim LPluginInstalled As New List(Of String)
                    For Each PI As PluginInfo In DirectCast(SelectedPC.Tag, Link).RemoteMachine.Plugins
                        LPluginInstalled.Add((PI.Name & PI.Version).GetHashCode)
                    Next
                    LList.Add(LPluginInstalled)
                Else
                    QuickActionsToolStripMenuItem.Enabled = False
                End If
            Next
            If QuickActionsToolStripMenuItem.Enabled Then

                For i = 0 To LList.Count - 2
                    Dim x As New List(Of String)
                    x.AddRange(LList(i + 1).Intersect(LList(i)))
                    LList(i + 1) = x
                Next
                QuickActionsToolStripMenuItem.DropDownItems.Clear()
                For Each Plugin As IClientPlugin In LPlugin

                    For Each l In LList(LList.Count - 1)
                        If l = (Plugin.Name & Plugin.Version).GetHashCode Then
                            For Each QuickAction As IClientPlugin.IQuickAction In Plugin.QuickActions
                                QuickActionsToolStripMenuItem.DropDownItems.Add(New ToolStripMenuItem(QuickAction.Name, QuickAction.Icon, Sub() Invoke(QuickAction.Action, LSelectedLink)))
                            Next
                        End If
                    Next
                    QuickActionsToolStripMenuItem.DropDownItems.Add(New ToolStripSeparator)
                Next
                Try
                    QuickActionsToolStripMenuItem.DropDownItems.Remove(QuickActionsToolStripMenuItem.DropDownItems(QuickActionsToolStripMenuItem.DropDownItems.Count - 1))
                Catch ex As Exception
                End Try
                If QuickActionsToolStripMenuItem.DropDownItems.Count = 0 Then QuickActionsToolStripMenuItem.Enabled = False
            End If


            ContextMenuStrip1.Show(MousePosition)
        End If
    End Sub
    Public Sub SendPluginData(ByVal sender As IClientPlugin, ByVal data As String, ByVal destination As List(Of Link))
        For Each PCLink As Link In destination
            PCLink.Send(XMLLibrary.ToText(New PluginPacket With {.Name = sender.Name, .Version = sender.Version, .Data = data}))
        Next
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False

        Dim files() As String = Directory.GetFiles("C:\Users\Davide\Desktop\Plugin dlls", "*.dll", SearchOption.TopDirectoryOnly)
        For Each FileName As String In files
            Dim asm As Assembly = Assembly.LoadFile(FileName)
            Dim myType As Type = asm.GetType(asm.GetName.Name + ".ClientPlugin")
            If GetType(IClientPlugin).IsAssignableFrom(myType) Then
                LPluginTypes.Add(myType)
                'Dim plugin As IClientPlugin = CType(Activator.CreateInstance(myType), IClientPlugin)
                'Try

                'plugin.ClientStarted()
                'AddHandler plugin.SendData, AddressOf SendPluginData
                'LPlugin.Add(plugin)
                'Catch ex As Exception

                'End Try
            End If
        Next
        Try
            For Each Group As GroupArrangementStruct In XMLLibrary.ToObject(My.Settings.GroupArrangement, GetType(List(Of GroupArrangementStruct)))
                ListView1.Groups.Add(New ListViewGroup(Group.GroupName))
            Next
        Catch ex As Exception
        End Try



        'Dim item As New ListViewItem
        'item.Text = Dns.GetHostEntry(IPAddress.Parse("192.168.1.102")).HostName
        'item.SubItems.Add("192.168.1.102")
        'Dim PCLink As New Link(New IPEndPoint(IPAddress.Parse("192.168.1.102"), 11000), LPluginTypes)
        'item.Tag = PCLink
        'If PCLink.LinkStatus = LinkStatus.Running Then item.ImageIndex = 1 Else item.ImageIndex = 0
        'AddHandler PCLink.Update, AddressOf LinkUpdate
        'AddHandler PCLink.ConnectionFailed, AddressOf ConnectionFailed
        'AddHandler PCLink.IncomingData, AddressOf IncomingData
        'AddItemToListView(item)
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As CancelEventArgs) Handles Me.FormClosing
        Dim GroupArrangementList As New List(Of GroupArrangementStruct)
        For Each Group As ListViewGroup In ListView1.Groups
            Dim GroupArrangement As New GroupArrangementStruct
            Dim ItemList As New List(Of String)
            For Each Item As ListViewItem In Group.Items
                ItemList.Add((Item.Text & Item.SubItems(1).Text).GetHashCode)
            Next
            GroupArrangement.GroupName = Group.Header
            GroupArrangement.ItemsName = ItemList.ToArray
            GroupArrangementList.Add(GroupArrangement)
        Next
        My.Settings.GroupArrangement = XMLLibrary.ToText(GroupArrangementList)
    End Sub


    Private Sub AddItemToListView(ByVal Item As ListViewItem)
        Try

            For Each Group As GroupArrangementStruct In XMLLibrary.ToObject(My.Settings.GroupArrangement, GetType(List(Of GroupArrangementStruct)))
                For Each ItemHash As String In Group.ItemsName
                    If (Item.Text & Item.SubItems(1).Text).GetHashCode = ItemHash Then
                        For Each Category As ListViewGroup In ListView1.Groups
                            If Category.Header = Group.GroupName Then
                                Item.Group = Category
                            End If
                        Next
                    End If
                Next
            Next
        Catch ex As Exception

        End Try
        ListView1.Items.Add(Item)
    End Sub
    Private Function CreateNewGroup(ByVal Name As String, ByVal IsNew As Boolean)
        Dim GroupName As String = Name
        If IsNew Then
            GroupName = InputBox("Insert name for the new group:")
            Dim NewCategory As New ListViewGroup(GroupName)
            ListView1.Groups.Add(NewCategory)
            For Each SelectedItem As ListViewItem In ListView1.SelectedItems
                SelectedItem.Group = ListView1.Groups(ListView1.Groups.Count - 1)
            Next
        End If
        For Each Category As ListViewGroup In ListView1.Groups
            If Category.Header = GroupName Then
                For Each SelectedItem As ListViewItem In ListView1.SelectedItems
                    SelectedItem.Group = Category
                Next
                Exit Function
            End If
        Next
        Return Nothing
    End Function


    Private Sub SearchToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SearchToolStripMenuItem.Click
        Dim SearchPC As New SearchPCForm
        SearchPC.ShowDialog()
        For Each PC As ConnectionStruct In SearchPC.SelectedList
            Dim isPresent As Boolean = False
            For Each Item As ListViewItem In ListView1.Items
                Dim ItemLink As Link = DirectCast(Item.Tag, Link)
                If ItemLink IsNot Nothing Then
                    If PC.LinkObject.RemoteEndPoint.Address.ToString = ItemLink.RemoteEndPoint.Address.ToString Then
                        isPresent = True
                        PC.LinkObject.Close()
                    End If
                End If
            Next
            If isPresent = False Then
                Dim item As New ListViewItem
                item.Text = PC.PCName
                item.SubItems.Add(PC.LinkObject.RemoteEndPoint.Address.ToString)
                item.Tag = PC.LinkObject
                If PC.LinkObject.LinkStatus = LinkStatus.Running Then item.ImageIndex = 1 Else item.ImageIndex = 0
                AddHandler PC.LinkObject.Update, AddressOf LinkUpdate
                AddHandler PC.LinkObject.ConnectionFailed, AddressOf ConnectionFailed
                AddHandler PC.LinkObject.IncomingData, AddressOf IncomingData
                AddItemToListView(item)
            End If
        Next
    End Sub

    Private Sub SaveConnectionsToFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveConnectionsToFileToolStripMenuItem.Click
        Dim ConnectionList As New List(Of ConnectionStruct)
        For Each item As ListViewItem In ListView1.SelectedItems
            Dim connection As New ConnectionStruct
            connection.PCName = item.Text 'TODO: Reference RemoteEndMachine 
            connection.PCAddress = DirectCast(item.Tag, Link).RemoteEndPoint.Address.ToString
            ConnectionList.Add(connection)
        Next
        Dim SaveDialog As New SaveFileDialog With {.Filter = "XML File|*.xml", .Title = "Save connections"}
        Dim DR As DialogResult = SaveDialog.ShowDialog()
        If DR = DialogResult.OK Then
            Dim objWriter As New StreamWriter(SaveDialog.FileName)
            objWriter.WriteLine(XMLLibrary.ToText(ConnectionList))
            objWriter.Close()
        End If
    End Sub

    Private Sub RetryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RetryToolStripMenuItem.Click
        For Each SelectedPC As ListViewItem In ListView1.SelectedItems
            Dim REP As IPEndPoint = DirectCast(SelectedPC.Tag, Link).RemoteEndPoint
            Dim connection As New Link(REP, LPluginTypes)
            AddHandler connection.Update, AddressOf LinkUpdate
            AddHandler connection.ConnectionFailed, AddressOf ConnectionFailed
            AddHandler connection.IncomingData, AddressOf IncomingData
            SelectedPC.Tag = connection
            Report(connection, "Connecting...", 0)
        Next
    End Sub
    Private Sub IncomingData(ByVal sender As Link, ByVal data As String)
        Dim Packet As New PluginPacket
        Packet = XMLLibrary.ToObject(data, GetType(PluginPacket))
        For Each Plugin As IClientPlugin In sender.LClientPlugin
            If Packet.Name = Plugin.Name And Plugin.Version = Packet.Version Then
                Plugin.IncomingData(sender, Packet.Data)
            End If
        Next

    End Sub

    Private Sub LinkUpdate(ByVal sender As Link, ByVal status As LinkStatus)
        If status = LinkStatus.Running Then
            For Each lvItem As ListViewItem In ListView1.Items
                If lvItem.Tag Is sender Then
                    lvItem.ImageIndex = 1
                    Report(sender, "Connection established", 1)
                End If
            Next
        End If
    End Sub

    Private Sub ConnectionFailed(ByVal sender As Link, ByVal ex As Exception)
        For Each lvItem As ListViewItem In ListView1.Items
            If lvItem.Tag Is sender Then
                lvItem.ImageIndex = 0
                Report(sender, ex.Message, 2)
            End If
        Next
    End Sub

    Public Sub Report(ByVal sender As Link, ByVal message As String, ByVal color As Integer)
        ListView2.Items.Insert(0, New ListViewItem({Dns.GetHostEntry(sender.RemoteEndPoint.Address).HostName.ToString, sender.RemoteEndPoint.Address.ToString, message}, color))
        ListView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

    End Sub



    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick

        Dim LinkManager As New LinkForm

        LinkManager.PCLink = ListView1.SelectedItems(0).Tag
        LinkManager.Show()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged

    End Sub
End Class


Public Structure GroupArrangementStruct
    Public Property GroupName As String
    Public Property ItemsName As String()
End Structure

Public Structure ConnectionStruct
    Public Property PCName As String
    Public Property PCAddress As String
    <Xml.Serialization.XmlIgnore()>
    Public Property LinkObject As Link
End Structure