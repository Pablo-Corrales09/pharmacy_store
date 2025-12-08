using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class listaUsuarios
    {
        [Required]
        [DisplayName("N° de código")]
        public string codigo_tipo { get; set; }

        [Required]
        [DisplayName("Descripción del tipo")]
        public string descripcion { get; set; }

    }
}