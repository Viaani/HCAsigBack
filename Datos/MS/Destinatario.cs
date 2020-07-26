using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MS
{
    public class Destinatario
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();
        public string[] AgregarDestinatario(Entidades.MS.Destinatario destinatario)
        {
            
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "INSERT INTO destinatarios(" +
                               "`nombre`, `apellido`,`email`,`tipoUsuario`,`area`) VALUES " +
                               "( '" + destinatario.Nombre + "', '" + destinatario.Apellido + "', '" + destinatario.Email + "', '" + destinatario.TipoUsuario + "', '" + destinatario.Area + "');";

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

        public List<Entidades.MS.Destinatario> MostrarDestinatarios(String TipoUsuario)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM destinatarios WHERE tipoUsuario = '" + TipoUsuario + "' ;";

                // si id es "null" extrae todos los programas
                if (TipoUsuario == null)
                {
                    query = "SELECT * FROM destinatarios;";
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MS.Destinatario> destinatarios = new List<Entidades.MS.Destinatario>();

                while (reader.Read())
                {
                    Entidades.MS.Destinatario destinatario = new Entidades.MS.Destinatario();

                    destinatario.Nombre = reader["nombre"].ToString();
                    destinatario.Apellido = reader["apellido"].ToString();
                    destinatario.Email = reader["email"].ToString();
                    destinatario.TipoUsuario = reader["tipoUsuario"].ToString();
                    destinatario.Area = reader["area"].ToString();

                    destinatarios.Add(destinatario);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return destinatarios;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarDestinatario(String email)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM destinatarios WHERE email = '" + email + "' ;";
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

        public string[] EditarDestinatario(Entidades.MS.Destinatario destinatario, String email)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "UPDATE destinatarios SET " +
                    " nombre= '" + destinatario.Nombre + "', " +
                    " apellido= '" + destinatario.Apellido + "', " +
                    " email= '" + destinatario.Email + "', " +
                    " tipoUsuario= '" + destinatario.TipoUsuario + "', " +
                    " area= '" + destinatario.Area + "' WHERE email = '" + email + "'; ";

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
