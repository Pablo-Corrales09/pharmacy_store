using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class EditarUsuario
    {
        [Required(ErrorMessage = "La cédula es requerida")]
        [Display(Name = "Cedula del usuario")]
        public string cedula { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido")]
        [Display(Name = "Nombre completo")]
        public string nombre_completo { get; set; }

        [Required(ErrorMessage ="El correo electrónico es requerido")]
        [Display(Name = "Correo electrónico")]
        public string correo_electronico { get; set; }

        [Required(ErrorMessage ="El tipo de usuario es requerido")]
        [Display(Name = "Tipo de usuario")]
        public string tipo_usuario { get; set; }

        [Required(ErrorMessage ="El estadp del usuario es requerido")]
        [Display(Name = "Estado del usuario")]
        public string estado { get; set; }
    }
}