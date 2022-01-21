USE AlvecoComercial10
GO
--Realiza la Obtención del Ultimo indice de la Tabla que contiene una columna Identity como 
--AutoIncrementable, en caso de que la Tabla este vacia, retorna 1, caso contrario el número 
--Actual donde se encuentra el Identity
--DROP FUNCTION ObtenerUltimoIndiceTabla
--GO

--CREATE FUNCTION ObtenerUltimoIndiceTabla( @NombreTabla VARCHAR(250))
--RETURNS INT	
--AS
--BEGIN
--	DECLARE @Indice			INT,
--			@CantidadTuplas INT,
--			@CadenaConsulta	VARCHAR(100)
			
--	--SET @CadenaConsulta = 'SELECT * FR0M ' + @NombreTabla
	
--	exec ObtenerNumeroTuplasTabla @NombreTabla, @CantidadTuplas OUTPUT
	
--	--EXEC (@CadenaConsulta)
--	--SET @CantidadTuplas = @@ROWCOUNT
--	SET @Indice = IDENT_CURRENT(@NombreTabla)
--	RETURN CASE WHEN @CantidadTuplas = 0 AND @Indice = 1 THEN 0 ELSE @Indice END 
--END
--GO


DROP PROCEDURE ObtenerUltimoIndiceTabla
GO

CREATE PROCEDURE ObtenerUltimoIndiceTabla
	@NombreTabla VARCHAR(250),
	@Indice		 INT OUTPUT
AS
BEGIN
	DECLARE @CadenaConsulta VARCHAR(100),
			@CantidadTuplas INT
			
	SET @CadenaConsulta = 'SELECT * FROM ' + @NombreTabla
	PRINT @CadenaConsulta
	EXEC (@CadenaConsulta)
	SET @CantidadTuplas = @@ROWCOUNT
	SET @Indice = IDENT_CURRENT(@NombreTabla)
	SET @Indice = CASE WHEN @CantidadTuplas = 0 AND @Indice = 1 THEN 0 ELSE @Indice END 
END
GO

--DECLARE @Indice INT
--EXEC ObtenerUltimoIndiceTabla 'Clientes', @Indice OUTPUT
--SELECT @Indice


--select dbo.ObtenerUltimoIndiceTabla('CuentasPorPagarPagos')
--DECLARE @CantidadTuplas int
--EXEC ObtenerNumeroTuplasTabla 'CuentasPorPagarPagos',@CantidadTuplas OUTPUT
--SELECT @CantidadTuplas

--select IDENT_CURRENT('ComprasProductos')
--select * from Almacenes
--SELECT * FROM CuentasPorPagar
--SELECT * FROM CuentasPorPagarPagos
--delete from CuentasPorPagarPagosç
--select * from ComprasProductos
--select * from ComprasProductosDetalle
--select * from ComprasProductosDetalleEntrega
--select * from InventariosProductos

