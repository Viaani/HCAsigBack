using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MC
{
    public class Equivalente
    {
        public string programaOrigen { get; set; }
        public string programaObjetivo { get; set; }
        public string codigoAsignaturaOrigen { get; set; }
        public string codigoAsignaturaObjetivo { get; set; }
    }

    public class EquivalenteNota
    {
        public string programaOrigen { get; set; }
        public string programaObjetivo { get; set; }
        public string codigoAsignaturaOrigen { get; set; }
        public string codigoAsignaturaObjetivo { get; set; }
        public float nota { get; set; }
    }

    public class Equivalentes
    {
        public List<Equivalente> ListaEquivalente { get; set; }
    }

    public class AsignaturasEquivalentes
    {
        public List<MDP.Asignatura> asignaturasOrigen { get; set; }
        public List<MDP.Asignatura> asignaturasObjetivo { get; set; }
    }
    public class AsignaturasEquivalentesNota
    {
        public List<MDP.AsignaturaNota> asignaturasOrigen { get; set; }
        public List<MDP.Asignatura> asignaturasObjetivo { get; set; }
        public float nota;
    }
}
