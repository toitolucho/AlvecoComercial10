USE AlvecoComercial10
GO


DROP TRIGGER TG_IAEVentasProductosDetalleEntrega
GO

CREATE TRIGGER TG_IAEVentasProductosDetalleEntrega
ON dbo.VentasProductosDetalleEntrega
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	--DECLARE @NUMERO INT
	DECLARE @ValoresInsertados	VARCHAR(MAX),
			@ValoresEliminados	VARCHAR(MAX),
			@BitacoraID			INT
	
	SELECT @ValoresInsertados =  NumeroAlmacen + ', ' + NumeroVentaProducto + ', ' + CodigoProducto + ', ' + FechaHoraEntrega + ', ' + FechaHoraCompraInventario + ', ' + CantidadEntregada + ', ' + PrecioUnitarioCompraInventario
	FROM inserted
	
	SELECT @ValoresEliminados =  NumeroAlmacen + ', ' + NumeroVentaProducto + ', ' + CodigoProducto + ', ' + FechaHoraEntrega + ', ' + FechaHoraCompraInventario + ', ' + CantidadEntregada + ', ' + PrecioUnitarioCompraInventario
	FROM deleted
	
	INSERT INTO Bitacora (EventType, Status, EventInfo)
	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')
	
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
