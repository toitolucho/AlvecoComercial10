USE AlvecoComercial10
GO



DROP PROC InsertarCuentasPorPagarPagos
GO
CREATE PROC InsertarCuentasPorPagarPagos
@NumeroCuentaPorPagar	INT,
@FechaHoraPago			DATETIME,
@Monto					DECIMAL(10,2),
@DIUsuario				CHAR(15)
AS
BEGIN
	INSERT INTO dbo.CuentasPorPagarPagos(NumeroCuentaPorPagar, FechaHoraPago, Monto, DIUsuario)
	VALUES (@NumeroCuentaPorPagar, GETDATE(), @Monto, @DIUsuario)
	
	IF(SELECT SUM(Monto) FROM CuentasPorPagarPagos
	WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar) =
	(SELECT Monto FROM CuentasPorPagar
	WHERE NumeroCuentaPorPagar = 
	@NumeroCuentaPorPagar)
	BEGIN
		UPDATE CuentasPorPagar
			SET CodigoEstado = 'P'
		WHERE NumeroCuentaPorPagar= @NumeroCuentaPorPagar
	END
END
GO


DROP PROC ActualizarCuentasPorPagarPagos
GO
CREATE PROC ActualizarCuentasPorPagarPagos
@NumeroPago		INT,
@FechaHoraPago	DATETIME,
@Monto			DECIMAL(10,2),
@DIUsuario		CHAR(15)
AS
BEGIN
	UPDATE dbo.CuentasPorPagarPagos
	SET	FechaHoraPago = @FechaHoraPago,
		Monto = @Monto,
		DIUsuario = DIUsuario
	WHERE NumeroPago = @NumeroPago
END
GO


DROP PROC EliminarCuentasPorPagarPagos
GO
CREATE PROC EliminarCuentasPorPagarPagos
@NumeroPago		INT
AS
BEGIN
	DELETE FROM dbo.CuentasPorPagarPagos WHERE NumeroPago = @NumeroPago
	
	SELECT @NumeroPago =  CPC.NumeroCuentaPorPagar
	FROM dbo.CuentasPorPagar CPC
	INNER JOIN dbo.CuentasPorPagarPagos CPCC
	ON CPC.NumeroCuentaPorPagar = CPCC.NumeroCuentaPorPagar
	
	IF( EXISTS(SELECT * FROM dbo.CuentasPorPagar 
		WHERE NumeroCuentaPorPagar = @NumeroPago 
		AND CodigoEstado = 'P' ))
	BEGIN
		UPDATE dbo.CuentasPorPagar
			SET CodigoEstado = 'D'
		WHERE NumeroCuentaPorPagar = @NumeroPago 
	END
END
GO


DROP PROC EliminarCuentasPorPagarPagosNumeroCuenta
GO
CREATE PROC EliminarCuentasPorPagarPagosNumeroCuenta
@NumeroCuentaPorPagar		INT
AS
BEGIN
	DELETE FROM dbo.CuentasPorPagarPagos WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
END
GO


DROP PROC ListarCuentasPorPagarPagos
GO
CREATE PROC ListarCuentasPorPagarPagos
@NumeroCuentaPorPagar		INT
AS
BEGIN
	SELECT 	NumeroCuentaPorPagar, NumeroPago, FechaHoraPago, Monto, DIUsuario
	FROM dbo.CuentasPorPagarPagos
	WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
END
GO

DROP PROC ObtenerCuentaPorPagarPago
GO
CREATE PROC ObtenerCuentaPorPagarPago
@NumeroPago		INT
AS
BEGIN
	SELECT 	NumeroCuentaPorPagar, NumeroPago, FechaHoraPago, Monto, DIUsuario
	FROM dbo.CuentasPorPagarPagos
	WHERE NumeroPago = @NumeroPago
END
GO

DROP PROC ListarCuentasPorPagarPagosDetallado
GO
CREATE PROC ListarCuentasPorPagarPagosDetallado
@NumeroCuentaPorPagar		INT
AS
BEGIN
	SELECT 	C.NumeroCuentaPorPagar, C.NumeroPago, C.FechaHoraPago, C.Monto, 
			c.DIUsuario, DBO.ObtenerNombreCompleto(c.DIUsuario) AS NombreCompleto
	FROM dbo.CuentasPorPagarPagos C
	WHERE C.NumeroCuentaPorPagar = @NumeroCuentaPorPagar
END
GO








