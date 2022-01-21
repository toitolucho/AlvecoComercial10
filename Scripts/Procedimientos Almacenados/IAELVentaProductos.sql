USE AlvecoComercial10
GO


DROP PROCEDURE InsertarVentaProducto
GO
CREATE PROCEDURE InsertarVentaProducto
	@NumeroAlmacen				INT,
	@CodigoCliente				INT,
	@DIUsuario					CHAR(15),
	@FechaHoraVenta				DATETIME,
	@FechaHoraEntrega			DATETIME,
	@NumeroComprobante			VARCHAR(100),
	@CodigoEstadoVenta			CHAR(1),
	@CodigoTipoVenta			CHAR(1),
	@CodigoMotivoVenta			CHAR(1),
	@NumeroFactura				VARCHAR(100),
	@MontoTotalVenta			DECIMAL(10,2),
	@MontoTotalPagoEfectivo		DECIMAL(10,2),
	@MontoTotalDescuento		DECIMAL(5,2),
	@NumeroCuentaPorCobrar		INT,
	@VentaParaDistribuir		BIT,
	@CodigoMovilidad			VARCHAR(10),
	@DIPersonaDistribuidor		CHAR(15),
	@Observaciones				TEXT
AS
BEGIN
	INSERT INTO dbo.VentasProductos(NumeroAlmacen, CodigoVentaProducto, CodigoCliente, DIUsuario, FechaHoraVenta, FechaHoraEntrega, NumeroComprobante, CodigoEstadoVenta, CodigoTipoVenta, CodigoMotivoVenta, NumeroFactura, MontoTotalVenta, MontoTotalPagoEfectivo, MontoTotalDescuento, NumeroCuentaPorCobrar, VentaParaDistribuir, CodigoMovilidad, DIPersonaDistribuidor, Observaciones)
	VALUES (@NumeroAlmacen, dbo.ObtenerCodigoVentaProducto(), @CodigoCliente, @DIUsuario, @FechaHoraVenta, @FechaHoraEntrega, @NumeroComprobante, @CodigoEstadoVenta, @CodigoTipoVenta, @CodigoMotivoVenta, @NumeroFactura, @MontoTotalVenta, @MontoTotalPagoEfectivo, @MontoTotalDescuento, @NumeroCuentaPorCobrar, @VentaParaDistribuir, @CodigoMovilidad, @DIPersonaDistribuidor, @Observaciones)
END
GO


DROP PROCEDURE ActualizarVentaProducto
GO
CREATE PROCEDURE ActualizarVentaProducto
	@NumeroAlmacen				INT,
	@NumeroVentaProducto		INT,
	@CodigoCliente				INT,
	@DIUsuario					CHAR(15),
	@FechaHoraVenta				DATETIME,
	@FechaHoraEntrega			DATETIME,
	@NumeroComprobante			VARCHAR(100),
	@CodigoEstadoVenta			CHAR(1),
	@CodigoTipoVenta			CHAR(1),
	@CodigoMotivoVenta			CHAR(1),
	@NumeroFactura				VARCHAR(100),
	@MontoTotalVenta			DECIMAL(10,2),
	@MontoTotalPagoEfectivo		DECIMAL(10,2),
	@MontoTotalDescuento		DECIMAL(5,2),
	@NumeroCuentaPorCobrar		INT,
	@VentaParaDistribuir		BIT,	
	@CodigoMovilidad			VARCHAR(10),
	@DIPersonaDistribuidor		CHAR(15),
	@Observaciones				TEXT
AS
BEGIN
	UPDATE 	dbo.VentasProductos
	SET			
		CodigoCliente			= @CodigoCliente,
		DIUsuario				= @DIUsuario,
		FechaHoraEntrega		= @FechaHoraEntrega,
		NumeroComprobante		= @NumeroComprobante,
		CodigoEstadoVenta		= @CodigoEstadoVenta,
		CodigoTipoVenta			= @CodigoTipoVenta,
		CodigoMotivoVenta		= @CodigoMotivoVenta,
		NumeroFactura			= @NumeroFactura,
		MontoTotalVenta			= @MontoTotalVenta,
		MontoTotalPagoEfectivo	= @MontoTotalPagoEfectivo,
		MontoTotalDescuento		= @MontoTotalDescuento,
		NumeroCuentaPorCobrar	= @NumeroCuentaPorCobrar,
		VentaParaDistribuir		= @VentaParaDistribuir,
		DIPersonaDistribuidor	= @DIPersonaDistribuidor,
		CodigoMovilidad			= @CodigoMovilidad,
		Observaciones			= @Observaciones
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
END
GO



DROP PROCEDURE EliminarVentaProducto
GO
CREATE PROCEDURE EliminarVentaProducto
	@NumeroAlmacen					INT,
	@NumeroVentaProducto			INT
AS
BEGIN
	BEGIN TRANSACTION
	DELETE 
	FROM VentasProductosDetalleEntrega
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	
	DELETE
	FROM VentasProductosDetalle
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	
	
	DELETE 
	FROM dbo.VentasProductos
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
	
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR('NO SE PUDO COMPLETAR LA ELIMINACIÓN DE LA TRANSACCIÓN ACTUAL',17,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO



DROP PROCEDURE ListarVentasProductos
GO
CREATE PROCEDURE ListarVentasProductos
AS
BEGIN
	SELECT	NumeroAlmacen, NumeroVentaProducto, CodigoCliente, DIUsuario, FechaHoraVenta, FechaHoraEntrega, NumeroComprobante, CodigoEstadoVenta, 
			CodigoTipoVenta, CodigoMotivoVenta, NumeroFactura, MontoTotalVenta, MontoTotalPagoEfectivo, 
			MontoTotalDescuento, NumeroCuentaPorCobrar, VentaParaDistribuir, DIPersonaDistribuidor, CodigoMovilidad, Observaciones
	FROM dbo.VentasProductos
	ORDER BY NumeroAlmacen, NumeroVentaProducto
END
GO



DROP PROCEDURE ObtenerVentaProducto
GO
CREATE PROCEDURE ObtenerVentaProducto
	@NumeroAlmacen					INT,
	@NumeroVentaProducto			INT
AS
BEGIN
	SELECT	NumeroAlmacen, NumeroVentaProducto, CodigoCliente, DIUsuario, FechaHoraVenta, FechaHoraEntrega, NumeroComprobante, CodigoEstadoVenta, 
			CodigoTipoVenta,CodigoMotivoVenta,  NumeroFactura, MontoTotalVenta, 
			MontoTotalPagoEfectivo, MontoTotalDescuento, NumeroCuentaPorCobrar, VentaParaDistribuir, DIPersonaDistribuidor, CodigoMovilidad, Observaciones
	FROM dbo.VentasProductos
	WHERE NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
END
GO




DROP PROCEDURE InsertarVentaProductoXMLDetalle
GO
CREATE PROCEDURE InsertarVentaProductoXMLDetalle
	@NumeroAlmacen				INT,
	@CodigoCliente				INT,
	@DIUsuario					CHAR(15),
	@FechaHoraVenta				DATETIME,
	@FechaHoraEntrega			DATETIME,
	@NumeroComprobante			VARCHAR(100),
	@CodigoEstadoVenta			CHAR(1),
	@CodigoTipoVenta			CHAR(1),
	@CodigoMotivoVenta			CHAR(1),
	@NumeroFactura				VARCHAR(100),
	@MontoTotalVenta			DECIMAL(10,2),
	@MontoTotalPagoEfectivo		DECIMAL(10,2),
	@MontoTotalDescuento		DECIMAL(5,2),
	@NumeroCuentaPorCobrar		INT,
	@VentaParaDistribuir		BIT,
	@CodigoMovilidad			VARCHAR(10),
	@DIPersonaDistribuidor		CHAR(15),
	@Observaciones				TEXT,
	@ProductosDetalleXML		TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	INSERT INTO dbo.VentasProductos(NumeroAlmacen, CodigoVentaProducto, CodigoCliente, DIUsuario, FechaHoraVenta, FechaHoraEntrega, NumeroComprobante, CodigoEstadoVenta, CodigoTipoVenta, CodigoMotivoVenta, NumeroFactura, MontoTotalVenta, MontoTotalPagoEfectivo, MontoTotalDescuento, NumeroCuentaPorCobrar, VentaParaDistribuir, DIPersonaDistribuidor, CodigoMovilidad, Observaciones)
	VALUES (@NumeroAlmacen, dbo.ObtenerCodigoVentaProducto(), @CodigoCliente, @DIUsuario, GETDATE(), @FechaHoraEntrega, @NumeroComprobante, @CodigoEstadoVenta, @CodigoTipoVenta, @CodigoMotivoVenta, @NumeroFactura, @MontoTotalVenta, @MontoTotalPagoEfectivo, @MontoTotalDescuento, @NumeroCuentaPorCobrar, @VentaParaDistribuir, @DIPersonaDistribuidor, @CodigoMovilidad, @Observaciones)
		
		DECLARE @punteroXMLProductosDetalle INT,
				@NumeroVentaProducto		INT
		
		--SET @NumeroVentaProducto = @@IDENTITY
		SET @NumeroVentaProducto = SCOPE_IDENTITY()--Devuelve el ultimo id Ingresado en una Tabla con una columna Identidad dentro del Ambito,
		--es Decir en este caso, devolverá el ultimo IDENTITY generado dentro de la Tabla VentasProductos para esta transacción
					
		IF(@CodigoTipoVenta = 'E')
			UPDATE dbo.VentasProductos
				SET MontoTotalPagoEfectivo = MontoTotalVenta
			WHERE NumeroAlmacen = @NumeroAlmacen
			AND NumeroVentaProducto = @NumeroVentaProducto
		
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		
		INSERT INTO dbo.VentasProductosDetalle(
				NumeroAlmacen,
				NumeroVentaProducto,
				CodigoProducto,
				CantidadVenta,
				CantidadEntregada,
				PrecioUnitarioVenta
				)
		SELECT  @NumeroAlmacen, 
				@NumeroVentaProducto, 
				CodigoProducto, 				
				Cantidad,
				0,
				PrecioUnitario
		FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductos/VentasProductosDetalle',2)
		WITH(	
				NumeroAlmacen			INT,
				NumeroVentaProducto	INT,
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2),
				TiempoGarantia			INT			
				
				)
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		
		DECLARE @MontoTotalVenta2 DECIMAL(10,2)
		SELECT @MontoTotalVenta2 =  SUM(VPD.CantidadVenta * VPD.PrecioUnitarioVenta)
		FROM VentasProductosDetalle VPD
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPD.NumeroVentaProducto = @NumeroVentaProducto
		
		UPDATE VentasProductos
			SET MontoTotalVenta = @MontoTotalVenta2
		FROM VentasProductosDetalle VPD		
		WHERE VentasProductos.NumeroAlmacen = @NumeroAlmacen
		AND VentasProductos.NumeroVentaProducto = @NumeroVentaProducto
		
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo ingresar la Venta de Productos',16,2)	
			ROLLBACK TRAN
		END
	COMMIT TRANSACTION
END
GO



DROP PROCEDURE ActualizarVentaProductoXMLDetalle
GO
CREATE PROCEDURE ActualizarVentaProductoXMLDetalle
	@NumeroAlmacen				INT,
	@NumeroVentaProducto		INT,
	@CodigoCliente				INT,
	@DIUsuario					CHAR(15),
	@FechaHoraVenta				DATETIME,
	@FechaHoraEntrega			DATETIME,
	@NumeroComprobante			VARCHAR(100),
	@CodigoEstadoVenta			CHAR(1),
	@CodigoTipoVenta			CHAR(1),
	@CodigoMotivoVenta			CHAR(1),
	@NumeroFactura				VARCHAR(100),
	@MontoTotalVenta			DECIMAL(10,2),
	@MontoTotalPagoEfectivo		DECIMAL(10,2),
	@MontoTotalDescuento		DECIMAL(5,2),
	@NumeroCuentaPorCobrar		INT,
	@VentaParaDistribuir		BIT,
	@CodigoMovilidad			VARCHAR(10),
	@DIPersonaDistribuidor		CHAR(15),
	@Observaciones				TEXT,
	@ProductosDetalleXML		TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	
		EXEC ActualizarVentaProducto @NumeroAlmacen, @NumeroVentaProducto, @CodigoCliente, @DIUsuario, @FechaHoraVenta, @FechaHoraEntrega, @NumeroComprobante, @CodigoEstadoVenta, @CodigoTipoVenta, @CodigoMotivoVenta, @NumeroFactura, @MontoTotalVenta, @MontoTotalPagoEfectivo, @MontoTotalDescuento, @NumeroCuentaPorCobrar, @VentaParaDistribuir, @CodigoMovilidad, @DIPersonaDistribuidor, @Observaciones
		DECLARE @punteroXMLProductosDetalle INT
				
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		--PARA INSERTAR LOS NUEVOS REGISTROS EN LA EDICIÓN 
		------------------------------------------------------------------------------------
		
		INSERT INTO dbo.VentasProductosDetalle(
				NumeroAlmacen,
				NumeroVentaProducto,
				CodigoProducto,
				CantidadVenta,
				PrecioUnitarioVenta,
				CantidadEntregada
				)
		SELECT  @NumeroAlmacen, 
				@NumeroVentaProducto, 
				CodigoProducto,				
				Cantidad,
				PrecioUnitario, 
				Cantidad
		FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductos/VentasProductosDetalle',2)		
		WITH(	
				NumeroAlmacen			INT,
				NumeroVentaServicio		INT,
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2),
				TiempoGarantia			INT			
				
				)
		WHERE CodigoProducto NOT IN(
			SELECT IAD.CodigoProducto
			FROM dbo.VentasProductosDetalle IAD
			WHERE IAD.NumeroAlmacen = @NumeroAlmacen
			AND IAD.NumeroVentaProducto = @NumeroVentaProducto
		)
		
		--ACTUALIZAR LOS REGISTROS
		------------------------------------------------------------------------------------
		UPDATE VentasProductosDetalle
			SET VentasProductosDetalle.PrecioUnitarioVenta = VSDXML.PrecioUnitario,
				VentasProductosDetalle.CantidadVenta = VSDXML.Cantidad
		FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductos/VentasProductosDetalle',2) 
		WITH(	
				NumeroAlmacen			INT,
				NumeroVentaProducto	INT,
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2),
				TiempoGarantia			INT			
				
				) VSDXML
		WHERE VentasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
		AND VentasProductosDetalle.NumeroVentaProducto = @NumeroVentaProducto
		AND VentasProductosDetalle.CodigoProducto = VSDXML.CodigoProducto
		
		--QUITAR LOS REGISTROS QUE FUERON ELIMINADOS
		--------------------------------------------------------------------------
		DELETE
		FROM VentasProductosDetalle
		WHERE CodigoProducto NOT IN
		(
			SELECT  CodigoProducto				
			FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductos/VentasProductosDetalle',2)		
			WITH(
					CodigoProducto			CHAR(15)
				)
		)
		AND VentasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
		AND VentasProductosDetalle.NumeroVentaProducto = @NumeroVentaProducto
		
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		
		DECLARE @MontoTotalVenta2 DECIMAL(10,2)
		SELECT @MontoTotalVenta2 =  SUM(VPD.CantidadVenta * VPD.PrecioUnitarioVenta)
		FROM VentasProductosDetalle VPD
		WHERE VPD.NumeroAlmacen = @NumeroAlmacen
		AND VPD.NumeroVentaProducto = @NumeroVentaProducto
		
		UPDATE VentasProductos
			SET MontoTotalVenta = @MontoTotalVenta2,
				MontoTotalPagoEfectivo = CASE WHEN CodigoTipoVenta = 'E' THEN @MontoTotalVenta2 ELSE MontoTotalPagoEfectivo END
		FROM VentasProductosDetalle VPD		
		WHERE VentasProductos.NumeroAlmacen = @NumeroAlmacen
		AND VentasProductos.NumeroVentaProducto = @NumeroVentaProducto
		
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo Actualizar la Venta de Productos',1,16)	
			ROLLBACK TRAN
		END
		ELSE
			COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarInventarioVentasProductos
GO

CREATE PROCEDURE ActualizarInventarioVentasProductos
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@FechaHoraVenta		DATETIME,
	@EsParaDistribucion	BIT
AS
BEGIN
	BEGIN TRANSACTION 
	
	IF(@EsParaDistribucion = 0)
	BEGIN
		INSERT INTO dbo.VentasProductosDetalleEntrega(NumeroAlmacen, NumeroVentaProducto, CodigoProducto, CantidadEntregada, FechaHoraEntrega, FechaHoraCompraInventario)
		--SELECT IAD.NumeroAlmacen, IAD.NumeroVentaProducto, IAD.CodigoProducto, IAD.CantidadVenta, @FechaHoraVenta, DATEADD(SECOND, 1, @FechaHoraVenta)
		SELECT IAD.NumeroAlmacen, IAD.NumeroVentaProducto, IAD.CodigoProducto, IAD.CantidadVenta, @FechaHoraVenta, GETDATE()
		FROM dbo.VentasProductosDetalle IAD
		WHERE IAD.NumeroAlmacen = @NumeroAlmacen
		AND IAD.NumeroVentaProducto = @NumeroVentaProducto
	END
	
	
	
	
	IF(NOT EXISTS(
		SELECT *
		FROM VentasProductosDetalleEntrega SADE
		INNER JOIN InventariosProductos IA
		ON SADE.NumeroAlmacen = IA.NumeroAlmacen
		AND SADE.CodigoProducto = IA.CodigoProducto
		WHERE SADE.NumeroAlmacen = @NumeroAlmacen
		AND SADE.NumeroVentaProducto = @NumeroVentaProducto
		AND SADE.FechaHoraEntrega = @FechaHoraVenta
		AND SADE.CantidadEntregada > IA.CantidadExistencia
	))
	BEGIN
		
		--UPDATE InventariosProductos
		--	SET CantidadExistencia = CantidadExistencia - SADE.CantidadEntregada
		--FROM dbo.VentasProductosDetalleEntrega SADE
		--WHERE SADE.NumeroAlmacen = @NumeroAlmacen
		--AND SADE.NumeroVentaProducto = @NumeroVentaProducto
		--AND SADE.FechaHoraEntrega = @FechaHoraVenta
		--AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
		--AND InventariosProductos.CodigoProducto = SADE.CodigoProducto
		
		UPDATE InventariosProductos
			SET CantidadExistencia = CantidadExistencia - SADE.CantidadEntregada,
				PrecioValoradoTotal = PrecioValoradoTotal - SADE.PrecioUnitarioIngresoInventario
		FROM 
		(
			SELECT SADE.NumeroAlmacen, SADE.NumeroVentaProducto, SADE.CodigoProducto, 
				   SUM(SADE.CantidadEntregada) AS CantidadEntregada,
				   CASE WHEN DBO.ObtenerCodigoTipoCalculoInventarioProducto(SADE.CodigoProducto) IN ('O') THEN 
						SUM(SADE.CantidadEntregada * VPD.PrecioUnitarioVenta) 
				   ELSE SUM(SADE.CantidadEntregada * SADE.PrecioUnitarioCompraInventario) END
						 AS PrecioUnitarioIngresoInventario
			FROM dbo.VentasProductosDetalleEntrega SADE
			INNER JOIN VentasProductosDetalle VPD
			ON SADE.NumeroAlmacen = VPD.NumeroAlmacen
			AND SADE.NumeroVentaProducto = VPD.NumeroVentaProducto
			AND SADE.CodigoProducto = VPD.CodigoProducto
			WHERE SADE.NumeroAlmacen = @NumeroAlmacen
			AND SADE.NumeroVentaProducto = @NumeroVentaProducto
			AND SADE.FechaHoraEntrega = @FechaHoraVenta
			GROUP BY SADE.NumeroAlmacen, SADE.NumeroVentaProducto, SADE.CodigoProducto		
		)SADE
		WHERE InventariosProductos.NumeroAlmacen = @NumeroAlmacen
		AND InventariosProductos.CodigoProducto = SADE.CodigoProducto
		
		
	    --SE DEBE DISMINUIR E INCLUSO ELIMINAR EL HISTORIAL DE INVENTARIOS
		--DE ACUERDO AL TIPO DE CALCULO CORRESPONDIENTES, 'UEPS','PEUS', ETC				
		DECLARE @TVentaProductosAux	TABLE
		(
			NumeroAlmacen		INT,
			CodigoProducto		CHAR(15),
			CantidadVenta		INT,
			FechaHoraEntrega	DATETIME
		)	
		
		INSERT INTO @TVentaProductosAux (NumeroAlmacen, CodigoProducto, CantidadVenta, FechaHoraEntrega)
		SELECT NumeroAlmacen, CodigoProducto, CantidadEntregada, FechaHoraEntrega
		FROM dbo.VentasProductosDetalleEntrega  SADE
		WHERE SADE.NumeroAlmacen = @NumeroAlmacen
		AND SADE.NumeroVentaProducto = @NumeroVentaProducto
		AND SADE.FechaHoraEntrega = @FechaHoraVenta
		
		
		DECLARE @CodigoProducto				CHAR(15),
				@NumeroAlmacenCP			INT,
				@CantidadVenta				INT
				
		
		SET ROWCOUNT 1
		SELECT @CodigoProducto = CodigoProducto, @CantidadVenta = CantidadVenta, @NumeroAlmacenCP = NumeroAlmacen 
		FROM @TVentaProductosAux				
		WHILE @@rowcount <> 0
		BEGIN
			
			
			EXEC ActualizarEliminarInventarioProductosCantidadesTransaccionesHistorial @NumeroAlmacenCP, @CodigoProducto, @CantidadVenta, @NumeroVentaProducto, @FechaHoraVenta
			
			DELETE @TVentaProductosAux WHERE CodigoProducto = @CodigoProducto
			SET ROWCOUNT 1
			SELECT @CodigoProducto = CodigoProducto, @CantidadVenta = CantidadVenta, @NumeroAlmacenCP = NumeroAlmacen 
			FROM @TVentaProductosAux	
		END
		SET ROWCOUNT 0	
		
		
		UPDATE dbo.VentasProductos
			SET FechaHoraEntrega = @FechaHoraVenta
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroVentaProducto = @NumeroVentaProducto
		
		
		UPDATE dbo.VentasProductosDetalle
			SET CantidadEntregada = ISNULL(VentasProductosDetalle.CantidadEntregada,0) + VPDE.CantidadEntregada
		FROM dbo.VListarVentasProductosDetalleEntrega VPDE
		WHERE VPDE.NumeroAlmacen = @NumeroAlmacen
		AND VPDE.NumeroVentaProducto = @NumeroVentaProducto
		AND VPDE.CodigoProducto = VentasProductosDetalle.CodigoProducto
		AND VentasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
		AND VentasProductosDetalle.NumeroVentaProducto = @NumeroVentaProducto
		
		
		IF(EXISTS (SELECT NumeroVentaProducto FROM VentasProductosDetalle 
					WHERE CantidadVenta <> CantidadEntregada
					AND NumeroAlmacen = @NumeroAlmacen
					AND NumeroVentaProducto = @NumeroVentaProducto) )
		BEGIN
			DECLARE @MontoNuevo DECIMAL(10,2)
			
			SELECT @MontoNuevo = ISNULL(SUM(CantidadEntregada * PrecioUnitarioVenta),0)
			FROM VListarVentasProductosDetalleEntrega VPDE
			WHERE VPDE.NumeroAlmacen = @NumeroAlmacen
			AND VPDE.NumeroVentaProducto = @NumeroVentaProducto
		
		
			UPDATE dbo.VentasProductos
				SET MontoTotalVenta = @MontoNuevo,
					MontoTotalPagoEfectivo = CASE WHEN (CodigoTipoVenta = 'E') THEN @MontoNuevo ELSE MontoTotalPagoEfectivo END			
			WHERE VentasProductos.NumeroAlmacen = @NumeroAlmacen
			AND VentasProductos.NumeroVentaProducto = @NumeroVentaProducto
			
			--PARA ACTUALIZAR LAS VENTAS A CREDITO SI ES POSIBLE, SE MODIFICA EL MONTO TOTAL DE CUENTA POR COBRAR
			DECLARE @NumeroCuentaPorCobrar	INT,
					@Monto					DECIMAL(10,2),
					@MontoTotalVenta		DECIMAL(10,2),
					@MontoTotalPagoEfectivo	DECIMAL(10,2)
			SELECT @NumeroCuentaPorCobrar = NumeroCuentaPorCobrar, @MontoTotalVenta = MontoTotalVenta,
					 @MontoTotalPagoEfectivo = MontoTotalPagoEfectivo
			FROM dbo.VentasProductos 
			WHERE NumeroAlmacen = @NumeroAlmacen 
			AND NumeroVentaProducto = @NumeroVentaProducto
			
			SELECT @Monto = Monto
			FROM dbo.CuentasPorCobrar
			WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
			
			IF(@MontoNuevo > @MontoTotalPagoEfectivo AND (SELECT ISNULL(SUM(Monto),0)
				FROM CuentasPorCobrarCobros WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar)
				+ @MontoTotalPagoEfectivo <= @MontoNuevo )
			BEGIN				
				UPDATE CuentasPorCobrar 
					SET Monto = @MontoNuevo -@MontoTotalPagoEfectivo
				WHERE NumeroCuentaPorCobrar = @NumeroCuentaPorCobrar
				
			END
			
			
			
		END
		
		IF(EXISTS (SELECT NumeroVentaProducto FROM VentasProductosDetalle 
				   WHERE dbo.ObtenerCodigoTipoCalculoInventarioProducto(VentasProductosDetalle.CodigoProducto) IN ('P','U')
					AND VentasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
					AND VentasProductosDetalle.NumeroVentaProducto = @NumeroVentaProducto))
		BEGIN
		
			UPDATE VentasProductosDetalle
				SET PrecioUnitarioVenta = ISNULL((SELECT SUM(SADE.CantidadEntregada * SADE.PrecioUnitarioCompraInventario)/ ISNULL(SUM(SADE.CantidadEntregada), 1)
											FROM VentasProductosDetalleEntrega SADE
											WHERE SADE.NumeroAlmacen = @NumeroAlmacen
											AND SADE.NumeroVentaProducto = @NumeroVentaProducto
											AND SADE.CodigoProducto = VentasProductosDetalle.CodigoProducto), VentasProductosDetalle.PrecioUnitarioVenta)
			WHERE dbo.ObtenerCodigoTipoCalculoInventarioProducto(VentasProductosDetalle.CodigoProducto) IN ('P','U')
			AND VentasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
			AND VentasProductosDetalle.NumeroVentaProducto = @NumeroVentaProducto
		
		
			UPDATE VentasProductos
				SET MontoTotalVenta = ISNULL(( SELECT SUM(SAD.PrecioUnitarioCompraInventario * SAD.CantidadEntregada)
										 FROM VentasProductosDetalleEntrega SAD
										 WHERE NumeroAlmacen = @NumeroAlmacen
										 AND NumeroVentaProducto = @NumeroVentaProducto),0)
			WHERE VentasProductos.NumeroAlmacen = @NumeroAlmacen
			AND VentasProductos.NumeroVentaProducto = @NumeroVentaProducto
		END
		
		IF(EXISTS(
			SELECT *	
			FROM VentasProductosDetalle SAD
			INNER JOIN InventariosProductos IA
			ON SAD.CodigoProducto = IA.CodigoProducto
			AND SAD.NumeroAlmacen = IA.NumeroAlmacen
			WHERE (IA.CantidadExistencia)< IA.StockMinimo
			AND SAD.NumeroAlmacen = @NumeroAlmacen
			AND SAD.NumeroVentaProducto = @NumeroVentaProducto
		))
		BEGIN
			UPDATE InventariosProductos
				SET CantidadRequerida = CantidadRequerida + (StockMinimo - CantidadExistencia)
			FROM dbo.VentasProductosDetalleEntrega SADE
			WHERE SADE.NumeroAlmacen = @NumeroAlmacen
			AND SADE.NumeroVentaProducto = @NumeroVentaProducto
			AND SADE.FechaHoraEntrega = @FechaHoraVenta
			AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
			AND InventariosProductos.CodigoProducto = SADE.CodigoProducto
			AND InventariosProductos.CantidadExistencia < InventariosProductos.StockMinimo
		END
		
		--UPDATE InventariosProductosEspecificos
		--	SET CodigoEstado = 'V'				
		--FROM VentasProductosEspecificos SAE
		--WHERE SAE.NumeroAlmacen = @NumeroAlmacen
		--AND SAE.NumeroVentaProducto = @NumeroVentaProducto
		--AND SAE.FechaHoraEntrega = @FechaHoraVenta
		--AND InventariosProductosEspecificos.NumeroAlmacen = @NumeroAlmacen
		--AND InventariosProductosEspecificos.CodigoProducto = SAE.CodigoProducto
		--AND InventariosProductosEspecificos.CodigoProductoEspecifico = SAE.CodigoProductoEspecifico
		
		--UPDATE VentasProductosEspecificos
		--	SET Entregado = 1
		--WHERE NumeroAlmacen = @NumeroAlmacen
		--AND NumeroVentaProducto = @NumeroVentaProducto
		--AND FechaHoraEntrega = @FechaHoraVenta
	END
	ELSE
	BEGIN
		RAISERROR('No se Pudo Actualizar la Venta de Productos, debido a la Insuficiente existencia de Productos',1,16)	
	END
	
		IF(@@ERROR <> 0)
	BEGIN
		RAISERROR('No se Pudo Actualizar el Venta de Productos',1,16)	
		ROLLBACK TRAN
	END
	ELSE
		COMMIT TRANSACTION

END
GO

DROP PROCEDURE ListarProductosCantidadSuperaStockMinimo
GO
	
CREATE PROCEDURE ListarProductosCantidadSuperaStockMinimo	
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT
AS 
BEGIN
	SELECT ia.CodigoProducto, dbo.ObtenerNombreProducto(IA.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia - SAD.CantidadVenta as CantidadNuevaExistencia, IA.StockMinimo	
	FROM VentasProductosDetalle SAD
	INNER JOIN InventariosProductos IA
	ON SAD.CodigoProducto = IA.CodigoProducto
	AND SAD.NumeroAlmacen = IA.NumeroAlmacen
	WHERE (IA.CantidadExistencia - SAD.CantidadVenta) <= IA.StockMinimo
	AND SAD.NumeroAlmacen = @NumeroAlmacen
	AND SAD.NumeroVentaProducto = @NumeroVentaProducto
END
GO

DROP PROCEDURE ListarProductosExistenciaInsuficiente
GO
	
CREATE PROCEDURE ListarProductosExistenciaInsuficiente	
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT
AS 
BEGIN
	SELECT ia.CodigoProducto, dbo.ObtenerNombreProducto(IA.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia
	FROM VentasProductosDetalle SAD
	INNER JOIN InventariosProductos IA
	ON SAD.CodigoProducto = IA.CodigoProducto
	AND SAD.NumeroAlmacen = IA.NumeroAlmacen
	WHERE SAD.CantidadVenta > IA.CantidadExistencia
	AND SAD.NumeroAlmacen = @NumeroAlmacen
	AND SAD.NumeroVentaProducto = @NumeroVentaProducto
END
GO




DROP PROCEDURE ListarProductosCantidadSuperaStockMinimoXML
GO
	
CREATE PROCEDURE ListarProductosCantidadSuperaStockMinimoXML	
	@NumeroAlmacen			INT,
	@ProductosDetalleXML	TEXT
AS 
BEGIN
	DECLARE @punteroXMLProductosDetalle INT
				
	EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML		

	SELECT  TXML.CodigoProducto, dbo.ObtenerNombreProducto(TXML.CodigoProducto) AS NombreProducto,
	IA.CantidadExistencia - TXML.Cantidad as CantidadNuevaExistencia, IA.StockMinimo	
	FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductos/VentasProductosDetalle',2)		
	WITH(
			CodigoProducto			CHAR(15),
			Cantidad				INT			
			) TXML
	INNER JOIN InventariosProductos IA
	ON TXML.CodigoProducto = IA.CodigoProducto	
	WHERE (IA.CantidadExistencia - TXML.Cantidad) <= IA.StockMinimo
	AND IA.NumeroAlmacen = @NumeroAlmacen
END
GO

DROP PROCEDURE ListarProductosExistenciaInsuficienteXML
GO
	
CREATE PROCEDURE ListarProductosExistenciaInsuficienteXML
	@NumeroAlmacen			INT,
	@ProductosDetalleXML	TEXT
AS 
BEGIN
	DECLARE @punteroXMLProductosDetalle INT
				
	EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML		

	SELECT  TXML.CodigoProducto, dbo.ObtenerNombreProducto(TXML.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia
	FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductos/VentasProductosDetalle',2)		
	WITH(
			CodigoProducto			CHAR(15),
			Cantidad				INT			
			) TXML
	INNER JOIN InventariosProductos IA
	ON TXML.CodigoProducto = IA.CodigoProducto	
	WHERE TXML.Cantidad > IA.CantidadExistencia
	AND IA.NumeroAlmacen = @NumeroAlmacen

END
GO


DROP PROCEDURE ListasVentaProductoReporte
GO
CREATE PROCEDURE ListasVentaProductoReporte
	@NumeroAlmacen					INT,
	@NumeroVentaProducto			INT
AS
BEGIN

	SELECT	SA.NumeroAlmacen, SA.NumeroVentaProducto as NumeroSalidaArticulo, 
			C.NombreCliente as NITFuncionario, C.NombreRepresentante,
			DBO.ObtenerNombreCompleto(U.DIUsuario) AS NombreUsuario, 
			FechaHoraVenta as FechaHoraSalida, FechaHoraEntrega as FechaHoraEntrega,
			NumeroComprobante, CodigoMotivoVenta,
			CASE CodigoTipoVenta WHEN 'E' THEN 'EFECTIVO' WHEN 'R' THEN 'CREDITO' ELSE 'INDETERMINADO' END AS TipoVenta,
			CASE CodigoEstadoVenta WHEN 'I'THEN 'INICIADA' WHEN 'F' THEN 'FINALIZADA' WHEN 'A' THEN 'ANULADA' END AS EstadoSalida, 
			SA.MontoTotalVenta as MontoTotalSalida, 0 as NumeroSolicitudSalidaArticulo, SA.Observaciones,
			'BOLIVIANOS' AS NombreMoneda, 'Bs' AS MascaraMoneda,
			MontoTotalPagoEfectivo + dbo.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroCuentaPorCobrar, 'C') as MontoTotalPagoEfectivo, dbo.ObtenerNombreCompleto(DIPersonaDistribuidor) as NombreDistribuidor,
			M.NombreMovilidad, NumeroFactura, 
			SA.MontoTotalVenta - MontoTotalPagoEfectivo - dbo.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroCuentaPorCobrar, 'C') AS MontoSaldo,
			dbo.ObtenerMontoNumerico_EnTexto(MontoTotalVenta,'BOLIVIANOS') AS MontoTotalTexto, CodigoVentaProducto
	FROM dbo.VentasProductos SA
	INNER JOIN Clientes C
	ON SA.CodigoCliente = C.CodigoCliente
	INNER JOIN Usuarios U
	ON SA.DIUsuario = U.DIUsuario
	LEFT JOIN Movilidades M
	ON SA.CodigoMovilidad = M.CodigoMovilidad
	WHERE SA.NumeroAlmacen = @NumeroAlmacen
	AND NumeroVentaProducto = @NumeroVentaProducto
END
GO

--exec ListasVentaProductoReporte 1, 9

DROP PROCEDURE ActualizarCodigoEstadoVenta
GO

CREATE PROCEDURE ActualizarCodigoEstadoVenta
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoEstadoVenta		CHAR(1)
AS
BEGIN
	UPDATE VentasProductos
		SET CodigoEstadoVenta = @CodigoEstadoVenta
	WHERE (NumeroVentaProducto = @NumeroVentaProducto)
	AND NumeroAlmacen = @NumeroAlmacen
END
GO

DROP PROCEDURE ActualizarVentaDistribucionDatos
GO

CREATE PROCEDURE ActualizarVentaDistribucionDatos
	@NumeroAlmacen			INT,
	@NumeroVentaProducto	INT,
	@CodigoEstadoVenta		CHAR(1),
	@DIPersonaDistribuidor	CHAR(15),
	@VentaParaDistribuir	BIT,
	@CodigoMovilidad		VARCHAR(10)
AS
BEGIN
	UPDATE VentasProductos
		SET CodigoEstadoVenta		= @CodigoEstadoVenta,
			DIPersonaDistribuidor	= @DIPersonaDistribuidor,
			VentaParaDistribuir		= @VentaParaDistribuir,
			CodigoMovilidad			= @CodigoMovilidad		
		
	WHERE (NumeroVentaProducto = @NumeroVentaProducto)
	AND NumeroAlmacen = @NumeroAlmacen
END
GO

--exec dbo.ActualizarVentaDistribucionDatos 1, 1, 'F', '0000000000', 1, '0000000001'
