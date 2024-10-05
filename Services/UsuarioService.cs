using Npgsql;
using System.Text.RegularExpressions;
using ApiUsuarios.Models;

public class UsuarioService
{
    private readonly IConfiguration _configuration;

    public UsuarioService(IConfiguration configuration)  // Aquí pasamos IConfiguration
    {
        _configuration = configuration;
    }

    public void RegistrarUsuario(Usuario usuario)
    {
        // Validaciones básicas
        if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Telefono) || string.IsNullOrEmpty(usuario.Direccion))
        {
            throw new ArgumentException("Nombre, teléfono y dirección son obligatorios.");
        }

        // Validación de longitud del nombre (máximo 100 caracteres)
        if (usuario.Nombre.Length > 100)
        {
            throw new ArgumentException("El nombre no debe exceder los 100 caracteres.");
        }

        // Validar que el teléfono tenga solo números y exactamente 10 dígitos
        if (!Regex.IsMatch(usuario.Telefono, @"^\d{10}$"))
        {
            throw new ArgumentException("El teléfono debe contener exactamente 10 dígitos y solo números.");
        }

        // Validación de unicidad del teléfono
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        using var cmdTelefono = new NpgsqlCommand("SELECT COUNT(1) FROM usuarios WHERE telefono = @telefono", conn);
        cmdTelefono.Parameters.AddWithValue("telefono", usuario.Telefono);
        var existsTelefono = (long)cmdTelefono.ExecuteScalar() > 0;

        if (existsTelefono)
        {
            throw new ArgumentException("El teléfono ya está registrado.");
        }

        // Validación de longitud de la dirección (máximo 255 caracteres)
        if (usuario.Direccion.Length > 255)
        {
            throw new ArgumentException("La dirección no debe exceder los 255 caracteres.");
        }

        // Conectar con PostgreSQL y llamar al Stored Procedure
        using var cmd = new NpgsqlCommand("CALL insertar_usuario(@nombre, @telefono, @direccion, @id_pais, @id_departamento, @id_municipio)", conn);
        cmd.Parameters.AddWithValue("nombre", usuario.Nombre);
        cmd.Parameters.AddWithValue("telefono", usuario.Telefono);
        cmd.Parameters.AddWithValue("direccion", usuario.Direccion);
        cmd.Parameters.AddWithValue("id_pais", usuario.IdPais);
        cmd.Parameters.AddWithValue("id_departamento", usuario.IdDepartamento);
        cmd.Parameters.AddWithValue("id_municipio", usuario.IdMunicipio);

        cmd.ExecuteNonQuery();
    }
}
