Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class XMLLibrary

    Public Shared Function ToText(ByVal obj As Object)
        Dim ns As New XmlSerializerNamespaces()
        ns.Add("", "")
        Dim settings As New XmlWriterSettings() With {.OmitXmlDeclaration = True, .Indent = False}
        Dim XMLser As New XmlSerializer(obj.GetType)
        Dim TextStream As New StringWriter
        XMLser.Serialize(TextStream, obj, ns)
        Return TextStream.ToString
    End Function
    Public Shared Function ToObject(ByVal text As String, ByVal type As Type)
        Dim XMLser As New XmlSerializer(type)
        Dim TextStream As New StringReader(text)
        Return XMLser.Deserialize(TextStream)
    End Function
    Public Shared Function TryObjects(ByVal text As String, ByVal possibleTypes() As Type)
        Dim obj As Object = Nothing
        For Each type As Type In possibleTypes
            Try
                Dim XMLser As New XmlSerializer(type)
                Dim TextStream As New StringReader(text)
                obj = XMLser.Deserialize(TextStream)
                Exit For
            Catch ex As Exception

            End Try

        Next
        Return obj
    End Function
End Class
