# API para Gestión de Usuarios, Países, Departamentos y Municipios

Este proyecto es una API desarrollada en C# con ASP.NET Core que permite gestionar usuarios, así como sus relaciones con países, departamentos y municipios. La API se conecta a una base de datos PostgreSQL y utiliza **Stored Procedures** para las operaciones de base de datos.

## Requisitos

- .NET 6 o superior
- PostgreSQL
- Visual Studio Code o Visual Studio
- Postman (opcional para probar la API)


## Configuración

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/Stefy28C/Coink.git

2. Configurar la cadena de conexión a la base de datos PostgreSQL en appsettings.json:
```bash
    {
     "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=coink_usuarios;Username=user;Password=psw"
                         }
    }
```

3. Ejecutar los scripts SQL para crear las tablas y los Stored Procedures:

- Los scripts se encuentran en la carpeta ScriptsSQL/:

    - crear_tablas.sql: Crea las tablas necesarias (usuario, país, departamento, municipio).
    - crear_stored_procedures.sql: Crea los Stored Procedures necesarios.

Ejecutar los scripts en tu instancia de PostgreSQL.

# Uso de la API
## Endpoints
### Usuarios
- Registrar un usuario:

    - POST /api/usuario/registrar
    - Ejemplo de cuerpo:

```bash
     {
  "Nombre": "Juan Pérez",
  "Telefono": "1234567890",
  "Direccion": "Calle Falsa 123",
  "IdPais": 1,
  "IdDepartamento": 1,
  "IdMunicipio": 1
}
```
- **Listar usuarios**:

    - `GET /api/usuario/listar`

### Países
- **Crear un país**:

    - `POST /api/pais/crear`
    - Ejemplo de cuerpo:
    
    ```bash
    {
      "Nombre": "Chile"
    }
    ```

- **Listar países**:

    - `GET /api/pais/listar`

- **Eliminar un país**:

    - `DELETE /api/pais/eliminar/{id}`

### Departamentos
- **Crear un departamento**:

    - `POST /api/departamento/crear`
    - Ejemplo de cuerpo:

    ```bash
    {
      "Nombre": "Santiago",
      "IdPais": 1
    }
    ```

- **Listar departamentos**:

    - `GET /api/departamento/listar`

- **Eliminar un departamento**:

    - `DELETE /api/departamento/eliminar/{id}`

### Municipios
- **Crear un municipio**:

    - `POST /api/municipio/crear`
    - Ejemplo de cuerpo:

    ```bash
    {
      "Nombre": "Providencia",
      "IdDepartamento": 1
    }
    ```

- **Listar municipios**:
```bash
    - GET /api/municipio/listar
```
- **Eliminar un municipio**:

    - `DELETE /api/municipio/eliminar/{id}`

## Patrones de Diseño Implementados

Se ha implementado el **Patrón de Servicio** para separar la lógica de negocios de los controladores, facilitando la mantenibilidad y escalabilidad del proyecto.

## Pruebas

Se recomienda usar **Postman** para probar los diferentes endpoints de la API. Se proporcionan ejemplos de solicitudes en la sección anterior.

## Contribuir

Si encuentras algún problema o deseas contribuir al proyecto, siéntete libre de abrir un **issue** o enviar un **pull request**.

## Licencia

Este proyecto está bajo la licencia MIT.
