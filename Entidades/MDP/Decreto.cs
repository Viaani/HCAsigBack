using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MDP
{
    public class Decreto
    {
        public int numero { get; set; }
        public string fecha { get; set; }
    }

    public class RetornoDecreto
    {
        public int numero { get; set; }
        public string fecha { get; set; }
        public List<Asignatura> asignaturas = new List<Asignatura>();
    }
}
