using System;
using System.Collections.Generic;
using System.Text;

namespace Logica.MS
{
    public class Destinatario
    {
        Datos.MS.Destinatario DatosDestinatario = new Datos.MS.Destinatario();
        public string[] AgregarDestinatario(Entidades.MS.Destinatario destinatario)
        {
            return DatosDestinatario.AgregarDestinatario(destinatario);
        }
        public List<Entidades.MS.Destinatario> MostrarDestinatarios(string tipoUsuario)
        {
            return DatosDestinatario.MostrarDestinatarios(tipoUsuario);
        }
        public string[] EditarDestinatario(Entidades.MS.Destinatario destinatario, string email)
        {
            return DatosDestinatario.EditarDestinatario(destinatario, email);
        }
        public string[] EliminarDestinatario(string email)
        {
            return DatosDestinatario.EliminarDestinatario(email);
        }
    }
}
