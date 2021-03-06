﻿using System;
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
    
    public class ConvalidacionController : Controller
    {
        Logica.MC.Convalidacion logicaConvalidacion = new Logica.MC.Convalidacion();

        [HttpPost]
        //[EnableCors("AllowOrigin")]
        public ActionResult<string> Post([FromBody] Entidades.MC.ConvalidacionHomologacion value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaConvalidacion.AgregarConvalidacion(value);
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

        [HttpGet]
        public ActionResult<IEnumerable<Entidades.MC.RetornoConvalidacionNota>> Get()
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaConvalidacion.MostrarConvalidaciones();
                if (respuesta != null)
                {
                    return Ok(respuesta);
                }
                else
                {
                    return BadRequest("Error al obtener convalidaciones");
                }
                }
            else
            {
                return BadRequest(validarToken[0]);
            }
}

        [HttpDelete("{id}")]
        public ActionResult<string> Delete(string id)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaConvalidacion.EliminarConvalidacion(id);
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