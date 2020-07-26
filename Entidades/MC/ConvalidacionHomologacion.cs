using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MC
{
    public class ConvalidacionHomologacion
    {
        public String run { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public String programaOrigen { get; set; }
        public String programaObjetivo { get; set; }
        public List<AsignaturasEquivalentesNota> listaAsignaturasEquivalentes { get; set; }
        public String emailSecretario { get; set; }
    }
    public class ConvalidacionHomologacionEquivalente
    {
        public String run { get; set; }
        public String programaOrigen { get; set; }
        public String programaObjetivo { get; set; }
        public String codigoAsignaturaOrigen { get; set; }
        public String codigoAsignaturaObjetivo { get; set; }
    }
    public class ConvalidacionHomologacionEquivalenteNota
    {
        public String run { get; set; }
        public String programaOrigen { get; set; }
        public String programaObjetivo { get; set; }
        public String codigoAsignaturaOrigen { get; set; }
        public String codigoAsignaturaObjetivo { get; set; }
        public float nota { get; set; }
    }
    public class RetornoHomologacion
    {
        public String run { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public MDP.ProgramaExterno programaOrigen { get; set; }
        public MDP.Programa programaObjetivo { get; set; }
        public List<AsignaturasEquivalentesNota> listaAsignaturasEquivalentes { get; set; }
    }
    public class RetornoConvalidacion
    {
        public String run { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public MDP.Programa programaOrigen { get; set; }
        public MDP.Programa programaObjetivo { get; set; }
        public List<AsignaturasEquivalentes> listaAsignaturasEquivalentes { get; set; }
    }
    public class RetornoConvalidacionNota
    {
        public String run { get; set; }
        public String nombre { get; set; }
        public String apellido { get; set; }
        public MDP.Programa programaOrigen { get; set; }
        public MDP.Programa programaObjetivo { get; set; }
        public List<AsignaturasEquivalentesNota> listaAsignaturasEquivalentes { get; set; }
    }
}

