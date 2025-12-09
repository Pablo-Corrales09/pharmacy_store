using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class ProductoVM
    {
        [Required]
        [Display(Name = "Código de Producto")]
        public string codigo_producto { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public decimal precio { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string estado { get; set; }

        [Required]
        [Display(Name = "Código del tipo")]
        public int id_tipoProducto { get; set; }
    }
}