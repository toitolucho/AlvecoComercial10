USE AlvecoComercial10
GO

DROP PROCEDURE InsertarCompraProducto
GO

CREATE PROCEDURE InsertarCompraProducto
	@NumeroAlmacen			INT,
	@CodigoProveedor		INT,
	@DIUsuario				CHAR(15),
	@CodigoTipoCompra		CHAR(1),
	@CodigoEstadoCompra	CHAR(1),
	@NumeroFactura			VARCHAR(100),
	@NumeroComprobante		VARCHAR(100),
	@MontoTotalCompra		DECIMAL(10,2),
	@MontoTotalPagoEfectivo DECIMAL(10,2),
	@NumeroCuentaPorPagar	INT,
	@Observaciones			TEXT
AS
BEGIN
	INSERT INTO dbo.ComprasProductos (NumeroAlmacen, CodigoCompraProducto, CodigoProveedor, DIUsuario, CodigoTipoCompra, CodigoEstadoCompra, NumeroFactura, NumeroComprobante, MontoTotalCompra, MontoTotalPagoEfectivo, NumeroCuentaPorPagar, Observaciones)
	VALUES (@NumeroAlmacen, dbo.ObtenerCodigoCompraProducto(), @CodigoProveedor, @DIUsuario, @CodigoTipoCompra, @CodigoEstadoCompra, @NumeroFactura, @NumeroComprobante, @MontoTotalCompra, @MontoTotalPagoEfectivo, @NumeroCuentaPorPagar, @Observaciones)
END	
GO

DROP PROCEDURE ActualizarCompraProducto
GO

CREATE PROCEDURE ActualizarCompraProducto
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProveedor		INT,
	@DIUsuario				CHAR(15),
	@CodigoTipoCompra		CHAR(1),
	@CodigoEstadoCompra		CHAR(1),
	@NumeroFactura			VARCHAR(100),
	@NumeroComprobante		VARCHAR(100),
	@MontoTotalCompra		DECIMAL(10,2),
	@MontoTotalPagoEfectivo	DECIMAL(10,2),
	@NumeroCuentaPorPagar	INT,
	@Observaciones			TEXT
AS
BEGIN
	UPDATE dbo.ComprasProductos
	SET					
		CodigoProveedor			= @CodigoProveedor,
		DIUsuario				= @DIUsuario,
		CodigoTipoCompra		= @CodigoTipoCompra,
		CodigoEstadoCompra		= @CodigoEstadoCompra,
		NumeroFactura			= @NumeroFactura,
		NumeroComprobante		= @NumeroComprobante,
		MontoTotalCompra		= @MontoTotalCompra,
		MontoTotalPagoEfectivo	= @MontoTotalPagoEfectivo,
		NumeroCuentaPorPagar	= @NumeroCuentaPorPagar,
		Observaciones			= @Observaciones
	WHERE (NumeroCompraProducto = @NumeroCompraProducto)
	AND NumeroAlmacen = @NumeroAlmacen
END
GO

DROP PROCEDURE EliminarCompraProducto
GO

CREATE PROCEDURE EliminarCompraProducto
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT
AS
BEGIN
	DECLARE @NumeroCuentaPorPagar INT
	
	BEGIN TRANSACTION	
	
		SELECT @NumeroCuentaPorPagar = NumeroCuentaPorPagar
		FROM ComprasProductos 
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroCuentaPorPagar = @NumeroCompraProducto

		DELETE FROM CuentasPorPagarPagos
		WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
		
		DELETE FROM CuentasPorPagar
		WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar

		DELETE FROM ComprasProductosDetalle
		WHERE (NumeroCompraProducto = @NumeroCompraProducto)
		AND NumeroAlmacen = @NumeroAlmacen

		DELETE 
		FROM ComprasProductos
		WHERE (NumeroCompraProducto = @NumeroCompraProducto)
		AND NumeroAlmacen = @NumeroAlmacen
	
	IF(@@ERROR <> 0)
	BEGIN
		RAISERROR('No se Pudo Eliminar la Compra de Productos, seguramente ya existen operaciones realizadas sobre la misma',2,16)	
		ROLLBACK TRAN
	END
	ELSE
		COMMIT TRANSACTION
END
GO

DROP PROCEDURE ListarComprasProductos
GO

CREATE PROCEDURE ListarComprasProductos
AS
BEGIN
	SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProveedor, DIUsuario, FechaHoraRegistro, FechaHoraRecepcion, CodigoTipoCompra, CodigoEstadoCompra, NumeroFactura, NumeroComprobante, MontoTotalCompra, MontoTotalPagoEfectivo, NumeroCuentaPorPagar, Observaciones
	FROM ComprasProductos
	ORDER BY NumeroCompraProducto
END
GO

DROP PROCEDURE ObtenerCompraProducto
GO

CREATE PROCEDURE ObtenerCompraProducto
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT
AS
BEGIN
	SELECT NumeroAlmacen, NumeroCompraProducto, CodigoProveedor, DIUsuario, FechaHoraRegistro, FechaHoraRecepcion, CodigoTipoCompra, CodigoEstadoCompra, NumeroFactura, NumeroComprobante, MontoTotalCompra, MontoTotalPagoEfectivo, NumeroCuentaPorPagar, Observaciones
	FROM ComprasProductos
	WHERE (NumeroCompraProducto = @NumeroCompraProducto)
	AND NumeroAlmacen = @NumeroAlmacen
END
GO


DROP PROCEDURE InsertarCompraProductoXMLDetalle
GO
CREATE PROCEDURE InsertarCompraProductoXMLDetalle
	@NumeroAlmacen			INT,
	@CodigoProveedor		INT,
	@DIUsuario				CHAR(15),
	@CodigoTipoCompra		CHAR(1),
	@CodigoEstadoCompra		CHAR(1),
	@NumeroFactura			VARCHAR(100),
	@NumeroComprobante		VARCHAR(100),
	@MontoTotalCompra		DECIMAL(10,2),
	@MontoTotalPagoEfectivo DECIMAL(10,2),
	@NumeroCuentaPorPagar	INT,
	@Observaciones			TEXT,
	@ProductosDetalleXML		TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	INSERT INTO dbo.ComprasProductos (NumeroAlmacen, CodigoCompraProducto, CodigoProveedor, DIUsuario, CodigoTipoCompra, CodigoEstadoCompra, NumeroFactura, NumeroComprobante, MontoTotalCompra, MontoTotalPagoEfectivo, NumeroCuentaPorPagar, Observaciones, FechaHoraRegistro)
	VALUES (@NumeroAlmacen,  dbo.ObtenerCodigoCompraProducto(),@CodigoProveedor, @DIUsuario, @CodigoTipoCompra, @CodigoEstadoCompra, @NumeroFactura, @NumeroComprobante, @MontoTotalCompra, @MontoTotalPagoEfectivo, @NumeroCuentaPorPagar, @Observaciones, GETDATE())
		
		
		
		DECLARE @punteroXMLProductosDetalle INT,
				@NumeroCompraProducto		INT
		
		--SET @NumeroVentaProducto = @@IDENTITY
		SET @NumeroCompraProducto = SCOPE_IDENTITY()--Devuelve el ultimo id Ingresado en una Tabla con una columna Identidad dentro del Ambito,
		--es Decir en este caso, devolverá el ultimo IDENTITY generado dentro de la Tabla VentasProductos para esta transacción
		
		
		IF(@CodigoTipoCompra = 'E')
			UPDATE dbo.ComprasProductos
				SET MontoTotalPagoEfectivo = @MontoTotalCompra			
			WHERE NumeroAlmacen = @NumeroAlmacen
			AND NumeroCompraProducto = @NumeroCompraProducto
					
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		
		INSERT INTO dbo.ComprasProductosDetalle(
				NumeroAlmacen,
				NumeroCompraProducto,
				CodigoProducto,
				CantidadCompra,
				CantidadEntregada,
				PrecioUnitarioCompra,
				TiempoGarantiaCompra
				)
		SELECT  @NumeroAlmacen, 
				@NumeroCompraProducto, 
				CodigoProducto, 				
				Cantidad,
				0, 				
				PrecioUnitario, 
				TiempoGarantia
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductos/ComprasProductosDetalle',2)
		WITH(	
				NumeroAgencia			INT,
				NumeroCompraProducto	INT,
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2),
				TiempoGarantia			INT			
				
				)
				
				
		UPDATE dbo.InventariosProductos
			SET PorcentajeGananciaVentaPorMayor = TA.PorcentajeGananciaVentaPorMayor,
				PorcentajeGananciaVentaPorMenor = TA.PorcentajeGananciaVentaPorMenor
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductos/ComprasProductosDetalle',2)
		WITH(	
				CodigoProducto					CHAR(15),
				PorcentajeGananciaVentaPorMayor	DECIMAL(10,2),
				PorcentajeGananciaVentaPorMenor	DECIMAL(10,2)
				
		)TA
		WHERE InventariosProductos.CodigoProducto = TA.CodigoProducto
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
			
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo ingresar el Compra de Productos',1,16)	
			ROLLBACK TRAN
		END
		ELSE
			COMMIT TRANSACTION
END
GO



DROP PROCEDURE ActualizarCompraProductoXMLDetalle
GO
CREATE PROCEDURE ActualizarCompraProductoXMLDetalle
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoProveedor		INT,
	@DIUsuario				CHAR(15),
	@CodigoTipoCompra		CHAR(1),
	@CodigoEstadoCompra	CHAR(1),
	@NumeroFactura			VARCHAR(100),
	@NumeroComprobante		VARCHAR(100),
	@MontoTotalCompra		DECIMAL(10,2),
	@MontoTotalPagoEfectivo	DECIMAL(10,2),
	@NumeroCuentaPorPagar	INT,
	@Observaciones			TEXT,
	@ProductosDetalleXML	TEXT
AS
BEGIN
	BEGIN TRANSACTION 
		--INSERT INTO dbo.VentasServicios(NumeroAgencia, DIUsuario, DIPersonaResponsable1, DIPersonaResponsable2, DIPersonaResponsable3, CodigoCliente,  FechaHoraEntregaServicio, CodigoEstadoServicio, CodigoTipoServicio, MontoTotal, NumeroFactura, NumeroCredito, CodigoMoneda, Observaciones)
		--VALUES (@NumeroAgencia, @DIUsuario, @DIPersonaResponsable1, @DIPersonaResponsable2, @DIPersonaResponsable3, @CodigoCliente, @FechaHoraVentaServicio, @CodigoEstadoServicio, @CodigoTipoServicio, @MontoTotal, @NumeroFactura, @NumeroCredito, @CodigoMoneda, @Observaciones)
		
		DECLARE @MontoTotalCompra2		DECIMAL(10,2),
				@CodigoTipoCompra2		CHAR(1),
				@NumeroCuentaPorPagar2	INT
				
		SELECT TOP(1)@NumeroCuentaPorPagar2 = NumeroCuentaPorPagar, @MontoTotalCompra2 = MontoTotalCompra, @CodigoTipoCompra2 = CodigoTipoCompra
		FROM ComprasProductos 
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroCompraProducto = @NumeroCompraProducto
		
		
		--SI MODIFICO LA COMPRA A CREDITO A EFECTIVO, DEBEMOS BORRAR LAS CUENTAS POR PAGAR
		IF(@CodigoTipoCompra = 'E' AND @CodigoTipoCompra2 = 'R')
		BEGIN	
			--EXEC DBO.EliminarCuentaPorPagar @NumeroCuentaPorPagar2
			SET @MontoTotalPagoEfectivo = @MontoTotalCompra			
		END
		
		IF(@CodigoTipoCompra = 'R' AND @CodigoTipoCompra2 = 'R'
		AND @NumeroCuentaPorPagar IS NOT NULL)
		BEGIN
			UPDATE CuentasPorPagar
				SET CodigoProveedor = @CodigoProveedor,
					Monto = @MontoTotalCompra - @MontoTotalPagoEfectivo
			WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar
		END
		--select * from CuentasPorPagar
		--select *from ComprasProductos
		--select *from ComprasProductosDetalle
		
		EXEC ActualizarCompraProducto @NumeroAlmacen, @NumeroCompraProducto, @CodigoProveedor, @DIUsuario, @CodigoTipoCompra, @CodigoEstadoCompra, @NumeroFactura, @NumeroComprobante, @MontoTotalCompra, @MontoTotalPagoEfectivo, @NumeroCuentaPorPagar, @Observaciones
		DECLARE @punteroXMLProductosDetalle INT
				
		
		--SET @NumeroVentaProducto = @@IDENTITY
		--SET @NumeroCompraProducto = SCOPE_IDENTITY()--Devuelve el ultimo id Ingresado en una Tabla con una columna Identidad dentro del Ambito,
		--es Decir en este caso, devolverá el ultimo IDENTITY generado dentro de la Tabla VentasProductos para esta transacción
					
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		--PARA INSERTAR LOS NUEVOS REGISTROS EN LA EDICIÓN 
		------------------------------------------------------------------------------------
		
		INSERT INTO dbo.ComprasProductosDetalle(
				NumeroAlmacen,
				NumeroCompraProducto,
				CodigoProducto,
				CantidadCompra,
				CantidadEntregada,				
				PrecioUnitarioCompra,
				TiempoGarantiaCompra
				)
		SELECT  @NumeroAlmacen, 
				@NumeroCompraProducto, 
				CodigoProducto,				
				Cantidad, 
				0 AS CantidadEntregada,
				PrecioUnitario, 
				TiempoGarantia
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductos/ComprasProductosDetalle',2)		
		WITH(	
				NumeroAgencia			INT,
				NumeroVentaServicio		INT,
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2),
				TiempoGarantia			INT			
				
				)
		WHERE CodigoProducto NOT IN(
			SELECT IAD.CodigoProducto
			FROM ComprasProductosDetalle IAD
			WHERE IAD.NumeroAlmacen = @NumeroAlmacen
			AND IAD.NumeroCompraProducto = @NumeroCompraProducto
		)
		
		--ACTUALIZAR LOS REGISTROS
		------------------------------------------------------------------------------------
		UPDATE ComprasProductosDetalle
			SET ComprasProductosDetalle.PrecioUnitarioCompra = VSDXML.PrecioUnitario,
				ComprasProductosDetalle.CantidadCompra = VSDXML.Cantidad,
				ComprasProductosDetalle.TiempoGarantiaCompra = VSDXML.TiempoGarantia
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductos/ComprasProductosDetalle',2) 
		WITH(	
				NumeroAlmacen			INT,
				NumeroCompraProducto	INT,
				CodigoProducto			CHAR(15),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2),
				TiempoGarantia			INT			
				
				) VSDXML
		WHERE ComprasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
		AND ComprasProductosDetalle.NumeroCompraProducto = @NumeroCompraProducto
		AND ComprasProductosDetalle.CodigoProducto = VSDXML.CodigoProducto
		
		--QUITAR LOS REGISTROS QUE FUERON ELIMINADOS
		--------------------------------------------------------------------------
		DELETE
		FROM ComprasProductosDetalle
		WHERE CodigoProducto NOT IN
		(
			SELECT  CodigoProducto				
			FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductos/ComprasProductosDetalle',2)		
			WITH(
					CodigoProducto			CHAR(15)
				)
		)
		AND ComprasProductosDetalle.NumeroAlmacen = @NumeroAlmacen
		AND ComprasProductosDetalle.NumeroCompraProducto = @NumeroCompraProducto
		
		UPDATE dbo.InventariosProductos
			SET PorcentajeGananciaVentaPorMayor = TA.PorcentajeGananciaVentaPorMayor,
				PorcentajeGananciaVentaPorMenor = TA.PorcentajeGananciaVentaPorMenor
		FROM OPENXML (@punteroXMLProductosDetalle, '/ComprasProductos/ComprasProductosDetalle',2)
		WITH(	
				CodigoProducto					CHAR(15),
				PorcentajeGananciaVentaPorMayor	DECIMAL(10,2),
				PorcentajeGananciaVentaPorMenor	DECIMAL(10,2)
				
		)TA
		WHERE InventariosProductos.CodigoProducto = TA.CodigoProducto
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
		
		
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		
		
		IF(@CodigoTipoCompra = 'E' AND @CodigoTipoCompra2 = 'R')
		BEGIN	
			EXEC DBO.EliminarCuentaPorPagar @NumeroCuentaPorPagar2
			--SET @MontoTotalPagoEfectivo = @MontoTotalCompra
			
			--DELETE FROM DBO.CuentasPorPagarPagos
			--WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar2
			
			--DELETE FROM dbo.CuentasPorPagar 
			--WHERE NumeroCuentaPorPagar = @NumeroCuentaPorPagar2
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

DROP PROCEDURE ActualizarInventarioComprasProductos
GO

CREATE PROCEDURE ActualizarInventarioComprasProductos
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@FechaHoraEntrega		DATETIME
AS
BEGIN
	BEGIN TRANSACTION 

	------------------------------------------------------------------------------------------------------------------------------------------------------------
	--SECTOR QUE SALTA LA RECEPCIÓN POR PARTES, ESTE CODIGO LO HACE DIRECTO
	------------------------------------------------------------------------------------------------------------------------------------------------------------
	--INSERT INTO dbo.ComprasProductosDetalleEntrega(NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CantidadEntregada, FechaHoraEntrega)
	--SELECT IAD.NumeroAlmacen, IAD.NumeroCompraProducto, IAD.CodigoProducto, IAD.CantidadCompra, @FechaHoraEntrega
	--FROM dbo.ComprasProductosDetalle IAD
	--WHERE IAD.NumeroAlmacen = @NumeroAlmacen
	--AND IAD.NumeroCompraProducto = @NumeroCompraProducto
	
	--INSERT INTO dbo.InventariosProductosCantidadesTransaccionesHistorial (NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, CodigoTipoTransaccion, FechaHoraCompra, CantidadExistente, PrecioUnitario)
	--SELECT IADE.NumeroAlmacen, IADE.NumeroCompraProducto, IADE.CodigoProducto, 'I', @FechaHoraEntrega, IADE.CantidadEntregada, IAD.PrecioUnitarioCompra
	--FROM ComprasProductosDetalleEntrega IADE
	--INNER JOIN ComprasProductosDetalle IAD
	--ON IAD.NumeroAlmacen = IADE.NumeroAlmacen
	--AND IAD.NumeroCompraProducto = IADE.NumeroCompraProducto
	--AND IAD.CodigoProducto = IADE.CodigoProducto
	--WHERE IADE.NumeroAlmacen = @NumeroAlmacen
	--AND IADE.NumeroCompraProducto= @NumeroCompraProducto
	--AND IADE.FechaHoraEntrega = @FechaHoraEntrega
	------------------------------------------------------------------------------------------------------------------------------------------------------------
	--FIN
	------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	
	
	
	--IF(NOT EXISTS(
	--	SELECT *
	--	FROM ComprasProductosDetalleEntrega IADE
	--	INNER JOIN InventariosProductos IA
	--	ON IADE.NumeroAlmacen = IA.NumeroAlmacen
	--	AND IADE.CodigoProducto = IA.CodigoProducto
	--	WHERE IADE.NumeroAlmacen = @NumeroAlmacen
	--	AND IADE.NumeroCompraProducto = @NumeroCompraProducto
	--	AND IADE.FechaHoraEntrega = @FechaHoraEntrega
	--	AND IADE.CantidadEntregada > IA.CantidadExistencia
	--))
	--BEGIN
		UPDATE InventariosProductos
			SET CantidadExistencia = CantidadExistencia + IADE.CantidadEntregada,
				PrecioUnitarioCompra = CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(IADE.CodigoProducto) 
										--WHEN 'O' THEN dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, IADE.CodigoProducto, null, null) / (CantidadExistencia + IADE.CantidadEntregada)
										WHEN 'O' THEN ISNULL((SELECT CAST( SUM(IACTH.PrecioUnitario * IACTH.CantidadExistente) / SUM(IACTH.CantidadExistente) AS DECIMAL(10,2))
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacen
														AND IACTH.CodigoTipoTransaccion = 'C'
														GROUP BY IACTH.NumeroAlmacen, IACTH.CodigoProducto
														),CPD.PrecioUnitarioCompra)
										WHEN 'P' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacen
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.FechaHoraCompra ASC, IACTH.NumeroTransaccionProducto DESC),CPD.PrecioUnitarioCompra)
										WHEN 'U' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacen
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.FechaHoraCompra DESC, IACTH.NumeroTransaccionProducto DESC),CPD.PrecioUnitarioCompra)
										WHEN 'A' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacen
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.PrecioUnitario DESC, IACTH.NumeroTransaccionProducto DESC),CPD.PrecioUnitarioCompra)
										WHEN 'B' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = IADE.NumeroAlmacen
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.PrecioUnitario ASC, IACTH.NumeroTransaccionProducto DESC),CPD.PrecioUnitarioCompra)
										END,
				PrecioValoradoTotal = ISNULL(PrecioValoradoTotal,0) + (IADE.CantidadEntregada * CPD.PrecioUnitarioCompra)
		FROM ComprasProductosDetalleEntrega IADE
		INNER JOIN ComprasProductosDetalle CPD
		ON IADE.NumeroAlmacen = CPD.NumeroAlmacen
		AND IADE.NumeroCompraProducto = CPD.NumeroCompraProducto	
		AND IADE.CodigoProducto = CPD.CodigoProducto	
		WHERE IADE.NumeroAlmacen = @NumeroAlmacen
		AND IADE.NumeroCompraProducto = @NumeroCompraProducto
		AND IADE.FechaHoraEntrega = @FechaHoraEntrega
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
		AND InventariosProductos.CodigoProducto = IADE.CodigoProducto
		AND CPD.NumeroAlmacen = @NumeroAlmacen
		AND CPD.NumeroCompraProducto = @NumeroCompraProducto
		
		UPDATE InventariosProductos
			SET PrecioUnitarioVentaPorMayor = InventariosProductos.PrecioUnitarioCompra + InventariosProductos.PrecioUnitarioCompra * InventariosProductos.PorcentajeGananciaVentaPorMayor /100,
				PrecioUnitarioVentaPorMenor = InventariosProductos.PrecioUnitarioCompra + InventariosProductos.PrecioUnitarioCompra * InventariosProductos.PorcentajeGananciaVentaPorMenor /100				
		FROM ComprasProductosDetalle CPD
		INNER JOIN Productos P
		ON P.CodigoProducto = CPD.CodigoProducto
		WHERE P.ActualizarPrecioVenta = 1
		AND CPD.NumeroCompraProducto = @NumeroCompraProducto
		AND CPD.NumeroAlmacen = @NumeroAlmacen
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
		AND InventariosProductos.CodigoProducto = CPD.CodigoProducto
				
		
		UPDATE ComprasProductosDetalle
			SET ComprasProductosDetalle.CantidadEntregada = ComprasProductosDetalle.CantidadEntregada + TA.CantidadEntregada
		FROM 
		(	SELECT CPDE.NumeroAlmacen, CPDE.NumeroCompraProducto, CPDE.CODIGOPRODUCTO, SUM(CPDE.CantidadEntregada) as CantidadEntregada
			FROM ComprasProductosDetalleEntrega CPDE
			WHERE CPDE.NumeroAlmacen = @NumeroAlmacen
			AND CPDE.NumeroCompraProducto = @NumeroCompraProducto
			AND CPDE.FechaHoraEntrega = @FechaHoraEntrega
			GROUP BY CPDE.NumeroAlmacen, CPDE.NumeroCompraProducto, CPDE.CODIGOPRODUCTO
		)TA
		WHERE ComprasProductosDetalle.CodigoProducto = TA.CodigoProducto
		AND ComprasProductosDetalle.NumeroAlmacen = TA.NumeroAlmacen
		AND ComprasProductosDetalle.NumeroCompraProducto = TA.NumeroCompraProducto		
		
		UPDATE ComprasProductos
			SET FechaHoraRecepcion = @FechaHoraEntrega
		WHERE NumeroAlmacen = @NumeroAlmacen
		AND NumeroCompraProducto = @NumeroCompraProducto
		
		IF(EXISTS(
			SELECT *
			FROM InventariosProductos IA
			INNER JOIN ComprasProductosDetalleEntrega IADE
			ON IA.NumeroAlmacen = IADE.NumeroAlmacen
			AND IA.CodigoProducto = IADE.CodigoProducto
			WHERE IADE.NumeroAlmacen = @NumeroAlmacen
			AND IADE.NumeroCompraProducto = @NumeroCompraProducto
			AND IADE.FechaHoraEntrega = @FechaHoraEntrega
			AND IA.CantidadRequerida <> 0	
		))
		BEGIN
			UPDATE InventariosProductos
				SET CantidadRequerida = CantidadRequerida - 
				(CASE WHEN IADE.CantidadEntregada > InventariosProductos.CantidadRequerida THEN InventariosProductos.CantidadRequerida
				ELSE IADE.CantidadEntregada END)
			FROM ComprasProductosDetalleEntrega IADE
			WHERE IADE.NumeroAlmacen = @NumeroAlmacen
			AND IADE.NumeroCompraProducto = @NumeroCompraProducto
			AND IADE.FechaHoraEntrega = @FechaHoraEntrega
			AND InventariosProductos.NumeroAlmacen = @NumeroAlmacen
			AND InventariosProductos.CodigoProducto = IADE.CodigoProducto 
			AND InventariosProductos.CantidadRequerida <> 0	
		END
		
		
		
		
	--END
	--ELSE
	--BEGIN
	--	RAISERROR('No se Pudo Actualizar el Compra de Productos, debido a la Insuficiente existencia de Productos',1,16)	
	--END
	
		IF(@@ERROR <> 0)
	BEGIN
		RAISERROR('No se Pudo Actualizar el Compra de Productos',1,16)	
		ROLLBACK TRAN
	END
	ELSE
		COMMIT TRANSACTION

END
GO


DROP PROCEDURE ListarCompraProductoReporte
GO

CREATE PROCEDURE ListarCompraProductoReporte
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT

AS 
BEGIN
	SELECT IA.NumeroAlmacen, IA.NumeroCompraProducto AS NumeroIngresoArticulo, P.NombreRazonSocial, P.NombreRepresentante,
	IA.DIUsuario, DBO.ObtenerNombreCompleto(IA.DIUsuario) AS NombreUsuario, IA.FechaHoraRecepcion, FechaHoraRegistro, 
	CASE CodigoTipoCompra WHEN 'E' THEN 'EFECTIVO' ELSE 'A CREDITO' END AS TipoIngreso, 
	CASE CodigoEstadoCompra WHEN 'I' THEN 'INICIADO' WHEN 'A' THEN 'ANULADO' 
	WHEN 'F' THEN 'FINALIZADO' WHEN 'X' THEN 'FINALIZADO INCOMPLETO' WHEN 'D' THEN 'PENDIENTE' END AS EstadoIngreso,
	NumeroFactura, NumeroComprobante, 
	MontoTotalCompra AS MontoTotalIngreso, NumeroCuentaPorPagar, IA.Observaciones,	
	MontoTotalPagoEfectivo + dbo.ObtenerMontoTotalCuentasCobrosPagos(IA.NumeroAlmacen, IA.NumeroCuentaPorPagar, 'P') as MontoTotalPagoEfectivo, 			
	IA.MontoTotalCompra - MontoTotalPagoEfectivo - dbo.ObtenerMontoTotalCuentasCobrosPagos(IA.NumeroAlmacen, IA.NumeroCuentaPorPagar, 'P') AS MontoSaldo,		
	'BOLIVIANOS' AS NombreMoneda, 'Bs' AS MascaraMoneda,
	dbo.ObtenerMontoNumerico_EnTexto(MontoTotalCompra,'BOLIVIANOS') AS MontoTotalTexto,
	P.NITProveedor
	FROM dbo.ComprasProductos IA
	INNER JOIN Proveedores P
	ON IA.CodigoProveedor = P.CodigoProveedor
	INNER JOIN Usuarios U
	ON IA.DIUsuario = U.DIUsuario
	WHERE (IA.NumeroCompraProducto = @NumeroCompraProducto)
	AND IA.NumeroAlmacen = @NumeroAlmacen
		
END
GO


DROP PROCEDURE ActualizarCodigoEstadoCompra
GO

CREATE PROCEDURE ActualizarCodigoEstadoCompra
	@NumeroAlmacen			INT,
	@NumeroCompraProducto	INT,
	@CodigoEstadoCompra		CHAR(1)
AS
BEGIN
	UPDATE ComprasProductos
		SET CodigoEstadoCompra = @CodigoEstadoCompra
	WHERE (NumeroCompraProducto = @NumeroCompraProducto)
	AND NumeroAlmacen = @NumeroAlmacen
END
GO


SELECT * FROM VENTASPRODUCTOS