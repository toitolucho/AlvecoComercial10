USE AlvecoComercial10
GO


DROP PROCEDURE ListarProductosRecepcionFaltantes
GO

CREATE PROCEDURE ListarProductosRecepcionFaltantes
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT
AS
BEGIN
	SELECT	IAD.CodigoProducto, dbo.ObtenerNombreProducto(IAD.CodigoProducto) AS NombreProducto,
			IAD.CantidadCompra, ISNULL(IACR.CantidadRecepcionada,0) AS CantidadRecepcionada, 
			(IAD.CantidadCompra - ISNULL(IACR.CantidadRecepcionada,0)) AS CantidadFaltante,
			CAST(0 AS BIT) as EsProductoEspecifico,
			IAD.PrecioUnitarioCompra
	FROM ComprasProductosDetalle IAD
	LEFT JOIN
	(
		SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProducto, SUM(CantidadEntregada) AS CantidadRecepcionada
		FROM ComprasProductosDetalleEntrega
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroCompraProducto = @NumeroCompraProducto
		GROUP BY NumeroAlmacen, NumeroCompraProducto, CodigoProducto
	) IACR
	ON IAD.NumeroAlmacen = IACR.NumeroAlmacen
	AND IAD.NumeroCompraProducto = IACR.NumeroCompraProducto
	AND IAD.CodigoProducto = IACR.CodigoProducto
	WHERE IAD.CantidadCompra <> ISNULL(IACR.CantidadRecepcionada,0)
	AND IAD.NumeroAlmacen = @NumeroAlmacen
	AND IAD.NumeroCompraProducto = @NumeroCompraProducto
END
GO

