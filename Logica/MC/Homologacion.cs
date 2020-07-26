using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MC
{
    public class Homologacion
    {
        public string[] AgregarHomologacion(Entidades.MC.ConvalidacionHomologacion homologacion)
        {
            Datos.MC.Homologacion datosMCHomologacion = new Datos.MC.Homologacion();
            List<Entidades.MC.ConvalidacionHomologacionEquivalenteNota> homologacionesEquivalentes = new List<Entidades.MC.ConvalidacionHomologacionEquivalenteNota>();

            foreach (Entidades.MC.AsignaturasEquivalentesNota asignaturasEquivalentes in homologacion.listaAsignaturasEquivalentes)
            {
                foreach (Entidades.MDP.AsignaturaNota asignaturaOrigen in asignaturasEquivalentes.asignaturasOrigen)
                {
                    foreach (Entidades.MDP.Asignatura asignaturaObjetivo in asignaturasEquivalentes.asignaturasObjetivo)
                    {
                        Entidades.MC.ConvalidacionHomologacionEquivalenteNota homologacionEquivalente = new Entidades.MC.ConvalidacionHomologacionEquivalenteNota();
                        homologacionEquivalente.run = homologacion.run;
                        homologacionEquivalente.programaOrigen = homologacion.programaOrigen;
                        homologacionEquivalente.programaObjetivo = homologacion.programaObjetivo;
                        homologacionEquivalente.codigoAsignaturaOrigen = asignaturaOrigen.Codigo;
                        homologacionEquivalente.codigoAsignaturaObjetivo = asignaturaObjetivo.Codigo;
                        homologacionEquivalente.nota = asignaturaOrigen.Nota;
                        homologacionesEquivalentes.Add(homologacionEquivalente);
                    }
                }
            }
            return datosMCHomologacion.AgregarHomologacion(homologacion, homologacionesEquivalentes);
        }

        public List<Entidades.MC.RetornoHomologacion> MostrarHomologaciones()
        {
            Datos.MC.Homologacion datosMCHomologacion = new Datos.MC.Homologacion();
            List<Entidades.MC.RetornoHomologacion> listaEnidadesMCHomologacion = datosMCHomologacion.MostrarHomologacion();
            foreach (Entidades.MC.RetornoHomologacion homologacion in listaEnidadesMCHomologacion)
            {
                homologacion.listaAsignaturasEquivalentes = new List<Entidades.MC.AsignaturasEquivalentesNota>();
                homologacion.listaAsignaturasEquivalentes = datosMCHomologacion.MostrarHomologacion_equivalente(homologacion.programaOrigen.Codigo, homologacion.programaObjetivo.Codigo, homologacion.run);
            }
            return listaEnidadesMCHomologacion;
        }

        public string[] EliminarHomologacion(string id)
        {
            Datos.MC.Homologacion DatosMChomologacion = new Datos.MC.Homologacion();
            return DatosMChomologacion.EliminarHomologacion(id);
        }
    }
}
