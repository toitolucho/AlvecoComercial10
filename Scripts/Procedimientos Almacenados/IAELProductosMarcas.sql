USE AlvecoComercial10
GO

DROP PROCEDURE InsertarProductoMarca
GO

CREATE PROCEDURE InsertarProductoMarca
	
	@NombreMarca		VARCHAR(250),
	@CodigoTipoMarca	char(1)
	
AS
BEGIN
	INSERT INTO dbo.ProductosMarcas ( NombreMarca, CodigoTipoMarca )								
	VALUES ( @NombreMarca, @CodigoTipoMarca)
END	
GO

DROP PROCEDURE ActualizarProductoMarca
GO

CREATE PROCEDURE ActualizarProductoMarca
	@CodigoMarca		INT,
	@NombreMarca		VARCHAR(250),
	@CodigoTipoMarca	char(1)
AS
BEGIN
	UPDATE 	dbo.ProductosMarcas
	SET					
		NombreMarca		= @NombreMarca,
		CodigoTipoMarca = @CodigoTipoMarca
		
	WHERE (CodigoMarca = @CodigoMarca)
END
GO

DROP PROCEDURE EliminarProductoMarca
GO

CREATE PROCEDURE EliminarProductoMarca
	@CodigoMarca	INT
AS
BEGIN
	DELETE 
	FROM dbo.ProductosMarcas
	WHERE (CodigoMarca = @CodigoMarca)
END
GO

DROP PROCEDURE ListarProductosMarcas
GO

CREATE PROCEDURE ListarProductosMarcas
AS
BEGIN
	SELECT CodigoMarca, NombreMarca, CodigoTipoMarca
	FROM dbo.ProductosMarcas
	ORDER BY NombreMarca
END
GO

DROP PROCEDURE ObtenerProductoMarca
GO

CREATE PROCEDURE ObtenerProductoMarca
	@CodigoMarca	INT
AS
BEGIN
	SELECT CodigoMarca, NombreMarca, CodigoTipoMarca
	FROM dbo.ProductosMarcas
	WHERE (CodigoMarca = @CodigoMarca)
END
GO
