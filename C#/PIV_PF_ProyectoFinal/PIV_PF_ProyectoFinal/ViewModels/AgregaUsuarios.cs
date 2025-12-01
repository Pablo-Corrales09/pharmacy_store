using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class AgregaUsuarios
    {
        [Required]
        [Display(Name = "Número de cédula")]
        public string cedula { get; set; }

        [Required]
        [Display(Name = "Nombre completo")]
        public string nombre_completo { get; set; }

        [Required]
        [Display(Name = "Correo electrónico")]
        public string correo_electronico { get; set; }

        [Required]
        [Display(Name = "Tipo de usuario")]
        public string tipo_usuario { get; set; }

        [Required]
        [Display(Name = "Estado del usuario")]
        public string estado { get; set; }
    }
}