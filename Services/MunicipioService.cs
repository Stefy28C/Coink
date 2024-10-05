using Npgsql;
using ApiUsuarios.Models;

public class MunicipioService
{
    private readonly IConfiguration _configuration;

    public MunicipioService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CrearMunicipio(Municipio municipio)
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        // Validar que el municipio no esté duplicado
        using var cmdValidacion = new NpgsqlCommand("SELECT COUNT(1) FROM municipio WHERE LOWER(TRIM(nombre)) = LOWER(TRIM(@nombre)) AND id_departamento = @id_departamento", conn);
        cmdValidacion.Parameters.AddWithValue("nombre", municipio.Nombre);
        cmdValidacion.Parameters.AddWithValue("id_departamento", municipio.IdDepartamento);
        var existsMuni = (long)cmdValidacion.ExecuteScalar() > 0;

        if (existsMuni)
        {
            throw new ArgumentException($"El municipio '{municipio.Nombre}' ya existe para el departamento seleccionado.");
        }

        // Si no existe, crear el municipio
        using var cmd = new NpgsqlCommand("INSERT INTO municipio (nombre, id_departamento) VALUES (@nombre, @id_departamento)", conn);
        cmd.Parameters.AddWithValue("nombre", municipio.Nombre);
        cmd.Parameters.AddWithValue("id_departamento", municipio.IdDepartamento);
        cmd.ExecuteNonQuery();
    }

    public void EliminarMunicipio(int id)
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        // Verificar que el municipio exista antes de eliminarlo
        using var cmdValidacion = new NpgsqlCommand("SELECT COUNT(1) FROM municipio WHERE id = @id", conn);
        cmdValidacion.Parameters.AddWithValue("id", id);
        var existsMuni = (long)cmdValidacion.ExecuteScalar() > 0;

        if (!existsMuni)
        {
            throw new ArgumentException($"El municipio con ID {id} no existe.");
        }

        // Eliminar el municipio
        using var cmd = new NpgsqlCommand("DELETE FROM municipio WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public List<Municipio> ListarMunicipios()
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM municipio", conn);
        var reader = cmd.ExecuteReader();

        var municipios = new List<Municipio>();
        while (reader.Read())
        {
            municipios.Add(new Municipio
            {
                Id = reader.GetInt32(0),
                Nombre = reader.GetString(1),
                IdDepartamento = reader.GetInt32(2)
            });
        }

        return municipios;
    }
}
