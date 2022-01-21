USE AlvecoComercial10
GO

DROP PROCEDURE ListarVentasProductosReportesPorFechasTipo
GO

CREATE PROCEDURE ListarVentasProductosReportesPorFechasTipo
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@OrdenacionPorFact	BIT
AS
BEGIN

	SET @FechaHoraInicio = DBO.FormatearFechaInicioFin(@FechaHoraInicio,1)
	SET @FechaHoraFin = DBO.FormatearFechaInicioFin(@FechaHoraFin,0)

	SELECT  TAPrincipal.NumeroAlmacen, TAPrincipal.TipoFacturacion, TAPrincipal.CodigoProducto, 
			PT.NombreProductoTipo as NombreTipoProducto, P.NombreProducto,
			TAPrincipal.CantidadRecepcionadaContado, TAPrincipal.ImporteTotalContado,
			TAPrincipal.CantidadRecepcionadaCredito, TAPrincipal.ImporteTotalCredito,
			TAPrincipal.CantidadRecepcionadaContado + TAPrincipal.CantidadRecepcionadaCredito AS CantidadTotal,
			TAPrincipal.ImporteTotalContado + TAPrincipal.ImporteTotalCredito AS ImporteTotal
	FROM
	(	
		SELECT	TA1.NumeroAlmacen, CASE WHEN TA1.CodigoEstadoFactura = 'F' THEN 'CON FACTURA' ELSE 'SIN FACTURA' END AS TipoFacturacion,
				TA1.CodigoProducto, TA1.CantidadRecepcionada AS CantidadRecepcionadaContado,
				TA1.PrecioTotal AS ImporteTotalContado, 
				CASE WHEN TA2.CantidadRecepcionada IS NULL THEN 0 ELSE TA2.CantidadRecepcionada END AS CantidadRecepcionadaCredito,
				CASE WHEN TA2.PrecioTotal IS NULL THEN 0 ELSE TA2.PrecioTotal END AS ImporteTotalCredito
		
		FROM	
		(
			SELECT	CP.NumeroAlmacen, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioVenta * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM VentasProductos CP
			INNER JOIN VentasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroVentaProducto = CPD.NumeroVentaProducto			
			WHERE CP.CodigoTipoVenta = 'E'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraVenta
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen,  CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA1
		LEFT JOIN
		(
			SELECT	CP.NumeroAlmacen, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioVenta * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM VentasProductos CP
			INNER JOIN VentasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroVentaProducto = CPD.NumeroVentaProducto			
			WHERE CP.CodigoTipoVenta = 'R'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraVenta
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen,  CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA2
		ON TA1.NumeroAlmacen = TA2.NumeroAlmacen
		AND TA1.CodigoEstadoFactura = TA2.CodigoEstadoFactura
		AND TA1.CodigoProducto = TA2.CodigoProducto
		
		UNION 
		
		
		
		SELECT	TA1.NumeroAlmacen, CASE WHEN TA1.CodigoEstadoFactura = 'F' THEN 'CON FACTURA' ELSE 'SIN FACTURA' END AS TipoFacturacion,
				TA1.CodigoProducto, 
				CASE WHEN TA2.CantidadRecepcionada IS NULL THEN 0 ELSE TA2.CantidadRecepcionada END AS CantidadRecepcionadaContado,
				CASE WHEN TA2.PrecioTotal IS NULL THEN 0 ELSE TA2.PrecioTotal END AS ImporteTotalContado,
				TA1.CantidadRecepcionada AS CantidadRecepcionadaCredito,
				TA1.PrecioTotal AS ImporteTotalCredito			
		
		FROM	
		(
			SELECT	CP.NumeroAlmacen, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioVenta * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM VentasProductos CP
			INNER JOIN VentasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroVentaProducto = CPD.NumeroVentaProducto			
			WHERE CP.CodigoTipoVenta = 'R'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraVenta
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen,  CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA1
		LEFT JOIN
		(
			SELECT	CP.NumeroAlmacen, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioVenta * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM VentasProductos CP
			INNER JOIN VentasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroVentaProducto = CPD.NumeroVentaProducto			
			WHERE CP.CodigoTipoVenta = 'E'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraVenta
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen,  CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA2
		ON TA1.NumeroAlmacen = TA2.NumeroAlmacen
		AND TA1.CodigoEstadoFactura = TA2.CodigoEstadoFactura
		AND TA1.CodigoProducto = TA2.CodigoProducto
		
		
	)TAPrincipal
	INNER JOIN Productos P
	ON TAPrincipal.CodigoProducto = P.CodigoProducto
	INNER JOIN ProductosTipos PT
	ON PT.CodigoProductoTipo = P.CodigoProductoTipo
	ORDER BY CASE WHEN @OrdenacionPorFact = 1 THEN TipoFacturacion ELSE NombreProductoTipo END ASC,
	CASE WHEN @OrdenacionPorFact = 1 THEN NombreProductoTipo ELSE TipoFacturacion END ASC, 5	
END
GO


--EXEC ListarVentasProductosReportesPorFechasTipo 1, '01/01/2000','31/12/2012', 0
