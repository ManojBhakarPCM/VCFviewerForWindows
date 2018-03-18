Public Class panelButton
    Inherits Windows.Forms.Button
    Public Sub New()
    End Sub

    Private Sub TheCrazyProgrammersBtn_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        Me.BackColor = Color.Blue
    End Sub

    Private Sub TheCrazyProgrammersBtn_MouseHover(sender As Object, e As EventArgs) Handles Me.MouseHover
        Me.BackColor = Color.LightBlue
    End Sub

    Private Sub TheCrazyProgrammersBtn_MouseLeave(sender As Object, e As EventArgs) Handles Me.MouseLeave
        Me.BackColor = Color.Blue
    End Sub

    Private Sub TheCrazyProgrammersBtn_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        Me.BackColor = Color.Gainsboro
    End Sub

End Class
