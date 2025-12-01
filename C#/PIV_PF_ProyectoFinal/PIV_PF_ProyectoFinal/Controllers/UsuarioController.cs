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
            var usuario = User.Identity.Name;
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
            return View(new AgregaUsuarios());
        }

        [HttpPost]
        public ActionResult CrearUsuario(AgregaUsuarios u)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return View("Create", u);
                }
                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    if (db.usuario.Any(x => x.id_usuario == u.id_usuario))
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El usuario con este ID ya está registrado.";
                        return View("Create", u);
                    }
                    var nuevoUsuario = new usuario();
                    nuevoUsuario.id_usuario = u.id_usuario.ToUpper();
                    nuevoUsuario.nombre_completo = u.nombre_completo.ToUpper();
                    nuevoUsuario.correo_electronico = u.correo_electronico.ToLower();
                    nuevoUsuario.tipo_usuario = u.tipo_usuario.ToUpper();
                    nuevoUsuario.estado = u.estado.ToUpper();
                    db.usuario.Add(nuevoUsuario);
                    db.SaveChanges();
                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Usuario registrado exitosamente.";
                }
                return View("Create");

            } catch (Exception ex) {

                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al agregar el usuario" + ex.Message;
                return View("Create", u);

            }
        }


        //GET: Usuario/Update
        [HttpGet]
        public ActionResult Update(string id)
        {
            try {
                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    var usuarioDb = db.usuario.Find(id);

                    if (usuarioDb == null)
                    {
                        return View((EditarUsuario)null);
                    }

                    var modeloVista = new EditarUsuario
                    {
                        id_usuario = usuarioDb.id_usuario,
                        nombre_completo = usuarioDb.nombre_completo,
                        correo_electronico = usuarioDb.correo_electronico,
                        tipo_usuario = usuarioDb.tipo_usuario,
                        estado = usuarioDb.estado
                    };

                    return View(modeloVista);

                }
            }
            catch (Exception ex){
                return View((EditarUsuario)null);
            }
        }

        [HttpPost]
        public ActionResult Update(EditarUsuario colaborador)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View((EditarUsuario)null);
                }

                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    var usuarioEditar = db.usuario.Find(colaborador.id_usuario);

                    usuarioEditar.nombre_completo = colaborador.nombre_completo.ToUpper();
                    usuarioEditar.correo_electronico = colaborador.correo_electronico.ToLower();
                    usuarioEditar.tipo_usuario = colaborador.tipo_usuario.ToUpper();
                    usuarioEditar.estado = colaborador.estado.ToUpper();
                    db.SaveChanges();

                    TempData["ValorMensaje"] = 1;
                    TempData["MensajeProceso"] = "Colaborador actualizado exitosamente.";

                    return RedirectToAction("Update", new { id = colaborador.id_usuario });
                }

            }
            catch (Exception)
            {
                TempData["ValorMensaje"] = 0;
                TempData["MensajeProceso"] = "Error al eliminar al colaborador.";
                return RedirectToAction("Update", new { id = colaborador.id_usuario });
            }
        }





        //GET: Usuario/Details
        [HttpGet]
        public ActionResult Details(string id)
        {
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }
                var usuario = db.usuario.Find(id);

                if (usuario == null)
                {
                    return View("Details");
                }
                return View("Details", usuario);
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

        [HttpGet]
        public ActionResult BuscarUsuario(string id, string sourceView)
        {
            try
            {
                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    var usuario = db.usuario.Find(id);
                    if (usuario == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Colaborador no encontrado";
                        return View(sourceView);
                    }

                    if (sourceView == "Update")
                    {
                        var usuarioViewModel = new EditarUsuario
                        {
                            id_usuario = usuario.id_usuario,
                            nombre_completo = usuario.nombre_completo,
                            correo_electronico = usuario.correo_electronico,
                            tipo_usuario = usuario.tipo_usuario,
                            estado = usuario.estado
                        };
                        return View(sourceView, usuarioViewModel);
                    }
                    else if (sourceView == "Delete")
                    {
                        var clienteViewModel = new EliminarUsuario
                        {
                            id_usuario = usuario.id_usuario,
                            nombre_completo = usuario.nombre_completo,
                            correo_electronico = usuario.correo_electronico,
                            tipo_usuario = usuario.tipo_usuario,
                            estado = usuario.estado
                        };
                        return View(sourceView, clienteViewModel);
                    }
                    return View("Update");
                }

            }
            catch (Exception)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al buscar el colaborador.";
                return View("Update");
            }
        }

        //GET: Usuario/Delete
        [HttpGet]
        public ActionResult Delete(string id)
        {
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var clienteDB = db.usuario.Find(id);

                if (clienteDB == null)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al eliminar al colaborador.";
                    return RedirectToAction("Index");
                }

                var modeloVista = new EliminarUsuario
                {
                    id_usuario = clienteDB.id_usuario,
                    nombre_completo = clienteDB.nombre_completo,
                    correo_electronico = clienteDB.correo_electronico,
                    tipo_usuario = clienteDB.tipo_usuario,
                    estado = clienteDB.estado
                };

                return View(modeloVista);
            }              
        }


        //POST: usuario/Delete
        [HttpPost]
        public ActionResult Delete(EliminarUsuario ColaboradorExistente)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Delete");
                }

                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    var usuarioEliminar = db.usuario.Find(ColaboradorExistente.id_usuario);
                    db.usuario.Remove(usuarioEliminar);
                    db.SaveChanges();

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Colaborador eliminado exitosamente.";
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al eliminar al colaborador.";
                return View("Delete", ColaboradorExistente);
            }

        }
    }
}