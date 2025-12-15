using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class ContFactura
    {
        [Display(Name = "Factura")]
        public AgregarFactura Factura { get; set; }

        [Display(Name = "Detalles de la factura")]
        public DetalleFacturaVM Detalle { get; set; }

        [Display(Name = "Productos")]
        public ListaProductos Productos { get; set; }

        [Display(Name = "Información del cliente")]
        public ClientesViewModel Clientes { get; set; }
    }
}