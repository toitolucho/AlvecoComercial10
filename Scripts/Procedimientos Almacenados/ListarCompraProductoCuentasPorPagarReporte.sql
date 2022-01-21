USE AlvecoComercial10
GO
DROP PROCEDURE ListarCompraProductoCuentasPorPagarReporte
GO

CREATE PROCEDURE ListarCompraProductoCuentasPorPagarReporte
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@FechaInicio			DATETIME,
	@FechaFin				DATETIME,
	@EstadoCuenta			CHAR(1)
AS
BEGIN
	DECLARE @MontoTotalCompra			DECIMAL(10,2),
			@CadenaMontoTotal			VARCHAR(255)
	
	
	SET @FechaInicio = DBO.FormatearFechaInicioFin(@FechaInicio,1)
	SET @FechaFin = DBO.FormatearFechaInicioFin(@FechaFin,0)
	
	IF(@FechaFin IS NULL AND @FechaInicio IS NULL)
	BEGIN
	
		SELECT  dbo.ObtenerNombreCompleto(CP.DIUsuario) as DatosUsuario, 
				CP.NumeroAlmacen AS NumeroAgencia, CP.NumeroCompraProducto, 
				CP.CodigoProveedor, P.NombreRazonSocial, CP.FechaHoraRegistro as Fecha,
				CASE (CP.NumeroCuentaPorPagar) WHEN null THEN -1 ELSE CP.NumeroCuentaPorPagar END AS NumeroCuentaPorPagar,
				CP.Observaciones,
				P.NITProveedor, CASE (P.CodigoTipoProveedor) WHEN 'P' THEN 'PERSONA' WHEN 'E' THEN 'EMPRESA' END AS TipoCliente, 
				P.NombreRepresentante,
				'Bs' AS MascaraMoneda,
				'Bolivianos' AS NombreMoneda,				
				CPP.Monto AS MontoCuentaPorPagar, 
				CASE WHEN CPP.CodigoEstado = 'P' THEN 'PAGADO' ELSE 'PENDIENTE' END AS EstadoCuenta,
				CPP.CodigoEstado, 
				DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, 'E') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, 'P') AS MontoPagado,
				CP.MontoTotalCompra, CPP.FechaLimite
				
		FROM ComprasProductos CP 
		INNER JOIN Proveedores P 
		ON P.CodigoProveedor = CP.CodigoProveedor
		LEFT JOIN CuentasPorPagar CPP
		ON CP.NumeroAlmacen = CPP.NumeroAlmacen
		AND CP.NumeroCuentaPorPagar = CPP.NumeroCuentaPorPagar
		WHERE CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND CP.CodigoTipoCompra = 'R'
		AND CAST(CP.NumeroCompraProducto AS VARCHAR(100))  LIKE 
		CASE WHEN @NumeroCompraProducto IS NULL THEN '%%' 
		ELSE CAST(@NumeroCompraProducto AS VARCHAR(100))END
		AND CPP.CodigoEstado LIKE 
		CASE WHEN @EstadoCuenta IS NULL THEN '%%'
		ELSE @EstadoCuenta END
	
	END
	ELSE
	BEGIN
		SELECT  dbo.ObtenerNombreCompleto(CP.DIUsuario) as DatosUsuario, 
				CP.NumeroAlmacen AS NumeroAgencia,CP.NumeroCompraProducto, CP.CodigoProveedor, P.NombreRazonSocial, CP.FechaHoraRegistro as Fecha,
				CASE (CP.NumeroCuentaPorPagar) WHEN null THEN -1 ELSE CP.NumeroCuentaPorPagar END AS NumeroCuentaPorPagar,
				CP.Observaciones,
				P.NITProveedor, CASE (P.CodigoTipoProveedor) WHEN 'P' THEN 'PERSONA' WHEN 'E' THEN 'EMPRESA' END AS TipoCliente, 
				P.NombreRepresentante,
				'Bs' AS MascaraMoneda,
				'Bolivianos' AS NombreMoneda,				
				CPP.Monto AS MontoCuentaPorPagar, 
				CASE WHEN CPP.CodigoEstado = 'P' THEN 'PAGADO' ELSE 'PENDIENTE' END AS EstadoCuenta,
				CPP.CodigoEstado, 
				DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, 'E') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, 'P') AS MontoPagado,
				CP.MontoTotalCompra	, CPP.FechaLimite			
		FROM ComprasProductos CP 
		INNER JOIN Proveedores P 
		ON P.CodigoProveedor = CP.CodigoProveedor
		LEFT JOIN CuentasPorPagar CPP
		ON CP.NumeroAlmacen = CPP.NumeroAlmacen
		AND CP.NumeroCuentaPorPagar = CPP.NumeroCuentaPorPagar
		WHERE CAST(CP.NumeroAlmacen AS VARCHAR(100)) LIKE CASE WHEN @NumeroAlmacen IS NULL THEN '%%'
		ELSE CAST(@NumeroAlmacen AS VARCHAR(100)) END
		AND CP.CodigoTipoCompra = 'R'
		AND CAST(CP.NumeroCompraProducto AS VARCHAR(100))  LIKE 
		CASE WHEN @NumeroCompraProducto IS NULL THEN '%%' 
		ELSE CAST(@NumeroCompraProducto AS VARCHAR(100))END
		AND CPP.CodigoEstado LIKE 
		CASE WHEN @EstadoCuenta IS NULL THEN '%%'
		ELSE @EstadoCuenta END
		AND CPP.FechaHoraRegistro BETWEEN @FechaInicio AND @FechaFin
	END
END
GO

--exec ListarCompraProductoCuentasPorPagarReporte null, null, null, null, null
