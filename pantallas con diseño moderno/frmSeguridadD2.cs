using pantallas_con_diseño_moderno.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pantallas_con_diseño_moderno
{
    public partial class frmSeguridadD2 : Form
    {
        public class parametros
        {
            public int idSeguridad { get; set; }  // Cambiado a idSeguridad
            public string nombre { get; set; }
            public string documento { get; set; }
            public string direccion { get; set; }
            public string telefono { get; set; }
        }

        private List<parametros> listadoUsuarios = new List<parametros>();
        private int index = 1;
        private int? idUsuarioActualizando = null;

        public frmSeguridadD2()
        {
            InitializeComponent();
            cargaData();
            this.Load += new EventHandler(frmClientesD2_Load);
        }

        private void frmClientesD2_Load(object sender, EventArgs e)
        {
            DGVclientes.AutoGenerateColumns = false; // Cambiado a false
            DGVclientes.Columns.Clear();

            // Configuración de columnas
            DGVclientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID seguridad",
                Name = "cid",
                DataPropertyName = "idSeguridad"
            });

            DGVclientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID empleado",
                Name = "colnombre",
                DataPropertyName = "idEmpleado"
            });

            DGVclientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Usuario",
                Name = "coldocumento",
                DataPropertyName = "Usuario"
            });

            DGVclientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Clave",
                Name = "coldireccion",
                DataPropertyName = "Clave"
            });

            DGVclientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Fecha Modifica",
                Name = "coltelefono",
                DataPropertyName = "FechaModifica"
            });

            DGVclientes.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Usuario Modifico",
                Name = "colusuario",
                DataPropertyName = "UsuarioModifico"
            });

            DGVclientes.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Editar",
                Name = "ceditar",
                Text = "Editar",
                UseColumnTextForButtonValue = true
            });

            DGVclientes.Columns.Add(new DataGridViewButtonColumn
            {
                HeaderText = "Borrar",
                Name = "cborrar",
                Text = "Borrar",
                UseColumnTextForButtonValue = true
            });

            DGVclientes.CellClick += detectorClick;
        }

        private void detectorClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DGVclientes.Columns["cborrar"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("¿Deseas eliminar este usuario?, no podrás volver a recuperarlo.", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    idSeguridad = (int)DGVclientes.Rows[e.RowIndex].Cells["cid"].Value;
                    if (eliminarSeguridad(idSeguridad))
                    {
                        DGVclientes.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            else if (e.ColumnIndex == DGVclientes.Columns["ceditar"].Index && e.RowIndex >= 0)
            {
                idSeguridad = (int)DGVclientes.Rows[e.RowIndex].Cells["cid"].Value;
                pnlClientesVisible();
                clearFields();
                cargarDatos(idSeguridad);
                //ActualizarUsuarioEnBaseDeDatos();
                CbtnGuardar.Text = "Actualizar";
                ClblTexto.Text = "Aquí podrás actualizar los <br> respectivos valores del usuario.";
                ClblTexto.Location = new Point(34, 22);
            }
        }

        private void crearUsuario()
        {
            var nuevoUsuario = new parametros
            {
                idSeguridad = index++, // Cambiado a idSeguridad
                nombre = etxtUsuario.Text,
                documento = etxtClave.Text,
            };

            listadoUsuarios.Add(nuevoUsuario);
            actualizarDataGrid();
            clearFields();
        }

        private void cargarDatos(int id)
        {
            var datos = listadoUsuarios.FirstOrDefault(e => e.idSeguridad == id); // Cambiado a idSeguridad
            if (datos != null)
            {
                etxtUsuario.Text = datos.nombre;
                etxtClave.Text = datos.documento;
                idUsuarioActualizando = datos.idSeguridad; // Guardar idSeguridad para actualizar
            }
        }

        private void actualizarDataGrid()
        {
            DGVclientes.DataSource = null;
            DGVclientes.DataSource = listadoUsuarios;
        }

        private void clearFields()
        {
            etxtClave.Clear();
            etxtUsuario.Clear();
        }

        private void pnlClientesVisible()
        {
            CpnlFields.Visible = !CpnlFields.Visible; // Alterna la visibilidad
        }

        private void Filtro(object sender, EventArgs e)
        {
            string filtro = CtxtBuscar.Text.ToLower();

            var usuariosFiltrados = listadoUsuarios
                                    .Where(usuario => usuario.nombre.ToLower().Contains(filtro))
                                    .ToList();
            DGVclientes.DataSource = null;
            DGVclientes.DataSource = usuariosFiltrados;
        }

        private void CbtnAgregar_Click_1(object sender, EventArgs e)
        {
            pnlClientesVisible();
        }

        private void CbtnBuscar_Click_1(object sender, EventArgs e)
        {
            Filtro(sender, e);
        }

        private void ClblCerrar_Click_1(object sender, EventArgs e)
        {
            CpnlFields.Visible = false;
        }

        private void CbtnGuardar_Click_1(object sender, EventArgs e)
        {
            ActualizarUsuarioEnBaseDeDatos(idSeguridad);
            CpnlFields.Visible = false;
        }

        private void cargaData()
        {
            try
            {
                string codigo = "SELECT idSeguridad, idEmpleado, Usuario, Clave, FechaModifica, UsuarioModifico FROM seguridad";
                using (SqlConnection ConexionDB = DBconexion.GetConnection())
                {
                    ConexionDB.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(codigo, ConexionDB);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    DGVclientes.DataSource = table;

                    ConexionDB.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private bool eliminarSeguridad(int id)
        {
            try
            {
                string codigo = "DELETE FROM seguridad WHERE idSeguridad = @idSeguridad";
                using (SqlConnection ConexionDB = DBconexion.GetConnection())
                {
                    ConexionDB.Open();
                    SqlCommand comando = new SqlCommand(codigo, ConexionDB);
                    comando.Parameters.AddWithValue("@idSeguridad", id);
                    int filasAfectadas = comando.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        MessageBox.Show("El usuario se ha eliminado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el usuario para eliminar", "Advertencia", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK);
                return false;
            }
        }

        private int idSeguridad;
        private void ActualizarUsuarioEnBaseDeDatos(int id)
        {
            if(etxtUsuario.Text == string.Empty && etxtClave.Text == string.Empty) 
            {
                MessageBox.Show("Por favor, ingrese la información requerida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CpnlFields.Visible = true;
            }
            else
            {
                try
                {
                    string codigo = "sp_actualizarSeguridad";
                    using (SqlConnection ConexionDB = DBconexion.GetConnection())
                    {
                        ConexionDB.Open();

                        SqlCommand comando = new SqlCommand(codigo, ConexionDB);
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@usuario", etxtUsuario.Text);
                        comando.Parameters.AddWithValue("@clave", etxtClave.Text);
                        comando.Parameters.AddWithValue("@id", idSeguridad);

                        comando.ExecuteNonQuery();
                        clearFields();
                        cargaData();
                        MessageBox.Show("El usuario ha sido actualizado con éxito", "Mensaje sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ConexionDB.Close();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void CpnlFields_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
