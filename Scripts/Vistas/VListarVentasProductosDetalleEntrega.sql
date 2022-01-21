USE AlvecoComercial10
GO


DROP VIEW VListarVentasProductosDetalleEntrega
GO

CREATE VIEW VListarVentasProductosDetalleEntrega
AS
	SELECT	VPDE.NumeroAlmacen, VPDE.NumeroVentaProducto, VPDE.CodigoProducto, 
			SUM(VPDE.CantidadEntregada) AS CantidadEntregada, VPD.PrecioUnitarioVenta
	FROM VentasProductosDetalleEntrega VPDE
	INNER JOIN VentasProductosDetalle VPD
	ON VPDE.NumeroAlmacen = VPD.NumeroAlmacen
	AND VPDE.NumeroVentaProducto = VPD.NumeroVentaProducto
	AND VPDE.CodigoProducto = VPD.CodigoProducto
	GROUP BY VPDE.NumeroAlmacen, VPDE.NumeroVentaProducto, VPDE.CodigoProducto, VPD.PrecioUnitarioVenta
GO


	
DROP VIEW VListarComprasProductosDetalleEntrega
GO

CREATE VIEW VListarComprasProductosDetalleEntrega
AS
	SELECT	VPDE.NumeroAlmacen, VPDE.NumeroCompraProducto, VPDE.CodigoProducto, 
			SUM(VPDE.CantidadEntregada) AS CantidadEntregada, VPD.PrecioUnitarioCompra
	FROM ComprasProductosDetalleEntrega VPDE
	INNER JOIN ComprasProductosDetalle VPD
	ON VPDE.NumeroAlmacen = VPD.NumeroAlmacen
	AND VPDE.NumeroCompraProducto = VPD.NumeroCompraProducto
	AND VPDE.CodigoProducto = VPD.CodigoProducto
	GROUP BY VPDE.NumeroAlmacen, VPDE.NumeroCompraProducto, VPDE.CodigoProducto, VPD.PrecioUnitarioCompra
GO