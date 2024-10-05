using Microsoft.AspNetCore.Mvc;
using ApiUsuarios.Models;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly DepartamentoService _departamentoService;

        public DepartamentoController(DepartamentoService departamentoService)
        {
            _departamentoService = departamentoService;
        }

        // Crear un nuevo departamento
        [HttpPost("crear")]
        public IActionResult CrearDepartamento([FromBody] Departamento departamento)
        {
            try
            {
                _departamentoService.CrearDepartamento(departamento);
                return Ok("Departamento creado exitosamente.");
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

        // Eliminar un departamento por ID
        [HttpDelete("eliminar/{id}")]
        public IActionResult EliminarDepartamento(int id)
        {
            try
            {
                _departamentoService.EliminarDepartamento(id);
                return Ok($"Departamento con ID {id} eliminado exitosamente.");
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el departamento: {ex.Message}");
            }
        }

        // Listar todos los departamentos
        [HttpGet("listar")]
        public IActionResult ListarDepartamentos()
        {
            try
            {
                var departamentos = _departamentoService.ListarDepartamentos();
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
