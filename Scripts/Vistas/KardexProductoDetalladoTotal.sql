--SELECT IDENT_SEED(TABLE_NAME) AS Seed,
--IDENT_INCR(TABLE_NAME) AS Increment,
--IDENT_CURRENT(TABLE_NAME) AS Current_Identity,
--TABLE_NAME
--FROM INFORMATION_SCHEMA.TABLES
--WHERE OBJECTPROPERTY(OBJECT_ID(TABLE_NAME), 'TableHasIdentity') = 1
--AND TABLE_TYPE = 'BASE TABLE'

USE AlvecoComercial10
GO

DROP VIEW KardexProductoDetalladoTotal
GO

CREATE VIEW KardexProductoDetalladoTotal
AS
SELECT	TAPrincipal.NumeroAlmacen,
		TAPrincipal.NumeroTransaccion, 
		TAPrincipal.NumeroTransaccionReal, TAPrincipal.FechaHoraEntrega,
		TAPrincipal.CodigoProducto, dbo.ObtenerNombreProducto(TAPrincipal.CodigoProducto)AS NombreProducto,
		TAPrincipal.TipoTransaccion, 
		TAPrincipal.CantidadEntregada,		
		TAPrincipal.PrecioUnitario,
		SUM(TASecundari2.CantidadEntregada) AS CantidadAcumulada,
		SUM(TASecundari2.PrecioTotal) AS PrecioTotalAcumulado,
		TAPrincipal.TipoMovimiento
FROM
(
	SELECT ROW_NUMBER() OVER(ORDER BY FechaHoraEntrega) AS NumeroTransaccion, 
			NumeroAlmacen, TipoTransaccion, NumeroTransaccionReal, FechaHoraEntrega, 
			CodigoProducto, CantidadEntregada, PrecioUnitario, PrecioTotal, TipoMovimiento
	FROM
	(
		--INVENTARIO INICIAL
		--SELECT	IP.NumeroAlmacen, -1 AS NumeroTransaccionReal, 
		--		'-1' AS CodigoTransaccionReal, DATEADD(YEAR, -10, GETDATE()) AS FechaHoraEntrega, 
		--		'IP' AS CodigoTipoTransaccion, 
		--		IP.CodigoProducto, 'I'  AS TipoTransaccion, 
		--		IP.CantidadExistencia - DBO.ObtenerCantidadTotalValoradoInventario(IP.NumeroAlmacen, IP.CodigoProducto, NULL, NULL) AS CantidadEntregada, 
		--		IP.PrecioUnitarioCompra  AS PrecioUnitarioCompra,
		--		(IP.CantidadExistencia - DBO.ObtenerCantidadTotalValoradoInventario(IP.NumeroAlmacen, IP.CodigoProducto, NULL, NULL)) *
		--		(IP.PrecioUnitarioCompra) AS PrecioTotal
		--FROM InventariosProductos IP
		--UNION ALL
	
		SELECT	IAD.NumeroAlmacen, 'I' as TipoTransaccion, IADE.NumeroCompraProducto AS NumeroTransaccionReal, IADE.FechaHoraEntrega, 
				IADE.CodigoProducto, IADE.CantidadEntregada, 
				IAD.PrecioUnitarioCompra as PrecioUnitario,
				IADE.CantidadEntregada * IAD.PrecioUnitarioCompra AS PrecioTotal, 'C' AS TipoMovimiento
		FROM ComprasProductosDetalle IAD
		INNER JOIN ComprasProductosDetalleEntrega IADE
		ON IAD.NumeroAlmacen = IADE.NumeroAlmacen
		AND IAD.NumeroCompraProducto = IADE.NumeroCompraProducto
		AND IAD.CodigoProducto = IADE.CodigoProducto
		
		UNION ALL

		SELECT	VPDD.NumeroAlmacenDevolucion, 'I', VPD.NumeroVentaProductoDevolucion, vpd.FechaHoraDevolucion,
				VPDD.CodigoProducto, vpdd.CantidadVentaDevolucion,
				VPDD.PrecioUnitarioDevolucion,
				VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion, 'DV'
		FROM VentasProductosDevoluciones VPD
		INNER JOIN VentasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion				
		WHERE VPD.CodigoEstadoVentaDevolucion = 'F'
		
		UNION ALL

		SELECT	VPD.NumeroAlmacenRecepctor, 'I', VPD.NumeroTransferenciaProducto, vpd.FechaHoraTransferencia,
				VPDD.CodigoProducto, vpdd.CantidadTransferencia,
				VPDD.PrecioUnitarioTransferencia,
				VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia, 'TI'
		FROM TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto				
		WHERE VPD.CodigoEstadoTransferencia = 'F'
		
		UNION ALL
		SELECT	SAD.NumeroAlmacen, 'E' as TipoTransaccion, SADE.NumeroVentaProducto, SADE.FechaHoraEntrega, 
				SADE.CodigoProducto, -SADE.CantidadEntregada, 
				CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.CodigoProducto) 
				WHEN 'P' THEN -SADE.PrecioUnitarioCompraInventario 
				WHEN 'U' THEN -SADE.PrecioUnitarioCompraInventario 
				WHEN 'A' THEN -SADE.PrecioUnitarioCompraInventario 
				WHEN 'B' THEN -SADE.PrecioUnitarioCompraInventario 
				ELSE -SAD.PrecioUnitarioVenta END AS PrecioUnitario,
				CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.CodigoProducto) 
				WHEN 'P' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'U' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'A' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'B' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				ELSE -SAD.PrecioUnitarioVenta * SADE.CantidadEntregada END AS PrecioTotal, 'V'
		FROM VentasProductosDetalle SAD
		INNER JOIN VentasProductosDetalleEntrega SADE
		ON SAD.NumeroAlmacen = SADE.NumeroAlmacen
		AND SAD.NumeroVentaProducto = SADE.NumeroVentaProducto
		AND SAD.CodigoProducto = SADE.CodigoProducto
		
		UNION ALL
		SELECT	VPD.NumeroAlmacenDevolucion, 'E', VPD.NumeroCompraProductoDevolucion, vpd.FechaHoraDevolucion,
				VPDD.CodigoProducto, -vpdd.CantidadCompraDevolucion,
				-VPDD.PrecioUnitarioDevolucion,
				-VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion, 'DC'
		FROM ComprasProductosDevoluciones VPD
		INNER JOIN ComprasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion				
		WHERE VPD.CodigoEstadoCompraDevolucion = 'F'
		
		UNION ALL
		
		SELECT	VPD.NumeroAlmacenEmisor, 'E', VPD.NumeroTransferenciaProducto, vpd.FechaHoraTransferencia,
				VPDD.CodigoProducto, -vpdd.CantidadTransferencia,
				-VPDD.PrecioUnitarioTransferencia,
				-VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia, 'TE'
		FROM TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto				
		WHERE VPD.CodigoEstadoTransferencia = 'F'
		
	)TAForanea2
) TAPrincipal
JOIN 
(
	SELECT ROW_NUMBER() OVER(ORDER BY FechaHoraEntrega) AS NumeroTransaccion, 
			NumeroAlmacen, TipoTransaccion, NumeroTransaccionReal, FechaHoraEntrega, 
			CodigoProducto, CantidadEntregada, PrecioUnitario, PrecioTotal, TipoMovimiento
	FROM
	(
		--SELECT	IP.NumeroAlmacen, -1 AS NumeroTransaccionReal, 
		--		'-1' AS CodigoTransaccionReal, DATEADD(YEAR, -10, GETDATE()) AS FechaHoraEntrega, 
		--		'IP' AS CodigoTipoTransaccion, 
		--		IP.CodigoProducto, 'I'  AS TipoTransaccion, 
		--		IP.CantidadExistencia - DBO.ObtenerCantidadTotalValoradoInventario(IP.NumeroAlmacen, IP.CodigoProducto, NULL, NULL) AS CantidadEntregada, 
		--		IP.PrecioUnitarioCompra  AS PrecioUnitarioCompra,
		--		(IP.CantidadExistencia - DBO.ObtenerCantidadTotalValoradoInventario(IP.NumeroAlmacen, IP.CodigoProducto, NULL, NULL)) *
		--		(IP.PrecioUnitarioCompra) AS PrecioTotal
		--FROM InventariosProductos IP
		--UNION ALL
	
		SELECT	IAD.NumeroAlmacen, 'I' as TipoTransaccion, IADE.NumeroCompraProducto AS NumeroTransaccionReal, IADE.FechaHoraEntrega, 
				IADE.CodigoProducto, IADE.CantidadEntregada, 
				IAD.PrecioUnitarioCompra as PrecioUnitario,
				IADE.CantidadEntregada * IAD.PrecioUnitarioCompra AS PrecioTotal, 'C' AS TipoMovimiento
		FROM ComprasProductosDetalle IAD
		INNER JOIN ComprasProductosDetalleEntrega IADE
		ON IAD.NumeroAlmacen = IADE.NumeroAlmacen
		AND IAD.NumeroCompraProducto = IADE.NumeroCompraProducto
		AND IAD.CodigoProducto = IADE.CodigoProducto
		
		UNION ALL

		SELECT	VPD.NumeroAlmacenDevolucion, 'I', VPD.NumeroVentaProductoDevolucion, vpd.FechaHoraDevolucion,
				VPDD.CodigoProducto, vpdd.CantidadVentaDevolucion,
				VPDD.PrecioUnitarioDevolucion,
				VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion, 'DV'
		FROM VentasProductosDevoluciones VPD
		INNER JOIN VentasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion				
		WHERE VPD.CodigoEstadoVentaDevolucion = 'F'
		
		UNION ALL 
		
		SELECT	VPD.NumeroAlmacenRecepctor, 'I', VPD.NumeroTransferenciaProducto, vpd.FechaHoraTransferencia,
				VPDD.CodigoProducto, vpdd.CantidadTransferencia,
				VPDD.PrecioUnitarioTransferencia,
				VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia, 'TI'
		FROM TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto				
		WHERE VPD.CodigoEstadoTransferencia = 'F'
		
		UNION ALL
		SELECT	SAD.NumeroAlmacen, 'E' as TipoTransaccion, SADE.NumeroVentaProducto, SADE.FechaHoraEntrega, 
				SADE.CodigoProducto, -SADE.CantidadEntregada, 
				CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.CodigoProducto) 
				WHEN 'P' THEN -SADE.PrecioUnitarioCompraInventario 
				WHEN 'U' THEN -SADE.PrecioUnitarioCompraInventario 
				WHEN 'A' THEN -SADE.PrecioUnitarioCompraInventario 
				WHEN 'B' THEN -SADE.PrecioUnitarioCompraInventario 
				ELSE -SAD.PrecioUnitarioVenta END AS PrecioUnitario,
				CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.CodigoProducto) 
				WHEN 'P' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'U' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'A' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				WHEN 'B' THEN -SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada
				ELSE -SAD.PrecioUnitarioVenta * SADE.CantidadEntregada END AS PrecioTotal, 'V'
		FROM VentasProductosDetalle SAD
		INNER JOIN VentasProductosDetalleEntrega SADE
		ON SAD.NumeroAlmacen = SADE.NumeroAlmacen
		AND SAD.NumeroVentaProducto = SADE.NumeroVentaProducto
		AND SAD.CodigoProducto = SADE.CodigoProducto
		
		UNION ALL
		SELECT	VPD.NumeroAlmacenDevolucion, 'E', VPD.NumeroCompraProductoDevolucion, vpd.FechaHoraDevolucion,
				VPDD.CodigoProducto, -vpdd.CantidadCompraDevolucion,
				-VPDD.PrecioUnitarioDevolucion,
				-VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion, 'DC'
		FROM ComprasProductosDevoluciones VPD
		INNER JOIN ComprasProductosDevolucionesDetalle VPDD
		ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion				
		WHERE VPD.CodigoEstadoCompraDevolucion = 'F'
		
		UNION ALL 
		SELECT	VPD.NumeroAlmacenEmisor, 'E', VPD.NumeroTransferenciaProducto, vpd.FechaHoraTransferencia,
				VPDD.CodigoProducto, -vpdd.CantidadTransferencia,
				-VPDD.PrecioUnitarioTransferencia,
				-VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia, 'TE'
		FROM TransferenciasProductos VPD
		INNER JOIN TransferenciasProductosDetalle VPDD
		ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto				
		WHERE VPD.CodigoEstadoTransferencia = 'F'
		
	)TAForanea2
)TASecundari2
ON TAPrincipal.NumeroTransaccion >= TASecundari2.NumeroTransaccion
AND TAPrincipal.CodigoProducto = TASecundari2.CodigoProducto
AND TAPrincipal.NumeroAlmacen = TASecundari2.NumeroAlmacen
GROUP BY TAPrincipal.NumeroAlmacen, TAPrincipal.NumeroTransaccion, TAPrincipal.CodigoProducto, 
TAPrincipal.CantidadEntregada, TAPrincipal.TipoTransaccion, 
TAPrincipal.NumeroTransaccionReal, TAPrincipal.FechaHoraEntrega,
TAPrincipal.PrecioUnitario,
TAPrincipal.TipoMovimiento
--ORDER BY TAPrincipal.NumeroTransaccion


--SELECT * 
--FROM KardexProductoDetalladoTotal
--where 
----tipomovimiento in ('ti','te')
--NumeroAlmacen = 3
--order by nombreproducto, fechahoraEntrega
