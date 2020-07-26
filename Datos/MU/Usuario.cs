using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.MU
{
    public class Usuario
    {
        Entidades.TipoRetorno TipoRetorno = new Entidades.TipoRetorno();

        public string[] AgregarUsuario(Entidades.MU.Usuario usuario)
        {


            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "INSERT INTO usuarios(" +
                               "`nombre`, `apellido`,`run`,`email`,`area`,`password`,`tipoUsuario`) VALUES " +
                               "( '" + usuario.nombre + "', '" + usuario.apellido + "', '" + usuario.run + "', '" + usuario.email + "', '" + usuario.area + "', '" + usuario.password + "', '" + usuario.tipoUsuario + "' );";

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

        public List<Entidades.MU.Usuario> MostrarUsuario(String id)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "SELECT * FROM usuarios WHERE codigo = '" + id + "' ;";

                // si id es "null" extrae todos los programas
                if (id == null)
                {
                    query = "SELECT * FROM usuarios;";
                }

                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();
                //reader.Read();

                List<Entidades.MU.Usuario> usuarios = new List<Entidades.MU.Usuario>();

                while (reader.Read())
                {
                    Entidades.MU.Usuario usuario = new Entidades.MU.Usuario();

                    usuario.nombre = reader["nombre"].ToString();
                    usuario.apellido = reader["apellido"].ToString();
                    usuario.run = reader["run"].ToString();
                    usuario.email = reader["email"].ToString();
                    usuario.area = reader["area"].ToString();
                    usuario.password = reader["password"].ToString();
                    usuario.tipoUsuario = reader["tipoUsuario"].ToString();

                    usuarios.Add(usuario);
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return usuarios;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }

        }

        public string[] EliminarUsuario(String id)
        {

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM usuarios WHERE email = '" + id + "' ;";
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

        public string[] EditarUsuario(Entidades.MU.Usuario usuario, String email)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query;
                if (usuario.password == "")
                {
                    query = "UPDATE usuarios SET " +
                    " nombre= '" + usuario.nombre + "', " +
                    " apellido= '" + usuario.apellido + "', " +
                    " run= '" + usuario.run + "', " +
                    " email= '" + usuario.email + "', " +
                    " area= '" + usuario.area + "', " +
                    " tipoUsuario= '" + usuario.tipoUsuario + "' WHERE email = '" + email + "'; ";
                }
                else
                {
                    query = "UPDATE usuarios SET " +
                    " nombre= '" + usuario.nombre + "', " +
                    " apellido= '" + usuario.apellido + "', " +
                    " run= '" + usuario.run + "', " +
                    " email= '" + usuario.email + "', " +
                    " area= '" + usuario.area + "', " +
                    " password= '" + usuario.password + "', " +
                    " tipoUsuario= '" + usuario.tipoUsuario + "' WHERE email = '" + email + "'; ";
                }

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

        public Entidades.MU.Token AutenticarUsuario(Entidades.MU.Credenciales credenciales)
        {

            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "select * from usuarios;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                while (reader.Read())
                {
                    if(  reader["email"].ToString() == credenciales.email && reader["password"].ToString() == Entidades.Encrypt.GetMD5(credenciales.password))
                    {
                        Entidades.MU.Token token = new Entidades.MU.Token();
                        token.token = Guid.NewGuid().ToString();
                        token.nombre = reader["nombre"].ToString();
                        token.apellido = reader["apellido"].ToString();
                        token.run = reader["run"].ToString();
                        token.email = reader["email"].ToString();
                        token.area = reader["area"].ToString();
                        token.tipoUsuario = reader["tipoUsuario"].ToString();

                        conexion.CommitTransaction();
                        conexion.CerrarConexion();

                        return token;
                    }
                }


                Entidades.MU.Token token2 = new Entidades.MU.Token();
                token2.token = "";
                token2.nombre = "";
                token2.apellido = "";
                token2.run = "";
                token2.email = "";
                token2.area = "";
                token2.tipoUsuario = "";

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return token2;
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return null;
            }
        }

        public string[] GuardarToken(string token, string email)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();
            try
            {
                string query = "insert into usuariosTemp (`token`,`expira`,`email`) values ( '" + token + "', date_add( now(),interval 2 day) ,'" + email + "'); ";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                conexion.CommitTransaction();
                return new string[] { "Exito", this.TipoRetorno.exito.ToString() };

            }
            catch (Exception e)
            {
                return new string[] { "Error", this.TipoRetorno.error.ToString() }; ;
            }
        }

        public string[] RevisarUsuarioConectado(string email)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "select * from usuariosTemp;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["email"].ToString() == email)
                    {
                        

                        conexion.CommitTransaction();
                        conexion.CerrarConexion();

                        return new string[] { "Conectado", this.TipoRetorno.exito.ToString() };
                    }
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { "Desconectado", this.TipoRetorno.exito.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return new string[] { "Error", this.TipoRetorno.error.ToString() };
            }
        }

        public string[] LimpiarUsuariosConectados()
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "delete from usuariosTemp where expira < now();";
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

        public string[] ValidarToken(string token)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "select * from usuariosTemp;";
                MySqlCommand commandDatabase = new MySqlCommand(query, conexion.databaseConnection);
                commandDatabase.CommandTimeout = 60;

                MySqlDataReader reader;
                reader = commandDatabase.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["token"].ToString() == token)
                    {


                        conexion.CommitTransaction();
                        conexion.CerrarConexion();

                        return new string[] { "Exito", this.TipoRetorno.exito.ToString() };
                    }
                }

                conexion.CommitTransaction();
                conexion.CerrarConexion();
                return new string[] { "TokenNoValido", this.TipoRetorno.error.ToString() };
            }
            catch (Exception e)
            {
                conexion.RollBack();
                conexion.CerrarConexion();
                return new string[] { "Error", this.TipoRetorno.error.ToString() };
            }
        }

        public string[] EliminarToken(string email)
        {
            Conexion conexion = new Conexion();
            conexion.AbrirConexion();
            conexion.BeginTransaction();

            try
            {
                string query = "DELETE FROM usuariosTemp WHERE email = '" + email + "' ;";
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
    }
}
