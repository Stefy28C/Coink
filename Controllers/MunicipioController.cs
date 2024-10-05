using Microsoft.AspNetCore.Mvc;
using ApiUsuarios.Models;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipioController : ControllerBase
    {
        private readonly MunicipioService _municipioService;

        public MunicipioController(MunicipioService municipioService)
        {
            _municipioService = municipioService;
        }

        // Crear un nuevo municipio
        [HttpPost("crear")]
        public IActionResult CrearMunicipio([FromBody] Municipio municipio)
        {
            try
            {
                _municipioService.CrearMunicipio(municipio);
                return Ok("Municipio creado exitosamente.");
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

        // Eliminar un municipio por ID
        [HttpDelete("eliminar/{id}")]
        public IActionResult EliminarMunicipio(int id)
        {
            try
            {
                _municipioService.EliminarMunicipio(id);
                return Ok($"Municipio con ID {id} eliminado exitosamente.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el municipio: {ex.Message}");
            }
        }

        // Listar todos los municipios
        [HttpGet("listar")]
        public IActionResult ListarMunicipios()
        {
            try
            {
                var municipios = _municipioService.ListarMunicipios();
                return Ok(municipios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
