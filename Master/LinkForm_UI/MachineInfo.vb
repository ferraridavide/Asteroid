Imports Interfaces

Public Class MachineInfo


    Private Sub MachineInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim RemoteMachine As Machine = DirectCast(Me.Tag, Link).RemoteMachine
        ListView1.Items(0).SubItems.Add(RemoteMachine.MachineName)
        ListView1.Items(1).SubItems.Add(RemoteMachine.LoggedUser)
        ListView1.Items(2).SubItems.Add(RemoteMachine.ProcessorName)
        ListView1.Items(3).SubItems.Add(RemoteMachine.ProcessorCoreNumber)
        ListView1.Items(4).SubItems.Add(RemoteMachine.MoboInfo)
        ListView1.Items(5).SubItems.Add(RemoteMachine.VideoControllerName)
        ListView1.Items(6).SubItems.Add(RemoteMachine.VideoResolution)
        ListView1.Items(7).SubItems.Add(RemoteMachine.OSName)
        ListView1.Items(8).SubItems.Add(RemoteMachine.OSArchitecture)
        ListView1.Items(9).SubItems.Add(RemoteMachine.RAM)
        Dim diskindex As Integer = 1
        For Each Disk As String In RemoteMachine.DiskDriveInfo.Split("&")
            If Disk <> "" Then
                Dim DiskItem As New ListViewItem("Disk drive (" & diskindex & ")")
                DiskItem.SubItems.Add(Disk)
                DiskItem.Group = ListView1.Groups(0)
                ListView1.Items.Add(DiskItem)
                diskindex += 1
            End If
        Next


    End Sub
End Class
