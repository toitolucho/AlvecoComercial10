USE AlvecoComercial10
GO

DROP FUNCTION ObtenerMontoTotalValorado
GO


CREATE FUNCTION ObtenerMontoTotalValorado(@NumeroAlmacen INT, @CodigoProducto CHAR(15),
										@FechaInicio DATETIME, @FechaFin DATETIME)
RETURNS DECIMAL(10,3)

AS
BEGIN
	DECLARE @MontoTotalValorado				DECIMAL(10,3)
	
	
	IF(@FechaInicio IS NULL AND @FechaFin IS NULL)
	BEGIN
		SELECT @MontoTotalValorado = ISNULL(SUM(TMT.PrecioTotal),0)
		FROM
		(
		SELECT IA.FechaHoraRecepcion AS FechaTransaccion, IADE.CantidadEntregada * IAD.PrecioUnitarioCompra AS PrecioTotal 
		FROM ComprasProductos IA
		INNER JOIN ComprasProductosDetalle IAD
		ON IA.NumeroAlmacen = IAD.NumeroAlmacen
		AND IA.NumeroCompraProducto = IAD.NumeroCompraProducto
		INNER JOIN ComprasProductosDetalleEntrega IADE
		ON IAD.NumeroAlmacen = IADE.NumeroAlmacen
		AND IAD.NumeroCompraProducto = IADE.NumeroCompraProducto
		AND IAD.CodigoProducto = IADE.CodigoProducto
		WHERE IA.NumeroAlmacen = @NumeroAlmacen
		AND IAD.CodigoProducto = @CodigoProducto
		AND  IA.CodigoEstadoCompra IN('F','X','D')
		UNION ALL
		SELECT VPD.FechaHoraDevolucion, VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion
		FROM VentasProductosDevoluciones VPD
		INNER JOIN VentasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPDD.CodigoProducto = @CodigoProducto
		AND VPD.CodigoEstadoVentaDevolucion = 'F'
		UNION ALL
		SELECT VPD.FechaHoraTransferencia, VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
		FROM dbo.TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
		WHERE VPDD.CodigoProducto = @CodigoProducto			
		AND VPD.NumeroAlmacenRecepctor = @NumeroAlmacen
		AND VPD.CodigoEstadoTransferencia = 'F'			
		UNION ALL
		SELECT	SA.FechaHoraVenta,
				CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(@CodigoProducto) 
				WHEN 'P' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'U' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'A' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'B' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				ELSE -SAD.PrecioUnitarioVenta * SADE.CantidadEntregada END AS PrecioTotal  		  
		FROM VentasProductos SA
		INNER JOIN VentasProductosDetalle SAD
		ON SA.NumeroAlmacen = SAD.NumeroAlmacen
		AND SA.NumeroVentaProducto = SAD.NumeroVentaProducto
		INNER JOIN VentasProductosDetalleEntrega SADE
		ON SAD.NumeroAlmacen = SADE.NumeroAlmacen
		AND SAD.NumeroVentaProducto = SADE.NumeroVentaProducto
		AND SAD.CodigoProducto = SADE.CodigoProducto
		WHERE SA.NumeroAlmacen = @NumeroAlmacen
		AND SAD.CodigoProducto = @CodigoProducto
		
		UNION ALL
		SELECT VPD.FechaHoraDevolucion, -VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion
		FROM ComprasProductosDevoluciones VPD
		INNER JOIN ComprasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPDD.CodigoProducto = @CodigoProducto
		AND VPD.CodigoEstadoCompraDevolucion = 'F'
		
		UNION ALL
		SELECT VPD.FechaHoraTransferencia, -VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
		FROM dbo.TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
		WHERE VPDD.CodigoProducto = @CodigoProducto			
		AND VPD.NumeroAlmacenEmisor = @NumeroAlmacen
		AND VPD.CodigoEstadoTransferencia = 'F'			
		
		) TMT

	END
	ELSE
	BEGIN
		SELECT @MontoTotalValorado = ISNULL(SUM(TMT.PrecioTotal),0)
		FROM
		(
		SELECT IA.FechaHoraRecepcion AS FechaTransaccion, IADE.CantidadEntregada * IAD.PrecioUnitarioCompra AS PrecioTotal 
		FROM ComprasProductos IA
		INNER JOIN ComprasProductosDetalle IAD
		ON IA.NumeroAlmacen = IAD.NumeroAlmacen
		AND IA.NumeroCompraProducto = IAD.NumeroCompraProducto
		INNER JOIN ComprasProductosDetalleEntrega IADE
		ON IAD.NumeroAlmacen = IADE.NumeroAlmacen
		AND IAD.NumeroCompraProducto = IADE.NumeroCompraProducto
		AND IAD.CodigoProducto = IADE.CodigoProducto
		WHERE IA.NumeroAlmacen = @NumeroAlmacen
		AND IAD.CodigoProducto = @CodigoProducto
		AND  IA.CodigoEstadoCompra IN('F','X','D')
		AND IADE.FechaHoraEntrega
		BETWEEN @FechaInicio AND @FechaFin
		UNION ALL
		SELECT VPD.FechaHoraDevolucion, VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion
		FROM VentasProductosDevoluciones VPD
		INNER JOIN VentasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPDD.CodigoProducto = @CodigoProducto
		AND VPD.CodigoEstadoVentaDevolucion = 'F'
		AND VPD.FechaHoraDevolucion 
		BETWEEN @FechaInicio AND @FechaFin
		UNION ALL
		SELECT VPD.FechaHoraTransferencia, VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
		FROM dbo.TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
		WHERE VPDD.CodigoProducto = @CodigoProducto			
		AND VPD.NumeroAlmacenRecepctor = @NumeroAlmacen
		AND VPD.CodigoEstadoTransferencia = 'F'	
		AND VPD.FechaHoraTransferencia
		BETWEEN @FechaInicio AND @FechaFin				
		UNION ALL
		SELECT	SA.FechaHoraVenta, ---SADE.CantidadEntregada * SAD.PrecioUnitarioVenta AS PrecioTotal
				CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(@CodigoProducto) 
				WHEN 'P' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'U' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'A' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'B' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				ELSE -SAD.PrecioUnitarioVenta * SADE.CantidadEntregada END AS PrecioTotal
		FROM VentasProductos SA
		INNER JOIN VentasProductosDetalle SAD
		ON SA.NumeroAlmacen = SAD.NumeroAlmacen
		AND SA.NumeroVentaProducto = SAD.NumeroVentaProducto
		INNER JOIN VentasProductosDetalleEntrega SADE
		ON SAD.NumeroAlmacen = SADE.NumeroAlmacen
		AND SAD.NumeroVentaProducto = SADE.NumeroVentaProducto
		AND SAD.CodigoProducto = SADE.CodigoProducto
		WHERE SA.NumeroAlmacen = @NumeroAlmacen
		AND SAD.CodigoProducto = @CodigoProducto
		AND SADE.FechaHoraEntrega
		BETWEEN @FechaInicio AND @FechaFin
		UNION ALL
		SELECT VPD.FechaHoraDevolucion, -VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion
		FROM ComprasProductosDevoluciones VPD
		INNER JOIN ComprasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPDD.CodigoProducto = @CodigoProducto
		AND VPD.CodigoEstadoCompraDevolucion = 'F'
		AND VPD.FechaHoraDevolucion
		BETWEEN @FechaInicio AND @FechaFin
		UNION ALL
		SELECT VPD.FechaHoraTransferencia, -VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
		FROM dbo.TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
		WHERE VPDD.CodigoProducto = @CodigoProducto			
		AND VPD.NumeroAlmacenEmisor = @NumeroAlmacen
		AND VPD.CodigoEstadoTransferencia = 'F'			
		AND VPD.FechaHoraTransferencia
		BETWEEN @FechaInicio AND @FechaFin
		) TMT

	END
	RETURN @MontoTotalValorado
END
GO
