USE AlvecoComercial10
GO


DROP TRIGGER TG_IAEProductos
GO

CREATE TRIGGER TG_IAEProductos
ON dbo.Productos
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	--DECLARE @NUMERO INT
	DECLARE @CodigoProducto CHAR(15)
	
	
	SELECT @CodigoProducto = CodigoProducto FROM inserted
	
	IF(NOT EXISTS(SELECT * FROM InventariosProductos WHERE CodigoProducto = @CodigoProducto) 
		AND @CodigoProducto IS NOT NULL)
	BEGIN
		INSERT INTO dbo.InventariosProductos(NumeroAlmacen, CodigoProducto, CantidadExistencia, CantidadRequerida, PrecioUnitarioCompra, PrecioUnitarioVentaPorMayor, PrecioUnitarioVentaPorMenor, PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor, TiempoGarantiaProducto, StockMinimo)
		SELECT NumeroAlmacen, @CodigoProducto, 0, 0, 0, 0, 0, 0, 0, 0, 1
		FROM Almacenes
	END

	
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	-- + ', ' + 
	SELECT	@ValoresInsertados =	CodigoProducto + ', ' + 
									CodigoProductoFabricante + ', ' + 
									NombreProducto + ', ' + 
									ISNULL(NombreProductoAlternativo, ' ') + ', ' + 
									ISNULL(CodigoProductoTipo,' ') + ', ' + 
									ISNULL(CAST(CodigoMarcaProducto AS VARCHAR(10)),' ') + ', ' + 
									ISNULL(CAST(CodigoUnidadProducto AS VARCHAR(10)), ' ') + ', ' + 
									CodigoTipoCalculoInventario + ', ' + 
									CAST(ActualizarPrecioVenta AS CHAR(1)) + ', ' + 
									ISNULL(CAST(CodigoProveedor AS VARCHAR(50)),' ')
	FROM Inserted
	
	SELECT	@ValoresEliminados =	CodigoProducto + ', ' + 
									CodigoProductoFabricante + ', ' + 
									NombreProducto + ', ' + 
									ISNULL(NombreProductoAlternativo, ' ') + ', ' + 
									ISNULL(CodigoProductoTipo,' ') + ', ' + 
									ISNULL(CAST(CodigoMarcaProducto AS VARCHAR(10)),' ') + ', ' + 
									ISNULL(CAST(CodigoUnidadProducto AS VARCHAR(10)), ' ') + ', ' + 
									CodigoTipoCalculoInventario + ', ' + 
									CAST(ActualizarPrecioVenta AS CHAR(1)) + ', ' + 
									ISNULL(CAST(CodigoProveedor AS VARCHAR(50)),' ')
	FROM deleted
	
	
	INSERT INTO Bitacora (EventType, Status, EventInfo)
	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')
	
	
	SET @BitacoraID = SCOPE_IDENTITY()
	
	UPDATE dbo.Bitacora
		SET ValoresInsertados = @ValoresInsertados,
			ValoresEliminados = @ValoresEliminados
	WHERE BitacoraID = @BitacoraID
	
	
END
GO

