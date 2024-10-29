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
    public partial class frmEmpleadosD2 : Form
    {
        public frmEmpleadosD2()
        {
            InitializeComponent();
            datosBD();
            this.Load += new EventHandler(frmClientesD2_Load);
        }

        private void frmClientesD2_Load(object sender, EventArgs e)
        {
            datosBD();
            DGVclientes.AutoGenerateColumns = false;
            DGVclientes.Columns.Clear();

            DataGridViewTextBoxColumn colid = new DataGridViewTextBoxColumn();
            colid.HeaderText = "ID";
            colid.Name = "colid";
            colid.DataPropertyName = "id";
            DGVclientes.Columns.Add(colid);

            DataGridViewTextBoxColumn colnombre = new DataGridViewTextBoxColumn();
            colnombre.HeaderText = "Nombre";
            colnombre.Name = "colnombre";
            colnombre.DataPropertyName = "Nombre";
            DGVclientes.Columns.Add(colnombre);

            DataGridViewTextBoxColumn coldocumento = new DataGridViewTextBoxColumn();
            coldocumento.HeaderText = "Documento";
            coldocumento.Name = "coldocumento";
            coldocumento.DataPropertyName = "Documento";
            DGVclientes.Columns.Add(coldocumento);

            DataGridViewTextBoxColumn coldireccion = new DataGridViewTextBoxColumn();
            coldireccion.HeaderText = "Dirección";
            coldireccion.Name = "coldireccion";
            coldireccion.DataPropertyName = "Direccion";
            DGVclientes.Columns.Add(coldireccion);

            DataGridViewTextBoxColumn coltelefono = new DataGridViewTextBoxColumn();
            coltelefono.HeaderText = "Télefono";
            coltelefono.Name = "coltelefono";
            coltelefono.DataPropertyName = "Telefono";
            DGVclientes.Columns.Add(coltelefono);

            DataGridViewTextBoxColumn colemail = new DataGridViewTextBoxColumn();
            colemail.HeaderText = "Email";
            colemail.Name = "colemail";
            colemail.DataPropertyName = "Email";
            DGVclientes.Columns.Add(colemail);

            DataGridViewTextBoxColumn colfechaingreso = new DataGridViewTextBoxColumn();
            colfechaingreso.HeaderText = "Fecha Ingreso";
            colfechaingreso.Name = "colfechaingreso";
            colfechaingreso.DataPropertyName = "FechaIngreso";
            DGVclientes.Columns.Add(colfechaingreso);

            DataGridViewTextBoxColumn coldatosadicionales = new DataGridViewTextBoxColumn();
            coldatosadicionales.HeaderText = "Datos";
            coldatosadicionales.Name = "coldatosadicionales";
            coldatosadicionales.DataPropertyName = "datosadicionales";
            DGVclientes.Columns.Add(coldatosadicionales);

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
                DialogResult result = MessageBox.Show("¿Deseas eliminar este empleado?, no podrás volver a recuperarlo.", "Eliminar empleado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int id = (int)DGVclientes.Rows[e.RowIndex].Cells["colid"].Value;
                    eliminarUsuario(id);
                    datosBD();
                }

            }
            else if (e.ColumnIndex == DGVclientes.Columns["ceditar"].Index && e.RowIndex >= 0)
            {
                empleadoID = (int)DGVclientes.Rows[e.RowIndex].Cells["colid"].Value;
                pnlClientesVisible();
                clearFields();
                CbtnGuardar.Text = "✏️ Actualizar";
                ClblTexto.Text = "❗ Aquí podras actualizar los <br> respectivos datos del empleado.";
                ClblTexto.Location = new Point(34, 22);
            }
        }

        private void clearFields()
        {
            CtxtDocumento.Clear();
            CtxtNombre.Clear();
            CtxtTelefono.Clear();
        }

        private void CbtnAgregar_Click(object sender, EventArgs e)
        {
            pnlClientesVisible();
        }

        private void ClblCerrar_Click(object sender, EventArgs e)
        {
            CpnlFields.Visible = false;
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

        private void datosBD()
        {
            string codigo = "sp_cargaEmpleados";
            using (SqlConnection conexionBD = DBconexion.GetConnection())
            {
                conexionBD.Open();
                using (SqlCommand comando = new SqlCommand(codigo, conexionBD))
                {
                    SqlDataReader reader = comando.ExecuteReader();
                    DataTable tabla = new DataTable();
                    tabla.Load(reader);
                    DGVclientes.DataSource = tabla;
                }
            }
        }

        private void eliminarUsuario(int id)
        {
            string codigo = "delete empleados where id = @id";
            using (SqlConnection conexionDB = DBconexion.GetConnection())
            {
                conexionDB.Open();
                SqlCommand comando = new SqlCommand(codigo, conexionDB);
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();
                conexionDB.Close();
            }
        }

        private void actualizarEmpleado(int id)
        {
            string codigo = "sp_actualizarEmpleado";
            using (SqlConnection conexionDB = DBconexion.GetConnection())
            {
                conexionDB.Open();
                SqlCommand comando = new SqlCommand(codigo, conexionDB);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@nombre", CtxtNombre.Text);
                comando.Parameters.AddWithValue("@documento", CtxtDocumento.Text);
                comando.Parameters.AddWithValue("@direccion", CtxtDireccion.Text);
                comando.Parameters.AddWithValue("@telefono", CtxtTelefono.Text);
                comando.Parameters.AddWithValue("@email", CtxtEmail.Text);
                comando.Parameters.AddWithValue("@datosadicionales", CtxtDatos.Text);
                comando.Parameters.AddWithValue("@id", id);
                comando.ExecuteNonQuery();
                conexionDB.Close();
            }
        }

        private int empleadoID;
        private void CbtnGuardar_Click(object sender, EventArgs e)
        {
            if(empleadoID > 0)
            {
                clearFields();
                CpnlFields.Visible = false;
                actualizarEmpleado(empleadoID);
                Console.WriteLine(CtxtNombre.Text,CtxtDocumento.Text,CtxtTelefono.Text,CtxtEmail.Text);
                datosBD();
                MessageBox.Show("El usuario se ha actualizado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }
    }
}
