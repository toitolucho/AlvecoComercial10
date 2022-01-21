USE AlvecoComercial10
GO

DROP PROCEDURE ListarKardexProductoDetalladoReporte
GO

CREATE PROCEDURE ListarKardexProductoDetalladoReporte
	@NumeroAlmacen		INT,
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@ListadoProductos	VARCHAR(4000)
AS
BEGIN

IF(@ListadoProductos IS NULL)
	BEGIN
		SELECT	KADT.CodigoProducto, KADT.NombreProducto, 
				A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, A.CodigoProducto, '01/01/2000', @FechaHoraInicio)
				- DBO.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, A.CodigoProducto, '01/01/2000',GETDATE()) AS CantidadExistenciaInicial,
				A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, A.CodigoProducto, '01/01/2000', @FechaHoraInicio)
				- DBO.ObtenerMontoTotalValorado(@NumeroAlmacen, A.CodigoProducto, '01/01/2000',GETDATE()) AS PrecioTotalValoradoInicial,
				KADT.FechaHoraEntrega as FechaHoraEntrega,
				KADT.NumeroTransaccionReal as NumeroCompraProducto,
				CASE KADT.TipoMovimiento WHEN 'DV' THEN 'DEVOLUCION POR VENTA' WHEN 'TI' THEN AI.NombreAlmacen ELSE PRO.NombreRazonSocial END AS NombreRazonSocial, 
				CASE KADT.TipoMovimiento WHEN 'DC' THEN 'DEVOLUCION POR COMPRA' WHEN 'TE' THEN AE.NombreAlmacen ELSE U.NombreCliente END AS FuncionarioRecepcion,
				CASE KADT.TipoTransaccion WHEN 'I' THEN ABS(KADT.CantidadEntregada) ELSE NULL END as CantidadCompra,
				CASE KADT.TipoTransaccion WHEN 'I' THEN ABS(KADT.PrecioUnitario) ELSE NULL END as PrecioUnitarioCompra,
				CASE KADT.TipoTransaccion WHEN 'I' THEN ABS(KADT.PrecioUnitario * KADT.CantidadEntregada) ELSE NULL END as PrecioValoradoCompra,
				CASE KADT.TipoTransaccion WHEN 'E' THEN ABS(KADT.CantidadEntregada) ELSE NULL END as CantidadVenta,
				CASE KADT.TipoTransaccion WHEN 'E' THEN ABS(KADT.PrecioUnitario) ELSE NULL END as PrecioUnitarioVenta,
				CASE KADT.TipoTransaccion WHEN 'E' THEN ABS(KADT.PrecioUnitario * KADT.CantidadEntregada) ELSE NULL END as PrecioValoradoVenta,
				A.CantidadExistencia - dbo.ObtenerCantidadTotalValoradoInventario(@NumeroAlmacen, KADT.CodigoProducto, '01/01/2000', getdate()) + KADT.CantidadAcumulada AS CantidadExistenciaSaldo, 
				A.PrecioValoradoTotal - dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, KADT.CodigoProducto, '01/01/2000', getdate()) + KADT.PrecioTotalAcumulado AS PrecioTotalValoradoSaldo,
				KADT.TipoMovimiento
		FROM 
		KardexProductoDetalladoTotal KADT
		INNER JOIN InventariosProductos A
		ON A.CodigoProducto = KADT.CodigoProducto
		AND A.NumeroAlmacen = KADT.NumeroAlmacen		
		LEFT JOIN ComprasProductos IA
		ON KADT.NumeroTransaccionReal = IA.NumeroCompraProducto
		AND KADT.TipoTransaccion = 'I'
		LEFT JOIN VentasProductos SA
		ON KADT.NumeroTransaccionReal = SA.NumeroVentaProducto
		AND KADT.TipoTransaccion = 'E'
		LEFT JOIN dbo.TransferenciasProductos TPE
		ON KADT.NumeroTransaccionReal = TPE.NumeroTransferenciaProducto
		AND KADT.NumeroAlmacen = TPE.NumeroAlmacenEmisor
		--AND KADT.TipoTransaccion = 'TE' 
		LEFT JOIN dbo.TransferenciasProductos TPI
		ON KADT.NumeroTransaccionReal = TPI.NumeroTransferenciaProducto
		AND KADT.NumeroAlmacen = TPI.NumeroAlmacenRecepctor
		--AND KADT.TipoTransaccion = 'TI' 
		LEFT JOIN Proveedores PRO
		ON IA.CodigoProveedor = PRO.CodigoProveedor
		LEFT JOIN Clientes U
		ON SA.CodigoCliente = U.CodigoCliente
		LEFT JOIN Almacenes AE
		ON AE.NumeroAlmacen = TPE.NumeroAlmacenRecepctor
		LEFT JOIN Almacenes AI
		ON AI.NumeroAlmacen = TPI.NumeroAlmacenEmisor
		WHERE KADT.FechaHoraEntrega		
		BETWEEN CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraInicio,120),120)
		AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, CONVERT(VARCHAR(10),@FechaHoraFin,120),120)))
		AND KADT.NumeroAlmacen = @NumeroAlmacen 
		ORDER BY 2, KADT.FechaHoraEntrega
	END	
ELSE
	BEGIN
	DECLARE @FechaHoraInicioLiteral VARCHAR(10),
			@FechaFinLiteral VARCHAR(10),
			@NumeroAlmacenListeral	VARCHAR(10)
	
	SET @FechaHoraInicioLiteral = RTRIM(CAST(DATEPART(DAY,@FechaHoraInicio) AS CHAR(2)))+'/'
												  +	RTRIM(CAST(DATEPART(MONTH,@FechaHoraInicio) AS CHAR(2)))+'/'
												  +	RTRIM(CAST(DATEPART(YEAR,@FechaHoraInicio) AS CHAR(4)))
	SET @FechaFinLiteral = CONVERT(VARCHAR(20),@FechaHoraFin,120)
	SET @NumeroAlmacenListeral = CAST(@NumeroAlmacen AS VARCHAR(10))
	
	
	EXEC ('SELECT	KADT.CodigoProducto, KADT.NombreProducto, 
					A.CantidadExistencia + dbo.ObtenerCantidadTotalValoradoInventario('+@NumeroAlmacenListeral+', A.CodigoProducto, ''01/01/2000'', ''' + @FechaHoraInicioLiteral + ''')
					- DBO.ObtenerCantidadTotalValoradoInventario('+@NumeroAlmacenListeral+', A.CodigoProducto, ''01/01/2000'',GETDATE()) AS CantidadExistenciaInicial,					
					A.PrecioValoradoTotal + dbo.ObtenerMontoTotalValorado('+@NumeroAlmacenListeral+', A.CodigoProducto, ''01/01/2000'', ''' + @FechaHoraInicioLiteral + ''')
					- DBO.ObtenerMontoTotalValorado('+@NumeroAlmacenListeral+', A.CodigoProducto, ''01/01/2000'',GETDATE()) AS PrecioTotalValoradoInicial,
					KADT.FechaHoraEntrega as FechaHoraEntrega,
					KADT.NumeroTransaccionReal as NumeroCompraProducto,
					CASE KADT.TipoMovimiento WHEN ''DV'' THEN ''DEVOLUCION POR VENTA''  WHEN ''TI'' THEN AI.NombreAlmacen ELSE PRO.NombreRazonSocial END AS NombreRazonSocial, 
					CASE KADT.TipoMovimiento WHEN  ''DC'' THEN ''DEVOLUCION POR COMPRA''  WHEN ''TE'' THEN AE.NombreAlmacen ELSE U.NombreCliente END AS FuncionarioRecepcion,
					CASE KADT.TipoTransaccion WHEN ''I'' THEN ABS(KADT.CantidadEntregada) ELSE NULL END as CantidadCompra,
					CASE KADT.TipoTransaccion WHEN ''I'' THEN ABS(KADT.PrecioUnitario) ELSE NULL END as PrecioUnitarioCompra,
					CASE KADT.TipoTransaccion WHEN ''I'' THEN ABS(KADT.PrecioUnitario * KADT.CantidadEntregada) ELSE NULL END as PrecioValoradoCompra,
					CASE KADT.TipoTransaccion WHEN ''E'' THEN ABS(KADT.CantidadEntregada) ELSE NULL END as CantidadVenta,
					CASE KADT.TipoTransaccion WHEN ''E'' THEN ABS(KADT.PrecioUnitario) ELSE NULL END as PrecioUnitarioVenta,
					CASE KADT.TipoTransaccion WHEN ''E'' THEN ABS(KADT.PrecioUnitario * KADT.CantidadEntregada) ELSE NULL END as PrecioValoradoVenta,
					A.CantidadExistencia - dbo.ObtenerCantidadTotalValoradoInventario('+@NumeroAlmacenListeral+', KADT.CodigoProducto, ''01/01/2000'', getdate()) + KADT.CantidadAcumulada AS CantidadExistenciaSaldo, 
					A.PrecioValoradoTotal - dbo.ObtenerMontoTotalValorado('+@NumeroAlmacenListeral+', KADT.CodigoProducto, ''01/01/2000'', getdate()) + KADT.PrecioTotalAcumulado AS PrecioTotalValoradoSaldo,
					KADT.TipoMovimiento
			FROM 
			KardexProductoDetalladoTotal KADT
			INNER JOIN InventariosProductos A
			ON A.CodigoProducto = KADT.CodigoProducto
			AND A.NumeroAlmacen = KADT.NumeroAlmacen		
			LEFT JOIN ComprasProductos IA
			ON KADT.NumeroTransaccionReal = IA.NumeroCompraProducto
			AND KADT.TipoTransaccion = ''I''
			LEFT JOIN VentasProductos SA
			ON KADT.NumeroTransaccionReal = SA.NumeroVentaProducto
			AND KADT.TipoTransaccion = ''E''
			LEFT JOIN dbo.TransferenciasProductos TPE
			ON KADT.NumeroTransaccionReal = TPE.NumeroTransferenciaProducto			
			AND KADT.NumeroAlmacen = TPE.NumeroAlmacenEmisor
			LEFT JOIN dbo.TransferenciasProductos TPI
			ON KADT.NumeroTransaccionReal = TPI.NumeroTransferenciaProducto			
			AND KADT.NumeroAlmacen = TPI.NumeroAlmacenRecepctor
			LEFT JOIN Proveedores PRO
			ON IA.CodigoProveedor = PRO.CodigoProveedor
			LEFT JOIN Clientes U
			ON SA.CodigoCliente = U.CodigoCliente
			LEFT JOIN Almacenes AE
			ON AE.NumeroAlmacen = TPE.NumeroAlmacenRecepctor
			LEFT JOIN Almacenes AI
			ON AI.NumeroAlmacen = TPI.NumeroAlmacenEmisor
			WHERE KADT.FechaHoraEntrega
			BETWEEN ''' + @FechaHoraInicioLiteral + '''
			AND DATEADD(SECOND,-1, DATEADD(DAY,1, CONVERT(DATETIME, ''' + @FechaFinLiteral + ''',120)))
			AND KADT.CodigoProducto IN (' + @ListadoProductos + ') 
			AND KADT.NumeroAlmacen = '+@NumeroAlmacenListeral+' 
			ORDER BY 2, KADT.FechaHoraEntrega')
			
	END
END
GO

--declare @fechaFin DATETIME = GETDATE(),
--		@CodigoProducto CHAR(15) = '''39500-0038'''
--exec ListarKardexProductoDetalladoReporte 1, '04/08/2011', @fechaFin, @CodigoProducto
--exec ListarKardexProductoDetalladoReporte 1, '04/08/2011', '31/12/2012', null

--SELECT 100 + dbo.ObtenerCantidadTotalValoradoInventario(1, '39500-0038', '01/01/2000', '2011-08-04')
--				- DBO.ObtenerCantidadTotalValoradoInventario(1, '39500-0038', '01/01/2000',GETDATE()) AS CantidadExistenciaInicial
--2011-08-04

