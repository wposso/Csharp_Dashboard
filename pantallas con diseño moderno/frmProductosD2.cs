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
    public partial class frmProductosD2 : Form
    {

        public frmProductosD2()
        {
            InitializeComponent();
            cargarData();
            this.Load += new EventHandler(frmClientesD2_Load);
        }

        private void frmClientesD2_Load(object sender, EventArgs e)
        {
            DGVclientes.AutoGenerateColumns = false;
            DGVclientes.Columns.Clear();

            DataGridViewTextBoxColumn colid = new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                Name = "cid",
                DataPropertyName = "idProducto"
            };
            DGVclientes.Columns.Add(colid);

            DataGridViewTextBoxColumn colnombre = new DataGridViewTextBoxColumn
            {
                HeaderText = "Producto",
                Name = "colnombre",
                DataPropertyName = "Nombre"
            };
            DGVclientes.Columns.Add(colnombre);

            DataGridViewTextBoxColumn coldocumento = new DataGridViewTextBoxColumn
            {
                HeaderText = "Categoria",
                Name = "coldocumento",
                DataPropertyName = "PrecioVenta"
            };
            DGVclientes.Columns.Add(coldocumento);

            DataGridViewTextBoxColumn coldireccion = new DataGridViewTextBoxColumn
            {
                HeaderText = "Precio",
                Name = "coldireccion",
                DataPropertyName = "idCategoria"
            };
            DGVclientes.Columns.Add(coldireccion);

            DataGridViewTextBoxColumn coltelefono = new DataGridViewTextBoxColumn
            {
                HeaderText = "Stock",
                Name = "coltelefono",
                DataPropertyName = "NumStock"
            };
            DGVclientes.Columns.Add(coltelefono);

            DataGridViewButtonColumn coleditar = new DataGridViewButtonColumn
            {
                HeaderText = "Editar",
                Name = "ceditar",
                Text = "Editar",
                UseColumnTextForButtonValue = true
            };
            DGVclientes.Columns.Add(coleditar);

            DataGridViewButtonColumn colborrar = new DataGridViewButtonColumn
            {
                HeaderText = "Borrar",
                Name = "cborrar",
                Text = "Borrar",
                UseColumnTextForButtonValue = true
            };
            DGVclientes.Columns.Add(colborrar);

            DGVclientes.CellClick += detectorClick;
        }

        private void detectorClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DGVclientes.Columns["cborrar"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("¿Deseas eliminar este producto?, no podrás volver a recuperarlo.", "Eliminar producto", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    idProductos = (int)DGVclientes.Rows[e.RowIndex].Cells["cid"].Value;
                    borrarProducto(idProductos);
                    cargarData();
                }
            }
            else if (e.ColumnIndex == DGVclientes.Columns["ceditar"].Index && e.RowIndex >= 0)
            {
                idProductos = (int)DGVclientes.Rows[e.RowIndex].Cells["cid"].Value;
                pnlClientesVisible();
                clearFields();

                CbtnGuardar.Text = "Actualizar";
                ClblTexto.Text = "Aquí podrás actualizar los <br> respectivos valores del producto.";
                ClblTexto.Location = new Point(34, 22);
            }
        }

        private void clearFields()
        {
            CtxtDocumento.Clear();
            CtxtNombre.Clear();
            CtxtDireccion.Clear();
            CtxtTelefono.Clear();
        }

        private void pnlClientesVisible()
        {
            if (!CpnlFields.Visible)
            {
                CpnlFields.Visible = true;
            }
        }

        private void Filtro(object sender, EventArgs e)
        {
        }

        private void CbtnAgregar_Click_1(object sender, EventArgs e)
        {
            pnlClientesVisible(); // el evento click del boton activa el panel
        }

        private void CbtnBuscar_Click_1(object sender, EventArgs e)
        {
            Filtro(sender, e); // el evento click del boton busca el filtro
        }

        private void ClblCerrar_Click_1(object sender, EventArgs e)
        {
            CpnlFields.Visible = false; // el evento click cierra el panel
        }

        private void CbtnGuardar_Click_1(object sender, EventArgs e)
        {
            if(CbtnGuardar.Text == "Guardar")
            {
                crearProducto();
            }
            else if(CbtnGuardar.Text == "Actualizar")
            {
                actualizarProducto(idProductos); // el evento click llama al metodo actualizar
            } 
        }

        private void CtxtPrecio_KeyPress(object sender, KeyPressEventArgs e) // metodo que admite solo valores numericos en el campo de texto
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void cargarData() // carga los datos desde la base de datos al datagrid
        {
            string codigo = "SELECT idProducto, Nombre, PrecioVenta, idCategoria, NumStock FROM productos";

            using (SqlConnection ConexionDB = DBconexion.GetConnection())
            {
                ConexionDB.Open();

                using (SqlCommand comando = new SqlCommand(codigo, ConexionDB))
                {
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        DGVclientes.DataSource = dataTable;
                    }
                }
                ConexionDB.Close();
            }
        }

        private void borrarProducto(int idProducto) // metodo para borrar un producto 
        {
            string codigo = "DELETE FROM productos WHERE idProducto = @idProducto"; // script

            using (SqlConnection ConexionDB = DBconexion.GetConnection()) // se obtiene la conexion
            {
                ConexionDB.Open(); // se abre la conexion

                using (SqlCommand comando = new SqlCommand(codigo, ConexionDB))
                {
                    comando.Parameters.AddWithValue("@idProducto", idProducto); // se añade un parametro

                    int filasAfectadas = comando.ExecuteNonQuery();
                    if (filasAfectadas > 0) // si la fila detectada no es null se ejecuta
                    {
                        MessageBox.Show("Producto eliminado exitosamente", "Éxito", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el producto", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
                ConexionDB.Close();
            }
        }

        private void crearProducto()
        {
            try
            {
                string codigo = "INSERT INTO productos VALUES (@Nombre, 'COL0023', '23.00', @PrecioVenta, @idCategoria, 'Descripción', '', @NumStock, GetDate(), 'system')";
                using (SqlConnection ConexionBD = DBconexion.GetConnection())
                {
                    ConexionBD.Open();

                    SqlCommand comando = new SqlCommand(codigo, ConexionBD);
                    comando.Parameters.AddWithValue("@Nombre", CtxtNombre.Text);
                    comando.Parameters.AddWithValue("@PrecioVenta", Convert.ToInt32(CtxtDireccion.Text));
                    comando.Parameters.AddWithValue("@idCategoria", CtxtDocumento.Text);
                    comando.Parameters.AddWithValue("@NumStock", CtxtTelefono.Text);

                    comando.ExecuteNonQuery(); // se ejecuta el script en la base de datos
                    cargarData();
                    clearFields();
                    CpnlFields.Visible = false;
                    MessageBox.Show("El producto se ha creado con éxito", "Éxito", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    ConexionBD.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear el producto: " + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private int idProductos;
        private void actualizarProducto(int idProductos)
        {
            string codigo = "UPDATE productos SET Nombre = @Nombre, PrecioVenta = @PrecioVenta, idCategoria = @idCategoria, NumStock = @NumStock WHERE idProducto = @idProducto";
            try
            {
                using (SqlConnection ConexionBD = DBconexion.GetConnection())
                {
                    ConexionBD.Open();
                    using (SqlCommand comando = new SqlCommand(codigo, ConexionBD))
                    {
                        comando.Parameters.AddWithValue("@Nombre", CtxtNombre.Text);
                        comando.Parameters.AddWithValue("@PrecioVenta", Convert.ToInt32(CtxtDireccion.Text));
                        comando.Parameters.AddWithValue("@idCategoria", CtxtDocumento.Text);
                        comando.Parameters.AddWithValue("@NumStock", CtxtTelefono.Text);
                        comando.Parameters.AddWithValue("@idProducto", idProductos);

                        comando.ExecuteNonQuery();
                        cargarData();
                        clearFields();
                        CbtnAgregar.Text = "Guardar";
                        CpnlFields.Visible = false;
                        MessageBox.Show("Producto actualizado con éxito", "Éxito", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    ConexionBD.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el producto: " + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
