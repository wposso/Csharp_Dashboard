using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_LogicaDeNegocios
{
    public class registrar_usuario
    {
        private string conexionString;
        public registrar_usuario(string conexionString) 
        {
            this.conexionString = conexionString;
        }
        public bool registrarUsuario(
            string nombre,
            string documento,
            string direccion,
            string telefono,
            string email,
            int idRolEmpleado,
            string datosAdicionales)
        {
            try
            {
                string codigo = "sp_registrarEmpleado";
                using (SqlConnection conexionSQL = Capa_AccesoDatos.Cls_Acceso_Datos.GetSqlConnection())
                {
                    conexionSQL.Open();

                    SqlCommand comando = new SqlCommand(codigo, conexionSQL);
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.AddWithValue("@Nombre", nombre);
                    comando.Parameters.AddWithValue("@Documento", documento);
                    comando.Parameters.AddWithValue("@Direccion", direccion);
                    comando.Parameters.AddWithValue("@Telefono", telefono);
                    comando.Parameters.AddWithValue("@Email", email);
                    comando.Parameters.AddWithValue("@idRolEmpleado", idRolEmpleado);
                    comando.Parameters.AddWithValue("@DatosAdicionales", datosAdicionales);
                    comando.Parameters.AddWithValue("@UsuarioModifico", "system");
                    comando.Parameters.AddWithValue("@usuario", "usuario2024");
                    comando.Parameters.AddWithValue("@clave", "clave2024");
                    int rows = comando.ExecuteNonQuery();

                    conexionSQL.Close();

                    return rows > 0;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
