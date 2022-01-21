USE AlvecoComercial10
GO

DROP PROCEDURE InsertarProductoTipo
GO

CREATE PROCEDURE InsertarProductoTipo
	@CodigoProductoTipo			CHAR(10),
	@CodigoProductoTipoPadre	CHAR(10),
	@NombreProductoTipo			VARCHAR(250),
	@DescripcionProductoTipo			VARCHAR(250)
AS
BEGIN
	

	
	BEGIN TRANSACTION
	
	IF(NOT EXISTS (SELECT * FROM dbo.ProductosTipos WHERE NombreProductoTipo =  @NombreProductoTipo))
	BEGIN
		INSERT INTO dbo.ProductosTipos (CodigoProductoTipo, CodigoProductoTipoPadre, NombreProductoTipo, DescripcionProductoTipo )								
		VALUES (@CodigoProductoTipo, @CodigoProductoTipoPadre, @NombreProductoTipo, @DescripcionProductoTipo)
	END
	ELSE
		RAISERROR('EL TIPO DE PRODUCTO QUE DESEA ALMACENAR YA SE ENCUENTRA REGISTRADO', 17,2)
	IF (@@ERROR <> 0)
    BEGIN
        ROLLBACK
        RAISERROR ('NO SE PUDO INSERTAR CORRECTAMENTE EL REGISTRO',16,2)
    END
    ELSE
        COMMIT TRANSACTION
END	
GO  

DROP PROCEDURE ActualizarProductoTipo
GO

CREATE PROCEDURE ActualizarProductoTipo
	@CodigoProductoTipo			char(10),
	@CodigoProductoTipoPadre		char(10),
	@NombreProductoTipo			VARCHAR(250),
	@DescripcionProductoTipo		VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.ProductosTipos
	SET	
		CodigoProductoTipoPadre	= @CodigoProductoTipoPadre,
		NombreProductoTipo		= @NombreProductoTipo,
		DescripcionProductoTipo	= @DescripcionProductoTipo
		
	WHERE (CodigoProductoTipo = @CodigoProductoTipo)
END
GO

DROP PROCEDURE EliminarProductoTipo
GO

CREATE PROCEDURE EliminarProductoTipo
	@CodigoProductoTipo	char(10)
AS
BEGIN
	DELETE 
	FROM dbo.ProductosTipos
	WHERE (CodigoProductoTipo = @CodigoProductoTipo)
END
GO

DROP PROCEDURE ListarProductosTipos
GO

CREATE PROCEDURE ListarProductosTipos
AS
BEGIN
	SELECT CodigoProductoTipo, CodigoProductoTipoPadre, NombreProductoTipo, DescripcionProductoTipo
	FROM dbo.ProductosTipos
	ORDER BY NombreProductoTipo
END
GO

DROP PROCEDURE ObtenerProductoTipo
GO

CREATE PROCEDURE ObtenerProductoTipo
	@CodigoProductoTipo	char(10)
AS
BEGIN
	SELECT CodigoProductoTipo, CodigoProductoTipoPadre, NombreProductoTipo, DescripcionProductoTipo
	FROM dbo.ProductosTipos
	WHERE (CodigoProductoTipo = @CodigoProductoTipo)
END
GO

DROP PROCEDURE ListarProductosTiposPadres
GO

CREATE PROCEDURE ListarProductosTiposPadres
AS
SELECT P.CodigoProductoTipo, P.CodigoProductoTipoPadre, P.NombreProductoTipo, P.DescripcionProductoTipo
FROM ProductosTipos P
WHERE P.CodigoProductoTipoPadre IS NULL
GO


DROP PROCEDURE ListarProductosTiposPorProductoTipoPadre
GO

CREATE PROCEDURE ListarProductosTiposPorProductoTipoPadre
@CodigoProductoTipoPadre		CHAR(10)
AS
SELECT P.CodigoProductoTipo, P.CodigoProductoTipoPadre, P.NombreProductoTipo, P.DescripcionProductoTipo
FROM ProductosTipos P
WHERE P.CodigoProductoTipoPadre = @CodigoProductoTipoPadre OR 
@CodigoProductoTipoPadre IS NULL AND CodigoProductoTipoPadre IS NULL
GO

DROP PROCEDURE ObtenerProductoTipoPorNombreProductoTipo
GO
	
CREATE PROCEDURE ObtenerProductoTipoPorNombreProductoTipo
	@NombreProductoTipo VARCHAR(250)
AS
BEGIN
	SELECT P.CodigoProductoTipo, P.CodigoProductoTipoPadre, P.NombreProductoTipo, P.DescripcionProductoTipo
	FROM ProductosTipos P
	WHERE P.NombreProductoTipo LIKE @NombreProductoTipo
END
GO