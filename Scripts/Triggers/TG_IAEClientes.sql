USE AlvecoComercial10
GO


DROP TRIGGER TG_IAEClientes
GO

CREATE TRIGGER TG_IAEClientes
ON dbo.Clientes
FOR DELETE, INSERT, UPDATE
AS
BEGIN

	
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	-- + ', ' + 
	SELECT	@ValoresInsertados =  CAST(ISNULL(CodigoCliente,-1) AS VARCHAR(100)) + ', ' + NombreCliente + ', ' + NombreRepresentante + ', ' + CodigoTipoCliente + ', ' + 
			ISNULL(NITCliente,' ') + ', ' + ISNULL(CodigoPais,' ') + ', ' + ISNULL(CodigoDepartamento,' ') + ', ' + 
			ISNULL(CodigoProvincia,' ') + ', ' + ISNULL(CodigoLugar,' ') + ', ' + 
			ISNULL(Direccion,' ') + ', ' + ISNULL(Telefono,' ') + ', '  + CodigoEstadoCliente
	FROM inserted
	
	SELECT @ValoresEliminados =  CAST(CodigoCliente AS VARCHAR(100))  + ', ' + NombreCliente + ', ' + NombreRepresentante + ', ' + CodigoTipoCliente + ', ' + 
			ISNULL(NITCliente,' ') + ', ' + ISNULL(CodigoPais,' ') + ', ' + ISNULL(CodigoDepartamento,' ') + ', ' + 
			ISNULL(CodigoProvincia,' ') + ', ' + ISNULL(CodigoLugar,' ') + ', ' + 
			ISNULL(Direccion,' ') + ', ' + ISNULL(Telefono,' ') + ', '  + CodigoEstadoCliente
	FROM deleted
	

	--SELECT Observaciones FROM deleted
	--DECLARE @TBitacoraAux TABLE
	--(
	--	EventType			NVARCHAR(30)	NULL,
	--	Status				INT				NULL,
	--	EventInfo			NVARCHAR(4000)	NULL
	--)
	
	--INSERT INTO @TBitacoraAux (EventType, Status, EventInfo)
	--EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')
	
	INSERT INTO Bitacora (EventType, Status, EventInfo)
	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')
	
	--INSERT INTO Bitacora (EventType, Status, EventInfo, ValoresInsertados, ValoresEliminados)
	--SELECT *, @ValoresInsertados, @ValoresEliminados
	--FROM @TBitacoraAux
	
	
	SET @BitacoraID = SCOPE_IDENTITY()
	
	UPDATE dbo.Bitacora
		SET ValoresInsertados = @ValoresInsertados,
			ValoresEliminados = @ValoresEliminados
	WHERE BitacoraID = @BitacoraID
	
	
	--SET @NUMERO = @@IDENTITY

	--UPDATE BITACORA
	--	SET Usuario = SYSTEM_USER,
	--	Fecha = GETDATE()
	--WHERE BitacoraID=@NUMERO
	
END
GO
