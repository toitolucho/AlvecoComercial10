USE AlvecoComercial10
GO



DROP PROCEDURE InsertarDepartamento
GO
CREATE PROCEDURE InsertarDepartamento
	@CodigoPais			CHAR(2),
	@CodigoDepartamento	CHAR(2),
	@NombreDepartamento	VARCHAR(250)
AS
BEGIN
	INSERT INTO dbo.Departamentos (CodigoPais, CodigoDepartamento, NombreDepartamento)
	VALUES (@CodigoPais, @CodigoDepartamento, @NombreDepartamento)
END
GO



DROP PROCEDURE ActualizarDepartamento
GO
CREATE PROCEDURE ActualizarDepartamento
	@CodigoPais			CHAR(2),
	@CodigoDepartamento	CHAR(2),
	@NombreDepartamento	VARCHAR(250)
AS
BEGIN
	UPDATE 	dbo.Departamentos
	SET
		NombreDepartamento = @NombreDepartamento
	WHERE	(CodigoPais = @CodigoPais)
		AND (CodigoDepartamento = @CodigoDepartamento)
END
GO



DROP PROCEDURE EliminarDepartamento
GO
CREATE PROCEDURE EliminarDepartamento
	@CodigoPais			CHAR(2),
	@CodigoDepartamento	CHAR(2)
AS
BEGIN
	DELETE 
	FROM dbo.Departamentos
	WHERE	(CodigoPais = @CodigoPais)
		AND (CodigoDepartamento = @CodigoDepartamento)
		
END
GO



DROP PROCEDURE ListarDepartamentos
GO
CREATE PROCEDURE ListarDepartamentos
AS
BEGIN
	SELECT CodigoPais, CodigoDepartamento, NombreDepartamento
	FROM dbo.Departamentos
	ORDER BY CodigoPais, CodigoDepartamento
END
GO



DROP PROCEDURE ObtenerDepartamento
GO
CREATE PROCEDURE ObtenerDepartamento
	@CodigoPais			CHAR(2),
	@CodigoDepartamento	CHAR(2)
AS
BEGIN
	SELECT CodigoPais, CodigoDepartamento, NombreDepartamento
	FROM dbo.Departamentos
	WHERE	(CodigoPais = @CodigoPais)
		AND (CodigoDepartamento = @CodigoDepartamento)
END
GO

DROP PROCEDURE ObtenerDepartamentosPorPais
GO

CREATE PROCEDURE ObtenerDepartamentosPorPais
@CodigoPais				CHAR(2)
AS
SELECT CodigoPais, CodigoDepartamento, NombreDepartamento
FROM Departamentos
WHERE CodigoPais = @CodigoPais
GO
