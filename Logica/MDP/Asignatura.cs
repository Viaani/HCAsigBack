using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MDP
{
    public class Asignatura
    {
        public string[] AgregarAsignatura(string codigo, string nombre, int creditos, int numeroDecreto, string path)
        {
            Entidades.MDP.Asignatura entidadMDPAsignatura = new Entidades.MDP.Asignatura();
            entidadMDPAsignatura.Codigo = codigo;
            entidadMDPAsignatura.Nombre = nombre;
            entidadMDPAsignatura.Creditos = creditos;
            entidadMDPAsignatura.NumeroDecreto = numeroDecreto;

            Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();

            return datosMDPAsignatura.AgregarAsignatura(entidadMDPAsignatura, path);
        }

        public string[] AgregarAsignaturaExterna(string codigo, string nombre, int creditos, string codigo_programaExterno, string path)
        {
            Entidades.MDP.AsignaturaExterna entidadMDPAsignatura = new Entidades.MDP.AsignaturaExterna();
            entidadMDPAsignatura.Codigo = codigo;
            entidadMDPAsignatura.Nombre = nombre;
            entidadMDPAsignatura.Creditos = creditos;
            entidadMDPAsignatura.Codigo_ProgramaExterno = codigo_programaExterno;

            Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();

            return datosMDPAsignatura.AgregarAsignaturaExterna(entidadMDPAsignatura, path);
        }

        public List<Entidades.MDP.Asignatura> MostrarAsignatura(String id)
        {
            Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();
            return datosMDPAsignatura.MostrarAsignatura(id);
        }

        //public List<Entidades.MDP.Asignatura> MostrarDecretos_asignaturas(int id)
        //{
        //    Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();
        //    return datosMDPAsignatura.MostrarDecretos_asignaturas(id);
        //}

        //public List<Entidades.MDP.AsignaturaExterna> MostrarProgramaExterno_asignaturas(string codigo_ProgramaExterno)
        //{
        //    Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();
        //    return datosMDPAsignatura.MostrarProgramaExterno_asignaturas(codigo_ProgramaExterno);
        //}

        public string[] EliminarAsignatura(string id)
        {
            Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();
            return datosMDPAsignatura.EliminarAsignatura(id);
        }

        public string[] EditarAsignatura(string nuevoCodigo, string codigo, string nombre, int creditos)
        {

            Entidades.MDP.Asignatura entidadMDPAsignatura = new Entidades.MDP.Asignatura();
            entidadMDPAsignatura.Codigo = codigo;
            entidadMDPAsignatura.Nombre = nombre;
            entidadMDPAsignatura.Creditos = creditos;

            Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();

            return datosMDPAsignatura.EditarAsignatura(entidadMDPAsignatura, nuevoCodigo);
        }

        public string[] EditarAsignaturaArchivo(string nuevoCodigo, string codigo, string nombre, int creditos, string path)
        {
            Entidades.MDP.Asignatura entidadMDPAsignatura = new Entidades.MDP.Asignatura();
            entidadMDPAsignatura.Codigo = codigo;
            entidadMDPAsignatura.Nombre = nombre;
            entidadMDPAsignatura.Creditos = creditos;

            Datos.MDP.Asignatura datosMDPAsignatura = new Datos.MDP.Asignatura();
            var respuesta = datosMDPAsignatura.EditarAsignatura(entidadMDPAsignatura, nuevoCodigo);
            if(respuesta[1] == "1")
            {
                return datosMDPAsignatura.EditarPathArchivo(nuevoCodigo, path);
            }
            else
            {
                return respuesta;
            }
        }

        public string[] ObtenerProgramaAcademico( string codigo )
        {
            Datos.MDP.Asignatura datosAsignatura = new Datos.MDP.Asignatura();
            return datosAsignatura.ObtenerProgramaAcademico(codigo);
        }
    }
}
