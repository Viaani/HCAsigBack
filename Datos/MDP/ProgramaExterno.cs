using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MDP
{
    public class ProgramaExterno
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarProgramaExterno(Entidades.MDP.ProgramaExterno programaExterno)
        {


            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "INSERT INTO programaExterno(`codigo`, `nombre`,`universidad`) VALUES ( '" + programaExterno.Codigo + "', '" + programaExterno.Nombre + "', '" + programaExterno.Universidad + "' );";

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

        public List<Entidades.MDP.ProgramaExterno> MostrarProgramaExterno(String codigo)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM programaExterno WHERE codigo = '" + codigo + "' ;";

                // si id es "null" extrae todos los programas
                if (codigo == null)
                {
                    query = "SELECT * FROM programaExterno;";
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MDP.ProgramaExterno> programasExternos = new List<Entidades.MDP.ProgramaExterno>();


                while (reader.Read())
                {
                    Entidades.MDP.ProgramaExterno programaExterno = new Entidades.MDP.ProgramaExterno();

                    programaExterno.Codigo = reader["codigo"].ToString();
                    programaExterno.Nombre = reader["nombre"].ToString();
                    programaExterno.Universidad = reader["universidad"].ToString();

                    programasExternos.Add(programaExterno);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return programasExternos;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarProgramaExterno(String codigo)
        {

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM programaExterno WHERE codigo = '" + codigo + "' ;";
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
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public string[] EditarProgramaExterno(Entidades.MDP.ProgramaExterno programaExterno, String codigo)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "UPDATE programaExterno SET codigo= '" + programaExterno.Codigo + "', nombre= '" + programaExterno.Nombre + "', universidad='" + programaExterno.Universidad + "' WHERE codigo = '" + codigo + "'; ";

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
