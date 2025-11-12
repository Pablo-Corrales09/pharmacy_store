using PIV_PF_ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class ClienteController : Controller
    {
        // GET: cliente
        public ActionResult Index()
        {
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var cliente = db.cliente.ToList();
                return View(cliente);

            }
        }

        public ActionResult ObtenerClientes()
        {
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var usuarios = db.cliente.ToList();
                return View("Index", usuarios);

            }
        }

      
        //GET: cliente/Create
        public ActionResult Create() {
            return View();
        
        }

        [HttpPost]
        public ActionResult CrearUsuario(cliente c)
        {
            try
            {
                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    db.cliente.Add(c);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch { 
                return View("Error");

            }
        }


    }//Fin clienteController
}//Fin namespace


//public ActionResult CrearUsuario(cliente c)
//{
//    try
//    {
//        using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
//        {
//            db.cliente.Add(c);
//            db.SaveChanges();
//        }

//        return Content("OK guardado!");
//    }
//    catch (DbEntityValidationException ex)
//    {
//        foreach (var eve in ex.EntityValidationErrors)
//        {
//            foreach (var ve in eve.ValidationErrors)
//            {
//                Response.Write("Propiedad: " + ve.PropertyName + " — Error: " + ve.ErrorMessage + "<br>");
//            }
//        }
//        return Content("Error validación");
//    }
//}