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
    public partial class frmCategoriasD2 : Form
    {

        public frmCategoriasD2()
        {
            InitializeComponent();
            cargarCategorias();
            this.Load += new EventHandler(frmClientesD2_Load);
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
            colnombre.HeaderText = "Descrición categoria";
            colnombre.Name = "colnombre";
            colnombre.DataPropertyName = "descripcion";
            DGVclientes.Columns.Add(colnombre);

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
                DialogResult result = MessageBox.Show("¿Deseas eliminar esta categoria?, no podrás volver a recuperarla.", "Eliminar categoria", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    idCategoria = (int)DGVclientes.Rows[e.RowIndex].Cells["colid"].Value;
                    borrarCategoria(idCategoria);
                    clearFields();
                }

            }
            else if (e.ColumnIndex == DGVclientes.Columns["ceditar"].Index && e.RowIndex >= 0)
            {
                idCategoria = (int)DGVclientes.Rows[e.RowIndex].Cells["colid"].Value;
                pnlClientesVisible();
                clearFields();

                //Arreglos cambio de estado
                CbtnGuardar.Text = "Actualizar";
                ClblTitulo.Text = "Actualizar";
                ClblTexto.Text = "Para actualizar la categoria <br> ingrese la información requerida.";
                ClblTitulo.Location = new Point(130, 4);
                ClblTexto.Location = new Point(44, 43);
            }
        }

        private void clearFields()
        {
            ctxtDescripcion.Clear();
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
                CpnlFields.Visible = false;
                crearCategoria();
            }
            else if(CbtnGuardar.Text == "Actualizar")
            {
                CpnlFields.Visible = false;
                actualizarCategoria(idCategoria);
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

        private void cargarCategorias()
        {
            try
            {
                string codigo = "sp_cargarCategorias";
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
                MessageBox.Show("Erro al cargar información." + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void crearCategoria()
        {
            if(ctxtDescripcion.Text != string.Empty)
            {
                string codigo = "sp_guardarCategoria";
                using (SqlConnection connection = DBconexion.GetConnection())
                {
                    connection.Open();
                    SqlCommand comando = new SqlCommand(codigo, connection);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Descripcion", ctxtDescripcion.Text);
                    comando.ExecuteNonQuery();
                    clearFields();
                    cargarCategorias();
                    MessageBox.Show("La categoria se ha creado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Ingrese los datos requeridos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int idCategoria;
        private void borrarCategoria(int idCategoria)
        {
            try
            {
                string codigo = "sp_borrarCategoria";
                using (SqlConnection connection = DBconexion.GetConnection())
                {
                    connection.Open();
                    SqlCommand comando = new SqlCommand(codigo, connection);
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@ID", idCategoria);
                    comando.ExecuteNonQuery();
                    cargarCategorias();
                    MessageBox.Show("Categoria borrada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Error al intentar borrar la categoria.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void actualizarCategoria(int idCategoria)
        {
            if(ctxtDescripcion.Text != string.Empty)
            {
                try
                {
                    string codigo = "sp_actualizarCategoria";
                    using (SqlConnection connection = DBconexion.GetConnection())
                    {
                        connection.Open();
                        SqlCommand comando = new SqlCommand(codigo, connection);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Descripcion", ctxtDescripcion.Text);
                        comando.Parameters.AddWithValue("@ID", idCategoria);
                        comando.ExecuteNonQuery();
                        cargarCategorias();
                        MessageBox.Show("La categoria se ha actualizado con éxito.","Éxito",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        CbtnGuardar.Text = "Guardar";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al intentar actualizar la categoria.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Para actualizar la categoria ingrese los datos requeridos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
