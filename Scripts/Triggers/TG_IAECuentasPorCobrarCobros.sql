USE AlvecoComercial10
GO


DROP TRIGGER TG_IAECuentasPorCobrarCobros
GO

CREATE TRIGGER TG_IAECuentasPorCobrarCobros
ON dbo.CuentasPorCobrarCobros
FOR DELETE, INSERT, UPDATE
AS
BEGIN
	--DECLARE @NUMERO INT
	INSERT INTO Bitacora (EventType, Status, EventInfo)
	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')

	DECLARE @NumeroCuentaPorCobrar	INT,
			@MontoTotal				DECIMAL(10,2)
	
	SELECT @NumeroCuentaPorCobrar = NumeroCuentaPorCobrar
	FROM inserted
	
	SELECT @MontoTotal = Monto
	FROM dbo.CuentasPorCobrar
	WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
	
	IF((SELECT SUM(Monto) FROM dbo.CuentasPorCobrarCobros WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar) = @MontoTotal)
	BEGIN
		UPDATE dbo.CuentasPorCobrar
			SET CodigoEstado = 'P'
		WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
	END
END
GO
