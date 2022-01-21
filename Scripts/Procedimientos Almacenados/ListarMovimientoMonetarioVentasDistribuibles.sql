USE AlvecoComercial10
GO

DROP PROCEDURE ListarMovimientoMonetarioVentasDistribuibles
GO

CREATE PROCEDURE ListarMovimientoMonetarioVentasDistribuibles
	@NumeroAlmacen			INT,
	@DIPersonaDistribuidor	CHAR(15),	
	@FechaHoraInicio		DATETIME,
	@FechaHoraFin			DATETIME	
AS 
BEGIN
	
	SET @FechaHoraInicio = DBO.FormatearFechaInicioFin(@FechaHoraInicio,1)
	SET @FechaHoraFin = DBO.FormatearFechaInicioFin(@FechaHoraFin,0)


	SELECT	NumeroVentaProducto AS NumeroTransaccion, CodigoVentaProducto as CodigoTransaccion, 
			FechaHoraVenta as FechaHoraTransaccion, 
			CASE WHEN CodigoTipoVenta = 'R' THEN 'CREDITO' ELSE 'EFECTIVO' END AS TipoPago,
			MontoTotalVenta AS MontoTotalTransaccion, MontoTotalPagoEfectivo AS MontoPagoCobroEfectivo, 
			dbo.ObtenerNombreCompleto(DIPersonaDistribuidor) AS NombreUsuario, 'VENTAS' AS TipoTransaccion,
			'INGRESOS' AS TipoMovimiento
	FROM dbo.VentasProductos
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND DIPersonaDistribuidor LIKE CASE WHEN @DIPersonaDistribuidor IS NULL THEN '%%' ELSE @DIPersonaDistribuidor END
	AND VentaParaDistribuir = 1
	AND FechaHoraVenta
	BETWEEN @FechaHoraInicio AND @FechaHoraFin	
	
	ORDER BY (CASE WHEN @DIPersonaDistribuidor IS NULL THEN 3 ELSE 7 END), (CASE WHEN @DIPersonaDistribuidor IS NULL THEN 7 ELSE 3 END)
END
GO


--EXEC DBO.ListarMovimientoMonetarioVentasDistribuibles 1, null, '01/01/2000', '31/12/2012'

