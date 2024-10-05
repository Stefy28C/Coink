using Npgsql;
using ApiUsuarios.Models;

public class PaisService
{
    private readonly IConfiguration _configuration;

    public PaisService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CrearPais(Pais pais)
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        // Validar que el país no esté duplicado
        using var cmdValidacion = new NpgsqlCommand("SELECT COUNT(1) FROM pais WHERE LOWER(TRIM(nombre)) = LOWER(TRIM(@nombre))", conn);
        cmdValidacion.Parameters.AddWithValue("nombre", pais.Nombre);
        var existsPais = (long)cmdValidacion.ExecuteScalar() > 0;

        if (existsPais)
        {
            throw new ArgumentException($"El país '{pais.Nombre}' ya existe.");
        }

        // Si no existe, crear el país
        using var cmd = new NpgsqlCommand("INSERT INTO pais (nombre) VALUES (@nombre)", conn);
        cmd.Parameters.AddWithValue("nombre", pais.Nombre);
        cmd.ExecuteNonQuery();
    }

    public void EliminarPais(int id)
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        // Verificar que el país exista antes de eliminarlo
        using var cmdValidacion = new NpgsqlCommand("SELECT COUNT(1) FROM pais WHERE id = @id", conn);
        cmdValidacion.Parameters.AddWithValue("id", id);
        var existsPais = (long)cmdValidacion.ExecuteScalar() > 0;

        if (!existsPais)
        {
            throw new ArgumentException($"El país con ID {id} no existe.");
        }

        // Eliminar el país si existe
        using var cmd = new NpgsqlCommand("DELETE FROM pais WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public List<Pais> ListarPaises()
    {
        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        conn.Open();

        using var cmd = new NpgsqlCommand("SELECT * FROM pais", conn);
        var reader = cmd.ExecuteReader();

        var paises = new List<Pais>();
        while (reader.Read())
        {
            paises.Add(new Pais { Id = reader.GetInt32(0), Nombre = reader.GetString(1) });
        }

        return paises;
    }
}
