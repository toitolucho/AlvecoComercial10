USE AlvecoComercial10
GO

DROP PROCEDURE InsertarLugar
GO

CREATE PROCEDURE InsertarLugar
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3),
	@NombreLugar			VARCHAR(250)
AS
BEGIN
	INSERT INTO dbo.Lugares (CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, NombreLugar)								
	VALUES (@CodigoPais, @CodigoDepartamento, @CodigoProvincia, @CodigoLugar, @NombreLugar)
END	
GO

DROP PROCEDURE ActualizarLugar
GO

CREATE PROCEDURE ActualizarLugar
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3),
	@NombreLugar			VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.Lugares
	SET	
		CodigoLugar	 = @CodigoLugar,
		NombreLugar	 = @NombreLugar	
	WHERE	(CodigoPais			= @CodigoPais)		
		AND (CodigoDepartamento = @CodigoDepartamento)
		AND (CodigoProvincia	= @CodigoProvincia)
		AND (CodigoLugar		= @CodigoLugar)
END
GO

DROP PROCEDURE EliminarLugar
GO

CREATE PROCEDURE EliminarLugar
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3)
AS
BEGIN
	DELETE 
	FROM dbo.Lugares
	WHERE	(CodigoPais			= @CodigoPais)		
		AND (CodigoDepartamento = @CodigoDepartamento)
		AND (CodigoProvincia	= @CodigoProvincia)
		AND (CodigoLugar		= @CodigoLugar)
END
GO

DROP PROCEDURE ListarLugares
GO

CREATE PROCEDURE ListarLugares
AS
BEGIN
	SELECT CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, NombreLugar 
	FROM dbo.Lugares
	ORDER BY CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar
END
GO

DROP PROCEDURE ObtenerLugar
GO

CREATE PROCEDURE ObtenerLugar
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3)
AS
BEGIN
	SELECT CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, NombreLugar
	FROM dbo.Lugares
	WHERE (CodigoPais = @CodigoPais) AND (CodigoDepartamento = @CodigoDepartamento)AND (CodigoProvincia = @CodigoProvincia) AND (CodigoLugar = @CodigoLugar)
END
GO
	
DROP PROCEDURE ObtenerLugaresPorProvincia
GO

CREATE PROCEDURE ObtenerLugaresPorProvincia
@CodigoPais			  CHAR(2),
@CodigoDepartamento   CHAR(2),
@CodigoProvincia      CHAR(2)
AS
SELECT CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, NombreLugar
FROM Lugares
WHERE CodigoPais  = @CodigoPais AND 
      CodigoDepartamento = @CodigoDepartamento AND
	  CodigoProvincia = @CodigoProvincia
	 
GO
