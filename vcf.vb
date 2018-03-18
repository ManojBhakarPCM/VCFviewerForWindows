Imports Microsoft.VisualBasic
Public Structure vCard
    Dim VERSION As String
    Dim N As String
    Dim FN As String
    Dim Tel() As String
    Dim info() As String
End Structure
Public Class Class1
    Public Shared Function ExtractvCardsFromFile(filepath As String) As vCard()
        Dim fileLines() As String = System.IO.File.ReadAllLines(filepath)
        Dim totLines = fileLines.Length - 1
        Dim line As String
        Dim fields As String
        Dim fieldsAr() As String
        Dim valueAr() As String
        Dim values As String
        Dim tmpFieldsValueAR() As String
        Dim vcards(5000) As vCard
        Dim counter As Integer = 0
        Dim vc As vCard
        For i = 0 To totLines
            line = fileLines(i)
            If (line.Equals("VCARD:BEGIN")) Then
                vc.FN = ""
                vc.Tel = Nothing
                vc.info = Nothing
            ElseIf (line.Equals("VCARD:END")) Then
                vcards(counter) = vc
                counter += 1
            Else
                tmpFieldsValueAR = split(line, ":")
                fields = tmpFieldsValueAR(0)
                values = tmpFieldsValueAR(1)
                fieldsAr = split(fields, ";")
                valueAr = split(values, ";")
                For Each f As String In fieldsAr
                    If f.Equals("FN") Then
                        vc.FN = values
                    ElseIf f.Equals("VERSION") Then
                        vc.VERSION = values
                    ElseIf f.Equals("N") Then
                        Dim n As String = ""
                        For Each itm In valueAr
                            n &= itm & " "
                        Next
                        vc.N = n
                    ElseIf f.Equals("TEL") Then
                        ReDim vc.Tel(1 To vc.Tel.Length + 1)
                        vc.Tel(vc.Tel.Length) = values
                    End If
                Next
            End If
        Next
        Return vcards
    End Function
    Public Sub makeFileFromVCFcards(filepath As String, vcfs() As vcfCard)

    End Sub
    Public Sub log(line As String)
        Form1.listbox1.items.add(line)
    End Sub
End Class
