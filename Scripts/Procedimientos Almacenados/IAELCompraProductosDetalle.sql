USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCompraProductoDetalle
GO

CREATE PROCEDURE InsertarCompraProductoDetalle
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProducto			CHAR(15),
	@CantidadEntregada		INT,
	@CantidadCompra			INT,
	@PrecioUnitarioCompra	DECIMAL(10,2),
	@TiempoGarantiaCompra	INT
AS
BEGIN
	INSERT INTO dbo.ComprasProductosDetalle (NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadCompra, CantidadEntregada, PrecioUnitarioCompra, TiempoGarantiaCompra)
	VALUES (@NumeroAlmacen, @NumeroCompraProducto, @CodigoProducto, @CantidadCompra, @CantidadEntregada, @PrecioUnitarioCompra, @TiempoGarantiaCompra)
END	
GO

DROP PROCEDURE ActualizarCompraProductoDetalle
GO

CREATE PROCEDURE ActualizarCompraProductoDetalle
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProducto			CHAR(15),
	@CantidadCompra			INT,	
	@CantidadEntregada		INT,	
	@PrecioUnitarioCompra	DECIMAL(10,2),
	@TiempoGarantiaCompra	INT
AS
BEGIN
	UPDATE 	dbo.ComprasProductosDetalle
	SET					
		NumeroAlmacen		  = @NumeroAlmacen,
		NumeroCompraProducto  = @NumeroCompraProducto,
		CodigoProducto		  = @CodigoProducto,
		CantidadCompra		  = @CantidadCompra,
		CantidadEntregada	  = @CantidadEntregada,
		PrecioUnitarioCompra  = @PrecioUnitarioCompra,
		TiempoGarantiaCompra  = @TiempoGarantiaCompra
	WHERE (NumeroAlmacen = @NumeroAlmacen) 
	AND NumeroCompraProducto = @NumeroCompraProducto
END
GO

DROP PROCEDURE EliminarCompraProductoDetalle
GO

CREATE PROCEDURE EliminarCompraProductoDetalle
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT
AS
BEGIN
	DELETE 
	FROM dbo.ComprasProductosDetalle
	WHERE (NumeroAlmacen = @NumeroAlmacen)
	AND NumeroCompraProducto = @NumeroAlmacen
END
GO

DROP PROCEDURE ListarComprasProductosDetalle
GO

CREATE PROCEDURE ListarComprasProductosDetalle
AS
BEGIN
	SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadCompra, CantidadEntregada, PrecioUnitarioCompra, TiempoGarantiaCompra
	FROM dbo.ComprasProductosDetalle
	ORDER BY NumeroAlmacen
END
GO

DROP PROCEDURE ObtenerCompraProductoDetalle
GO

CREATE PROCEDURE ObtenerCompraProductoDetalle
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT
AS
BEGIN
	SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadCompra, CantidadEntregada, PrecioUnitarioCompra, TiempoGarantiaCompra
	FROM dbo.ComprasProductosDetalle
	WHERE (NumeroAlmacen = @NumeroAlmacen)
	AND NumeroCompraProducto = @NumeroAlmacen
END
GO


DROP PROCEDURE ListarComprasProductosDetalleParaMostrar
GO

CREATE PROCEDURE ListarComprasProductosDetalleParaMostrar
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT
AS
BEGIN
	SELECT IAD.CodigoProducto, dbo.ObtenerNombreProducto(IAD.CodigoProducto) AS NombreProducto, 
	IAD.CantidadCompra, IAD.CantidadEntregada, IAD.PrecioUnitarioCompra,  IAD.CantidadCompra * IAD.PrecioUnitarioCompra AS PrecioTotal, 
	TiempoGarantiaCompra, NombreUnidad, PM.NombreMarca
	FROM dbo.ComprasProductosDetalle IAD
	LEFT JOIN dbo.VListarComprasProductosDetalleEntrega CPDE
	ON IAD.NumeroAlmacen = CPDE.NumeroAlmacen
	AND IAD.NumeroCompraProducto = CPDE.NumeroCompraProducto
	AND IAD.CodigoProducto = CPDE.CodigoProducto
	INNER JOIN Productos A
	ON A.CodigoProducto = IAD.CodigoProducto
	INNER JOIN ProductosUnidades AU
	ON A.CodigoUnidadProducto = AU.CodigoUnidad
	INNER JOIN ProductosMarcas PM
	ON A.CodigoMarcaProducto = PM.CodigoMarca
	WHERE (IAD.NumeroAlmacen = @NumeroAlmacen)
	AND IAD.NumeroCompraProducto = @NumeroCompraProducto
END
GO