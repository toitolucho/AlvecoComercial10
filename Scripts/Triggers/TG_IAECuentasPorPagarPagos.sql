USE AlvecoComercial10
GO


DROP TRIGGER TG_IAECuentasPorPagarPagos
GO

CREATE TRIGGER TG_IAECuentasPorPagarPagos
ON dbo.CuentasPorPagarPagos
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	DECLARE @NUMERO INT
	INSERT INTO Bitacora (EventType, Status, EventInfo)
	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')

	DECLARE @NumeroCuentaPorPagar	INT,
			@MontoTotal				DECIMAL(10,2),
			@MontoTotalPagos		DECIMAL(10,2)
	
	SELECT @NumeroCuentaPorPagar = NumeroCuentaPorPagar
	FROM inserted
	
	SELECT @MontoTotal = Monto
	FROM dbo.CuentasPorPagar
	WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
	
	SELECT @MontoTotalPagos = SUM(Monto) 
	FROM dbo.CuentasPorPagarPagos
	WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
	
	IF(@MontoTotalPagos = @MontoTotal)
	BEGIN
		UPDATE dbo.CuentasPorPagar
			SET CodigoEstado = 'P'
		WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
	END
	
END
GO
