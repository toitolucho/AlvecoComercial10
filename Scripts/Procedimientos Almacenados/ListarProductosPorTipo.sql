USE AlvecoComercial10
GO

DROP PROCEDURE ListarProductosPorTipo
GO

CREATE PROCEDURE ListarProductosPorTipo
	@NumeroAlmacen		INT,
	@CantidadExistencia	INT
AS
BEGIN
	SELECT	PT.NombreProductoTipo, PTH.NombreProductoTipo AS NombreProductoTipoPadre, P.NombreProducto,
			P.CodigoProducto, PU.NombreUnidad, PM.NombreMarca, IA.CantidadExistencia,
			IA.PrecioUnitarioVentaPorMayor, IA.PrecioUnitarioVentaPorMenor
	FROM Productos P
	INNER JOIN ProductosTipos PT
	ON P.CodigoProductoTipo = PT.CodigoProductoTipo
	LEFT JOIN  ProductosTipos PTH
	ON PT.CodigoProductoTipoPadre = PTH.CodigoProductoTipo 
	INNER JOIN InventariosProductos IA
	ON P.CodigoProducto = IA.CodigoProducto
	INNER JOIN ProductosUnidades PU
	ON P.CodigoUnidadProducto= PU.CodigoUnidad
	INNER JOIN ProductosMarcas PM
	ON P.CodigoMarcaProducto = PM.CodigoMarca
	WHERE IA.NumeroAlmacen = @NumeroAlmacen
	AND IA.CantidadExistencia >= (CASE WHEN @CantidadExistencia IS NULL THEN -100 ELSE @CantidadExistencia END)
END
GO


--EXEC ListarProductosPorTipo 1, NULL

--select pt.NombreProductoTipo, pth.NombreProductoTipo as NombreProductoTipoPadre
--from ProductosTipos PT	
--	LEFT JOIN  ProductosTipos PTH
--	ON PT.CodigoProductoTipoPadre = PTH.CodigoProductoTipo 