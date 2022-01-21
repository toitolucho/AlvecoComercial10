USE AlvecoComercial10


DROP PROC InsertarCuentaPorCobrar
GO

CREATE PROC InsertarCuentaPorCobrar
@NumeroAlmacen			INT,
@FechaHoraRegistro		DATETIME,
@NumeroConcepto			INT,
@CodigoCliente			INT,
@Monto					DECIMAL(10,2),
@FechaLimite			DATETIME,
@CodigoEstado			CHAR(1),
@Observaciones			TEXT,
@DIUsuario				CHAR(15)
AS
BEGIN
	INSERT INTO dbo.CuentasPorCobrar (NumeroAlmacen, FechaHoraRegistro, NumeroConcepto, CodigoCliente, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario)
	VALUES (@NumeroAlmacen, GETDATE(), @NumeroConcepto, @CodigoCliente, @Monto, @FechaLimite, @CodigoEstado, @Observaciones, @DIUsuario)
END
GO


DROP PROC ActualizarCuentaPorCobrar
GO

CREATE PROC ActualizarCuentaPorCobrar
@NumeroCuentaPorCobrar	INT,
@NumeroAlmacen			INT,
@FechaHoraRegistro		DATETIME,
@NumeroConcepto			INT,
@CodigoCliente			INT,
@Monto					DECIMAL(10,2),
@FechaLimite			DATETIME,
@CodigoEstado			CHAR(1),
@Observaciones			TEXT,
@DIUsuario				CHAR(15)
AS
BEGIN
	IF(DBO.ObtenerMontoAmortiguadoCuentasPorPagarCobrar(@NumeroAlmacen,@NumeroCuentaPorCobrar, 'C') <= @Monto )	
	BEGIN
		UPDATE dbo.CuentasPorCobrar
		SET	NumeroAlmacen = @NumeroAlmacen,
			NumeroConcepto = @NumeroConcepto,
			CodigoCliente = @CodigoCliente,
			Monto = @Monto,		
			FechaLimite = @FechaLimite,
			CodigoEstado = @CodigoEstado,
			Observaciones = @Observaciones,
			DIUsuario = @DIUsuario
		WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
	END
	ELSE
		RAISERROR('No puede modificar el monto a uno inferior a la suma de cobros ya realizados',17,2)
END
GO



DROP PROC EliminarCuentaPorCobrar
GO

CREATE PROC EliminarCuentaPorCobrar
@NumeroCuentaPorCobrar		INT
AS
BEGIN
	DELETE FROM dbo.CuentasPorCobrar 
	WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
END
GO



DROP PROC ListarCuentasPorCobrar
GO

CREATE PROC ListarCuentasPorCobrar
AS
BEGIN
	SELECT NumeroCuentaPorCobrar, NumeroAlmacen, FechaHoraRegistro, NumeroConcepto, CodigoCliente, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario
	FROM dbo.CuentasPorCobrar
END
GO


/*
DROP PROC ListarCuentasPorPagarEstado
GO

CREATE PROC ListarCuentasPorPagarEstado
@Estado		VARCHAR(9)
AS
BEGIN
	SELECT NumeroCuentaPorPagar, NumeroAlmacen, FechaHoraRegistro, CodigoCliente, CodigoMoneda, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario, NumeroAsiento
	FROM dbo.CuentasPorPagar
	WHERE CodigoEstado = @Estado
END
GO
*/


DROP PROC ObtenerCuentaPorCobrar
GO

CREATE PROC ObtenerCuentaPorCobrar
@NumeroCuentaPorCobrar	INT
AS
BEGIN
	SELECT NumeroCuentaPorCobrar, NumeroAlmacen, FechaHoraRegistro, NumeroConcepto, CodigoCliente, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario
	FROM dbo.CuentasPorCobrar
	WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
END
GO


DROP PROC ListarCuentasPorCobrarBusqueda
GO

CREATE PROC ListarCuentasPorCobrarBusqueda
@Palabra	VARCHAR(128),
@Fecha1		DATETIME,
@Fecha2		DATETIME, 
@Estado1	CHAR(1),
@Estado2	CHAR(1)
AS
BEGIN
	SELECT CPC.NumeroCuentaPorCobrar , A.NumeroAlmacen, A.NombreAgencia AS 'Nombre Agencia', CPC.FechaHoraRegistro AS 'Fecha/Hora de Registro', C.NumeroConcepto, C.Concepto, P.CodigoCliente, P.NombreRazonSocial AS 'Proveedor', M.CodigoMoneda, M.NombreMoneda AS 'Moneda', CPC.Monto, CPC.FechaLimite AS 'Fecha Límite', CASE CPC.CodigoEstado WHEN 'P' THEN 'Pendiente' WHEN 'C' THEN 'Cancelado' END AS 'Estado', CPC.Observaciones, CPC.DIUsuario, CPC.NumeroAsiento
	FROM dbo.CuentasPorCobrar CPC 
	INNER JOIN Clientes P 
	ON CPC.CodigoCliente = P.CodigoCliente
	INNER JOIN Monedas M 
	ON CPC.CodigoMoneda = M.CodigoMoneda
	INNER JOIN Agencias A 
	ON CPC.NumeroAlmacen = A.NumeroAlmacen
	INNER JOIN Conceptos C 
	ON CPC.NumeroConcepto = C.NumeroConcepto
	WHERE ((CONVERT(VARCHAR(128), CPC.NumeroCuentaPorCobrar) LIKE '%'+@Palabra+'%') 
		  OR (CONVERT(VARCHAR(128), A.NombreAgencia) LIKE '%'+@Palabra+'%') OR (P.NombreRazonSocial LIKE '%'+@Palabra+'%') 
		  OR (CONVERT(VARCHAR(128), CPC.Observaciones) LIKE '%'+@Palabra+'%') OR (C.Concepto LIKE '%'+@Palabra+'%') 
		  OR (CONVERT(VARCHAR(128), CPC.Monto) LIKE '%'+@Palabra+'%')) 
		  AND ((CONVERT(DATETIME, CONVERT(VARCHAR(10), CPC.FechaHoraRegistro, 21), 21) 
		  BETWEEN CONVERT(DATETIME, @Fecha1, 21) AND CONVERT(DATETIME, @Fecha2, 21))) AND ((CPC.CodigoEstado = @Estado1) OR (CPC.CodigoEstado = @Estado2)) 
		  OR (CPC.NumeroCuentaPorCobrar = (SELECT MAX(CPD.NumeroCuentaPorCobrar) FROM dbo.CuentasPorCobrarCobros CPD WHERE (CONVERT(DATETIME, CONVERT(VARCHAR(10), CPD.FechaHoraCobro, 21), 21) BETWEEN CONVERT(DATETIME, @Fecha1, 21) AND (CONVERT(DATETIME, @Fecha2, 21))) AND (CONVERT(VARCHAR(128),CPD.Monto) LIKE '%'+@Palabra+'%')))			
			
END
GO

DROP PROC ReporteCuentasPorCobrar
GO
CREATE PROC ReporteCuentasPorCobrar
AS
BEGIN
	SELECT CPC.NumeroCuentaPorCobrar , A.NumeroAlmacen, A.NombreAgencia, CPC.FechaHoraRegistro, C.NumeroConcepto, C.Concepto, P.CodigoCliente, P.NombreRazonSocial, M.CodigoMoneda, M.NombreMoneda, CPC.Monto, CPC.FechaLimite, CASE CPC.CodigoEstado WHEN 'P' THEN 'Pendiente' WHEN 'C' THEN 'Cancelado' END AS 'Estado', 
			CC.FechaHoraCobro, CC.Monto AS 'Monto Cobro',
			CPC.Observaciones, CC.DIUsuario, U.Nombres, U.Paterno, U.Materno
	FROM dbo.CuentasPorCobrar CPC 
	INNER JOIN Clientes P 
	ON CPC.CodigoCliente = P.CodigoCliente
	INNER JOIN Monedas M 
	ON CPC.CodigoMoneda = M.CodigoMoneda
	INNER JOIN Agencias A 
	ON CPC.NumeroAlmacen = A.NumeroAlmacen
	INNER JOIN Conceptos C 
	ON CPC.NumeroConcepto = C.NumeroConcepto
	INNER JOIN CuentasPorCobrarCobros CC 
	ON CPC.NumeroCuentaPorCobrar = CC.NumeroCuentaPorCobrar
	INNER JOIN Usuarios U 
	ON CC.DIUsuario = U.DIUsuario
	--GROUP BY CPC.NumeroCuentaPorCobrar , A.NumeroAlmacen, A.NombreAgencia, CPC.FechaHoraRegistro, C.NumeroConcepto, C.Concepto, P.CodigoCliente, P.NombreRazonSocial, M.CodigoMoneda, M.NombreMoneda, CPC.Monto, CPC.FechaLimite, CPC.CodigoEstado, CPC.Observaciones, CC.DIUsuario, U.Nombres, U.Paterno, U.Materno
END
GO

DROP PROC ReporteCuentasPorCobrarNumero
GO
CREATE PROC ReporteCuentasPorCobrarNumero
@NumeroCuentaPorCobrar	INT
AS
BEGIN
	SELECT CPC.NumeroCuentaPorCobrar , A.NumeroAlmacen, A.NombreAgencia, CPC.FechaHoraRegistro, C.NumeroConcepto, C.Concepto, P.CodigoCliente, P.NombreRazonSocial, M.CodigoMoneda, M.NombreMoneda, CPC.Monto, CPC.FechaLimite, CASE CPC.CodigoEstado WHEN 'P' THEN 'Pendiente' WHEN 'C' THEN 'Cancelado' END AS 'Estado', 
			CC.FechaHoraCobro, CC.Monto AS 'Monto Cobro',
			CPC.Observaciones, CC.DIUsuario, U.Nombres, U.Paterno, U.Materno
	FROM dbo.CuentasPorCobrar CPC 
	INNER JOIN Clientes P 
	ON CPC.CodigoCliente = P.CodigoCliente
	INNER JOIN Monedas M 
	ON CPC.CodigoMoneda = M.CodigoMoneda
	INNER JOIN Agencias A 
	ON CPC.NumeroAlmacen = A.NumeroAlmacen
	INNER JOIN Conceptos C 
	ON CPC.NumeroConcepto = C.NumeroConcepto
	INNER JOIN CuentasPorCobrarCobros CC 
	ON CPC.NumeroCuentaPorCobrar = CC.NumeroCuentaPorCobrar
	INNER JOIN Usuarios U 
	ON CC.DIUsuario = U.DIUsuario
	WHERE CPC.NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
	--GROUP BY CPC.NumeroCuentaPorCobrar , A.NumeroAlmacen, A.NombreAgencia, CPC.FechaHoraRegistro, C.NumeroConcepto, C.Concepto, P.CodigoCliente, P.NombreRazonSocial, M.CodigoMoneda, M.NombreMoneda, CPC.Monto, CPC.FechaLimite, CPC.CodigoEstado, CPC.Observaciones, CC.DIUsuario, U.Nombres, U.Paterno, U.Materno
END
GO













