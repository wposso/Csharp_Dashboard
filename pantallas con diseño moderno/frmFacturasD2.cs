using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pantallas_con_diseño_moderno
{
    public partial class frmFacturasD2 : Form
    {
        public class parametros
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public string documento { get; set; }
            public string direccion { get; set; }
            public string telefono { get; set; }
        }

        private List<parametros> listadoUsuarios = new List<parametros>();
        private int index = 1;

        public frmFacturasD2()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmClientesD2_Load);
        }

        private void frmClientesD2_Load(object sender, EventArgs e)
        {
            DGVclientes.AutoGenerateColumns = false;
            DGVclientes.Columns.Clear();

            DataGridViewTextBoxColumn colid = new DataGridViewTextBoxColumn();
            colid.HeaderText = "No. factura";
            colid.Name = "cid";
            colid.DataPropertyName = "id";
            DGVclientes.Columns.Add(colid);

            DataGridViewTextBoxColumn colnombre = new DataGridViewTextBoxColumn();
            colnombre.HeaderText = "Cliente";
            colnombre.Name = "colnombre";
            colnombre.DataPropertyName = "nombre";
            DGVclientes.Columns.Add(colnombre);

            DataGridViewTextBoxColumn coldocumento = new DataGridViewTextBoxColumn();
            coldocumento.HeaderText = "Emisión";
            coldocumento.Name = "coldocumento";
            coldocumento.DataPropertyName = "documento";
            DGVclientes.Columns.Add(coldocumento);

            DataGridViewTextBoxColumn coldireccion = new DataGridViewTextBoxColumn();
            coldireccion.HeaderText = "Valor";
            coldireccion.Name = "coldireccion";
            coldireccion.DataPropertyName = "direccion";
            DGVclientes.Columns.Add(coldireccion);

            DataGridViewTextBoxColumn coltelefono = new DataGridViewTextBoxColumn();
            coltelefono.HeaderText = "Estado";
            coltelefono.Name = "coltelefono";
            coltelefono.DataPropertyName = "telefono";
            DGVclientes.Columns.Add(coltelefono);

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
                DialogResult result = MessageBox.Show("¿Deseas eliminar esta factura?, no podrás volver a recuperarla.", "Eliminar factura", MessageBoxButtons.YesNo);
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
                ClblTexto.Text = "❗ Aquí podras actualizar los <br> respectivos datos de la factura.";
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
                direccion = CtxtDireccion.Text,
                telefono = CtxtTelefono.Text
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
                    usuario.direccion = CtxtDireccion.Text;
                    usuario.telefono = CtxtTelefono.Text;

                    actualizarDataGrid();
                    idUsuarioActualizando = null;
                    clearFields();
                    CbtnGuardar.Text = "📂 Guardar";
                    ClblTexto.Text = "❗Por favor, ingrese la información <br> \r\nrequerida para poder crear el la factura.";
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
                CtxtDireccion.Text = datos.direccion;
                CtxtTelefono.Text = datos.telefono;

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
            CtxtDireccion.Clear();
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

        private void CtxtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

    }
}
