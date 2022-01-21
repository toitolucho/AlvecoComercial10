USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCliente
GO
CREATE PROCEDURE InsertarCliente
	@CodigoCliente			INT,
	@NombreCliente			VARCHAR(250),
	@NombreRepresentante	VARCHAR(250),
	@CodigoTipoCliente		CHAR(1),
	@NITCliente				VARCHAR(30),
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3),
	@Direccion				VARCHAR(250),
	@Telefono				VARCHAR(50),
	@Email					TEXT,
	@Observaciones			TEXT,
	@CodigoEstadoCliente	CHAR(1)
AS
BEGIN
	BEGIN TRANSACTION
	IF(NOT EXISTS (SELECT * FROM Clientes WHERE NombreCliente = @NombreCliente OR NombreRepresentante = @NombreRepresentante))
		INSERT INTO dbo.Clientes (NombreCliente, NombreRepresentante, CodigoTipoCliente, NITCliente, CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Email, Observaciones, CodigoEstadoCliente)
		VALUES (@NombreCliente, @NombreRepresentante, @CodigoTipoCliente, @NITCliente, @CodigoPais, @CodigoDepartamento, @CodigoProvincia, @CodigoLugar, @Direccion, @Telefono, @Email, @Observaciones, @CodigoEstadoCliente)
	ELSE
		RAISERROR ('EL NOMBRE DEL CLIENTE O SU REPRESENTANTE YA SE ENCUENTRA REGISTRADO',16, 2)
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO INSERTAR CORRECTAMENTE EL REGISTRO',16, 2)
	END
	ELSE
		COMMIT TRANSACTION



	
END
GO

DROP PROCEDURE ActualizarCliente
GO

CREATE PROCEDURE ActualizarCliente
	@CodigoCliente			INT,
	@NombreCliente			VARCHAR(250),
	@NombreRepresentante	VARCHAR(250),
	@CodigoTipoCliente		CHAR(1),
	@NITCliente				VARCHAR(30),
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3),
	@Direccion				VARCHAR(250),
	@Telefono				VARCHAR(50),
	@Email					TEXT,
	@Observaciones			TEXT,
	@CodigoEstadoCliente	CHAR(1)
AS
BEGIN

	BEGIN TRANSACTION
	IF(NOT EXISTS (SELECT * FROM Clientes WHERE (NombreCliente = @NombreCliente or NombreRepresentante = @NombreRepresentante) AND CodigoCliente <> @CodigoCliente))
		UPDATE 	dbo.Clientes
		SET						
			NombreCliente		= @NombreCliente,
			NombreRepresentante = @NombreRepresentante,
			CodigoTipoCliente	= @CodigoTipoCliente,
			NITCliente			= @NITCliente,
			CodigoPais			= @CodigoPais,
			CodigoDepartamento	= @CodigoDepartamento,
			CodigoProvincia		= @CodigoProvincia,
			CodigoLugar			= @CodigoLugar,
			Direccion			= @Direccion,
			Telefono			= @Telefono,
			Email				= @Email,
			Observaciones		= @Observaciones,
			CodigoEstadoCliente	= @CodigoEstadoCliente
		WHERE CodigoCliente = @CodigoCliente
	ELSE
		RAISERROR ('EL NOMBRE DEL CLIENTE O SU REPRESENTANTE YA SE ENCUENTRA REGISTRADO',16,2)
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR ('NO SE PUDO INSERTAR CORRECTAMENTE EL REGISTRO',16,2)
	END
	ELSE
		COMMIT TRANSACTION
	
END
GO



DROP PROCEDURE EliminarCliente
GO

CREATE PROCEDURE EliminarCliente
@CodigoCliente INT
AS
BEGIN
	DELETE 
	FROM dbo.Clientes
	WHERE @CodigoCliente = CodigoCliente
END
GO

DROP PROCEDURE ListarClientes
GO

CREATE PROCEDURE ListarClientes
AS
BEGIN
	SELECT CodigoCliente, NombreCliente, NombreRepresentante, CodigoTipoCliente, NITCliente, CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Email, Observaciones, CodigoEstadoCliente
	FROM dbo.Clientes
	ORDER BY CodigoCliente
END
GO

DROP PROCEDURE ListarClientesActivos
GO

CREATE PROCEDURE ListarClientesActivos
AS
BEGIN
	SELECT CodigoCliente, NombreCliente, NombreRepresentante, CodigoTipoCliente, NITCliente, CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Email, Observaciones, CodigoEstadoCliente
	FROM dbo.Clientes
	where CodigoEstadoCliente ='H'
	ORDER BY CodigoCliente
END
GO

DROP PROCEDURE ObtenerCliente
GO
CREATE PROCEDURE ObtenerCliente
	@CodigoCliente INT
AS
BEGIN
	SELECT CodigoCliente, NombreCliente, NombreRepresentante, CodigoTipoCliente, NITCliente, CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Email, Observaciones, CodigoEstadoCliente
	FROM dbo.Clientes
	WHERE CodigoCliente = @CodigoCliente
END
GO

DROP PROCEDURE ListarClientesReporte
GO

CREATE PROCEDURE ListarClientesReporte
AS
BEGIN
SELECT Cl.CodigoCliente, Cl.NombreCliente, Cl.NombreRepresentante, TipoCliente = CASE Cl.CodigoTipoCliente WHEN 'E' THEN 'EMPRESA' WHEN 'P' THEN 'PERSONA' END, 
Cl.NITCliente, Pa.NombrePais, De.NombreDepartamento, Prov.NombreProvincia, Lu.NombreLugar,
Cl.Direccion, Cl.Telefono, Cl.Email, Cl.Observaciones, 
EstadoCliente = CASE Cl.CodigoEstadoCliente WHEN 'H' THEN 'HABILITADO' WHEN 'S' THEN 'SUSPENDIDO' WHEN 'I' THEN 'INHABILITADO' END
FROM dbo.Clientes Cl
LEFT JOIN Paises Pa ON
	Pa.CodigoPais = Cl.CodigoPais
	LEFT JOIN Departamentos De ON
	De.CodigoPais = Cl.CodigoPais
	AND De.CodigoDepartamento = Cl.CodigoDepartamento
	LEFT JOIN Provincias Prov ON
	Prov.CodigoPais = Cl.CodigoPais
	AND Prov.CodigoDepartamento = Cl.CodigoDepartamento
	AND Prov.CodigoProvincia = Cl.CodigoProvincia
	LEFT JOIN Lugares Lu ON
	Lu.CodigoPais = Cl.CodigoPais
	AND Lu.CodigoDepartamento = Cl.CodigoDepartamento
	AND Lu.CodigoProvincia = Cl.CodigoProvincia
END
GO