using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MDP
{
    public class Programa
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarPrograma(Entidades.MDP.Programa programa){
            

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "INSERT INTO programas(`codigo`, `nombre`,`numero_decreto`) VALUES ( '" + programa.Codigo + "', '" + programa.Nombre + "', " + programa.Numero_Decreto + " );";
                
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                
                conexion.CommitTransaction();
                return new string[] {"Exito", this.TipoRetorno.exito.ToString() };
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

        //public List<Entidades.MDP.Programa> ListaProgramas()
        //{
        //    Conexion conexion = new Conexion();
        //    conexion.AbrirConexion();
        //    conexion.BeginTransaction();

        //    try
        //    {
        //        string query = "SELECT * FROM Programa;";
        //        MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
        //        commandDatabase.CommandTimeout = 60;

        //        MySqlDataReader reader;
        //        reader = commandDatabase.ExecuteReader();
        //        reader.Read();

        //        List<Entidades.MDP.Programa> programas = new List<Entidades.MDP.Programa>();

                
        //        while (reader.Read())
        //        {
        //            Entidades.MDP.Programa programa = new Entidades.MDP.Programa();

        //            programa.Codigo = reader["codigo"].ToString();
        //            programa.Nombre = reader["nombre"].ToString();

        //            programas.Add(programa);
        //        }

        //        conexion.CommitTransaction();
        //        conexion.CerrarConexion();
        //        return programas;
        //    }
        //    catch (Exception e)
        //    {
        //        conexion.RollBack();
        //        conexion.CerrarConexion();
        //        return null;
        //    }
            
        //}

        public List<Entidades.MDP.Programa> MostrarPrograma(String id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM programas WHERE codigo = '" + id + "' ;";

                // si id es "null" extrae todos los programas
                if (id == null)
                {
                    query = "SELECT * FROM programas;";
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MDP.Programa> programas = new List<Entidades.MDP.Programa>();


                while (reader.Read())
                {
                    Entidades.MDP.Programa programa = new Entidades.MDP.Programa();

                    programa.Codigo = reader["codigo"].ToString();
                    programa.Nombre = reader["nombre"].ToString();
                    programa.Numero_Decreto = Convert.ToInt32(reader["numero_decreto"]);

                    programas.Add(programa);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return programas;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarPrograma(String id)
        {

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM programas WHERE codigo = '" + id + "' ;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                commandDatabase.ExecuteReader();
                

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return new string[] { e.ToString() , this.TipoRetorno.error.ToString()};
            }
            
        }

        public string[] EditarPrograma(Entidades.MDP.Programa programa, String id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "UPDATE programas SET codigo= '" + programa.Codigo + "', nombre= '" + programa.Nombre + "', numero_decreto=" + programa.Numero_Decreto + " WHERE codigo = '" + id + "'; ";
                
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
