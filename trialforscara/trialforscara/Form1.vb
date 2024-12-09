Public Class Form1
    Const home1 As Integer = 50
    Dim currentStep As Double = 50
    Dim lowerLimit As Integer = 0
    Dim upperLimit As Integer = 5000

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'For default values
        ComboBox1.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        ComboBox5.SelectedIndex = 0
        Mode.SelectedIndex = 0
        cs1.Text = home1


        'Serialport
        SerialPort1.PortName = "COM6"
        SerialPort1.BaudRate = 9600
        SerialPort1.Open()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim input_steps As Integer = Convert.ToInt32(NumericUpDown1.Text)
        Dim direction1 As String = ComboBox1.Text
        Dim modes As String = Mode.Text
        Dim delay1 As Integer = NumericUpDown2.Text

        Dim trueSteps1 As Double = StepModeConverter(input_steps, modes)
        Dim result1 = stepCounter(lowerLimit, upperLimit, currentStep, trueSteps1, direction1)
        Dim addedSteps = result1.Steps
        Dim updatedSteps = result1.updatedSteps
        currentStep = updatedSteps

        r1.Text = updatedSteps
        l1.Text = upperLimit - updatedSteps
        Dim dir1 = directionConverter(direction1)
        Dim command As String = "M1:" & addedSteps & "," & "Dir1:" & dir1
        SerialPort1.WriteLine(command)
    End Sub

    Function StepModeConverter(addedSteps As Integer, mode As String) As Double
        Dim trueSteps As Double
        Select Case mode
            Case "FULL_STEP"
                trueSteps = addedSteps
            Case "HALF_STEP"
                trueSteps = addedSteps / 2
            Case "QUARTER_STEP"
                trueSteps = addedSteps / 4
            Case "EIGHTH_STEP"
                trueSteps = addedSteps / 8
            Case "SIXTEENTH_STEP"
                trueSteps = addedSteps / 16
            Case Else
                MessageBox.Show("Mode not within the range")
        End Select
        Return trueSteps
    End Function
    Function stepCounter(lowerLimit As Integer, upperLimit As Integer, currentSteps As Double, addedSteps As Double, direction As String) As (Steps As Double, updatedSteps As Double)
        Dim val As Integer
        If direction = "RIGHT" Then
            val = currentSteps + addedSteps
            If val > upperLimit Then
                MessageBox.Show("Value too high")
                Return (0, currentSteps)
            End If
            Return (addedSteps, currentSteps + addedSteps)
        ElseIf direction = "LEFT" Then
            val = currentSteps - addedSteps
            If val < lowerLimit Then
                MessageBox.Show("Value too high")
                Return (0, currentSteps)
            End If
            Return (addedSteps, currentSteps - addedSteps)
        End If
        Return (0, 0)
    End Function

    Function directionConverter(direction As String) As Integer
        Return If(direction = "RIGHT", 1, 2)
    End Function
End Class
