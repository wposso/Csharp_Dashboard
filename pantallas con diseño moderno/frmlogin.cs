using Capa_LogicaDeNegocios;
using pantallas_con_diseño_moderno.clases;
using System.Data;
using System.Data.SqlClient;

namespace pantallas_con_diseño_moderno
{

    public partial class frmlogin : Form
    {

        private DBconexion sqlconnection;

        public frmlogin()
        {
            InitializeComponent();
            dataRoles();
            parametroConexion();
        }

        private validar_usuario Validar_Usuario;
        private registrar_usuario Registrar_Usuario;

        private void parametroConexion()
        {
            string conexionSQL = "server = ADMINSYSTEM; database = DB_Facturacion; user = sa; password = masterkey";
            string connectionString = conexionSQL;

            Validar_Usuario = new validar_usuario(connectionString);
            Registrar_Usuario = new registrar_usuario(connectionString);
        }

        private void validarUsuario(object sender, EventArgs e)
        {
            if (TxtContraseña.Text != string.Empty && TxtUsuario.Text != string.Empty)
            {
                string usuario = TxtUsuario.Text;
                string clave = TxtContraseña.Text;
                bool esValido = Validar_Usuario.validarUsuario(usuario, clave);
                if (esValido)
                {
                    MessageBox.Show(" Inicio de sesión exitoso, bienvenido", "Mensaje sistema", MessageBoxButtons.OK);
                    this.Hide();
                    frmPrincipal frmPrincipal = new frmPrincipal();
                    frmPrincipal.Show();
                }
            }
            else
            {
                MessageBox.Show("Por favor, llene todos los campos", "Mensaje sistema", MessageBoxButtons.OK);
            }
            deleteFields();

        }

        private void registrarUsuario(object sender, EventArgs e)
        {
            try
            {
                if (cbxOpciones.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, seleccione un rol", "Alerta", MessageBoxButtons.OK);
                }
                else
                {
                    string nombre = GBtxt1.Text;
                    string documento = GBtxt2.Text;
                    string direccion = GBtxt3.Text;
                    string telefono = GBtxt4.Text;
                    string email = GBtxt5.Text;
                    int idRolEmpleado = obtenerRolId(cbxOpciones.SelectedItem.ToString());
                    string datosAdicionales = GBtxt6.Text;
                    bool esValido = Registrar_Usuario.registrarUsuario(
                        nombre,
                        documento,
                        direccion,
                        telefono,
                        email,
                        idRolEmpleado,
                        datosAdicionales);
                    if (esValido)
                    {
                        MessageBox.Show(
                            "El usuario se ha registrado con éxito",
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Error al registrar el usuario",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            deleteFields();
        }

        private void chkContraseña_CheckedChanged(object sender, EventArgs e)
        {
            if (chkContraseña.Checked)
            {
                TxtContraseña.PasswordChar = '\0';
            }
            else
            {
                TxtContraseña.PasswordChar = '•';
            }
        }

        private void GpnlB_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbxOpciones_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataRoles()
        {
            cbxOpciones.Items.Add("Administrador");
            cbxOpciones.Items.Add("Supervisor");
            cbxOpciones.Items.Add("Usuario regular");
        }

        private int obtenerRolId(string rolSeleccionado)
        {
            switch (rolSeleccionado)
            {
                case "Administrador":
                    return 1;
                case "Supervisor":
                    return 2; ;
                case "Usuario regular":
                    return 3;

                default:
                    throw new Exception("Rol seleccionado no valido");
            }
        }

        private void GBbtnGuardar_Click(object sender, EventArgs e)
        {
            registrarUsuario(sender, e);
        }

        private void deleteFields()
        {
            GBtxt1.Clear(); GBtxt2.Clear(); GBtxt3.Clear();
            GBtxt4.Clear(); GBtxt5.Clear(); GBtxt6.Clear();
            cbxOpciones.SelectedItem = null;
        }

        private void lblSesión_Click(object sender, EventArgs e)
        {
            GpnlA.Location = new Point(664, 0);
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            validarUsuario(sender, e);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            GpnlA.Location = new Point(0, 0);
        }
    }

}

