USE AlvecoComercial10
GO

DROP PROCEDURE ListarVentasProductosReportesPorCreditosFechasCliente
GO

CREATE PROCEDURE ListarVentasProductosReportesPorCreditosFechasCliente
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@OrdenacionPorPersona	BIT
AS
BEGIN
	SELECT  CASE WHEN VP.NumeroVentaProducto IS NULL THEN 'SIN FACTURA' ELSE 'CON FACTURA' END AS TipoFacturacion, 
			 CL.NombreCliente AS NombreCompletoPersona ,
			VP.FechaHoraVenta,  VPDE.CantidadEntregada, DBO.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto,
			VPDE.PrecioUnitarioVenta, (VPDE.CantidadEntregada * VPDE.PrecioUnitarioVenta) AS PrecioTotal, 
			ISNULL(C.Monto, 0) as MontoTotalPagado
	FROM VentasProductos VP
	INNER JOIN VListarVentasProductosDetalleEntrega VPDE
	ON VP.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VP.NumeroVentaProducto = VPDE.NumeroVentaProducto
	INNER JOIN Clientes CL
	ON VP.CodigoCliente = CL.CodigoCliente
	LEFT JOIN 
	(
		SELECT NumeroCuentaPorCobrar, SUM(Monto) AS Monto
		FROM CuentasPorCobrarCobros
		GROUP BY NumeroCuentaPorCobrar
	) C
	ON VP.NumeroCuentaPorCobrar  = C.NumeroCuentaPorCobrar	
	WHERE VP.NumeroAlmacen = @NumeroAlmacen
	AND VP.NumeroCuentaPorCobrar IS NOT NULL
	AND VP.FechaHoraVenta
	BETWEEN DBO.FormatearFechaInicioFin(@FechaHoraInicio,1) AND DBO.FormatearFechaInicioFin(@FechaHoraFin,0)
	ORDER BY 1
END
GO



--EXEC ListarVentasProductosReportesPorCreditosFechasCliente 1, '01/01/2000', '31/12/2012', 1