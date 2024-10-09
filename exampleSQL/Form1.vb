
Imports MySql.Data.MySqlClient

Public Class Form1
    Private Sub btnEnter_Click(sender As Object, e As EventArgs) Handles btnEnter.Click

        Dim connectionString As String = "Server=localhost;Database=dbUser;User ID=root;Password=12Yellow34!;"

        ' captured username / password gtom the form
        Dim enteredUsername As String = txtUsername.Text
        Dim enteredPassword As String = txtPassword.Text

        ' SQL Query to check for users from tblUser table 
        ' The query is vulnerable to SQL Injection as there are no prepared statements
        ' ExampleS:  
        '    ' OR 1=1;
        '    ' OR 1=1 -- -
        '    ' OR 1=1; DELETE FROM tbluser;
        '    ' OR 1=1; DROP TABLE 
        '    ' OR 1=1; DROP DATABASE dbuser;
        '    ' OR 1=1; UPDATE tbluser SET passwd = 'newpassword' WHERE username = 'admin';

        ' Any number of SQL statements could be injected once the dtaabase is vulnerable

        Dim query As String = "SELECT username, passwd FROM tbluser WHERE username = '" & enteredUsername & "' AND passwd = '" & enteredPassword & "'"

        ' Create a MySQL connection object
        Using conn As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                conn.Open()

                Using cmd As New MySqlCommand(query, conn)
                    ' Execute the query and read the results
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            ' Valid user found
                            While reader.Read()
                                MessageBox.Show("User authenticated: " & reader("username").ToString())
                                frmMenu.Show()
                                Me.Hide()
                            End While
                        Else
                            ' No valid user found
                            MessageBox.Show("Invalid username or password")
                        End If
                    End Using
                End Using

            Catch ex As MySqlException
                ' Handle any database errors
                Console.WriteLine("Error: " & ex.Message)
            Finally
                ' Close the connection
                conn.Close()
            End Try
        End Using

        ' Keep the console open
        Console.ReadLine()

    End Sub

    Function ValidateUser(ByVal enteredUsername As String, ByVal username As String, ByVal enteredPassword As String, ByVal passwd As String) As Boolean
        ' Example logic: let's say a valid user has "admin" as a username

        If enteredUsername = username And enteredPassword = passwd Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
