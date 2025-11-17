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
            return View();
        
        }

        //Agrega un registro en la tabla 'Cliente' dentro de la base de datos
        //POST: cliente/Create
        [HttpPost]
        public ActionResult CrearCliente(cliente c)
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

        //Carga la vista 'Read' con el listado de clientes registrados
        //GET: cliente/Read
        public ActionResult Read()
        {
            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var cliente = db.cliente.ToList();
                return View(cliente);

            }

        }


        //Muestra los detalles de un cliente en particular
        //GET: cliente/Details
        public ActionResult Details(string id)
        {
            // 1. Validar si el ID está vacío antes de buscar
            if (string.IsNullOrEmpty(id))
            {
                // Si no hay ID, volvemos a la vista sin un mensaje de error
                return View("Buscar");
            }

            using (PIV_PF_ProyectoFinalEntities db = new PIV_PF_ProyectoFinalEntities())
            {
                var cliente = db.cliente.Find(id);

                if (cliente == null)
                {
                    // 2. Si no se encuentra, establecemos un indicador de error
                    ViewBag.ClienteNoEncontrado = true;

                    // Volvemos a la misma vista, pero con el indicador de error
                    return View("Buscar");
                }

                // 3. Si se encuentra, enviamos el cliente a la vista (para mostrar los detalles)
                return View("Buscar", cliente);
            }
        }

        // GET: Cliente/Update
        public ActionResult Update()
        {
            return View();
        }


    }//Fin clienteController
}//Fin namespace

