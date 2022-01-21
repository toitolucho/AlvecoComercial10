USE AlvecoComercial10
GO

DROP PROCEDURE InsertarPais
GO

CREATE PROCEDURE InsertarPais
	@CodigoPais		CHAR(2),
	@NombrePais		VARCHAR(250),
	@Nacionalidad	VARCHAR(250)
AS
BEGIN
	INSERT INTO dbo.Paises (CodigoPais, NombrePais, Nacionalidad)								
	VALUES (@CodigoPais, @NombrePais, @Nacionalidad)
END	
GO

DROP PROCEDURE ActualizarPais
GO

CREATE PROCEDURE ActualizarPais
	@CodigoPais		CHAR(2),
	@NombrePais		VARCHAR(250),
	@Nacionalidad	VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.Paises
	SET					
		NombrePais		= @NombrePais,
		Nacionalidad	= @Nacionalidad
	WHERE (CodigoPais = @CodigoPais)
END
GO

DROP PROCEDURE EliminarPais
GO

CREATE PROCEDURE EliminarPais
	@CodigoPais	CHAR(2)
AS
BEGIN
	DELETE 
	FROM dbo.Paises
	WHERE (CodigoPais = @CodigoPais)
END
GO

DROP PROCEDURE ListarPaises
GO

CREATE PROCEDURE ListarPaises
AS
BEGIN
	SELECT CodigoPais, NombrePais, Nacionalidad
	FROM dbo.Paises
	ORDER BY NombrePais
END
GO

DROP PROCEDURE ObtenerPais
GO

CREATE PROCEDURE ObtenerPais
	@CodigoPais	CHAR(2)
AS
BEGIN
	SELECT CodigoPais, NombrePais, Nacionalidad 
	FROM dbo.Paises
	WHERE (CodigoPais = @CodigoPais)
END
GO
