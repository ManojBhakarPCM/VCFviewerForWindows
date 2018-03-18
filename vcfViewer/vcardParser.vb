Module vcardParser
    Public Structure vCard
        Dim VERSION As String
        Dim N As String
        Dim FN As String
        Dim Tel As String
        Dim info As String
    End Structure
    Public Function ExtractvCardsFromFile(filepath As String) As List(Of vCard)
        Dim fileLines() As String = System.IO.File.ReadAllLines(filepath)
        Dim totLines = fileLines.Length - 1
        Dim line As String
        Dim fields As String
        Dim fieldsAr() As String
        Dim valueAr() As String
        Dim values As String
        Dim tmpFieldsValueAR() As String

        Dim counter As Integer = 0
        Dim vcards As New List(Of vCard)
        Dim itel As String = ""
        Dim iInfo As String
        Dim vc As New vCard
        For i = 0 To totLines
            line = fileLines(i)
            If Not (InStr(line, ":") > 0) Then Continue For
            If (line.Equals("BEGIN:VCARD")) Then
                vc.FN = ""
                itel = ""
                iInfo = ""
            ElseIf (line.Equals("END:VCARD")) Then
                vc.Tel = Mid(itel, 2)
                vcards.Add(vc)
            Else

                tmpFieldsValueAR = Split(line, ":")
                    fields = tmpFieldsValueAR(0)
                    values = tmpFieldsValueAR(1)
                    fieldsAr = Split(fields, ";")
                    valueAr = Split(values, ";")
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
                            itel &= "," & values
                        End If
                    Next
                End If

        Next
        Return vcards
    End Function

    Public Sub log(line As String)
        Form1.ListBox1.Items.Add(line)
    End Sub
    Public Sub txtFileToVcf(csvpath As String, vcardPath As String)
        'txt format- name:tel,tel,tel,..... (no header row)
        Dim outLines As New List(Of String)
        Dim lines() As String = IO.File.ReadAllLines(csvpath)
        For Each line As String In lines
            If (InStr(line, ":") > 0) Then
                Dim arFNTEL() As String = Split(line, ":")
                outLines.Add(createVCF(arFNTEL(0), arFNTEL(1)))
            End If
        Next
        IO.File.WriteAllLines(vcardPath, outLines.ToArray)
    End Sub
    Public Function createVCF(FN As String, tel As String) As String
        'Add Header,Version,N,FN,TEL,Footer
        Dim out As String = "BEGIN:VCARD" & vbCrLf & "VERSION:3.0" & vbCrLf & "N:"
        Dim nAr() As String = Split(FN, " ")
        Dim count As Integer = nAr.Count
        For i = count - 1 To 0 Step -1
            out &= nAr(i) & ";"
        Next
        If Not count > 4 Then
            out &= StrDup(4 - count, ";")
        End If
        out &= vbCrLf & "FN:" & FN
        If InStr(tel, ",") > 0 Then
            Dim telAr() As String = Split(tel, ",")
            'out &= "TEL;TYPE=CELL:" & tel(0)
            For i = 0 To telAr.Count - 1
                out &= vbCrLf & "TEL;TYPE=CELL:" & telAr(i)
            Next
        Else
            out &= vbCrLf & "TEL;TYPE=CELL:" & tel
        End If
        out &= vbCrLf & "END:VCARD"
        Return out
    End Function
End Module
