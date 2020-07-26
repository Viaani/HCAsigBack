using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.MDP
{
    // Atributos de la clase Asignatura
    public class Asignatura
    {
        public string Nombre { get; set; }
        public int Creditos { get; set; }
        public string Codigo { get; set; }
        public int NumeroDecreto { get; set; }
    }
    public class AsignaturaExterna
    {
        public string Nombre { get; set; }
        public int Creditos { get; set; }
        public string Codigo { get; set; }
        public string Codigo_ProgramaExterno { get; set; }
    }
    public class AsignaturaNota
    {
        public string Nombre { get; set; }
        public int Creditos { get; set; }
        public string Codigo { get; set; }
        public int NumeroDecreto { get; set; }
        public float Nota { get; set; }
    }
    public class AsignaturaExternaNota
    {
        public string Nombre { get; set; }
        public int Creditos { get; set; }
        public string Codigo { get; set; }
        public string Codigo_ProgramaExterno { get; set; }
        public float nota { get; set; }
    }
}
