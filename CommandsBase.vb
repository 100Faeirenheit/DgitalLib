Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Net
Imports System.Data.SqlClient
Imports System.Net.Sockets
Namespace Commands
    MustInherit Class CommandsBase
        Public Name As String
        Public Descrption As String
        'Public cmdAction As System.Action(Of Object)
        Public cmdList As Dictionary(Of String, Object)
        Public Property CurrentCmdName As String
        Public Property DescriptionProperty As String

        Public Property InputRequired As Boolean
        Sub New()
            'Name = String.Empty
            'Descrption = String.Empty
            'cmdAction = New Action(Of Object)(AddressOf PerformAction)
            cmdList = New Dictionary(Of String, Object)

            'CurrentCmdName = String.Empty
            'DescriptionProperty = String.Empty
        End Sub

        Public Sub PerformAction(CMDname As String)
            For Each name As String In cmdList.Keys


                cmdList(CMDname).CMDEXE()

            Next
        End Sub

    End Class
    Class ShowDescription
        Inherits CommandsBase
        Implements IControl

        Public Property OutputBox As TextBox
        Public Property InputBox As TextBox
        Public Property form1 As Form

        Private Property IControl_OutputBox As TextBox Implements IControl.OutputBox
            Get
                Return Me.OutputBox
            End Get
            Set(value As TextBox)
                value = Me.OutputBox
            End Set
        End Property

        Private Property IControl_InputBox As TextBox Implements IControl.InputBox
            Get
                Return Me.InputBox
            End Get
            Set(value As TextBox)
                value = Me.InputBox
            End Set
        End Property

        Public Property OkButton As Button Implements IControl.OkButton
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Button)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property CancelButton As Button Implements IControl.CancelButton
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Button)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property CurrentCommand As String Implements IControl.CurrentCommand
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Form1Ref As Object Implements IControl.Form1Ref
            Get
                Return Me.form1
            End Get
            Set(value As Object)
                value = Me.form1
            End Set
        End Property

        Sub New()

            Me.Name = "Show Description"
            Me.Descrption = "Show CMD Description"
            Me.InputRequired = False
            cmdList = New Dictionary(Of String, Object)
            cmdList.Add("Show Description", Me)

            cmdList.Add("SQL", New SqlServe)
            cmdList("SQL").CurrentCommand = Me.CurrentCmdName
            cmdList.Add("TCPServer", New TCPServer)
            cmdList("TCPServer").CurrentCommand = Me.CurrentCmdName
        End Sub

        Public Sub CMDEXE(Optional SubCommand As String = "")
            DescriptionProperty = Me.Descrption

            OutputBox.AppendText(cmdList(CurrentCmdName).Descrption & Environment.NewLine)


        End Sub

        Private Sub IControl_CMDEXE(Optional SubCommand As String = "") Implements IControl.CMDEXE
            Throw New NotImplementedException()
        End Sub
    End Class

    Class SqlServe
        Inherits CommandsBase
        Implements IControl


        Property CurrentCommand As String Implements IControl.CurrentCommand


        Dim conNfo As String
        Dim sqlCon As SqlConnection
        Dim commandSql As SqlCommand
        Public Property Form1Ref As Object Implements IControl.Form1Ref
        Public Property OutputBox As TextBox Implements IControl.OutputBox


        Public Property InputBox As TextBox Implements IControl.InputBox


        Public Property OkButton As Button Implements IControl.OkButton
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Button)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property CancelButton As Button Implements IControl.CancelButton
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Button)
                Throw New NotImplementedException()
            End Set
        End Property
        Dim findallusers As String
        Dim finduser As String
        Dim id As String
        Sub New()

            Me.Name = "SQL"
            Me.Descrption = "Access Sql Server: ShowLogin to show * from login. Example Type EXE click ok,Type SQL click ok,Type ShowLogin Click ok."

            conNfo = "Data Source=T510-PC\SQLSERV;Initial Catalog=toj;Persist Security Info=True;User ID=sa;Password=mikester"


            sqlCon = New SqlConnection(conNfo)
            findallusers = "Select * From auth"
            finduser = "SELECT id FROM auth where id=" & id
            commandSql = New SqlCommand(findallusers, sqlCon)
            CurrentCommand = String.Empty

            'CurrentCommand = form1.CommandCurently
            'Me.CurrentCommand = Form1Ref.CommandCurrently
        End Sub

        Public Sub CMDEXE(Optional SubCommand As String = "") Implements IControl.CMDEXE
            'OutputBox.AppendText("Type a query name." & Environment.NewLine)
            id = 0
            If SubCommand = "ShowLogin" Then
                sqlCon.Open()

                commandSql.ExecuteNonQuery()
                Dim sqlRead As SqlDataReader = commandSql.ExecuteReader()

                While sqlRead.Read
                    OutputBox.AppendText(String.Format("{0},{1},{2},{3}", sqlRead(0), sqlRead(1), sqlRead(2), sqlRead(3) & Environment.NewLine))
                End While
                sqlCon.Close()
            End If

            If SubCommand = "FindUser" Then
                sqlCon.Open()
                commandSql.CommandText = finduser
                'id = SubCommand.Split(":").Contains(id)
                Dim sqlRead As SqlDataReader = commandSql.ExecuteReader

                While sqlRead.Read
                    OutputBox.AppendText(String.Format("{0}", sqlRead(0)))
                End While
            End If
        End Sub
    End Class
    Class TCPServer
        Inherits CommandsBase
        Implements IControl

        Dim tcpListen As TcpListener
        Dim tcpClient As TcpClient
        Dim port As Integer
        Const listen As String = "Listening"
        Sub New()
            Me.Name = "TCPServer"
            Me.Descrption = "Connect to Digital Whirlwind"
            port = 8800
        End Sub
        Function initTCP() As Boolean
            Try
                tcpListen = TcpListener.Create(port)
                OutputBox.AppendText("Server start" & Environment.NewLine)

                tcpListen.Start()

                Dim bytes(1024) As Byte
                Dim data As String = Nothing

                While listen = "Listening"
                    OutputBox.AppendText("Waiting for connection" & Environment.NewLine)
                    Dim client As TcpClient = tcpListen.AcceptTcpClient
                    OutputBox.AppendText("Connected!" & Environment.NewLine)
                    data = Nothing

                    Dim stream As NetworkStream = client.GetStream

                    Dim i As Int32

                    i = stream.Read(bytes, 0, bytes.Length)

                    While (i <> 0)
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i)

                        data = data.ToUpper
                        Dim msg As Byte() = Text.Encoding.ASCII.GetBytes(data)

                        stream.Write(msg, 0, msg.Length)
                        OutputBox.AppendText("Sent: {0}" & data & Environment.NewLine)

                        i = stream.Read(bytes, 0, bytes.Length)

                        client.Close()
                    End While
                End While
            Catch ex As SocketException
                OutputBox.AppendText("Socket Exception: {0}" & ex.Message & Environment.NewLine)
            Finally
                tcpListen.Stop()
            End Try

            Return True
        End Function

        Public Property CancelButton As Button Implements IControl.CancelButton
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Button)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property CurrentCommand As String Implements IControl.CurrentCommand
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property Form1Ref As Object Implements IControl.Form1Ref
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Object)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property InputBox As TextBox Implements IControl.InputBox
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As TextBox)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property OkButton As Button Implements IControl.OkButton
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Button)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Property OutputBox As TextBox Implements IControl.OutputBox
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As TextBox)
                Throw New NotImplementedException()
            End Set
        End Property

        Public Sub CMDEXE(Optional SubCommand As String = "") Implements IControl.CMDEXE
            If SubCommand = "TCPINIT" Then
                initTCP()
            End If
        End Sub
    End Class
    Public Class CommandFace

        Dim show_Description As ShowDescription

        Public Property OutputBox As TextBox
        Public Property InputBox As TextBox
        Public Property currentCMD() As String
        Public Property form1 As Form
        Public Property SubCommand As String
        Sub New()
            show_Description = New ShowDescription
            show_Description.OutputBox = Me.OutputBox
            show_Description.InputBox = Me.InputBox



            show_Description.CurrentCmdName = Me.currentCMD

        End Sub

        Public Sub SetTextBoxes()
            For Each cmd As Object In show_Description.cmdList.Values
                cmd.OutputBox = Me.OutputBox
                cmd.InputBox = Me.InputBox
            Next

        End Sub
        Public Sub SetForm1Reference()

            For Each cmd As Object In show_Description.cmdList.Values
                cmd.Form1Ref = Me.form1
            Next
        End Sub
        Public Function EXECommand(cmdName As String) As String
            show_Description.CurrentCmdName = cmdName
            Me.currentCMD = cmdName
            For Each name As String In show_Description.cmdList.Keys
                If name = cmdName Then
                    'OutputBox.AppendText(cmdName & "is executing" & Environment.NewLine)
                    show_Description.cmdList(cmdName).CMDEXE(SubCommand)

                End If

            Next
            Return "0"
        End Function

        ' Public Function CheckCmdProperty(name As String) As String
        'Try
        'Try
        'Me.currentCMD = name
        'For Each cmd As Object In show_Description.cmdList
        '    show_Description.PerformAction(InputBox.Text)
        'Next




        'Catch argNull As ArgumentNullException
        '  Debug.Print(argNull.Message)
        'End Try
        'Catch keyex As KeyNotFoundException
        ' Debug.Print("No key found!")
        'End Try
        'Return "0"
        ' End Function

        'Public Function CheckCmdProperty(value As String) As String
        'show_Description.cmdAction.Invoke(show_Description)
        'Me.show_Description.CurrentCmdName = value
        'Return Me.show_Description.CurrentCmdName
        'End Function

        Public Function ListCommands(Outputbox As TextBox, Inputbox As TextBox) As String
            For Each cmd As Object In show_Description.cmdList.Values
                Outputbox.AppendText(cmd.name & Environment.NewLine)
            Next
            Return "0"
        End Function
        Public Function showDescription(Outbox As TextBox, currentCommand As String) As String
            Try
                For Each cmd As Object In show_Description.cmdList.Values



                    Outbox.AppendText(show_Description.cmdList(currentCommand).Descrption & Environment.NewLine)






                Next
            Catch keyNot As KeyNotFoundException
                Debug.Print("Key not exist.")
            End Try
            Return "0"
        End Function
    End Class
End Namespace
