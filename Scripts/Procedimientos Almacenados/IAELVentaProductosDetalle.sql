USE AlvecoComercial10
GO


DROP PROCEDURE InsertarVentasProductosDetalle
GO
CREATE PROCEDURE InsertarVentasProductosDetalle
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15),	
	@CantidadVenta			INT,
	@CantidadEntregada		INT,
	@PrecioUnitarioVenta	DECIMAL(10,2),
	@PorcentajeDescuento	DECIMAL(5,2),
	@TiempoGarantiaVenta	INT
AS
BEGIN
	INSERT INTO dbo.VentasProductosDetalle(NumeroAlmacen, NumeroVentaProducto, CodigoProducto, CantidadVenta, CantidadEntregada, PrecioUnitarioVenta, PorcentajeDescuento, TiempoGarantiaVenta)
	VALUES (@NumeroAlmacen, @NumeroVentaProducto, @CodigoProducto, @CantidadVenta, @CantidadEntregada, @PrecioUnitarioVenta, @PorcentajeDescuento, @TiempoGarantiaVenta)
END
GO


DROP PROCEDURE ActualizarVentasProductosDetalle
GO
CREATE PROCEDURE ActualizarVentasProductosDetalle
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15),	
	@CantidadVenta			INT,
	@CantidadEntregada		INT,
	@PrecioUnitarioVenta	DECIMAL(10,2),
	@PorcentajeDescuento	DECIMAL(5,2),
	@TiempoGarantiaVenta	INT
AS
BEGIN
	UPDATE 	dbo.VentasProductosDetalle
	SET					
		CantidadVenta		= @CantidadVenta,
		CantidadEntregada	= @CantidadEntregada,
		PrecioUnitarioVenta	= @PrecioUnitarioVenta,
		PorcentajeDescuento = @PorcentajeDescuento,
		TiempoGarantiaVenta = @TiempoGarantiaVenta
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	AND CodigoProducto = @CodigoProducto
	
END
GO



DROP PROCEDURE EliminarVentasProductosDetalle
GO
CREATE PROCEDURE EliminarVentasProductosDetalle
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15)
AS
BEGIN
	DELETE 
	FROM dbo.VentasProductosDetalle
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	AND CodigoProducto = @CodigoProducto
END
GO



DROP PROCEDURE ListarVentasProductosDetalle
GO
CREATE PROCEDURE ListarVentasProductosDetalle
AS
BEGIN
	SELECT NumeroAlmacen, NumeroVentaProducto, CodigoProducto, CantidadVenta, CantidadEntregada, PrecioUnitarioVenta, PorcentajeDescuento, TiempoGarantiaVenta
	FROM dbo.VentasProductosDetalle
	ORDER BY NumeroAlmacen, NumeroVentaProducto, CodigoProducto
END
GO



DROP PROCEDURE ObtenerVentasProductosDetalle
GO
CREATE PROCEDURE ObtenerVentasProductosDetalle
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoProducto			CHAR(15)
AS
BEGIN
	SELECT NumeroAlmacen, NumeroVentaProducto, CodigoProducto, CantidadVenta, CantidadEntregada, PrecioUnitarioVenta, PorcentajeDescuento, TiempoGarantiaVenta
	FROM dbo.VentasProductosDetalle
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	AND CodigoProducto = @CodigoProducto
END
GO

DROP PROCEDURE ListarVentasProductosDetalleParaMostrar
GO

CREATE PROCEDURE ListarVentasProductosDetalleParaMostrar
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT
AS
BEGIN
	SELECT SAD.CodigoProducto, dbo.ObtenerNombreProducto(SAD.CodigoProducto) as NombreProducto, 
	CantidadVenta, PrecioUnitarioVenta, 
	(CASE WHEN VP.CodigoEstadoVenta = 'F' AND VP.VentaParaDistribuir = 1 THEN CantidadEntregada ELSE CantidadVenta END )* PrecioUnitarioVenta as PrecioTotal, 
	TiempoGarantiaVenta, CantidadEntregada, AU.NombreUnidad, PM.NombreMarca, PorcentajeDescuento
	FROM dbo.VentasProductos VP
	INNER JOIN 	dbo.VentasProductosDetalle SAD
	ON VP.NumeroAlmacen = SAD.NumeroAlmacen
	AND VP.NumeroVentaProducto = SAD.NumeroVentaProducto
	INNER JOIN Productos A
	ON SAD.CodigoProducto = A.CodigoProducto
	INNER JOIN ProductosUnidades AU
	ON A.CodigoUnidadProducto = AU.CodigoUnidad
	INNER JOIN ProductosMarcas PM
	ON PM.CodigoMarca = A.CodigoMarcaProducto
	WHERE SAD.NumeroAlmacen = @NumeroAlmacen
	AND SAD.NumeroVentaProducto = @NumeroVentaProducto
END
GO