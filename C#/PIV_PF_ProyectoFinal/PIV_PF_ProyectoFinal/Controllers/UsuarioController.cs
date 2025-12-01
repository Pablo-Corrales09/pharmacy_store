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
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    if (db.usuario.Any(x => x.cedula == u.cedula))
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El usuario con este ID ya está registrado.";
                        return View("Create", u);
                    }
                    var nuevoUsuario = new usuario();
                    nuevoUsuario.cedula = u.cedula.ToUpper();
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
        public ActionResult Update(string cedula)
        {
            if (string.IsNullOrEmpty(cedula))
            {
                return View((EditarUsuario)null);
            }

            try {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var usuario = db.usuario.FirstOrDefault(u => u.cedula == cedula);

                    if (usuario == null)
                    {
                        return View((EditarUsuario)null);
                    }

                    var modeloVista = new EditarUsuario
                    {
                        cedula = usuario.cedula,
                        nombre_completo = usuario.nombre_completo,
                        correo_electronico = usuario.correo_electronico,
                        tipo_usuario = usuario.tipo_usuario,
                        estado = usuario.estado
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
                    return View(colaborador);
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var usuarioEditar = db.usuario.FirstOrDefault(u => u.cedula == colaborador.cedula);

                    usuarioEditar.nombre_completo = colaborador.nombre_completo.ToUpper();
                    usuarioEditar.correo_electronico = colaborador.correo_electronico.ToLower();
                    usuarioEditar.tipo_usuario = colaborador.tipo_usuario.ToUpper();
                    usuarioEditar.estado = colaborador.estado.ToUpper();
                    db.SaveChanges();

                    TempData["ValorMensaje"] = 1;
                    TempData["MensajeProceso"] = "Colaborador actualizado exitosamente.";

                    return RedirectToAction("Update", new { colaborador.cedula });
                }

            }
            catch (Exception)
            {
                TempData["ValorMensaje"] = 0;
                TempData["MensajeProceso"] = "Error al eliminar al colaborador.";
                return RedirectToAction("Update", new { colaborador.cedula });
            }
        }





        //GET: Usuario/Details
        [HttpGet]
        public ActionResult Details(string cedula)
        {
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    if (string.IsNullOrEmpty(cedula))
                    {
                        return View("Details", null);
                    }
                    var usuario = db.usuario.FirstOrDefault(u => u.cedula == cedula);
                    return View("Details", usuario);
                }
            }
            catch (Exception)
            {
                return View("Details", null);
            }
        }

        //Método privado para obtener la lista de usuarios desde la base de datos
        private List<ListaUsuarios> ObtenerUsuarios()
        {
            List<ListaUsuarios> userList = new List<ListaUsuarios>();
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    userList = (from user in db.usuario
                                select new ListaUsuarios
                                {
                                    cedula = user.cedula,
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
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
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
                            cedula = usuario.cedula,
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
                            cedula = usuario.cedula,
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
        public ActionResult Delete(string cedula)
        {
            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                var usuario = db.usuario.FirstOrDefault(u => u.cedula == cedula);

                if (usuario == null)
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al eliminar al colaborador.";
                    return RedirectToAction("Index");
                }

                var usuarioExistente = new EliminarUsuario
                {
                    cedula = usuario.cedula,
                    nombre_completo = usuario.nombre_completo,
                    correo_electronico = usuario.correo_electronico,
                    tipo_usuario = usuario.tipo_usuario,
                    estado = usuario.estado
                };

                return View(usuarioExistente);
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
                    return View("Delete", ColaboradorExistente);
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var usuarioEliminar = db.usuario.FirstOrDefault(u => u.cedula == ColaboradorExistente.cedula);

                    if (usuarioEliminar == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "El colaborador que intenta eliminar ya no existe.";
                        return RedirectToAction("Index");
                    }

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