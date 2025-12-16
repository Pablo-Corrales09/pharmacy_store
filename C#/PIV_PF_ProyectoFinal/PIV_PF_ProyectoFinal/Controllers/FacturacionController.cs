using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class FacturacionController : Controller
    {
        public ActionResult Index()
        {
            return View(ObtenerFacturas());
        }

        private List<ViewModels.ListaFacturas> ObtenerFacturas()
        {
            List<ViewModels.ListaFacturas> listaFacturas = new List<ViewModels.ListaFacturas>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    listaFacturas = (from factura in db.factura
                                     join cliente in db.cliente on factura.id_cliente equals cliente.id_cliente
                                     select new ViewModels.ListaFacturas
                                     {
                                         id_factura = factura.id_factura,
                                         codigo_factura = factura.codigo_factura,
                                         fecha_compra = factura.fecha_compra,
                                         nombre_cliente = cliente.nombre_completo,
                                         metodo_pago = factura.metodo_pago,
                                         sub_total = factura.sub_total
                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener facturas: " + ex.Message);
            }
            return listaFacturas;
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
                System.Diagnostics.Debug.WriteLine("Error al cargar clientes: " + ex.Message);
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

                var facturaView = new ContFactura
                {
                    Factura = new AgregarFactura { fecha_compra = DateTime.Now },
                    Detalle = new DetalleFacturaVM(),
                    Productos = new ListaProductos(),
                    Clientes = new ClientesViewModel()
                };

                if (productos == null || productos.Count == 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "No hay productos disponibles";
                }

                if (clientes == null || clientes.Count == 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "No hay clientes disponibles";
                }

                ViewBag.ListaProductos = productos ?? new List<ListaProductos>();
                ViewBag.ListaClientes = new SelectList(clientes ?? new List<ClientesViewModel>(),
                                                       "cedula",
                                                       "nombre_completo");

                return View(facturaView);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al cargar el formulario";
                System.Diagnostics.Debug.WriteLine("Error en Create GET: " + ex.Message);

                return View(new ContFactura
                {
                    Factura = new AgregarFactura { fecha_compra = DateTime.Now },
                    Detalle = new DetalleFacturaVM(),
                    Productos = new ListaProductos(),
                    Clientes = new ClientesViewModel()
                });
            }
        }

        private int ObtenerIdCliente(string cedula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedula))
                    return 0;

                using (var db = new PIV_PF_Proyecto_Final_Entities())
                {
                    return db.cliente
                             .Where(c => c.cedula == cedula)
                             .Select(c => c.id_cliente)
                             .FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al obtener el id del cliente: " + ex.Message);
                return 0;
            }
        }

        private factura RegistrarFactura(ContFactura infoFactura)
        {
            try
            {
                if (infoFactura.Detalle.cantidad <= 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "La cantidad debe ser mayor a 0";
                    return null;
                }

                if (infoFactura.Detalle.valor_unitario <= 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Debes seleccionar un producto válido";
                    return null;
                }

                if (string.IsNullOrWhiteSpace(infoFactura.Clientes?.cedula))
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Debes seleccionar un cliente";
                    return null;
                }

                using (var db = new PIV_PF_Proyecto_Final_Entities())
                {
                    decimal subtotal = infoFactura.Detalle.cantidad * infoFactura.Detalle.valor_unitario;

                    if (infoFactura.Factura.metodo_pago == "Tarjeta")
                    {
                        subtotal = subtotal * 1.02m;
                    }

                    if (subtotal <= 0)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El subtotal debe ser mayor a 0";
                        return null;
                    }

                    var idCliente = ObtenerIdCliente(infoFactura.Clientes.cedula);

                    if (idCliente <= 0)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Cliente no encontrado en la base de datos";
                        return null;
                    }

                    var nuevaFactura = new factura
                    {
                        codigo_factura = "FAC-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        fecha_compra = infoFactura.Factura.fecha_compra,
                        id_cliente = idCliente,
                        metodo_pago = infoFactura.Factura.metodo_pago,
                        sub_total = subtotal
                    };

                    db.factura.Add(nuevaFactura);
                    db.SaveChanges();

                    System.Diagnostics.Debug.WriteLine("Factura registrada correctamente: " + nuevaFactura.id_factura);

                    return nuevaFactura;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al registrar la factura: " + ex.Message;
                System.Diagnostics.Debug.WriteLine("Error al registrar factura: " + ex.Message);
                return null;
            }
        }

        private detalleFactura RegistrarDetalleFactura(int idFactura, ContFactura infoFactura)
        {
            try
            {
                using (var db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var nuevoDetalle = new detalleFactura
                    {
                        id_factura = idFactura,
                        codigo_producto = infoFactura.Detalle.codigo_producto,
                        cantidad = infoFactura.Detalle.cantidad,
                        valor_unitario = infoFactura.Detalle.valor_unitario,
                        total_linea = infoFactura.Detalle.cantidad * infoFactura.Detalle.valor_unitario
                    };

                    db.detalleFactura.Add(nuevoDetalle);
                    db.SaveChanges();

                    System.Diagnostics.Debug.WriteLine("Detalle registrado correctamente: " + nuevoDetalle.num_consecutivo);

                    return nuevoDetalle;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al registrar detalle: " + ex.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult Create(ContFactura model, FormCollection form)
        {
            try
            {
                ModelState.Remove("Factura.sub_total");
                ModelState.Remove("Detalle.valor_unitario");
                ModelState.Remove("Factura.codigo_factura");

                var productos = ObtenerProductos();
                var clientes = ObtenerClientes();
                ViewBag.ListaProductos = productos ?? new List<ListaProductos>();
                ViewBag.ListaClientes = new SelectList(clientes ?? new List<ClientesViewModel>(),
                                                         "cedula",
                                                         "nombre_completo");

                string valorUnitarioStr = form["Detalle.valor_unitario"];
                decimal valorUnitarioCorregido = 0;

                //Valida que el valor unitario del producto sea válido
                if (string.IsNullOrEmpty(valorUnitarioStr) ||
                    !decimal.TryParse(valorUnitarioStr, NumberStyles.Any, CultureInfo.InvariantCulture, out valorUnitarioCorregido))
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Debes seleccionar un producto válido";
                    return View(model);
                }

                model.Detalle.valor_unitario = valorUnitarioCorregido;

                //Valida que el código de producto no esté vacío
                string codigoProducto = form["Detalle.codigo_producto"];
                if (string.IsNullOrEmpty(codigoProducto))
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error: código de producto no encontrado";
                    return View(model);
                }

                model.Detalle.codigo_producto = codigoProducto;

                //Valida la cantidad de productos válida
                if (model.Detalle.cantidad <= 0)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "La cantidad debe ser mayor a 0";
                    return View(model);
                }

                // Valida información del cliente
                if (string.IsNullOrWhiteSpace(model.Clientes?.cedula))
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Debes seleccionar un cliente";
                    return View(model);
                }

                // Validación del método de pago
                if (string.IsNullOrWhiteSpace(model.Factura?.metodo_pago))
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Debes seleccionar un método de pago";
                    return View(model);
                }

                System.Diagnostics.Debug.WriteLine("Iniciando registro de factura...");
                System.Diagnostics.Debug.WriteLine("Cliente: " + model.Clientes.cedula);
                System.Diagnostics.Debug.WriteLine("Producto: " + model.Detalle.codigo_producto);
                System.Diagnostics.Debug.WriteLine("Cantidad: " + model.Detalle.cantidad);
                System.Diagnostics.Debug.WriteLine("Precio: " + model.Detalle.valor_unitario);
                System.Diagnostics.Debug.WriteLine("Método Pago: " + model.Factura.metodo_pago);

                // Registrar factura
                var factura = RegistrarFactura(model);

                if (factura == null)
                {
                    System.Diagnostics.Debug.WriteLine("La factura no se registró");
                    return View(model);
                }

                System.Diagnostics.Debug.WriteLine("Factura registrada con ID: " + factura.id_factura);

                // Registrar detalle de la factura
                model.Factura.id_factura = factura.id_factura;
                var detalleFactura = RegistrarDetalleFactura(factura.id_factura, model);

                if (detalleFactura == null)
                {
                    System.Diagnostics.Debug.WriteLine("El detalle no se registró");
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al registrar el detalle de la factura";
                    return View(model);
                }

                System.Diagnostics.Debug.WriteLine("Detalle registrado con ID: " + detalleFactura.num_consecutivo);

                // Mostrar mensaje de éxito
                ViewBag.ValorMensaje = 1;
                ViewBag.MensajeProceso = "Factura creada correctamente";

                var facturaView = new ContFactura
                {
                    Factura = new AgregarFactura { fecha_compra = DateTime.Now },
                    Detalle = new DetalleFacturaVM(),
                    Productos = new ListaProductos(),
                    Clientes = new ClientesViewModel()
                };

                return View(facturaView);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error general en Create POST: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack: " + ex.StackTrace);
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error inesperado: " + ex.Message;
                return View(model);
            }
        }
    }
}