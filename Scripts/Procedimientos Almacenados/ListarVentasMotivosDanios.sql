USE AlvecoComercial10
GO

DROP PROCEDURE ListarVentasMotivosDanios
GO

CREATE PROCEDURE ListarVentasMotivosDanios
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME

AS
BEGIN
	SELECT	VP.Numeroalmacen, VP.NumeroVentaProducto, 
			VPDE.CodigoProducto, dbo.ObtenerNombreProducto(VPDE.CodigoProducto) AS NombreProducto,
			VPD.PrecioUnitarioVenta, VPDE.CantidadEntregada, 
			VPDE.FechaHoraEntrega, Observaciones, 
			CASE CodigoMotivoVenta WHEN 'D' THEN 'DAÑADO' WHEN 'V' THEN 'VENCIDO' WHEN 'P' THEN 'PERDIDO' WHEN 'O' THEN 'OTROS' ELSE 'NO IDENTIFICADO' END AS Motivo,
			CodigoMotivoVenta			
	FROM dbo.VentasProductos VP
	INNER JOIN dbo.VentasProductosDetalle VPD
	ON VP.NumeroAlmacen = VPD.NumeroAlmacen
	AND VP.NumeroVentaProducto = VPD.NumeroVentaProducto
	INNER JOIN dbo.VentasProductosDetalleEntrega VPDE
	ON VPD.NumeroAlmacen = VPDE.NumeroAlmacen
	AND VPD.NumeroVentaProducto = VPDE.NumeroVentaProducto
	AND VPD.CodigoProducto = VPDE.CodigoProducto
	--WHERE VP.CodigoEstadoVenta IN ('F','D','X')	
	WHERE CodigoMotivoVenta NOT IN ('N')
	AND VPDE.FechaHoraEntrega
	BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
	AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))	
	AND  VP.NumeroAlmacen  = @NumeroAlmacen
END
