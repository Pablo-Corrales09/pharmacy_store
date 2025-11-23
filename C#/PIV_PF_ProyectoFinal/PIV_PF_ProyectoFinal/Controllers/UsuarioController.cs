using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIV_PF_ProyectoFinal.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [HttpGet]
        public ActionResult Index()
        {
            return View(ObtenerUsuarios());
        }

        //GET: Usuario/Seleccion
        [HttpGet]
        public ActionResult Seleccion()
        {
            return View();
        }

        //GET: Usuario/Create
        [HttpGet]
        public ActionResult Create() {
            return View();
        }


        //POST: Usuario/Update
        [HttpGet]
        public ActionResult Update()
        {
            return View();
        }

        //POST: Usuario/Delete
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }



        //GET: Usuario/Details
        [HttpGet]
        public ActionResult Details()
        {
            List<ListaUsuarios> userList = null;
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                userList = (from user in db.usuario
                            select new ListaUsuarios
                            {
                                id_usuario = user.id_usuario,
                                nombre_completo = user.nombre_completo,
                                correo_electronico = user.correo_electronico,
                                tipo_usuario = user.tipo_usuario,
                                estado = user.estado
                            }).ToList();

                return View(userList);

            }

        }

        //Método privado para obtener la lista de usuarios desde la base de datos
        private List<ListaUsuarios> ObtenerUsuarios()
        {
            List<ListaUsuarios> userList = new List<ListaUsuarios>();
            try
            {
                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    userList = (from user in db.usuario
                                select new ListaUsuarios
                                {
                                    id_usuario = user.id_usuario,
                                    nombre_completo = user.nombre_completo,
                                    correo_electronico = user.correo_electronico,
                                    tipo_usuario = user.tipo_usuario,
                                    estado = user.estado
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
            }
            return userList;
        }
    }
}