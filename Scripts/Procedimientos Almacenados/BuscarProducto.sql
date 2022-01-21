USE AlvecoComercial10
GO

DROP PROCEDURE BuscarProducto
GO

CREATE PROCEDURE BuscarProducto
	@TextoBusqueda		VARCHAR(250),
	@CantidadExistencia	INT,
	@CodigoProveedor	INT,
	@NumeroAlmacen		INT
	
AS
BEGIN
	SELECT P.*, PT.NombreProductoTipo, PM.NombreMarca, PU.NombreUnidad, 
			CASE P.CodigoTipoCalculoInventario WHEN 'P' THEN 'PEPS' WHEN 'U' THEN 'UEPS' WHEN 'O' THEN 'PONDERADO' END AS TipoCalculoInvenetario,
			IA.*
	FROM Productos P
	INNER JOIN ProductosTipos PT
	ON P.CodigoProductoTipo = PT.CodigoProductoTipo
	INNER JOIN ProductosMarcas PM
	ON P.CodigoMarcaProducto = PM.CodigoMarca
	INNER JOIN ProductosUnidades PU
	ON P.CodigoUnidadProducto = PU.CodigoUnidad
	INNER JOIN InventariosProductos IA
	ON P.CodigoProducto = IA.CodigoProducto
	AND IA.NumeroAlmacen = @NumeroAlmacen
	WHERE (P.NombreProducto LIKE '%' + @TextoBusqueda +'%'
	OR P.NombreProductoAlternativo LIKE '%' + @TextoBusqueda +'%'
	OR P.CodigoMarcaProducto LIKE '%' + @TextoBusqueda +'%'
	OR P.CodigoProducto LIKE '%' + @TextoBusqueda +'%'
	OR PM.NombreMarca LIKE '%' + @TextoBusqueda +'%'
	OR PT.NombreProductoTipo LIKE '%' + @TextoBusqueda +'%'
	OR PU.NombreUnidad LIKE '%' + @TextoBusqueda +'%')
	AND IA.CantidadExistencia >= @CantidadExistencia
	AND CAST(ISNULL(P.CodigoProveedor,'') AS VARCHAR(100)) LIKE 
	CASE WHEN @CodigoProveedor IS NULL THEN '%%' ELSE
	CAST(@CodigoProveedor AS VARCHAR(100)) END
	
END
GO
