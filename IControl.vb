Imports System.Windows.Forms
Namespace Commands
    Public Interface IControl
        Property OutputBox As TextBox
        Property InputBox As TextBox

        Property OkButton As Button

        Property CancelButton As Button

        Property CurrentCommand As String

        Property Form1Ref As Object
        Sub CMDEXE(Optional SubCommand As String = "")

    End Interface
End Namespace