using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Registro_Inventarios.Constructor;

namespace Registro_Inventarios.Vistas
{
    public class VistaProducto
    {
        // Métodos para interactuar con los controles del formulario
        public string ObtenerCodigoBarra(TextBox txtCodigoBarra)
        {
            return txtCodigoBarra.Text;
        }

        public string ObtenerNombre(TextBox txtNombre)
        {
            return txtNombre.Text;
        }

        public string ObtenerDescripcion(TextBox txtDescripcion)
        {
            return txtDescripcion.Text;
        }

        public decimal ObtenerPrecio(TextBox txtPrecio)
        {
            decimal precio;
            if (decimal.TryParse(txtPrecio.Text, out precio))
            {
                return precio;
            }
            return 0;  // O lanzar una excepción si el valor no es válido
        }

        public int ObtenerStock(TextBox txtStock)
        {
            int stock;
            if (int.TryParse(txtStock.Text, out stock))
            {
                return stock;
            }
            return 0;  // O lanzar una excepción si el valor no es válido
        }

        public void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje);
        }

        // Puedes agregar más métodos si necesitas interactuar con otros controles como DataGridView, etc.
        public void CargarProductos(DataGridView dgvProductos, List<Producto> productos)
        {
            dgvProductos.DataSource = productos;
        }
    }
}
