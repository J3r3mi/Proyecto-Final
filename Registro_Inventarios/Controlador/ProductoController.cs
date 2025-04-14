using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro_Inventarios.Constructor;
using Registro_Inventarios.Conexion;
using System.Windows.Forms;



namespace Registro_Inventarios.Controlador
{
    public class ProductoController
    {
        private Registro_Inventarios.Conexion.Conexion conexion = new Registro_Inventarios.Conexion.Conexion();


        // Método para registrar un producto
        public void RegistrarProducto(Producto producto)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                // Validar que el precio no sea menor que 0.01
                if (producto.precio < 0.01m)
                {
                    producto.precio = 0.01m;
                }

                string query = "INSERT INTO Productos (codigo_barra, nombre, descripcion, precio, stock) VALUES (@codigo_barra, @nombre, @descripcion, @precio, @stock)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@codigo_barra", producto.codigo_barra);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@stock", producto.stock);

                int filasafectadas = cmd.ExecuteNonQuery();
                if (filasafectadas > 0)
                {
                    MessageBox.Show("Producto registrado correctamente.");
                }
                else
                {
                    MessageBox.Show("Error: No se registró ningún producto.");
                }
            }
        }

        // Método para obtener todos los productos
        public List<Producto> ObtenerTodosLosProductos()
        {
            List<Producto> listaProductos = new List<Producto>();
            string query = "SELECT * FROM Productos"; // Cambia "Productos" por el nombre real de tu tabla

            using (SqlConnection conn = new Registro_Inventarios.Conexion.Conexion().ObtenerConexion())
            {
                
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Producto producto = new Producto
                    {
                        id_producto = int.Parse(reader["id_producto"].ToString()),
                        codigo_barra = reader["codigo_barra"].ToString(),
                        nombre = reader["nombre"].ToString(),
                        descripcion = reader["descripcion"].ToString(),
                        precio = decimal.Parse(reader["precio"].ToString()),
                        stock = int.Parse(reader["stock"].ToString())
                    };
                    listaProductos.Add(producto);
                }
            }
            return listaProductos;
        }

        // Método para editar un producto
        public void EditarProducto(Producto producto)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                string query = "UPDATE Productos SET nombre = @nombre, descripcion = @descripcion, precio = @precio, stock = @stock WHERE codigo_barra = @codigo_barra";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@codigo_barra", producto.codigo_barra);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@descripcion", producto.descripcion);
                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.ExecuteNonQuery();
            }
        }

        // Método para eliminar un producto
        public void EliminarProductoConReduccion(string codigoBarra)
        {
            Producto producto = BuscarPorCodigo(codigoBarra);
            if (producto == null)
            {
                MessageBox.Show("Producto no encontrado.");
                return;
            }

            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                if (producto.stock > 1)
                {
                    int nuevoStock = producto.stock - 1;
                    string query = "UPDATE Productos SET stock = @stock WHERE codigo_barra = @codigo";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@stock", nuevoStock);
                    cmd.Parameters.AddWithValue("@codigo", codigoBarra);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Stock reducido en 1.");
                }
                else
                {
                    // Eliminar primero del historial
                    string deleteHistorial = "DELETE FROM Historial_Inventarios WHERE id_producto = @id_producto";
                    SqlCommand cmdHistorial = new SqlCommand(deleteHistorial, conn);
                    cmdHistorial.Parameters.AddWithValue("@id_producto", producto.id_producto);
                    cmdHistorial.ExecuteNonQuery();

                    // Luego eliminar el producto
                    string deleteProducto = "DELETE FROM Productos WHERE id_producto = @id_producto";
                    SqlCommand cmdProducto = new SqlCommand(deleteProducto, conn);
                    cmdProducto.Parameters.AddWithValue("@id_producto", producto.id_producto);
                    cmdProducto.ExecuteNonQuery();

                    MessageBox.Show("Producto y su historial eliminados correctamente.");
                }
            }
        }

        public Producto BuscarPorCodigo(string codigo)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                string query = "SELECT * FROM Productos WHERE codigo_barra = @codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@codigo", codigo);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Producto
                        {
                            id_producto = int.Parse(reader["id_producto"].ToString()),
                            codigo_barra = reader["codigo_barra"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            descripcion = reader["descripcion"].ToString(),
                            precio = decimal.Parse(reader["precio"].ToString()),
                            stock = int.Parse(reader["stock"].ToString())
                        };
                    }
                }
            }
            return null;
        }

        public void ActualizarStock(Producto producto)
        {
            using (SqlConnection conn = conexion.ObtenerConexion())
            {
                string query = "UPDATE Productos SET stock = @stock WHERE codigo_barra = @codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.Parameters.AddWithValue("@codigo", producto.codigo_barra);
                cmd.ExecuteNonQuery();
            }
        }
    }
    }
    
