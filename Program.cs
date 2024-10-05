using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar la conexión a PostgreSQL
builder.Services.AddControllers();  // Agregamos soporte para controladores
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Servicio UsuarioService 
builder.Services.AddScoped<UsuarioService>();
// Servicio PaisService 
builder.Services.AddScoped<PaisService>();
// Servicio MunicipioService
builder.Services.AddScoped<MunicipioService>();
// Servicio DepartamentoService 
builder.Services.AddScoped<DepartamentoService>();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();  // Habilitamos el mapeo de controladores

// Weather forecast endpoint (puedes mantenerlo si lo deseas)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
