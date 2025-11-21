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
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var usuarios = db.cliente.ToList();
                return View("Index", usuarios);

            }
        }

      
        //GET: cliente/Create
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

                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    cliente nuevoCliente = new cliente();
                    nuevoCliente.id_cliente = c.id_cliente;
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
        public ActionResult Read()
        {
            List < ListaClientes > listCli = null;
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                listCli = (from client in db.cliente
                            select new ListaClientes
                            {
                                id_cliente = client.id_cliente,
                                nombre_completo = client.nombre_completo,
                                correo_electronico = client.correo_electronico
                            }).ToList();

                return View(listCli);

            }

        }


        //Obtener objeto cliente por id
        private cliente ObtenerCliente(string id)
        {
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                if (string.IsNullOrEmpty(id))
                {
                    return null;
                }
                return db.cliente.Find(id);               
            }
        }


        //Muestra los detalles de un cliente en particular
        //GET: cliente/Details
        public ActionResult Details(string id)
        {
                var cliente = ObtenerCliente(id);

                if (cliente == null)
                {
                    return View("Details");
                }
                return View("Details", cliente);

        }


        // GET: Cliente/Update
        public ActionResult Update()
        {
            return View();
        }

        //GET: cliente/BuscarEdit
        public ActionResult BuscarEdit(string id)
        {
            try {
                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {
                    var cliente = db.cliente.Find(id);
                    if (cliente == null)
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Cliente no encontrado";
                        return View("Update");
                    }
                    var clienteViewModel = new EditarCliente
                    {
                        id_cliente = cliente.id_cliente,
                        nombre_completo = cliente.nombre_completo,
                        correo_electronico = cliente.correo_electronico
                    };
                    return View("Update", clienteViewModel);
                }                
            }
            catch (Exception ex) 
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Error al buscar el cliente: ";
                return View("Update");
            }
        }


        //GET: cliente/Editarcliente
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
                    return View("Update");
                }

                using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
                {    
                    var clienteExistente = db.cliente.Find(ClienteEdit.id_cliente);
                    clienteExistente.nombre_completo= ClienteEdit.nombre_completo.ToUpper();
                    clienteExistente.correo_electronico= ClienteEdit.correo_electronico.ToLower();
                    db.SaveChanges();

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Cliente editado exitosamente.";   
                }
                return View("Update");
            }
                catch (Exception) 
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Error al editar el cliente: ";
                    return View("Update", ClienteEdit);
                }
            
        }


        //GET: cliente/Delete
        public ActionResult Delete()
        {
            return View();

        }

        //GET: cliente/EliminarCliente
        public ActionResult EliminarCliente()
        {
            return View();

        }

        //POST: cliente/EliminarCliente
        [HttpPost]
        public ActionResult EliminarCliente(string id)
        {

            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var cliente = db.cliente.Find(id);
                if (cliente == null)
                {
                    return View("Delete");
                }
                db.cliente.Remove(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //GET: cliente/BuscarDelete
        public ActionResult BuscarDelete(string id)
        {
            var cliente = ObtenerCliente(id);
            if (cliente == null)
            {
                return View("Delete");
            }
            return View("Delete", cliente);
        }

    }//Fin clienteController
}//Fin namespace

