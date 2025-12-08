using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class cLogin
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserUsuario { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public string CtrUsuario { get; set; }
    }
}