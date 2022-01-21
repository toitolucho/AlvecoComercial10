USE AlvecoComercial10
GO

DROP PROCEDURE ActualizarEliminarInventarioProductosCantidadesTransaccionesHistorial
GO

CREATE PROCEDURE ActualizarEliminarInventarioProductosCantidadesTransaccionesHistorial
	@NumeroAlmacen			INT,	
	@CodigoProducto			CHAR(15),
	@CantidadEgreso			INT,
	@NumeroTransaccion		INT,
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	DECLARE	@CodigoTipoCalculoInventario	CHAR(1),
			@ContadorCantidades				INT,
			@CantidadExistente				INT,
			@FechaHoraCompra				DATETIME,
			@PrecioUnitario					DECIMAL(10,3)
	SET @ContadorCantidades = 0	

	SET @CodigoTipoCalculoInventario = dbo.ObtenerCodigoTipoCalculoInventarioProducto(@CodigoProducto)	 
	--U'->UEPS, 'P'->PEPS, 'O'->Ponderado, 'B'-> Precio mas Bajo, 'A'->Precio mas alto, 'T'-> Ultimo Precio
	IF(@CodigoTipoCalculoInventario = 'U')--UEPS	
	BEGIN
		SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra, @PrecioUnitario =PrecioUnitario
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
		ORDER BY FechaHoraCompra DESC, NumeroTransaccionProducto DESC
		
		IF(@CantidadExistente > @CantidadEgreso)
		BEGIN
			UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
			WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
			AND FechaHoraCompra = @FechaHoraCompra
			
			UPDATE TOP(1) VentasProductosDetalleEntrega
				SET PrecioUnitarioCompraInventario = @PrecioUnitario,
					FechaHoraCompraInventario = @FechaHoraCompra
			WHERE NumeroAlmacen = @NumeroAlmacen
			AND NumeroVentaProducto = @NumeroTransaccion
			AND CodigoProducto = @CodigoProducto
			AND FechaHoraEntrega = @FechaHoraEntrega
			
		END
		ELSE IF(@CantidadExistente IS NOT NULL)
		BEGIN
		
			DELETE FROM VentasProductosDetalleEntrega
			WHERE NumeroAlmacen  = @NumeroAlmacen
			AND NumeroVentaProducto = @NumeroTransaccion
			AND CodigoProducto = @CodigoProducto
			AND FechaHoraEntrega = @FechaHoraEntrega
			
			WHILE( @CantidadEgreso >= @CantidadExistente )
			BEGIN
			
				IF(NOT EXISTS(SELECT * FROM VentasProductosDetalleEntrega
						WHERE NumeroAlmacen  = @NumeroAlmacen
					AND NumeroVentaProducto = @NumeroTransaccion
					AND CodigoProducto = @CodigoProducto
					AND FechaHoraEntrega = @FechaHoraEntrega 
					AND FechaHoraCompraInventario = @FechaHoraCompra))
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadExistente, @PrecioUnitario, @FechaHoraCompra)
				END
				ELSE
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadExistente, @PrecioUnitario, DATEADD(MILLISECOND, 1, @FechaHoraCompra))
				END
				DELETE TOP (1)
				FROM InventariosProductosCantidadesTransaccionesHistorial				
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra
				
				SET @CantidadEgreso = @CantidadEgreso - @CantidadExistente
				
				SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra, @PrecioUnitario = PrecioUnitario
				FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				ORDER BY FechaHoraCompra DESC, NumeroTransaccionProducto DESC
				
				IF(@CantidadExistente IS NULL)
					BREAK
				
			END
			
			IF(@CantidadEgreso > 0)
			BEGIN
				IF(NOT EXISTS(SELECT * FROM VentasProductosDetalleEntrega
						WHERE NumeroAlmacen  = @NumeroAlmacen
					AND NumeroVentaProducto = @NumeroTransaccion
					AND CodigoProducto = @CodigoProducto
					AND FechaHoraEntrega = @FechaHoraEntrega 
					AND FechaHoraCompraInventario = @FechaHoraCompra))
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadEgreso, @PrecioUnitario, @FechaHoraCompra)
				END
				ELSE
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadEgreso, @PrecioUnitario, DATEADD(MILLISECOND, 1, @FechaHoraCompra))
				END
				
			
				--INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
				--VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadEgreso, @PrecioUnitario, @FechaHoraCompra)
			
				UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra	
			END
		END
		ELSE 
		BEGIN
			UPDATE VentasProductosDetalleEntrega
				SET VentasProductosDetalleEntrega.PrecioUnitarioCompraInventario = SAD.PrecioUnitarioVenta
			FROM VentasProductosDetalle SAD
			WHERE SAD.NumeroAlmacen = @NumeroAlmacen
			AND SAD.NumeroVentaProducto = @NumeroTransaccion
			AND VentasProductosDetalleEntrega.CodigoProducto = @CodigoProducto
			AND VentasProductosDetalleEntrega.FechaHoraEntrega = @FechaHoraEntrega
		END
	END
	
	ELSE IF(@CodigoTipoCalculoInventario = 'P')	--PEPS
	BEGIN
		SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra, @PrecioUnitario = PrecioUnitario
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
		ORDER BY FechaHoraCompra ASC, NumeroTransaccionProducto ASC
		
		IF(@CantidadExistente > @CantidadEgreso)
		BEGIN
			UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
			WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
			AND FechaHoraCompra = @FechaHoraCompra
			
			UPDATE TOP(1) VentasProductosDetalleEntrega
				SET PrecioUnitarioCompraInventario = @PrecioUnitario,
					FechaHoraCompraInventario = @FechaHoraCompra
			WHERE NumeroAlmacen = @NumeroAlmacen
			AND NumeroVentaProducto = @NumeroTransaccion
			AND CodigoProducto = @CodigoProducto
			AND FechaHoraEntrega = @FechaHoraEntrega
		END
		ELSE IF(@CantidadExistente IS NOT NULL)
		BEGIN
			
			DELETE FROM VentasProductosDetalleEntrega
			WHERE NumeroAlmacen  = @NumeroAlmacen
			AND NumeroVentaProducto = @NumeroTransaccion
			AND CodigoProducto = @CodigoProducto
			AND FechaHoraEntrega = @FechaHoraEntrega
			
			WHILE( @CantidadEgreso >= @CantidadExistente )
			BEGIN
				IF(NOT EXISTS(SELECT * FROM VentasProductosDetalleEntrega
						WHERE NumeroAlmacen  = @NumeroAlmacen
					AND NumeroVentaProducto = @NumeroTransaccion
					AND CodigoProducto = @CodigoProducto
					AND FechaHoraEntrega = @FechaHoraEntrega 
					AND FechaHoraCompraInventario = @FechaHoraCompra))
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadExistente, @PrecioUnitario, @FechaHoraCompra)
				END
				ELSE
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadExistente, @PrecioUnitario, DATEADD(MILLISECOND, 1, @FechaHoraCompra))
				END
				
				DELETE TOP (1)
				FROM InventariosProductosCantidadesTransaccionesHistorial				
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra
				
				SET @CantidadEgreso =  @CantidadEgreso - @CantidadExistente
				
				SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra, @PrecioUnitario = PrecioUnitario
				FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				ORDER BY FechaHoraCompra ASC, NumeroTransaccionProducto ASC
				
				IF(@CantidadExistente IS NULL)
					BREAK
				
			END
			
			IF(@CantidadEgreso > 0)
			BEGIN
				
				IF(NOT EXISTS(SELECT * FROM VentasProductosDetalleEntrega
						WHERE NumeroAlmacen  = @NumeroAlmacen
					AND NumeroVentaProducto = @NumeroTransaccion
					AND CodigoProducto = @CodigoProducto
					AND FechaHoraEntrega = @FechaHoraEntrega 
					AND FechaHoraCompraInventario = @FechaHoraCompra))
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadEgreso, @PrecioUnitario, @FechaHoraCompra)
				END
				ELSE
				BEGIN
					INSERT INTO dbo.VentasProductosDetalleEntrega (NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, CantidadEntregada, PrecioUnitarioCompraInventario, FechaHoraCompraInventario)
					VALUES (@NumeroAlmacen, @NumeroTransaccion, @CodigoProducto, @FechaHoraEntrega, @CantidadEgreso, @PrecioUnitario, DATEADD(MILLISECOND, 1, @FechaHoraCompra))
				END
				
			
				UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra	
			END
		END
		ELSE 
		BEGIN
			UPDATE VentasProductosDetalleEntrega
				SET VentasProductosDetalleEntrega.PrecioUnitarioCompraInventario = SAD.PrecioUnitarioVenta
			FROM VentasProductosDetalle SAD
			WHERE SAD.NumeroAlmacen = @NumeroAlmacen
			AND SAD.NumeroVentaProducto = @NumeroTransaccion
			AND VentasProductosDetalleEntrega.CodigoProducto = @CodigoProducto
			AND VentasProductosDetalleEntrega.FechaHoraEntrega = @FechaHoraEntrega
		END
	END
	
	ELSE IF(@CodigoTipoCalculoInventario = 'O')	--Ponderado
	BEGIN
	
		SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra, @PrecioUnitario = PrecioUnitario
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
		ORDER BY FechaHoraCompra DESC, NumeroTransaccionProducto ASC
		
		UPDATE VentasProductosDetalleEntrega
			SET PrecioUnitarioCompraInventario = @PrecioUnitario,
				FechaHoraCompraInventario = @FechaHoraCompra
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroVentaProducto = @NumeroTransaccion
		AND CodigoProducto = @CodigoProducto
		AND FechaHoraEntrega = @FechaHoraEntrega			
		
		IF(@CantidadExistente > @CantidadEgreso)
		BEGIN
			UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
			WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
			AND FechaHoraCompra = @FechaHoraCompra
			
		END
		ELSE
		BEGIN
			
			WHILE( @CantidadEgreso >= @CantidadExistente )
			BEGIN
				DELETE TOP (1)
				FROM InventariosProductosCantidadesTransaccionesHistorial				
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra
				
				SET @CantidadEgreso =  @CantidadEgreso - @CantidadExistente
				
				SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
				FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				ORDER BY FechaHoraCompra DESC, NumeroTransaccionProducto DESC
				
				IF(@CantidadExistente IS NULL)
					BREAK
				
			END
			
			IF(@CantidadEgreso > 0)
				UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra	
			
		END
	END
	
	ELSE IF(@CodigoTipoCalculoInventario = 'B')	--Precio mas Bajo, DISEMINUYE EL DE PRECIO MAS ALTO
	BEGIN
		SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
		ORDER BY PrecioUnitario DESC, NumeroTransaccionProducto ASC
		
		IF(@CantidadExistente > @CantidadEgreso)
		BEGIN
			UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
			WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
			AND FechaHoraCompra = @FechaHoraCompra
		END
		ELSE
		BEGIN
			
			WHILE( @CantidadEgreso >= @CantidadExistente )
			BEGIN
				DELETE TOP (1)
				FROM InventariosProductosCantidadesTransaccionesHistorial				
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra
				
				SET @CantidadEgreso =  @CantidadEgreso - @CantidadExistente
				
				SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
				FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				ORDER BY PrecioUnitario DESC, NumeroTransaccionProducto ASC
				
				IF(@CantidadExistente IS NULL)
					BREAK				
			END
			
			IF(@CantidadEgreso > 0)
				UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra	
			
		END
	END
	
	ELSE IF(@CodigoTipoCalculoInventario = 'A')	--Precio mas Alto, DISMINUYE EL DE PRECIO MAS BAJO
	BEGIN
		SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
		ORDER BY PrecioUnitario ASC
		
		IF(@CantidadExistente > @CantidadEgreso)
		BEGIN
			UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
			WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
			AND FechaHoraCompra = @FechaHoraCompra
		END
		ELSE
		BEGIN
			
			WHILE( @CantidadEgreso >= @CantidadExistente )
			BEGIN
				DELETE TOP (1)
				FROM InventariosProductosCantidadesTransaccionesHistorial				
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra
				
				SET @CantidadEgreso = @CantidadEgreso - @CantidadExistente
				
				SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
				FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				ORDER BY PrecioUnitario ASC
				
				IF(@CantidadExistente IS NULL)
					BREAK
			END
			
			IF(@CantidadEgreso > 0)
				UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra	
			
		END
	END
	
	ELSE IF(@CodigoTipoCalculoInventario = 'T')	--Precio Ultimo de Compra, DISMINUYE EL DE PRECIO MAS BAJO
	BEGIN
		SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
		FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
		WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
		ORDER BY FechaHoraCompra ASC, NumeroTransaccionProducto ASC
		
		IF(@CantidadExistente > @CantidadEgreso)
		BEGIN
			UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
			WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
			AND FechaHoraCompra = @FechaHoraCompra
		END
		ELSE
		BEGIN
			
			WHILE( @CantidadEgreso >= @CantidadExistente )
			BEGIN
				DELETE TOP (1)
				FROM InventariosProductosCantidadesTransaccionesHistorial				
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra
				
				SET @CantidadEgreso = @CantidadEgreso - @CantidadExistente
				
				SELECT TOP 1 @CantidadExistente = CantidadExistente, @FechaHoraCompra = FechaHoraCompra
				FROM dbo.InventariosProductosCantidadesTransaccionesHistorial
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				ORDER BY FechaHoraCompra ASC, NumeroTransaccionProducto ASC
				
				IF(@CantidadExistente IS NULL)
					BREAK
				
			END
			
			IF(@CantidadEgreso > 0)
				UPDATE TOP(1) InventariosProductosCantidadesTransaccionesHistorial 
				SET CantidadExistente = CantidadExistente - @CantidadEgreso
				WHERE NumeroAlmacen = @NumeroAlmacen AND CodigoProducto = @CodigoProducto
				AND FechaHoraCompra = @FechaHoraCompra	
			
		END
	END
	
END
GO
