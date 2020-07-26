using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MDP
{
    public class Decreto
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarDecreto(Entidades.MDP.Decreto decreto)
        {


            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "INSERT INTO decretos(`numero`, `fecha`) VALUES ( " + decreto.numero + ", '" + decreto.fecha + "');";
               
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }

            catch (Exception e)
            {
                if (e.ToString().Contains("Duplicate"))
                {
                    return new string[] { "duplicado", this.TipoRetorno.error.ToString() };
                }
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public List<Entidades.MDP.Decreto> MostrarDecreto(String id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM decretos WHERE numero = " + id + " ;";

                // si id es "null" extrae todos los decretos
                if (id == null)
                {
                    query = "SELECT * FROM decretos;";
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MDP.Decreto> decretos = new List<Entidades.MDP.Decreto>();


                while (reader.Read())
                {
                    Entidades.MDP.Decreto decreto = new Entidades.MDP.Decreto();

                    decreto.numero = Convert.ToInt32(reader["numero"]);
                    string[] fecha = reader["fecha"].ToString().Split(' ')[0].Split('-');
                    decreto.fecha = fecha[2] + '-' + fecha[1] + '-' + fecha[0];
                    
                    decretos.Add(decreto);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return decretos;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarDecreto(int id)
        {

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM decretos WHERE numero = " + id + " ;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                commandDatabase.ExecuteReader();

                try
                {
                    query = "DELETE FROM programas WHERE numero_decreto = "+id+";";
                    commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                    commandDatabase.CommandTimeout = 60;

                    commandDatabase.ExecuteReader();
                }
                catch(Exception e)
                {

                }


                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public string[] EditarDecreto(Entidades.MDP.Decreto decreto, int id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "UPDATE decretos SET numero= " + decreto.numero+ ", fecha= '" + decreto.fecha + "' WHERE numero = " + id + "; ";
               
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("Duplicate"))
                {
                    return new string[] { "duplicado", this.TipoRetorno.error.ToString() };
                }
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }
        }
    }
}
