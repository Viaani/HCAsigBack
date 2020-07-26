using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Controlador.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    // Activacion CORS
    [EnableCors("AllowCors")]
    public class ProgramaExternoController : Controller
    {
        
        // GET: Programa
        //[HttpGet]
        ////[EnableCors("AllowOrigin")]
        //public JsonResult ListaPrograma()
        //{
        //    Logica.MDP.Programa logicaMDPPrograma = new Logica.MDP.Programa();
        //    return Json(logicaMDPPrograma.MostrarPrograma(null));
        //}

        [HttpGet]
        public ActionResult<IEnumerable<Entidades.MDP.RetornoProgramaExterno>> Get()
        {
            Logica.MDP.ProgramaExterno logicaMDPProgramaExterno = new Logica.MDP.ProgramaExterno();

            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaMDPProgramaExterno.MostrarProgramaExterno(null);
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener Programas Externos");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpGet("{id}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Get(string codigo)
        {
            Logica.MDP.ProgramaExterno logicaMDPProgramaExterno = new Logica.MDP.ProgramaExterno();
            
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaMDPProgramaExterno.MostrarProgramaExterno(codigo);
                if(respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener Programa Externo");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpPost]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Post([FromBody] Entidades.MDP.ProgramaExterno value)
        {
            if (ModelState.IsValid)
            {

                Logica.MDP.ProgramaExterno logicaMDPProgramaExterno = new Logica.MDP.ProgramaExterno();

                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaMDPProgramaExterno.AgregarProgramaExterno(value.Codigo, value.Nombre, value.Universidad);
                    if (respuesta[1] == "1")
                    {
                        return Ok(respuesta[0]);
                    }
                    else
                    {
                        return BadRequest("Error al agregar programa externo");
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
        public ActionResult<string> Put(string id, [FromBody] Entidades.MDP.ProgramaExterno value)
        {
            if (ModelState.IsValid)
            {

                Logica.MDP.ProgramaExterno logicaMDPProgramaExterno = new Logica.MDP.ProgramaExterno();

                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaMDPProgramaExterno.EditarProgramaExterno(id, value.Codigo, value.Nombre, value.Universidad);
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

        [HttpDelete("{id}")]
        //[EnableCors("AllowOrigin")]
        public ActionResult<String> Delete(String id)
        {
            Logica.MDP.ProgramaExterno logicaMDPProgramaExterno = new Logica.MDP.ProgramaExterno();

            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaMDPProgramaExterno.EliminarProgramaExterno(id);
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