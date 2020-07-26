using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MDP
{
    public class Programa
    {
        public string[] AgregarPrograma(string codigo, string nombre, int numero_decreto)
        {
            Entidades.MDP.Programa entidadMDPPrograma = new Entidades.MDP.Programa();
            entidadMDPPrograma.Codigo = codigo;
            entidadMDPPrograma.Nombre = nombre;
            entidadMDPPrograma.Numero_Decreto = numero_decreto;
            
            Datos.MDP.Programa datosMDPPrograma = new Datos.MDP.Programa();

            return datosMDPPrograma.AgregarPrograma(entidadMDPPrograma);
        }

        public List<Entidades.MDP.Programa> MostrarPrograma(String id)
        {
            Datos.MDP.Programa datosMDPPrograma = new Datos.MDP.Programa();
            return datosMDPPrograma.MostrarPrograma(id);
        }
        
        public string[] EliminarPrograma(string id)
        {
            Datos.MDP.Programa datosMDPPrograma = new Datos.MDP.Programa();
            return datosMDPPrograma.EliminarPrograma(id);
        }

        public string[] EditarPrograma(string nuevoCodigo , string codigo, string nombre, int numero_decreto)
        {

            Entidades.MDP.Programa entidadMDPPrograma = new Entidades.MDP.Programa();
            entidadMDPPrograma.Codigo = codigo;
            entidadMDPPrograma.Nombre = nombre;
            entidadMDPPrograma.Numero_Decreto = numero_decreto;
            
            Datos.MDP.Programa datosMDPPrograma = new Datos.MDP.Programa();

            return datosMDPPrograma.EditarPrograma(entidadMDPPrograma, nuevoCodigo);
        }
    }
}
