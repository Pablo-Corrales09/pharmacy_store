using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class TipoProductoController : Controller
    {
        // GET: TipoProducto
        public ActionResult Index()
        {
            return View(ObtenerTipos());
        }

        private List<ViewModels.TipoProducto> ObtenerTipos()
        {
            List<ViewModels.TipoProducto> ProductoTypeList = new List<ViewModels.TipoProducto>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    ProductoTypeList = (from tipoProducto in db.tipoProducto
                                select new ViewModels.TipoProducto
                                {
                                    codigo_tipo = tipoProducto.codigo_tipo,
                                    descripcion = tipoProducto.descripcion
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return ProductoTypeList;
        }

        //GET: TipoProducto/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ViewModels.TipoProducto());
        }

        //POST: TipoProducto/Create
        [HttpPost]
        public ActionResult Create(ViewModels.TipoProducto t)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", t);
                }
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    if (db.tipoProducto.Any(x => x.codigo_tipo == t.codigo_tipo))
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El tipo de producto con este ID ya está registrado.";
                        return View("Create", t);
                    }
                    var nuevoProducto = new Models.tipoProducto();
                    nuevoProducto.codigo_tipo = t.codigo_tipo.ToUpper();
                    nuevoProducto.descripcion = t.descripcion.ToUpper();
                    db.tipoProducto.Add(nuevoProducto);
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
                return View("Create", t);

            }
        }


        //GET: TipoProducto/Delete
        [HttpGet]
        public ActionResult Delete(string codigo_tipo)
        {
            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                var tipo = db.tipoProducto.FirstOrDefault(t => t.codigo_tipo == codigo_tipo);

                if (tipo == null)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al eliminar el tipo de producto.";
                    return RedirectToAction("Index");
                }

                var tipoExistente = new TipoProducto
                {
                    codigo_tipo = tipo.codigo_tipo,
                    descripcion = tipo.descripcion
                };

                return View(tipoExistente);
            }
        }


        //POST: TipoProducto/Delete
        [HttpPost]
        public ActionResult Delete(TipoProducto tipoExistente)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Delete", tipoExistente);
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var tipoEliminar = db.tipoProducto.FirstOrDefault(t => t.codigo_tipo == tipoExistente.codigo_tipo);

                    if (tipoEliminar == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El tipo de producto que intenta eliminar ya no existe.";
                        return RedirectToAction("Index");
                    }

                    db.tipoProducto.Remove(tipoEliminar);
                    db.SaveChanges();

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Tipo de producto eliminado exitosamente.";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al eliminar el tipo de producto.";
                return View("Delete", tipoExistente);
            }

        }

    }
}