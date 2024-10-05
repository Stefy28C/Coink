using Npgsql;
using ApiUsuarios.Models;

public class DepartamentoService
{
    private readonly IConfiguration _configuration;

    public DepartamentoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CrearDepartamento(Departamento departamento)
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        // Validar que el departamento no esté duplicado
        using var cmdValidacion = new NpgsqlCommand("SELECT COUNT(1) FROM departamento WHERE LOWER(TRIM(nombre)) = LOWER(TRIM(@nombre)) AND id_pais = @id_pais", conn);
        cmdValidacion.Parameters.AddWithValue("nombre", departamento.Nombre);
        cmdValidacion.Parameters.AddWithValue("id_pais", departamento.IdPais);
        var existsDepto = (long)cmdValidacion.ExecuteScalar() > 0;

        if (existsDepto)
        {
            throw new ArgumentException($"El departamento '{departamento.Nombre}' ya existe para el país seleccionado.");
        }

        // Si no existe, crear el departamento
        using var cmd = new NpgsqlCommand("INSERT INTO departamento (nombre, id_pais) VALUES (@nombre, @id_pais)", conn);
        cmd.Parameters.AddWithValue("nombre", departamento.Nombre);
        cmd.Parameters.AddWithValue("id_pais", departamento.IdPais);
        cmd.ExecuteNonQuery();
    }

    public void EliminarDepartamento(int id)
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        // Verificar que el departamento exista antes de eliminarlo
        using var cmdValidacion = new NpgsqlCommand("SELECT COUNT(1) FROM departamento WHERE id = @id", conn);
        cmdValidacion.Parameters.AddWithValue("id", id);
        var existsDepto = (long)cmdValidacion.ExecuteScalar() > 0;

        if (!existsDepto)
        {
            throw new ArgumentException($"El departamento con ID {id} no existe.");
        }

        // Eliminar el departamento si existe
        using var cmd = new NpgsqlCommand("DELETE FROM departamento WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public List<Departamento> ListarDepartamentos()
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM departamento", conn);
        var reader = cmd.ExecuteReader();

        var departamentos = new List<Departamento>();
        while (reader.Read())
        {
            departamentos.Add(new Departamento
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                IdPais = reader.GetInt32(2)
            });
        }

        return departamentos;
    }
}
