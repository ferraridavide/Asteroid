Imports System.ComponentModel
Imports Interfaces

Public Class LinkForm
    Public PCLink As Link
    Private Sub LinkForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim LList As New List(Of List(Of String))
        Dim LLocalPlugin As New List(Of String)
        For Each Plugin As IClientPlugin In Form1.LPlugin
            LLocalPlugin.Add((Plugin.Name & Plugin.Version).GetHashCode)
        Next
        LList.Add(LLocalPlugin)
        Dim LPluginInstalled As New List(Of String)
        For Each PI As PluginInfo In PCLink.RemoteMachine.Plugins
            LPluginInstalled.Add((PI.Name & PI.Version).GetHashCode)
        Next
        LList.Add(LPluginInstalled)
        For i = 0 To LList.Count - 2
            Dim x As New List(Of String)
            x.AddRange(LList(i + 1).Intersect(LList(i)))
            LList(i + 1) = x
        Next
        For Each Plugin As IClientPlugin In Form1.LPlugin

            For Each l In LList(LList.Count - 1)
                If l = (Plugin.Name & Plugin.Version).GetHashCode Then

                    ImageList1.Images.Add(Plugin.Icon)
                    TreeView1.Nodes.Add(New TreeNode(Plugin.Name, ImageList1.Images.Count - 1, ImageList1.Images.Count - 1) With {.Tag = Plugin.UI})


                End If
            Next
        Next

        AddHandler PCLink.Update, AddressOf LinkUpdate
        AddHandler PCLink.ConnectionFailed, AddressOf ConnectionFailed
        AddHandler PCLink.IncomingData, AddressOf IncomingData
        ToolStripStatusLabel1.Text = PCLink.RemoteMachine.LoggedUser

        TreeView1.Nodes(0).Tag = New MachineInfo
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        If e.Node.Tag Is Nothing Then
            Panel1.Controls.Clear()
        Else
            Panel1.Controls.Clear()

            Dim UI As UserControl = e.Node.Tag
            UI.Tag = PCLink
            Panel1.Controls.Add(UI)
            UI.Dock = DockStyle.Fill
            Panel1.ResumeLayout()

        End If

    End Sub

    Private Sub IncomingData(ByVal sender As Link, ByVal data As String)
        MsgBox("incoming")

    End Sub

    Private Sub LinkUpdate(ByVal sender As Link, ByVal status As LinkStatus)
        MsgBox("update")

    End Sub

    Private Sub ConnectionFailed(ByVal sender As Link, ByVal ex As Exception)
        MsgBox("failed")
    End Sub

    Private Sub LinkForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        RemoveHandler PCLink.Update, AddressOf LinkUpdate
        RemoveHandler PCLink.ConnectionFailed, AddressOf ConnectionFailed
        RemoveHandler PCLink.IncomingData, AddressOf IncomingData
    End Sub
End Class