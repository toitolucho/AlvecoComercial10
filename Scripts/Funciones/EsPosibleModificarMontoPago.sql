USE AlvecoComercial10
GO

DROP FUNCTION EsPosibleModificarMontoPagoVenta
GO
	
CREATE FUNCTION EsPosibleModificarMontoPagoVenta	(
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@MontoNuevo				DECIMAL(10,2)
)
RETURNS BIT
AS 
BEGIN
	DECLARE @EsPosible	BIT
	
	IF( EXISTS(SELECT * FROM dbo.VentasProductos 
		WHERE NumeroAlmacen = @NumeroAlmacen 
		AND NumeroVentaProducto = @NumeroVentaProducto
		AND CodigoTipoVenta = 'E'))
	BEGIN
		SET @EsPosible = 1
	END
	ELSE
	BEGIN
		DECLARE @NumeroCuentaPorCobrar	INT,
				@Monto					DECIMAL(10,2),
				@MontoTotalVenta		DECIMAL(10,2),
				@MontoTotalPagoEfectivo	DECIMAL(10,2)
		SELECT @NumeroCuentaPorCobrar = NumeroCuentaPorCobrar, @MontoTotalVenta = MontoTotalVenta,
				 @MontoTotalPagoEfectivo = MontoTotalPagoEfectivo
		FROM dbo.VentasProductos 
		WHERE NumeroAlmacen = @NumeroAlmacen 
		AND NumeroVentaProducto = @NumeroVentaProducto
		
		SELECT @Monto = Monto
		FROM dbo.CuentasPorCobrar
		WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
		
		IF(@MontoNuevo > @MontoTotalPagoEfectivo AND (SELECT ISNULL(SUM(Monto),0)
			FROM CuentasPorCobrarCobros WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar)
			+ @MontoTotalPagoEfectivo <= @MontoNuevo )
			SET @EsPosible = 1
		ELSE
			SET @EsPosible = 0
	END
	
	RETURN ISNULL(@EsPosible,0)
END
GO
