USE AlvecoComercial10
GO


DROP PROCEDURE InsertarPersona
GO

CREATE PROCEDURE InsertarPersona
	@DIPersona			CHAR(15),
	@Paterno			VARCHAR(40),
	@Materno			VARCHAR(40),
	@Nombres			VARCHAR(40),
	@FechaNacimiento	DATETIME,
	@Sexo				CHAR(1),	
	@EstadoCivil		CHAR(1),
	@NombreCargo		VARCHAR(250),
	@Celular			VARCHAR(30),
	@Email				TEXT,
	@CodigoPaisD		CHAR(2),
	@CodigoDepartamentoD CHAR(2),
	@CodigoProvinciaD	CHAR(2),
	@CodigoLugarD		CHAR(2),
	@DireccionD			VARCHAR(250),
	@TelefonoD			VARCHAR(250),
	@Observaciones		TEXT
AS
BEGIN
	BEGIN TRANSACTION
	
	IF(EXISTS((SELECT * FROM Personas WHERE 
			UPPER(RTRIM(LTRIM(Nombres))) = UPPER(RTRIM(LTRIM(@Nombres))) 
			AND UPPER(RTRIM(LTRIM(Paterno))) = UPPER(RTRIM(LTRIM(@Paterno)))
			AND UPPER(RTRIM(LTRIM(Materno))) = UPPER(RTRIM(LTRIM(@Materno)))
			)))
	BEGIN
		RAISERROR ('LOS DATOS GENERALES(NOMBRE, APPELIDOS) DEL Persona YA SE ENCUENTRAN REGISTRADOS',16,2)
	END
	ELSE
	BEGIN
		INSERT INTO Personas (DIPersona, Paterno, Materno, Nombres, FechaNacimiento, Sexo, EstadoCivil, NombreCargo, Celular,
							  Email, CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD, DireccionD, TelefonoD, Observaciones)
		VALUES (@DIPersona, @Paterno, @Materno, @Nombres, @FechaNacimiento, @Sexo, @EstadoCivil, @NombreCargo,@Celular,
				@Email, @CodigoPaisD, @CodigoDepartamentoD, @CodigoProvinciaD, @CodigoLugarD, @DireccionD, @TelefonoD, @Observaciones)
	END
			
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUEDE INGRESAR LOS DATOS DEL Persona, LOS MISMOS SE ENCUENTRAN YA REGISTRADOS',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ListarPersonas
GO

CREATE PROCEDURE ListarPersonas
	@DIPersona		CHAR(15)
AS
BEGIN
	SELECT	P.DIPersona, P.Nombres, P.Paterno, P.Materno, P.FechaNacimiento, P.Sexo, 
			UPPER(RTRIM(P.Nombres) + ' ' +  RTRIM(P.Paterno) + ' ' + RTRIM(P.Materno)) AS NombreCompleto,
			CASE P.Sexo WHEN 'F' THEN 'FEMENINO' ELSE 'MASCULINO' END AS SexoCadena,
			CASE P.EstadoCivil 
			WHEN 'S' THEN 'SOLTERO' 
			WHEN 'C' THEN 'CASADO' 
			WHEN 'V' THEN 'VIUDO' 
			WHEN 'D' THEN 'DIVORCIADO' END AS NombreEstadoCivil, NombreCargo,
			p.EstadoCivil,
			P.CodigoPaisD, P.CodigoDepartamentoD, P.CodigoProvinciaD, P.CodigoLugarD,
			P.Celular, P.Email, P.DireccionD, P.TelefonoD, P.Observaciones,
			PA.NombrePais, DE.NombreDepartamento, PR.NombreProvincia, LU.NombreLugar
	FROM Personas P
	LEFT JOIN Paises PA
	ON P.CodigoPaisD = PA.CodigoPais
	LEFT JOIN Departamentos DE
	ON P.CodigoDepartamentoD = DE.CodigoDepartamento
	LEFT JOIN Provincias PR
	ON P.CodigoProvinciaD = PR.CodigoProvincia
	LEFT JOIN Lugares LU
	ON P.CodigoLugarD = LU.CodigoLugar
	WHERE P.DIPersona LIKE CASE WHEN @DIPersona IS NULL THEN '%%' 
	ELSE @DIPersona END
	ORDER BY 7
END
GO



DROP PROCEDURE EliminarPersona
GO

CREATE PROCEDURE EliminarPersona
	@DIPersona		CHAR(15)
AS
BEGIN
	BEGIN TRANSACTION
	DELETE FROM Personas
	WHERE DIPersona = @DIPersona
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO ELIMINAR EL REGISTRO, PROBABLEMENTE EL Persona YA REALIZÓ TRANSACCIONES EN EL SISTEMA',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarPersona
GO

CREATE PROCEDURE ActualizarPersona
	@DIPersona			CHAR(15),
	@Paterno			VARCHAR(40),
	@Materno			VARCHAR(40),
	@Nombres			VARCHAR(40),
	@FechaNacimiento	DATETIME,
	@Sexo				CHAR(1),	
	@EstadoCivil		CHAR(1),
	@NombreCargo		VARCHAR(250),
	@Celular			VARCHAR(30),
	@Email				TEXT,
	@CodigoPaisD		CHAR(2),
	@CodigoDepartamentoD CHAR(2),
	@CodigoProvinciaD	CHAR(2),
	@CodigoLugarD		CHAR(2),
	@DireccionD			VARCHAR(250),
	@TelefonoD			VARCHAR(250),
	@Observaciones		TEXT	
AS
BEGIN
	BEGIN TRANSACTION
		UPDATE PERSONAS
			SET 
				Paterno						= @Paterno,
				Materno						= @Materno,
				Nombres						= @Nombres,
				FechaNacimiento				= @FechaNacimiento,
				Sexo						= @Sexo,				
				EstadoCivil					= @EstadoCivil,
				NombreCargo					= @NombreCargo,
				Celular						= @Celular,
				Email						= @Email,
				CodigoPaisD					= @CodigoPaisD,
				CodigoDepartamentoD			= @CodigoDepartamentoD,
				CodigoProvinciaD			= @CodigoProvinciaD,
				CodigoLugarD				= @CodigoLugarD,
				DireccionD					= @DireccionD,
				TelefonoD					= @TelefonoD,
				Observaciones				= @Observaciones
		WHERE DIPersona = @DIPersona
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO ACTUALIZAR EL REGISTRO',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO


DROP PROCEDURE ListarPersonasParticulares
GO

CREATE PROCEDURE ListarPersonasParticulares
	@DIPersona		CHAR(15)
AS
BEGIN
	SELECT	P.DIPersona, P.Nombres, P.Paterno, P.Materno, P.FechaNacimiento, P.Sexo, 
			UPPER(RTRIM(P.Nombres) + ' ' +  RTRIM(P.Paterno) + ' ' + RTRIM(P.Materno)) AS NombreCompleto,
			CASE P.Sexo WHEN 'F' THEN 'FEMENINO' ELSE 'MASCULINO' END AS SexoCadena,
			CASE P.EstadoCivil 
			WHEN 'S' THEN 'SOLTERO' 
			WHEN 'C' THEN 'CASADO' 
			WHEN 'V' THEN 'VIUDO' 
			WHEN 'D' THEN 'DIVORCIADO' END AS NombreEstadoCivil, NombreCargo,
			p.EstadoCivil,
			P.CodigoPaisD, P.CodigoDepartamentoD, P.CodigoProvinciaD, P.CodigoLugarD,
			P.Celular, P.Email, P.DireccionD, P.TelefonoD, P.Observaciones,
			PA.NombrePais, DE.NombreDepartamento, PR.NombreProvincia, LU.NombreLugar
	FROM Personas P
	LEFT JOIN Paises PA
	ON P.CodigoPaisD = PA.CodigoPais
	LEFT JOIN Departamentos DE
	ON P.CodigoDepartamentoD = DE.CodigoDepartamento
	LEFT JOIN Provincias PR
	ON P.CodigoProvinciaD = PR.CodigoProvincia
	LEFT JOIN Lugares LU
	ON P.CodigoLugarD = LU.CodigoLugar
	WHERE P.DIPersona NOT IN
	(
		SELECT DIUsuario FROM Usuarios
	)
	AND (P.DIPersona LIKE CASE WHEN @DIPersona IS NULL THEN '%%' 
	ELSE @DIPersona END )
END
GO

