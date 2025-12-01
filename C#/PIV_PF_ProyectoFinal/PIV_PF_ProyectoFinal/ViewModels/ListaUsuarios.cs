using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIV_PF_ProyectoFinal.ViewModels
{
    public class ListaUsuarios
    {
        public string cedula { get; set; }
        public string nombre_completo { get; set; }
        public string correo_electronico { get; set; }
        public string tipo_usuario { get; set; }
        public string estado { get; set; }
    }
}