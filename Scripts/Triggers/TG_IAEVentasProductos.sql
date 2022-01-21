USE AlvecoComercial10
GO


DROP TRIGGER TG_IAEVentasProductos
GO

CREATE TRIGGER TG_IAEVentasProductos
ON dbo.VentasProductos
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	--SELECT * FROM VENTASPRODUCTOS
	
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	-- + ', ' + 
	SELECT  @ValoresInsertados = CAST(NumeroAlmacen AS VARCHAR(10)) + ', ' + 
								 CAST(NumeroVentaProducto AS VARCHAR(10))+ ', ' + 
								 CodigoVentaProducto+ ', ' +  
								 CAST(CodigoCliente AS VARCHAR(100)) + ', ' + 
								 DIUsuario + ', ' + 
								 CAST(FechaHoraVenta AS VARCHAR(20)) + ', ' + 
								 ISNULL(CAST(FechaHoraEntrega AS VARCHAR(20)),' ') + ', ' + 
								 ISNULL(NumeroComprobante, ' ') + ', ' +  
								 CodigoEstadoVenta + ', ' +  
								 CodigoTipoVenta + ', ' +  
								 ISNULL(NumeroFactura, ' ') + ', ' +  
								 CAST(MontoTotalVenta AS VARCHAR(100)) + ', ' + 
								 CAST(MontoTotalPagoEfectivo AS VARCHAR(100))+ ', ' + 
								 ISNULL(DIPersonaDistribuidor,' ')+ ', ' + 
								 ISNULL(CAST(VentaParaDistribuir AS CHAR(1)), ' ')+ ', ' + 
								 ISNULL(CodigoMovilidad, ' ') + ', ' + 
								 ISNULL(CAST(MontoTotalDescuento AS VARCHAR(100)),' ')+ ', ' + 
								 ISNULL(CAST(NumeroCuentaPorCobrar AS VARCHAR(100)),' ') + ', ' +  
								 ISNULL(CAST(EsVentaDistribuible AS CHAR(1)),' ')
								 --ISNULL(CAST(Observaciones AS VARCHAR(MAX)),' ')
								 --ISNULL(CAST(EsVentaDistribuible AS CHAR(1)),' ')+ ', ' + 
								 --ISNULL(CAST(Observaciones AS VARCHAR(MAX)),' ')
	FROM inserted
								 
	SELECT  @ValoresEliminados = CAST(NumeroAlmacen AS VARCHAR(10)) + ', ' + 
								 CAST(NumeroVentaProducto AS VARCHAR(10))+ ', ' + 
								 CodigoVentaProducto+ ', ' +  
								 CAST(CodigoCliente AS VARCHAR(100)) + ', ' + 
								 DIUsuario + ', ' + 
								 CAST(FechaHoraVenta AS VARCHAR(20)) + ', ' + 
								 ISNULL(CAST(FechaHoraEntrega AS VARCHAR(20)),' ') + ', ' + 
								 ISNULL(NumeroComprobante, ' ') + ', ' +  
								 CodigoEstadoVenta + ', ' +  
								 CodigoTipoVenta + ', ' +  
								 ISNULL(NumeroFactura, ' ') + ', ' +  
								 CAST(MontoTotalVenta AS VARCHAR(100)) + ', ' + 
								 CAST(MontoTotalPagoEfectivo AS VARCHAR(100))+ ', ' + 
								 ISNULL(DIPersonaDistribuidor,' ')+ ', ' + 
								 ISNULL(CAST(VentaParaDistribuir AS CHAR(1)), ' ')+ ', ' + 
								 ISNULL(CodigoMovilidad, ' ') + ', ' + 
								 ISNULL(CAST(MontoTotalDescuento AS VARCHAR(100)),' ')+ ', ' + 
								 ISNULL(CAST(NumeroCuentaPorCobrar AS VARCHAR(100)),' ') + ', ' +  
								 ISNULL(CAST(EsVentaDistribuible AS CHAR(1)),' ')
								 --ISNULL(CAST(Observaciones AS VARCHAR(MAX)),' ')
								 --ISNULL(CAST(EsVentaDistribuible AS CHAR(1)),' ')+ ', ' + 
								 --ISNULL(CAST(Observaciones AS VARCHAR(MAX)),' ')
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
