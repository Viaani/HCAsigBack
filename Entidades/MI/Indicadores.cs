using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MI
{
    public class Indicadores
    {
        public int cantidadProgramas { get; set; }
        public int cantidadProgramasExternos { get; set; }
        public int cantidadDecretos { get; set; }
        public int cantidadHomologaciones { get; set; }
        public int cantidadConvalidaciones { get; set; }
        public List<CantidadEquivalenciasUsuario> cantidadEquivalenciasUsuario { get; set; }
    }
    public class CantidadEquivalenciasUsuario
    {
        public string secretarioAcademico { get; set; }
        public int cantidadConvalidaciones { get; set; }
        public int cantidadHomologaciones { get; set; }
    }
}
