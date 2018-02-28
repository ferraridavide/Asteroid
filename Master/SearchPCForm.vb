Imports System.ComponentModel
Imports System.DirectoryServices
Imports System.IO
Imports System.Net
Imports Interfaces
Imports System.Threading.Tasks

Public Class SearchPCForm
    Public SelectedList As New List(Of ConnectionStruct)
    Dim Links As New List(Of Link)

    Private Sub ConnectionFailed(ByVal sender As Link, ByVal ex As Exception)
        Try
            For Each lvItem As ListViewItem In ListView1.Items
                If lvItem.Tag Is sender Then
                    lvItem.SubItems(2).Text = "No"
                    lvItem.ImageIndex = 0
                    Exit For
                End If
            Next

            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        Catch exe As Exception

        End Try
    End Sub
    Private Sub UpdateLink(ByVal sender As Link, ByVal status As LinkStatus)
        Try
            If status = LinkStatus.Running Then
                For Each lvItem As ListViewItem In ListView1.Items
                    If lvItem.Tag Is sender Then
                        lvItem.SubItems(2).Text = "Yes"
                        lvItem.Checked = True
                        lvItem.ImageIndex = 1
                        Exit For
                    End If
                Next


                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AddPCToList(ByVal PCName As String)
        For Each address As IPAddress In Dns.GetHostEntry(PCName).AddressList
            If address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                AddPCToList(address, PCName)
                Exit For
            End If
        Next
    End Sub
    Private Sub AddPCToList(ByVal PCAddress As IPAddress, ByVal Optional PCName As String = Nothing)
        Try
            Me.Invoke(Sub() Label3.Text = PCAddress.ToString)
            For Each PCItem As ListViewItem In ListView1.Items
                If PCItem.SubItems(1).Text = PCAddress.ToString Then
                    PCItem.SubItems(2).Text = "..."
                    TryCast(PCItem.Tag, Link).Close()
                    Dim PCNewLink As New Link(New IPEndPoint(PCAddress, 11000))
                    AddHandler PCNewLink.ConnectionFailed, AddressOf ConnectionFailed
                    AddHandler PCNewLink.Update, AddressOf UpdateLink
                    PCItem.Tag = PCNewLink
                    Exit Sub
                End If
            Next
            If PCName = "" Then PCName = Dns.GetHostEntry(PCAddress).HostName
            Dim NewPCItem As New ListViewItem With {.Text = PCName}
            NewPCItem.ImageIndex = 0
            NewPCItem.SubItems.Add(PCAddress.ToString)
            NewPCItem.SubItems.Add("...")
            Dim PCLink As New Link(New IPEndPoint(PCAddress, 11000))
            AddHandler PCLink.ConnectionFailed, AddressOf ConnectionFailed
            AddHandler PCLink.Update, AddressOf UpdateLink
            NewPCItem.Tag = PCLink
            ListView1.Items.Add(NewPCItem)
            ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub GetNetworkComputers()
        Me.Invoke(Sub() Button8.Enabled = False)
        Me.Invoke(Sub() ProgressBar1.Style = ProgressBarStyle.Marquee)
        Dim alWorkGroups As New ArrayList
        Dim de As New DirectoryEntry

        de.Path = "WinNT:"
        For Each d As DirectoryEntry In de.Children
            If d.SchemaClassName = "Domain" Then alWorkGroups.Add(d.Name)
            d.Dispose()
        Next

        For Each workgroup As String In alWorkGroups

            de.Path = "WinNT://" & workgroup
            For Each d As DirectoryEntry In de.Children
                If d.SchemaClassName = "Computer" Then
                    Me.Invoke(Sub() AddPCToList(d.Name))
                End If
                d.Dispose()
            Next
        Next
        Me.Invoke(Sub() ProgressBar1.Style = ProgressBarStyle.Blocks)
        Me.Invoke(Sub() Button8.Enabled = True)
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim IPA As IPAddress
        If IPAddress.TryParse(TextBox1.Text, IPA) And TextBox1.Text.Count(Function(c As Char) c = ".") = 3 Then

            TextBox1.BackColor = Color.Honeydew
            Button3.Enabled = True
        Else
            TextBox1.BackColor = Color.MistyRose
            Button3.Enabled = False
        End If
        If TextBox1.BackColor = Color.Honeydew And TextBox2.BackColor = Color.Honeydew Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim IPA As IPAddress
        Dim status As Boolean
        If IPAddress.TryParse(TextBox2.Text, IPA) And TextBox2.Text.Count(Function(c As Char) c = ".") = 3 And TextBox1.BackColor = Color.Honeydew Then

            Dim startIntList As New List(Of String)
            Dim endIntList As New List(Of String)

            startIntList.AddRange(TextBox1.Text.Split("."))
            endIntList.AddRange(TextBox2.Text.Split("."))

            Dim elaborateList As New List(Of List(Of String))
            elaborateList.AddRange({startIntList, endIntList})

            For Each list As List(Of String) In elaborateList
                For a = 0 To list.Count - 1
                    For i = list(a).Length To 2
                        list(a) = list(a).Insert(0, "0")
                    Next
                Next
            Next

            If Val(String.Join("", startIntList.ToArray)) <= Val(String.Join("", endIntList.ToArray)) Then
                status = True
            Else
                status = False
            End If
        Else
            status = False
        End If
        If status Then TextBox2.BackColor = Color.Honeydew Else TextBox2.BackColor = Color.MistyRose
        If TextBox1.BackColor = Color.Honeydew And TextBox2.BackColor = Color.Honeydew Then

            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim startIPRange As IPAddress = IPAddress.Parse(TextBox1.Text)
        Dim endIPRange As IPAddress = IPAddress.Parse(TextBox2.Text)
        Dim startIP() As Byte = startIPRange.GetAddressBytes
        Array.Reverse(startIP)
        Dim endIP() As Byte = endIPRange.GetAddressBytes
        Array.Reverse(endIP)
        Dim start As UInt32 = BitConverter.ToUInt32(startIP, 0)
        Dim finish As UInt32 = BitConverter.ToUInt32(endIP, 0)
        For anip As UInt32 = start To finish
            Dim ipbyt() As Byte = BitConverter.GetBytes(anip)
            Array.Reverse(ipbyt)
            Dim ipaddr As New IPAddress(ipbyt)
            Task.Run(Sub() AddPCToList(ipaddr))
        Next
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim OpenDialog As New OpenFileDialog With {.Filter = "XML File|*.xml", .Title = "Opne connections file"}
        Dim DR As DialogResult = OpenDialog.ShowDialog
        If DR = DialogResult.OK Then
            TextBox4.Text = OpenDialog.FileName
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If File.Exists(TextBox4.Text) Then
            Dim objReader As New StreamReader(TextBox4.Text)
            Dim ConnectionList As List(Of ConnectionStruct) = XMLLibrary.ToObject(objReader.ReadToEnd, GetType(List(Of ConnectionStruct)))
            objReader.Close()
            For Each connection As ConnectionStruct In ConnectionList
                Task.Run(Sub() AddPCToList(IPAddress.Parse(connection.PCAddress)))
            Next
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If File.Exists(TextBox4.Text) Then
            TextBox4.BackColor = Color.Honeydew
            Button7.Enabled = True
        Else
            TextBox4.BackColor = Color.MistyRose
            Button7.Enabled = False
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Task.Run(AddressOf GetNetworkComputers)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Task.Run(Sub() AddPCToList(IPAddress.Parse(TextBox1.Text)))
    End Sub

    Private Sub SearchPCForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub SearchPCForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Me.DialogResult <> DialogResult.OK Then
            For Each PCItem As ListViewItem In ListView1.Items
                TryCast(PCItem.Tag, Link).Close()
            Next
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        For Each item As ListViewItem In ListView1.Items
            If item.Checked = True Then
                SelectedList.Add(New ConnectionStruct With {.PCName = item.Text, .PCAddress = item.SubItems(1).Text, .LinkObject = item.Tag})
            Else
                TryCast(item.Tag, Link).Close()
            End If
        Next
        Me.DialogResult = DialogResult.OK
    End Sub
End Class

