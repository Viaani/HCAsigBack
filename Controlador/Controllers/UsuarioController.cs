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

    public class UsuarioController : Controller
    {
        [HttpPost("Autentificar/")]
        public ActionResult<string> accessUsuario([FromBody] Entidades.MU.Credenciales value)
        {
            Logica.MU.Usuario logicaMUUsuario = new Logica.MU.Usuario();
            var resultado = logicaMUUsuario.AutenticarUsuario(value);
            if( resultado == null )
            {
                return BadRequest("Error");
            }
            if(resultado.email == "")
            {
                return BadRequest("NoExiste");
            }
            return Ok(resultado);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Entidades.MU.Usuario>> Get()
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();

            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {   
                Logica.MU.Usuario logicaMUUsuario = new Logica.MU.Usuario();
                var mostrarUsuarios = logicaMUUsuario.MostrarUsuario(null);

                if(mostrarUsuarios != null){
                    return Ok(mostrarUsuarios);
                }
                else
                {
                    return BadRequest("Error al obtener usuarios");
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
                Logica.MU.Usuario logicaMUUsuario = new Logica.MU.Usuario();
                return Ok(Json(logicaMUUsuario.MostrarUsuario(id)));
            }
            else
            {
                return BadRequest(validarToken[0]);
            }

        }

        [HttpPost]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Post([FromBody] Entidades.MU.Usuario value)
        {

            if (ModelState.IsValid)
            {
                
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = usuarioLogica.AgregarUsuario(value.nombre, value.apellido, value.run , value.email, value.area, value.password , value.tipoUsuario);
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
        public ActionResult<string> Put(string id, [FromBody] Entidades.MU.Usuario value)
        {
            if (ModelState.IsValid)
            {

                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = usuarioLogica.EditarUsuario(id, value.nombre, value.apellido, value.run, value.email, value.area, value.password, value.tipoUsuario);
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
                var respuesta = usuarioLogica.EliminarUsuario(id);
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