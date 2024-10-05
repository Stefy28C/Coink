using Microsoft.AspNetCore.Mvc;
using ApiUsuarios.Models;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly PaisService _paisService;

        public PaisController(PaisService paisService)
        {
            _paisService = paisService;
        }

        // Crear un nuevo país
        [HttpPost("crear")]
        public IActionResult CrearPais([FromBody] Pais pais)
        {
            try
            {
                _paisService.CrearPais(pais);
                return Ok("País creado exitosamente.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // Eliminar un país por ID
        [HttpDelete("eliminar/{id}")]
        public IActionResult EliminarPais(int id)
        {
            try
            {
                _paisService.EliminarPais(id);
                return Ok($"País con ID {id} eliminado exitosamente.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el país: {ex.Message}");
            }
        }

        // Listar todos los países
        [HttpGet("listar")]
        public IActionResult ListarPaises()
        {
            try
            {
                var paises = _paisService.ListarPaises();
                return Ok(paises);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
