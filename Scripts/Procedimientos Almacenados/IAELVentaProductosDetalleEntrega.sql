USE AlvecoComercial10
GO


DROP PROCEDURE InsertarVentaProductoDetalleEntrega
GO
CREATE PROCEDURE InsertarVentaProductoDetalleEntrega
	@NumeroAlmacen						INT,
	@NumeroVentaProducto				INT,
	@CodigoProducto						CHAR(15),
	@FechaHoraEntrega					DATETIME,
	@CantidadEntregada					INT
AS
BEGIN
	INSERT INTO dbo.VentasProductosDetalleEntrega(NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, FechaHoraCompraInventario, CantidadEntregada)
	VALUES (@NumeroAlmacen, @NumeroVentaProducto, @CodigoProducto, @FechaHoraEntrega, GETDATE(), @CantidadEntregada)
END
GO


DROP PROCEDURE ActualizarVentaProductoDetalleEntrega
GO
CREATE PROCEDURE ActualizarVentaProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15),
	@FechaHoraEntrega		DATETIME,
	@CantidadEntregada		INT
AS
BEGIN
	UPDATE 	dbo.VentasProductosDetalleEntrega
	SET					
		CantidadEntregada = @CantidadEntregada
	WHERE NumeroAlmacen	 = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	AND CodigoProducto = @CodigoProducto
	AND FechaHoraEntrega = @FechaHoraEntrega
END
GO



DROP PROCEDURE EliminarVentaProductoDetalleEntrega
GO
CREATE PROCEDURE EliminarVentaProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15),
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	DELETE 
	FROM dbo.VentasProductosDetalleEntrega
	WHERE NumeroAlmacen	 = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	AND CodigoProducto = @CodigoProducto
	AND FechaHoraEntrega = @FechaHoraEntrega
END
GO



DROP PROCEDURE ListarVentasProductosDetalleEntregas
GO
CREATE PROCEDURE ListarVentasProductosDetalleEntregas
AS
BEGIN
	SELECT NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario
	FROM dbo.VentasProductosDetalleEntrega
	ORDER BY NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega
END
GO



DROP PROCEDURE ObtenerVentaProductoDetalleEntrega
GO
CREATE PROCEDURE ObtenerVentaProductoDetalleEntrega
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15),
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	SELECT NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario
	FROM dbo.VentasProductosDetalleEntrega
	WHERE NumeroAlmacen	 = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	AND CodigoProducto = @CodigoProducto
	AND FechaHoraEntrega = @FechaHoraEntrega
END
GO
