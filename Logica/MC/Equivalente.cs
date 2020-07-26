using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MC
{
    public class Equivalente
    {
        public string[] AgregarEquivalenteConvalidacion(Entidades.MC.Equivalentes ListaEquivalentes)
        {
            Datos.MC.Equivalente datosMCEquivalente = new Datos.MC.Equivalente();
            return datosMCEquivalente.AgregarEquivalenteConvalidacion(ListaEquivalentes);
        }

        public string[] AgregarEquivalenteHomologacion(Entidades.MC.Equivalentes ListaEquivalentes)
        {
            Datos.MC.Equivalente datosMCEquivalente = new Datos.MC.Equivalente();
            return datosMCEquivalente.AgregarEquivalenteHomologacion(ListaEquivalentes);
        }

        public List<Entidades.MC.AsignaturasEquivalentes> MostrarEquivalenteConvalidacion(string programaOrigen, string programaObjetivo)
        {
            Datos.MC.Equivalente datosMCEquivalente = new Datos.MC.Equivalente();
            return datosMCEquivalente.BuscarEquivalenteConvalidacion(programaOrigen, programaObjetivo);
        }

        public List<Entidades.MC.AsignaturasEquivalentes> MostrarEquivalenteHomologacion(string programaOrigen, string programaObjetivo)
        {
            Datos.MC.Equivalente datosMCEquivalente = new Datos.MC.Equivalente();
            return datosMCEquivalente.BuscarEquivalenteHomologacion(programaOrigen, programaObjetivo);
        }

        public string[] EliminarEquivalenteConvalidacion(Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes, string programaOrigen, string programaObjetivo)
        {
            Datos.MC.Equivalente datosMCEquivalente = new Datos.MC.Equivalente();
            List<Entidades.MC.Equivalente> listaEquivalente = new List<Entidades.MC.Equivalente>();

            foreach( Entidades.MDP.Asignatura asignaturaOriegen in asignaturasEquivalentes.asignaturasOrigen)
            {
                foreach ( Entidades.MDP.Asignatura asignaturaObjetivo in asignaturasEquivalentes.asignaturasObjetivo)
                {
                    Entidades.MC.Equivalente equivalente = new Entidades.MC.Equivalente();
                    equivalente.programaOrigen = programaOrigen;
                    equivalente.programaObjetivo = programaObjetivo;
                    equivalente.codigoAsignaturaOrigen = asignaturaOriegen.Codigo;
                    equivalente.codigoAsignaturaObjetivo = asignaturaObjetivo.Codigo;
                    listaEquivalente.Add(equivalente);
                }
            }
            return datosMCEquivalente.EliminarEquivalenteConvalidacion(listaEquivalente);
        }

        public string[] EliminarEquivalenteHomologacion(Entidades.MC.AsignaturasEquivalentes asignaturasEquivalentes, string programaOrigen, string programaObjetivo)
        {
            Datos.MC.Equivalente datosMCEquivalente = new Datos.MC.Equivalente();
            List<Entidades.MC.Equivalente> listaEquivalente = new List<Entidades.MC.Equivalente>();

            foreach (Entidades.MDP.Asignatura asignaturaOriegen in asignaturasEquivalentes.asignaturasOrigen)
            {
                foreach (Entidades.MDP.Asignatura asignaturaObjetivo in asignaturasEquivalentes.asignaturasObjetivo)
                {
                    Entidades.MC.Equivalente equivalente = new Entidades.MC.Equivalente();
                    equivalente.programaOrigen = programaOrigen;
                    equivalente.programaObjetivo = programaObjetivo;
                    equivalente.codigoAsignaturaOrigen = asignaturaOriegen.Codigo;
                    equivalente.codigoAsignaturaObjetivo = asignaturaObjetivo.Codigo;
                    listaEquivalente.Add(equivalente);
                }
            }
            return datosMCEquivalente.EliminarEquivalenteHomologacion(listaEquivalente);
        }
    }

}
