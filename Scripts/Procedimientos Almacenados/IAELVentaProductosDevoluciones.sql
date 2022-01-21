USE AlvecoComercial10
GO


DROP PROCEDURE InsertarVentaProductoDevolucion
GO
CREATE PROCEDURE InsertarVentaProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@CodigoDevolucionVentaProducto		CHAR(12),	
	@DIUsuario							CHAR(15),	
	@FechaHoraRegistro					DATETIME,	
	@CodigoEstadoVentaDevolucion		CHAR(1),
	@MontoTotalVentaDevolucion			DECIMAL(10,2),
	@MontoTotalPagoEfectivo				DECIMAL(10,2),
	@NumeroVentaProducto				INT,		
	@NumeroAlmacen						INT,	
	@FechaHoraDevolucion				DATETIME,
	@Observaciones						TEXT	
AS
BEGIN
	INSERT INTO dbo.VentasProductosDevoluciones(NumeroAlmacenDevolucion, CodigoDevolucionVentaProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoVentaDevolucion, MontoTotalVentaDevolucion, MontoTotalPagoEfectivo, NumeroVentaProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones)
	VALUES (@NumeroAlmacenDevolucion, dbo.ObtenerCodigoVentaProductoDevolucion(), @DIUsuario, @FechaHoraRegistro, @CodigoEstadoVentaDevolucion, @MontoTotalVentaDevolucion, @MontoTotalPagoEfectivo, @NumeroVentaProducto, @NumeroAlmacen, @FechaHoraDevolucion, @Observaciones)
END
GO


DROP PROCEDURE ActualizarVentaProductoDevolucion
GO
CREATE PROCEDURE ActualizarVentaProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT,
	@CodigoDevolucionVentaProducto		CHAR(12),	
	@DIUsuario							CHAR(15),	
	@FechaHoraRegistro					DATETIME,	
	@CodigoEstadoVentaDevolucion		CHAR(1),
	@MontoTotalVentaDevolucion			DECIMAL(10,2),
	@MontoTotalPagoEfectivo				DECIMAL(10,2),
	@NumeroVentaProducto				INT,		
	@NumeroAlmacen						INT,	
	@FechaHoraDevolucion				DATETIME,
	@Observaciones						TEXT	
AS
BEGIN
	UPDATE 	dbo.VentasProductosDevoluciones
	SET			
		DIUsuario					= @DIUsuario,
		CodigoEstadoVentaDevolucion	= @CodigoEstadoVentaDevolucion,
		MontoTotalVentaDevolucion	= @MontoTotalVentaDevolucion,	
		MontoTotalPagoEfectivo		= @MontoTotalPagoEfectivo,	
		--NumeroVentaProducto			= @NumeroVentaProducto,	
		--NumeroAlmacen				= @NumeroAlmacen,			
		FechaHoraDevolucion			= @FechaHoraDevolucion,
		Observaciones				= @Observaciones				
		
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
END
GO



DROP PROCEDURE EliminarVentaProductoDevolucion
GO
CREATE PROCEDURE EliminarVentaProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT
AS
BEGIN
	BEGIN TRANSACTION
	
	DELETE
	FROM VentasProductosDevolucionesDetalle
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
	
	
	DELETE 
	FROM dbo.VentasProductosDevoluciones
	WHERE NumeroAlmacen = @NumeroAlmacenDevolucion
	AND NumeroVentaProducto = @NumeroVentaProductoDevolucion
	
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR('NO SE PUDO COMPLETAR LA ELIMINACIÓN DE LA TRANSACCIÓN ACTUAL',16,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO



DROP PROCEDURE ListarVentasProductosDevoluciones
GO
CREATE PROCEDURE ListarVentasProductosDevoluciones
AS
BEGIN
	SELECT	NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion, CodigoDevolucionVentaProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoVentaDevolucion, MontoTotalVentaDevolucion, MontoTotalPagoEfectivo, NumeroVentaProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones
	FROM dbo.VentasProductosDevoluciones
	ORDER BY NumeroAlmacen, NumeroVentaProducto
END
GO



DROP PROCEDURE ObtenerVentaProductoDevolucion
GO
CREATE PROCEDURE ObtenerVentaProductoDevolucion
	@NumeroAlmacenDevolucion			INT,
	@NumeroVentaProductoDevolucion		INT
AS
BEGIN
	SELECT	NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion, CodigoDevolucionVentaProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoVentaDevolucion, MontoTotalVentaDevolucion, MontoTotalPagoEfectivo, NumeroVentaProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones
	FROM dbo.VentasProductosDevoluciones
	WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
	AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
END
GO




DROP PROCEDURE InsertarVentaProductoDevolucionXMLDetalle
GO
CREATE PROCEDURE InsertarVentaProductoDevolucionXMLDetalle
	@NumeroAlmacenDevolucion		INT,
	@CodigoDevolucionVentaProducto	CHAR(12),	
	@DIUsuario						CHAR(15),	
	@FechaHoraRegistro				DATETIME,	
	@CodigoEstadoVentaDevolucion	CHAR(1),
	@MontoTotalVentaDevolucion		DECIMAL(10,2),
	@MontoTotalPagoEfectivo			DECIMAL(10,2),
	@NumeroVentaProducto			INT,		
	@NumeroAlmacen					INT,	
	@FechaHoraDevolucion			DATETIME,
	@Observaciones					TEXT,
	@ProductosDetalleXML			TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	INSERT INTO dbo.VentasProductosDevoluciones(NumeroAlmacenDevolucion, CodigoDevolucionVentaProducto, DIUsuario, FechaHoraRegistro, CodigoEstadoVentaDevolucion, MontoTotalVentaDevolucion, MontoTotalPagoEfectivo, NumeroVentaProducto, NumeroAlmacen, FechaHoraDevolucion, Observaciones)
	VALUES (@NumeroAlmacenDevolucion, dbo.ObtenerCodigoVentaProductoDevolucion(), @DIUsuario, @FechaHoraRegistro, @CodigoEstadoVentaDevolucion, @MontoTotalVentaDevolucion, @MontoTotalPagoEfectivo, @NumeroVentaProducto, @NumeroAlmacen, @FechaHoraDevolucion, @Observaciones)
		
		DECLARE @punteroXMLProductosDetalle		INT,
				@NumeroVentaProductoDevolucion	INT
		
		--SET @NumeroVentaProducto = @@IDENTITY
		SET @NumeroVentaProductoDevolucion = SCOPE_IDENTITY()--Devuelve el ultimo id Ingresado en una Tabla con una columna Identidad dentro del Ambito,
		--es Decir en este caso, devolverá el ultimo IDENTITY generado dentro de la Tabla VentasProductos para esta transacción
					
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		
		INSERT INTO dbo.VentasProductosDevolucionesDetalle(
				NumeroAlmacenDevolucion,
				NumeroVentaProductoDevolucion,
				CodigoProducto,
				CantidadVentaDevolucion,
				PrecioUnitarioDevolucion
				)
		SELECT  @NumeroAlmacenDevolucion, 
				@NumeroVentaProductoDevolucion, 
				CodigoProducto, 				
				Cantidad,
				PrecioUnitario
		FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductosDevoluciones/VentasProductosDevolucionesDetalle',2)
		WITH(	
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)				
				)
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		
		DECLARE @MontoTotalVenta2 DECIMAL(10,2)
		SELECT @MontoTotalVenta2 =  SUM(VPD.CantidadVentaDevolucion * VPD.PrecioUnitarioDevolucion)
		FROM VentasProductosDevolucionesDetalle VPD
		WHERE VPD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		
		UPDATE VentasProductosDevoluciones
			SET MontoTotalVentaDevolucion = @MontoTotalVenta2		
		WHERE VentasProductosDevoluciones.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND VentasProductosDevoluciones.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo ingresar la Venta de Productos',16,2)	
			ROLLBACK TRAN
		END
	COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarVentaProductoDevolucionXMLDetalle
GO
CREATE PROCEDURE ActualizarVentaProductoDevolucionXMLDetalle
	@NumeroAlmacenDevolucion		INT,
	@NumeroVentaProductoDevolucion	INT,
	@CodigoDevolucionVentaProducto	CHAR(12),	
	@DIUsuario						CHAR(15),	
	@FechaHoraRegistro				DATETIME,	
	@CodigoEstadoVentaDevolucion	CHAR(1),
	@MontoTotalVentaDevolucion		DECIMAL(10,2),
	@MontoTotalPagoEfectivo			DECIMAL(10,2),
	@NumeroVentaProducto			INT,		
	@NumeroAlmacen					INT,	
	@FechaHoraDevolucion			DATETIME,
	@Observaciones					TEXT,	
	@ProductosDetalleXML			TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	
		EXEC dbo.ActualizarVentaProductoDevolucion @NumeroAlmacenDevolucion, @NumeroVentaProductoDevolucion, null, @DIUsuario, @FechaHoraRegistro, @CodigoEstadoVentaDevolucion, @MontoTotalVentaDevolucion, @MontoTotalPagoEfectivo, @NumeroVentaProducto, @NumeroAlmacen, @FechaHoraDevolucion, @Observaciones
		DECLARE @punteroXMLProductosDetalle INT
				
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		--PARA INSERTAR LOS NUEVOS REGISTROS EN LA EDICIÓN 
		------------------------------------------------------------------------------------
		
		INSERT INTO dbo.VentasProductosDevolucionesDetalle(
				NumeroAlmacenDevolucion,
				NumeroVentaProductoDevolucion,
				CodigoProducto,
				CantidadVentaDevolucion,
				PrecioUnitarioDevolucion
				)
		SELECT  @NumeroAlmacenDevolucion, 
				@NumeroVentaProductoDevolucion, 
				CodigoProducto, 				
				Cantidad,
				PrecioUnitario
		FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductosDevoluciones/VentasProductosDevolucionesDetalle',2)
		WITH(	
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)				
				)
		WHERE CodigoProducto NOT IN(
			SELECT IAD.CodigoProducto
			FROM dbo.VentasProductosDevolucionesDetalle IAD
			WHERE IAD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
			AND IAD.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		)
		
		--ACTUALIZAR LOS REGISTROS
		------------------------------------------------------------------------------------
		UPDATE VentasProductosDevolucionesDetalle
			SET VentasProductosDevolucionesDetalle.PrecioUnitarioDevolucion = VSDXML.PrecioUnitario,
				VentasProductosDevolucionesDetalle.CantidadVentaDevolucion = VSDXML.Cantidad
		FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductosDevoluciones/VentasProductosDevolucionesDetalle',2)
		WITH(	
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)				
		) VSDXML
		WHERE VentasProductosDevolucionesDetalle.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND VentasProductosDevolucionesDetalle.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		AND VentasProductosDevolucionesDetalle.CodigoProducto = VSDXML.CodigoProducto
		
		--QUITAR LOS REGISTROS QUE FUERON ELIMINADOS
		--------------------------------------------------------------------------
		DELETE
		FROM VentasProductosDevolucionesDetalle
		WHERE CodigoProducto NOT IN
		(
			SELECT  CodigoProducto				
			FROM OPENXML (@punteroXMLProductosDetalle, '/VentasProductosDevoluciones/VentasProductosDevolucionesDetalle',2)
			WITH(
					CodigoProducto			CHAR(15)
				)
		)
		AND VentasProductosDevolucionesDetalle.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND VentasProductosDevolucionesDetalle.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		
		DECLARE @MontoTotalVenta2 DECIMAL(10,2)
		SELECT @MontoTotalVenta2 =  SUM(VPD.CantidadVentaDevolucion * VPD.PrecioUnitarioDevolucion)
		FROM VentasProductosDevolucionesDetalle VPD
		WHERE VPD.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND VPD.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		
		UPDATE VentasProductosDevoluciones
			SET MontoTotalVentaDevolucion = @MontoTotalVenta2
		WHERE VentasProductosDevoluciones.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND VentasProductosDevoluciones.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo Actualizar la Venta de Productos',1,16)	
			ROLLBACK TRAN
		END
		ELSE
			COMMIT TRANSACTION
END
GO


DROP PROCEDURE ActualizarInventarioVentasProductosDevolucion
GO

CREATE PROCEDURE ActualizarInventarioVentasProductosDevolucion
	@NumeroAlmacenDevolucion		INT,
	@NumeroVentaProductoDevolucion	INT
AS
BEGIN
	DECLARE @FechaHoraVenta		DATETIME
	SET @FechaHoraVenta = GETDATE()

	BEGIN TRANSACTION 

		UPDATE InventariosProductos
			SET CantidadExistencia = CantidadExistencia + IADE.CantidadVentaDevolucion,
				PrecioUnitarioCompra = CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(IADE.CodigoProducto) 
										--WHEN 'O' THEN dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, IADE.CodigoProducto, null, null) / (CantidadExistencia + IADE.CantidadEntregada)
										WHEN 'O' THEN ISNULL((SELECT CAST( SUM(IACTH.PrecioUnitario * IACTH.CantidadExistente) / SUM(IACTH.CantidadExistente) AS DECIMAL(10,2))
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacenDevolucion
														AND IACTH.CodigoTipoTransaccion = 'C'
														GROUP BY NumeroAlmacen, CodigoProducto
														),IADE.PrecioUnitarioDevolucion)
										WHEN 'P' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacenDevolucion
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.FechaHoraCompra ASC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioDevolucion)
										WHEN 'U' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacenDevolucion
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.FechaHoraCompra DESC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioDevolucion)
										WHEN 'A' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacenDevolucion
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.PrecioUnitario DESC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioDevolucion)
										WHEN 'B' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacenDevolucion
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.PrecioUnitario ASC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioDevolucion)
										END,
										PrecioValoradoTotal = ISNULL(PrecioValoradoTotal,0) + (IADE.CantidadVentaDevolucion * IADE.PrecioUnitarioDevolucion)
		FROM VentasProductosDevolucionesDetalle IADE		
		WHERE IADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND IADE.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion		
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenDevolucion
		AND InventariosProductos.CodigoProducto = IADE.CodigoProducto

		--ANALIZAR ESTE PROCESO PARA DEVOLUCIONES POR VENTAS
		--UPDATE InventariosProductos
		--	SET PrecioUnitarioVentaPorMayor = InventariosProductos.PrecioUnitarioCompra + InventariosProductos.PrecioUnitarioCompra * InventariosProductos.PorcentajeGananciaVentaPorMayor /100,
		--		PrecioUnitarioVentaPorMenor = InventariosProductos.PrecioUnitarioCompra + InventariosProductos.PrecioUnitarioCompra * InventariosProductos.PorcentajeGananciaVentaPorMenor /100
		--FROM ComprasProductosDetalle CPD
		--INNER JOIN Productos P
		--ON P.CodigoProducto = CPD.CodigoProducto
		--WHERE P.ActualizarPrecioVenta = 1
		--AND CPD.NumeroCompraProducto = @NumeroCompraProducto
		--AND CPD.NumeroAlmacen = @NumeroAlmacen
		--AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
		--AND InventariosProductos.CodigoProducto = CPD.CodigoProducto
				
		
		
		UPDATE VentasProductosDevoluciones
			SET FechaHoraDevolucion = @FechaHoraVenta
		WHERE NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
		AND NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
		
		IF(EXISTS(
			SELECT *
			FROM InventariosProductos IA
			INNER JOIN VentasProductosDevolucionesDetalle IADE
			ON IA.NumeroAlmacen = IADE.NumeroAlmacenDevolucion
			AND IA.CodigoProducto = IADE.CodigoProducto
			WHERE IADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
			AND IADE.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion			
			AND IA.CantidadRequerida <> 0	
		))
		BEGIN
			UPDATE InventariosProductos
				SET CantidadRequerida = CantidadRequerida - 
				(CASE WHEN IADE.CantidadVentaDevolucion > InventariosProductos.CantidadRequerida THEN InventariosProductos.CantidadRequerida
				ELSE IADE.CantidadVentaDevolucion END)
			FROM VentasProductosDevolucionesDetalle IADE
			WHERE IADE.NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
			AND IADE.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion			
			AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenDevolucion
			AND InventariosProductos.CodigoProducto = IADE.CodigoProducto 
			AND InventariosProductos.CantidadRequerida <> 0	
		END
		
	IF(@@ERROR <> 0)
	BEGIN
		RAISERROR('No se Pudo Actualizar el Compra de Productos',1,16)	
		ROLLBACK TRAN
	END
	ELSE
		COMMIT TRANSACTION

END
GO




DROP PROCEDURE ListasVentaProductoDevolucionReporte
GO
CREATE PROCEDURE ListasVentaProductoDevolucionReporte
	@NumeroAlmacenDevolucion		INT,
	@NumeroVentaProductoDevolucion	INT
AS
BEGIN

	SELECT SA.NumeroAlmacenDevolucion as NumeroAlmacen, SA.NumeroVentaProductoDevolucion as NumeroSalidaArticuloDevolucion, 
	C.NombreCliente as NITFuncionario, C.NombreRepresentante,
	DBO.ObtenerNombreCompleto(U.DIUsuario) AS NombreUsuario, FechaHoraRegistro, FechaHoraDevolucion ,
	CASE CodigoEstadoVentaDevolucion WHEN 'I'THEN 'INICIADA' WHEN 'F' THEN 'FINALIZADA' WHEN 'A' THEN 'ANULADA' END AS EstadoSalidaDevolucion, 
	SA.MontoTotalVentaDevolucion as MontoTotalDevolucion, VP.NumeroVentaProducto as NumeroSalidaArticulo, SA.Observaciones,
	'BOLIVIANOS' AS NombreMoneda, 'Bs' AS MascaraMoneda, 
	dbo.ObtenerMontoNumerico_EnTexto(MontoTotalVentaDevolucion,'BOLIVIANOS') AS MontoTotalTexto, CodigoEstadoVentaDevolucion
	FROM dbo.VentasProductosDevoluciones SA
	INNER JOIN VentasProductos VP
	ON VP.NumeroAlmacen = SA.NumeroAlmacenDevolucion
	AND VP.NumeroVentaProducto = SA.NumeroVentaProducto
	INNER JOIN Clientes C
	ON VP.CodigoCliente = C.CodigoCliente
	INNER JOIN Usuarios U
	ON SA.DIUsuario = U.DIUsuario
	WHERE SA.NumeroAlmacen = @NumeroAlmacenDevolucion
	AND SA.NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion
END
GO


DROP PROCEDURE ActualizarCodigoEstadoVentaDevolucion
GO

CREATE PROCEDURE ActualizarCodigoEstadoVentaDevolucion
	@NumeroAlmacenDevolucion		INT,
	@NumeroVentaProductoDevolucion	INT,
	@CodigoEstadoVenta				CHAR(1)
AS
BEGIN
	UPDATE VentasProductosDevoluciones
		SET CodigoEstadoVentaDevolucion = @CodigoEstadoVenta
	WHERE (NumeroVentaProductoDevolucion = @NumeroVentaProductoDevolucion)
	AND NumeroAlmacenDevolucion = @NumeroAlmacenDevolucion
END
GO
