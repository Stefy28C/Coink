using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ApiUsuarios.Models;

namespace ApiUsuarios.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration _configuration;  // Agregar _configuration

        public UsuarioController(UsuarioService usuarioService, IConfiguration configuration)  // Inyectar IConfiguration
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                _usuarioService.RegistrarUsuario(usuario);
                return Ok("Usuario registrado exitosamente.");
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

        // Función para listar todos los usuarios
        [HttpGet("listar")]
        public IActionResult ListarUsuarios()
        {
            try
            {
                using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();

                using var cmd = new NpgsqlCommand("SELECT * FROM usuarios", conn);
                var reader = cmd.ExecuteReader();

                var usuarios = new List<Usuario>();
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Telefono = reader.GetString(2),
                        Direccion = reader.GetString(3),
                        IdPais = reader.GetInt32(4),
                        IdDepartamento = reader.GetInt32(5),
                        IdMunicipio = reader.GetInt32(6)
                    });
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al listar usuarios: {ex.Message}");
            }
        }
    }
}
