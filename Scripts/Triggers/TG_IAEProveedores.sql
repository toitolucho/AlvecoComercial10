USE AlvecoComercial10
GO


DROP TRIGGER TG_IAEProveedores
GO

CREATE TRIGGER TG_IAEProveedores
ON dbo.Proveedores
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	-- + ', ' + 
	SELECT	@ValoresInsertados =  CAST(ISNULL(CodigoProveedor,-1) AS VARCHAR(100)) + ', ' + NombreRazonSocial + ', ' + NombreRepresentante + ', ' + CodigoTipoProveedor + ', ' + 
			ISNULL(NITProveedor,' ') + ', ' + ISNULL(CodigoPais,' ') + ', ' + ISNULL(CodigoDepartamento,' ') + ', ' + 
			ISNULL(CodigoProvincia,' ') + ', ' + ISNULL(CodigoLugar,' ') + ', ' + 
			ISNULL(Direccion,' ') + ', ' + ISNULL(Telefono,' ') + ', '  + 
			ISNULL(Fax,' ') + ', ' + ISNULL(Casilla,' ') + ', '  + 
			CAST(ProveedorActivo AS CHAR(1))
	FROM inserted
	
	SELECT @ValoresEliminados =  CAST(CodigoProveedor AS VARCHAR(100))  + ', ' + NombreRazonSocial + ', ' + NombreRepresentante + ', ' + CodigoTipoProveedor + ', ' + 
			ISNULL(NITProveedor,' ') + ', ' + ISNULL(CodigoPais,' ') + ', ' + ISNULL(CodigoDepartamento,' ') + ', ' + 
			ISNULL(CodigoProvincia,' ') + ', ' + ISNULL(CodigoLugar,' ') + ', ' + 
			ISNULL(Direccion,' ') + ', ' + ISNULL(Telefono,' ') + ', '  + 
			ISNULL(Fax,' ') + ', ' + ISNULL(Casilla,' ') + ', '  + 
			CAST(ProveedorActivo AS CHAR(1))
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

