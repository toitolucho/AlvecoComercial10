USE AlvecoComercial10
GO

DROP FUNCTION EsPosibleCulminarDevolucion
GO

CREATE FUNCTION EsPosibleCulminarDevolucion
(
	@NumeroAlmacenDevolucion	INT,
	@NumeroTransaccionDev		INT,
	@TipoTransaccionDev			CHAR(1)	
)
RETURNS	VARCHAR(250)
AS
BEGIN
	DECLARE @NumeroTransaccion	INT,
			@NombreProducto		VARCHAR(250)
	
	IF(@TipoTransaccionDev = 'C')
	BEGIN
		SELECT @NumeroTransaccion = NumeroCompraProducto
		FROM ComprasProductosDevoluciones
		WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND NumeroCompraProductoDevolucion = @NumeroTransaccionDev
		
		
		SELECT TOP(1) @NombreProducto = DBO.ObtenerNombreProducto(CPDD.CodigoProducto) 
		FROM ComprasProductosDevolucionesDetalle CPDD		
		LEFT JOIN
		(
			SELECT CodigoProducto, SUM(CantidadExistente) AS CantidadExistente
			FROM
			(
				SELECT CodigoProducto, CantidadEntregada AS CantidadExistente
				FROM ComprasProductosDetalleEntrega
				WHERE NumeroAlmacen = @NumeroAlmacenDevolucion
				AND NumeroCompraProducto = @NumeroTransaccion
				UNION ALL
				SELECT CPDD.CodigoProducto, -CPDD.CantidadCompraDevolucion
				FROM ComprasProductosDevolucionesDetalle CPDD
				INNER JOIN ComprasProductosDevoluciones CPD
				ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
				AND CPD.NumeroCompraProductoDevolucion  = CPDD.NumeroCompraProductoDevolucion
				WHERE CPDD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
				--AND CPDD.NumeroCompraProductoDevolucion = @NumeroTransaccionDev
				AND CPD.CodigoEstadoCompraDevolucion IN ('F','X')
				AND CPD.NumeroCompraProductoDevolucion <> @NumeroTransaccionDev
			)TAUX
			GROUP BY TAUX.CodigoProducto
		) TA
		ON CPDD.CodigoProducto = TA.CodigoProducto
		WHERE CPDD.NumeroCompraProductoDevolucion = @NumeroTransaccionDev
		AND CPDD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND CPDD.CantidadCompraDevolucion > TA.CantidadExistente
		
	END
	ELSE
	BEGIN
		SELECT @NumeroTransaccion = NumeroVentaProducto
		FROM VentasProductosDevoluciones
		WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND NumeroVentaProductoDevolucion = @NumeroTransaccionDev
		
		
		SELECT TOP(1) @NombreProducto = DBO.ObtenerNombreProducto(CPDD.CodigoProducto) 
		FROM VentasProductosDevolucionesDetalle CPDD		
		LEFT JOIN
		(
			SELECT CodigoProducto, SUM(CantidadExistente) AS CantidadExistente
			FROM
			(
				SELECT CodigoProducto, CantidadEntregada AS CantidadExistente
				FROM VentasProductosDetalleEntrega
				WHERE NumeroAlmacen = @NumeroAlmacenDevolucion
				AND NumeroVentaProducto = @NumeroTransaccion
				UNION ALL
				SELECT CPDD.CodigoProducto, -CPDD.CantidadVentaDevolucion
				FROM VentasProductosDevolucionesDetalle CPDD
				INNER JOIN VentasProductosDevoluciones CPD
				ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
				AND CPD.NumeroVentaProductoDevolucion  = CPDD.NumeroVentaProductoDevolucion
				WHERE CPDD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
				--AND CPDD.NumeroVentaProductoDevolucion = @NumeroTransaccionDev
				AND CPD.CodigoEstadoVentaDevolucion IN ('F','X')
				AND CPD.NumeroVentaProductoDevolucion <> @NumeroTransaccionDev
			)TAUX
			GROUP BY TAUX.CodigoProducto
		) TA
		ON CPDD.CodigoProducto = TA.CodigoProducto
		WHERE CPDD.NumeroVentaProductoDevolucion = @NumeroTransaccionDev
		AND CPDD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND CPDD.CantidadVentaDevolucion > TA.CantidadExistente 
	END
	
	RETURN @NombreProducto
END
GO
