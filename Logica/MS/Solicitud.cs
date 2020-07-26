using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Logica.MS
{
    public class Solicitud
    {
        public string[] EnviarArchivo(MailMessage mail)
        {
            try
            {
                //Smpt detalles de cliente
                SmtpClient detallesDeCliente = new SmtpClient();
                detallesDeCliente.Port = Convert.ToInt32(587);
                detallesDeCliente.Host = "smtp.gmail.com";
                detallesDeCliente.EnableSsl = true;
                detallesDeCliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                detallesDeCliente.UseDefaultCredentials = false;
                detallesDeCliente.Credentials = new NetworkCredential("hcasignaturas.unab@gmail.com", "hcasignaturas123"); // Cuenta q envia
                // Activar uso de aplicaciones externas en cuentra que envia el mail
                detallesDeCliente.Send(mail);
                mail.Dispose();
                return new string[] { "Exito", "1" };
            }
            catch(Exception e)
            {
                return new string[] { e.ToString(), "-1" };
            }
        }
    }
}
