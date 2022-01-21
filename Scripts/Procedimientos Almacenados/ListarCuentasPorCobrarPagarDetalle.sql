USE AlvecoComercial10
GO


DROP PROCEDURE ListarCuentasPorCobrarPagarDetalle
GO

CREATE PROCEDURE ListarCuentasPorCobrarPagarDetalle
	@NumeroAlmacen		INT,
	@NumeroCuenta		INT,
	@EstadoCuenta		CHAR(1),
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@TipoCuenta			CHAR(1)
AS
BEGIN
	IF(@TipoCuenta = 'C')--CUENTAS POR COBRAR
	BEGIN
		IF(@FechaHoraInicio IS NOT NULL AND @FechaHoraFin IS NOT NULL)
		BEGIN
			SELECT	CPC.NumeroCuentaPorCobrar AS NumeroCuenta, CPC.FechaHoraRegistro, C.NumeroConcepto,
					CPC.FechaLimite, CPC.Observaciones, CLI.NombreCliente AS EntidadResponsable,
					CASE CPC.CodigoEstado WHEN 'D' THEN 'PENDIENTE' ELSE 'PAGADO' END AS Estado,
					CPC.CodigoEstado, C.Concepto, CPC.Monto AS MontoCuenta, 					
					DBO.ObtenerNombreCompleto(CPCC.DIUsuario) AS NombreCompletoUsuario,
					CPCC.FechaHoraCobro AS FechaAmortizacion, CPCC.Monto AS MontoAmortiguado					
			FROM CuentasPorCobrar CPC
			LEFT JOIN CuentasPorCobrarCobros CPCC			
			ON CPC.NumeroCuentaPorCobrar = CPCC.NumeroCuentaPorCobrar
			INNER JOIN Conceptos C
			ON CPC.NumeroConcepto = C.NumeroConcepto
			LEFT JOIN Clientes CLI
			ON CPC.CodigoCliente = CLI.CodigoCliente
			WHERE CPC.NumeroAlmacen = @NumeroAlmacen
			AND CAST(CPC.NumeroCuentaPorCobrar AS VARCHAR(100)) LIKE
			CASE WHEN @NumeroCuenta IS NULL THEN '%%' ELSE CAST(@NumeroCuenta AS VARCHAR(100)) END
			AND CPC.CodigoEstado LIKE CASE WHEN @EstadoCuenta IS NULL  THEN '%%' ELSE @EstadoCuenta END
			AND CPC.FechaHoraRegistro BETWEEN @FechaHoraInicio AND @FechaHoraFin
		END
		ELSE
		BEGIN
			SELECT	CPC.NumeroCuentaPorCobrar AS NumeroCuenta, CPC.FechaHoraRegistro, C.NumeroConcepto,
					CPC.FechaLimite, CPC.Observaciones, CLI.NombreCliente AS EntidadResponsable,
					CASE CPC.CodigoEstado WHEN 'D' THEN 'PENDIENTE' ELSE 'PAGADO' END AS Estado,
					CPC.CodigoEstado, C.Concepto, CPC.Monto AS MontoCuenta, 					
					DBO.ObtenerNombreCompleto(CPCC.DIUsuario) AS NombreCompletoUsuario,
					CPCC.FechaHoraCobro AS FechaAmortizacion, CPCC.Monto AS MontoAmortiguado					
			FROM CuentasPorCobrar CPC
			LEFT JOIN CuentasPorCobrarCobros CPCC			
			ON CPC.NumeroCuentaPorCobrar = CPCC.NumeroCuentaPorCobrar
			INNER JOIN Conceptos C
			ON CPC.NumeroConcepto = C.NumeroConcepto
			LEFT JOIN Clientes CLI
			ON CPC.CodigoCliente = CLI.CodigoCliente
			WHERE CPC.NumeroAlmacen = @NumeroAlmacen
			AND CAST(CPC.NumeroCuentaPorCobrar AS VARCHAR(100)) LIKE
			CASE WHEN @NumeroCuenta IS NULL THEN '%%' ELSE CAST(@NumeroCuenta AS VARCHAR(100)) END
			AND CPC.CodigoEstado LIKE CASE WHEN @EstadoCuenta IS NULL  THEN '%%' ELSE @EstadoCuenta END		
		END
	END
	ELSE--CUENTAS POR PAGAR
	BEGIN
		IF(@FechaHoraInicio IS NOT NULL AND @FechaHoraFin IS NOT NULL)
		BEGIN
			SELECT	CPC.NumeroCuentaPorPagar AS NumeroCuenta, CPC.FechaHoraRegistro, C.NumeroConcepto,
					CPC.FechaLimite, CPC.Observaciones, CLI.NombreRazonSocial AS EntidadResponsable,
					CASE CPC.CodigoEstado WHEN 'D' THEN 'PENDIENTE' ELSE 'PAGADO' END AS Estado,
					CPC.CodigoEstado, C.Concepto, CPC.Monto AS MontoCuenta, 					
					DBO.ObtenerNombreCompleto(CPCC.DIUsuario) AS NombreCompletoUsuario,
					CPCC.FechaHoraPago AS FechaAmortizacion, CPCC.Monto AS MontoAmortiguado					
			FROM CuentasPorPagar CPC
			LEFT JOIN CuentasPorPagarPagos CPCC			
			ON CPC.NumeroCuentaPorPagar = CPCC.NumeroCuentaPorPagar
			INNER JOIN Conceptos C
			ON CPC.NumeroConcepto = C.NumeroConcepto
			LEFT JOIN Proveedores CLI
			ON CPC.CodigoProveedor = CLI.CodigoProveedor
			WHERE CPC.NumeroAlmacen = @NumeroAlmacen
			AND CAST(CPC.NumeroCuentaPorPagar AS VARCHAR(100)) LIKE
			CASE WHEN @NumeroCuenta IS NULL THEN '%%' ELSE CAST(@NumeroCuenta AS VARCHAR(100)) END
			AND CPC.CodigoEstado LIKE CASE WHEN @EstadoCuenta IS NULL  THEN '%%' ELSE @EstadoCuenta END
			AND CPC.FechaHoraRegistro BETWEEN @FechaHoraInicio AND @FechaHoraFin
		END
		ELSE
		BEGIN
			SELECT	CPC.NumeroCuentaPorPagar AS NumeroCuenta, CPC.FechaHoraRegistro, C.NumeroConcepto,
					CPC.FechaLimite, CPC.Observaciones, CLI.NombreRazonSocial AS EntidadResponsable,
					CASE CPC.CodigoEstado WHEN 'D' THEN 'PENDIENTE' ELSE 'PAGADO' END AS Estado,
					CPC.CodigoEstado, C.Concepto, CPC.Monto AS MontoCuenta, 					
					DBO.ObtenerNombreCompleto(CPCC.DIUsuario) AS NombreCompletoUsuario,
					CPCC.FechaHoraPago AS FechaAmortizacion, CPCC.Monto AS MontoAmortiguado					
			FROM CuentasPorPagar CPC
			LEFT JOIN CuentasPorPagarPagos CPCC			
			ON CPC.NumeroCuentaPorPagar = CPCC.NumeroCuentaPorPagar
			INNER JOIN Conceptos C
			ON CPC.NumeroConcepto = C.NumeroConcepto
			LEFT JOIN Proveedores CLI
			ON CPC.CodigoProveedor = CLI.CodigoProveedor
			WHERE CPC.NumeroAlmacen = @NumeroAlmacen
			AND CAST(CPC.NumeroCuentaPorPagar AS VARCHAR(100)) LIKE
			CASE WHEN @NumeroCuenta IS NULL THEN '%%' ELSE CAST(@NumeroCuenta AS VARCHAR(100)) END
			AND CPC.CodigoEstado LIKE CASE WHEN @EstadoCuenta IS NULL  THEN '%%' ELSE @EstadoCuenta END
		END
	END
END
GO


--exec ListarCuentasPorCobrarPagarDetalle 1, null, NULL, NULL, NULL, 'P'