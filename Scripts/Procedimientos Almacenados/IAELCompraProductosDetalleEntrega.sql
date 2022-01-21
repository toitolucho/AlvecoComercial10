USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCompraProductoDetalleEntrega
GO

CREATE PROCEDURE InsertarCompraProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProducto			CHAR(15),
	@CantidadEntregada		INT,
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	INSERT INTO dbo.ComprasProductosDetalleEntrega (NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadEntregada, FechaHoraEntrega)
	VALUES (@NumeroAlmacen, @NumeroCompraProducto, @CodigoProducto, @CantidadEntregada, @FechaHoraEntrega)
END	
GO

DROP PROCEDURE ActualizarCompraProductoDetalleEntrega
GO

CREATE PROCEDURE ActualizarCompraProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProducto			CHAR(15),
	@CantidadEntregada		INT,
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	UPDATE 	dbo.ComprasProductosDetalleEntrega
	SET					
		CantidadEntregada		= @CantidadEntregada,
		FechaHoraEntrega		= @FechaHoraEntrega
	WHERE (NumeroAlmacen = @NumeroAlmacen)
	AND NumeroCompraProducto = @NumeroCompraProducto
	AND CodigoProducto = @CodigoProducto
	AND FechaHoraEntrega = @FechaHoraEntrega
END
GO

DROP PROCEDURE EliminarCompraProductoDetalleEntrega
GO

CREATE PROCEDURE EliminarCompraProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProducto			CHAR(15),
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	DELETE 
	FROM dbo.ComprasProductosDetalleEntrega
	WHERE (NumeroAlmacen = @NumeroAlmacen)
	AND NumeroCompraProducto = @NumeroCompraProducto
	AND CodigoProducto = @CodigoProducto
	AND FechaHoraEntrega = @FechaHoraEntrega
END
GO

DROP PROCEDURE ListarComprasProductosDetalleEntrega
GO

CREATE PROCEDURE ListarComprasProductosDetalleEntrega
AS
BEGIN
	SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadEntregada, FechaHoraEntrega
	FROM dbo.ComprasProductosDetalleEntrega
	ORDER BY NumeroAlmacen, NumeroCompraProducto
END
GO

DROP PROCEDURE ObtenerCompraProductoDetalleEntrega
GO

CREATE PROCEDURE ObtenerCompraProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProducto			CHAR(15),
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadEntregada, FechaHoraEntrega
	FROM dbo.ComprasProductosDetalleEntrega
	WHERE (NumeroAlmacen = @NumeroAlmacen)
	AND NumeroCompraProducto = @NumeroCompraProducto
	AND CodigoProducto = @CodigoProducto
	AND FechaHoraEntrega = @FechaHoraEntrega
END
GO
