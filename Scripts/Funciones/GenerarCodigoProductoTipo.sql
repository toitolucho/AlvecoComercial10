USE AlvecoComercial10
GO


DROP FUNCTION GenerarCodigoProductoTipo
GO

CREATE FUNCTION GenerarCodigoProductoTipo()	
	RETURNS CHAR(10)
BEGIN

	DECLARE @CodigoProductoTipo CHAR(10)
	SELECT TOP(1) @CodigoProductoTipo =  CodigoProductoTipo
	FROM ProductosTipos
	ORDER BY CodigoProductoTipo DESC
	
	SET @CodigoProductoTipo = CAST( CAST( ISNULL(@CodigoProductoTipo,'0') AS INT) + 1 AS VARCHAR(10))
	
	SET @CodigoProductoTipo = RIGHT( '00000000000' + RTRIM(@CodigoProductoTipo),10)
	RETURN ISNULL( @CodigoProductoTipo ,'00000000001')
END
GO

