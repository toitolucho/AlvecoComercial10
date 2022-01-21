USE AlvecoComercial10
GO

DROP PROCEDURE ListarMovimientoDevoluciones
GO

CREATE PROCEDURE ListarMovimientoDevoluciones
	@NumeroAlmacen		INT,
	@DIUsuario			CHAR(15),	
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME	
AS 
BEGIN
	
	SET @FechaHoraInicio = DBO.FormatearFechaInicioFin(@FechaHoraInicio,1)
	SET @FechaHoraFin = DBO.FormatearFechaInicioFin(@FechaHoraFin,0)
	
	SELECT *
	FROM
	(	
		SELECT  VPD.NumeroVentaProductoDevolucion AS NumeroTransaccion, 
				VPD.CodigoDevolucionVentaProducto AS CodigoTransaccion,
				VPD.FechaHoraRegistro AS FechaHoraTransaccion, 
				VPD.MontoTotalVentaDevolucion AS MontoTotalTransaccion, 
				VPD.MontoTotalPagoEfectivo AS MontoPagoCobroEfectivo,
				dbo.ObtenerNombreCompleto(DIUsuario) as NombreUsuario, 'DEVOLUCION POR VENTA' AS TipoTransaccion, 
				DBO.ObtenerNombreProducto(VPDD.CodigoProducto) AS NombreProducto,
				VPDD.CodigoProducto, VPDD.CantidadVentaDevolucion AS CantidadDevolucion, 
				VPDD.PrecioUnitarioDevolucion AS PrecioDevolucion
		FROM dbo.VentasProductosDevoluciones VPD
		INNER JOIN dbo.VentasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPD.DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND VPD.FechaHoraRegistro
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL		
		
		SELECT  CPD.NumeroCompraProductoDevolucion, CPD.CodigoDevolucionCompraProducto,
				CPD.FechaHoraRegistro, 
				CPD.MontoTotalCompraDevolucion, CPD.MontoTotalPagoEfectivo,
				dbo.ObtenerNombreCompleto(CPD.DIUsuario), 'DEVOLUCION POR COMPRA',
				DBO.ObtenerNombreProducto(CPDD.CodigoProducto) AS NombreProducto,
				CPDD.CodigoProducto, CPDD.CantidadCompraDevolucion, CPDD.PrecioUnitarioDevolucion			
		FROM dbo.ComprasProductosDevoluciones CPD
		INNER JOIN dbo.ComprasProductosDevolucionesDetalle CPDD
		ON CPD.NumeroAlmacen = CPDD.NumeroAlmacenDevolucion
		AND CPD.NumeroCompraProductoDevolucion = CPDD.NumeroCompraProductoDevolucion
		WHERE CPD.NumeroAlmacen = @NumeroAlmacen
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraRegistro
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
	)TA	
	ORDER BY (CASE WHEN @DIUsuario IS NULL THEN 7 ELSE 6 END), (CASE WHEN @DIUsuario IS NULL THEN 3 ELSE 7 END), 1
END
GO


--EXEC DBO.ListarMovimientoDevoluciones 1, null, '01/01/2000', '31/12/2012'

