USE AlvecoComercial10
GO

DROP PROCEDURE ListarProductosEnTransitoPorPedido
GO

CREATE PROCEDURE ListarProductosEnTransitoPorPedido
	@NumeroAlmacen	INT
AS
BEGIN
	SELECT	CP.NumeroAlmacen, CP.NumeroCompraProducto, CP.CodigoCompraProducto, CP.FechaHoraRegistro as Fecha, 
			P.CodigoProducto, P.NombreProducto, PM.NombreMarca as NombreMarcaProducto, PU.NombreUnidad,
			CPD.CantidadCompra AS CantidadSolicitada, ISNULL(CPDE.CantidadEntregada,0) AS CantidadRecepcionada, 
			CPD.CantidadCompra - ISNULL(CPDE.CantidadEntregada,0) AS CantidadPendiente
	FROM ComprasProductos CP
	INNER JOIN ComprasProductosDetalle CPD
	ON CP.NumeroAlmacen = CPD.NumeroAlmacen
	AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto
	LEFT JOIN VListarComprasProductosDetalleEntrega CPDE
	ON CPD.NumeroCompraProducto =  CPDE.NumeroCompraProducto
	AND CPD.NumeroAlmacen  = CPDE.NumeroAlmacen
	AND CPD.CodigoProducto = CPDE.CodigoProducto
	INNER JOIN Productos P
	ON P.CodigoProducto = CPD.CodigoProducto
	INNER JOIN ProductosMarcas PM
	ON P.CodigoMarcaProducto = PM.CodigoMarca
	INNER JOIN ProductosUnidades PU
	ON PU.CodigoUnidad = P.CodigoUnidadProducto
	WHERE CP.CodigoEstadoCompra IN ('I','D')
	AND CPD.CantidadCompra <> ISNULL(CPDE.CantidadEntregada,0)	
	AND CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
	ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
END
GO
