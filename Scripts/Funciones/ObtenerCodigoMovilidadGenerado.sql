USE AlvecoComercial10
GO

DROP FUNCTION ObtenerCodigoMovilidadGenerado
GO

CREATE FUNCTION ObtenerCodigoMovilidadGenerado()
RETURNS CHAR(10)
WITH ENCRYPTION 
AS
BEGIN
	DECLARE @CodigoMovilidadGenerado	CHAR(10),
			@CantidadMovilidades		INTEGER
			
	SELECT @CantidadMovilidades = COUNT(*)
	FROM dbo.Movilidades
	
	SET @CantidadMovilidades = @CantidadMovilidades + 1
	
	SET @CodigoMovilidadGenerado = RIGHT('0000000000' + RTRIM(CAST(ISNULL(@CantidadMovilidades,0) AS CHAR(10))),10)
	
	--SET @CantidadMovilidades = ISNULL(@CantidadMovilidades,0)
	--SELECT CAST(@CantidadMovilidades AS CHAR(6))
	RETURN ISNULL(@CodigoMovilidadGenerado,'0000000001')
END
GO

