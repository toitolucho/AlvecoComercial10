USE AlvecoComercial10
GO


DROP TRIGGER TG_IAEComprasProductos
GO

CREATE TRIGGER TG_IAEComprasProductos
ON dbo.ComprasProductos
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	-- + ', ' + 
	SELECT	@ValoresInsertados =	CAST(NumeroAlmacen AS VARCHAR(10)) + ', ' + 
									CAST(NumeroCompraProducto AS VARCHAR(100))+ ', ' + 
									CodigoCompraProducto + ', ' + 
									CAST(CodigoProveedor AS VARCHAR(10))+ ', ' + 
									DIUsuario + ', ' + 
									CAST(FechaHoraRegistro AS VARCHAR(20))+ ', ' + 
									ISNULL(CAST(FechaHoraRecepcion AS VARCHAR(20)),' ') + ', ' + 
									CodigoTipoCompra + ', ' + 
									CodigoEstadoCompra + ', ' + 
									ISNULL(NumeroFactura,' ') + ', ' + 
									ISNULL(NumeroComprobante,' ') + ', ' + 
									CAST(MontoTotalCompra AS VARCHAR(100))+ ', ' + 
									ISNULL(CAST(MontoTotalPagoEfectivo AS VARCHAR(100)),' ') + ', ' + 
									ISNULL(CAST(NumeroCuentaPorPagar AS VARCHAR(100)),' ')
	FROM inserted
	
	SELECT	@ValoresEliminados =	CAST(NumeroAlmacen AS VARCHAR(10)) + ', ' + 
									CAST(NumeroCompraProducto AS VARCHAR(100))+ ', ' + 
									CodigoCompraProducto + ', ' + 
									CAST(CodigoProveedor AS VARCHAR(10))+ ', ' + 
									DIUsuario + ', ' + 
									CAST(FechaHoraRegistro AS VARCHAR(20))+ ', ' + 
									ISNULL(CAST(FechaHoraRecepcion AS VARCHAR(20)),' ') + ', ' + 
									CodigoTipoCompra + ', ' + 
									CodigoEstadoCompra + ', ' + 
									ISNULL(NumeroFactura,' ') + ', ' + 
									ISNULL(NumeroComprobante,' ') + ', ' + 
									CAST(MontoTotalCompra AS VARCHAR(100))+ ', ' + 
									ISNULL(CAST(MontoTotalPagoEfectivo AS VARCHAR(100)),' ') + ', ' + 
									ISNULL(CAST(NumeroCuentaPorPagar AS VARCHAR(100)),' ')
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
