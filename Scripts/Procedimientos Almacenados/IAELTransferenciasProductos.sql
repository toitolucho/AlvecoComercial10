USE AlvecoComercial10
GO



DROP PROCEDURE InsertarTransferenciaProducto
GO
CREATE PROCEDURE InsertarTransferenciaProducto
	@NumeroAlmacenEmisor			INT,	
	@NumeroAlmacenRecepctor			INT,
	@DIUsuario						CHAR(15),
	@FechaHoraTransferencia			DATETIME,
	@CodigoEstadoTransferencia		CHAR(1),
	@MontoTotalTransferencia		DECIMAL(10,2),
	@Observaciones					TEXT
AS
BEGIN
	SET @FechaHoraTransferencia = ISNULL(@FechaHoraTransferencia, GETDATE())
	INSERT INTO dbo.TransferenciasProductos (NumeroAlmacenEmisor, NumeroAlmacenRecepctor, DIUsuario, FechaHoraTransferencia, CodigoEstadoTransferencia, MontoTotalTransferencia, Observaciones)
	VALUES (@NumeroAlmacenEmisor, @NumeroAlmacenRecepctor, @DIUsuario, @FechaHoraTransferencia, @CodigoEstadoTransferencia, @MontoTotalTransferencia, @Observaciones)
END
GO



DROP PROCEDURE ActualizarTransferenciaProducto
GO
CREATE PROCEDURE ActualizarTransferenciaProducto
	@NumeroAlmacenEmisor			INT,	
	@NumeroTransferenciaProducto	INT,
	@NumeroAlmacenRecepctor			INT,
	@DIUsuario						CHAR(15),
	@FechaHoraTransferencia			DATETIME,
	@CodigoEstadoTransferencia		CHAR(1),
	@MontoTotalTransferencia		DECIMAL(10,2),
	@Observaciones					TEXT
AS
BEGIN
	UPDATE 	dbo.TransferenciasProductos
	SET	
		
		NumeroAlmacenRecepctor			= @NumeroAlmacenRecepctor,
		DIUsuario					= @DIUsuario,
		--FechaHoraTransferencia			= @FechaHoraTransferencia,
		CodigoEstadoTransferencia		= @CodigoEstadoTransferencia,
		MontoTotalTransferencia			= @MontoTotalTransferencia,
		Observaciones					= @Observaciones
	WHERE	(NumeroAlmacenEmisor = @NumeroAlmacenEmisor)
		AND (NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
END
GO



DROP PROCEDURE EliminarTransferenciaProducto
GO
CREATE PROCEDURE EliminarTransferenciaProducto
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS
BEGIN
	BEGIN TRANSACTION
	
	DELETE 
	FROM dbo.TransferenciasProductosDetalle
	WHERE	(NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
		AND (NumeroAlmacenEmisor = @NumeroAlmacenEmisor)
	
	DELETE 
	FROM dbo.TransferenciasProductos
	WHERE	(NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
		AND (NumeroAlmacenEmisor = @NumeroAlmacenEmisor)
	IF(@@ERROR <> 0)
	BEGIN
		ROLLBACK TRANSACTION
		RAISERROR('NO SE PUDO COMPLETAR LA ELIMINACIÓN DE LA TRANSACCIÓN ACTUAL',17,2)
	END
	ELSE
		COMMIT TRANSACTION
END
GO


DROP PROCEDURE ListarTransferenciasProductos
GO
CREATE PROCEDURE ListarTransferenciasProductos
	@NumeroAlmacenEmisor	INT
AS
BEGIN
	SELECT NumeroAlmacenEmisor, NumeroTransferenciaProducto, NumeroAlmacenRecepctor, DIUsuario, FechaHoraTransferencia, CodigoEstadoTransferencia, MontoTotalTransferencia, Observaciones
	FROM dbo.TransferenciasProductos
	WHERE (NumeroAlmacenEmisor= @NumeroAlmacenEmisor)
	ORDER BY NumeroTransferenciaProducto
END
GO



DROP PROCEDURE ObtenerTransferenciaProducto
GO
CREATE PROCEDURE ObtenerTransferenciaProducto
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS
BEGIN
	SELECT NumeroAlmacenEmisor, NumeroTransferenciaProducto, NumeroAlmacenRecepctor, DIUsuario, FechaHoraTransferencia, CodigoEstadoTransferencia, MontoTotalTransferencia, Observaciones
	FROM dbo.TransferenciasProductos
	WHERE	(NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
		AND (NumeroAlmacenEmisor = @NumeroAlmacenEmisor)
END
GO


--IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = object_id(N'ActualizarCodigoEstadoTransferencia') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
--	BEGIN
--		DROP PROCEDURE ActualizarCodigoEstadoTransferencia
--	END
--GO	
--CREATE PROCEDURE ActualizarCodigoEstadoTransferencia
--	@NumeroAlmacenEmisor			INT,
--	@NumeroTransferenciaProducto	INT,
--	@CodigoEstadoTransferencia		CHAR(15),	
--	@MontoTotalTransferencia		DECIMAL(10,2),
--	@CodigoTipoEnvioRecepcion		CHAR(1)
--AS
--BEGIN
--	IF(@CodigoTipoEnvioRecepcion = 'R')
--		SET	@NumeroAlmacenEmisor = dbo.ObtenerNumeroAlmacenEmisorDeTransferencias(@NumeroTransferenciaProducto, @NumeroAlmacenEmisor)
--	IF(@MontoTotalTransferencia IS NOT NULL)
--		UPDATE 	dbo.TransferenciasProductos
--		SET		
--			CodigoEstadoTransferencia		= @CodigoEstadoTransferencia,
--			MontoTotalTransferencia			= @MontoTotalTransferencia,
--			FechaHoraTransferencia			= GETDATE()				
--		WHERE	(NumeroAlmacenEmisor = @NumeroAlmacenEmisor)
--			AND (NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
--	ELSE
--		UPDATE 	dbo.TransferenciasProductos
--		SET		
--			CodigoEstadoTransferencia		= @CodigoEstadoTransferencia			
--		WHERE	(NumeroAlmacenEmisor = @NumeroAlmacenEmisor)
--			AND (NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
--END
--GO



DROP PROCEDURE InsertarTransferenciaProductoXMLDetalle
GO
CREATE PROCEDURE InsertarTransferenciaProductoXMLDetalle
	@NumeroAlmacenEmisor			INT,	
	@NumeroAlmacenRecepctor			INT,
	@DIUsuario						CHAR(15),
	@FechaHoraTransferencia			DATETIME,
	@CodigoEstadoTransferencia		CHAR(1),
	@MontoTotalTransferencia		DECIMAL(10,2),
	@Observaciones					TEXT,
	@ProductosDetalle				TEXT
AS
BEGIN
	BEGIN TRANSACTION
		
		SET @FechaHoraTransferencia = ISNULL(@FechaHoraTransferencia, GETDATE())
		
		INSERT INTO dbo.TransferenciasProductos (NumeroAlmacenEmisor, NumeroAlmacenRecepctor, DIUsuario, FechaHoraTransferencia, CodigoEstadoTransferencia, MontoTotalTransferencia, Observaciones)
		VALUES (@NumeroAlmacenEmisor, @NumeroAlmacenRecepctor, @DIUsuario, @FechaHoraTransferencia, @CodigoEstadoTransferencia, @MontoTotalTransferencia, @Observaciones)
		
		DECLARE @punteroXMLProductosDetalle		INT,
				@NumeroTransferenciaProducto	INT
		
		SET @NumeroTransferenciaProducto = @@IDENTITY
					
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalle
		
		INSERT INTO dbo.TransferenciasProductosDetalle(NumeroAlmacenEmisor, NumeroTransferenciaProducto, CodigoProducto, CantidadTransferencia, PrecioUnitarioTransferencia)
		SELECT    @NumeroAlmacenEmisor, @NumeroTransferenciaProducto, CodigoProducto, Cantidad, PrecioUnitario
		FROM       OPENXML (@punteroXMLProductosDetalle, '/TransferenciasProductos/TransferenciasProductosDetalle',2)
					WITH (CodigoProducto		VARCHAR(15),
						  Cantidad				INT,
						  PrecioUnitario		DECIMAL(10,2)						  
					)
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo ingresar la Transferencia de Productos',17,2)	
			ROLLBACK TRAN
		END
	COMMIT TRANSACTION	
END
GO

/*<TransferenciasProductos>
  <TransferenciasProductosDetalle>
    <CodigoProducto>0000000        </CodigoProducto>
    <Cantidad>1</Cantidad>
    <PrecioUnitario>22.00</PrecioUnitario>
    <Existencia>90</Existencia>
    <TiempoGarantia>0</TiempoGarantia>
    <MarcaProducto>LA ESTRELLA</MarcaProducto>
    <PrecioTotal>22.00</PrecioTotal>
    <CodigoProveedor>2</CodigoProveedor>
  </TransferenciasProductosDetalle>
  <TransferenciasProductosDetalle>
    <CodigoProducto>0000001        </CodigoProducto>
    <Cantidad>1</Cantidad>
    <PrecioUnitario>20.00</PrecioUnitario>
    <Existencia>100</Existencia>
    <TiempoGarantia>0</TiempoGarantia>
    <MarcaProducto>KRIS</MarcaProducto>
    <PrecioTotal>20.00</PrecioTotal>
    <CodigoProveedor>2</CodigoProveedor>
  </TransferenciasProductosDetalle>
</TransferenciasProductos>*/

DROP PROCEDURE ActualizarTransferenciaProductoXMLDetalle
GO
CREATE PROCEDURE ActualizarTransferenciaProductoXMLDetalle
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT,	
	@NumeroAlmacenRecepctor			INT,
	@DIUsuario						CHAR(15),
	@FechaHoraTransferencia			DATETIME,
	@CodigoEstadoTransferencia		CHAR(1),
	@MontoTotalTransferencia		DECIMAL(10,2),
	@Observaciones					TEXT,
	@ProductosDetalleXML			TEXT
AS
BEGIN
	BEGIN TRANSACTION 
	
		EXEC ActualizarTransferenciaProducto @NumeroAlmacenEmisor, @NumeroTransferenciaProducto, @NumeroAlmacenRecepctor, @DIUsuario, @FechaHoraTransferencia, @CodigoEstadoTransferencia, @MontoTotalTransferencia, @Observaciones
		DECLARE @punteroXMLProductosDetalle INT
				
		EXEC sp_xml_preparedocument @punteroXMLProductosDetalle OUTPUT, @ProductosDetalleXML
		--PARA INSERTAR LOS NUEVOS REGISTROS EN LA EDICIÓN 
		------------------------------------------------------------------------------------
		
		INSERT INTO dbo.TransferenciasProductosDetalle(
				NumeroAlmacenEmisor, NumeroTransferenciaProducto, CodigoProducto, CantidadTransferencia, PrecioUnitarioTransferencia
				)
		SELECT  @NumeroAlmacenEmisor, 
				@NumeroTransferenciaProducto, 
				CodigoProducto,				
				Cantidad,
				PrecioUnitario
		FROM OPENXML (@punteroXMLProductosDetalle, '/TransferenciasProductos/TransferenciasProductosDetalle',2)		
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
			FROM dbo.TransferenciasProductosDetalle IAD
			WHERE IAD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
			AND IAD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		)
		
		--ACTUALIZAR LOS REGISTROS
		------------------------------------------------------------------------------------
		UPDATE dbo.TransferenciasProductosDetalle
			SET dbo.TransferenciasProductosDetalle.PrecioUnitarioTransferencia = VSDXML.PrecioUnitario,
				dbo.TransferenciasProductosDetalle.CantidadTransferencia = VSDXML.Cantidad
		FROM OPENXML (@punteroXMLProductosDetalle, '/TransferenciasProductos/TransferenciasProductosDetalle',2) 
		WITH(	CodigoProducto			CHAR(10),
				Cantidad				INT,
				PrecioUnitario			DECIMAL(10,2)			
				) VSDXML
		WHERE dbo.TransferenciasProductosDetalle.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND dbo.TransferenciasProductosDetalle.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		AND dbo.TransferenciasProductosDetalle.CodigoProducto = VSDXML.CodigoProducto
		
		--QUITAR LOS REGISTROS QUE FUERON ELIMINADOS
		--------------------------------------------------------------------------
		DELETE
		FROM TransferenciasProductosDetalle
		WHERE CodigoProducto NOT IN
		(
			SELECT  CodigoProducto				
			FROM OPENXML (@punteroXMLProductosDetalle, '/TransferenciasProductos/TransferenciasProductosDetalle',2)		
			WITH(
					CodigoProducto			CHAR(15)
				)
		)
		AND TransferenciasProductosDetalle.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND TransferenciasProductosDetalle.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		
		EXEC sp_xml_removedocument @punteroXMLProductosDetalle
		
		DECLARE @MontoTotalVenta2 DECIMAL(10,2)
		SELECT @MontoTotalVenta2 =  SUM(VPD.CantidadTransferencia * VPD.PrecioUnitarioTransferencia)
		FROM dbo.TransferenciasProductosDetalle VPD
		WHERE VPD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND VPD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		
		UPDATE dbo.TransferenciasProductos
			SET MontoTotalTransferencia = @MontoTotalVenta2						
		WHERE NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		
		IF(@@ERROR <> 0)
		BEGIN
			RAISERROR('No se Pudo Actualizar la Transferencia de Productos',17,2)	
			ROLLBACK TRAN
		END
		ELSE
			COMMIT TRANSACTION
END
GO

DROP PROCEDURE ActualizarInventarioTransferenciasProductos
GO

CREATE PROCEDURE ActualizarInventarioTransferenciasProductos
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS
BEGIN
	BEGIN TRANSACTION 
	
	DECLARE @FechaHoraTransferencia DATETIME,
			@NumeroAlmacenRecepctor INT 
		
	SELECT @FechaHoraTransferencia = FechaHoraTransferencia , @NumeroAlmacenRecepctor = NumeroAlmacenRecepctor
	FROM dbo.TransferenciasProductos
	WHERE NumeroAlmacenEmisor = @NumeroAlmacenEmisor
	AND NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		
	
	IF(NOT EXISTS(
		SELECT SADE.CodigoProducto
		FROM dbo.TransferenciasProductosDetalle SADE
		INNER JOIN InventariosProductos IA
		ON SADE.NumeroAlmacenEmisor = IA.NumeroAlmacen
		AND SADE.CodigoProducto = IA.CodigoProducto
		WHERE SADE.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND SADE.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		AND SADE.CantidadTransferencia > IA.CantidadExistencia
	))
	BEGIN
		
		--CODIGO QUE DEBE DISMINUIR LA EXISTENCIA DEL ALMACEN EMISOR		
		UPDATE InventariosProductos
			SET CantidadExistencia = CantidadExistencia - TPD.CantidadTransferencia,
				PrecioValoradoTotal = PrecioValoradoTotal - (TPD.CantidadTransferencia * TPD.PrecioUnitarioTransferencia)
		FROM dbo.TransferenciasProductosDetalle TPD
		WHERE TPD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND TPD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		AND InventariosProductos.CodigoProducto = TPD.CodigoProducto
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenEmisor
		
		
	    --SE DEBE DISMINUIR E INCLUSO ELIMINAR EL HISTORIAL DE INVENTARIOS
		--DE ACUERDO AL TIPO DE CALCULO CORRESPONDIENTES, 'UEPS','PEUS', ETC				
		DECLARE @TTransferenciasProductosAux	TABLE
		(
			NumeroAlmacen		INT,
			CodigoProducto		CHAR(15),
			CantidadVenta		INT,
			FechaHoraEntrega	DATETIME
		)	
		
		INSERT INTO @TTransferenciasProductosAux (NumeroAlmacen, CodigoProducto, CantidadVenta, FechaHoraEntrega)
		SELECT NumeroAlmacenEmisor, CodigoProducto, CantidadTransferencia, @FechaHoraTransferencia
		FROM dbo.TransferenciasProductosDetalle  SADE
		WHERE SADE.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND SADE.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
				
		
		DECLARE @CodigoProducto				CHAR(15),
				@NumeroAlmacenEmisorCP			INT,
				@CantidadVenta				INT
				
		
		SET ROWCOUNT 1
		SELECT @CodigoProducto = CodigoProducto, @CantidadVenta = CantidadVenta, @NumeroAlmacenEmisorCP = NumeroAlmacen 
		FROM @TTransferenciasProductosAux				
		WHILE @@rowcount <> 0
		BEGIN
			
			
			EXEC ActualizarEliminarInventarioProductosCantidadesTransaccionesHistorial @NumeroAlmacenEmisorCP, @CodigoProducto, @CantidadVenta, @NumeroTransferenciaProducto, @FechaHoraTransferencia
			
			DELETE @TTransferenciasProductosAux WHERE CodigoProducto = @CodigoProducto
			SET ROWCOUNT 1
			SELECT @CodigoProducto = CodigoProducto, @CantidadVenta = CantidadVenta, @NumeroAlmacenEmisorCP = NumeroAlmacen 
			FROM @TTransferenciasProductosAux	
		END
		SET ROWCOUNT 0	
		
		IF(EXISTS(
			SELECT SAD.CodigoProducto	
			FROM dbo.TransferenciasProductosDetalle SAD
			INNER JOIN InventariosProductos IA
			ON SAD.CodigoProducto = IA.CodigoProducto
			AND SAD.NumeroAlmacenEmisor = IA.NumeroAlmacen
			WHERE (IA.CantidadExistencia)< IA.StockMinimo
			AND SAD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
			AND SAD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		))
		BEGIN
			UPDATE InventariosProductos
				SET CantidadRequerida = CantidadRequerida + (StockMinimo - CantidadExistencia)
			FROM dbo.TransferenciasProductosDetalle SADE
			WHERE SADE.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
			AND SADE.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
			AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenEmisor
			AND InventariosProductos.CodigoProducto = SADE.CodigoProducto
			AND InventariosProductos.CantidadExistencia < InventariosProductos.StockMinimo
		END
		
		
		
		
		
		--PARA EL ALMACEN RECEPTOR, SE ACTUALIZA SU INVENTARIO AUMENTANDO SUS CANTIDADES Y VALORADO
		INSERT INTO dbo.InventariosProductosCantidadesTransaccionesHistorial
			(NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, CodigoTipoTransaccion, FechaHoraCompra, CantidadExistente, PrecioUnitario)
		SELECT @NumeroAlmacenRecepctor, NumeroTransferenciaProducto, CodigoProducto, 'C', @FechaHoraTransferencia, CantidadTransferencia, PrecioUnitarioTransferencia
		FROM dbo.TransferenciasProductosDetalle TPD
		WHERE TPD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND TPD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		
		UPDATE InventariosProductos
			SET CantidadExistencia = CantidadExistencia + IADE.CantidadTransferencia,
				PrecioUnitarioCompra = CASE dbo.ObtenerCodigoTipoCalculoInventarioProducto(IADE.CodigoProducto) 
										--WHEN 'O' THEN dbo.ObtenerMontoTotalValorado(@NumeroAlmacen, IADE.CodigoProducto, null, null) / (CantidadExistencia + IADE.CantidadEntregada)
										WHEN 'O' THEN ISNULL((SELECT CAST( SUM(IACTH.PrecioUnitario * IACTH.CantidadExistente) / SUM(IACTH.CantidadExistente) AS DECIMAL(10,2))
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = @NumeroAlmacenRecepctor
														AND IACTH.CodigoTipoTransaccion = 'C'
														GROUP BY IACTH.NumeroAlmacen, IACTH.CodigoProducto
														),IADE.PrecioUnitarioTransferencia)
										WHEN 'P' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = @NumeroAlmacenRecepctor
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.FechaHoraCompra ASC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioTransferencia)
										WHEN 'U' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = @NumeroAlmacenRecepctor
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.FechaHoraCompra DESC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioTransferencia)
										WHEN 'A' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = @NumeroAlmacenRecepctor
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.PrecioUnitario DESC, IACTH.NumeroTransaccionProducto DESC), IADE.PrecioUnitarioTransferencia)
										WHEN 'B' THEN ISNULL((SELECT TOP(1) IACTH.PrecioUnitario 
														FROM InventariosProductosCantidadesTransaccionesHistorial IACTH
														WHERE IACTH.CodigoProducto = IADE.CodigoProducto
														AND IACTH.NumeroAlmacen = @NumeroAlmacenRecepctor
														AND IACTH.CodigoTipoTransaccion = 'C'
														ORDER BY IACTH.PrecioUnitario ASC, IACTH.NumeroTransaccionProducto DESC),IADE.PrecioUnitarioTransferencia)
										END,
				PrecioValoradoTotal = ISNULL(PrecioValoradoTotal,0) + (IADE.CantidadTransferencia * IADE.PrecioUnitarioTransferencia)
		FROM dbo.TransferenciasProductosDetalle IADE
		WHERE IADE.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND IADE.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenRecepctor
		AND InventariosProductos.CodigoProducto = IADE.CodigoProducto
		
		
		UPDATE InventariosProductos
			SET PrecioUnitarioVentaPorMayor = InventariosProductos.PrecioUnitarioCompra + InventariosProductos.PrecioUnitarioCompra * InventariosProductos.PorcentajeGananciaVentaPorMayor /100,
				PrecioUnitarioVentaPorMenor = InventariosProductos.PrecioUnitarioCompra + InventariosProductos.PrecioUnitarioCompra * InventariosProductos.PorcentajeGananciaVentaPorMenor /100				
		FROM dbo.TransferenciasProductosDetalle CPD
		INNER JOIN Productos P
		ON P.CodigoProducto = CPD.CodigoProducto
		WHERE P.ActualizarPrecioVenta = 1
		AND CPD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
		AND CPD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
		AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenRecepctor
		AND InventariosProductos.CodigoProducto = CPD.CodigoProducto
		
		
		IF(EXISTS(
			SELECT *
			FROM InventariosProductos IA
			INNER JOIN dbo.TransferenciasProductosDetalle IADE			
			ON IA.CodigoProducto = IADE.CodigoProducto
			WHERE IADE.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
			AND IADE.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
			AND IA.CantidadRequerida <> 0	
			AND IA.NumeroAlmacen = @NumeroAlmacenRecepctor
		))
		BEGIN
			UPDATE InventariosProductos
				SET CantidadRequerida = CantidadRequerida - 
				(CASE WHEN IADE.CantidadTransferencia > InventariosProductos.CantidadRequerida THEN InventariosProductos.CantidadRequerida
				ELSE IADE.CantidadTransferencia END)
			FROM dbo.TransferenciasProductosDetalle IADE
			WHERE IADE.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
			AND IADE.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
			AND InventariosProductos.NumeroAlmacen = @NumeroAlmacenRecepctor
			AND InventariosProductos.CodigoProducto = IADE.CodigoProducto 
			AND InventariosProductos.CantidadRequerida <> 0	
		END
		
		
		
		
	END
	ELSE
	BEGIN
		RAISERROR('No se Pudo Actualizar la Transferencia de Productos, debido a la Insuficiente existencia de Productos',1,16)	
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


DROP PROCEDURE ListarProductosTransferidosSuperaStockMinimo
GO
	
CREATE PROCEDURE ListarProductosTransferidosSuperaStockMinimo	
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS 
BEGIN
	SELECT ia.CodigoProducto, dbo.ObtenerNombreProducto(IA.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia - SAD.CantidadTransferencia as CantidadNuevaExistencia, IA.StockMinimo	
	FROM TransferenciasProductosDetalle SAD
	INNER JOIN InventariosProductos IA
	ON SAD.CodigoProducto = IA.CodigoProducto
	AND SAD.NumeroTransferenciaProducto = IA.NumeroAlmacen
	WHERE (IA.CantidadExistencia - SAD.CantidadTransferencia) <= IA.StockMinimo
	AND SAD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
	AND SAD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
END
GO

DROP PROCEDURE ListarProductosTransferenciaInsuficiente
GO
	
CREATE PROCEDURE ListarProductosTransferenciaInsuficiente	
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS 
BEGIN
	SELECT ia.CodigoProducto, dbo.ObtenerNombreProducto(IA.CodigoProducto) AS NombreProducto,
		IA.CantidadExistencia
	FROM dbo.TransferenciasProductosDetalle SAD
	INNER JOIN InventariosProductos IA
	ON SAD.CodigoProducto = IA.CodigoProducto
	AND SAD.NumeroAlmacenEmisor = IA.NumeroAlmacen
	WHERE SAD.CantidadTransferencia > IA.CantidadExistencia
	AND SAD.NumeroAlmacenEmisor = @NumeroAlmacenEmisor
	AND SAD.NumeroTransferenciaProducto = @NumeroTransferenciaProducto
END
GO


IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = object_id(N'ListarTransferenciaProductosReporte') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP PROCEDURE ListarTransferenciaProductosReporte
	END
GO	
CREATE PROCEDURE ListarTransferenciaProductosReporte
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS
BEGIN
	SELECT	NumeroAlmacenEmisor, NumeroTransferenciaProducto, NumeroAlmacenRecepctor, 
			DIUsuario, DBO.ObtenerNombreCompleto(TP.DIUsuario) AS NombreUsuario,  
			FechaHoraTransferencia, CodigoEstadoTransferencia,
			CASE CodigoEstadoTransferencia WHEN 'I' THEN 'INICIADA' WHEN 'A' THEN 'ANULADA' WHEN 'F' THEN 'FINALIZADA' ELSE 'INDETERMINADO' END AS EstadoTransferencia,  
			MontoTotalTransferencia, Observaciones, A1.NombreAlmacen AS NombreAlmacenEmisor,
			A2.NombreAlmacen AS NombreAlmacenReceptor
	FROM dbo.TransferenciasProductos TP
	INNER JOIN Almacenes A1
	ON TP.NumeroAlmacenEmisor = A1.NumeroAlmacen
	INNER JOIN Almacenes A2
	ON TP.NumeroAlmacenRecepctor = A2.NumeroAlmacen
	WHERE NumeroAlmacenEmisor = @NumeroAlmacenEmisor
	AND NumeroTransferenciaProducto = @NumeroTransferenciaProducto	
END
GO


IF EXISTS( SELECT * FROM dbo.sysobjects WHERE id = object_id(N'ListarTransferenciaProductosDetalleReporte') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP PROCEDURE ListarTransferenciaProductosDetalleReporte
	END
GO	
CREATE PROCEDURE ListarTransferenciaProductosDetalleReporte
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT
AS
BEGIN		

	SELECT	CodigoProducto, DBO.ObtenerNombreProducto(CodigoProducto) AS NombreProducto, 
			CantidadTransferencia, PrecioUnitarioTransferencia,
			CantidadTransferencia * PrecioUnitarioTransferencia AS PrecioTotal
	FROM dbo.TransferenciasProductosDetalle
	WHERE NumeroAlmacenEmisor = @NumeroAlmacenEmisor
	AND NumeroTransferenciaProducto = @NumeroTransferenciaProducto	
END
GO
--exec ListasVentaProductoReporte 1, 9

DROP PROCEDURE ActualizarCodigoEstadoTransferencia
GO

CREATE PROCEDURE ActualizarCodigoEstadoTransferencia
	@NumeroAlmacenEmisor			INT,
	@NumeroTransferenciaProducto	INT,
	@CodigoEstadoTransferencia		CHAR(1)
AS
BEGIN
	UPDATE dbo.TransferenciasProductos
		SET CodigoEstadoTransferencia = @CodigoEstadoTransferencia
	WHERE (NumeroTransferenciaProducto = @NumeroTransferenciaProducto)
	AND NumeroAlmacenEmisor = @NumeroAlmacenEmisor
END
GO

