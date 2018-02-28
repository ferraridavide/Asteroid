Public Class MachineInfo
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        For Each sel As ListViewItem In ListView1.SelectedItems
            sel.Selected = False
        Next
        ListView1.Enabled = False
    End Sub
End Class
