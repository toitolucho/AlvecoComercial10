USE AlvecoComercial10
GO

DROP PROCEDURE ListarVentasProductosPersonasDistribucion
GO

CREATE PROCEDURE ListarVentasProductosPersonasDistribucion
	@NumeroAlmacen		INT,	
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@DIPersona			CHAR(15),
	@CodigoCliente		INT
AS
BEGIN
	
	SET @FechaHoraInicio = ISNULL(DBO.FormatearFechaInicioFin(@FechaHoraInicio,1),'01/01/2000')
	SET @FechaHoraFin = ISNULL(DBO.FormatearFechaInicioFin(@FechaHoraFin,0), GETDATE())


	SELECT	DBO.ObtenerNombreCompleto(P.DIPersona) AS NombrePersonaDistribuidor, C.NombreCliente, 
			VPDE.CodigoProducto, DBO.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto, 
			SUM(VPDE.CantidadEntregada) AS CantidadEntregada, 
			SUM(VPDE.CantidadEntregada * VPDE.PrecioUnitarioVenta) AS PrecioTotal			
	FROM VentasProductos VP
	INNER JOIN Personas P
	ON VP.DIPersonaDistribuidor = P.DIPersona
	INNER JOIN Clientes C
	ON VP.CodigoCliente = C.CodigoCliente
	INNER JOIN VListarVentasProductosDetalleEntrega VPDE
	ON VP.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VP.NumeroVentaProducto = VPDE.NumeroVentaProducto
	WHERE VP.FechaHoraVenta
	BETWEEN @FechaHoraInicio AND @FechaHoraFin
	AND CAST(VP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
	ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
	AND VP.DIPersonaDistribuidor LIKE CASE WHEN @DIPersona IS NULL THEN '%%' ELSE @DIPersona END
	AND CAST(VP.CodigoCliente AS VARCHAR(100)) LIKE CASE WHEN @CodigoCliente IS NULL THEN '%%' ELSE CAST(@CodigoCliente AS VARCHAR(100)) END
	GROUP BY P.DIPersona, C.NombreCliente, VPDE.CodigoProducto
	ORDER BY CASE WHEN @DIPersona IS NOT NULL THEN P.DIPersona ELSE C.NombreCliente END,
	CASE WHEN @CodigoCliente IS NOT NULL THEN C.NombreCliente ELSE P.DIPersona END
END
GO