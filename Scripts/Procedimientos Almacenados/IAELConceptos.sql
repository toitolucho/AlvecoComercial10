USE AlvecoComercial10
GO

DROP PROC InsertarConcepto
GO
CREATE PROC InsertarConcepto
@Concepto		VARCHAR(256)
AS
BEGIN
	BEGIN TRANSACTION
	IF(NOT EXISTS (SELECT * FROM Conceptos WHERE UPPER(RTRIM(LTRIM(Concepto))) = UPPER(RTRIM(LTRIM(@Concepto)))))
		INSERT INTO dbo.Conceptos (Concepto)
		VALUES (@Concepto)
	ELSE
		RAISERROR ('EL NOMBRE DEL CONCEPTO YA SE ENCUENTRA REGISTRADO',16, 2)
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO INSERTAR CORRECTAMENTE EL REGISTRO',16, 2)
	END
	ELSE
		COMMIT TRANSACTION

	
END
GO


DROP PROC EliminarConcepto
GO
CREATE PROC EliminarConcepto
@NumeroConcepto		INT
AS
BEGIN
	DELETE FROM dbo.Conceptos
	WHERE NumeroConcepto= @NumeroConcepto
END
GO

DROP PROC ActualizarConcepto
GO
CREATE PROC ActualizarConcepto
@NumeroConcepto	INT,
@Concepto		VARCHAR(256)
AS
BEGIN
	BEGIN TRANSACTION
	IF(NOT EXISTS (SELECT * FROM Conceptos WHERE UPPER(RTRIM(LTRIM(Concepto))) = UPPER(RTRIM(LTRIM(@Concepto))) AND NumeroConcepto <> @NumeroConcepto))
		UPDATE dbo.Conceptos
			SET Concepto = @Concepto
		WHERE NumeroConcepto = @NumeroConcepto

	ELSE
		RAISERROR ('EL NOMBRE DEL CONCEPTO YA SE ENCUENTRA REGISTRADO',16,2)
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO INSERTAR CORRECTAMENTE EL REGISTRO',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROC ListarConceptos
GO
CREATE PROC ListarConceptos
AS
BEGIN
	SELECT NumeroConcepto, Concepto
	FROM dbo.Conceptos
END
GO

DROP PROC ObtenerConcepto
GO
CREATE PROC ObtenerConcepto
@NumeroConcepto	INT
AS
BEGIN
	SELECT NumeroConcepto, Concepto
	FROM dbo.Conceptos
	WHERE NumeroConcepto = @NumeroConcepto
END
GO