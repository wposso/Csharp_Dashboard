using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Capa_AccesoDatos

{
    public class Cls_Acceso_Datos
    {
        private static String conexionString = "server = ADMINSYSTEM; database = DB_Facturacion; user = sa; password = masterkey";
        private static SqlConnection conexionSQL;

        public static SqlConnection GetSqlConnection()
        {
            conexionSQL = new SqlConnection(conexionString);
            return conexionSQL;
        }

        public static void openConnection()
        {
            if(conexionSQL.State == System.Data.ConnectionState.Closed)
            {
                conexionSQL.Open();
            }
        }

        public static void closeConnection()
        {
            if(conexionSQL.State == System.Data.ConnectionState.Open)
            {
                conexionSQL.Close();
            }
        }

    }
}
