Public Class Utils
    Public Shared Function BytesToString(ByVal byteCount As Long) As String
        Dim suf As String() = {"B", "KB", "MB", "GB", "TB", "PB", "EB"}
        If byteCount = 0 Then Return "0" & suf(0)
        Dim bytes As Long = Math.Abs(byteCount)
        Dim place As Integer = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)))
        Dim num As Double = Math.Round(bytes / Math.Pow(1024, place), 3)
        Return (Math.Sign(byteCount) * num).ToString() & " " & suf(place)
    End Function
End Class
