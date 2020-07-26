using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MDP
{
    public class ProgramaExterno
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Universidad { get; set; }
    }

    public class RetornoProgramaExterno
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Universidad { get; set; }
        public List<AsignaturaExterna> asignaturasExternas = new List<AsignaturaExterna>();
    }
}
