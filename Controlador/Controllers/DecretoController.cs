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
    [Route("api/[controller]")]
    [ApiController]

    // Activacion CORS
    [EnableCors("AllowCors")]

    public class DecretoController : Controller
    {
        
        Logica.MDP.Decreto logicaDecreto = new Logica.MDP.Decreto();

        [HttpGet]
        public ActionResult<IEnumerable<Entidades.MDP.RetornoDecreto>> ListaDecreto()
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaDecreto.MostrarDecreto(null);
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener decretos");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] Entidades.MDP.Decreto value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaDecreto.AgregarDecreto(value.numero, value.fecha);
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
        public ActionResult<string> Put(int id, [FromBody] Entidades.MDP.Decreto value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaDecreto.EditarDecreto (id, value.numero, value.fecha);
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
        public ActionResult<String> Delete(int id)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaDecreto.EliminarDecreto(id);
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