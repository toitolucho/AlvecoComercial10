USE AlvecoComercial10
GO

DROP PROCEDURE ListarComprasProductosReportesPorFechasProveedor
GO

CREATE PROCEDURE ListarComprasProductosReportesPorFechasProveedor
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@OrdenacionPorProve	BIT
AS
BEGIN
	
	SET @FechaHoraInicio = DBO.FormatearFechaInicioFin(@FechaHoraInicio,1)
	SET @FechaHoraFin = DBO.FormatearFechaInicioFin(@FechaHoraFin,0)
	
	SELECT TAPrincipal.NumeroAlmacen, TAPrincipal.NombreRazonSocial, TAPrincipal.TipoFacturacion, PT.NombreProductoTipo as NombreTipoProducto, 
			TAPrincipal.CodigoProducto, P.NombreProducto,
			TAPrincipal.CantidadRecepcionadaContado, TAPrincipal.ImporteTotalContado,
			TAPrincipal.CantidadRecepcionadaCredito, TAPrincipal.ImporteTotalCredito,
			TAPrincipal.CantidadRecepcionadaContado + TAPrincipal.CantidadRecepcionadaCredito AS CantidadTotal,
			TAPrincipal.ImporteTotalContado + TAPrincipal.ImporteTotalCredito AS ImporteTotal
	FROM
	(	
		
		SELECT	TA1.NumeroAlmacen, P.NombreRazonSocial, CASE WHEN TA1.CodigoEstadoFactura = 'F' THEN 'CON FACTURA' ELSE 'SIN FACTURA' END AS TipoFacturacion,
				TA1.CodigoProducto, TA1.CantidadRecepcionada AS CantidadRecepcionadaContado,
				TA1.PrecioTotal AS ImporteTotalContado, 
				CASE WHEN TA2.CantidadRecepcionada IS NULL THEN 0 ELSE TA2.CantidadRecepcionada END AS CantidadRecepcionadaCredito,
				CASE WHEN TA2.PrecioTotal IS NULL THEN 0 ELSE TA2.PrecioTotal END AS ImporteTotalCredito
		
		FROM	
		(
			SELECT	CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioCompra * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM ComprasProductos CP
			INNER JOIN ComprasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto			
			WHERE CP.CodigoTipoCompra = 'E'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraRegistro
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA1
		LEFT JOIN
		(
			SELECT	CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioCompra * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM ComprasProductos CP
			INNER JOIN ComprasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto			
			WHERE CP.CodigoTipoCompra = 'R'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraRegistro
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA2
		ON TA1.NumeroAlmacen = TA2.NumeroAlmacen
		AND TA1.CodigoEstadoFactura = TA2.CodigoEstadoFactura
		AND TA1.CodigoProducto = TA2.CodigoProducto
		INNER JOIN Proveedores P
		ON TA1.CodigoProveedor = P.CodigoProveedor
		UNION 
		
		
		
		SELECT	TA1.NumeroAlmacen, P.NombreRazonSocial, CASE WHEN TA1.CodigoEstadoFactura = 'F' THEN 'CON FACTURA' ELSE 'SIN FACTURA' END AS TipoFacturacion,
				TA1.CodigoProducto, 
				CASE WHEN TA2.CantidadRecepcionada IS NULL THEN 0 ELSE TA2.CantidadRecepcionada END AS CantidadRecepcionadaContado,
				CASE WHEN TA2.PrecioTotal IS NULL THEN 0 ELSE TA2.PrecioTotal END AS ImporteTotalContado,
				TA1.CantidadRecepcionada AS CantidadRecepcionadaCredito,
				TA1.PrecioTotal AS ImporteTotalCredito			
		
		FROM	
		(
			SELECT	CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioCompra * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM ComprasProductos CP
			INNER JOIN ComprasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto			
			WHERE CP.CodigoTipoCompra = 'R'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraRegistro
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA1
		LEFT JOIN
		(
			SELECT	CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END AS CodigoEstadoFactura , 
					CPD.CodigoProducto, SUM(CPD.PrecioUnitarioCompra * CPD.CantidadEntregada) AS PrecioTotal, 
					SUM(CPD.CantidadEntregada) AS CantidadRecepcionada
			FROM ComprasProductos CP
			INNER JOIN ComprasProductosDetalle CPD
			ON CP.NumeroAlmacen = CPD.NumeroAlmacen
			AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto			
			WHERE CP.CodigoTipoCompra = 'E'
			AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
			ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
			AND CP.FechaHoraRegistro
			BETWEEN @FechaHoraInicio AND @FechaHoraFin
			GROUP BY CP.NumeroAlmacen, CP.CodigoProveedor, CASE WHEN LEN(RTRIM(CP.NumeroFactura)) > 0 THEN 'F' ELSE 'S' END, CPD.CodigoProducto
		) TA2
		ON TA1.NumeroAlmacen = TA2.NumeroAlmacen
		AND TA1.CodigoEstadoFactura = TA2.CodigoEstadoFactura
		AND TA1.CodigoProducto = TA2.CodigoProducto
		INNER JOIN Proveedores P
		ON TA1.CodigoProveedor = P.CodigoProveedor
		
		
	)TAPrincipal
	INNER JOIN Productos P
	ON TAPrincipal.CodigoProducto = P.CodigoProducto
	INNER JOIN ProductosTipos PT
	ON PT.CodigoProductoTipo = P.CodigoProductoTipo
	ORDER BY CASE WHEN @OrdenacionPorProve = 1 THEN NombreRazonSocial ELSE NombreProductoTipo END ASC,
	CASE WHEN @OrdenacionPorProve = 1 THEN NombreProductoTipo ELSE NombreRazonSocial END ASC, 5	
END
GO


--EXEC ListarComprasProductosReportesPorFechasProveedor 1, '01/01/2000','31/12/2012', 1