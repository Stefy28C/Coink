-- Stored Procedure para insertar un usuario con validaciones
CREATE OR REPLACE PROCEDURE insertar_usuario(
    _nombre VARCHAR(100),
    _telefono VARCHAR(15),
    _direccion VARCHAR(255),
    _id_pais INT,
    _id_departamento INT,
    _id_municipio INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- 1. Validación de campos obligatorios (nombre, teléfono, dirección)
    IF _nombre IS NULL OR _telefono IS NULL OR _direccion IS NULL THEN
        RAISE EXCEPTION 'Error: Los campos nombre, teléfono y dirección no pueden ser nulos.';
    END IF;
    
    -- 2. Validación de la longitud del teléfono (exactamente 10 dígitos)
    IF LENGTH(_telefono) <> 10 THEN
        RAISE EXCEPTION 'Error: El teléfono debe tener exactamente 10 dígitos.';
    END IF;
    
    -- 3. Verificar si el país existe en la tabla pais
    IF NOT EXISTS (SELECT 1 FROM pais WHERE id = _id_pais) THEN
        RAISE EXCEPTION 'Error: El país con ID % no existe.', _id_pais;
    END IF;
    
    -- 4. Verificar si el departamento existe y pertenece al país seleccionado
    IF NOT EXISTS (SELECT 1 FROM departamento WHERE id = _id_departamento AND id_pais = _id_pais) THEN
        RAISE EXCEPTION 'Error: El departamento con ID % no pertenece al país con ID %.', _id_departamento, _id_pais;
    END IF;
    
    -- 5. Verificar si el municipio existe y pertenece al departamento seleccionado
    IF NOT EXISTS (SELECT 1 FROM municipio WHERE id = _id_municipio AND id_departamento = _id_departamento) THEN
        RAISE EXCEPTION 'Error: El municipio con ID % no pertenece al departamento con ID %.', _id_municipio, _id_departamento;
    END IF;
    
    -- 6. Verificar si ya existe un usuario con el mismo teléfono para evitar duplicados
    IF EXISTS (SELECT 1 FROM usuarios WHERE telefono = _telefono) THEN
        RAISE EXCEPTION 'Error: Ya existe un usuario registrado con el teléfono %.', _telefono;
    END IF;
    
    -- 7. Insertar el usuario si todas las validaciones son correctas
    INSERT INTO usuarios (nombre, telefono, direccion, id_pais, id_departamento, id_municipio)
    VALUES (_nombre, _telefono, _direccion, _id_pais, _id_departamento, _id_municipio);
    
    -- Mensaje de éxito
    RAISE NOTICE 'Usuario % insertado correctamente con el teléfono %.', _nombre, _telefono;
END;
$$ LANGUAGE plpgsql;

-- Stored Procedure para insertar un país
CREATE OR REPLACE PROCEDURE insertar_pais(_nombre VARCHAR(100))
LANGUAGE plpgsql
AS $$
BEGIN
    -- Validar que el nombre no esté vacío
    IF _nombre IS NULL OR TRIM(_nombre) = '' THEN
        RAISE EXCEPTION 'El nombre del país no puede ser nulo o vacío';
    END IF;

    -- Insertar el país
    INSERT INTO pais (nombre) VALUES (_nombre);
END;
$$;

-- Stored Procedure para insertar un departamento
CREATE OR REPLACE PROCEDURE insertar_departamento(_nombre VARCHAR(100), _id_pais INT)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Validar que el nombre y el país no sean nulos
    IF _nombre IS NULL OR TRIM(_nombre) = '' THEN
        RAISE EXCEPTION 'El nombre del departamento no puede ser nulo o vacío';
    END IF;

    IF _id_pais IS NULL THEN
        RAISE EXCEPTION 'El ID del país no puede ser nulo';
    END IF;

    -- Insertar el departamento
    INSERT INTO departamento (nombre, id_pais) VALUES (_nombre, _id_pais);
END;
$$;

-- Stored Procedure para insertar un municipio
CREATE OR REPLACE PROCEDURE insertar_municipio(_nombre VARCHAR(100), _id_departamento INT)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Validar que el nombre y el departamento no sean nulos
    IF _nombre IS NULL OR TRIM(_nombre) = '' THEN
        RAISE EXCEPTION 'El nombre del municipio no puede ser nulo o vacío';
    END IF;

    IF _id_departamento IS NULL THEN
        RAISE EXCEPTION 'El ID del departamento no puede ser nulo';
    END IF;

    -- Insertar el municipio
    INSERT INTO municipio (nombre, id_departamento) VALUES (_nombre, _id_departamento);
END;
$$;
