using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class EliminarCliente
    {
        [Required]
        [DisplayName("Número de identificación")]
        public string id_cliente { get; set; }


        [DisplayName("Nombre completo")]
        public string nombre_completo { get; set; }

        [DisplayName("Correo electrónico")]
        public string correo_electronico { get; set; }
    }
}