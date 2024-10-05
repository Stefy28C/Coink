namespace ApiUsuarios.Models
{
    public class Usuario
    {
        public int Id { get; set; }  // ID de usuario
        public string Nombre { get; set; }  // Nombre del usuario
        public string Telefono { get; set; }  // Teléfono del usuario
        public string Direccion { get; set; }  // Dirección del usuario
        public int IdPais { get; set; }  // ID del país
        public int IdDepartamento { get; set; }  // ID del departamento
        public int IdMunicipio { get; set; }  // ID del municipio
    }
}
