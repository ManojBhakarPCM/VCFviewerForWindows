
Imports System.Drawing.Drawing2D

Public Class Form1
    Dim FNfont As Font
    Dim watermarkFont As Font
    Dim v As List(Of vCard)
    Private Sub displayAll()
        p.Controls.Clear()
        If v Is Nothing Then Exit Sub
        For Each vv As vCard In v
            showContact(vv.FN, Replace(vv.Tel, ",", vbCrLf), "")
            Application.DoEvents()
        Next
        labInfo.Text = "total contacts: " & p.Controls.Count
    End Sub
    Private Sub search(what As String)
        p.Controls.Clear()
        If v Is Nothing Then Exit Sub
        For Each vv As vCard In v
            If InStr(vv.FN.ToLower, what) > 0 Then
                showContact(vv.FN, Replace(vv.Tel, ",", vbCrLf), "")
                Application.DoEvents()
            End If
        Next
        labInfo.Text = "total contacts: " & p.Controls.Count
    End Sub

    Private Sub showContact(FN As String, TelNo As String, picdata As String)
        Dim pb As New PictureBox
        pb.BackgroundImage = My.Resources.pb_background
        pb.BackgroundImageLayout = ImageLayout.Stretch
        pb.Width = p.Width - 10
        pb.Height = 100
        '-------------DP------------------------------
        Dim dp As New PictureBox
        dp.Height = 60
        dp.Width = 60
        dp.BorderStyle = BorderStyle.None
        dp.BackColor = Color.White
        dp.BackgroundImage = My.Resources.anonymous_user
        dp.BackgroundImageLayout = ImageLayout.Stretch
        dp.Location = New Point(20, 20)
        pb.Controls.Add(dp)
        '-------------FN label------------------------
        Dim fnL As New Label
        fnL.Width = pb.Width - dp.Width - 45
        fnL.Height = FNfont.Height
        fnL.Text = FN
        fnL.Font = FNfont

        fnL.Location = New Point(dp.Left + dp.Width + 10, dp.Top)
        pb.Controls.Add(fnL)
        '------------TEL label-----------------------
        Dim tel As New Label
        tel.AutoSize = False
        tel.Height = 40
        tel.Text = TelNo
        tel.ForeColor = Color.Teal
        tel.Location = New Point(dp.Left + dp.Width + 10, fnL.Top + fnL.Height)
        pb.Controls.Add(tel)
        '--------------Making an entry---------------
        pb.Location = New Point(0, 95 * p.Controls.OfType(Of PictureBox)().Count)
        p.Controls.Add(pb)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FNfont = New Font("Segoe UI", 12, FontStyle.Bold)
        watermarkFont = New Font("Segoe UI", 12, FontStyle.Italic)
        Dim arguments As String() = Environment.GetCommandLineArgs()
        menu.BackColor = Color.FromArgb(255, Color.White)
        If arguments.Length < 2 Then Exit Sub
        v = ExtractvCardsFromFile(arguments(1))
        Me.Text = Me.Text & " " & v.Count
        'v = ExtractvCardsFromFile("G:\Uncat\data\2016-08-31_220550.vcf")
        displayAll()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "search contacts" Then Exit Sub
        Dim what As String = TextBox1.Text.ToLower

        If what.Length > 1 Then
            search(what)
        ElseIf what.Length = 0 Then
            If p.Controls.Count <> 0 AndAlso p.Controls.Count = v.Count Then Exit Sub
            displayAll()
        End If

    End Sub

    Private Sub makeDefaultCSVViewr()
        My.Computer.Registry.ClassesRoot.CreateSubKey(".vcf").SetValue("", "VCF", Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.ClassesRoot.CreateSubKey("VCF\shell\open\command").SetValue("", Application.ExecutablePath & " ""%l"" ", Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.ClassesRoot.CreateSubKey("VCF\DefaultIcon").SetValue("", Application.ExecutablePath & ",1", Microsoft.Win32.RegistryValueKind.ExpandString)
        MsgBox("OK, this program will open when you click vcf file")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        menu.Visible = Not menu.Visible
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub menu_MouseLeave(sender As Object, e As EventArgs) Handles menu.MouseLeave
        menu.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.ShowDialog()

        Dim fp As String = OpenFileDialog1.FileName
        If Not fp = "" Then
            Dim filename As String = IO.Path.GetFileNameWithoutExtension(fp)
            Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "/" & filename & ".vcf"
            txtFileToVcf(csvpath:=fp, vcardPath:=desktopPath)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        makeDefaultCSVViewr()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim desktopPath As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "/" & "ExportedContacts.txt"
        Dim out As String = ""
        If v Is Nothing Then Exit Sub
        If (v.Count > 0) Then
            For Each vv As vCard In v
                out &= vv.FN & ":" & vv.Tel & vbCrLf
            Next
            IO.File.WriteAllText(desktopPath, out)
            MsgBox("Exporting completed!")
        Else
            MsgBox("No Vcards in current list")
        End If
    End Sub

    Private Sub TextBox1_LostFocus(sender As Object, e As EventArgs) Handles TextBox1.LostFocus
        If TextBox1.Text = "" Then
            TextBox1.Font = watermarkFont
            TextBox1.Text = "search contacts"
            TextBox1.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub TextBox1_GotFocus(sender As Object, e As EventArgs) Handles TextBox1.GotFocus
        TextBox1.Font = FNfont
        TextBox1.Text = ""
        TextBox1.ForeColor = Color.Black
    End Sub
End Class
