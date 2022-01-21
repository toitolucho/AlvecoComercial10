USE AlvecoComercial10
GO


DROP TRIGGER TG_IAECuentasPorPagar
GO

CREATE TRIGGER TG_IAECuentasPorPagar
ON dbo.CuentasPorPagar
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	-- + ', ' + 
	SELECT	@ValoresInsertados =CAST(NumeroCuentaPorPagar AS VARCHAR(100)) + ', ' + 
								CAST(NumeroAlmacen AS VARCHAR(10))+ ', ' + 
								CAST(FechaHoraRegistro AS VARCHAR(20))+ ', ' + 
								ISNULL(CAST(CodigoProveedor AS VARCHAR(20)),' ')+ ', ' + 
								ISNULL(CAST(NumeroConcepto AS VARCHAR(10)) ,' ') + ', ' + 
								CAST(Monto AS VARCHAR(100)) + ', ' + 
								ISNULL(CAST(FechaLimite AS VARCHAR(20)), ' ')+ ', ' + 
								CodigoEstado+ ', ' + 								
								ISNULL(DIUsuario,' ')
	FROM inserted
	
	SELECT	@ValoresInsertados =CAST(NumeroCuentaPorPagar AS VARCHAR(100)) + ', ' + 
								CAST(NumeroAlmacen AS VARCHAR(10))+ ', ' + 
								CAST(FechaHoraRegistro AS VARCHAR(20))+ ', ' + 
								ISNULL(CAST(CodigoProveedor AS VARCHAR(20)),' ')+ ', ' + 
								ISNULL(CAST(NumeroConcepto AS VARCHAR(10)) ,' ') + ', ' + 
								CAST(Monto AS VARCHAR(100)) + ', ' + 
								ISNULL(CAST(FechaLimite AS VARCHAR(20)), ' ')+ ', ' + 
								CodigoEstado+ ', ' + 								
								ISNULL(DIUsuario,' ')
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
