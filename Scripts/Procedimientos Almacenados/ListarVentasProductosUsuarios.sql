USE AlvecoComercial10
GO

DROP PROCEDURE ListarVentasProductosUsuarios
GO

CREATE PROCEDURE ListarVentasProductosUsuarios
	@NumeroAlmacen		INT,	
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@DIUsuario			CHAR(10)
AS
BEGIN
	
	SET @FechaHoraInicio = ISNULL(DBO.FormatearFechaInicioFin(@FechaHoraInicio,1),'01/01/2000')
	SET @FechaHoraFin = ISNULL(DBO.FormatearFechaInicioFin(@FechaHoraFin,0), GETDATE())


	SELECT	VP.NumeroVentaProducto, VP.CodigoVentaProducto, VP.NumeroAlmacen,
			dbo.FormatearFechaInicioFin(VP.FechaHoraVenta, 1) as FechaHoraVenta,
			DBO.ObtenerNombreCompleto(VP.DIUsuario) AS NombreUsuario, C.NombreCliente, 
			VPDE.CodigoProducto, DBO.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto, 
			VPDE.CantidadEntregada, VPDE.PrecioUnitarioVenta, 		
			VPDE.CantidadEntregada * VPDE.PrecioUnitarioVenta as PrecioTotal
	FROM VentasProductos VP
	INNER JOIN Clientes C
	ON VP.CodigoCliente = C.CodigoCliente
	INNER JOIN VListarVentasProductosDetalleEntrega VPDE
	ON VP.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VP.NumeroVentaProducto = VPDE.NumeroVentaProducto
	WHERE VP.FechaHoraVenta
	BETWEEN @FechaHoraInicio AND @FechaHoraFin
	AND CAST(VP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
	ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
	AND VP.DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END	
	
	ORDER BY CASE WHEN @DIUsuario IS NOT NULL THEN VP.DIUsuario ELSE 3 END,
	CASE WHEN @DIUsuario IS NOT NULL THEN 3 ELSE VP.DIUsuario END
END
GO


--exec dbo.ListarVentasProductosUsuarios 1, '01/01/2000',NULL, NULL