USE AlvecoComercial10
GO

DROP PROCEDURE InsertarProvincia
GO

CREATE PROCEDURE InsertarProvincia
@CodigoPais				CHAR(2),
@CodigoDepartamento		CHAR(2),
@CodigoProvincia		CHAR(2),
@NombreProvincia		VARCHAR(250)
AS
BEGIN
	INSERT INTO	dbo.Provincias(CodigoPais, CodigoDepartamento, CodigoProvincia, NombreProvincia)
	VALUES(@CodigoPais, @CodigoDepartamento, @CodigoProvincia, @NombreProvincia)
END
GO

DROP PROCEDURE ActualizarProvincia
GO

CREATE PROCEDURE ActualizarProvincia
@CodigoPais				CHAR(2),
@CodigoDepartamento		CHAR(2),
@CodigoProvincia		CHAR(2),
@NombreProvincia		VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.Provincias
	SET
		CodigoPais			= @CodigoPais,
		CodigoDepartamento	= @CodigoDepartamento,
		CodigoProvincia		= @CodigoProvincia,
		NombreProvincia		= @NombreProvincia
	WHERE CodigoPais = @CodigoPais
	AND CodigoDepartamento = @CodigoDepartamento
	AND CodigoProvincia	= @CodigoProvincia
END
GO

DROP PROCEDURE EliminarProvincia
GO

CREATE PROCEDURE EliminarProvincia
@CodigoPais				CHAR(2),
@CodigoDepartamento		CHAR(2),
@CodigoProvincia		CHAR(2)
AS
BEGIN
	DELETE 
	FROM dbo.Provincias
	WHERE CodigoPais = @CodigoPais
	AND CodigoDepartamento = @CodigoDepartamento
	AND CodigoProvincia	= @CodigoProvincia
END
GO

DROP PROCEDURE ListarProvincias
GO

CREATE PROCEDURE ListarProvincias
AS
BEGIN
	SELECT CodigoPais, CodigoDepartamento, CodigoProvincia, NombreProvincia 
	FROM dbo.Provincias
	ORDER BY CodigoPais, CodigoDepartamento, NombreProvincia
END
GO

DROP PROCEDURE ObtenerProvincia
GO

CREATE PROCEDURE ObtenerProvincia
@CodigoPais				CHAR(2),
@CodigoDepartamento		CHAR(2),
@CodigoProvincia		CHAR(2)
AS
BEGIN
	SELECT CodigoPais, CodigoDepartamento, CodigoProvincia, NombreProvincia 
	FROM dbo.Provincias
	WHERE CodigoPais = @CodigoPais
	AND CodigoDepartamento = @CodigoDepartamento
	AND CodigoProvincia	= @CodigoProvincia
END
GO

DROP PROCEDURE ObtenerProvinciasPorDepartamento
GO

CREATE PROCEDURE ObtenerProvinciasPorDepartamento
@CodigoPais				CHAR(2),
@CodigoDepartamento		CHAR(2)
AS

SELECT CodigoPais, CodigoDepartamento, CodigoProvincia, NombreProvincia
FROM Provincias
WHERE   CodigoPais = @CodigoPais AND
		CodigoDepartamento = @CodigoDepartamento
GO
