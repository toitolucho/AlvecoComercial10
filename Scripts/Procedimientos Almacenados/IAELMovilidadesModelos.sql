USE AlvecoComercial10
GO



DROP PROCEDURE InsertarMovilidadModelo
GO
CREATE PROCEDURE InsertarMovilidadModelo	
	@NombreModelo	VARCHAR(250)
AS
BEGIN
	INSERT INTO dbo.MovilidadesModelos (NombreModelo)								
	VALUES (@NombreModelo)
END
GO



DROP PROCEDURE ActualizarMovilidadModelo
GO
CREATE PROCEDURE ActualizarMovilidadModelo
	@CodigoModelo	TINYINT,
	@NombreModelo	VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.MovilidadesModelos
	SET				
		NombreModelo = @NombreModelo
	WHERE (CodigoModelo = @CodigoModelo)
END
GO



DROP PROCEDURE EliminarMovilidadModelo
GO
CREATE PROCEDURE EliminarMovilidadModelo
	@CodigoModelo	INT
AS
BEGIN
	DELETE 
	FROM dbo.MovilidadesModelos
	WHERE (CodigoModelo = @CodigoModelo)
END
GO



DROP PROCEDURE ListarMovilidadesModelos
GO
CREATE PROCEDURE ListarMovilidadesModelos
AS
BEGIN
	SELECT CodigoModelo, NombreModelo
	FROM dbo.MovilidadesModelos
	ORDER BY CodigoModelo
END
GO



DROP PROCEDURE ObtenerMovilidadModelo
GO
CREATE PROCEDURE ObtenerMovilidadModelo
	@CodigoModelo	INT
AS
BEGIN
	SELECT CodigoModelo, NombreModelo
	FROM dbo.MovilidadesModelos
	WHERE (CodigoModelo = @CodigoModelo)
END
GO



--DROP PROCEDURE ObtenerMovilidadModelos
--GO
--CREATE PROCEDURE ObtenerMovilidadModelos
--AS
--BEGIN
--	SELECT CodigoModelo, NombreModelo
--	FROM dbo.MovilidadesModelos
--END
--GO

