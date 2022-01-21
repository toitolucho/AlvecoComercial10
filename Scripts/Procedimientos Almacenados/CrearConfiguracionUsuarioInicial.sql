USE AlvecoComercial10

DROP PROCEDURE CrearConfiguracionUsuarioInicial
GO

CREATE PROCEDURE CrearConfiguracionUsuarioInicial
AS
BEGIN
--primera tarea

	EXEC ('USE AlvecoComercial10; EXEC sp_droprolemember N''db_owner'', ''administrador''')
	EXEC ('USE AlvecoComercial10; DROP USER administrador')
	EXEC ('USE master; DROP LOGIN  administrador')	

	--segunda tarea
	EXEC ('USE master; CREATE LOGIN ' + 'administrador' + ' WITH PASSWORD =N''' + 'administrador' + ''', DEFAULT_LANGUAGE=Español, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF; USE ' + 'AlvecoComercial10')
	EXEC ('USE AlvecoComercial10; CREATE USER administrador FOR LOGIN administrador' )
	EXEC ('USE AlvecoComercial10; EXEC sp_addrolemember N''db_owner'', N''' + 'administrador' + '''')
	
	--GRANT CONTROL SERVER TO administrador;
	EXEC sp_addsrvrolemember 'administrador', 'sysadmin';


	--INSERT INTO Personas(DIPersona, Paterno, Materno, Nombres, FechaNacimiento, Sexo, Celular, Email, CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD, DireccionD, TelefonoD, Observaciones) 
	--VALUES('0000000000', 'ROXANA', 'GIRA', 'MANCILLA', NULL, 'M', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)


	--INSERT INTO Usuarios(NombreUsuario, Contrasena, DIUsuario, Observaciones) 
	--VALUES('administrador', PWDENCRYPT('administrador'), '0000000000', 'Administrador del Sistema')

END
GO

--UPDATE Usuarios
--	SET NombreUsuario ='administrador',
--		Contrasena = PWDENCRYPT('administrador')
--WHERE DIUsuario = '0000000000'
