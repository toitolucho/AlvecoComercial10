USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCompraProductoDevolucion
GO

CREATE PROCEDURE InsertarCompraProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@CodigoDevolucionCompraProducto		CHAR(12),
	@DIUsuario							CHAR(15),		
	@FechaHoraRegistro					DATETIME,		
	@CodigoEstadoCompraDevolucion		CHAR(1),		
	@MontoTotalCompraDevolucion			DECIMAL(10,2),
	@MontoTotalPagoEfectivo				DECIMAL(10,2),	
	@NumeroCompraProducto				INT,	
	@NumeroAlmacen						INT,			
	@FechaHoraDevolucion				DATETIME,
	@Observaciones						TEXT	
AS
BEGIN
	INSERT INTO dbo.ComprasProductosDevoluciones (NumeroAlmacenDevolucion, CodigoDevolucionCompraProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoCompraDevolucion, MontoTotalCompraDevolucion, MontoTotalPagoEfectivo, NumeroCompraProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones)
	VALUES (@NumeroAlmacenDevolucion, @CodigoDevolucionCompraProducto, @DIUsuario, @FechaHoraRegistro, @CodigoEstadoCompraDevolucion, @MontoTotalCompraDevolucion, @MontoTotalPagoEfectivo, @NumeroCompraProducto, @NumeroAlmacen, @FechaHoraDevolucion, @Observaciones)
END	
GO

DROP PROCEDURE ActualizarCompraProductoDevolucion
GO

CREATE PROCEDURE ActualizarCompraProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT,
	@CodigoDevolucionCompraProducto		CHAR(12),
	@DIUsuario							CHAR(15),		
	@FechaHoraRegistro					DATETIME,		
	@CodigoEstadoCompraDevolucion		CHAR(1),		
	@MontoTotalCompraDevolucion			DECIMAL(10,2),
	@MontoTotalPagoEfectivo				DECIMAL(10,2),	
	@NumeroCompraProducto				INT,	
	@NumeroAlmacen						INT,			
	@FechaHoraDevolucion				DATETIME,
	@Observaciones						TEXT			
AS
BEGIN
	UPDATE dbo.ComprasProductosDevoluciones
	SET					
		DIUsuario						= @DIUsuario,
		FechaHoraRegistro				= @FechaHoraRegistro,
		CodigoEstadoCompraDevolucion	= @CodigoEstadoCompraDevolucion,
		MontoTotalCompraDevolucion		= @MontoTotalCompraDevolucion,
		MontoTotalPagoEfectivo			= @MontoTotalPagoEfectivo,
		--NumeroCompraProducto			= @NumeroCompraProducto,
		--NumeroAlmacen					= @NumeroAlmacen,
		FechaHoraDevolucion				= @FechaHoraDevolucion,
		Observaciones					= @Observaciones
		
	WHERE (NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion)
	AND NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO

DROP PROCEDURE EliminarCompraProductoDevolucion
GO

CREATE PROCEDURE EliminarCompraProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion	INT
AS
BEGIN
	DECLARE @NumeroCuentaPorPagar INT
	
	BEGIN TRANSACTION	
	

		DELETE FROM ComprasProductosDevolucionesDetalle
		WHERE (NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion)
		AND NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion

		DELETE 
		FROM ComprasProductosDevoluciones
		WHERE (NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion)
		AND NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	
	IF(@@ERROR <> 0)
	BEGIN
		RAISERROR('No se Pudo Eliminar la Devolución por la Compra de Productos, seguramente ya existen operaciones realizadas sobre la misma',2,16)	
		ROLLBACK TRAN
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ListarComprasProductosDevolucion
GO

CREATE PROCEDURE ListarComprasProductosDevolucion
AS
BEGIN
	SELECT NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion, CodigoDevolucionCompraProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoCompraDevolucion, MontoTotalCompraDevolucion, MontoTotalPagoEfectivo, NumeroCompraProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones
	FROM ComprasProductosDevoluciones
	ORDER BY NumeroCompraProductoDevolucion
END
GO

DROP PROCEDURE ObtenerCompraProductoDevolucion
GO

CREATE PROCEDURE ObtenerCompraProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion	INT
AS
BEGIN
	SELECT NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion, CodigoDevolucionCompraProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoCompraDevolucion, MontoTotalCompraDevolucion, MontoTotalPagoEfectivo, NumeroCompraProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones
	FROM ComprasProductosDevoluciones
	WHERE (NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion)
	AND NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
END
GO


DROP PROCEDURE InsertarCompraProductoDevolucionXMLDetalle
GO
CREATE PROCEDURE InsertarCompraProductoDevolucionXMLDetalle
	@NumeroAlmacenDevolucion			INT,
	@CodigoDevolucionCompraProducto		CHAR(12),
	@DIUsuario							CHAR(15),		
	@FechaHoraRegistro					DATETIME,		
	@CodigoEstadoCompraDevolucion		CHAR(1),		
	@MontoTotalCompraDevolucion			DECIMAL(10,2),
	@MontoTotalPagoEfectivo				DECIMAL(10,2),	
	@NumeroCompraProducto				INT,	
	@NumeroAlmacen						INT,			
	@FechaHoraDevolucion				DATETIME,
	@Observaciones						TEXT,			
	@ProductosDetalleXML				TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	INSERT INTO dbo.ComprasProductosDevoluciones (NumeroAlmacenDevolucion, CodigoDevolucionCompraProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoCompraDevolucion, MontoTotalCompraDevolucion, MontoTotalPagoEfectivo, NumeroCompraProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones)
	VALUES (@NumeroAlmacenDevolucion, @CodigoDevolucionCompraProducto, @DIUsuario, @FechaHoraRegistro, @CodigoEstadoCompraDevolucion, @MontoTotalCompraDevolucion, @MontoTotalPagoEfectivo, @NumeroCompraProducto, @NumeroAlmacen, @FechaHoraDevolucion, @Observaciones)
		
		DECLARE @punteroXMLProductosDetalle INT,
				@NumeroCompraProductoDevolucion		INT
		
		--SET @NumeroVentaProducto = @@IDENTITY
		SET @NumeroCompraProductoDevolucion = SCOPE_IDENTITY()--Devuelve el ultimo id Ingresado en una Tabla con una columna Identidad dentro del Ambito,
		--es Decir en este caso, devolverá el ultimo IDENTITY generado dentro de la Tabla VentasProductos para esta transacción
					
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		
		INSERT INTO dbo.ComprasProductosDevolucionesDetalle(
				NumeroAlmacenDevolucion,
				NumeroCompraProductoDevolucion,
				CodigoProducto,
				CantidadCompraDevolucion,
				PrecioUnitarioDevolucion				
				)
		SELECT  @NumeroAlmacenDevolucion, 
				@NumeroCompraProductoDevolucion, 
				CodigoProducto, 				
				Cantidad,	
				PrecioUnitario
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductosDevoluciones/ComprasProductosDevolucionesDetalle',2)
		WITH(	
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)
				
				)
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo ingresar la Devolución por la Compra de Productos',1,16)	
			ROLLBACK TRAN
		END
		ELSE
			COMMIT TRANSACTION
END
GO



DROP PROCEDURE ActualizarCompraProductoDevolucionXMLDetalle
GO
CREATE PROCEDURE ActualizarCompraProductoDevolucionXMLDetalle
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion		INT,
	@CodigoDevolucionCompraProducto		CHAR(12),
	@DIUsuario							CHAR(15),		
	@FechaHoraRegistro					DATETIME,		
	@CodigoEstadoCompraDevolucion		CHAR(1),		
	@MontoTotalCompraDevolucion			DECIMAL(10,2),
	@MontoTotalPagoEfectivo				DECIMAL(10,2),	
	@NumeroCompraProducto				INT,	
	@NumeroAlmacen						INT,			
	@FechaHoraDevolucion				DATETIME,
	@Observaciones						TEXT,			
	@ProductosDetalleXML				TEXT
AS
BEGIN
	BEGIN TRANSACTION 
		--INSERT INTO dbo.VentasServicios(NumeroAgencia, DIUsuario, DIPersonaResponsable1, DIPersonaResponsable2, DIPersonaResponsable3, CodigoCliente,  FechaHoraEntregaServicio, CodigoEstadoServicio, CodigoTipoServicio, MontoTotal, NumeroFactura, NumeroCredito, CodigoMoneda, Observaciones)
		--VALUES (@NumeroAgencia, @DIUsuario, @DIPersonaResponsable1, @DIPersonaResponsable2, @DIPersonaResponsable3, @CodigoCliente, @FechaHoraVentaServicio, @CodigoEstadoServicio, @CodigoTipoServicio, @MontoTotal, @NumeroFactura, @NumeroCredito, @CodigoMoneda, @Observaciones)
		
		DECLARE @MontoTotalCompra2		DECIMAL(10,2),
				@CodigoTipoCompra2		CHAR(1),
				@NumeroCuentaPorPagar2	INT
				
		EXEC ActualizarCompraProductoDevolucion @NumeroAlmacenDevolucion, @NumeroCompraProductoDevolucion, @CodigoDevolucionCompraProducto, @DIUsuario, @FechaHoraRegistro, @CodigoEstadoCompraDevolucion, @MontoTotalCompraDevolucion, @MontoTotalPagoEfectivo, @NumeroCompraProducto, @NumeroAlmacen, @FechaHoraDevolucion, @Observaciones
		DECLARE @punteroXMLProductosDetalle INT
				
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		--PARA INSERTAR LOS NUEVOS REGISTROS EN LA EDICIÓN 
		------------------------------------------------------------------------------------
		
		INSERT INTO dbo.ComprasProductosDevolucionesDetalle(
				NumeroAlmacenDevolucion,		
				NumeroCompraProductoDevolucion,
				CodigoProducto,
				CantidadCompraDevolucion,		
				PrecioUnitarioDevolucion
				)
		SELECT  @NumeroAlmacen, 
				@NumeroCompraProductoDevolucion, 
				CodigoProducto,				
				Cantidad, 
				PrecioUnitario
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductosDevoluciones/ComprasProductosDevolucionesDetalle',2)		
		WITH(	
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)				
				)
		WHERE CodigoProducto NOT IN(
			SELECT IAD.CodigoProducto
			FROM ComprasProductosDevolucionesDetalle IAD
			WHERE IAD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
			AND IAD.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		)
		
		--ACTUALIZAR LOS REGISTROS
		------------------------------------------------------------------------------------
		UPDATE ComprasProductosDevolucionesDetalle
			SET ComprasProductosDevolucionesDetalle.PrecioUnitarioDevolucion = VSDXML.PrecioUnitario,
				ComprasProductosDevolucionesDetalle.CantidadCompraDevolucion = VSDXML.Cantidad
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductosDevoluciones/ComprasProductosDevolucionesDetalle',2)		
		WITH(	
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)				
			) VSDXML
		WHERE ComprasProductosDevolucionesDetalle.NumeroAlmacenDevolucion = @NumeroAlmacen
		AND ComprasProductosDevolucionesDetalle.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		AND ComprasProductosDevolucionesDetalle.CodigoProducto = VSDXML.CodigoProducto
		
		--QUITAR LOS REGISTROS QUE FUERON ELIMINADOS
		--------------------------------------------------------------------------
		DELETE
		FROM ComprasProductosDevolucionesDetalle
		WHERE CodigoProducto NOT IN
		(
			SELECT  CodigoProducto				
			FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductosDevoluciones/ComprasProductosDevolucionesDetalle',2)
			WITH(
					CodigoProducto			CHAR(15)
				)
		)
		AND ComprasProductosDevolucionesDetalle.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND ComprasProductosDevolucionesDetalle.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo Actualizar la Devolución por la Compra de Productos',1,16)	
			ROLLBACK TRAN
		END
		ELSE
			COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarInventarioComprasProductosDevolucion
GO

CREATE PROCEDURE ActualizarInventarioComprasProductosDevolucion
	@NumeroAlmacenDevolucion		INT,
	@NumeroCompraProductoDevolucion	INT
AS
BEGIN
	DECLARE @FechaHoraVenta	DATETIME 
	SET @FechaHoraVenta = GETDATE()
BEGIN TRANSACTION 
	
	IF(NOT EXISTS(
		SELECT *
		FROM ComprasProductosDevolucionesDetalle SADE
		INNER JOIN InventariosProductos IA
		ON SADE.NumeroAlmacenDevolucion = IA.NumeroAlmacen
		AND SADE.CodigoProducto = IA.CodigoProducto
		WHERE SADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND SADE.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		AND SADE.CantidadCompraDevolucion > IA.CantidadExistencia
	))
	BEGIN
		
		UPDATE InventariosProductos
			SET CantidadExistencia = CantidadExistencia - SADE.CantidadCompraDevolucion,
				PrecioValoradoTotal = PrecioValoradoTotal - (SADE.CantidadCompraDevolucion * SADE.PrecioUnitarioDevolucion)
				--PrecioValoradoTotal = PrecioValoradoTotal - SADE.PrecioUnitarioIngresoInventario
		FROM dbo.ComprasProductosDevolucionesDetalle SADE
		WHERE SADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND SADE.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenDevolucion
		AND InventariosProductos.CodigoProducto = SADE.CodigoProducto
		
	    --SE DEBE DISMINUIR E INCLUSO ELIMINAR EL HISTORIAL DE INVENTARIOS
		--DE ACUERDO AL TIPO DE CALCULO CORRESPONDIENTES, 'UEPS','PEUS', ETC				
		DECLARE @TCompraProductosDevolucion	TABLE
		(
			NumeroAlmacen		INT,
			CodigoProducto		CHAR(15),
			CantidadVenta		INT
		)	
		
		INSERT INTO @TCompraProductosDevolucion (NumeroAlmacen, CodigoProducto, CantidadVenta)
		SELECT NumeroAlmacenDevolucion, CodigoProducto, CantidadCompraDevolucion
		FROM dbo.ComprasProductosDevolucionesDetalle  SADE
		WHERE SADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND SADE.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		
		DECLARE @CodigoProducto				CHAR(15),
				@NumeroAlmacenCP			INT,
				@CantidadVenta				INT
				
		
		SET ROWCOUNT 1
		SELECT @CodigoProducto = CodigoProducto, @CantidadVenta = CantidadVenta, @NumeroAlmacenCP = NumeroAlmacen 
		FROM @TCompraProductosDevolucion				
		WHILE @@rowcount <> 0
		BEGIN
			
			
			EXEC ActualizarEliminarInventarioProductosCantidadesTransaccionesHistorial @NumeroAlmacenCP, @CodigoProducto, @CantidadVenta, @NumeroCompraProductoDevolucion, @FechaHoraVenta
			
			DELETE @TCompraProductosDevolucion WHERE CodigoProducto = @CodigoProducto
			SET ROWCOUNT 1
			SELECT @CodigoProducto = CodigoProducto, @CantidadVenta = CantidadVenta, @NumeroAlmacenCP = NumeroAlmacen 
			FROM @TCompraProductosDevolucion	
		END
		SET ROWCOUNT 0	
		
		
		UPDATE dbo.ComprasProductosDevoluciones
			SET FechaHoraDevolucion = @FechaHoraVenta
		WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		
		
		--REVISAR SI REALMENTE SE TIENE QUE HACER ESTAS ACCIONES
		--IF(EXISTS (SELECT * FROM ComprasProductosDevolucionesDetalle 
		--		   WHERE dbo.ObtenerCodigoTipoCalculoInventarioProducto(ComprasProductosDevolucionesDetalle.CodigoProducto) IN ('P','U')
		--			AND ComprasProductosDevolucionesDetalle.NumeroAlmacenDevolucion = @NumeroCompraProductoDevolucion
		--			AND ComprasProductosDevolucionesDetalle.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion))
		--BEGIN
		
		--	UPDATE VentasProductosDetalle
		--		SET PrecioUnitarioVenta = ISNULL((SELECT SUM(SADE.CantidadEntregada * SADE.PrecioUnitarioCompraInventario)/ ISNULL(SUM(SADE.CantidadEntregada), 1)
		--									FROM VentasProductosDetalleEntrega SADE
		--									WHERE SADE.NumeroAlmacen = @NumeroAlmacen
		--									AND SADE.NumeroVentaProducto = @NumeroVentaProducto
		--									AND SADE.CodigoProducto = VentasProductosDetalle.CodigoProducto), PrecioUnitarioVenta)
		--	WHERE dbo.ObtenerCodigoTipoCalculoInventarioProducto(VentasProductosDetalle.CodigoProducto) IN ('P','U')
		--	AND VentasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
		--	AND VentasProductosDetalle.NumeroVentaProducto = @NumeroVentaProducto
		
		
		--	UPDATE VentasProductos
		--		SET MontoTotalVenta = ISNULL(( SELECT SUM(SAD.PrecioUnitarioCompraInventario * SAD.CantidadEntregada)
		--								 FROM VentasProductosDetalleEntrega SAD
		--								 WHERE NumeroAlmacen = @NumeroAlmacen
		--								 AND NumeroVentaProducto = @NumeroVentaProducto),0)
		--	WHERE VentasProductos.NumeroAlmacen = @NumeroAlmacen
		--	AND VentasProductos.NumeroVentaProducto = @NumeroVentaProducto
		--END
		
		IF(EXISTS(
			SELECT *	
			FROM ComprasProductosDevolucionesDetalle SAD
			INNER JOIN InventariosProductos IA
			ON SAD.CodigoProducto = IA.CodigoProducto
			AND SAD.NumeroAlmacenDevolucion = IA.NumeroAlmacen
			WHERE (IA.CantidadExistencia)< IA.StockMinimo
			AND SAD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
			AND SAD.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
		))
		BEGIN
			UPDATE InventariosProductos
				SET CantidadRequerida = CantidadRequerida + (StockMinimo - CantidadExistencia)
			FROM dbo.ComprasProductosDevolucionesDetalle SADE
			WHERE SADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
			AND SADE.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion			
			AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenDevolucion
			AND InventariosProductos.CodigoProducto = SADE.CodigoProducto
			AND InventariosProductos.CantidadExistencia < InventariosProductos.StockMinimo
		END
		
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


DROP PROCEDURE ListarCompraProductoDevolucionReporte
GO

CREATE PROCEDURE ListarCompraProductoDevolucionReporte
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion	INT

AS 
BEGIN
	SELECT IA.NumeroAlmacenDevolucion as NumeroAlmacen, IA.NumeroCompraProductoDevolucion AS NumeroSalidaArticuloDevolucion, 
	P.NombreRazonSocial as NITFuncionario, P.NombreRepresentante as NombreOficina,
	IA.DIUsuario, DBO.ObtenerNombreCompleto(IA.DIUsuario) AS NombreUsuario, IA.FechaHoraDevolucion, IA.FechaHoraRegistro, 
	CASE CodigoEstadoCompraDevolucion WHEN 'I' THEN 'INICIADO' WHEN 'A' THEN 'ANULADO' 
	WHEN 'F' THEN 'FINALIZADO' WHEN 'X' THEN 'FINALIZADO INCOMPLETO'END AS EstadoSalidaDevolucion, 
	MontoTotalCompraDevolucion AS MontoTotalDevolucion, IA.NumeroCompraProducto as NumeroSalidaArticulo, IA.Observaciones,
	'BOLIVIANOS' AS NombreMoneda, 'Bs' AS MascaraMoneda,
	dbo.ObtenerMontoNumerico_EnTexto(MontoTotalCompraDevolucion,'BOLIVIANOS') AS MontoTotalTexto,
	P.NITProveedor
	FROM dbo.ComprasProductosDevoluciones IA
	INNER JOIN ComprasProductos CP
	ON CP.NumeroAlmacen = IA.NumeroAlmacenDevolucion
	AND CP.NumeroCompraProducto = IA.NumeroCompraProducto
	INNER JOIN Proveedores P
	ON CP.CodigoProveedor = P.CodigoProveedor
	INNER JOIN Usuarios U
	ON IA.DIUsuario = U.DIUsuario
	WHERE (IA.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion)
	AND IA.NumeroAlmacen = @NumeroAlmacenDevolucion
		
END
GO


DROP PROCEDURE ActualizarCodigoEstadoCompraDevolucion
GO

CREATE PROCEDURE ActualizarCodigoEstadoCompraDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroCompraProductoDevolucion	INT,
	@CodigoEstadoCompra		CHAR(1)
AS
BEGIN
	UPDATE ComprasProductosDevoluciones
		SET CodigoEstadoCompraDevolucion = @CodigoEstadoCompra
	WHERE (NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion)
	AND NumeroAlmacen = @NumeroAlmacenDevolucion
END
GO



DROP PROCEDURE ListarProductosCantidadSuperaStockMinimoDevolucion
GO
	
CREATE PROCEDURE ListarProductosCantidadSuperaStockMinimoDevolucion
	@NumeroAlmacenDevolucion		INT,
	@NumeroCompraProductoDevolucion	INT
AS 
BEGIN
	SELECT ia.CodigoProducto, dbo.ObtenerNombreProducto(IA.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia - SAD.CantidadCompraDevolucion as CantidadNuevaExistencia, IA.StockMinimo	
	FROM ComprasProductosDevolucionesDetalle SAD
	INNER JOIN InventariosProductos IA
	ON SAD.CodigoProducto = IA.CodigoProducto
	AND SAD.NumeroAlmacenDevolucion = IA.NumeroAlmacen
	WHERE (IA.CantidadExistencia - SAD.CantidadCompraDevolucion) <= IA.StockMinimo
	AND SAD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND SAD.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO

DROP PROCEDURE ListarProductosExistenciaInsuficienteDevolucion
GO
	
CREATE PROCEDURE ListarProductosExistenciaInsuficienteDevolucion
	@NumeroAlmacenDevolucion		INT,
	@NumeroCompraProductoDevolucion	INT
AS 
BEGIN
	SELECT ia.CodigoProducto, dbo.ObtenerNombreProducto(IA.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia
	FROM ComprasProductosDevolucionesDetalle SAD
	INNER JOIN InventariosProductos IA
	ON SAD.CodigoProducto = IA.CodigoProducto
	AND SAD.NumeroAlmacenDevolucion = IA.NumeroAlmacen
	WHERE SAD.CantidadCompraDevolucion > IA.CantidadExistencia
	AND SAD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND SAD.NumeroCompraProductoDevolucion = @NumeroCompraProductoDevolucion
END
GO


