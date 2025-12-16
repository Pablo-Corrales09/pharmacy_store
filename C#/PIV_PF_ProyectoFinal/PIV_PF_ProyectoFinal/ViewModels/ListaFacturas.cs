using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class ListaFacturas
    {
        public int id_factura { get; set; }
        public string codigo_factura { get; set; }
        public DateTime fecha_compra { get; set; }
        public string nombre_cliente { get; set; }
        public string metodo_pago { get; set; }
        public decimal sub_total { get; set; }
    }
}