using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Datos.MDP
{
    public class Asignatura
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarAsignatura(Entidades.MDP.Asignatura asignatura, string path)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "BEGIN;" +
                    "INSERT INTO asignaturas(`codigo`, `nombre`,`creditos`) VALUES ( '" + asignatura.Codigo + "', '" + asignatura.Nombre + "', " + asignatura.Creditos + ");" +
                    "INSERT INTO decretos_asignaturas(`numero_decreto`, `codigo_asignatura`) VALUES ("+asignatura.NumeroDecreto+", '" + asignatura.Codigo + "');" +
                    "INSERT INTO programasAcademicos(`codigo`, `path`) VALUES ( '" + asignatura.Codigo + "','" + path + "');" +
                    "COMMIT;";

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
                    try
                    {
                        string query = "BEGIN;" +
                        "INSERT INTO decretos_asignaturas(`numero_decreto`, `codigo_asignatura`) VALUES (" + asignatura.NumeroDecreto + ", '" + asignatura.Codigo + "');" +
                        "COMMIT;";

                        MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                        commandDatabase.CommandTimeout = 60;

                        MySqlDataReader reader;
                        reader = commandDatabase.ExecuteReader();

                        conexion.CommitTransaction();
                        return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
                    }
                    catch(Exception e2)
                    {
                        return new string[] { e2.ToString(), this.TipoRetorno.error.ToString() };
                    }
                    
                }
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }

        public string[] AgregarAsignaturaExterna(Entidades.MDP.AsignaturaExterna asignatura, string path)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "BEGIN;" +
                    "INSERT INTO asignaturas(`codigo`, `nombre`,`creditos`) VALUES ( '" + asignatura.Codigo + "', '" + asignatura.Nombre + "', " + asignatura.Creditos + ");" +
                    "INSERT INTO programaExterno_asignaturas(`codigo_programaExterno`, `codigo_asignatura`) VALUES ('" + asignatura.Codigo_ProgramaExterno + "', '" + asignatura.Codigo + "');" +
                    "INSERT INTO programasAcademicos(`path`, `codigo`) VALUES ( '" + path + "', '" + asignatura.Codigo + "');" +
                    "COMMIT;";

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
                    try
                    {
                        string query = "BEGIN;" +
                        "INSERT INTO programaExterno_asignaturas(`codigo_programaExterno`, `codigo_asignatura`) VALUES ('" + asignatura.Codigo_ProgramaExterno + "', '" + asignatura.Codigo + "');" +
                        "COMMIT;";

                        MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                        commandDatabase.CommandTimeout = 60;

                        MySqlDataReader reader;
                        reader = commandDatabase.ExecuteReader();

                        conexion.CommitTransaction();
                        return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
                    }
                    catch (Exception e2)
                    {
                        return new string[] { e2.ToString(), this.TipoRetorno.error.ToString() };
                    }

                }
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }

        }
        
        public List<Entidades.MDP.Asignatura> MostrarAsignatura(String id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM asignaturas WHERE codigo = '" + id + "' ;";

                // si id es "null" extrae todos los programas
                if (id == null)
                {
                    query = "SELECT * FROM asignaturas;";
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MDP.Asignatura> asignaturas = new List<Entidades.MDP.Asignatura>();


                while (reader.Read())
                {
                    Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                    asignatura.Codigo = reader["codigo"].ToString();
                    asignatura.Nombre = reader["nombre"].ToString();
                    asignatura.Creditos = Convert.ToInt32(reader["creditos"]);

                    asignaturas.Add(asignatura);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return asignaturas;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public List<Entidades.MDP.Asignatura> MostrarDecretos_asignaturas(int id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM asignaturas WHERE codigo IN (SELECT codigo_asignatura FROM decretos_asignaturas WHERE numero_decreto ='" + id + "');";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MDP.Asignatura > asignaturas = new List<Entidades.MDP.Asignatura>();

                while (reader.Read())
                {
                    Entidades.MDP.Asignatura asignatura = new Entidades.MDP.Asignatura();

                    asignatura.Codigo = reader["codigo"].ToString();
                    asignatura.Nombre = reader["nombre"].ToString();
                    asignatura.Creditos = Convert.ToInt32(reader["creditos"]);
                    asignatura.NumeroDecreto = id;

                    asignaturas.Add(asignatura);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return asignaturas;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public List<Entidades.MDP.AsignaturaExterna> MostrarProgramaExterno_asignaturas(string id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM asignaturas WHERE codigo IN (SELECT codigo_asignatura FROM programaExterno_asignaturas WHERE codigo_programaExterno ='" + id + "');";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MDP.AsignaturaExterna> asignaturas = new List<Entidades.MDP.AsignaturaExterna>();


                while (reader.Read())
                {
                    Entidades.MDP.AsignaturaExterna asignatura = new Entidades.MDP.AsignaturaExterna();

                    asignatura.Codigo = reader["codigo"].ToString();
                    asignatura.Nombre = reader["nombre"].ToString();
                    asignatura.Creditos = Convert.ToInt32(reader["creditos"]);
                    asignatura.Codigo_ProgramaExterno = id;

                    asignaturas.Add(asignatura);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return asignaturas;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarAsignatura(String id)
        {

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM asignaturas WHERE codigo = '" + id + "' ;";
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

        public string[] EditarAsignatura(Entidades.MDP.Asignatura asignatura, String id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "UPDATE asignaturas SET codigo= '" + asignatura.Codigo + "', nombre= '" + asignatura.Nombre + "', creditos=" + asignatura.Creditos + " WHERE codigo = '" + id + "'; ";

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

        public string[] EditarPathArchivo(string id, string path)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "UPDATE programasAcademicos SET path= '" + path + "' WHERE codigo = '" + id + "';";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }
        }

        public string[] ObtenerProgramaAcademico(string codigo)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "Select * from programasAcademicos where codigo = '"+codigo+"';";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                reader.Read();
                string path = reader["path"].ToString();

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { path, this.TipoRetorno.exito.ToString() };
            }

            catch (Exception e)
            {
                conexion.RollBack();
                return new string[] { e.ToString(), this.TipoRetorno.error.ToString() };
            }
        }
    }
}
