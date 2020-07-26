using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MDP
{
    public class Decreto
    {

        public string[] AgregarDecreto (int numero, string fecha)
        {
            Entidades.MDP.Decreto entidadMDPDecreto = new Entidades.MDP.Decreto();
            entidadMDPDecreto.numero = numero;
            entidadMDPDecreto.fecha = fecha;
          
            Datos.MDP.Decreto datosMDPDecreto = new Datos.MDP.Decreto();

            return datosMDPDecreto.AgregarDecreto(entidadMDPDecreto);
        }

        public List<Entidades.MDP.RetornoDecreto> MostrarDecreto(String id)
        {
            Datos.MDP.Decreto datosMDPDecreto = new Datos.MDP.Decreto();
            Datos.MDP.Asignatura DatosAsignatura = new Datos.MDP.Asignatura();
            List<Entidades.MDP.RetornoDecreto> retornoDecretos = new List<Entidades.MDP.RetornoDecreto>();
            var decretos = datosMDPDecreto.MostrarDecreto(id);
            foreach (var decreto in decretos)
            {
                Entidades.MDP.RetornoDecreto retornoDecreto = new Entidades.MDP.RetornoDecreto();
                retornoDecreto.numero = decreto.numero;
                retornoDecreto.fecha = decreto.fecha;
                retornoDecreto.asignaturas = new List<Entidades.MDP.Asignatura>();
                retornoDecreto.asignaturas = DatosAsignatura.MostrarDecretos_asignaturas(decreto.numero);
                retornoDecretos.Add(retornoDecreto);
            }
            return retornoDecretos;
        }

        public string[] EliminarDecreto(int id)
        {
            Datos.MDP.Decreto datosMDPDecreto = new Datos.MDP.Decreto();
            return datosMDPDecreto.EliminarDecreto(id);
        }

        public string[] EditarDecreto(int nuevoNumero, int numero, String fecha)
        {

            Entidades.MDP.Decreto entidadMDPDecreto = new Entidades.MDP.Decreto();
            entidadMDPDecreto.numero = numero;
            entidadMDPDecreto.fecha = fecha;

            Datos.MDP.Decreto datosMDPDecreto = new Datos.MDP.Decreto();

            return datosMDPDecreto.EditarDecreto(entidadMDPDecreto, nuevoNumero);
        }

    }
}
