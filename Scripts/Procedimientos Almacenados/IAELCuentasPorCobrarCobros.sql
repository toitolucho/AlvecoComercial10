USE AlvecoComercial10
GO

DROP PROC InsertarCuentasPorCobrarCobros
GO
CREATE PROC InsertarCuentasPorCobrarCobros
@NumeroCuentaPorCobrar	INT,
@FechaHoraCobro			DATETIME,
@Monto					DECIMAL(10,2),
@DIUsuario				CHAR(15)
AS
BEGIN
	INSERT INTO dbo.CuentasPorCobrarCobros(NumeroCuentaPorCobrar, FechaHoraCobro, Monto, DIUsuario)
	VALUES (@NumeroCuentaPorCobrar, GETDATE(), @Monto, @DIUsuario)
	
	IF(SELECT SUM(Monto) FROM CuentasPorCobrarCobros
	WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar) =
	(SELECT Monto FROM CuentasPorCobrar 
	WHERE NumeroCuentaPorCobrar = 
	@NumeroCuentaPorCobrar)
	BEGIN
		UPDATE CuentasPorCobrar
			SET CodigoEstado = 'P'
		WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
	END
END
GO


DROP PROC ActualizarCuentasPorCobrarCobros
GO
CREATE PROC ActualizarCuentasPorCobrarCobros
@NumeroCobro		INT,
@FechaHoraCobro		DATETIME,
@Monto				DECIMAL(10,2),
@DIUsuario			CHAR(15)
AS
BEGIN
	UPDATE dbo.CuentasPorCobrarCobros
	SET	FechaHoraCobro	= @FechaHoraCobro,
		Monto			= @Monto,
		DIUsuario	= @DIUsuario
	WHERE NumeroCobro = @NumeroCobro
END
GO


DROP PROC EliminarCuentasPorCobrarCobros
GO
CREATE PROC EliminarCuentasPorCobrarCobros
@NumeroCobro		INT
AS
BEGIN
	DELETE FROM dbo.CuentasPorCobrarCobros 
	WHERE NumeroCobro = @NumeroCobro
	
	SELECT @NumeroCobro =  CPC.NumeroCuentaPorCobrar
	FROM dbo.CuentasPorCobrar CPC
	INNER JOIN dbo.CuentasPorCobrarCobros CPCC
	ON CPC.NumeroCuentaPorCobrar = CPCC.NumeroCuentaPorCobrar
	
	IF( EXISTS(SELECT * FROM dbo.CuentasPorCobrar 
		WHERE NumeroCuentaPorCobrar = @NumeroCobro 
		AND CodigoEstado = 'P' ))
	BEGIN
		UPDATE dbo.CuentasPorCobrar
			SET CodigoEstado = 'D'
		WHERE NumeroCuentaPorCobrar = @NumeroCobro 
	END
END
GO


DROP PROC EliminarCuentasPorCobrarCobrosNumeroCuenta
GO
CREATE PROC EliminarCuentasPorCobrarCobrosNumeroCuenta
@NumeroCuentaPorCobrar		INT
AS
BEGIN
	DELETE FROM dbo.CuentasPorCobrarCobros 
	WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
END
GO



DROP PROC ListarCuentasPorCobrarCobros
GO
CREATE PROC ListarCuentasPorCobrarCobros
@NumeroCuentaPorCobrar		INT
AS
BEGIN
	SELECT 	NumeroCuentaPorCobrar, NumeroCobro, FechaHoraCobro, Monto, DIUsuario
	FROM dbo.CuentasPorCobrarCobros
	WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
END
GO

DROP PROC ObtenerdCuentaPorCobrarCobro
GO
CREATE PROC ObtenerdCuentaPorCobrarCobro
@NumeroCobro		INT
AS
BEGIN
	SELECT 	NumeroCuentaPorCobrar, NumeroCobro, FechaHoraCobro, Monto, DIUsuario
	FROM dbo.CuentasPorCobrarCobros
	WHERE NumeroCobro = @NumeroCobro
END
GO


DROP PROC ListarCuentasPorCobrarCobrosDetallado
GO
CREATE PROC ListarCuentasPorCobrarCobrosDetallado
@NumeroCuentaPorCobrar		INT
AS
BEGIN
	SELECT 	C.NumeroCuentaPorCobrar, C.NumeroCobro, C.FechaHoraCobro, C.Monto, U.DIPersona, U.Nombres, U.Paterno, U.Materno
	FROM dbo.CuentasPorCobrarCobros C 
	INNER JOIN dbo.Personas U 
	ON C.DIUsuario = U.DIPersona
	WHERE C.NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
END
GO