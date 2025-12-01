using PIV_PF_ProyectoFinal.Models;
using PIV_PF_ProyectoFinal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
            return View();
        }

        public ActionResult ObtenerClientes()
        {
            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                var usuarios = db.cliente.ToList();
                return View("Index", usuarios);

            }
        }


        //GET: cliente/Create
        [HttpGet]
        public ActionResult Create() {
            return View(new AgregaClientes());
        
        }

        //Agrega un registro en la tabla 'Cliente' dentro de la base de datos
        //POST: cliente/Create
        [HttpPost]
        public ActionResult CrearCliente(AgregaClientes c)
        {
            try
            {
                if (!ModelState.IsValid) {
                    return View("Create", c);
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    cliente nuevoCliente = new cliente();
                    nuevoCliente.cedula = c.cedula;
                    nuevoCliente.nombre_completo = c.nombre_completo.ToUpper();
                    nuevoCliente.correo_electronico = c.correo_electronico.ToLower();
                    db.cliente.Add(nuevoCliente);
                    db.SaveChanges();
                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Cliente agregado exitosamente.";
                }
                return View("Create");
            }
            catch (Exception ex){
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al agregar el cliente: " + ex.Message;
                return View("Create", c);

            }
        }

        //Carga la vista 'Read' con el listado de clientes registrados
        //GET: cliente/Read
        [HttpGet]
        public ActionResult Read()
        {
            List < ListaClientes > listCli = null;
            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                listCli = (from client in db.cliente
                            select new ListaClientes
                            {
                                cedula = client.cedula,
                                nombre_completo = client.nombre_completo,
                                correo_electronico = client.correo_electronico
                            }).ToList();

                return View(listCli);

            }

        }


        //Obtener objeto cliente por id
        private cliente ObtenerCliente(string cedula)
        {
            using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
            {
                if (string.IsNullOrEmpty(cedula))
                {
                    return null;
                }
                return db.cliente.FirstOrDefault(u => u.cedula == cedula);
            }
        }


        //Muestra los detalles de un cliente en particular
        //GET: cliente/Details
        [HttpGet]
        public ActionResult Details(string cedula)
        {
            try
            {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {



                    var cliente = db.cliente.FirstOrDefault(u => u.cedula == cedula);
                    if (cliente == null)
                    {
                        return View("Details", null);
                    }
                    return View("Details", cliente);
                }
            }
            catch (Exception e)
            {
                return View("Read");
            }
        }


        // GET: Cliente/Update
        [HttpGet]
        public ActionResult Update()
        {
            return View();
        }

        //GET: cliente/BuscarEdit
        [HttpGet]
        public ActionResult BuscarCliente(string cedula, string sourceView)
        {
            try {
                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var cliente = db.cliente.FirstOrDefault(u => u.cedula == cedula);
                    if (cliente == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Cliente no encontrado";
                        return View(sourceView);
                    }
                    
                    if (sourceView == "Update") { 
                    var clienteViewModel = new EditarCliente
                     {
                       cedula = cliente.cedula,
                       nombre_completo = cliente.nombre_completo,
                       correo_electronico = cliente.correo_electronico
                       };
                        return View(sourceView, clienteViewModel);
                    }
                    else if(sourceView == "Delete") { 
                            var clienteViewModel = new EliminarCliente
                            {
                                cedula = cliente.cedula,
                                nombre_completo = cliente.nombre_completo,
                                correo_electronico = cliente.correo_electronico
                            };
                            return View(sourceView, clienteViewModel);
                        }
                    return View("Update");
                    }
                        
                }                            
            catch (Exception) 
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al buscar el cliente: ";
                return View("Update");
            }
        }


        //GET: cliente/Editarcliente
        [HttpGet]
        public ActionResult EditarCliente()
        {
            return View();

        }

        //POST: cliente/EditarCliente
        [HttpPost]
        public ActionResult EditarCliente(EditarCliente ClienteEdit) {
           
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Update", ClienteEdit);
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {    
                    var clienteExistente = db.cliente.FirstOrDefault(u => u.cedula == ClienteEdit.cedula);

                    if (clienteExistente == null) { 
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Cliente no encontrado.";
                        return View("Update", ClienteEdit);
                    }

                    clienteExistente.nombre_completo= ClienteEdit.nombre_completo.ToUpper();
                    clienteExistente.correo_electronico= ClienteEdit.correo_electronico.ToLower();
                    db.SaveChanges();

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Cliente editado exitosamente.";   
                }
                return View("Update", ClienteEdit);
            }
                catch (Exception) 
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al editar el cliente: ";
                    return View("Update", ClienteEdit);
                }
            
        }


        //GET: cliente/Delete
        [HttpGet]
        public ActionResult Delete()
        {
            return View();

        }

        //GET: cliente/EliminarCliente
        [HttpGet]
        public ActionResult EliminarCliente()
        {
            return View();

        }

        //POST: cliente/EliminarCliente
        [HttpPost]     
        public ActionResult EliminarCliente(EliminarCliente ClienteExistente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Delete", ClienteExistente);
                }

                using (PIV_PF_Proyecto_Final_Entities db = new PIV_PF_Proyecto_Final_Entities())
                {
                    var clienteEliminar = db.cliente.FirstOrDefault(u => u.cedula == ClienteExistente.cedula);

                    if (clienteEliminar == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Cliente no encontrado.";
                        return View("Delete", ClienteExistente);
                    }
                    db.factura.RemoveRange(db.factura.Where(f => f.id_cliente == clienteEliminar.id_cliente));
                    db.cliente.Remove(clienteEliminar);
                    db.SaveChanges();

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Cliente eliminado exitosamente.";
                }
                return RedirectToAction("Index", "Cliente");
            }
            catch (Exception)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al eliminar el cliente.";
                return View("Delete", ClienteExistente);
            }

        }

    }//Fin clienteController
}//Fin namespace

