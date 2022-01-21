USE AlvecoComercial10
GO

IF OBJECT_ID ('TG_IAEInventariosProductos','TR') IS NOT NULL
   DROP TRIGGER TG_IAEInventariosProductos;
GO

CREATE TRIGGER TG_IAEInventariosProductos ON InventariosProductos
AFTER UPDATE, INSERT
AS
	--DECLARE @CodigoProducto				CHAR(15),
	--		@NumeroAgencia				INT,
	--		@PrecioUnitarioCompra		DECIMAL(10,2),
	--		@PrecioUnitarioCompra2		DECIMAL(10,2),
	--		@PorcentajeUtilidad1		DECIMAL(5,2),
	--		@PorcentajeUtilidad2		DECIMAL(5,2),
	--		@CantidadExistencia			INT,
	--		@CantidadExistencia2		INT
			
			
	--SELECT @CodigoProducto = CodigoProducto, @NumeroAgencia = NumeroAlmacen, @PrecioUnitarioCompra = PrecioUnitarioCompra,
	--	   @PorcentajeUtilidad1 = PorcentajeGananciaVentaPorMayor, @PorcentajeUtilidad2 = PorcentajeGananciaVentaPorMenor, 
	--	   @CantidadExistencia = CantidadExistencia
	--FROM INSERTED	
	
	--SELECT @PrecioUnitarioCompra2	= PrecioUnitarioCompra,
	--	   @CantidadExistencia2		= CantidadExistencia
	--FROM DELETED (NOLOCK) 
	--WHERE NumeroAlmacen = @NumeroAgencia 
	--AND CodigoProducto = @CodigoProducto
	

	--IF(@PrecioUnitarioCompra2 <> @PrecioUnitarioCompra)	
	--BEGIN
		
	--	UPDATE InventariosProductos
	--	SET PrecioUnitarioVentaPorMayor = ROUND(@PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad1 /100 , 2),
	--		PrecioUnitarioVentaPorMenor = ROUND(@PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad2 /100 , 2)
	--	--SET PrecioUnitarioVenta1 = @PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad1 /100,
	--	--	PrecioUnitarioVenta2 = @PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad2 /100,
	--	--	PrecioUnitarioVenta3 = @PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad3 /100,
	--	--	PrecioUnitarioVenta4 = @PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad1 /100  + (@PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad1 /100) * @PorcentajeImpuestoIVA /100,
	--	--	PrecioUnitarioVenta5 = @PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad2 /100  + (@PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad2 /100) * @PorcentajeImpuestoIVA /100,
	--	--	PrecioUnitarioVenta6 = @PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad3 /100  + (@PrecioUnitarioCompra + @PrecioUnitarioCompra * @PorcentajeUtilidad3 /100) * @PorcentajeImpuestoIVA /100 
	--	WHERE NumeroAlmacen = @NumeroAgencia 
	--	AND CodigoProducto = @CodigoProducto
	--END	
	
	INSERT INTO Bitacora (EventType, Status, EventInfo)
	EXEC ('DBCC INPUTBUFFER(' + @@SPID +')')
	
GO