using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
//Cors
using Microsoft.AspNetCore.Cors;

namespace Controlador.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    // Activacion CORS
    [EnableCors("AllowCors")]

    public class ProgramaController : Controller
    {
        Logica.MDP.Programa programaLogica = new Logica.MDP.Programa();
        [HttpGet]
        public ActionResult<IEnumerable<Entidades.MDP.Programa>> Get()
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = programaLogica.MostrarPrograma(null);
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener Programa");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
        
        [HttpGet("{id}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Get(string id)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = programaLogica.MostrarPrograma(id);
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener Programa");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpPost]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Post([FromBody] Entidades.MDP.Programa value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = programaLogica.AgregarPrograma(value.Codigo, value.Nombre, value.Numero_Decreto);
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

        [HttpPut("{id}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Put(string id, [FromBody] Entidades.MDP.Programa value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = programaLogica.EditarPrograma(id, value.Codigo, value.Nombre, value.Numero_Decreto);
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

        [HttpDelete("{id}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<String> Delete(String id)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = programaLogica.EliminarPrograma(id);
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