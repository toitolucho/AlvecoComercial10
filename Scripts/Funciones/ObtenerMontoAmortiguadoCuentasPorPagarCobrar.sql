USE AlvecoComercial10
GO

DROP FUNCTION ObtenerMontoAmortiguadoCuentasPorPagarCobrar
GO

CREATE FUNCTION ObtenerMontoAmortiguadoCuentasPorPagarCobrar(@NumeroAlmacen INT, @NumeroCuenta INT, @TipoCuenta CHAR(1)) 
RETURNS DECIMAL(10,2)
AS
BEGIN
	DECLARE @MontoTotalAmortizacion DECIMAL(10,2)
	
	IF(@TipoCuenta = 'C')--CUENTAS POR COBRAR
	BEGIN
		SELECT @MontoTotalAmortizacion = Monto
		FROM CuentasPorCobrarCobros
		WHERE NumeroCuentaPorCobrar = @NumeroCuenta
	END
	ELSE IF(@TipoCuenta = 'P')
	BEGIN
		SELECT @MontoTotalAmortizacion = Monto
		FROM CuentasPorPagarPagos
		WHERE NumeroCuentaPorPagar = @NumeroCuenta
	END
	
	RETURN ISNULL(@MontoTotalAmortizacion,0)
END
GO




