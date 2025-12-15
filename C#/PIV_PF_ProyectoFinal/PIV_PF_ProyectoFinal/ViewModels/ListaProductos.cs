using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class ListaProductos
    {

        [Display(Name = "Código de Producto")]
        public string codigo_producto { get; set; }

        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Display(Name = "Precio")]
        public decimal precio { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; }

        [Display(Name = "Código del tipo")]
        public int id_tipoProducto { get; set; }
    }
}