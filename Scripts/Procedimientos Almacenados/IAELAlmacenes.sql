USE AlvecoComercial10
GO

DROP PROCEDURE InsertarAlmacen
GO

CREATE PROCEDURE InsertarAlmacen
	
	@NombreAlmacen		VARCHAR(250),
	@Descripcion		VARCHAR(250)
AS
BEGIN
	IF(NOT EXISTS (SELECT * FROM Almacenes WHERE NombreAlmacen = @NombreAlmacen))
	BEGIN
		INSERT INTO dbo.Almacenes ( NombreAlmacen, Descripcion )								
		VALUES ( @NombreAlmacen, @Descripcion)
		
		DECLARE @NumeroAlmacen	INT
		SET @NumeroAlmacen = SCOPE_IDENTITY()
		
		INSERT INTO dbo.InventariosProductos(NumeroAlmacen, CodigoProducto, CantidadExistencia, CantidadRequerida, PrecioUnitarioCompra, PrecioUnitarioVentaPorMayor, PrecioUnitarioVentaPorMenor, PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor, TiempoGarantiaProducto, StockMinimo)
		SELECT @NumeroAlmacen, CodigoProducto, 0, 0, 0 , 0,0,0,0,0,0
		FROM dbo.Productos
	END
	ELSE
	BEGIN
		RAISERROR ('NO PUEDE REGISTRAR ALMACENES CON NOMBRES REPETIDOS, ACTUALMENTE YA EXISTE UN ALMACEN DON ESE DATO',17,2)		
	END
	
	
END	
GO

DROP PROCEDURE ActualizarAlmacen
GO

CREATE PROCEDURE ActualizarAlmacen
	@NumeroAlmacen		INT,
	@NombreAlmacen		VARCHAR(250),
	@Descripcion		VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.Almacenes
	SET					
		NombreAlmacen		= @NombreAlmacen,
		Descripcion			= @Descripcion
	WHERE (NumeroAlmacen = @NumeroAlmacen)
END
GO

DROP PROCEDURE EliminarAlmacen
GO

CREATE PROCEDURE EliminarAlmacen
	@NumeroAlmacen	INT
AS
BEGIN
	DELETE 
	FROM dbo.Almacenes
	WHERE (NumeroAlmacen = @NumeroAlmacen)
END
GO

DROP PROCEDURE ListarAlmacenes
GO

CREATE PROCEDURE ListarAlmacenes
AS
BEGIN
	SELECT NumeroAlmacen, NombreAlmacen, Descripcion
	FROM dbo.Almacenes
	ORDER BY NumeroAlmacen
END
GO

DROP PROCEDURE ObtenerAlmacen
GO

CREATE PROCEDURE ObtenerAlmacen
	@NumeroAlmacen	INT
AS
BEGIN
	SELECT NumeroAlmacen, NombreAlmacen, Descripcion
	FROM dbo.Almacenes
	WHERE (NumeroAlmacen = @NumeroAlmacen)
END
GO
