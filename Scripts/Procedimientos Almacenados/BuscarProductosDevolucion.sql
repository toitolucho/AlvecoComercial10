USE AlvecoComercial10
GO

DROP PROCEDURE BuscarProductosDevolucion
GO

CREATE PROCEDURE BuscarProductosDevolucion
	@NumeroAlmacenDevolucion	INT,
	@TextoBusqueda				VARCHAR(250),
	@TipoTransaccionDev			CHAR(1),
	@NumeroTransaccionDev		INT
AS
BEGIN
	IF(@TipoTransaccionDev = 'C')
	BEGIN
		SELECT P.CodigoProducto, P.NombreProducto, PU.NombreUnidad, PM.NombreMarca, CPD.PrecioUnitarioCompra AS PrecioUnitarioTransaccion, ISNULL(TADEV.CantidadDevuelta,0) AS CantidadDevolucion , ISNULL(SUM(CPDE.CantidadEntregada),0) AS CantidadEntregada, CPD.TiempoGarantiaCompra  as TiempoGarantia
		FROM ComprasProductosDetalleEntrega CPDE
		INNER JOIN Productos P
		ON CPDE.CodigoProducto = P.CodigoProducto		
		INNER JOIN ProductosUnidades PU
		ON P.CodigoUnidadProducto = PU.CodigoUnidad
		INNER JOIN ProductosMarcas PM
		ON P.CodigoMarcaProducto = PM.CodigoMarca
		INNER JOIN ComprasProductosDetalle CPD
		ON CPD.NumeroAlmacen = CPDE.NumeroAlmacen
		AND CPD.NumeroCompraProducto = CPDE.NumeroCompraProducto
		AND CPD.CodigoProducto = CPDE.CodigoProducto
		LEFT JOIN
		(
			SELECT CPDD.CodigoProducto, SUM(CPDD.CantidadCompraDevolucion) AS CantidadDevuelta
			FROM ComprasProductosDevoluciones CPD
			INNER JOIN ComprasProductosDevolucionesDetalle CPDD
			ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
			AND CPD.NumeroCompraProductoDevolucion = CPDD.NumeroCompraProductoDevolucion			
			WHERE CPD.CodigoEstadoCompraDevolucion IN ('F','X')
			AND NumeroCompraProducto = @NumeroTransaccionDev
			GROUP BY CPDD.CodigoProducto
		) TADEV
		ON TADEV.CodigoProducto = CPDE.CodigoProducto		
		WHERE CPDE.NumeroAlmacen = @NumeroAlmacenDevolucion
		AND CPDE.NumeroCompraProducto = @NumeroTransaccionDev		
		AND( P.NombreProducto LIKE '%'+ @TextoBusqueda + '%'
		OR P.CodigoProducto LIKE '%'+ @TextoBusqueda + '%'
		)		
		GROUP BY P.CodigoProducto, P.NombreProducto, PU.NombreUnidad, PM.NombreMarca, CPD.PrecioUnitarioCompra, TADEV.CantidadDevuelta, CPD.TiempoGarantiaCompra
		HAVING SUM(CPDE.CantidadEntregada) > ISNULL(TADEV.CantidadDevuelta,0) 
	END
	ELSE
	BEGIN
		SELECT P.CodigoProducto, P.NombreProducto, PU.NombreUnidad, PM.NombreMarca, CPD.PrecioUnitarioVenta AS PrecioUnitarioTransaccion, ISNULL(TADEV.CantidadDevuelta,0) AS CantidadDevolucion, ISNULL(SUM(CPDE.CantidadEntregada),0) AS CantidadEntregada, cpd.TiempoGarantiaVenta as TiempoGarantia
		FROM VentasProductosDetalleEntrega CPDE
		INNER JOIN Productos P
		ON CPDE.CodigoProducto = P.CodigoProducto		
		INNER JOIN ProductosUnidades PU
		ON P.CodigoUnidadProducto = PU.CodigoUnidad
		INNER JOIN ProductosMarcas PM
		ON P.CodigoMarcaProducto = PM.CodigoMarca
		INNER JOIN VentasProductosDetalle CPD
		ON CPD.NumeroAlmacen = CPDE.NumeroAlmacen
		AND CPD.NumeroVentaProducto = CPDE.NumeroVentaProducto
		AND CPD.CodigoProducto = CPDE.CodigoProducto
		LEFT JOIN
		(
			SELECT CPDD.CodigoProducto, SUM(CPDD.CantidadVentaDevolucion) AS CantidadDevuelta
			FROM VentasProductosDevoluciones CPD
			INNER JOIN VentasProductosDevolucionesDetalle CPDD
			ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
			AND CPD.NumeroVentaProductoDevolucion = CPDD.NumeroVentaProductoDevolucion			
			WHERE CPD.CodigoEstadoVentaDevolucion IN ('F','X')
			AND NumeroVentaProducto = @NumeroTransaccionDev
			GROUP BY CPDD.CodigoProducto
		) TADEV
		ON TADEV.CodigoProducto = CPDE.CodigoProducto		
		WHERE CPDE.NumeroAlmacen = @NumeroAlmacenDevolucion
		AND CPDE.NumeroVentaProducto = @NumeroTransaccionDev
		AND( P.NombreProducto LIKE '%'+ @TextoBusqueda + '%'
		OR P.CodigoProducto LIKE '%'+ @TextoBusqueda + '%'
		)		
		GROUP BY P.CodigoProducto, P.NombreProducto, PU.NombreUnidad, PM.NombreMarca, CPD.PrecioUnitarioVenta, TADEV.CantidadDevuelta, cpd.TiempoGarantiaVenta 
		HAVING SUM(CPDE.CantidadEntregada) > ISNULL(TADEV.CantidadDevuelta,0) 
	END
END
GO

--exec BuscarProductosDevolucion 1, 'a', 'V', 6

--select * from VentasProductosDetalle
--select * from VentasProductosDetalleEntrega
--SELECT * FROM VentasProductosDevoluciones