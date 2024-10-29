using pantallas_con_diseño_moderno.clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pantallas_con_diseño_moderno
{
    public partial class frmClientesD2 : Form
    {

        public frmClientesD2()
        {
            InitializeComponent();
            cargarClientes();
        }

        private void frmClientesD2_Load(object sender, EventArgs e)
        {
            DGVclientes.AutoGenerateColumns = false;
            DGVclientes.Columns.Clear();

            DataGridViewTextBoxColumn colid = new DataGridViewTextBoxColumn();
            colid.HeaderText = "ID";
            colid.Name = "colid";
            colid.DataPropertyName = "id";
            DGVclientes.Columns.Add(colid);

            DataGridViewTextBoxColumn colnombre = new DataGridViewTextBoxColumn();
            colnombre.HeaderText = "Cliente";
            colnombre.Name = "colcliente";
            colnombre.DataPropertyName = "Nombre";
            DGVclientes.Columns.Add(colnombre);

            DataGridViewTextBoxColumn coldocumento = new DataGridViewTextBoxColumn();
            coldocumento.HeaderText = "Documento";
            coldocumento.Name = "coldocumento";
            coldocumento.DataPropertyName = "documento";
            DGVclientes.Columns.Add(coldocumento);

            DataGridViewTextBoxColumn coldireccion = new DataGridViewTextBoxColumn();
            coldireccion.HeaderText = "Dirección";
            coldireccion.Name = "coldireccion";
            coldireccion.DataPropertyName = "direccion";
            DGVclientes.Columns.Add(coldireccion);

            DataGridViewTextBoxColumn coltelefono = new DataGridViewTextBoxColumn();
            coltelefono.HeaderText = "Télefono";
            coltelefono.Name = "coltelefono";
            coltelefono.DataPropertyName = "telefono";
            DGVclientes.Columns.Add(coltelefono);

            DataGridViewTextBoxColumn colemail = new DataGridViewTextBoxColumn();
            colemail.HeaderText = "Email";
            colemail.Name = "colemail";
            colemail.DataPropertyName = "Email";
            DGVclientes.Columns.Add(colemail);

            DataGridViewTextBoxColumn colfechamodifica = new DataGridViewTextBoxColumn();
            colfechamodifica.HeaderText = "Fecha Modifica";
            colfechamodifica.Name = "colfechamodifica";
            colfechamodifica.DataPropertyName = "fechamodifica";
            DGVclientes.Columns.Add(colfechamodifica);

            DataGridViewTextBoxColumn colusuariomodifico = new DataGridViewTextBoxColumn();
            colusuariomodifico.HeaderText = "Usuario Modifico";
            colusuariomodifico.Name = "colusuariomodifico";
            colusuariomodifico.DataPropertyName = "usuariomodifico";
            DGVclientes.Columns.Add(colusuariomodifico);

            DataGridViewButtonColumn coleditar = new DataGridViewButtonColumn();
            coleditar.HeaderText = "Editar";
            coleditar.Name = "ceditar";
            coleditar.Text = "Editar";
            coleditar.UseColumnTextForButtonValue = true;
            DGVclientes.Columns.Add(coleditar);

            DataGridViewButtonColumn colborrar = new DataGridViewButtonColumn();
            colborrar.HeaderText = "Borrar";
            colborrar.Name = "cborrar";
            colborrar.Text = "Borrar";
            colborrar.UseColumnTextForButtonValue = true;
            DGVclientes.Columns.Add(colborrar);

            DGVclientes.CellClick += detectorClick;
        }

        private void detectorClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DGVclientes.Columns["cborrar"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("¿Deseas eliminar este usuario?, no podrás volver a recuperarlo.", "Eliminar usuario", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    idCliente = (int)DGVclientes.Rows[e.RowIndex].Cells["colid"].Value;
                    eliminarCliente(idCliente);
                }

            }
            else if (e.ColumnIndex == DGVclientes.Columns["ceditar"].Index && e.RowIndex >= 0)
            {
                idCliente = (int)DGVclientes.Rows[e.RowIndex].Cells["colid"].Value;
                pnlClientesVisible();
                clearFields();

                //Parametros Cambio de estado
                CbtnGuardar.Text = "Actualizar";
                ClblTexto.Text = "Aquí podras actualizar los <br> respectivos datos del cliente.";
                ClblTexto.Location = new Point(34, 22);
            }
        }

        private void clearFields()
        {
            CtxtDocumento.Clear();
            CtxtNombre.Clear();
            CtxtDireccion.Clear();
            CtxtTelefono.Clear();
            CtxtEmail.Clear();
        }

        private void CbtnAgregar_Click(object sender, EventArgs e)
        {
            pnlClientesVisible();
        }

        private void ClblCerrar_Click(object sender, EventArgs e)
        {
            CpnlFields.Visible = false;
        }

        private void CbtnGuardar_Click(object sender, EventArgs e)
        {
            if(CbtnGuardar.Text == "Guardar")
            {
                agregarCliente();
            }
            else if(CbtnGuardar.Text == "Actualizar")
            {
                actualizarCliente(idCliente);
            }
        }

        private void pnlClientesVisible()
        {
            if (CpnlFields.Visible == false)
            {
                CpnlFields.Visible = true;
            }

        }

        private void CbtnBuscar_Click(object sender, EventArgs e)
        {
            Filtro(sender, e);
        }

        private void Filtro(object sender, EventArgs e)
        {
        }

        private void DGVclientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cargarClientes()
        {
            try
            {
                string codigo = "sp_cargarClientes";
                using (SqlConnection connection = DBconexion.GetConnection())
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(codigo, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    DGVclientes.DataSource = table;
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar información.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private int idCliente;
        private void eliminarCliente(int idCliente)
        {
            try
            {
                string codigo = "sp_eliminarCliente";
                using (SqlConnection connection = DBconexion.GetConnection())
                {
                    connection.Open();
                    SqlCommand comando = new SqlCommand(codigo, connection);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Id", idCliente);
                    comando.ExecuteNonQuery();
                    clearFields();
                    cargarClientes();
                    MessageBox.Show("El cliente ha sido eliminado con éxito.","Éxito",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar el cliente.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void agregarCliente()
        {
            try
            {
                string codigo = "sp_agregarCliente";
                using (SqlConnection connection = DBconexion.GetConnection())
                {
                    connection.Open();
                    SqlCommand comando = new SqlCommand(codigo, connection);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Nombre", CtxtNombre.Text);
                    comando.Parameters.AddWithValue("@Documento", CtxtDocumento.Text);
                    comando.Parameters.AddWithValue("@Direccion", CtxtDireccion.Text);
                    comando.Parameters.AddWithValue("@Telefono", CtxtTelefono.Text);
                    comando.Parameters.AddWithValue("@Email", CtxtEmail.Text);
                    comando.Parameters.AddWithValue("@UsuarioModifico", "system");
                    comando.ExecuteNonQuery();
                    clearFields();
                    cargarClientes();
                    MessageBox.Show("El cliente se ha creado con éxito.","Éxito",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    connection.Close();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error al intentar agregar el cliente."+ex,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void actualizarCliente(int idCliente)
        {
            try
            {
                string codigo = "sp_actualizarCliente";
                using (SqlConnection connection = DBconexion.GetConnection())
                {
                    connection.Open();
                    SqlCommand comando = new SqlCommand(codigo, connection);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Nombre", CtxtNombre.Text);
                    comando.Parameters.AddWithValue("@Documento", CtxtDocumento.Text);
                    comando.Parameters.AddWithValue("@Direccion", CtxtDireccion.Text);
                    comando.Parameters.AddWithValue("@Telefono", CtxtTelefono.Text);
                    comando.Parameters.AddWithValue("@Email", CtxtEmail.Text);
                    comando.Parameters.AddWithValue("@UsuarioModifico", "system");
                    comando.Parameters.AddWithValue("@Id", idCliente);
                    comando.ExecuteNonQuery();
                    clearFields();
                    cargarClientes();
                    CpnlFields.Visible = false;
                    MessageBox.Show("El cliente se ha actualizado con éxito.","Éxito",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    connection.Close();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error al intentar actualizar el cliente.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            } 
        }
    }
}
