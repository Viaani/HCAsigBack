using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MC
{
    public class Convalidacion
    {
        public string[] AgregarConvalidacion(Entidades.MC.ConvalidacionHomologacion convalidacion)
        {
            Datos.MC.Convalidacion datosMCConvalidacion = new Datos.MC.Convalidacion();
            List<Entidades.MC.ConvalidacionHomologacionEquivalenteNota> convalidacionEquivalentes = new List<Entidades.MC.ConvalidacionHomologacionEquivalenteNota>();

            foreach (Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes in convalidacion.listaAsignaturasEquivalentes)
            {
                foreach (Entidades.MDP.AsignaturaNota asignaturaOrigen in asignaturasEquivalentes.asignaturasOrigen)
                {
                    foreach (Entidades.MDP.Asignatura asignaturaObjetivo in asignaturasEquivalentes.asignaturasObjetivo)
                    {
                        Entidades.MC.ConvalidacionHomologacionEquivalenteNota convalidacionEquivalente = new Entidades.MC.ConvalidacionHomologacionEquivalenteNota();
                        convalidacionEquivalente.run = convalidacion.run;
                        convalidacionEquivalente.programaOrigen = convalidacion.programaOrigen;
                        convalidacionEquivalente.programaObjetivo = convalidacion.programaObjetivo;
                        convalidacionEquivalente.codigoAsignaturaOrigen = asignaturaOrigen.Codigo;
                        convalidacionEquivalente.codigoAsignaturaObjetivo = asignaturaObjetivo.Codigo;
                        convalidacionEquivalente.nota = asignaturaOrigen.Nota;
                        convalidacionEquivalentes.Add(convalidacionEquivalente);
                    }
                }
            }
            return datosMCConvalidacion.AgregarConvalidacion(convalidacion, convalidacionEquivalentes);
        }

        public List<Entidades.MC.RetornoConvalidacionNota> MostrarConvalidaciones()
        {
            Datos.MC.Convalidacion datosMCConvalidacion = new Datos.MC.Convalidacion();
            List<Entidades.MC.RetornoConvalidacionNota> listaEnidadesMCConvalidacion = datosMCConvalidacion.MostrarConvalidacion();
            foreach (Entidades.MC.RetornoConvalidacionNota convalidacion in listaEnidadesMCConvalidacion)
            {
                convalidacion.listaAsignaturasEquivalentes = new List<Entidades.MC.AsignaturasEquivalentesNota>();
                convalidacion.listaAsignaturasEquivalentes = datosMCConvalidacion.MostrarConvalidacion_equivalente(convalidacion.programaOrigen.Codigo, convalidacion.programaObjetivo.Codigo, convalidacion.run);
            }
            return listaEnidadesMCConvalidacion;
        }

        public string[] EliminarConvalidacion (String id)
        {
            Datos.MC.Convalidacion DatosMCconvalidacion = new Datos.MC.Convalidacion();
            return DatosMCconvalidacion.EliminarConvalidacion(id);
        }
    }
}
