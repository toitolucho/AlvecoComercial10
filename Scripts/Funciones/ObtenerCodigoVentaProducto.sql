USE AlvecoComercial10
GO

DROP FUNCTION ObtenerCodigoVentaProducto
GO

CREATE FUNCTION ObtenerCodigoVentaProducto()
RETURNS CHAR(12)

AS
BEGIN
	DECLARE @CodigoVentaProducto	CHAR(12)
	DECLARE	@FechaHoraActual		DATETIME,
			@CantidadTotalVentas	INT
	
	
	SET @FechaHoraActual = GETDATE()

	SELECT @CantidadTotalVentas = ISNULL(COUNT(*),0)
	FROM VentasProductos CP
	WHERE DATEPART(MONTH,CP.FechaHoraVenta ) = DATEPART(MONTH, GETDATE())
	
	SET @CantidadTotalVentas = @CantidadTotalVentas + 1
	
	SET @CodigoVentaProducto = RIGHT('000'+ RTRIM(CAST(@CantidadTotalVentas AS CHAR(4))),4 )
	+ '-' + RIGHT('0'+ RTRIM(CAST(DATEPART(MONTH,@FechaHoraActual) AS CHAR(2))),2 )
	+ '-' + CAST(DATEPART(YEAR, @FechaHoraActual) AS CHAR(4))
	
	RETURN @CodigoVentaProducto
END
GO

--SELECT dbo.ObtenerCodigoVentaProducto()

