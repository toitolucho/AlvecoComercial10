USE AlvecoComercial10
GO

DROP PROCEDURE InsertarProductoUnidad
GO

CREATE PROCEDURE InsertarProductoUnidad
@NombreUnidad		VARCHAR(250)
	
AS
BEGIN
	INSERT INTO dbo.ProductosUnidades (NombreUnidad )								
	VALUES (@NombreUnidad)
END	
GO

DROP PROCEDURE ActualizarProductoUnidad
GO

CREATE PROCEDURE ActualizarProductoUnidad
@CodigoUnidad		INT,
@NombreUnidad		VARCHAR(250)
	
AS
BEGIN
	UPDATE 	dbo.ProductosUnidades
	SET					
		NombreUnidad		= @NombreUnidad
		
	WHERE (CodigoUnidad = @CodigoUnidad)
END
GO

DROP PROCEDURE EliminarProductoUnidad
GO

CREATE PROCEDURE EliminarProductoUnidad
@CodigoUnidad	INT
AS
BEGIN
	DELETE 
	FROM dbo.ProductosUnidades
	WHERE (CodigoUnidad = @CodigoUnidad)
END
GO

DROP PROCEDURE ListarProductosUnidades
GO

CREATE PROCEDURE ListarProductosUnidades
AS
BEGIN
	SELECT CodigoUnidad, NombreUnidad
	FROM dbo.ProductosUnidades
	ORDER BY NombreUnidad
END
GO

DROP PROCEDURE ObtenerProductoUnidad
GO

CREATE PROCEDURE ObtenerProductoUnidad
@CodigoUnidad	INT
AS
BEGIN
	SELECT CodigoUnidad, NombreUnidad
	FROM dbo.ProductosUnidades
	WHERE (CodigoUnidad = @CodigoUnidad)
END
GO