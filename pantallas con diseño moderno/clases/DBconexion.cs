using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace pantallas_con_diseño_moderno.clases
{
    internal class DBconexion
    {
        private static string connectionString = "server= ADMINSYSTEM; database= DB_Facturacion; user = sa; password = masterkey;integrated security = false";
        private static SqlConnection conexionDB;
        
        public static SqlConnection GetConnection()
        {
            conexionDB = new SqlConnection(connectionString);
            return conexionDB;
        }

        public static void OpenConnection()
        {
            if(conexionDB.State == System.Data.ConnectionState.Closed)
            {
                conexionDB.Open();
            }
        }

        public static void closeConnection()
        {
            if(conexionDB.State == System.Data.ConnectionState.Open)
            {
                conexionDB.Close();
            }
        }
    }

}
