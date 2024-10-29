using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_LogicaDeNegocios
{
    internal class Cls_clientes
    {
        private void cargarClientes()
        {
            try
            {
                string codigo = "sp_cargarClientes";
                using (SqlConnection conexionSQL = Capa_AccesoDatos.Cls_Acceso_Datos.GetSqlConnection())
                {
                    conexionSQL.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(codigo, conexionSQL);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    conexionSQL.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
