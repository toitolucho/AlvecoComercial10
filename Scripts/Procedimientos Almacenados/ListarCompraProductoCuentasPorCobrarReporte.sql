USE AlvecoComercial10
GO
DROP PROCEDURE ListarVentaProductoCuentasPorCobrarReporte
GO

CREATE PROCEDURE ListarVentaProductoCuentasPorCobrarReporte
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@FechaInicio			DATETIME,
	@FechaFin				DATETIME,
	@EstadoCuenta			CHAR(1)
AS
BEGIN
	DECLARE @MontoTotalVenta			DECIMAL(10,2),
			@CadenaMontoTotal			VARCHAR(255)
			
	
	IF(@FechaInicio IS NOT NULL AND @FechaFin IS NOT NULL)
	BEGIN
		SELECT  dbo.ObtenerNombreCompleto(VP.DIUsuario) as DatosUsuario, 
				VP.NumeroAlmacen AS NumeroAlmacen,VP.NumeroVentaProducto, VP.CodigoCliente, C.NombreCliente, VP.FechaHoraVenta,
				CASE (VP.NumeroCuentaPorCobrar) WHEN null THEN -1 ELSE VP.NumeroCuentaPorCobrar END AS NumeroCuentaPorCobrar,
				VP.Observaciones,
				C.NITCliente, CASE (C.CodigoTipoCliente) WHEN 'P' THEN 'PERSONA' WHEN 'E' THEN 'EMPRESA' END AS TipoCliente, 
				C.NombreRepresentante,
				'Bs' AS MascaraMoneda,
				'Bolivianos' AS NombreMoneda,
				CPP.Monto AS MontoCuentaPorCobrar,
				CASE WHEN CPP.CodigoEstado = 'C' THEN 'CANCELADO' ELSE 'PENDIENTE' END AS EstadoCuenta,
				CPP.CodigoEstado, 
				DBO.ObtenerMontoTotalCuentasCobrosPagos(VP.NumeroAlmacen, VP.NumeroVentaProducto, 'V') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(VP.NumeroAlmacen, VP.NumeroCuentaPorCobrar, 'C') AS MontoPagado,
				VP.MontoTotalVenta
		FROM VentasProductos VP 
		INNER JOIN Clientes C 
		ON C.CodigoCliente = VP.CodigoCliente
		INNER JOIN CuentasPorCobrar CPP
		ON VP.NumeroAlmacen = CPP.NumeroAlmacen
		AND VP.NumeroCuentaPorCobrar = CPP.NumeroCuentaPorCobrar
		WHERE VP.NumeroAlmacen = @NumeroAlmacen
		AND CAST(VP.NumeroVentaProducto AS VARCHAR(100))  like 
		CASE WHEN @NumeroVentaProducto IS NULL THEN '%%' 
		ELSE CAST(@NumeroVentaProducto AS VARCHAR(100))END
		AND CPP.CodigoEstado LIKE 
		CASE WHEN @EstadoCuenta IS NULL THEN '%%'
		ELSE @EstadoCuenta END
		AND CPP.FechaHoraRegistro BETWEEN @FechaInicio AND @FechaFin
	END
	ELSE	
	BEGIN
		
		SELECT  dbo.ObtenerNombreCompleto(VP.DIUsuario) as DatosUsuario, 
				VP.NumeroAlmacen AS NumeroAlmacen,VP.NumeroVentaProducto, VP.CodigoCliente, C.NombreCliente, VP.FechaHoraVenta,
				CASE (VP.NumeroCuentaPorCobrar) WHEN null THEN -1 ELSE VP.NumeroCuentaPorCobrar END AS NumeroCuentaPorCobrar,
				VP.Observaciones,
				C.NITCliente, CASE (C.CodigoTipoCliente) WHEN 'P' THEN 'PERSONA' WHEN 'E' THEN 'EMPRESA' END AS TipoCliente, 
				C.NombreRepresentante,
				'Bs' AS MascaraMoneda,
				'Bolivianos' AS NombreMoneda,
				CPP.Monto AS MontoCuentaPorCobrar,
				CASE WHEN CPP.CodigoEstado = 'C' THEN 'CANCELADO' ELSE 'PENDIENTE' END AS EstadoCuenta,
				CPP.CodigoEstado, 
				DBO.ObtenerMontoTotalCuentasCobrosPagos(VP.NumeroAlmacen, VP.NumeroVentaProducto, 'V') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(VP.NumeroAlmacen, VP.NumeroCuentaPorCobrar, 'C') AS MontoPagado,
				VP.MontoTotalVenta
		FROM VentasProductos VP 
		INNER JOIN Clientes C 
		ON C.CodigoCliente = VP.CodigoCliente
		INNER JOIN CuentasPorCobrar CPP
		ON VP.NumeroAlmacen = CPP.NumeroAlmacen
		AND VP.NumeroCuentaPorCobrar = CPP.NumeroCuentaPorCobrar
		WHERE VP.NumeroAlmacen = @NumeroAlmacen
		AND CAST(VP.NumeroVentaProducto AS VARCHAR(100))  like 
		CASE WHEN @NumeroVentaProducto IS NULL THEN '%%' 
		ELSE CAST(@NumeroVentaProducto AS VARCHAR(100))END
		AND CPP.CodigoEstado LIKE 
		CASE WHEN @EstadoCuenta IS NULL THEN '%%'
		ELSE @EstadoCuenta END

		
	END
END
GO

--EXEC ListarVentaProductoCuentasPorCobrarReporte 1, NULL, null, null, null
--select * from CuentasPorCobrar
--select * from VentasProductos
--where NumeroVentaProducto = 257
--update VentasProductos
--set CodigoEstadoVenta = 'X'
--where NumeroVentaProducto = 257