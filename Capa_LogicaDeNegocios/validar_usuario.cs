using System.Data.SqlClient;
using System.Security.Policy;

namespace Capa_LogicaDeNegocios
{
    public class validar_usuario
    {
        private string connectionString;

        public validar_usuario(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool validarUsuario(String usuario, String clave)
        {
            string codigo = "select * from seguridad where Usuario = @Usuario and Clave = @Clave";
            using (SqlConnection conexionSQL = Capa_AccesoDatos.Cls_Acceso_Datos.GetSqlConnection())
            {
                try
                {
                    conexionSQL.Open();

                    SqlCommand command = new SqlCommand(codigo, conexionSQL);
                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Clave", clave);
                    int result = (int)command.ExecuteScalar();
                    return result > 0;
                }
                catch(Exception ex) 
                { 
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if(conexionSQL.State == System.Data.ConnectionState.Open)
                    {
                        conexionSQL.Close();
                    }
                }
            }
            return false;
        }

    }
}
