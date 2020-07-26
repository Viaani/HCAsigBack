using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MU
{
    public class Usuario
    {
        public string[] AgregarUsuario(string nombre, string apellido, string run, string email, string area, string password, string tipoUsuario)
        {
            Entidades.MU.Usuario entidadMUUsuario = new Entidades.MU.Usuario();
            entidadMUUsuario.nombre = nombre;
            entidadMUUsuario.apellido = apellido;
            entidadMUUsuario.run = run;
            entidadMUUsuario.email = email;
            entidadMUUsuario.area = area;
            entidadMUUsuario.password = Entidades.Encrypt.GetMD5(password);
            entidadMUUsuario.tipoUsuario = tipoUsuario;

            Datos.MU.Usuario datosMUUsuario = new Datos.MU.Usuario();

            return datosMUUsuario.AgregarUsuario(entidadMUUsuario);
        }

        public List<Entidades.MU.Usuario> MostrarUsuario(String id)
        {
            Datos.MU.Usuario datosMUUsuario = new Datos.MU.Usuario();
            return datosMUUsuario.MostrarUsuario(id);
        }

        public string[] EliminarUsuario(string id)
        {
            Datos.MU.Usuario datosMUUsuario = new Datos.MU.Usuario();
            return datosMUUsuario.EliminarUsuario(id);
        }

        public string[] EditarUsuario(string nuevoCodigo, string nombre, string apellido, string run, string email, string area, string password, string tipoUsuario)
        {

            Entidades.MU.Usuario entidadMUUsuario = new Entidades.MU.Usuario();
            entidadMUUsuario.nombre = nombre;
            entidadMUUsuario.apellido = apellido;
            entidadMUUsuario.run = run;
            entidadMUUsuario.email = email;
            entidadMUUsuario.area = area;
            if (password == "")
            {
                entidadMUUsuario.password = password;
            }
            else
            {
                entidadMUUsuario.password = Entidades.Encrypt.GetMD5(password);
            }
            
            entidadMUUsuario.tipoUsuario = tipoUsuario;

            Datos.MU.Usuario datosMUUsuario = new Datos.MU.Usuario();

            return datosMUUsuario.EditarUsuario(entidadMUUsuario, nuevoCodigo);
        }

        public Entidades.MU.Token AutenticarUsuario (Entidades.MU.Credenciales credenciales)
        {
            Datos.MU.Usuario datosMUUsuario = new Datos.MU.Usuario();
            Entidades.MU.Token token2 = new Entidades.MU.Token();
            token2.token = "";
            token2.nombre = "";
            token2.apellido = "";
            token2.run = "";
            token2.email = "";
            token2.area = "";
            token2.tipoUsuario = "";

            var usuario = datosMUUsuario.AutenticarUsuario(credenciales);
            if(usuario == null)
            {
                return null;
            }

            datosMUUsuario.LimpiarUsuariosConectados();

            if (usuario.email == "")
            {
                return token2;
            }
            var usuarioConectado = datosMUUsuario.RevisarUsuarioConectado(usuario.email);
            if (usuarioConectado[1] == "-1")
            {
                return token2;
            }
            else if (usuarioConectado[1] == "1" && usuarioConectado[0] == "Conectado")
            {
                var eliminarToken = datosMUUsuario.EliminarToken(usuario.email);
                if(eliminarToken[1] == "-1")
                {
                    return token2;
                }
            }
            var guardarToken = datosMUUsuario.GuardarToken(usuario.token, usuario.email);
            if(guardarToken[1] == "-1")
            {
                return token2;
            }
            return usuario;
        }

        public string[] validarToken(string token)
        {
            Datos.MU.Usuario datosUsuario = new Datos.MU.Usuario();
            datosUsuario.LimpiarUsuariosConectados();
            return datosUsuario.ValidarToken(token);
        }
    }
    
}
