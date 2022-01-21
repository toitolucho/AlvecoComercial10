use AlvecoComercial10
GO

DROP PROCEDURE InsertarProveedor
GO

CREATE PROCEDURE InsertarProveedor
	@NombreRazonSocial		VARCHAR(250),
	@NombreRepresentante	VARCHAR(250),
	@CodigoTipoProveedor	CHAR(1),
	@Rubro					VARCHAR(250),
	@NITProveedor			VARCHAR(30),
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3),
	@Direccion				VARCHAR(250),
	@Telefono				VARCHAR(50),
	@Fax					VARCHAR(250),
	@Casilla				CHAR(15),
	@PorcentajeGananciaVentaPorMayor	DECIMAL(10,2),
	@PorcentajeGananciaVentaPorMenor	DECIMAL(10,2),
	@Email					TEXT,
	@Observaciones			TEXT,
	@ProveedorActivo		BIT
AS
BEGIN
	INSERT INTO dbo.Proveedores (NombreRazonSocial,	NombreRepresentante,CodigoTipoProveedor, Rubro, NITProveedor, CodigoPais,CodigoDepartamento,CodigoProvincia,CodigoLugar,Direccion,Telefono,Fax,Casilla, PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor, Email,Observaciones, ProveedorActivo)
	VALUES (@NombreRazonSocial,@NombreRepresentante,@CodigoTipoProveedor,@Rubro, @NITProveedor, @CodigoPais, @CodigoDepartamento,@CodigoProvincia,@CodigoLugar,@Direccion,@Telefono,@Fax,@Casilla, @PorcentajeGananciaVentaPorMayor, @PorcentajeGananciaVentaPorMenor ,@Email,@Observaciones, @ProveedorActivo)
END
GO

DROP PROCEDURE ActualizarProveedor
GO

CREATE PROCEDURE ActualizarProveedor
	@CodigoProveedor		INT,
	@NombreRazonSocial		VARCHAR(250),
	@NombreRepresentante	VARCHAR(250),
	@CodigoTipoProveedor	CHAR(1),
	@Rubro					VARCHAR(250),
	@NITProveedor			VARCHAR(30),
	@CodigoPais				CHAR(2),
	@CodigoDepartamento		CHAR(2),
	@CodigoProvincia		CHAR(2),
	@CodigoLugar			CHAR(3),
	@Direccion				VARCHAR(250),
	@Telefono				VARCHAR(50),
	@Fax					VARCHAR(250),
	@Casilla				CHAR(15),
	@PorcentajeGananciaVentaPorMayor	DECIMAL(10,2),
	@PorcentajeGananciaVentaPorMenor	DECIMAL(10,2),
	@Email					TEXT,
	@Observaciones			TEXT,
	@ProveedorActivo		BIT
AS
BEGIN
	UPDATE 	dbo.Proveedores
	SET		
		NombreRazonSocial		= @NombreRazonSocial,
		NombreRepresentante		= @NombreRepresentante,
		CodigoTipoProveedor		= @CodigoTipoProveedor,
		Rubro					= @Rubro,
		NITProveedor			= @NITProveedor,
		CodigoPais				= @CodigoPais,
		CodigoDepartamento		= @CodigoDepartamento,
		CodigoProvincia			= @CodigoProvincia,
		CodigoLugar				= @CodigoLugar,
		Direccion				= @Direccion,
		Telefono				= @Telefono,
		Fax						= @Fax,
		PorcentajeGananciaVentaPorMayor = @PorcentajeGananciaVentaPorMayor,
		PorcentajeGananciaVentaPorMenor = @PorcentajeGananciaVentaPorMenor,
		Casilla					= @Casilla,
		Email					= @Email,
		Observaciones			= @Observaciones,
		ProveedorActivo			= @ProveedorActivo
	WHERE (@CodigoProveedor = CodigoProveedor)
END
GO

DROP PROCEDURE EliminarProveedor
GO

CREATE PROCEDURE EliminarProveedor
@CodigoProveedor INT
AS
BEGIN
	DELETE 
	FROM dbo.Proveedores
	WHERE (@CodigoProveedor = CodigoProveedor)
END
GO

DROP PROCEDURE ListarProveedores
GO

CREATE PROCEDURE ListarProveedores
AS
BEGIN
	SELECT	CodigoProveedor, NombreRazonSocial, NombreRepresentante, CodigoTipoProveedor, Rubro, NITProveedor, 
			CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Fax, 
			PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor,
			Casilla, Email, Observaciones, ProveedorActivo
	FROM dbo.Proveedores
	ORDER BY CodigoProveedor
END
GO



DROP PROCEDURE ListarProveedoresActivos
GO

CREATE PROCEDURE ListarProveedoresActivos
AS
BEGIN
	SELECT	CodigoProveedor, NombreRazonSocial, NombreRepresentante, CodigoTipoProveedor, Rubro, NITProveedor, 
			CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Fax, 
			PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor,
			Casilla, Email, Observaciones, ProveedorActivo
	FROM dbo.Proveedores
	where ProveedorActivo = 1
	ORDER BY CodigoProveedor
END
GO

DROP PROCEDURE ObtenerProveedor
GO

CREATE PROCEDURE ObtenerProveedor
@CodigoProveedor INT
AS
BEGIN
	SELECT	CodigoProveedor, NombreRazonSocial, NombreRepresentante, CodigoTipoProveedor,Rubro, NITProveedor,
			CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar, Direccion, Telefono, Fax, 
			PorcentajeGananciaVentaPorMayor, PorcentajeGananciaVentaPorMenor,
			Casilla, Email, Observaciones, ProveedorActivo
	FROM dbo.Proveedores
	WHERE (@CodigoProveedor = CodigoProveedor)
END
GO
