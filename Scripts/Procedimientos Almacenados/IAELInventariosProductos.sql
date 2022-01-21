USE AlvecoComercial10
GO


DROP PROCEDURE InsertarInventarioProducto
GO
CREATE PROCEDURE InsertarInventarioProducto	
	@NumeroAlmacen						INT,
	@CodigoProducto						CHAR(15),
	@CantidadExistencia					INT,
	@CantidadRequerida					INT,
	@PrecioUnitarioCompra				DECIMAL(10,2),
	@PrecioUnitarioVentaPorMayor		DECIMAL(10,2),
	@PrecioUnitarioVentaPorMenor		DECIMAL(10,2),
	@PorcentajeGananciaVentaPorMayor	DECIMAL(10,2),
	@PorcentajeGananciaVentaPorMenor	DECIMAL(10,2),
	@TiempoGarantiaProducto				INT,
	@StockMinimo						INT
AS
BEGIN
	INSERT INTO dbo.InventariosProductos(NumeroAlmacen, CodigoProducto, CantidadExistencia, CantidadRequerida, PrecioUnitarioCompra, PrecioUnitarioVentaPorMayor, PrecioUnitarioVentaPorMenor, PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor, TiempoGarantiaProducto, StockMinimo)
	VALUES (@NumeroAlmacen, @CodigoProducto, @CantidadExistencia, @CantidadRequerida, @PrecioUnitarioCompra, @PrecioUnitarioVentaPorMayor, @PrecioUnitarioVentaPorMenor, @PorcentajeGananciaVentaPorMayor, @PorcentajeGananciaVentaPorMenor, @TiempoGarantiaProducto, @StockMinimo)
END
GO


DROP PROCEDURE ActualizarInventarioProducto
GO
CREATE PROCEDURE ActualizarInventarioProducto
	@NumeroAlmacen						INT,
	@CodigoProducto						CHAR(15),
	@CantidadExistencia					INT,
	@CantidadRequerida					INT,
	@PrecioUnitarioCompra				DECIMAL(10,2),
	@PrecioUnitarioVentaPorMayor		DECIMAL(10,2),
	@PrecioUnitarioVentaPorMenor		DECIMAL(10,2),
	@PorcentajeGananciaVentaPorMayor	DECIMAL(10,2),
	@PorcentajeGananciaVentaPorMenor	DECIMAL(10,2),
	@TiempoGarantiaProducto				INT,
	@StockMinimo						INT
AS
BEGIN
	UPDATE 	dbo.InventariosProductos
	SET		
		NumeroAlmacen					= @NumeroAlmacen,
		CodigoProducto					= @CodigoProducto,
		CantidadExistencia				= @CantidadExistencia,
		CantidadRequerida				= @CantidadRequerida,
		PrecioUnitarioCompra			= @PrecioUnitarioCompra,
		PrecioUnitarioVentaPorMayor		= @PrecioUnitarioVentaPorMayor,	
		PrecioUnitarioVentaPorMenor		= @PrecioUnitarioVentaPorMenor,
		PorcentajeGananciaVentaPorMayor	= @PorcentajeGananciaVentaPorMayor,
		PorcentajeGananciaVentaPorMenor	= @PorcentajeGananciaVentaPorMenor,		
		TiempoGarantiaProducto			= @TiempoGarantiaProducto,
		StockMinimo						= @StockMinimo
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND CodigoProducto = @CodigoProducto
END
GO



DROP PROCEDURE EliminarInventarioProducto
GO
CREATE PROCEDURE EliminarInventarioProducto
	@NumeroAlmacen						INT,
	@CodigoProducto						CHAR(15)
AS
BEGIN
	DELETE 
	FROM dbo.InventariosProductos
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND CodigoProducto = @CodigoProducto
END
GO



DROP PROCEDURE ListarInventariosProductos
GO
CREATE PROCEDURE ListarInventariosProductos
AS
BEGIN
	SELECT	NumeroAlmacen, CodigoProducto, CantidadExistencia, CantidadRequerida, PrecioUnitarioCompra, 
			PrecioUnitarioVentaPorMayor, PrecioUnitarioVentaPorMenor, PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor,
			TiempoGarantiaProducto, StockMinimo
	FROM dbo.InventariosProductos
	ORDER BY NumeroAlmacen, CodigoProducto
END
GO



DROP PROCEDURE ObtenerInventarioProducto
GO
CREATE PROCEDURE ObtenerInventarioProducto
	@NumeroAlmacen						INT,
	@CodigoProducto						CHAR(15)
AS
BEGIN
	SELECT	NumeroAlmacen, CodigoProducto, CantidadExistencia, CantidadRequerida, PrecioUnitarioCompra, 
			PrecioUnitarioVentaPorMayor, PrecioUnitarioVentaPorMenor, PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor,
			TiempoGarantiaProducto, StockMinimo
	FROM dbo.InventariosProductos
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND CodigoProducto = @CodigoProducto
END
GO


DROP PROCEDURE ListarInventarioProductosReportes
GO

CREATE PROCEDURE ListarInventarioProductosReportes
	@NumeroAlmacen	INT
AS 
BEGIN
	SELECT IA.NumeroAlmacen, A.CodigoProducto, A.NombreProducto, U.NombreUnidad, 
	--CASE WHEN dbo.ObtenerCodigoTipoCalculoInventarioProducto(IA.CodigoProducto) IN ('P','U') AND IACT.CantidadExistente IS NOT NULL
	--THEN IACT.CantidadExistente WHEN dbo.ObtenerCodigoTipoCalculoInventarioProducto(IA.CodigoProducto) NOT IN ('P','U')
	--THEN IA.CantidadExistencia ELSE IA.CantidadExistencia END AS CantidadExistencia, 
	IA.CantidadExistencia,
	IA.CantidadRequerida, IA.StockMinimo, 
	--CASE WHEN dbo.ObtenerCodigoTipoCalculoInventarioProducto(IA.CodigoProducto) IN ('P','U') AND IACT.PrecioUnitario IS NOT NULL
	--THEN IACT.PrecioUnitario WHEN dbo.ObtenerCodigoTipoCalculoInventarioProducto(IA.CodigoProducto) NOT IN ('P','U')
	--THEN IA.PrecioUnitarioCompra ELSE IA.PrecioUnitarioCompra END AS PrecioUnitarioCompra 
	IA.PrecioUnitarioCompra	 
	FROM InventariosProductos IA
	INNER JOIN Productos A	
	ON IA.CodigoProducto = A.CodigoProducto
	--AND IA.NumeroAlmacen = @NumeroAlmacen
	INNER JOIN ProductosUnidades U
	ON A.CodigoUnidadProducto = U.CodigoUnidad
	--INNER JOIN InventariosProductosCantidadesTransaccionesHistorial IACT
	--ON IA.CodigoProducto = IACT.CodigoProducto
	--AND IA.NumeroAlmacen= IACT.NumeroAlmacen
	----AND IACT.NumeroAlmacen =@NumeroAlmacen
	WHERE CAST(IA.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
	ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
	ORDER BY 2
END
GO

DROP PROCEDURE ListarInventarioProductosRequeridosReportes
GO

CREATE PROCEDURE ListarInventarioProductosRequeridosReportes
	@NumeroAlmacen	INT
AS 
BEGIN
	SELECT A.CodigoProducto, A.NombreProducto as NombreArticulo1, U.NombreUnidad, IA.CantidadExistencia, IA.CantidadRequerida, IA.StockMinimo, IA.PrecioUnitarioCompra
	FROM InventariosProductos IA
	INNER JOIN Productos A
	ON IA.CodigoProducto = A.CodigoProducto
	INNER JOIN ProductosUnidades U
	ON A.CodigoUnidadProducto = U.CodigoUnidad
	WHERE CAST(IA.NumeroAlmacen AS VARCHAR(10)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%' ELSE CAST(@NumeroAlmacen as VARCHAR(10)) END
	AND (IA.StockMinimo > IA.CantidadExistencia
	or IA.CantidadRequerida > 0)
	ORDER BY 2
END
GO

--exec ListarInventarioProductosRequeridosReportes 1

