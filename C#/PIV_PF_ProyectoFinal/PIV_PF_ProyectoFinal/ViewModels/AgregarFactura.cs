using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class AgregarFactura
    {
        [Required(ErrorMessage = "El código de factura es requerido")]
        [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
        [Display(Name = "Número de factura")]
        public string codigo_factura { get; set; }

        [Required(ErrorMessage = "La fecha de compra es requerida")]
        [Display(Name = "Fecha de compra")]
        [DataType(DataType.DateTime)]
        public System.DateTime fecha_compra { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Cliente inválido")]
        [Display(Name = "Cliente")]
        public int id_cliente { get; set; }

        [Required(ErrorMessage = "El método de pago es requerido")]
        [StringLength(50)]
        [Display(Name = "Método de pago")]
        public string metodo_pago { get; set; }

        [Required(ErrorMessage = "El subtotal es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El subtotal debe ser mayor a 0")]
        [Display(Name = "Sub total")]
        public decimal sub_total { get; set; }

        [Display(Name = "Total")]
        public decimal total { get; set; }

        [Display(Name = "IVA")]
        public decimal iva { get; set; }

        [Display(Name = "Detalles de la factura")]
        public List<DetalleFacturaVM> detalles { get; set; } = new List<DetalleFacturaVM>();
    }
}