USE AlvecoComercial10
GO

DROP PROCEDURE ListarMovimientoProductoReporte
GO

CREATE PROCEDURE ListarMovimientoProductoReporte
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@ListadoProductos	VARCHAR(4000)

AS
BEGIN
	IF(@ListadoProductos IS NULL OR RTRIM(@ListadoProductos) = '')
	BEGIN
		SELECT	A.CodigoProducto, dbo.ObtenerNombreProducto(A.CodigoProducto) AS NombreProducto, 				
				A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), @FechaHoraInicio)
				- DBO.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) AS CantidadInicial,									
				A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), @FechaHoraInicio)
				- DBO.ObtenerMontoTotalValorado(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) AS PrecioTotalInicial,					
				ISNULL(TIA.CantidadCompras,0) AS CantidadCompras, ISNULL(TIA.PrecioTotalCompras,0) AS PrecioTotalCompras, 
				ISNULL(TSA.CantidadVenta,0) AS CantidadVenta, ISNULL(TSA.PrecioTotalVentas,0) AS PrecioTotalVentas,
				A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), @FechaHoraInicio)
				- DBO.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, A.CodigoProducto,DATEADD(YEAR, -50,GETDATE()),GETDATE()) + 
				ISNULL(TIA.CantidadCompras,0) - ISNULL(TSA.CantidadVenta,0)AS CantidadSaldo,					
				A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),@FechaHoraInicio)
				- DBO.ObtenerMontoTotalValorado(@NumeroAlmacen, A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) + 				
				ISNULL(TIA.PrecioTotalCompras,0) - ISNULL(TSA.PrecioTotalVentas,0) AS PrecioTotalSaldo
		FROM InventariosProductos A
		LEFT JOIN
		(
			SELECT	INGRESOS.NumeroAlmacen, INGRESOS.CodigoProducto, 
					ISNULL(SUM(INGRESOS.CantidadCompras),0) AS CantidadCompras, 
					ISNULL( SUM(INGRESOS.PrecioTotalCompras), 0) AS PrecioTotalCompras
			FROM
			(
				SELECT IADE.NumeroAlmacen, IADE.CodigoProducto, IADE.CantidadEntregada AS CantidadCompras, IAD.PrecioUnitarioCompra * IADE.CantidadEntregada AS PrecioTotalCompras
				FROM ComprasProductosDetalleEntrega IADE
				INNER JOIN ComprasProductosDetalle IAD
				ON IADE.CodigoProducto = IAD.CodigoProducto
				AND IADE.NumeroCompraProducto = IAD.NumeroCompraProducto
				AND IADE.NumeroAlmacen = IAD.NumeroAlmacen
				INNER JOIN ComprasProductos IA
				ON IADE.NumeroAlmacen = IA.NumeroAlmacen
				AND IADE.NumeroCompraProducto = IA.NumeroCompraProducto
				WHERE IA.CodigoEstadoCompra IN ('F','X','D')
				AND IADE.FechaHoraEntrega 
				BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
				AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))			
				AND IAD.NumeroAlmacen = @NumeroAlmacen
				UNION ALL
				SELECT VPD.NumeroAlmacenDevolucion, VPDD.CodigoProducto, VPDD.CantidadVentaDevolucion, VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion
				FROM VentasProductosDevoluciones VPD
				INNER JOIN VentasProductosDevolucionesDetalle VPDD
				ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
				AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion
				WHERE VPD.CodigoEstadoVentaDevolucion = 'F'
				AND VPD.FechaHoraDevolucion
				BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
				AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))			
				AND VPD.NumeroVentaProductoDevolucion = @NumeroAlmacen
				UNION ALL
				SELECT VPD.NumeroAlmacenRecepctor, VPDD.CodigoProducto, VPDD.CantidadTransferencia, VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
				FROM dbo.TransferenciasProductos VPD
				INNER JOIN TransferenciasProductosDetalle VPDD
				ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
				AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
				AND VPD.NumeroAlmacenRecepctor = @NumeroAlmacen
				AND VPD.CodigoEstadoTransferencia = 'F'	
				AND VPD.FechaHoraTransferencia
				BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
				AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))			
			)INGRESOS
			GROUP BY INGRESOS.NumeroAlmacen, INGRESOS.CodigoProducto
		) TIA
		ON A.CodigoProducto = TIA.CodigoProducto	
		AND A.NumeroAlmacen = TIA.NumeroAlmacen
		LEFT JOIN
		(
			SELECT EGRESOS.NumeroAlmacen, EGRESOS.CodigoProducto,
				   ISNULL(SUM(EGRESOS.CantidadVenta),0) AS CantidadVenta,
				   ISNULL(SUM(EGRESOS.PrecioTotalVentas),0) AS PrecioTotalVentas
			FROM
			(
				SELECT SADE.NumeroAlmacen, SADE.CodigoProducto, ISNULL(SUM(SADE.CantidadEntregada),0) AS CantidadVenta, 
						CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.codigoProducto)
						WHEN 'P' THEN ISNULL(SUM(SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada),0)
						WHEN 'U' THEN ISNULL(SUM(SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada),0)
						ELSE ISNULL(SUM(SAD.PrecioUnitarioVenta * SADE.CantidadEntregada),0) END AS PrecioTotalVentas					
				FROM VentasProductosDetalleEntrega SADE
				INNER JOIN VentasProductosDetalle SAD
				ON SADE.NumeroAlmacen = SAD.NumeroAlmacen
				AND SADE.NumeroVentaProducto = SAD.NumeroVentaProducto
				AND SADE.CodigoProducto = SAD.CodigoProducto
				INNER JOIN VentasProductos SA
				ON SADE.NumeroAlmacen = SA.NumeroAlmacen
				AND SADE.NumeroVentaProducto = SA.NumeroVentaProducto
				WHERE SA.CodigoEstadoVenta IN ('F','X','E')
				AND SADE.FechaHoraEntrega 
				BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
				AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))
				AND SAD.NumeroAlmacen = @NumeroAlmacen
				GROUP BY SADE.NumeroAlmacen,SADE.CodigoProducto
				UNION ALL
				SELECT VPD.NumeroAlmacen, VPDD.CodigoProducto, VPDD.CantidadCompraDevolucion, VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion
				FROM ComprasProductosDevoluciones VPD
				INNER JOIN ComprasProductosDevolucionesDetalle VPDD
				ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
				AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion
				WHERE VPD.CodigoEstadoCompraDevolucion = 'F'
				AND VPD.FechaHoraDevolucion
				BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
				AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))			
				AND VPD.NumeroAlmacenDevolucion = @NumeroAlmacen
				UNION ALL 
				SELECT VPD.NumeroAlmacenEmisor, VPDD.CodigoProducto, VPDD.CantidadTransferencia, VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
				FROM dbo.TransferenciasProductos VPD
				INNER JOIN TransferenciasProductosDetalle VPDD
				ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
				AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
				AND VPD.NumeroAlmacenEmisor = @NumeroAlmacen
				AND VPD.CodigoEstadoTransferencia = 'F'	
				AND VPD.FechaHoraTransferencia
				BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
				AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))					
			)EGRESOS
			GROUP BY EGRESOS.NumeroAlmacen, EGRESOS.CodigoProducto
		) TSA
		ON A.CodigoProducto = TSA.CodigoProducto
		AND A.NumeroAlmacen = TSA.NumeroAlmacen
		WHERE A.NumeroAlmacen = @NumeroAlmacen
	
	END
	ELSE
	BEGIN
		DECLARE @ConsultaSQL	VARCHAR(max)
		DECLARE @FechaInicioLiteral VARCHAR(10) = CONVERT(VARCHAR(20),@FechaHoraInicio,103),
				@FechaFinLiteral VARCHAR(10) = CONVERT(VARCHAR(20),@FechaHoraFin,103),
				@NumeroAgenciaLiteral	VARCHAR(10) = RTRIM(CAST(@NumeroAlmacen AS VARCHAR(10)))
		--SET @ConsultaSQL =
		--'
		--	SELECT A.CodigoProducto, dbo.ObtenerNombreProducto(A.CodigoProducto) AS NombreProducto, 
		--		A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
		--		- DBO.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) AS CantidadInicial,					
		--		A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
		--		- DBO.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) AS PrecioTotalInicial,					
		--		ISNULL(TIA.CantidadCompras,0) AS CantidadCompras, ISNULL(TIA.PrecioTotalCompras,0) AS PrecioTotalCompras, 
		--		ISNULL(TSA.CantidadVenta,0) AS CantidadVenta, ISNULL(TSA.PrecioTotalVentas,0) AS PrecioTotalVentas,
		--		A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
		--		- DBO.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto,DATEADD(YEAR, -50,GETDATE()),GETDATE()) + 
		--		ISNULL(TIA.CantidadCompras,0) - ISNULL(TSA.CantidadVenta,0)AS CantidadSaldo,					
		--		A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
		--		- DBO.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) + 				
		--		ISNULL(TIA.PrecioTotalCompras,0) - ISNULL(TSA.PrecioTotalVentas,0) AS PrecioTotalSaldo
		--	FROM InventariosProductos A
		--	LEFT JOIN
		--	(
		--		SELECT	INGRESOS.NumeroAlmacen, INGRESOS.CodigoProducto, 
		--			ISNULL(SUM(INGRESOS.CantidadCompras),0) AS CantidadCompras, 
		--			ISNULL( SUM(INGRESOS.PrecioTotalCompras), 0) AS PrecioTotalCompras
		--		FROM
		--		(
		--			SELECT IADE.NumeroAlmacen, IADE.CodigoProducto, IADE.CantidadEntregada AS CantidadCompras, IAD.PrecioUnitarioCompra * IADE.CantidadEntregada AS PrecioTotalCompras
		--			FROM ComprasProductosDetalleEntrega IADE
		--			INNER JOIN ComprasProductosDetalle IAD
		--			ON IADE.CodigoProducto = IAD.CodigoProducto
		--			AND IADE.NumeroCompraProducto = IAD.NumeroCompraProducto
		--			AND IADE.NumeroAlmacen = IAD.NumeroAlmacen
		--			INNER JOIN ComprasProductos IA
		--			ON IADE.NumeroAlmacen = IA.NumeroAlmacen
		--			AND IADE.NumeroCompraProducto = IA.NumeroCompraProducto
		--			WHERE IA.CodigoEstadoCompra IN (''F'',''X'')
		--			AND IADE.FechaHoraEntrega 
		--			BETWEEN  '''+@FechaInicioLiteral+''' AND '''+@FechaFinLiteral +'''	
		--			AND IADE.CodigoProducto IN (' + @ListadoProductos + ')	
		--			UNION ALL
		--			SELECT VPD.NumeroAlmacen, VPDD.CodigoProducto, VPDD.CantidadVentaDevolucion, VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion
		--			FROM VentasProductosDevoluciones VPD
		--			INNER JOIN VentasProductosDevolucionesDetalle VPDD
		--			ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		--			AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion
		--			WHERE VPD.CodigoEstadoVentaDevolucion = ''F''
		--			AND VPD.FechaHoraDevolucion
		--			BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +''' 	
		--			AND VPDD.CodigoProducto IN (' + @ListadoProductos + ')	
		--		)INGRESOS
		--		GROUP BY INGRESOS.NumeroAlmacen, INGRESOS.CodigoProducto
		--	) TIA
		--	ON A.CodigoProducto = TIA.CodigoProducto	
		--	AND A.NumeroAlmacen = TIA.NumeroAlmacen
		--	LEFT JOIN
		--	(
		--		SELECT EGRESOS.NumeroAlmacen, EGRESOS.CodigoProducto,
		--		   ISNULL(SUM(EGRESOS.CantidadVenta),0) AS CantidadVenta,
		--		   ISNULL(SUM(EGRESOS.PrecioTotalVentas),0) AS PrecioTotalVentas
		--		FROM
		--		(
		--			SELECT SADE.NumeroAlmacen, SADE.CodigoProducto, ISNULL(SUM(SADE.CantidadEntregada),0) AS CantidadVenta, 
		--					CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.codigoProducto)
		--					WHEN ''P'' THEN ISNULL(SUM(SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada),0)
		--					WHEN ''U'' THEN ISNULL(SUM(SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada),0)
		--					ELSE ISNULL(SUM(SAD.PrecioUnitarioVenta * SADE.CantidadEntregada),0) END AS PrecioTotalVentas					
		--			FROM VentasProductosDetalleEntrega SADE
		--			INNER JOIN VentasProductosDetalle SAD
		--			ON SADE.NumeroAlmacen = SAD.NumeroAlmacen
		--			AND SADE.NumeroVentaProducto = SAD.NumeroVentaProducto
		--			AND SADE.CodigoProducto = SAD.CodigoProducto
		--			INNER JOIN VentasProductos SA
		--			ON SADE.NumeroAlmacen = SA.NumeroAlmacen
		--			AND SADE.NumeroVentaProducto = SA.NumeroVentaProducto
		--			WHERE SA.CodigoEstadoVenta IN (''F'',''X'')
		--			AND SADE.FechaHoraEntrega 
		--			BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +''' 
		--			AND SADE.CodigoProducto IN (' + @ListadoProductos + ')
		--			GROUP BY SADE.NumeroAlmacen,SADE.CodigoProducto
		--			UNION ALL
		--			SELECT VPD.NumeroAlmacen, VPDD.CodigoProducto, VPDD.CantidadCompraDevolucion, VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion
		--			FROM ComprasProductosDevoluciones VPD
		--			INNER JOIN ComprasProductosDevolucionesDetalle VPDD
		--			ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
		--			AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion
		--			WHERE VPD.CodigoEstadoCompraDevolucion = ''F''
		--			AND VPD.FechaHoraDevolucion
		--			BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +'''
		--			AND VPDD.CodigoProducto IN (' + @ListadoProductos + ')
		--		)EGRESOS
		--		GROUP BY EGRESOS.NumeroAlmacen, EGRESOS.CodigoProducto
		--	) TSA
		--	ON A.CodigoProducto = TSA.CodigoProducto
		--	AND A.NumeroAlmacen = TSA.NumeroAlmacen
		--'
		--PRINT @ConsultaSQL + 'WHERE A.CodigoProducto IN  (' + @ListadoProductos + ') ORDER BY 2'	
		--EXEC (@ConsultaSQL+ 'WHERE A.CodigoProducto IN  (' + @ListadoProductos + ') ORDER BY 2')
		EXEC('
			SELECT A.CodigoProducto, dbo.ObtenerNombreProducto(A.CodigoProducto) AS NombreProducto, 
				A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
				- DBO.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) AS CantidadInicial,					
				A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
				- DBO.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) AS PrecioTotalInicial,					
				ISNULL(TIA.CantidadCompras,0) AS CantidadCompras, ISNULL(TIA.PrecioTotalCompras,0) AS PrecioTotalCompras, 
				ISNULL(TSA.CantidadVenta,0) AS CantidadVenta, ISNULL(TSA.PrecioTotalVentas,0) AS PrecioTotalVentas,
				A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
				- DBO.ObtenerCantidadTotalValoradoInventario('+ @NumeroAgenciaLiteral + ', A.CodigoProducto,DATEADD(YEAR, -50,GETDATE()),GETDATE()) + 
				ISNULL(TIA.CantidadCompras,0) - ISNULL(TSA.CantidadVenta,0)AS CantidadSaldo,					
				A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()), '''+@FechaInicioLiteral +''')
				- DBO.ObtenerMontoTotalValorado('+ @NumeroAgenciaLiteral + ', A.CodigoProducto, DATEADD(YEAR, -50,GETDATE()),GETDATE()) + 				
				ISNULL(TIA.PrecioTotalCompras,0) - ISNULL(TSA.PrecioTotalVentas,0) AS PrecioTotalSaldo
			FROM InventariosProductos A
			LEFT JOIN
			(
				SELECT	INGRESOS.NumeroAlmacen, INGRESOS.CodigoProducto, 
					ISNULL(SUM(INGRESOS.CantidadCompras),0) AS CantidadCompras, 
					ISNULL( SUM(INGRESOS.PrecioTotalCompras), 0) AS PrecioTotalCompras
				FROM
				(
					SELECT IADE.NumeroAlmacen, IADE.CodigoProducto, IADE.CantidadEntregada AS CantidadCompras, IAD.PrecioUnitarioCompra * IADE.CantidadEntregada AS PrecioTotalCompras
					FROM ComprasProductosDetalleEntrega IADE
					INNER JOIN ComprasProductosDetalle IAD
					ON IADE.CodigoProducto = IAD.CodigoProducto
					AND IADE.NumeroCompraProducto = IAD.NumeroCompraProducto
					AND IADE.NumeroAlmacen = IAD.NumeroAlmacen
					INNER JOIN ComprasProductos IA
					ON IADE.NumeroAlmacen = IA.NumeroAlmacen
					AND IADE.NumeroCompraProducto = IA.NumeroCompraProducto
					WHERE IA.CodigoEstadoCompra IN (''F'',''X'',''D'')
					AND IADE.FechaHoraEntrega 
					BETWEEN  '''+@FechaInicioLiteral+''' AND '''+@FechaFinLiteral +'''	
					AND IADE.CodigoProducto IN (' + @ListadoProductos + ')
					AND IAD.NumeroAlmacen = '+ @NumeroAgenciaLiteral + '
					UNION ALL
					SELECT VPD.NumeroAlmacen, VPDD.CodigoProducto, VPDD.CantidadVentaDevolucion, VPDD.CantidadVentaDevolucion * VPDD.PrecioUnitarioDevolucion
					FROM VentasProductosDevoluciones VPD
					INNER JOIN VentasProductosDevolucionesDetalle VPDD
					ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
					AND VPD.NumeroVentaProductoDevolucion = VPDD.NumeroVentaProductoDevolucion
					WHERE VPD.CodigoEstadoVentaDevolucion = ''F''
					AND VPD.FechaHoraDevolucion
					BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +''' 	
					AND VPDD.CodigoProducto IN (' + @ListadoProductos + ')	
					AND VPD.NumeroAlmacenDevolucion = '+ @NumeroAgenciaLiteral + '
					UNION ALL
					SELECT VPD.NumeroAlmacenRecepctor, VPDD.CodigoProducto, VPDD.CantidadTransferencia, VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
					FROM dbo.TransferenciasProductos VPD
					INNER JOIN TransferenciasProductosDetalle VPDD
					ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
					AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
					AND VPD.NumeroAlmacenRecepctor = '+ @NumeroAgenciaLiteral + '
					AND VPD.CodigoEstadoTransferencia = ''F''
					AND VPDD.CodigoProducto IN (' + @ListadoProductos + ')	
					AND VPD.FechaHoraTransferencia
					BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +''' 	
				)INGRESOS
				GROUP BY INGRESOS.NumeroAlmacen, INGRESOS.CodigoProducto
			) TIA
			ON A.CodigoProducto = TIA.CodigoProducto	
			AND A.NumeroAlmacen = TIA.NumeroAlmacen
			LEFT JOIN
			(
				SELECT EGRESOS.NumeroAlmacen, EGRESOS.CodigoProducto,
				   ISNULL(SUM(EGRESOS.CantidadVenta),0) AS CantidadVenta,
				   ISNULL(SUM(EGRESOS.PrecioTotalVentas),0) AS PrecioTotalVentas
				FROM
				(
					SELECT SADE.NumeroAlmacen, SADE.CodigoProducto, ISNULL(SUM(SADE.CantidadEntregada),0) AS CantidadVenta, 
							CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(SADE.codigoProducto)
							WHEN ''P'' THEN ISNULL(SUM(SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada),0)
							WHEN ''U'' THEN ISNULL(SUM(SADE.PrecioUnitarioCompraInventario * SADE.CantidadEntregada),0)
							ELSE ISNULL(SUM(SAD.PrecioUnitarioVenta * SADE.CantidadEntregada),0) END AS PrecioTotalVentas					
					FROM VentasProductosDetalleEntrega SADE
					INNER JOIN VentasProductosDetalle SAD
					ON SADE.NumeroAlmacen = SAD.NumeroAlmacen
					AND SADE.NumeroVentaProducto = SAD.NumeroVentaProducto
					AND SADE.CodigoProducto = SAD.CodigoProducto
					INNER JOIN VentasProductos SA
					ON SADE.NumeroAlmacen = SA.NumeroAlmacen
					AND SADE.NumeroVentaProducto = SA.NumeroVentaProducto
					WHERE SA.CodigoEstadoVenta IN (''F'',''X'',''E'')
					AND SADE.FechaHoraEntrega 
					BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +''' 
					AND SADE.CodigoProducto IN (' + @ListadoProductos + ')
					AND SAD.NumeroAlmacen = '+ @NumeroAgenciaLiteral + '
					GROUP BY SADE.NumeroAlmacen,SADE.CodigoProducto
					UNION ALL
					SELECT VPD.NumeroAlmacen, VPDD.CodigoProducto, VPDD.CantidadCompraDevolucion, VPDD.CantidadCompraDevolucion * VPDD.PrecioUnitarioDevolucion
					FROM ComprasProductosDevoluciones VPD
					INNER JOIN ComprasProductosDevolucionesDetalle VPDD
					ON VPD.NumeroAlmacen = VPDD.NumeroAlmacenDevolucion
					AND VPD.NumeroCompraProductoDevolucion = VPDD.NumeroCompraProductoDevolucion
					WHERE VPD.CodigoEstadoCompraDevolucion = ''F''
					AND VPD.FechaHoraDevolucion
					BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +'''
					AND VPDD.CodigoProducto IN (' + @ListadoProductos + ')
					AND SAD.NumeroAlmacenDevolucion = '+ @NumeroAgenciaLiteral + '
					UNION ALL 
					SELECT VPD.NumeroAlmacenEmisor, VPDD.CodigoProducto, VPDD.CantidadTransferencia, VPDD.CantidadTransferencia * VPDD.PrecioUnitarioTransferencia
					FROM dbo.TransferenciasProductos VPD
					INNER JOIN TransferenciasProductosDetalle VPDD
					ON VPD.NumeroAlmacenEmisor = VPDD.NumeroAlmacenEmisor
					AND VPD.NumeroTransferenciaProducto = VPDD.NumeroTransferenciaProducto			
					AND VPD.NumeroAlmacenEmisor = '+ @NumeroAgenciaLiteral + '
					AND VPD.CodigoEstadoTransferencia = ''F''	
					AND VPDD.CodigoProducto IN (' + @ListadoProductos + ')
					AND VPD.FechaHoraTransferencia
					BETWEEN  '''+ @FechaInicioLiteral+''' AND '''+@FechaFinLiteral +'''
				)EGRESOS
				GROUP BY EGRESOS.NumeroAlmacen, EGRESOS.CodigoProducto
			) TSA
			ON A.CodigoProducto = TSA.CodigoProducto
			AND A.NumeroAlmacen = TSA.NumeroAlmacen
			WHERE A.CodigoProducto IN  (' + @ListadoProductos + ')
			A.NumeroAlmacen = '+ @NumeroAgenciaLiteral + '
		')
	END
	
END
GO

--DECLARE @Fecha DATETIME = GETDATE()
--EXEC ListarMovimientoProductoReporte 1,'01/01/2000','31/12/2012',NULL

--DECLARE @Fecha DATETIME = GETDATE()
--EXEC ListarMovimientoProductoReporte 1,@Fecha,@Fecha ,'''39500-0038'', ''ASDF'''
