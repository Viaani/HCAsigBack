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
    [Produces("application/json")]
    public class EquivalenteController : Controller
    {
        Logica.MC.Equivalente logicaEquivalente = new Logica.MC.Equivalente();
        [HttpPost("Convalidacion")]
        public ActionResult<string> PostConvalidacion([FromBody] Entidades.MC.Equivalentes value)
        {
            if (ModelState.IsValid)
            {
                Entidades.MC.Equivalentes EntidadesMCEquivalentes = new Entidades.MC.Equivalentes();
                EntidadesMCEquivalentes.ListaEquivalente = new List<Entidades.MC.Equivalente>();

                foreach (Entidades.MC.Equivalente EntidadesMCEquivalente in value.ListaEquivalente)
                {
                    Entidades.MC.Equivalente EntidadesMCEquivalenteAgregada = new Entidades.MC.Equivalente();

                    EntidadesMCEquivalenteAgregada.programaOrigen = EntidadesMCEquivalente.programaOrigen;
                    EntidadesMCEquivalenteAgregada.programaObjetivo = EntidadesMCEquivalente.programaObjetivo;
                    EntidadesMCEquivalenteAgregada.codigoAsignaturaOrigen = EntidadesMCEquivalente.codigoAsignaturaOrigen;
                    EntidadesMCEquivalenteAgregada.codigoAsignaturaObjetivo = EntidadesMCEquivalente.codigoAsignaturaObjetivo;

                    EntidadesMCEquivalentes.ListaEquivalente.Add(EntidadesMCEquivalenteAgregada);
                }

                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaEquivalente.AgregarEquivalenteConvalidacion(EntidadesMCEquivalentes);
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

        [HttpPost("Homologacion")]
        public ActionResult<string> PostHomologacion([FromBody] Entidades.MC.Equivalentes value)
        {
            if (ModelState.IsValid)
            {
                Entidades.MC.Equivalentes EntidadesMCEquivalentes = new Entidades.MC.Equivalentes();
                EntidadesMCEquivalentes.ListaEquivalente = new List<Entidades.MC.Equivalente>();

                foreach (Entidades.MC.Equivalente EntidadesMCEquivalente in value.ListaEquivalente)
                {
                    Entidades.MC.Equivalente EntidadesMCEquivalenteAgregada = new Entidades.MC.Equivalente();

                    EntidadesMCEquivalenteAgregada.programaOrigen = EntidadesMCEquivalente.programaOrigen;
                    EntidadesMCEquivalenteAgregada.programaObjetivo = EntidadesMCEquivalente.programaObjetivo;
                    EntidadesMCEquivalenteAgregada.codigoAsignaturaOrigen = EntidadesMCEquivalente.codigoAsignaturaOrigen;
                    EntidadesMCEquivalenteAgregada.codigoAsignaturaObjetivo = EntidadesMCEquivalente.codigoAsignaturaObjetivo;

                    EntidadesMCEquivalentes.ListaEquivalente.Add(EntidadesMCEquivalenteAgregada);
                }

                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaEquivalente.AgregarEquivalenteHomologacion(EntidadesMCEquivalentes);
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

        [HttpGet("Convalidacion/{programaOrigen},{programaObjetivo}")]
        public ActionResult<IEnumerable<Entidades.MC.AsignaturasEquivalentes>> GetConvalidacion(string programaOrigen, string programaObjetivo)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaEquivalente.MostrarEquivalenteConvalidacion(programaOrigen, programaObjetivo);
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener Convalidaciones");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpGet("Homologacion/{programaOrigen},{programaObjetivo}")]
        public ActionResult<IEnumerable<Entidades.MC.AsignaturasEquivalentes>> GetHomologacion(string programaOrigen, string programaObjetivo)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaEquivalente.MostrarEquivalenteHomologacion(programaOrigen, programaObjetivo);
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener Homologaciones");
                }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }

        [HttpPost("Convalidacion/{programaOrigen},{programaObjetivo}")]
        public ActionResult<string> DeleteConvalidacion(string programaOrigen, string programaObjetivo, [FromBody] Entidades.MC.AsignaturasEquivalentes value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaEquivalente.EliminarEquivalenteConvalidacion(value, programaOrigen, programaObjetivo);
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

        [HttpPost("Homologacion/{programaOrigen},{programaObjetivo}")]
        public ActionResult<string> DeleteHomologacion(string programaOrigen, string programaObjetivo, [FromBody] Entidades.MC.AsignaturasEquivalentes value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaEquivalente.EliminarEquivalenteHomologacion(value, programaOrigen, programaObjetivo);
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

    }
}