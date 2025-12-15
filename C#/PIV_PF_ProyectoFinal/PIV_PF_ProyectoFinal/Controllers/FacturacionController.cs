using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class FacturacionController : Controller
    {
        // GET: Facturacion
        public ActionResult Index()
        {
            return View();
        }

        private List<ListaProductos> ObtenerProductos()
        {
            List<ListaProductos> lista_de_productos = new List<ListaProductos>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    lista_de_productos = (from producto in db.producto
                                         select new ListaProductos
                                         {
                                             codigo_producto = producto.codigo_producto,
                                             descripcion = producto.descripcion,
                                             precio = producto.precio,
                                             estado = producto.estado,
                                             id_tipoProducto = producto.id_tipoProducto
                                         }).ToList();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = "0";
                ViewBag.MensajeProceso = "Error al cargar productos";
                System.Diagnostics.Debug.WriteLine("Error al cargar productos: " + ex.Message);
            }
            return lista_de_productos;
        }

        private List<ClientesViewModel> ObtenerClientes()
        {
            List<ClientesViewModel> lista_de_clientes = new List<ClientesViewModel>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    lista_de_clientes = (from cliente in db.cliente
                                         select new ClientesViewModel
                                         {
                                             cedula = cliente.cedula,
                                             nombre_completo = cliente.nombre_completo,
                                             correo_electronico = cliente.correo_electronico
                                         }).ToList();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = "0";
                ViewBag.MensajeProceso = "Error al cargar clientes";
                return null;
            }
            return lista_de_clientes;
        }

        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                var productos = ObtenerProductos();
                var clientes = ObtenerClientes();

                if (productos == null || productos.Count == 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "No hay productos disponibles";
                    return View();
                }

                if (clientes == null || clientes.Count == 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "No hay clientes disponibles";
                    return View();
                }

                var facturaView = new ContFactura
                {
                    Factura = new AgregarFactura { fecha_compra = DateTime.Now },
                    Detalle = new DetalleFacturaVM(),
                    Productos = new ListaProductos(),
                    Clientes = new ClientesViewModel()
                };

                ViewBag.ListaProductos = productos;
                ViewBag.ListaClientes = new SelectList(clientes, "cedula", "nombre_completo");

                return View(facturaView);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al cargar el formulario: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("Error en Create: " + ex.Message);
                return View();
            }
        }


    }
}