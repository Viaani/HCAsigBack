using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class Conexion
    {

        public MySqlConnection databaseConnection;
        //Abre conexion a la base de datos
        public void AbrirConexion()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=aaaaaaa;database=hcasignaturasMDP;";
            this.databaseConnection = new MySqlConnection(connectionString);
            this.databaseConnection.Open();
        }

        //Cierra conexion a la base de datos
        public void CerrarConexion()
        {
            this.databaseConnection.Close();
        }

        public void BeginTransaction()
        {

        }
        public void CommitTransaction()
        {

        }

        public void RollBack()
        {

        }
    }
}
