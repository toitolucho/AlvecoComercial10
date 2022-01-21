USE AlvecoComercial10
GO

DROP PROCEDURE ListarComprasProductosUsuarios
GO

CREATE PROCEDURE ListarComprasProductosUsuarios
	@NumeroAlmacen		INT,	
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@DIUsuario			CHAR(10)
AS
BEGIN
	
	SET @FechaHoraInicio = ISNULL(DBO.FormatearFechaInicioFin(@FechaHoraInicio,1),'01/01/2000')
	SET @FechaHoraFin = ISNULL(DBO.FormatearFechaInicioFin(@FechaHoraFin,0), GETDATE())


	SELECT	VP.NumeroCompraProducto, VP.CodigoCompraProducto, VP.NumeroAlmacen,
			dbo.FormatearFechaInicioFin(VP.FechaHoraRegistro, 1) as FechaHoraCompra,
			DBO.ObtenerNombreCompleto(VP.DIUsuario) AS NombreUsuario, C.NombreRazonSocial, 
			VPDE.CodigoProducto, DBO.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto, 
			VPDE.CantidadEntregada, VPDE.PrecioUnitarioCompra, 		
			VPDE.CantidadEntregada * VPDE.PrecioUnitarioCompra as PrecioTotal
	FROM ComprasProductos VP
	INNER JOIN Proveedores C
	ON VP.CodigoProveedor = C.CodigoProveedor
	INNER JOIN VListarComprasProductosDetalleEntrega VPDE
	ON VP.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VP.NumeroCompraProducto = VPDE.NumeroCompraProducto
	WHERE VP.FechaHoraRegistro
	BETWEEN @FechaHoraInicio AND @FechaHoraFin
	AND CAST(VP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
	ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
	AND VP.DIUsuario LIKE CASE WHEN @DIUsuario IS NULL THEN '%%' ELSE @DIUsuario END	
	
	ORDER BY CASE WHEN @DIUsuario IS NOT NULL THEN VP.DIUsuario ELSE 3 END,
	CASE WHEN @DIUsuario IS NOT NULL THEN 3 ELSE VP.DIUsuario END
END
GO

--exec dbo.ListarComprasProductosUsuarios null, '01/01/2012','01/12/2012', null
