USE AlvecoComercial10
GO

DROP FUNCTION ObtenerCodigoProductoGenerado
GO

CREATE FUNCTION ObtenerCodigoProductoGenerado()
RETURNS CHAR(15)
 
AS
BEGIN
	DECLARE @CodigoMovilidadGenerado	CHAR(15),
			@CantidadProductos		INTEGER
			
	SELECT top(1) @CantidadProductos = CAST(CodigoProducto AS INT)
	FROM dbo.Productos
	order by CodigoProducto DESC
	
	SET @CantidadProductos = @CantidadProductos + 1
	
	SET @CodigoMovilidadGenerado = RIGHT('0000000' + RTRIM(CAST(ISNULL(@CantidadProductos,0) AS CHAR(7))),7)
	
	--SET @CantidadProductos = ISNULL(@CantidadProductos,0)
	--SELECT CAST(@CantidadProductos AS CHAR(6))
	RETURN ISNULL(@CodigoMovilidadGenerado,'0000001')
END
GO
