USE AlvecoComercial10
GO


DROP PROCEDURE InsertarUsuario
GO

CREATE PROCEDURE InsertarUsuario
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
	@Observaciones		TEXT,
	@NombreUsuario		CHAR(32),
	@Contrasena			VARCHAR(255),
	@FechaRegistro		DATETIME,
	@CodigoTipoUsuario	CHAR(1),
	@NumeroAlmacen		INT
	
AS
BEGIN
	BEGIN TRANSACTION
	--	IF(NOT EXISTS (SELECT * FROM Personas WHERE 
	--		UPPER(RTRIM(LTRIM(Nombres))) = UPPER(RTRIM(LTRIM(@Nombres))) 
	--		AND UPPER(RTRIM(LTRIM(Paterno))) = UPPER(RTRIM(LTRIM(@Paterno)))
	--		AND UPPER(RTRIM(LTRIM(Materno))) = UPPER(RTRIM(LTRIM(@Materno)))
	--		)
	--		AND NOT EXISTS(SELECT * FROM Usuarios 
	--		WHERE UPPER(RTRIM(LTRIM(NombreUsuario))) = UPPER(RTRIM(LTRIM(@NombreUsuario))))
	--		)
	--		INSERT INTO Personas (DIPersona, Paterno, Materno, Nombres, FechaNacimiento, Sexo, EstadoCivil, Celular,
	--							  Email, CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD, DireccionD, TelefonoD, Observaciones)
	--		VALUES (@DIPersona, @Paterno, @Materno, @Nombres, @FechaNacimiento, @Sexo, @EstadoCivil, @Celular,
	--				@Email, @CodigoPaisD, @CodigoDepartamentoD, @CodigoProvinciaD, @CodigoLugarD, @DireccionD, @TelefonoD, @Observaciones)

	--ELSE
	--	RAISERROR ('EL NOMBRE DEL CONCEPTO YA SE ENCUENTRA REGISTRADO',16,2)
	
	IF(EXISTS((SELECT * FROM Personas WHERE 
			UPPER(RTRIM(LTRIM(Nombres))) = UPPER(RTRIM(LTRIM(@Nombres))) 
			AND UPPER(RTRIM(LTRIM(Paterno))) = UPPER(RTRIM(LTRIM(@Paterno)))
			AND UPPER(RTRIM(LTRIM(Materno))) = UPPER(RTRIM(LTRIM(@Materno)))
			)))
	BEGIN
		RAISERROR ('LOS DATOS GENERALES(NOMBRE, APPELIDOS) DEL USUARIO YA SE ENCUENTRAN REGISTRADOS',16,2)
	END
	ELSE
	BEGIN
		IF(EXISTS(SELECT * FROM Usuarios 
			WHERE UPPER(RTRIM(LTRIM(NombreUsuario))) = UPPER(RTRIM(LTRIM(@NombreUsuario)))))
		BEGIN
			RAISERROR ('LA CUENTA DE USUARIO YA SE ENCUENTRA REGISTRADA',16,2)
		END
		ELSE
		BEGIN
			
			EXEC ('USE master; CREATE LOGIN ' + @NombreUsuario + ' WITH PASSWORD =N''' + @Contrasena + ''', DEFAULT_LANGUAGE=Español, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF; USE AlvecoComercial10')
			EXEC ('USE AlvecoComercial10; CREATE USER ' + @NombreUsuario + ' FOR LOGIN ' + @NombreUsuario)
			EXEC ('USE AlvecoComercial10; EXEC sp_addrolemember N''db_owner'', N''' + @NombreUsuario + '''')
			
			INSERT INTO Personas (DIPersona, Paterno, Materno, Nombres, FechaNacimiento, Sexo, EstadoCivil, NombreCargo, Celular,
								  Email, CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD, DireccionD, TelefonoD, Observaciones)
			VALUES (@DIPersona, @Paterno, @Materno, @Nombres, @FechaNacimiento, @Sexo, @EstadoCivil, @NombreCargo, @Celular,
					@Email, @CodigoPaisD, @CodigoDepartamentoD, @CodigoProvinciaD, @CodigoLugarD, @DireccionD, @TelefonoD, @Observaciones)
			
			SET @FechaRegistro = ISNULL(@FechaRegistro, GETDATE())
				
			INSERT INTO Usuarios(DIUsuario, NombreUsuario, Contrasena, FechaRegistro, Observaciones, CodigoTipoUsuario, NumeroAlmacen)
			VALUES (@DIPersona, @NombreUsuario, PWDENCRYPT(@Contrasena), @FechaRegistro, @Observaciones, @CodigoTipoUsuario, @NumeroAlmacen)
			
			
		END
	END
			
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUEDE INGRESAR LOS DATOS DEL USUARIO, LOS MISMOS SE ENCUENTRAN YA REGISTRADOS',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ListarUsuariosPersonas
GO

CREATE PROCEDURE ListarUsuariosPersonas
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
			CASE U.CodigoTipoUsuariO
			WHEN 'A' THEN 'ADMINISTRADOR'
			WHEN 'V' THEN 'RESPONSABLE VENTAS'
			WHEN 'M' THEN 'ENGARGADO ALMACENES'
			WHEN 'O' THEN 'OTROS USUARIOS' END AS TipoUsuario, CodigoTipoUsuario,
			p.EstadoCivil, NumeroAlmacen,
			P.CodigoPaisD, P.CodigoDepartamentoD, P.CodigoProvinciaD, P.CodigoLugarD,
			P.Celular, P.Email, P.DireccionD, P.TelefonoD, P.Observaciones,
			PA.NombrePais, DE.NombreDepartamento, PR.NombreProvincia, LU.NombreLugar,
			U.NombreUsuario, U.FechaRegistro, CAST(U.Contrasena AS VARCHAR(255)) AS Contrasena
	FROM Personas P
	INNER JOIN Usuarios U
	ON P.DIPersona = U.DIUsuario
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
END
GO

exec ListarUsuariosPersonas null

DROP PROCEDURE EliminarUsuario
GO

CREATE PROCEDURE EliminarUsuario
	@DIPersona		CHAR(15)
AS
BEGIN
	DECLARE @NombreUsuario	CHAR(32)
	SELECT @NombreUsuario = NombreUsuario FROM Usuarios WHERE DIUsuario = @DIPersona
	BEGIN TRANSACTION
	
	
	EXEC ('USE AlvecoComercial10; EXEC sp_droprolemember N''db_owner'', N''' + @NombreUsuario + '''')
	EXEC ('USE AlvecoComercial10; DROP USER ' + @NombreUsuario)
	EXEC ('USE master; DROP LOGIN ' + @NombreUsuario)	
	
	
	DELETE FROM Usuarios
	WHERE DIUsuario = @DIPersona
	
	DELETE FROM Personas 
	WHERE DIPersona = @DIPersona
	
	
	
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO ELIMINAR EL REGISTRO, PROBABLEMENTE EL USUARIO YA REALIZÓ TRANSACCIONES EN EL SISTEMA',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarUsuario
GO

CREATE PROCEDURE ActualizarUsuario
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
	@Observaciones		VARCHAR(MAX),
	@NombreUsuario		CHAR(32),
	@Contrasena			VARCHAR(255),
	@FechaRegistro		DATETIME,
	@CodigoTipoUsuariO	CHAR(1),
	@NumeroAlmacen		INT
	
AS
BEGIN
	BEGIN TRANSACTION
	
		DECLARE @DIPersonaAux	VARCHAR(15)
				
		IF(CHARINDEX('|',@Observaciones) > 0)
		BEGIN
			SET @DIPersonaAux = LTRIM(RTRIM(SUBSTRING(@Observaciones, CHARINDEX('|',@Observaciones) + 1, LEN(@Observaciones))))
			SET @Observaciones = SUBSTRING(@Observaciones,0,  CHARINDEX('|',@Observaciones))
			
			--PRINT @Observaciones + ',  CI :' + @DIPersonaAux
		END
		
		IF(@DIPersonaAux IS NULL)
		BEGIN
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
					
			UPDATE USUARIOS
				SET
					NombreUsuario				= @NombreUsuario,
					CodigoTipoUsuario			= @CodigoTipoUsuario,
					NumeroAlmacen				= @NumeroAlmacen
					--Contrasena					= PWDENCRYPT(@Contrasena)
			WHERE DIUsuario = @DIPersona
		END	
		ELSE
		BEGIN
			UPDATE PERSONAS
				SET DIPersona					= @DIPersona, 
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
			WHERE DIPersona = @DIPersonaAux
					
			UPDATE USUARIOS
				SET
					NombreUsuario				= @NombreUsuario,
					CodigoTipoUsuario			= @CodigoTipoUsuario,
					NumeroAlmacen				= @NumeroAlmacen
					--Contrasena					= PWDENCRYPT(@Contrasena)
			WHERE DIUsuario = @DIPersonaAux
		END	
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO ACTUALIZAR EL REGISTRO',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarContrasenaUsuario
GO 

CREATE PROCEDURE ActualizarContrasenaUsuario
	@DIPersona		CHAR(15),
	@NombreUsuario	CHAR(32),
	@Contrasena		VARCHAR(255)
AS
BEGIN
	--BEGIN TRANSACTION
		UPDATE Usuarios
			SET Contrasena = PWDENCRYPT(@Contrasena)
		WHERE DIUsuario = @DIPersona
		
		EXEC ('USE master; ALTER LOGIN ' + @NombreUsuario + ' WITH PASSWORD = '''+ @Contrasena+ '''')	
		
	
	--IF (@@ERROR <> 0)
	--BEGIN
	--	DECLARE @MensajeError	VARCHAR(MAX) = 'OCURRIO LA SIGUIENTE EXCEPCION, LA CONSULTA NO EJECUTA : "'
	--	 + 'USE master; ALTER LOGIN ' + @NombreUsuario + ' WITH PASSWORD = '''+ @Contrasena+ ''''
	--	RAISERROR (@MensajeError, 17,2)
	--END
	--ELSE
	--	COMMIT TRANSACTION
	
END
GO

--VENTA NORMAL
--PRODUCTOS DAÑADOS
--PRODUCTOS PERDIDOS

--AGREGAR BOTON ANULAR EN UNA VENTA, COMPRA CUANDO ESTA INICIADA (NO BORRAR)
--REPORTE INGRESOS Y SALIDAS EN FECHAS

