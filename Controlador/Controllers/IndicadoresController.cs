using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Controlador.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    // Activacion CORS
    [EnableCors("AllowCors")]

    public class IndicadoresController : Controller
    {
        Logica.MI.Indicadores indicadoresLogica = new Logica.MI.Indicadores();
        [HttpGet]
        public ActionResult<string> GetIndicadores()
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = indicadoresLogica.MostrarIndicadores();
            if (respuesta != null)
            {
                return Ok(respuesta);
            }
            else
            {
                return BadRequest("Error al obtener Indicadores");
            }
            }
            else
            {
                return BadRequest(validarToken[0]);
            }
        }
    }
}