USE AlvecoComercial10
GO

DROP PROCEDURE InsertarProducto
GO
CREATE PROCEDURE InsertarProducto
	@CodigoProducto					CHAR(15),
	@CodigoProductoFabricante		CHAR(30),	
	@NombreProducto					VARCHAR(250),
	@NombreProductoAlternativo		VARCHAR(250),
	@CodigoProductoTipo				CHAR(10),
	@CodigoMarcaProducto			INT,
	@CodigoUnidadProducto			INT,
	@CodigoTipoCalculoInventario	CHAR(1),
	@ActualizarPrecioVenta			BIT,
	@CodigoProveedor				INT,
	@Descripcion					TEXT,
	@Observaciones					TEXT
AS
BEGIN
	--//todos los productos se actualizan por el campo @ActualizarPrecioVenta

	INSERT INTO dbo.Productos(CodigoProducto, CodigoProductoFabricante, NombreProducto, NombreProductoAlternativo, CodigoProductoTipo, CodigoMarcaProducto, CodigoUnidadProducto , CodigoTipoCalculoInventario, ActualizarPrecioVenta, CodigoProveedor, Descripcion, Observaciones)
	VALUES (@CodigoProducto, @CodigoProductoFabricante, @NombreProducto, @NombreProductoAlternativo, @CodigoProductoTipo, @CodigoMarcaProducto, @CodigoUnidadProducto, @CodigoTipoCalculoInventario, 1, @CodigoProveedor, @Descripcion, @Observaciones)
END
GO

DROP PROCEDURE ActualizarProducto
GO
CREATE PROCEDURE ActualizarProducto
	@CodigoProducto					CHAR(15),
	@CodigoProductoFabricante		CHAR(30),	
	@NombreProducto					VARCHAR(250),
	@NombreProductoAlternativo		VARCHAR(250),
	@CodigoProductoTipo				CHAR(10),
	@CodigoMarcaProducto			INT,
	@CodigoUnidadProducto			INT,
	@CodigoTipoCalculoInventario	CHAR(1),
	@ActualizarPrecioVenta			BIT,
	@CodigoProveedor				INT,
	@Descripcion					TEXT,
	@Observaciones					TEXT	
AS
BEGIN
	UPDATE 	dbo.Productos
	SET			
		CodigoProducto				= @CodigoProducto,
		CodigoProductoFabricante	= @CodigoProductoFabricante,
		NombreProducto				= @NombreProducto,
		NombreProductoAlternativo	= @NombreProductoAlternativo,
		CodigoProductoTipo			= @CodigoProductoTipo,
		CodigoMarcaProducto			= @CodigoMarcaProducto,
		CodigoUnidadProducto		= @CodigoUnidadProducto,
		CodigoTipoCalculoInventario	= @CodigoTipoCalculoInventario,
		ActualizarPrecioVenta		= @ActualizarPrecioVenta,
		CodigoProveedor				= @CodigoProveedor,
		Descripcion					= @Descripcion,
		Observaciones				= @Observaciones
	WHERE	(CodigoProducto = @CodigoProducto)
END
GO

DROP PROCEDURE EliminarProducto
GO
CREATE PROCEDURE EliminarProducto
	@CodigoProducto CHAR(15)
AS
BEGIN
	BEGIN TRY
	
	IF(EXISTS (SELECT * FROM dbo.VentasProductosDetalle
		WHERE CodigoProducto = @CodigoProducto)
		or
		EXISTS(SELECT * FROM dbo.ComprasProductosDetalle
		WHERE CodigoProducto = @CodigoProducto)
		)
	BEGIN
		RAISERROR ('Existen transacciones realizadas con este Producto, elimínelas primeramente antes de continuar', 17 ,2)
	END
	ELSE
	BEGIN
		DELETE 
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE CodigoProducto = @CodigoProducto
		
		DELETE 
		FROM dbo.InventariosProductos
		WHERE CodigoProducto = @CodigoProducto
		
		DELETE 
		FROM dbo.Productos
		WHERE	(CodigoProducto = @CodigoProducto)
	END
END TRY
BEGIN CATCH
	RAISERROR ('Existen transacciones realizadas con este Producto, elimínelas primeramente antes de continuar', 17 ,2)
	RETURN
END CATCH
	
	
END
GO

DROP PROCEDURE ObtenerProducto
GO
CREATE PROCEDURE ObtenerProducto
	@CodigoProducto	CHAR(15)
AS
BEGIN
	SELECT	CodigoProducto, CodigoProductoFabricante, NombreProducto, NombreProductoAlternativo, CodigoProveedor, CodigoProductoTipo, CodigoMarcaProducto, 
			CodigoUnidadProducto, CodigoTipoCalculoInventario, ActualizarPrecioVenta, Descripcion, Observaciones
	FROM dbo.Productos
	WHERE	(CodigoProducto = @CodigoProducto)
END
GO

DROP PROCEDURE ListarProductos
GO
CREATE PROCEDURE ListarProductos
AS
BEGIN
	SELECT	CodigoProducto, CodigoProductoFabricante, NombreProducto, NombreProductoAlternativo, CodigoProveedor, CodigoProductoTipo, CodigoMarcaProducto, 
			CodigoUnidadProducto, CodigoTipoCalculoInventario, ActualizarPrecioVenta, Descripcion, Observaciones
	FROM dbo.Productos
	ORDER BY CodigoProducto
END
GO

DROP PROCEDURE ListarProductosPorProductoTipo
GO
CREATE PROCEDURE ListarProductosPorProductoTipo
@CodigoProductoTipo	CHAR(10)
AS
BEGIN
	SELECT	CodigoProducto, CodigoProductoFabricante, NombreProducto, NombreProductoAlternativo, CodigoProveedor, CodigoProductoTipo, CodigoMarcaProducto, 
			CodigoUnidadProducto, CodigoTipoCalculoInventario, ActualizarPrecioVenta, Descripcion, Observaciones
	FROM dbo.Productos
	WHERE CodigoProductoTipo = @CodigoProductoTipo
	ORDER BY CodigoProducto
END
GO

