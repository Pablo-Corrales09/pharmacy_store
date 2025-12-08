using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class LoginController : Controller
    {

        // GET: Ingresar
        [HttpGet]
        public ActionResult Ingresar()
        {
            return View("Ingresar");
        }


        [HttpPost]
        public ActionResult Ingresar(cLogin login)
        {
            if (!ModelState.IsValid)
            {
                return View("Ingresar");
            }

            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {

                var existenciaUsuario = db.UsuarioRegistrado.Where(p => p.user.ToUpper() == login.UserUsuario.ToUpper() && p.password == login.CtrUsuario).FirstOrDefault();

                if (existenciaUsuario != null)
                {
                    if (existenciaUsuario.rol== "ADMINISTRADOR")
                    {
                        Session["ROL"] = existenciaUsuario.rol;
                        Session.Timeout = 45;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Usuario no es ADMINISTRADOR";
                        return View("Ingresar");
                    }
                }
                else
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Usuario no existe";
                    return View("Ingresar");
                }

            }
        }


    }
}