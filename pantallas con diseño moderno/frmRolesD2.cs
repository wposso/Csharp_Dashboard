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
    public partial class frmRolesD2 : Form
    {
        public class parametros
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public string documento { get; set; }
        }

        private List<parametros> listadoUsuarios = new List<parametros>();
        private int index = 1;

        public frmRolesD2()
        {
            InitializeComponent();
            cargarData();
            this.Load += new EventHandler(frmClientesD2_Load);
        }

        private void frmClientesD2_Load(object sender, EventArgs e)
        {
            DGVclientes.AutoGenerateColumns = true;
            DGVclientes.Columns.Clear();

            DataGridViewTextBoxColumn colid = new DataGridViewTextBoxColumn();
            colid.HeaderText = "ID";
            colid.Name = "cid";
            colid.DataPropertyName = "idRol";
            DGVclientes.Columns.Add(colid);

            DataGridViewTextBoxColumn colnombre = new DataGridViewTextBoxColumn();
            colnombre.HeaderText = "Nombre";
            colnombre.Name = "colnombre";
            colnombre.DataPropertyName = "Descripcion";
            DGVclientes.Columns.Add(colnombre);

            //DataGridViewTextBoxColumn coldocumento = new DataGridViewTextBoxColumn();
            //coldocumento.HeaderText = "Documento";
            //coldocumento.Name = "coldocumento";
            //coldocumento.DataPropertyName = "documento";
            //DGVclientes.Columns.Add(coldocumento);

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
                DialogResult result = MessageBox.Show("¿Deseas eliminar este rol?, no podrás volver a recuperarlo.", "Eliminar rol", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int id = (int)DGVclientes.Rows[e.RowIndex].Cells["cid"].Value;
                    eliminarUsuario(id);
                }

            }
            else if (e.ColumnIndex == DGVclientes.Columns["ceditar"].Index && e.RowIndex >= 0)
            {
                int id = (int)DGVclientes.Rows[e.RowIndex].Cells["cid"].Value;
                pnlClientesVisible();
                clearFields();
                cargarDatos(id);
                CbtnGuardar.Text = "✏️ Actualizar";
                ClblTexto.Text = "❗ Aquí podras actualizar los <br> respectivos datos del rol.";
                ClblTexto.Location = new Point(34, 22);
            }
        }

        private void crearUsuario()
        {
            parametros parametros = new parametros
            {
                id = index++,
                nombre = CtxtNombre.Text,
                documento = CtxtDocumento.Text,
            };

            listadoUsuarios.Add(parametros);
            actualizarDataGrid();
            clearFields();
        }

        private void eliminarUsuario(int id)
        {
            var usuario = listadoUsuarios.FirstOrDefault(e => e.id == id);
            if (usuario != null)
            {
                listadoUsuarios.Remove(usuario);
                actualizarDataGrid();
            }
        }

        private int? idUsuarioActualizando = null;

        private void actualizarUsuario(object sender, EventArgs e)
        {
            if (idUsuarioActualizando.HasValue)
            {
                var usuario = listadoUsuarios.FirstOrDefault(e => e.id == idUsuarioActualizando.Value);
                if (usuario != null)
                {
                    usuario.nombre = CtxtNombre.Text;
                    usuario.documento = CtxtDocumento.Text;

                    actualizarDataGrid();
                    idUsuarioActualizando = null;
                    clearFields();
                    CbtnGuardar.Text = "📂 Guardar";
                    ClblTexto.Text = "❗Por favor, ingrese la información <br> \r\nrequerida para poder crear el rol.";
                }
            }
        }

        private void cargarDatos(int id)
        {
            var datos = listadoUsuarios.FirstOrDefault(e => e.id == id);
            if (datos != null)
            {
                CtxtNombre.Text = datos.nombre;
                CtxtDocumento.Text = datos.documento;

                idUsuarioActualizando = id;
            }
        }

        private void actualizarDataGrid()
        {
            DGVclientes.DataSource = null;
            DGVclientes.DataSource = listadoUsuarios;
        }

        private void clearFields()
        {
            CtxtDocumento.Clear();
            CtxtNombre.Clear();
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
            CpnlFields.Visible = false;

            if (idUsuarioActualizando.HasValue)
            {
                actualizarUsuario(sender, e);
            }
            else
            {
                crearUsuario();
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
            string filtro = CtxtBuscar.Text.ToLower();

            var clientesFiltrados = listadoUsuarios
                                    .Where(usuario => usuario.nombre.ToLower().Contains(filtro))
                                    .ToList();
            CtxtBuscar.Clear();
            DGVclientes.DataSource = null;
            DGVclientes.DataSource = clientesFiltrados;
        }

        private void cargarData()
        {
            try
            {
                string codigo = "select idRol, Descripcion from roles";
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
                MessageBox.Show("Error: al cargar datos" + ex);
            }
            
        }
    }
}
