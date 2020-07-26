using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controlador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // Activacion CORS
    [EnableCors("AllowCors")]
    [Produces("application/json")]
    public class DestinatarioController : Controller
    {
        Logica.MU.Usuario LogicaUSuarios = new Logica.MU.Usuario();
        Logica.MS.Destinatario LogicaDestinatario = new Logica.MS.Destinatario();

        [HttpGet]
        public ActionResult<IEnumerable<Entidades.MS.Destinatario>> Get()
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            var validarToken = LogicaUSuarios.validarToken(token);
            if (validarToken[1] == "1")
            {
                var mostrarDestinatarios = LogicaDestinatario.MostrarDestinatarios(null);
                if (mostrarDestinatarios != null)
                {
                    return Ok(mostrarDestinatarios);
                }
                else
                {
                    return BadRequest("Error al obtener destinatarios");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpGet("{tipoUsuario}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<IEnumerable<Entidades.MS.Destinatario>> GetTipoUsuario(string tipoUsuario)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            var validarToken = LogicaUSuarios.validarToken(token);
            if (validarToken[1] == "1")
            {
                var mostrarDestinatarios = LogicaDestinatario.MostrarDestinatarios(tipoUsuario);
                if (mostrarDestinatarios != null)
                {
                    return Ok(mostrarDestinatarios);
                }
                else
                {
                    return BadRequest("Error al obtener destinatarios");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }

        }

        [HttpPost]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Post([FromBody] Entidades.MS.Destinatario value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                var validarToken = LogicaUSuarios.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = LogicaDestinatario.AgregarDestinatario(value);
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest(respuesta[0]);
                    }
                }
                else
                {
                    return BadRequest(validarToken[0]);
                }
            }
            else return BadRequest(ModelState);

        }

        [HttpPut("{email}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Put(string email, [FromBody] Entidades.MS.Destinatario value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                var validarToken = LogicaUSuarios.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = LogicaDestinatario.EditarDestinatario(value, email);
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest(respuesta[0]);
                    }
                }
                else
                {
                    return BadRequest(validarToken[0]);
                }
            }
            else return BadRequest(value);
        }

        [HttpDelete("{email}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<String> Delete(String email)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            var validarToken = LogicaUSuarios.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = LogicaDestinatario.EliminarDestinatario(email);
                if (respuesta[1] == "1")
                {
                    return Ok(respuesta[0]);
                }
                else
                {
                    return BadRequest(respuesta[0]);
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
    }
}