using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Registro_Inventarios.Constructor
{
    public class Producto
    {
        public int id_producto { get; set; }           // Identificador único del producto (si lo tienes)
        public string codigo_barra { get; set; }       // Código de barras del producto
        public string nombre { get; set; }            // Nombre del producto
        public string descripcion { get; set; }       // Descripción del producto
        public decimal precio { get; set; }           // Precio del producto
        public int stock { get; set; }                // Cantidad en stock

        // Constructor vacío (puedes agregar más si es necesario)
        public Producto() { }

        // Constructor con parámetros (opcional)
        public Producto(string codigobarra, string Nombre, string Descripcion, decimal Precio, int Stock)
        {
            codigo_barra = codigobarra;
            nombre = Nombre;
            descripcion = Descripcion;
            precio = Precio;
            stock = Stock;
        }
    }
}
