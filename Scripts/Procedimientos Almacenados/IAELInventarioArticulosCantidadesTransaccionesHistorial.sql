USE AlvecoComercial10
GO


DROP PROCEDURE InsertarInventarioProductoCantidadTransaccionHistorial
GO
CREATE PROCEDURE InsertarInventarioProductoCantidadTransaccionHistorial
	@NumeroAlmacen				INT,
	@NumeroTransaccionProducto	INT,
	@CodigoProducto				CHAR(15),
	@CodigoTipoTransaccion		CHAR(1),
	@FechaHoraCompra			DATETIME,
	@CantidadExistente			INT,
	@PrecioUnitario				DECIMAL(10,2)
AS
BEGIN
	INSERT INTO dbo.InventariosProductosCantidadesTransaccionesHistorial(NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, CodigoTipoTransaccion, FechaHoraCompra, CantidadExistente, PrecioUnitario)
	VALUES (@NumeroAlmacen, @NumeroTransaccionProducto, @CodigoProducto, @CodigoTipoTransaccion, @FechaHoraCompra, @CantidadExistente, @PrecioUnitario)
END
GO


DROP PROCEDURE ActualizarInventarioProductoCantidadTransaccionHistorial
GO
CREATE PROCEDURE ActualizarInventarioProductoCantidadTransaccionHistorial
	@NumeroAlmacen				INT,
	@NumeroTransaccionProducto	INT,
	@CodigoProducto				CHAR(15),
	@CodigoTipoTransaccion		CHAR(1),
	@FechaHoraCompra			DATETIME,
	@CantidadExistente			INT,
	@PrecioUnitario				DECIMAL(10,2)
AS
BEGIN
	UPDATE 	dbo.InventariosProductosCantidadesTransaccionesHistorial
	SET			
		NumeroAlmacen				= @NumeroAlmacen,
		NumeroTransaccionProducto	= @NumeroTransaccionProducto,
		CodigoProducto				= @CodigoProducto,
		CodigoTipoTransaccion		= @CodigoTipoTransaccion,
		FechaHoraCompra			= @FechaHoraCompra,
		CantidadExistente			= @CantidadExistente,
		PrecioUnitario				= @PrecioUnitario
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroTransaccionProducto = @NumeroTransaccionProducto
	AND CodigoProducto = @CodigoProducto	
	AND FechaHoraCompra = @FechaHoraCompra
	
END
GO



DROP PROCEDURE EliminarInventarioProductoCantidadTransaccionHistorial
GO
CREATE PROCEDURE EliminarInventarioProductoCantidadTransaccionHistorial
	@NumeroAlmacen				INT,
	@NumeroTransaccionProducto	INT,
	@CodigoProducto				CHAR(15),
	@FechaHoraCompra			DATETIME
AS
BEGIN
	DELETE 
	FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroTransaccionProducto = @NumeroTransaccionProducto
	AND CodigoProducto = @CodigoProducto	
	AND FechaHoraCompra = @FechaHoraCompra
END
GO



DROP PROCEDURE ListarInventarioProductosCantidadesTransaccionesHistorial
GO
CREATE PROCEDURE ListarInventarioProductosCantidadesTransaccionesHistorial
AS
BEGIN
	SELECT NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, CodigoTipoTransaccion, FechaHoraCompra, CantidadExistente, PrecioUnitario
	FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
	ORDER BY NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, FechaHoraCompra
END
GO



DROP PROCEDURE ObtenerInventarioProductoCantidadTransaccionHistorial
GO
CREATE PROCEDURE ObtenerInventarioProductoCantidadTransaccionHistorial
	@NumeroAlmacen				INT,
	@NumeroTransaccionProducto	INT,
	@CodigoProducto				CHAR(15),
	@FechaHoraCompra			DATETIME
AS
BEGIN
	SELECT NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, CodigoTipoTransaccion, FechaHoraCompra, CantidadExistente, PrecioUnitario
	FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroTransaccionProducto = @NumeroTransaccionProducto
	AND CodigoProducto = @CodigoProducto	
	AND FechaHoraCompra = @FechaHoraCompra
END
GO
