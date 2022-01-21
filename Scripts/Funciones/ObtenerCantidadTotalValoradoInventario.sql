USE AlvecoComercial10
GO

DROP FUNCTION ObtenerCantidadTotalValoradoInventario
GO


CREATE FUNCTION ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen INT, @CodigoProducto CHAR(15),
										@FechaInicio DATETIME, @FechaFin DATETIME)
RETURNS INT

AS
BEGIN
	DECLARE @CantidadTotalValorada		INT,
			@CantidadExistenciaActual	INT
	
	SELECT @CantidadExistenciaActual = CantidadExistencia
	FROM InventariosProductos
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND CodigoProducto = @CodigoProducto
	
	IF(@FechaInicio IS NULL AND @FechaFin IS NULL)
	BEGIN
		
		SELECT @CantidadTotalValorada = ISNULL(SUM(TMT.CantidadTotal),0)
		FROM
		(
			--INGRESOS
			SELECT IAD.CantidadEntregada AS CantidadTotal
			FROM ComprasProductos IA
			INNER JOIN ComprasProductosDetalleEntrega IAD
			ON IA.NumeroAlmacen = IAD.NumeroAlmacen
			AND IA.NumeroCompraProducto = IAD.NumeroCompraProducto
			WHERE IA.NumeroAlmacen = @NumeroAlmacen
			AND IAD.CodigoProducto = @CodigoProducto
			AND IA.CodigoEstadoCompra IN ('F','X','D')
			UNION ALL
			SELECT VPDD.CantidadVentaDevolucion
			FROM VentasProductosDevoluciones VPD
			INNER JOIN VentasProductosDevolucionesDetalle VPDD
			ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
			AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion			
			WHERE VPDD.CodigoProducto = @CodigoProducto			
			AND VPD.NumeroAlmacen = @NumeroAlmacen
			AND VPD.CodigoEstadoVentaDevolucion = 'F'
			UNION ALL
			SELECT VPDD.CantidadTransferencia
			FROM dbo.TransferenciasProductos VPD
			INNER JOIN TransferenciasProductosDetalle VPDD
			ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
			AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
			WHERE VPDD.CodigoProducto = @CodigoProducto			
			AND VPD.NumeroAlmacenRecepctor = @NumeroAlmacen
			AND VPD.CodigoEstadoTransferencia = 'F'			
			UNION ALL
			--EGRESOS
			SELECT -SAD.CantidadEntregada AS CantidadTotal
			FROM VentasProductos SA
			INNER JOIN VentasProductosDetalleEntrega SAD
			ON SA.NumeroAlmacen = SAD.NumeroAlmacen
			AND SA.NumeroVentaProducto = SAD.NumeroVentaProducto
			WHERE SA.NumeroAlmacen = @NumeroAlmacen
			AND SAD.CodigoProducto = @CodigoProducto
			AND SA.CodigoEstadoVenta IN ('F','X','E')
			UNION ALL
			SELECT -CPDD.CantidadCompraDevolucion
			FROM ComprasProductosDevoluciones CPD
			INNER JOIN ComprasProductosDevolucionesDetalle CPDD
			ON CPD.NumeroAlmacen = CPDD.NumeroAlmacenDevolucion
			AND CPD.NumeroCompraProductoDevolucion = CPDD.NumeroCompraProductoDevolucion			
			WHERE CPDD.CodigoProducto = @CodigoProducto			
			AND CPD.NumeroAlmacen = @NumeroAlmacen
			AND CPD.CodigoEstadoCompraDevolucion = 'F'			
			UNION ALL
			SELECT -VPDD.CantidadTransferencia
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
		SELECT @CantidadTotalValorada = ISNULL(SUM(TMT.CantidadTotal),0)
		FROM
		(
			SELECT IAD.CantidadEntregada AS CantidadTotal 
			FROM ComprasProductos IA
			INNER JOIN ComprasProductosDetalleEntrega IAD
			ON IA.NumeroAlmacen = IAD.NumeroAlmacen
			AND IA.NumeroCompraProducto = IAD.NumeroCompraProducto
			WHERE IA.NumeroAlmacen = @NumeroAlmacen
			AND IAD.CodigoProducto = @CodigoProducto
			AND IA.CodigoEstadoCompra IN ('F','X','D')
			AND IAD.FechaHoraEntrega
			BETWEEN @FechaInicio AND @FechaFin
			UNION ALL
			SELECT VPDD.CantidadVentaDevolucion
			FROM VentasProductosDevoluciones VPD
			INNER JOIN VentasProductosDevolucionesDetalle VPDD
			ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
			AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion			
			WHERE VPDD.CodigoProducto = @CodigoProducto			
			AND VPD.NumeroAlmacen = @NumeroAlmacen
			AND VPD.CodigoEstadoVentaDevolucion = 'F'
			AND VPD.FechaHoraRegistro
			BETWEEN @FechaInicio AND @FechaFin
			UNION ALL
			SELECT VPDD.CantidadTransferencia
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
			SELECT -SAD.CantidadEntregada
			FROM VentasProductos SA
			INNER JOIN VentasProductosDetalleEntrega SAD
			ON SA.NumeroAlmacen = SAD.NumeroAlmacen
			AND SA.NumeroVentaProducto = SAD.NumeroVentaProducto
			WHERE SA.NumeroAlmacen = @NumeroAlmacen
			AND SAD.CodigoProducto = @CodigoProducto
			AND SA.CodigoEstadoVenta IN ('F','X','E')
			AND SAD.FechaHoraEntrega
			BETWEEN @FechaInicio AND @FechaFin
			UNION ALL
			SELECT -CPDD.CantidadCompraDevolucion
			FROM ComprasProductosDevoluciones CPD
			INNER JOIN ComprasProductosDevolucionesDetalle CPDD
			ON CPD.NumeroAlmacen = CPDD.NumeroAlmacenDevolucion
			AND CPD.NumeroCompraProductoDevolucion = CPDD.NumeroCompraProductoDevolucion			
			WHERE CPDD.CodigoProducto = @CodigoProducto			
			AND CPD.NumeroAlmacen = @NumeroAlmacen
			AND CPD.CodigoEstadoCompraDevolucion = 'F'
			AND CPD.FechaHoraRegistro
			BETWEEN @FechaInicio AND @FechaFin
			UNION ALL
			SELECT -VPDD.CantidadTransferencia
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
	RETURN @CantidadTotalValorada
END
GO
