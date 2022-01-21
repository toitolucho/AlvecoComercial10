USE AlvecoComercial10



-----------------------------------------------------------------------------------------
-- creacion del usuario SuperAdministrador [No debe modificarse sus valores]
-----------------------------------------------------------------------------------------

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
	--VALUES('0000000000', 'Administrador', 'administrador', 'administrador', NULL, 'M', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)


	--INSERT INTO Usuarios(NombreUsuario, Contrasena, DIUsuario, Observaciones) 
	--VALUES('administrador', PWDENCRYPT('administrador'), '0000000000', 'Super Administrador del Sistema')


-----------------------------------------------------------------------------------------
--Usuario Administrador
-----------------------------------------------------------------------------------------

	EXEC ('USE AlvecoComercial10; EXEC sp_droprolemember N''db_owner'', ''roxana''')
	EXEC ('USE AlvecoComercial10; DROP USER roxana')
	EXEC ('USE master; DROP LOGIN  roxana')	


	--segunda tarea
	EXEC ('USE master; CREATE LOGIN ' + 'roxana' + ' WITH PASSWORD =N''' + 'roxana' + ''', DEFAULT_LANGUAGE=Español, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF; USE ' + 'AlvecoComercial10')
	EXEC ('USE AlvecoComercial10; CREATE USER roxana FOR LOGIN roxana' )
	EXEC ('USE AlvecoComercial10; EXEC sp_addrolemember N''db_owner'', N''' + 'roxana' + '''')
	
	--GRANT CONTROL SERVER TO administrador;
	EXEC sp_addsrvrolemember 'roxana', 'sysadmin';


	--INSERT INTO Personas(DIPersona, Paterno, Materno, Nombres, FechaNacimiento, Sexo, Celular, Email, CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD, DireccionD, TelefonoD, Observaciones) 
	--VALUES('5058785', 'ROXANA', 'GIRA', 'MANSILLA', NULL, 'F', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)


	--INSERT INTO Usuarios(NombreUsuario, Contrasena, DIUsuario, Observaciones) 
	--VALUES('roxana', PWDENCRYPT('roxana'), '5058785', 'Administrador del Sistema')


--select * from Usuarios

--UPDATE Usuarios
--			SET Contrasena = PWDENCRYPT('administrador')
--		WHERE DIUsuario = '0000000000'     
		
--		EXEC ('USE master; ALTER LOGIN ' + @NombreUsuario + ' WITH PASSWORD = '''+ @Contrasena+ '''')	
		
	--select * from usuarios
	


DELETE FROM USUARIOS 
WHERE DIUsuario NOT IN('0000000000','5058785')



