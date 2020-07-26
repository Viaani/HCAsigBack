using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MI
{
    public class Indicadores
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();
        public Entidades.MI.Indicadores MostrarIndicadores()
        {
            Conexion conexion = new Conexion();
            
            conexion.AbrirConexion();
            conexion.BeginTransaction();
            try
            {
                string query = " SELECT( " +
                      "SELECT COUNT(*) " +
                      "FROM   programas " +
                      ") AS cantidadProgramas, " +
                      "(SELECT COUNT(*) " +
                      "FROM   programaexterno " +
                      ") AS cantidadProgramasExternos, " +
                      "(select count(*) " +
                      "from decretos " +
                      ") as cantidadDecretos, " +
                      "(select count(*) " +
                      "from homologacion " +
                      ") as cantidadHomologaciones, " +
                      "(select count(*) " +
                      "from convalidacion " +
                      ") as cantidadConvalidaciones " +
                "FROM dual;";

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                reader.Read();

                Entidades.MI.Indicadores indicadores = new Entidades.MI.Indicadores();

                indicadores.cantidadProgramas = Convert.ToInt32(reader["cantidadProgramas"]);
                indicadores.cantidadDecretos = Convert.ToInt32(reader["cantidadDecretos"]);
                indicadores.cantidadConvalidaciones = Convert.ToInt32(reader["cantidadConvalidaciones"]);
                indicadores.cantidadHomologaciones = Convert.ToInt32(reader["cantidadHomologaciones"]);
                indicadores.cantidadProgramasExternos = Convert.ToInt32(reader["cantidadProgramasExternos"]);

                conexion.CommitTransaction();
                conexion.CerrarConexion();

                Conexion conexion2 = new Conexion();
                conexion2.AbrirConexion();
                conexion2.BeginTransaction();
                
                string query2 = "SELECT u.nombre, u.apellido, " +
                        "(SELECT COUNT(*) " +
                        "FROM convalidacion as c " +
                        "WHERE c.emailSecretario = u.email " +
                        ") AS cantidadConvalidaciones, " +
                        "(SELECT COUNT(*) " +
                        "FROM homologacion as h " +
                        "WHERE h.emailSecretario = u.email " +
                        ") AS cantidadHomologaciones " +
                        "FROM usuarios as u " +
                        "ORDER BY u.email; ";

                MySqlCommand commandDatabase2 = new MySqlCommand(query2, conexion2.databaseConnection);
                commandDatabase2.CommandTimeout = 60;

                MySqlDataReader reader2;
                reader2 = commandDatabase2.ExecuteReader();

                indicadores.cantidadEquivalenciasUsuario = new List<Entidades.MI.CantidadEquivalenciasUsuario>();
                while (reader2.Read())
                {
                    Entidades.MI.CantidadEquivalenciasUsuario equivalenciasUsuario = new Entidades.MI.CantidadEquivalenciasUsuario();
                    equivalenciasUsuario.secretarioAcademico = $"{reader2["nombre"].ToString()} {reader2["apellido"].ToString()}";
                    equivalenciasUsuario.cantidadConvalidaciones = Convert.ToInt32(reader2["cantidadConvalidaciones"]);
                    equivalenciasUsuario.cantidadHomologaciones = Convert.ToInt32(reader2["cantidadHomologaciones"]);

                    indicadores.cantidadEquivalenciasUsuario.Add(equivalenciasUsuario);
                }
                conexion2.CommitTransaction();
                conexion2.CerrarConexion();

                return indicadores;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }
        }

    }
}
