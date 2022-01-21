USE AlvecoComercial10
GO

DROP PROCEDURE ListarMovimientoTransaccionalProductos
GO

CREATE PROCEDURE ListarMovimientoTransaccionalProductos
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME

	
AS
BEGIN
	SELECT	VP.Numeroalmacen, VP.NumeroVentaProducto AS NumeroTransaccion, VPDE.CodigoProducto, dbo.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto ,
			VPD.PrecioUnitarioVenta AS PrecioUnitarioTransaccion, 
			VPDE.CantidadEntregada AS CantidadTransaccion, 
			VPDE.FechaHoraEntrega AS FechaTransaccion, 'VENTA' AS TipoTransaccion, 'SALIDA' AS TipoMovimiento
	FROM dbo.VentasProductos VP
	INNER JOIN dbo.VentasProductosDetalle VPD
	ON VP.NumeroAlmacen = VPD.NumeroAlmacen
	AND VP.NumeroVentaProducto = VPD.NumeroVentaProducto
	INNER JOIN dbo.VentasProductosDetalleEntrega VPDE
	ON VPD.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VPD.NumeroVentaProducto = VPDE.NumeroVentaProducto
	AND VPD.CodigoProducto = VPDE.CodigoProducto
	WHERE VP.CodigoEstadoVenta IN ('F','D','X')	
	AND VPDE.FechaHoraEntrega
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  VP.NumeroAlmacen  = @NumeroAlmacen
	
	UNION ALL
	
	SELECT	CPD.NumeroAlmacenDevolucion, CPD.NumeroVentaProductoDevolucion, CPDD.CodigoProducto, dbo.ObtenerNombreProducto(CPDD.CodigoProducto),
			CPDD.PrecioUnitarioDevolucion, CPDD.CantidadVentaDevolucion, CPD.FechaHoraDevolucion,
			'DEVOLUCION POR VENTA', 'SALIDA'
	FROM dbo.VentasProductosDevoluciones CPD
	INNER JOIN dbo.VentasProductosDevolucionesDetalle CPDD
	ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
	AND CPD.NumeroVentaProductoDevolucion = CPDD.NumeroVentaProductoDevolucion
	WHERE CPD.CodigoDevolucionVentaProducto IN ('F')
	AND CPD.FechaHoraDevolucion
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  CPD.NumeroAlmacenDevolucion  = @NumeroAlmacen
	
	
	UNION ALL
	
	SELECT	VPD.NumeroAlmacenRecepctor, VPD.NumeroTransferenciaProducto, VPDD.CodigoProducto,  dbo.ObtenerNombreProducto(VPDD.CodigoProducto),
			VPDD.PrecioUnitarioTransferencia, VPDD.CantidadTransferencia, VPD.FechaHoraTransferencia,
			'TRANSFERENCIA', 'SALIDA'
	FROM dbo.TransferenciasProductos VPD
	INNER JOIN TransferenciasProductosDetalle VPDD
	ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
	AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
	AND VPD.NumeroAlmacenRecepctor = @NumeroAlmacen
	AND VPD.CodigoEstadoTransferencia = 'F'	
	AND VPD.FechaHoraTransferencia
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  VPD.NumeroAlmacenRecepctor  = @NumeroAlmacen

	UNION ALL
	
	SELECT	VP.NumeroAlmacen, VP.NumeroCompraProducto, VPDE.CodigoProducto, dbo.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto ,
			VPD.PrecioUnitarioCompra, VPDE.CantidadEntregada, VPDE.FechaHoraEntrega, 'COMPRA', 'INGRESO' AS TipoMovimiento
	FROM dbo.ComprasProductos VP
	INNER JOIN dbo.ComprasProductosDetalle VPD
	ON VP.NumeroAlmacen = VPD.NumeroAlmacen
	AND VP.NumeroCompraProducto = VPD.NumeroCompraProducto
	INNER JOIN dbo.ComprasProductosDetalleEntrega VPDE
	ON VPD.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VPD.NumeroCompraProducto = VPDE.NumeroCompraProducto
	AND VPD.CodigoProducto = VPDE.CodigoProducto
	WHERE VP.CodigoEstadoCompra IN ('F','X','D')
	AND VPDE.FechaHoraEntrega
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  VP.NumeroAlmacen = @NumeroAlmacen
	
	UNION ALL
	
	SELECT	CPD.NumeroAlmacenDevolucion, CPD.NumeroCompraProductoDevolucion, CPDD.CodigoProducto, dbo.ObtenerNombreProducto(CPDD.CodigoProducto),
			CPDD.PrecioUnitarioDevolucion, CPDD.CantidadCompraDevolucion, CPD.FechaHoraDevolucion,
			'DEVOLUCION POR COMPRA', 'INGRESO'
	FROM dbo.ComprasProductosDevoluciones CPD
	INNER JOIN dbo.ComprasProductosDevolucionesDetalle CPDD
	ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
	AND CPD.NumeroCompraProductoDevolucion = CPDD.NumeroCompraProductoDevolucion
	WHERE CPD.CodigoDevolucionCompraProducto IN ('F')
	AND CPD.FechaHoraDevolucion
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  CPD.NumeroAlmacenDevolucion  = @NumeroAlmacen

	
	
	UNION ALL
	
	SELECT	VPD.NumeroAlmacenEmisor, VPD.NumeroTransferenciaProducto, VPDD.CodigoProducto, dbo.ObtenerNombreProducto(VPDD.CodigoProducto),
			VPDD.PrecioUnitarioTransferencia, VPDD.CantidadTransferencia, VPD.FechaHoraTransferencia,
			'TRANSFERENCIA', 'INGRESO'
	FROM dbo.TransferenciasProductos VPD
	INNER JOIN TransferenciasProductosDetalle VPDD
	ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
	AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
	AND VPD.NumeroAlmacenEmisor = @NumeroAlmacen
	AND VPD.CodigoEstadoTransferencia = 'F'	
	AND VPD.FechaHoraTransferencia
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  VPD.NumeroAlmacenEmisor  = @NumeroAlmacen
	
	
	ORDER BY 9, 7
END
GO

--EXEC ListarMovimientoTransaccionalProductos 1, '01/01/2000', '31/12/2012'