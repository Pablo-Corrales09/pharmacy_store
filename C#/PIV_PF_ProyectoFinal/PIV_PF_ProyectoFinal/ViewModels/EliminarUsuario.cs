using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class EliminarUsuario
    {

        [Required]
        [Display(Name = "Cédula del usuario")]
        public string cedula { get; set; }

        [Display(Name = "Nombre completo")]
        public string nombre_completo { get; set; }

        [Display(Name = "Correo electrónico")]
        public string correo_electronico { get; set; }

        [Display(Name = "Tipo de usuario")]
        public string tipo_usuario { get; set; }

        [Display(Name = "Estado del usuario")]
        public string estado { get; set; }
    }
}