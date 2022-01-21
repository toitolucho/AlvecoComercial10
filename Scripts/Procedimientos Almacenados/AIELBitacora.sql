USE AlvecoComercial10
GO

DROP TABLE Bitacora
GO

CREATE TABLE Bitacora
(
BitacoraID			INT				NOT NULL IDENTITY (1, 1) PRIMARY KEY ,
EventType			NVARCHAR(30)	NOT NULL,
Status				INT				NOT NULL,
EventInfo			NVARCHAR(4000)	NOT NULL,
Usuario				VARCHAR(200)	NULL DEFAULT (suser_sname()),
Fecha				DATETIME		NULL DEFAULT (getdate()),
ValoresInsertados	TEXT			NULL,
ValoresEliminados	TEXT			NULL
) 
GO


--/* Trigger de Monitoreo */
--DROP TRIGGER trig_ProductosMarcas
--GO

--CREATE TRIGGER trig_ProductosMarcas
--ON dbo.ProductosMarcas
--FOR DELETE, INSERT, UPDATE
--AS
--BEGIN
--	DECLARE @NUMERO INT
--	INSERT INTO Bitacora (EventType, Status, EventInfo)
--	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')

--	SET @NUMERO = @@IDENTITY

--	--UPDATE BITACORA
--	--	SET Usuario = SYSTEM_USER,
--	--	Fecha = GETDATE()
--	--WHERE BitacoraID=@NUMERO
--END
--GO


--INSERT INTO ProductosMarcas (NombreMarca)
--VALUES('NACIONAL')
--SELECT * FROM ProductosMarcas
--SELECT * FROM Bitacora

--UPDATE ProductosMarcas SET NombreMarca = 'PRUEBA DE DATOS'


--CREATE TRIGGER trig_tablabitacora
--GO

--CREATE TRIGGER trig_tablabitacora
--ON NivelesAcademicos
--FOR DELETE, INSERT, UPDATE
--AS
--BEGIN
--DECLARE @NUMERO INT
--INSERT INTO Bitacora (EventType, Status, EventInfo)
--exec sp_executesql N'DBCC INPUTBUFFER( @i )', N'@i int',
--@i=@@spid
--END



--SELECT * FROM BITACORA
--GO

--use alvecocomercial10


-- 

INSERT INTO Personas(DIPersona, Paterno, Materno, Nombres, FechaNacimiento, Sexo, Celular, Email, CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD, DireccionD, TelefonoD, Observaciones) 
VALUES('0000000000', 'ANONIMO', 'ANONIMO', 'ANONIMO', NULL, 'M', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

INSERT INTO Usuarios(NombreUsuario, Contrasena, DIUsuario, Observaciones) 
VALUES('postgres', PWDENCRYPT('postgres'), '0000000000', 'Administrador del Sistema')

 
	
use AlvecoComercial10
select * from Personas		
select * from usuarios

--declare @NombreUsuario VARCHAR(160) = 'postgres',
--		@Contrasena	   VARCHAR(160) = 'postgres'



--primera tarea

EXEC ('USE AlvecoComercial10; EXEC sp_droprolemember N''db_owner'', ''postgres''')
EXEC ('USE AlvecoComercial10; DROP USER postgres')
EXEC ('USE master; DROP LOGIN  postgres')	


--segunda tarea
EXEC ('USE master; CREATE LOGIN ' + 'postgres' + ' WITH PASSWORD =N''' + 'postgres' + ''', DEFAULT_LANGUAGE=Español, CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF; USE ' + 'AlvecoComercial10')
EXEC ('USE AlvecoComercial10; CREATE USER postgres FOR LOGIN postgres' )
EXEC ('USE AlvecoComercial10; EXEC sp_addrolemember N''db_owner'', N''' + 'postgres' + '''')
use AlvecoComercial10
--GRANT CONTROL SERVER TO postgres;
EXEC sp_addsrvrolemember 'postgres', 'sysadmin';
