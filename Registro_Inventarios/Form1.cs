using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registro_Inventarios.Constructor;
using Registro_Inventarios.Controlador;
using Registro_Inventarios.Conexion;
using System.Windows.Forms;
using Registro_Inventarios.Vistas;
using System.Data.SqlClient;

namespace Registro_Inventarios
{
    public partial class Form1 : Form
    {
        private ProductoController productoController;
        private VistaProducto vistaProducto;

        public Form1()
        {
            InitializeComponent();
            productoController = new ProductoController();
            vistaProducto = new VistaProducto();
            this.Activated += Form1_Activated;
            this.txtCodigoBarra.KeyDown += TxtCodigoBarra_KeyDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
            CargarProductos();
        }

        private void ConfigurarDataGridView()
        {
            try
            {
                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = false;

                dataGridView1.Columns.Add("idProducto", "ID");
                dataGridView1.Columns.Add("CodigoBarra", "Código de Barra");
                dataGridView1.Columns.Add("Nombre", "Nombre");
                dataGridView1.Columns.Add("Descripcion", "Descripción");
                dataGridView1.Columns.Add("Precio", "Precio");
                dataGridView1.Columns.Add("Stock", "Stock");

                dataGridView1.Columns["idProducto"].Width = 50;
                dataGridView1.Columns["Precio"].DefaultCellStyle.Format = "C2";
                dataGridView1.Columns["Stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Registro_Inventarios.Conexion.Conexion conexion = new Registro_Inventarios.Conexion.Conexion();

                using (SqlConnection conn = conexion.ObtenerConexion())
                {
                    string consulta = "SELECT id_producto, codigo_barra, nombre, descripcion, precio, stock FROM Productos";
                    SqlCommand command = new SqlCommand(consulta, conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add(
                                reader["id_producto"],
                                reader["codigo_barra"],
                                reader["nombre"],
                                reader["descripcion"],
                                reader["precio"],
                                reader["stock"]
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al configurar y cargar el DataGridView: " + ex.Message);
            }
        }

        private void CargarProductos()
        {
            try
            {
                ConfigurarDataGridView();
                List<Producto> productos = productoController.ObtenerTodosLosProductos();

                foreach (var producto in productos)
                {
                    dataGridView1.Rows.Add(producto.id_producto, producto.codigo_barra, producto.nombre, producto.descripcion, producto.precio, producto.stock);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message);
            }
        }

        private void TxtCodigoBarra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string codigo = txtCodigoBarra.Text.Trim();

                if (!string.IsNullOrEmpty(codigo))
                {
                    var productoExistente = productoController.BuscarPorCodigo(codigo);

                    if (productoExistente != null)
                    {
                        productoExistente.stock += 1;
                        productoController.ActualizarStock(productoExistente);
                        MessageBox.Show("Stock actualizado para producto existente.");
                    }
                    else
                    {
                        Producto nuevoProducto = new Producto(
                            codigo,
                            "Auto-registrado",
                            "Producto escaneado",
                            0,
                            1
                        );

                        productoController.RegistrarProducto(nuevoProducto);
                        MessageBox.Show("Producto registrado automáticamente.");
                    }

                    CargarProductos();
                    txtCodigoBarra.Clear();
                    txtCodigoBarra.Focus();
                }
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            txtCodigoBarra.Focus();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                Producto nuevoProducto = new Producto(
                    txtCodigoBarra.Text,
                    txtNombre.Text,
                    txtDescripcion.Text,
                    decimal.Parse(txtPrecio.Text),
                    int.Parse(txtStock.Text)
                );

                productoController.RegistrarProducto(nuevoProducto);
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar el producto: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigoBarra = dataGridView1.SelectedRows[0].Cells["CodigoBarra"].Value?.ToString();

                if (!string.IsNullOrEmpty(codigoBarra))
                {
                    try
                    {
                        Producto productoEditado = new Producto
                        {
                            codigo_barra = codigoBarra,
                            nombre = txtNombre.Text,
                            descripcion = txtDescripcion.Text,
                            precio = decimal.Parse(txtPrecio.Text),
                            stock = int.Parse(txtStock.Text)
                        };

                        productoController.EditarProducto(productoEditado);
                        MessageBox.Show("Producto actualizado correctamente.");
                        CargarProductos(); // Refresca la tabla
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el producto: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No se pudo obtener el código de barras del producto seleccionado.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener el código de barras desde la fila seleccionada
                string codigoBarra = dataGridView1.SelectedRows[0].Cells["CodigoBarra"].Value?.ToString();

                if (!string.IsNullOrEmpty(codigoBarra))
                {
                    productoController.EliminarProductoConReduccion(codigoBarra);
                    CargarProductos(); // Recargar la tabla
                }
                else
                {
                    MessageBox.Show("Error: No se pudo obtener el código de barras del producto.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                txtCodigoBarra.Text = fila.Cells["CodigoBarra"].Value?.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
                txtDescripcion.Text = fila.Cells["Descripcion"].Value?.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value?.ToString();
                txtStock.Text = fila.Cells["Stock"].Value?.ToString();
            }
        }
    }
}