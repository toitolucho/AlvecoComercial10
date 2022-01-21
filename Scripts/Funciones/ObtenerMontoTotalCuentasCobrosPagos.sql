USE AlvecoComercial10
GO

DROP FUNCTION ObtenerMontoTotalCuentasCobrosPagos
GO
--Funcion que retorna el Monto Total Acumulado de un Pago o un Cobro, o en Todo Caos el importe Parcial por una Compra
-- @CodigoTipoCuenta : 'P'->Cuentas Por Pagar , 'C'->Cuentas Por Pagar, 'E'->CompraCreditoEfectivo
-- 'T'->Total Cuenta Por Pagar, --'V'->Monto Total Venta
CREATE FUNCTION ObtenerMontoTotalCuentasCobrosPagos(
	@NumeroAlmacen INT, @NumeroCuenta INT, @CodigoTipoCuenta CHAR(1)
)
RETURNS DECIMAL(10,2)

AS
BEGIN
	DECLARE @MontoTotal	DECIMAL(10,2)
	IF(@CodigoTipoCuenta = 'P')
		SELECT @MontoTotal = SUM(CPPP.Monto)
		FROM CuentasPorPagarPagos CPPP
		INNER JOIN CuentasPorPagar CPP
		ON CPP.NumeroCuentaPorPagar = CPPP.NumeroCuentaPorPagar
		AND CPP.NumeroAlmacen = @NumeroAlmacen
		WHERE CPPP.NumeroCuentaPorPagar = @NumeroCuenta		
	ELSE IF(@CodigoTipoCuenta = 'C')
		SELECT @MontoTotal = SUM(CPCC.Monto)
		FROM CuentasPorCobrarCobros CPCC
		INNER JOIN CuentasPorCobrar CPC
		ON CPCC.NumeroCuentaPorCobrar = CPC.NumeroCuentaPorCobrar
		AND CPC.NumeroAlmacen = @NumeroAlmacen
		WHERE CPCC.NumeroCuentaPorCobrar = @NumeroCuenta		
	ELSE IF(@CodigoTipoCuenta = 'E')
		--SELECT @MontoTotal = SUM(CPPD.MontoTotalPago)
		--FROM ComprasProductosPagosDetalle CPPD
		--WHERE CPPD.NumeroAlmacen = @NumeroAlmacen
		--AND CPPD.NumeroCompraProducto = @NumeroCuenta
		SELECT @MontoTotal = CPPD.MontoTotalPagoEfectivo
		FROM ComprasProductos CPPD
		WHERE CPPD.NumeroAlmacen = @NumeroAlmacen
		AND CPPD.NumeroCompraProducto = @NumeroCuenta
	ELSE IF(@CodigoTipoCuenta = 'V')
		SELECT @MontoTotal = CPPD.MontoTotalPagoEfectivo
		FROM VentasProductos CPPD
		WHERE CPPD.NumeroAlmacen = @NumeroAlmacen
		AND CPPD.NumeroVentaProducto = @NumeroCuenta
	ELSE	
		SELECT @MontoTotal = Monto
		FROM CuentasPorPagar 
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroCuentaPorPagar = @NumeroCuenta
	RETURN ISNULL(@MontoTotal,0)
END
GO

select * from comprasproductos