using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MU
{
    public class Usuario
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string run { get; set; }
        public string email { get; set; }
        public string area { get; set; }
        public string password { get; set; }
        public string tipoUsuario { get; set; }
    }
    public class Credenciales
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class Token
    {
        public string token { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string run { get; set; }
        public string email { get; set; }
        public string area { get; set; }
        public string tipoUsuario { get; set; }
    }
}