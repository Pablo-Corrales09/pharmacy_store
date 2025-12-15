using PIV_PF_ProyectoFinal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class DetalleFacturaVM
    {
        [Display(Name = "Número de consecutivo")]
        public int? num_consecutivo { get; set; }

        [Required]
        [Display(Name = "Número de factura")]
        public int id_factura { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public string codigo_producto { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor unitario debe ser mayor a 0")]
        [Display(Name = "Valor unitario")]
        public decimal valor_unitario { get; set; }

        [Required]
        [Display(Name = "Monto total")]
        public decimal total_linea { get; set; }

        public virtual factura factura { get; set; }
    }
}