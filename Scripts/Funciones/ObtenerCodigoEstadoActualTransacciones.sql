USE AlvecoComercial10
GO

DROP FUNCTION ObtenerCodigoEstadoActualTransacciones
GO

CREATE FUNCTION ObtenerCodigoEstadoActualTransacciones (@NumeroAlmacen INT, @NumeroTransaccion INT, @TipoTransaccion CHAR(1))
RETURNS CHAR(1)
AS
BEGIN
	DECLARE @CodigoEstadoTransaccion CHAR(1)
	IF(@TipoTransaccion = 'V')
		SELECT @CodigoEstadoTransaccion = CodigoEstadoVenta
		FROM dbo.VentasProductos
		WHERE NumeroAlmacen =  @NumeroAlmacen AND NumeroVentaProducto = @NumeroTransaccion	
	IF(@TipoTransaccion = 'C')
		SELECT @CodigoEstadoTransaccion = CodigoEstadoCompra
		FROM dbo.ComprasProductos
		WHERE NumeroAlmacen =  @NumeroAlmacen AND NumeroCompraProducto = @NumeroTransaccion	
	--IF(@TipoTransaccion = 'T') --Cotizaciones
	--	SELECT @CodigoEstadoTransaccion = CodigoEstadoCotizacion
	--	FROM dbo.CotizacionVentasProductos	
	--	WHERE NumeroAlmacen =  @NumeroAlmacen AND NumeroCotizacionVentaProducto = @NumeroTransaccion	
	--IF(@TipoTransaccion = 'E') --Transferencias
	--	SELECT @CodigoEstadoTransaccion = CodigoEstadoTransferencia
	--	FROM dbo.TransferenciasProductos
	--	WHERE NumeroAlmacenEmisora =  @NumeroAlmacen AND NumeroTransferenciaProducto = @NumeroTransaccion	
	--IF(@TipoTransaccion = 'R') --Transferencias
	--	SELECT @CodigoEstadoTransaccion = CodigoEstadoTransferencia
	--	FROM dbo.TransferenciasProductos
	--	WHERE NumeroAlmacenRecepctora =  @NumeroAlmacen AND NumeroTransferenciaProducto = @NumeroTransaccion	
	--IF(@TipoTransaccion = 'S') --Servicios Por Ventas e Individuales
	--	SELECT @CodigoEstadoTransaccion = CodigoEstadoServicio
	--	FROM VentasServicios
	--	WHERE NumeroAlmacen = @NumeroAlmacen AND NumeroVentaServicio = @NumeroTransaccion	
   	RETURN(@CodigoEstadoTransaccion)
END
GO
