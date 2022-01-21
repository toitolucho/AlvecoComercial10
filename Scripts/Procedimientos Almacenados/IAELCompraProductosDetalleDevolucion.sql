USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCompraProductoDetalleDevolucion
GO

CREATE PROCEDURE InsertarCompraProductoDetalleDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT,
	@CodigoProducto						CHAR(15),	
	@CantidadCompraDevolucion			INT,			
	@PrecioUnitarioDevolucion			DECIMAL(10,2)	
AS
BEGIN
	INSERT INTO dbo.ComprasProductosDevolucionesDetalle (NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion, CodigoProducto, CantidadCompraDevolucion, PrecioUnitarioDevolucion)
	VALUES (@NumeroAlmacenDevolucion, @NumeroCompraProductoDevolucion, @CodigoProducto, @CantidadCompraDevolucion, @PrecioUnitarioDevolucion)
END	
GO

DROP PROCEDURE ActualizarCompraProductoDetalleDevolucion
GO

CREATE PROCEDURE ActualizarCompraProductoDetalleDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT,
	@CodigoProducto						CHAR(15),	
	@CantidadCompraDevolucion			INT,	
	@PrecioUnitarioDevolucion			DECIMAL(10,2)	
AS
BEGIN
	UPDATE 	dbo.ComprasProductosDevolucionesDetalle
	SET					
		CantidadCompraDevolucion = @CantidadCompraDevolucion,
		PrecioUnitarioDevolucion = @PrecioUnitarioDevolucion		
	WHERE (NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion) 
	AND NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO

DROP PROCEDURE EliminarCompraProductoDetalleDevolucion
GO

CREATE PROCEDURE EliminarCompraProductoDetalleDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT
AS
BEGIN
	DELETE 
	FROM dbo.ComprasProductosDevolucionesDetalle
	WHERE (NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion)
	AND NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO

DROP PROCEDURE ListarComprasProductosDetalleDevolucion
GO

CREATE PROCEDURE ListarComprasProductosDetalleDevolucion
AS
BEGIN
	SELECT NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion, CodigoProducto, CantidadCompraDevolucion, PrecioUnitarioDevolucion
	FROM dbo.ComprasProductosDevolucionesDetalle
	ORDER BY NumeroAlmacenDevolucion
END
GO

DROP PROCEDURE ObtenerCompraProductoDetalleDevolucion
GO

CREATE PROCEDURE ObtenerCompraProductoDetalleDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT
AS
BEGIN
	SELECT NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion, CodigoProducto, CantidadCompraDevolucion, PrecioUnitarioDevolucion
	FROM dbo.ComprasProductosDevolucionesDetalle
	WHERE (NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion)
	AND NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO


DROP PROCEDURE ListarComprasProductosDevolucionDetalleParaMostrar
GO

CREATE PROCEDURE ListarComprasProductosDevolucionDetalleParaMostrar
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT
AS
BEGIN
	SELECT IAD.CodigoProducto, dbo.ObtenerNombreProducto(IAD.CodigoProducto) AS NombreProducto, 
	CantidadCompraDevolucion, PrecioUnitarioDevolucion,  CantidadCompraDevolucion * PrecioUnitarioDevolucion AS PrecioTotal, 
	NombreUnidad, PM.NombreMarca, 0 as TiempoGarantia
	FROM dbo.ComprasProductosDevolucionesDetalle IAD
	INNER JOIN Productos A
	ON A.CodigoProducto = IAD.CodigoProducto
	INNER JOIN ProductosUnidades AU
	ON A.CodigoUnidadProducto = AU.CodigoUnidad
	INNER JOIN ProductosMarcas PM
	ON A.CodigoMarcaProducto = PM.CodigoMarca
	WHERE (NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion)
	AND NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO