using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace Registro_Inventarios.Conexion
{
    public class Conexion

    {

        private readonly string cadenaConexion = @"Server=(localdb)\Jeremias;Database=Registro_Inventarios;Trusted_Connection=True;";

        // Propiedad pública para acceder a cadenaConexion
        public string CadenaConexion
        {
            get { return cadenaConexion; }
        }

        public SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            try
            {
                conexion.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar con la base de datos: " + ex.Message);
            }
            return conexion;
        }
    }
}


