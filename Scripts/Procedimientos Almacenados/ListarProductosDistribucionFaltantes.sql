USE AlvecoComercial10
GO


DROP PROCEDURE ListarProductosDistribucionFaltantes
GO

CREATE PROCEDURE ListarProductosDistribucionFaltantes
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT
AS
BEGIN
	SELECT	IAD.CodigoProducto, dbo.ObtenerNombreProducto(IAD.CodigoProducto) AS NombreProducto,
			IAD.CantidadVenta, ISNULL(IACR.CantidadRecepcionada,0) AS CantidadDistribuida, 
			(IAD.CantidadVenta - ISNULL(IACR.CantidadRecepcionada,0)) AS CantidadFaltante,
			CAST(0 AS BIT) as EsProductoEspecifico,
			IAD.PrecioUnitarioVenta
	FROM VentasProductosDetalle IAD
	LEFT JOIN
	(
		SELECT NumeroAlmacen, NumeroVentaProducto, CodigoProducto, SUM(CantidadEntregada) AS CantidadRecepcionada
		FROM VentasProductosDetalleEntrega
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroVentaProducto = @NumeroVentaProducto
		GROUP BY NumeroAlmacen, NumeroVentaProducto, CodigoProducto
	) IACR
	ON IAD.NumeroAlmacen = IACR.NumeroAlmacen
	AND IAD.NumeroVentaProducto = IACR.NumeroVentaProducto
	AND IAD.CodigoProducto = IACR.CodigoProducto
	WHERE IAD.CantidadVenta <> ISNULL(IACR.CantidadRecepcionada,0)
	AND IAD.NumeroAlmacen = @NumeroAlmacen
	AND IAD.NumeroVentaProducto = @NumeroVentaProducto
END
GO

--EXEC ListarProductosDistribucionFaltantes 1, 2

--select * from dbo.VentasProductos
--select * from dbo.VentasProductosdetalle
--select * from dbo.VentasProductosdetalleEntrega