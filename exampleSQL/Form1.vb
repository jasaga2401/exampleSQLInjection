
' uses a user with changed priviledges to limit their access to the database

Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports MySql.Data.MySqlClient

Public Class Form1
    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click

        Dim connectionString As String = "Server=localhost;Database=dbuser;User ID=newuser;Password=12Yellow34!"

        ' Capture the username and password from the form
        Dim username As String = txtUsername.Text
        Dim passwd As String = txtPassword.Text

        ' ----------------- SQL Protection 1 ----------------- (sanitised input)
        ' Sanitising against text inputs protects from SQL injection attacks

        ' sanitising the text inputs
        username = username.Replace("'", "").Replace(";", "")
        passwd = passwd.Replace("'", "").Replace(";", "")

        ' Using regular expressions to sanitise the text inputs
        If Not System.Text.RegularExpressions.Regex.IsMatch(username, "^[a-zA-Z0-9]+$") Then
            MessageBox.Show("Invalid characters in username")
            Exit Sub
        End If

        If Not System.Text.RegularExpressions.Regex.IsMatch(passwd, "^[a-zA-Z0-9]+$") Then
            MessageBox.Show("Invalid characters in password")
            Exit Sub
        End If


        ' ----------------- SQL Protection 2 ----------------- (parameterised queries)
        ' SQL Query with parameters
        ' Dim query As String = "SELECT * FROM tblUser WHERE username = @username AND password = @password"

        ' Use of stored procedure with parameters to execute a query
        Dim query As String = "CALL sp_ValidateUser1(@username, @password)"

        ' Create a new MySQL connection
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                ' Use a MySQL command to execute the query
                Using cmd As New MySqlCommand(query, conn)

                    ' Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@password", passwd)

                    ' Execute the query and read the results
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            ' Valid user found
                            While reader.Read()
                                MessageBox.Show("User authenticated: " & reader("username").ToString())
                            End While
                        Else
                            ' No valid user found
                            MessageBox.Show("Invalid username or password")
                        End If
                    End Using
                End Using

            Catch ex As MySqlException
                MessageBox.Show("Database error: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using

    End Sub



End Class
