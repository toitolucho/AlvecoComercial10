USE AlvecoComercial10
GO


DROP PROCEDURE InsertarVentaProductoDevolucionDetalle
GO
CREATE PROCEDURE InsertarVentaProductoDevolucionDetalle
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT,
	@CodigoProducto						CHAR(15),
	@CantidadVentaDevolucion			INT,			
	@PrecioUnitarioDevolucion			DECIMAL(10,2)
AS
BEGIN
	INSERT INTO dbo.VentasProductosDevolucionesDetalle(NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion, CodigoProducto, CantidadVentaDevolucion, PrecioUnitarioDevolucion)
	VALUES (@NumeroAlmacenDevolucion, @NumeroVentaProductoDevolucion, @CodigoProducto, @CantidadVentaDevolucion, @PrecioUnitarioDevolucion)
END
GO


DROP PROCEDURE ActualizarVentaProductoDevolucionDetalle
GO
CREATE PROCEDURE ActualizarVentaProductoDevolucionDetalle
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT,
	@CodigoProducto						CHAR(15),
	@CantidadVentaDevolucion			INT,			
	@PrecioUnitarioDevolucion			DECIMAL(10,2)
AS
BEGIN
	UPDATE 	dbo.VentasProductosDevolucionesDetalle
	SET					
		CantidadVentaDevolucion	= @CantidadVentaDevolucion,
		PrecioUnitarioDevolucion= @PrecioUnitarioDevolucion
		
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
	AND CodigoProducto = @CodigoProducto
	
END
GO



DROP PROCEDURE EliminarVentaProductoDevolucionDetalle
GO
CREATE PROCEDURE EliminarVentaProductoDevolucionDetalle
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT,
	@CodigoProducto						CHAR(15)
AS
BEGIN
	DELETE 
	FROM dbo.VentasProductosDevolucionesDetalle
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
	AND CodigoProducto = @CodigoProducto
END
GO



DROP PROCEDURE ListarVentaProductoDevolucionDetalle
GO
CREATE PROCEDURE ListarVentaProductoDevolucionDetalle
AS
BEGIN
	SELECT NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion, CodigoProducto, CantidadVentaDevolucion, PrecioUnitarioDevolucion
	FROM dbo.VentasProductosDevolucionesDetalle
	ORDER BY NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion, CodigoProducto
END
GO



DROP PROCEDURE ObtenerVentaProductoDevolucionDetalle
GO
CREATE PROCEDURE ObtenerVentaProductoDevolucionDetalle
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT,
	@CodigoProducto						CHAR(15)
AS
BEGIN
	SELECT NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion, CodigoProducto, CantidadVentaDevolucion, PrecioUnitarioDevolucion
	FROM dbo.VentasProductosDevolucionesDetalle
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
	AND CodigoProducto = @CodigoProducto
END
GO

DROP PROCEDURE ListarVentaProductoDevolucionDetalleParaMostrar
GO

CREATE PROCEDURE ListarVentaProductoDevolucionDetalleParaMostrar
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT
AS
BEGIN
	SELECT SAD.CodigoProducto, dbo.ObtenerNombreProducto(SAD.CodigoProducto) as NombreProducto, 
	CantidadVentaDevolucion, PrecioUnitarioDevolucion, 
	CantidadVentaDevolucion * PrecioUnitarioDevolucion as PrecioTotal, 
	AU.NombreUnidad, PM.NombreMarca, 0 as TiempoGarantia
	FROM dbo.VentasProductosDevolucionesDetalle SAD
	INNER JOIN Productos A
	ON SAD.CodigoProducto = A.CodigoProducto
	INNER JOIN ProductosUnidades AU
	ON A.CodigoUnidadProducto = AU.CodigoUnidad
	INNER JOIN ProductosMarcas PM
	ON PM.CodigoMarca = A.CodigoMarcaProducto
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
END
GO