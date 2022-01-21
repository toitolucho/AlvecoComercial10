USE AlvecoComercial10
GO



DROP PROCEDURE InsertarMovilidad
GO
CREATE PROCEDURE InsertarMovilidad
	@CodigoMovilidad		VARCHAR(10),
	@NombreMovilidad		VARCHAR(250),
	@CodigoPlaca			CHAR(20),
	@CodigoMarca			INT,
	@CodigoModelo			INT,
	@CodigoEstadoMovilidad	CHAR(1),
	@Descripcion			TEXT
AS
BEGIN

	BEGIN TRANSACTION
	IF(NOT EXISTS (SELECT * FROM Movilidades WHERE CodigoMovilidad = @CodigoMovilidad))
		INSERT INTO dbo.Movilidades (CodigoMovilidad, NombreMovilidad,CodigoPlaca, CodigoMarca, CodigoModelo, CodigoEstadoMovilidad, Descripcion)
		VALUES (@CodigoMovilidad, @NombreMovilidad, @CodigoPlaca, @CodigoMarca, @CodigoModelo, @CodigoEstadoMovilidad, @Descripcion)
	ELSE
		RAISERROR ('EL CODIGO DE LA MOVILIDAD YA SE ENCUENTRA REGISTRADO',16, 2)
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO INSERTAR CORRECTAMENTE EL REGISTRO, PROBABLEMENTE EL NOMBRE ES REPETIDO O QUISAS LA PLACA',16, 2)
	END
	ELSE
		COMMIT TRANSACTION

	
END
GO



DROP PROCEDURE ActualizarMovilidad
GO
CREATE PROCEDURE ActualizarMovilidad
	@CodigoMovilidad		VARCHAR(10),
	@NombreMovilidad		VARCHAR(250),
	@CodigoPlaca			CHAR(20),
	@CodigoMarca			INT,
	@CodigoModelo			INT,
	@CodigoEstadoMovilidad	CHAR(1),
	@Descripcion			TEXT	
AS
BEGIN
	UPDATE 	dbo.Movilidades
	SET				
		NombreMovilidad 		= @NombreMovilidad,
		CodigoPlaca				= @CodigoPlaca,
		CodigoMarca				= @CodigoMarca,
		CodigoModelo			= @CodigoModelo,
		CodigoEstadoMovilidad	= @CodigoEstadoMovilidad,
		Descripcion				= @Descripcion
	WHERE (CodigoMovilidad = @CodigoMovilidad)
END
GO



DROP PROCEDURE EliminarMovilidad
GO
CREATE PROCEDURE EliminarMovilidad
	@CodigoMovilidad	VARCHAR(10)
AS
BEGIN
	DELETE 
	FROM dbo.Movilidades
	WHERE (CodigoMovilidad = @CodigoMovilidad)
END
GO



DROP PROCEDURE ListarMovilidades
GO
CREATE PROCEDURE ListarMovilidades
AS
BEGIN
	SELECT CodigoMovilidad, NombreMovilidad, CodigoPlaca, CodigoMarca, CodigoModelo, CodigoEstadoMovilidad, Descripcion
	FROM dbo.Movilidades
	ORDER BY CodigoMovilidad
END
GO



DROP PROCEDURE ObtenerMovilidad
GO
CREATE PROCEDURE ObtenerMovilidad
	@CodigoMovilidad	VARCHAR(10)
AS

BEGIN
	SELECT CodigoMovilidad, NombreMovilidad, CodigoPlaca, CodigoMarca, CodigoModelo, CodigoEstadoMovilidad, Descripcion
	FROM dbo.Movilidades
	WHERE (CodigoMovilidad = @CodigoMovilidad)
END
GO
