Imports System.Data.SqlClient
Imports System.Configuration

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'CADENA DE CONEXION
        Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"

        ' Crear una nueva conexión a SQL Server
        Dim connection As New SqlConnection(connectionString)

        Try
            ' Abrir la conexión
            connection.Open()
            ' La conexión se ha abierto exitosamente, mostrar un mensaje de éxito
            'MessageBox.Show("Conexión exitosa a la base de datos.")

            'LUEGO CREAR UN COMANDO PARA PODER EJECUTAR COMANDOS
            Dim comando As New SqlCommand("SP_MTTO_USUARIO", connection)

            'AGREGAR DE QUE TIPO ES EL COMANDO
            comando.CommandType = CommandType.StoredProcedure

            'AGREGAR LOS VALORES A ENVIAR AL SP
            comando.Parameters.AddWithValue("@opcion", "0")

            'EJECUTAR UN ADAPTADOR
            Dim adaptador As New SqlDataAdapter(comando)

            'GUARDAR EN UNA TABLA
            Dim tabla As New DataTable
            adaptador.Fill(tabla)

            DGVUSUARIO.DataSource = tabla

            'MOSTRAMOS UN VALOR
            'MessageBox.Show(Convert.ToString(tabla.Rows(0)(1)))

            ' Cerrar la conexión
            connection.Close()
        Catch ex As Exception
            ' Manejar errores de conexión
            MessageBox.Show("Error al conectar a la base de datos: " + ex.Message)
        Finally
            ' Asegurarse de cerrar la conexión al salir del bloque Try-Catch
            connection.Close()
        End Try

    End Sub


    Private Sub DGVUSUARIO_MouseClick(sender As Object, e As MouseEventArgs) Handles DGVUSUARIO.MouseClick, Button1.MouseClick
        Dim rowIndex As Integer = DGVUSUARIO.CurrentRow.Index

        If rowIndex <> -1 Then
            'obtenemos datos
            Dim id As String = DGVUSUARIO.Rows(rowIndex).Cells(0).Value.ToString()
            Dim usuario As String = DGVUSUARIO.Rows(rowIndex).Cells(1).Value.ToString()
            Dim contra As String = DGVUSUARIO.Rows(rowIndex).Cells(2).Value.ToString()
            Dim intentos As String = DGVUSUARIO.Rows(rowIndex).Cells(3).Value.ToString()
            Dim nivel As String = DGVUSUARIO.Rows(rowIndex).Cells(4).Value.ToString()

            If ((String.IsNullOrWhiteSpace(id) And String.IsNullOrWhiteSpace(usuario) And String.IsNullOrWhiteSpace(contra) And String.IsNullOrWhiteSpace(intentos) And String.IsNullOrWhiteSpace(nivel)) = False) Then
                'mostramos datos
                txtUsu.Text = usuario
                txtCon.Text = contra
                numInt.Value = Convert.ToDecimal(intentos)
                numNiv.Value = Convert.ToDecimal(nivel)
                btnGuardar.Enabled = False
                btnEditar.Enabled = True
                btnEliminar.Enabled = True
            Else Limpiar()
            End If
        End If



    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'CADENA DE CONEXION
        Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        'Dim connection As New SqlConnection(connectionString)

        Mostrar(connectionString)
    End Sub

    Private Sub Mostrar(ByVal con As String)
        Limpiar()
        'CADENA DE CONEXION
        'Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        Dim connection As New SqlConnection(con)

        Dim comando As New SqlCommand("SP_MTTO_USUARIO", connection)

        'AGREGAR DE QUE TIPO ES EL COMANDO
        comando.CommandType = CommandType.StoredProcedure

        'AGREGAR LOS VALORES A ENVIAR AL SP
        comando.Parameters.AddWithValue("@opcion", "0")

        'EJECUTAR UN ADAPTADOR
        Dim adaptador As New SqlDataAdapter(comando)

        'GUARDAR EN UNA TABLA
        Dim tabla As New DataTable
        adaptador.Fill(tabla)

        DGVUSUARIO.DataSource = tabla
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        'CADENA DE CONEXION
        Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        'Dim connection As New SqlConnection(connectionString)
        If txtUsu.Text = "" Then
            MessageBox.Show("Ingresar usuario")
            Return
        ElseIf txtCon.Text = "" Then
            MessageBox.Show("Ingresar contrasenia")
            Return
        ElseIf numInt.Value <= 0 Then
            MessageBox.Show("El numero de intentos debe de ser diferente y mayor a 0")
            Return
        ElseIf numNiv.Value <= 0 Then
            MessageBox.Show("El numero de nivel debe de ser diferente y mayor a 0")
            Return
        Else Guardar(connectionString, txtUsu.Text, txtCon.Text, numInt.Value.ToString(), numNiv.Value.ToString())
        End If



    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        'CADENA DE CONEXION
        Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        'Dim connection As New SqlConnection(connectionString)
        Dim VALOR As String = DGVUSUARIO.Rows(DGVUSUARIO.CurrentRow.Index).Cells(0).Value.ToString()
        If String.IsNullOrWhiteSpace(VALOR) Then
            MessageBox.Show("Seleccionar una fila que no este vacia")
            Return
        ElseIf txtUsu.Text = "" Then
        MessageBox.Show("Ingresar usuario")
            Return
        ElseIf txtCon.Text = "" Then
            MessageBox.Show("Ingresar contrasenia")
            Return
        ElseIf numInt.Value <= 0 Then
            MessageBox.Show("El numero de intentos debe de ser diferente y mayor a 0")
            Return
        ElseIf numNiv.Value <= 0 Then
            MessageBox.Show("El numero de nivel debe de ser diferente y mayor a 0")
            Return
        Else Actualizar(connectionString, txtUsu.Text, txtCon.Text, numInt.Value.ToString(), numNiv.Value.ToString(), VALOR)
        End If

    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        'CADENA DE CONEXION
        Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        'Dim connection As New SqlConnection(connectionString)
        Dim VALOR As String = DGVUSUARIO.Rows(DGVUSUARIO.CurrentRow.Index).Cells(0).Value.ToString()
        If String.IsNullOrWhiteSpace(VALOR) Then
            MessageBox.Show("Seleccionar una fila que no este vacia")
            Return
        Else Eliminar(connectionString, VALOR)
        End If

    End Sub

    Private Sub Guardar(ByVal con As String, ByVal usu As String, ByVal contra As String, ByVal inte As String, ByVal niv As String)

        'CADENA DE CONEXION
        'Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        Dim connection As New SqlConnection(con)

        Dim comando As New SqlCommand("SP_MTTO_USUARIO", connection)

        'AGREGAR DE QUE TIPO ES EL COMANDO
        comando.CommandType = CommandType.StoredProcedure

        'AGREGAR LOS VALORES A ENVIAR AL SP
        comando.Parameters.AddWithValue("@opcion", "1")
        comando.Parameters.AddWithValue("@usuario", usu)
        comando.Parameters.AddWithValue("@contrasena", contra)
        comando.Parameters.AddWithValue("@intentos", inte)
        comando.Parameters.AddWithValue("@nivelSeg", niv)

        'EJECUTAR UN ADAPTADOR
        Dim adaptador As New SqlDataAdapter(comando)

        'GUARDAR EN UNA TABLA
        Dim tabla As New DataTable
        adaptador.Fill(tabla)


        If tabla.Rows(0)(0).ToString() = "1" Then
            MessageBox.Show("El dato se guardo correctamente")
            Mostrar(con)
        Else MessageBox.Show("El dato no se guardo correctamente")
        End If

        'DGVUSUARIO.DataSource = tabla
    End Sub

    Private Sub Actualizar(ByVal con As String, ByVal usu As String, ByVal contra As String, ByVal inte As String, ByVal niv As String, ByVal id As String)

        'CADENA DE CONEXION
        'Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        Dim connection As New SqlConnection(con)

        Dim comando As New SqlCommand("SP_MTTO_USUARIO", connection)

        'AGREGAR DE QUE TIPO ES EL COMANDO
        comando.CommandType = CommandType.StoredProcedure

        'AGREGAR LOS VALORES A ENVIAR AL SP
        comando.Parameters.AddWithValue("@opcion", "2")
        comando.Parameters.AddWithValue("@usuario", usu)
        comando.Parameters.AddWithValue("@contrasena", contra)
        comando.Parameters.AddWithValue("@intentos", inte)
        comando.Parameters.AddWithValue("@nivelSeg", niv)
        comando.Parameters.AddWithValue("@id", id)

        'EJECUTAR UN ADAPTADOR
        Dim adaptador As New SqlDataAdapter(comando)

        'GUARDAR EN UNA TABLA
        Dim tabla As New DataTable
        adaptador.Fill(tabla)


        If tabla.Rows(0)(0).ToString() = "1" Then
            MessageBox.Show("El dato se actualizo correctamente")
            Mostrar(con)
        Else MessageBox.Show("El dato no se actualizo correctamente")
        End If
    End Sub

    Private Sub Eliminar(ByVal con As String, ByVal id As String)

        'CADENA DE CONEXION
        'Dim connectionString As String = "Data source=DESKTOP-B1JIUS0\SQLEXPRESS02; Initial Catalog=Prueba01;Integrated Security=True"
        ' Crear una nueva conexión a SQL Server
        Dim connection As New SqlConnection(con)

        Dim comando As New SqlCommand("SP_MTTO_USUARIO", connection)

        'AGREGAR DE QUE TIPO ES EL COMANDO
        comando.CommandType = CommandType.StoredProcedure

        'AGREGAR LOS VALORES A ENVIAR AL SP
        comando.Parameters.AddWithValue("@opcion", "3")
        comando.Parameters.AddWithValue("@id", id)

        'EJECUTAR UN ADAPTADOR
        Dim adaptador As New SqlDataAdapter(comando)

        'GUARDAR EN UNA TABLA
        Dim tabla As New DataTable
        adaptador.Fill(tabla)


        If tabla.Rows(0)(0).ToString() = "1" Then
            MessageBox.Show("El dato se elimino correctamente")
            Mostrar(con)
        Else MessageBox.Show("El dato no se elimino correctamente")
        End If
    End Sub

    Private Sub Limpiar()
        txtUsu.Text = ""
        txtCon.Text = ""
        numInt.Value = 0
        numNiv.Value = 0
        btnGuardar.Enabled = True
        btnEditar.Enabled = False
        btnEliminar.Enabled = False

    End Sub
End Class
