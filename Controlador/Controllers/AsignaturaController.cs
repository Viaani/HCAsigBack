//Cors
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
//Archivo
using System.IO;
using System.Threading.Tasks;

namespace Controlador.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    // Activacion CORS
    [EnableCors("AllowCors")]

    public class AsignaturaController : Controller
    {

        Logica.MDP.Asignatura logicaAsignatura = new Logica.MDP.Asignatura();

        [HttpPost("{codigo},{nombre},{creditos},{numeroDecreto}/")]
        public ActionResult<string> Post(string codigo, string nombre, int creditos, int numeroDecreto, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    try
                    {
                        string uploads = @"Programas Academicos/";
                        var filePath = Path.Combine(uploads, archivo.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            archivo.CopyTo(stream);
                        }
                        var respuesta = logicaAsignatura.AgregarAsignatura(codigo, nombre, creditos, numeroDecreto , filePath);
                        if (respuesta[1] == "1")
                        {
                            return Ok(respuesta[0]);
                        }
                        else
                        {
                            return BadRequest(respuesta[0]);
                        }
                    }
                    catch(Exception e)
                    {
                        return BadRequest(e.ToString());
                    }
                }
                else
                {
                    return BadRequest(validarToken[0]);
                }
            }
            else return BadRequest(ModelState);

        }
        
        [HttpPost("ProgramaExterno_asignaturas/{codigo},{nombre},{creditos},{codigo_ProgramaExterno}/")]
        public ActionResult<string> PostProgramaExterno(string codigo, string nombre, int creditos, string codigo_ProgramaExterno, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    try
                    {
                        string uploads = @"Programas Academicos/";
                        var filePath = Path.Combine(uploads, archivo.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            archivo.CopyTo(stream);
                        }
                        var respuesta = logicaAsignatura.AgregarAsignaturaExterna(codigo, nombre, creditos, codigo_ProgramaExterno, filePath);
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
            else return BadRequest(ModelState);

        }

        [HttpPut("{id}")]
        public ActionResult<string> Put(string id, [FromBody] Entidades.MDP.Asignatura value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaAsignatura.EditarAsignatura(id, value.Codigo, value.Nombre, value.Creditos);
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

        [HttpPut("archivo/{codigo},{nombre},{creditos},{id}")]
        public ActionResult<string> PutArchivo(string codigo, string nombre, int creditos , string id, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    try
                    {
                        string uploads = @"Programas Academicos/";
                        var filePath = Path.Combine(uploads, archivo.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            archivo.CopyTo(stream);
                        }
                        var respuesta = logicaAsignatura.EditarAsignaturaArchivo(id, codigo, nombre, creditos, filePath);
                        if (respuesta[1] == "1")
                        {
                            return Ok(respuesta[0]);
                        }
                        else
                        {
                            return BadRequest(respuesta[0]);
                        }
                    }
                    catch(Exception e)
                    {
                        return BadRequest(e.ToString());
                    }
                }
                else
                {
                    return BadRequest(validarToken[0]);
                }
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("ProgramaExterno_asignaturas/{id}")]
        public ActionResult<string> PutProgramaExterno_asignaturas(string id, [FromBody] Entidades.MDP.AsignaturaExterna value)
        {
            if (ModelState.IsValid)
            {
                Request.Headers.TryGetValue("Authorization", out var header);
                var token = header.ToString();
                Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
                var validarToken = usuarioLogica.validarToken(token);
                if (validarToken[1] == "1")
                {
                    var respuesta = logicaAsignatura.EditarAsignatura(id, value.Codigo, value.Nombre, value.Creditos);
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
        public ActionResult<String> Delete(String id)
        {
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                var respuesta = logicaAsignatura.EliminarAsignatura(id);
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

        [HttpGet("DescargarProgramaAcademico/{id}")]
        public async Task<IActionResult> PostArchivo(string id)
        {
            //string uploads = "C://Users//diego//Desktop//Proyecto Tesis//Prototipo 1//HCAsignaturas 0.1.2//HCAsignaturas 0.1.2//Datos//Programas Academicos//";
            
            Request.Headers.TryGetValue("Authorization", out var header);
            var token = header.ToString();
            Logica.MU.Usuario usuarioLogica = new Logica.MU.Usuario();
            var validarToken = usuarioLogica.validarToken(token);
            if (validarToken[1] == "1")
            {
                try
                {

                    string[] path = logicaAsignatura.ObtenerProgramaAcademico(id);
                    if(path[1] == "1")
                    {
                        var memory = new MemoryStream();
                        using (var stream = new FileStream(path[0], FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;
                        return File(memory, "application/pdf");
                    }
                    else
                    {
                        return BadRequest(path[0]);
                    }
                }
                catch(Exception e)
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