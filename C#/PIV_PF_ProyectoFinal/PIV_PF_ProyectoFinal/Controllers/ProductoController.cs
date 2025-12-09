using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
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

        //GET: Producto/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ViewModels.ProductoVM());
        }

        //POST: Producto/Create
        [HttpPost]
        public ActionResult Create(ViewModels.ProductoVM p)
        {
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


    }
}