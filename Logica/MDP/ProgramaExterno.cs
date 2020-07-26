using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MDP
{
    public class ProgramaExterno
    {
        public string[] AgregarProgramaExterno(string codigo, string nombre, string universidad)
        {
            Entidades.MDP.ProgramaExterno entidadMDPProgramaExterno = new Entidades.MDP.ProgramaExterno();
            entidadMDPProgramaExterno.Codigo = codigo;
            entidadMDPProgramaExterno.Nombre = nombre;
            entidadMDPProgramaExterno.Universidad = universidad;

            Datos.MDP.ProgramaExterno datosMDPProgramaExterno = new Datos.MDP.ProgramaExterno();

            return datosMDPProgramaExterno.AgregarProgramaExterno(entidadMDPProgramaExterno);
        }

        public List<Entidades.MDP.RetornoProgramaExterno> MostrarProgramaExterno(String codigo)
        {
            Datos.MDP.ProgramaExterno datosMDPProgramaExterno = new Datos.MDP.ProgramaExterno();
            Datos.MDP.Asignatura DatosAsignatura = new Datos.MDP.Asignatura();
            List<Entidades.MDP.RetornoProgramaExterno> retornoProgramasExternos = new List<Entidades.MDP.RetornoProgramaExterno>();
            var programasExternos = datosMDPProgramaExterno.MostrarProgramaExterno(codigo);
            foreach(var programaExterno in programasExternos)
            {
                Entidades.MDP.RetornoProgramaExterno retornoProgramaExterno = new Entidades.MDP.RetornoProgramaExterno();
                retornoProgramaExterno.Codigo = programaExterno.Codigo;
                retornoProgramaExterno.Nombre = programaExterno.Nombre;
                retornoProgramaExterno.Universidad = programaExterno.Universidad;
                retornoProgramaExterno.asignaturasExternas = new List<Entidades.MDP.AsignaturaExterna>();
                retornoProgramaExterno.asignaturasExternas = DatosAsignatura.MostrarProgramaExterno_asignaturas(programaExterno.Codigo);
                retornoProgramasExternos.Add(retornoProgramaExterno);
            }
            return retornoProgramasExternos;
        }

        public string[] EliminarProgramaExterno(string codigo)
        {
            Datos.MDP.ProgramaExterno datosMDPProgramaExterno = new Datos.MDP.ProgramaExterno();
            return datosMDPProgramaExterno.EliminarProgramaExterno(codigo);
        }

        public string[] EditarProgramaExterno(string nuevoCodigo, string codigo, string nombre, string universidad)
        {

            Entidades.MDP.ProgramaExterno entidadMDPProgramaExterno = new Entidades.MDP.ProgramaExterno();
            entidadMDPProgramaExterno.Codigo = codigo;
            entidadMDPProgramaExterno.Nombre = nombre;
            entidadMDPProgramaExterno.Universidad = universidad;

            Datos.MDP.ProgramaExterno datosMDPProgramaExterno = new Datos.MDP.ProgramaExterno();

            return datosMDPProgramaExterno.EditarProgramaExterno(entidadMDPProgramaExterno, nuevoCodigo);
        }
    }
}
