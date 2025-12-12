using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto
        public ActionResult Index()
        {
            return View(ObtenerProductos());
        }

        private List<ViewModels.ProductoVM> ObtenerProductos()
        {
            List<ViewModels.ProductoVM> ProductList = new List<ViewModels.ProductoVM>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    ProductList = (from Producto in db.producto
                                        select new ViewModels.ProductoVM
                                        {
                                            codigo_producto = Producto.codigo_producto,
                                            descripcion = Producto.descripcion,
                                            precio = Producto.precio,
                                            estado = Producto.estado,
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return ProductList;
        }

        private List<ViewModels.TipoProducto> ObtenerTipoProducto()
        {
            List<ViewModels.TipoProducto> TipoProductList = new List<ViewModels.TipoProducto>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    TipoProductList = (from TipoProducto in db.tipoProducto
                                   select new ViewModels.TipoProducto
                                   {
                                       id_tipoProducto = TipoProducto.id_tipoProducto,
                                       codigo_tipo = TipoProducto.codigo_tipo,
                                       descripcion = TipoProducto.descripcion,

                                   }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return TipoProductList;
        }


        private string ObtenerTipoAsociado(int id_tipoProductoI)
        {
            if (id_tipoProductoI <= 0)
            {
                return null;
            }

            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    string tipo_producto = (from t in db.tipoProducto
                                            where t.id_tipoProducto == id_tipoProductoI
                                            select t.descripcion).FirstOrDefault();

                    if (tipo_producto == null)
                    {
                        return null;
                    }

                    return tipo_producto;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al obtener el tipo de producto: " + ex.Message;
                return null;
            }
        }

        //GET: Producto/Details
        [HttpGet]
        public ActionResult Details(string codigo_producto)
        {
            if (string.IsNullOrEmpty(codigo_producto))
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "El ID del tipo de producto no es válido.";
                return View("Details", null);
            }
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var producto = db.producto.FirstOrDefault(p => p.codigo_producto == codigo_producto);

                    if (producto == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El producto no fue encontrado.";
                        return View("Details", null);
                    }
                    var productoVM = new ProductoVM()
                    {
                        codigo_producto = producto.codigo_producto,
                        descripcion = producto.descripcion,
                        precio = producto.precio,
                        estado = producto.estado,
                        id_tipoProducto = producto.id_tipoProducto
                    };

                    string descripcionTipo = ObtenerTipoAsociado(producto.id_tipoProducto);
                    ViewBag.DescripcionTipoProducto = descripcionTipo ?? "No asignado";
                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Producto cargado exitosamente.";

                    return View("Details", productoVM);
                }
            }
            catch (Exception)
            {
                return View("Details", null);
            }
        }



        //GET: Producto/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListaTipoProducto = new SelectList(ObtenerTipoProducto(), "id_tipoProducto", "descripcion");
            return View(new ViewModels.ProductoVM());
        }

        private void CargarDropdownTipos(int? idSeleccionado = null)
        {
            var tipos = ObtenerTipoProducto();
            ViewBag.ListaTipoProducto = new SelectList(tipos, "id_tipoProducto", "descripcion", idSeleccionado);
        }


        //POST: Producto/Create
        [HttpPost]
        public ActionResult Create(ViewModels.ProductoVM p)
        {
            CargarDropdownTipos(p.id_tipoProducto);

            ObtenerTipoProducto();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", p);
                }
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    if (db.producto.Any(x => x.codigo_producto == p.codigo_producto))
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El producto con este ID ya está registrado.";
                        return View("Create", p);
                    }
                    var nuevoProducto = new Models.producto();
                    nuevoProducto.codigo_producto = p.codigo_producto.ToUpper();
                    nuevoProducto.descripcion = p.descripcion.ToUpper();
                    nuevoProducto.precio = p.precio;
                    nuevoProducto.estado = p.estado.ToUpper();
                    nuevoProducto.id_tipoProducto = p.id_tipoProducto;
                    db.producto.Add(nuevoProducto);
                    db.SaveChanges();
                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Tipo de producto registrado exitosamente.";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al agregar el tipo de producto" + ex.Message;
                return View("Create", p);

            }
        }

        //GET: Producto/Delete
        [HttpGet]
        public ActionResult Delete(string codigo_producto)
        {
            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                var productoVM = db.producto.FirstOrDefault(p => p.codigo_producto == codigo_producto);
                if (productoVM == null)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al cargar el producto.";
                    return RedirectToAction("Index");
                }

                var ProductoExistente = new ProductoVM
                {
                    codigo_producto = productoVM.codigo_producto,
                    descripcion = productoVM.descripcion,
                    estado = productoVM.estado,
                    precio = productoVM.precio,
                    id_tipoProducto = productoVM.id_tipoProducto
                };

                string descripcionTipo = ObtenerTipoAsociado(ProductoExistente.id_tipoProducto);
                ViewBag.DescripcionTipoProducto = descripcionTipo ?? "No asignado";
                return View(ProductoExistente);
            }
        }



        //POST: Producto/EliminarProducto
        [HttpPost]
        public ActionResult EliminarProducto(string codigo_producto)
        {
                try
                {
                    using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                    {
                        var EliminarProducto = db.producto.FirstOrDefault(p => p.codigo_producto == codigo_producto);
                        if (EliminarProducto == null)
                        {
                            ViewBag.ValorMensaje = 0;
                            ViewBag.MensajeProceso = "El producto que intenta eliminar ya no existe.";
                            return RedirectToAction("Index");
                        }

                        db.producto.Remove(EliminarProducto);
                        db.SaveChanges();

                        ViewBag.ValorMensaje = 1;
                        ViewBag.MensajeProceso = "Producto eliminado exitosamente.";
                        return RedirectToAction("Index");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al eliminar el producto.";
                    return RedirectToAction("Delete", new { codigo_producto });
                }
            }


        //GET: Producto/Update
        [HttpGet]
        public ActionResult Update(string codigo_producto)
        {
            

            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                var productoVM = db.producto.FirstOrDefault(p => p.codigo_producto == codigo_producto);
                if (productoVM == null)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al cargar el producto.";
                    return RedirectToAction("Index");
                }

                var ProductoExistente = new ProductoVM
                {
                    codigo_producto = productoVM.codigo_producto,
                    descripcion = productoVM.descripcion,
                    estado = productoVM.estado,
                    precio = productoVM.precio,
                    id_tipoProducto = productoVM.id_tipoProducto
                };

                CargarDropdownTipos(ProductoExistente.id_tipoProducto);

                string descripcionTipo = ObtenerTipoAsociado(ProductoExistente.id_tipoProducto);
                ViewBag.DescripcionTipoProducto = descripcionTipo ?? "No asignado";
                return View(ProductoExistente);
            }
        }

        [HttpPost]
        public ActionResult ActualizarProducto(ProductoVM productoActualizado)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Update", new { codigo_producto = productoActualizado.codigo_producto });
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var productoExistente = db.producto.FirstOrDefault(p => p.codigo_producto == productoActualizado.codigo_producto);
                    if (productoExistente == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El producto que intenta actualizar no está disponible.";
                        return RedirectToAction("Index");
                    }


                    productoExistente.codigo_producto = productoActualizado.codigo_producto.ToUpper();
                    productoExistente.descripcion = productoActualizado.descripcion.ToUpper();
                    productoExistente.precio = productoActualizado.precio;
                    productoExistente.estado = productoActualizado.estado.ToUpper();
                    productoExistente.id_tipoProducto = productoActualizado.id_tipoProducto;

                    db.SaveChanges();

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Producto actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
            }

            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al actualizar el producto.";
                return RedirectToAction("Update", new { codigo_producto = productoActualizado.codigo_producto });
            }
        }





    }
}