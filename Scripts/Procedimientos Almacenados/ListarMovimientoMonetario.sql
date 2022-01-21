USE AlvecoComercial10
GO

DROP PROCEDURE ListarMovimientoMonetario
GO

CREATE PROCEDURE ListarMovimientoMonetario
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
		SELECT	NumeroCompraProducto AS NumeroTransaccion, CodigoCompraProducto as CodigoTransaccion, 
				FechaHoraRegistro as FechaHoraTransaccion, 
				CASE WHEN CodigoTipoCompra = 'R' THEN 'CREDITO' ELSE 'EFECTIVO' END AS TipoPago,
				MontoTotalCompra AS MontoTotalTransaccion, MontoTotalPagoEfectivo AS MontoPagoCobroEfectivo, 
				dbo.ObtenerNombreCompleto(DIUsuario) AS NombreUsuario, 'COMPRAS' AS TipoTransaccion,
				'SALIDAS' AS TipoMovimiento, NumeroAlmacen
		FROM dbo.ComprasProductos
		WHERE CAST(NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraRegistro
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL
			
		SELECT NumeroTransferenciaProducto, ' ', FechaHoraTransferencia, 'EFECTIVO', MontoTotalTransferencia,
				MontoTotalTransferencia, dbo.ObtenerNombreCompleto(DIUsuario) AS NombreUsuario, 
				'TRANSFERENCIA' AS TipoTransaccion, 'SALIDAS' AS TipoMovimiento, NumeroAlmacenEmisor
		FROM dbo.TransferenciasProductos TP
		WHERE CAST(NumeroAlmacenEmisor AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraTransferencia
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL
		
		SELECT  NumeroVentaProductoDevolucion, CodigoDevolucionVentaProducto,
				FechaHoraRegistro, 'EFECTIVO',
				MontoTotalVentaDevolucion, MontoTotalPagoEfectivo,
				dbo.ObtenerNombreCompleto(DIUsuario), 'DEVOLUCION POR VENTA', 'SALIDAS'	,NumeroAlmacen		
		FROM dbo.VentasProductosDevoluciones
		WHERE CAST(NumeroAlmacenDevolucion AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraRegistro
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL
		
		SELECT	CPPP.NumeroCuentaPorPagar, ' ',  CPPP.FechaHoraPago, 'EFECTIVO',
				CPPP.Monto, CPPP.Monto, dbo.ObtenerNombreCompleto(CPPP.DIUsuario), 'CUENTAS POR PAGAR', 'SALIDAS',NumeroAlmacen
		FROM dbo.CuentasPorPagarPagos CPPP
		INNER JOIN dbo.CuentasPorPagar CPP
		ON CPPP.NumeroCuentaPorPagar = CPP.NumeroCuentaPorPagar
		WHERE CAST(CPP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND CPPP.DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND CPPP.FechaHoraPago
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		
		
		UNION ALL
		
		SELECT	NumeroVentaProducto AS NumeroTransaccion, CodigoVentaProducto as CodigoTransaccion, 
				FechaHoraVenta as FechaHoraTransaccion, 
				CASE WHEN CodigoTipoVenta = 'R' THEN 'CREDITO' ELSE 'EFECTIVO' END AS TipoPago,
				MontoTotalVenta AS MontoTotalTransaccion, MontoTotalPagoEfectivo AS MontoPagoCobroEfectivo, 
				dbo.ObtenerNombreCompleto(DIUsuario) AS NombreUsuario, 'VENTAS' AS TipoTransaccion, 
				'INGRESOS' AS TipoMovimiento, NumeroAlmacen
		FROM dbo.VentasProductos
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraVenta
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL
		
		SELECT  NumeroCompraProductoDevolucion, CodigoDevolucionCompraProducto,
				FechaHoraRegistro, 'EFECTIVO',
				MontoTotalCompraDevolucion, MontoTotalPagoEfectivo,
				dbo.ObtenerNombreCompleto(DIUsuario), 'DEVOLUCION POR COMPRA', 'INGRESOS', NumeroAlmacen			
		FROM dbo.ComprasProductosDevoluciones
		WHERE CAST(NumeroAlmacenDevolucion AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraRegistro
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL
		
		SELECT	CPCC.NumeroCuentaPorCobrar, ' ',  CPCC.FechaHoraCobro, 'EFECTIVO',
				CPCC.Monto, CPCC.Monto, dbo.ObtenerNombreCompleto(CPCC.DIUsuario), 'CUENTAS POR COBRAR', 'INGRESOS', NumeroAlmacen
		FROM dbo.CuentasPorCobrarCobros CPCC
		INNER JOIN dbo.CuentasPorCobrar CPC
		ON CPCC.NumeroCuentaPorCobrar = CPC.NumeroCuentaPorCobrar
		WHERE CAST(NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND CPCC.DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND CPCC.FechaHoraCobro
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
		
		UNION ALL
			
		SELECT NumeroTransferenciaProducto, ' ', FechaHoraTransferencia, 'EFECTIVO', MontoTotalTransferencia,
				MontoTotalTransferencia, dbo.ObtenerNombreCompleto(DIUsuario) AS NombreUsuario, 
				'TRANSFERENCIA' AS TipoTransaccion, 'INGRESOS' AS TipoMovimiento, NumeroAlmacenRecepctor
		FROM dbo.TransferenciasProductos TP
		WHERE CAST(NumeroAlmacenRecepctor AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END
		AND FechaHoraTransferencia
		BETWEEN @FechaHoraInicio AND @FechaHoraFin
	)T
	
	ORDER BY (CASE WHEN @DIUsuario IS NULL THEN 9 ELSE 7 END), (CASE WHEN @DIUsuario IS NULL THEN 3 ELSE 9 END)
END
GO


--EXEC DBO.ListarMovimientoMonetario 1, null, '01/01/2000', '31/12/2012'
