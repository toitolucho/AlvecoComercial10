USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCuentaPorPagar
GO

CREATE PROCEDURE InsertarCuentaPorPagar
@NumeroAlmacen			INT,
@FechaHoraRegistro		DATETIME,
@NumeroConcepto			INT,
@CodigoProveedor		INT,
@Monto					DECIMAL(10,2),
@FechaLimite			DATETIME,
@CodigoEstado			CHAR(1),
@Observaciones			TEXT,
@DIUsuario				CHAR(15)
AS
BEGIN
	INSERT INTO CuentasPorPagar(NumeroAlmacen, FechaHoraRegistro, CodigoProveedor, NumeroConcepto, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario)
	VALUES (@NumeroAlmacen, GETDATE(), @CodigoProveedor, @NumeroConcepto, @Monto, @FechaLimite, @CodigoEstado, @Observaciones, @DIUsuario)
END
GO


DROP PROC ActualizarCuentaPorPagar
GO

CREATE PROC ActualizarCuentaPorPagar
@NumeroCuentaPorPagar	INT,
@NumeroAlmacen			INT,
@FechaHoraRegistro		DATETIME,
@NumeroConcepto			INT,
@CodigoProveedor		INT,
@Monto					DECIMAL(10,2),
@FechaLimite			DATETIME,
@CodigoEstado			CHAR(1),
@Observaciones			TEXT,
@DIUsuario				CHAR(15)
AS
BEGIN
	IF(DBO.ObtenerMontoAmortiguadoCuentasPorPagarCobrar(@NumeroAlmacen,@NumeroCuentaPorPagar, 'P') <= @Monto )	
	BEGIN
		UPDATE dbo.CuentasPorPagar
		SET	NumeroAlmacen		= @NumeroAlmacen,
			FechaHoraRegistro	= @FechaHoraRegistro,
			NumeroConcepto		= @NumeroConcepto,
			CodigoProveedor		= @CodigoProveedor,
			Monto				= @Monto,		
			FechaLimite			= @FechaLimite,
			CodigoEstado		= @CodigoEstado,
			Observaciones		= @Observaciones,
			DIUsuario			= @DIUsuario
		WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
	END
	ELSE
		RAISERROR('No puede modificar el monto a uno inferior a la suma de PAGOS ya realizados',17,2)
END
GO



DROP PROC EliminarCuentaPorPagar
GO

CREATE PROC EliminarCuentaPorPagar
@NumeroCuentaPorPagar		INT
AS
BEGIN
	
	BEGIN TRANSACTION	
	
		IF(NOT EXISTS (SELECT *
			FROM ComprasProductos 
			WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar))
		BEGIN
			DELETE FROM DBO.CuentasPorPagarPagos
			WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
			
			DELETE FROM dbo.CuentasPorPagar 
			WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
		END	
		ELSE
			RAISERROR('No se pudo Eliminar la Cuenta Por Pagar, seguramente corresponde a una transacción de Compra',2,16)	
	
	IF(@@ERROR <> 0)
	BEGIN
		RAISERROR('No se Puede Eliminar el Registro Actual ya que existen referencias sobre la misma o esta en uso',2,16)	
		ROLLBACK TRAN
	END
	ELSE
		COMMIT TRANSACTION
	
END
GO



DROP PROC ListarCuentasPorPagar
GO

CREATE PROC ListarCuentasPorPagar
AS
BEGIN
	SELECT NumeroCuentaPorPagar, NumeroAlmacen, FechaHoraRegistro, NumeroConcepto, CodigoProveedor, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario
	FROM dbo.CuentasPorPagar
END
GO


/*
DROP PROC ListarCuentasPorPagarEstado
GO

CREATE PROC ListarCuentasPorPagarEstado
@Estado		VARCHAR(9)
AS
BEGIN
	SELECT NumeroCuentaPorPagar, NumeroAlmacen, FechaHoraRegistro, CodigoProveedor, CodigoMoneda, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario, NumeroAsiento
	FROM dbo.CuentasPorPagar
	WHERE CodigoEstado = @Estado
END
GO
*/


DROP PROC ObtenerCuentaPorPagar
GO

CREATE PROC ObtenerCuentaPorPagar
@NumeroCuentaPorPagar	INT
AS
BEGIN
	SELECT NumeroCuentaPorPagar, NumeroAlmacen, FechaHoraRegistro, NumeroConcepto, CodigoProveedor, Monto, FechaLimite, CodigoEstado, Observaciones, DIUsuario
	FROM dbo.CuentasPorPagar
	WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
END
GO


DROP PROC ListarCuentasPorPagarBusqueda
GO

CREATE PROC ListarCuentasPorPagarBusqueda
@Palabra	VARCHAR(128),
@Fecha1		DATETIME,
@Fecha2		DATETIME, 
@Estado1	CHAR(1),
@Estado2	CHAR(1)
AS
BEGIN
	SELECT	CPP.NumeroCuentaPorPagar, CPP.FechaHoraRegistro AS 'Fecha/Hora de Registro', 
			C.NumeroConcepto, C.Concepto, P.CodigoProveedor, P.NombreRazonSocial AS 'Proveedor', 
			CPP.Monto, CASE WHEN CPP.FechaLimite IS NULL THEN GETDATE() ELSE CPP.FechaLimite END AS 'Fecha Límite', 
			CASE CPP.CodigoEstado WHEN 'P' THEN 'Pendiente' WHEN 'C' THEN 'Cancelado' END AS 'Estado', 
			CPP.Observaciones, CPP.DIUsuario, P.NombreRazonSocial
	FROM dbo.CuentasPorPagar CPP INNER JOIN Proveedores P ON CPP.CodigoProveedor = P.CodigoProveedor
	INNER JOIN Conceptos C 
	ON CPP.NumeroConcepto = C.NumeroConcepto
	WHERE ((CONVERT(VARCHAR(128), CPP.NumeroCuentaPorPagar) LIKE '%'+@Palabra+'%') OR (P.NombreRazonSocial LIKE '%'+@Palabra+'%') OR
			(CONVERT(VARCHAR(128), CPP.Observaciones) LIKE '%'+@Palabra+'%') OR (C.Concepto LIKE '%'+@Palabra+'%') OR (CONVERT(VARCHAR(128), CPP.Monto) LIKE '%'+@Palabra+'%')) AND 
			((CONVERT(DATETIME, CONVERT(VARCHAR(10), CPP.FechaHoraRegistro, 21), 21) BETWEEN CONVERT(DATETIME, @Fecha1, 21) AND CONVERT(DATETIME, @Fecha2, 21))) AND ((CPP.CodigoEstado = @Estado1) OR (CPP.CodigoEstado = @Estado2)) OR
			(CPP.NumeroCuentaPorPagar = (SELECT MAX(CPD.NumeroCuentaPorPagar) FROM dbo.CuentasPorPagarPagos CPD WHERE (CONVERT(DATETIME, CONVERT(VARCHAR(10), CPD.FechaHoraPago, 21), 21) BETWEEN CONVERT(DATETIME, @Fecha1, 21) AND (CONVERT(DATETIME, @Fecha2, 21))) AND (CONVERT(VARCHAR(128),CPD.Monto) LIKE '%'+@Palabra+'%')))			
END
GO

 

DROP PROC ReporteCuentasPorPagarNumero
GO
CREATE PROC ReporteCuentasPorPagarNumero
@NumeroCuentaPorPagar	INT
AS
BEGIN
	SELECT	CPC.NumeroCuentaPorPagar, CPC.FechaHoraRegistro, C.NumeroConcepto, C.Concepto, 
			P.CodigoProveedor, P.NombreRazonSocial, CPC.Monto, CPC.FechaLimite, 
			CASE CPC.CodigoEstado WHEN 'P' THEN 'Pendiente' WHEN 'C' THEN 'Cancelado' END AS 'Estado', 
			CC.FechaHoraPago, CC.Monto AS 'Monto Pago',
			CPC.Observaciones, CC.DIUsuario, dbo.ObtenerNombreCompleto(u.DIUsuario) as NombreCompleto
	FROM dbo.CuentasPorPagar CPC 
	INNER JOIN Proveedores P 
	ON CPC.CodigoProveedor = P.CodigoProveedor
	INNER JOIN Conceptos C 
	ON CPC.NumeroConcepto = C.NumeroConcepto
	INNER JOIN CuentasPorPagarPagos CC 
	ON CPC.NumeroCuentaPorPagar = CC.NumeroCuentaPorPagar
	INNER JOIN Usuarios U 
	ON CC.DIUsuario = U.DIUsuario
	WHERE CPC.NumeroCuentaPorPagar = @NumeroCuentaPorPagar
	--GROUP BY CPC.NumeroCuentaPorCobrar , A.NumeroAlmacen, A.NombreAgencia, CPC.FechaHoraRegistro, C.NumeroConcepto, C.Concepto, P.CodigoProveedor, P.NombreRazonSocial, M.CodigoMoneda, M.NombreMoneda, CPC.Monto, CPC.FechaLimite, CPC.CodigoEstado, CPC.Observaciones, CC.DIUsuario, U.Nombres, U.Paterno, U.Materno
END
GO
