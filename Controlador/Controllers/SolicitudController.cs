using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Controlador.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    // Activacion CORS
    [EnableCors("AllowCors")]

    public class SolicitudController : Controller
    {
        Logica.MS.Solicitud LogicaSolicitud = new Logica.MS.Solicitud();
        [HttpPost("RevicionConvalidacion/{mail}")]
        public ActionResult<string> enviarArchivoRevicionConvalidacion(string mail, IFormFile archivo)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                try
                {
                    //Detalles de Mensaje
                    MailMessage detalleMail = new MailMessage();
                    detalleMail.From = new MailAddress("hcasignaturas.unab@gmail.com"); // Email que envía
                    detalleMail.To.Add("diego.quilodran.t@gmail.com");
                    detalleMail.Subject = "Solicitud de revición Homologación";
                    detalleMail.Body = "Se adjunta el documento con la Homologación";

                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        Attachment att = new Attachment(new MemoryStream(fileBytes), archivo.FileName);
                        detalleMail.Attachments.Add(att);
                    }
                    var respuesta = LogicaSolicitud.EnviarArchivo( detalleMail );
                    detalleMail.Dispose();
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest(respuesta[0]);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
        [HttpPost("OficiarConvalidacion/{mail}")]
        public ActionResult<string> enviarArchivoOficiarConvalidacion(string mail, IFormFile archivo)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                try
                {
                    //Detalles de Mensaje
                    MailMessage detalleMail = new MailMessage();
                    detalleMail.From = new MailAddress("hcasignaturas.unab@gmail.com"); // Email que envía
                    detalleMail.To.Add("diego.quilodran.t@gmail.com");
                    detalleMail.Subject = "Solicitud de Oficiar Homologación";
                    detalleMail.Body = "Se adjunta el documento con la Homologación";

                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        Attachment att = new Attachment(new MemoryStream(fileBytes), archivo.FileName);
                        detalleMail.Attachments.Add(att);
                    }
                    var respuesta = LogicaSolicitud.EnviarArchivo(detalleMail);
                    detalleMail.Dispose();
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest(respuesta[0]);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
        [HttpPost("RevicionHomologacion/{mail}")]
        public ActionResult<string> enviarArchivoRevicionHomologacion(string mail, IFormFile archivo)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                try
                {
                    //Detalles de Mensaje
                    MailMessage detalleMail = new MailMessage();
                    detalleMail.From = new MailAddress("hcasignaturas.unab@gmail.com"); // Email que envía
                    detalleMail.To.Add("diego.quilodran.t@gmail.com");
                    detalleMail.Subject = "Solicitud de revición Convalidación";
                    detalleMail.Body = "Se adjunta el documento con la Convalidación";

                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        Attachment att = new Attachment(new MemoryStream(fileBytes), archivo.FileName);
                        detalleMail.Attachments.Add(att);
                    }
                    var respuesta = LogicaSolicitud.EnviarArchivo(detalleMail);
                    detalleMail.Dispose();
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest(respuesta[0]);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
        [HttpPost("OficiarHomologacion/{mail}")]
        public ActionResult<string> enviarArchivoOficiarHomologacion(string mail, IFormFile archivo)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                try
                {
                    //Detalles de Mensaje
                    MailMessage detalleMail = new MailMessage();
                    detalleMail.From = new MailAddress("hcasignaturas.unab@gmail.com"); // Email que envía
                    detalleMail.To.Add("diego.quilodran.t@gmail.com");
                    detalleMail.Subject = "Solicitud de Oficiar Convalidación";
                    detalleMail.Body = "Se adjunta el documento con la Convalidación";

                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        Attachment att = new Attachment(new MemoryStream(fileBytes), archivo.FileName);
                        detalleMail.Attachments.Add(att);
                    }
                    var respuesta = LogicaSolicitud.EnviarArchivo(detalleMail);
                    detalleMail.Dispose();
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest(respuesta[0]);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.ToString());
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
    }
}