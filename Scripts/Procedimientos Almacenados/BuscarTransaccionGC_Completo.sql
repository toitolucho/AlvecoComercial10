USE AlvecoComercial10

DROP PROCEDURE BuscarTransaccionGC
GO

CREATE PROCEDURE BuscarTransaccionGC
@CodigoAmbitoBusqueda		CHAR(1),
@TextoABuscar				VARCHAR(160),
@NumeroAlmacen				INT,
@NumeroTransaccion			INT,
@TipoTransaccion			CHAR(1),
@FechaInicio				DATETIME,
@FechaFin					DATETIME,
@ExactamenteIgual			BIT,
@CodigoEstadoTranssacion	CHAR(1)

AS
DECLARE @S NVARCHAR(2000)
DECLARE @F VARCHAR(8000)
DECLARE @Condicion NVARCHAR(2000)
DECLARE @W NVARCHAR(4000)
DECLARE @AUX NVARCHAR(2000)
DECLARE @ScriptSQL VARCHAR(8000)
DECLARE @PosicionInicial TINYINT
DECLARE @PosicionActual TINYINT
DECLARE @PosicionFinal TINYINT
DECLARE @OperadorComparacion VARCHAR(4)
DECLARE @TextoABuscarOptimizado VARCHAR(170)
DECLARE @NombreCampo VARCHAR(250)

SET @W = ''
IF (@TipoTransaccion = 'S') --SI ES PARA SALIDAS POR VENTAS
BEGIN
	SET @S = '	SELECT	DISTINCT SA.NumeroAlmacen, SA.NumeroVentaProducto AS NumeroTransaccion, 
				SA.NumeroComprobante AS NumeroComprobante, 
				SA.FechaHoraVenta AS FechaHoraRegistro, 
				C.NombreCliente AS NombrePersonaTransaccion,
				CASE(SA.CodigoTipoVenta) WHEN ''E'' THEN ''EFECTIVO'' ELSE ''CREDITO'' END AS TipoTransaccion,	
				CASE SA.CodigoEstadoVenta WHEN ''I'' THEN ''INICIADA'' WHEN ''F'' THEN ''FINALIZADA Y DISTRIBUIDA''
				WHEN ''A'' THEN ''ANULADA'' WHEN ''X'' THEN ''ENTREGA INCOMPLETA'' ELSE ''INDETERMINADO'' END AS EstadoTransaccion,					
				CAST(SA.Observaciones AS VARCHAR(4000)) AS Observaciones,
				SA.CodigoEstadoVenta AS CodigoEstadoTransaccion,
				SA.CodigoTipoVenta AS CodigoTipoTransaccion,				
				SA.MontoTotalVenta AS MontoTotalTransaccion,
				CASE WHEN SA.CodigoTipoVenta =''E'' THEN DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto, ''V'') 
				ELSE DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto, ''V'') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroCuentaPorCobrar, ''C'')  END AS MontoCancelado,
				CASE WHEN CodigoTipoVenta =''E'' THEN SA.MontoTotalVenta - DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto, ''V'') 
				ELSE SA.MontoTotalVenta -(DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto,''V'') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroCuentaPorCobrar, ''C''))  END AS MontoSaldo, 
				SA.NumeroCuentaPorCobrar AS NumeroCuentaCreditoTransaccion,				
				SA.FechaHoraEntrega AS FechaHoraCulminacion,
				DBO.ObtenerNombreCompleto(SA.DIUsuario) AS UsuarioResponsable '
	SET @F = '	FROM VentasProductos SA
				INNER JOIN VentasProductosDetalle SAD
				ON SA.NumeroAlmacen = SAD.NumeroAlmacen
				AND SA.NumeroVentaProducto = SAD.NumeroVentaProducto
				INNER JOIN Productos P
				ON SAD.CodigoProducto = P.CodigoProducto
				INNER JOIN Clientes C
				ON SA.CodigoCliente = C.CodigoCliente '
	SET @W = 'WHERE SA.NumeroAlmacen = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND SA.NumeroVentaProducto LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(SA.FechaHoraVenta As Float)) As DateTime) BETWEEN  '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' SA.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN		
		SET @W = @W + ' AND ( SA.CodigoEstadoVenta = ''' + @CodigoEstadoTranssacion + ''')'
	END
END
ELSE IF (@TipoTransaccion = 'I') -- SI ES PARA INGRESOS POR COMPRAS
BEGIN
	SET @S = '	SELECT DISTINCT CP.NumeroAlmacen, CP.NumeroCompraProducto AS NumeroTransaccion, 
				CP.NumeroComprobante AS NumeroComprobante, 
				CP.FechaHoraRegistro AS FechaHoraRegistro, 
				PR.NombreRazonSocial AS NombrePersonaTransaccion,
				CASE(CodigoTipoCompra) WHEN ''E'' THEN ''EFECTIVO'' ELSE ''CREDITO'' END AS TipoTransaccion, 
				CASE
				WHEN CodigoEstadoCompra = ''I'' THEN ''INICIADA'' 
				WHEN CodigoEstadoCompra = ''A'' THEN ''ANULADA'' 
				WHEN CodigoTipoCompra = ''E'' AND CodigoEstadoCompra = ''P'' THEN ''PAGADA EN TRANSITO'' 
				WHEN CodigoTipoCompra = ''R'' AND CodigoEstadoCompra = ''P'' THEN ''A CREDITO EN TRANSITO'' 
				WHEN CodigoEstadoCompra = ''D'' THEN ''PENDIENTE''  
				WHEN CodigoEstadoCompra = ''F'' THEN ''FINALIZADO Y RECIBIDO''  
				WHEN CodigoEstadoCompra = ''X'' THEN ''FINALIZADO INCOMPLETO'' END AS EstadoTransaccion, 
				CAST(CP.Observaciones AS NVARCHAR(4000)) AS Observaciones, 
				CP.CodigoEstadoCompra AS CodigoEstadoTransaccion, 
				CP.CodigoTipoCompra AS CodigoTipoTransaccion,
				MontoTotalCompra AS MontoTotalTransaccion,  
				--CASE WHEN CP.MontoDescuento IS NULL THEN 0 ELSE CP.MontoDescuento END AS MontoDescuento,
				CASE WHEN CodigoTipoCompra =''E'' THEN DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, ''E'') 
				ELSE DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, ''E'') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, ''P'')  END AS MontoCancelado,
				CASE WHEN CodigoTipoCompra =''E'' THEN cp.MontoTotalCompra - DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, ''E'') 
				ELSE cp.MontoTotalCompra -(DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto,''E'') 
				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, ''P''))  END AS MontoSaldo, 
				CP.NumeroCuentaPorPagar AS NumeroCuentaCreditoTransaccion,
				CP.FechaHoraRecepcion AS FechaHoraCulminacion,
				DBO.ObtenerNombreCompleto(CP.DIUsuario) AS UsuarioResponsable '
	SET @F = '	FROM ComprasProductos CP 
				INNER JOIN ComprasProductosDetalle CPD 
				ON CP.NumeroAlmacen = CPD.NumeroAlmacen 
				AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto	
				INNER JOIN Productos P 
				ON CPD.CodigoProducto = P.CodigoProducto
				INNER JOIN Proveedores PR 
				ON PR.CodigoProveedor = CP.CodigoProveedor'
	SET @W = 'WHERE CP.NumeroAlmacen = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND CP.NumeroCompraProducto LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(CP.FechaHoraRegistro As Float)) As DateTime) BETWEEN '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' SA.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN
		SET @W = @W + ' AND ( CP.CodigoEstadoCompra = ''' + @CodigoEstadoTranssacion + ''')'
	END
END

ELSE IF (@TipoTransaccion = 'D') -- SI ES PARA DEVOLUCIONES POR COMPRAS
BEGIN
	SET @S = '	SELECT DISTINCT CPD.NumeroAlmacenDevolucion AS NumeroAlmacen, CPD.NumeroCompraProductoDevolucion AS NumeroTransaccion,
				'' '' AS NumeroComprobante, CPD.FechaHoraRegistro, 
				PR.NombreRazonSocial AS NombrePersonaTransaccion,
				''EFECTIVO'' AS TipoTransaccion,
				CASE
				WHEN CPD.CodigoEstadoCompraDevolucion = ''I'' THEN ''INICIADA'' 
				WHEN CPD.CodigoEstadoCompraDevolucion = ''A'' THEN ''ANULADA'' 
				WHEN CPD.CodigoEstadoCompraDevolucion = ''F'' THEN ''FINALIZADO Y DEVUELTO''  END AS EstadoTransaccion,  
				CAST(CPD.Observaciones AS NVARCHAR(4000)) AS Observaciones, 
				CPD.CodigoEstadoCompraDevolucion AS CodigoEstadoTransaccion, 
				''E'' AS CodigoTipoTransaccion,
				CPD.MontoTotalCompraDevolucion AS MontoTotalTransaccion,  
				CPD.MontoTotalCompraDevolucion AS MontoCancelado,
				0 AS MontoSaldo, 
				NULL AS NumeroCuentaCreditoTransaccion,
				CPD.FechaHoraDevolucion AS FechaHoraCulminacion,
				DBO.ObtenerNombreCompleto(CP.DIUsuario) AS UsuarioResponsable  '
	SET @F = '	FROM ComprasProductosDevoluciones CPD
				INNER JOIN ComprasProductosDevolucionesDetalle CPDD
				ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
				AND CPD.NumeroCompraProductoDevolucion = CPDD.NumeroCompraProductoDevolucion
				INNER JOIN ComprasProductos CP
				ON CP.NumeroAlmacen = CPD.NumeroAlmacen
				AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto
				INNER JOIN Proveedores PR
				ON CP.CodigoProveedor = PR.CodigoProveedor
				INNER JOIN Productos P
				ON CPDD.CodigoProducto = P.CodigoProducto'
	SET @W = 'WHERE CPD.NumeroAlmacenDevolucion = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND CPD.NumeroCompraProductoDevolucion LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(CPD.FechaHoraRegistro As Float)) As DateTime) BETWEEN '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' CPD.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN
		SET @W = @W + ' AND ( CPD.CodigoEstadoCompraDevolucion = ''' + @CodigoEstadoTranssacion + ''')'
	END
END


ELSE IF (@TipoTransaccion = 'V') -- SI ES PARA DEVOLUCIONES POR VENTAS
BEGIN
	SET @S = '	SELECT DISTINCT CPD.NumeroAlmacenDevolucion AS NumeroAlmacen, CPD.NumeroVentaProductoDevolucion AS NumeroTransaccion,
				'' '' AS NumeroComprobante, CPD.FechaHoraRegistro, 
				C.NombreCliente AS NombrePersonaTransaccion,
				''EFECTIVO'' AS TipoTransaccion,
				CASE
				WHEN CPD.CodigoEstadoVentaDevolucion = ''I'' THEN ''INICIADA'' 
				WHEN CPD.CodigoEstadoVentaDevolucion = ''A'' THEN ''ANULADA'' 
				WHEN CPD.CodigoEstadoVentaDevolucion = ''F'' THEN ''FINALIZADO Y DEVUELTO''  END AS EstadoTransaccion,  
				CAST(CPD.Observaciones AS NVARCHAR(4000)) AS Observaciones, 
				CPD.CodigoEstadoVentaDevolucion AS CodigoEstadoTransaccion, 
				''E'' AS CodigoTipoTransaccion,
				CPD.MontoTotalVentaDevolucion AS MontoTotalTransaccion,  
				CPD.MontoTotalVentaDevolucion AS MontoCancelado,
				0 AS MontoSaldo, 
				NULL AS NumeroCuentaCreditoTransaccion,
				CPD.FechaHoraDevolucion AS FechaHoraCulminacion,
				DBO.ObtenerNombreCompleto(CP.DIUsuario) AS UsuarioResponsable  '
	SET @F = '	FROM VentasProductosDevoluciones CPD
				INNER JOIN VentasProductosDevolucionesDetalle CPDD
				ON CPD.NumeroAlmacenDevolucion = CPDD.NumeroAlmacenDevolucion
				AND CPD.NumeroVentaProductoDevolucion = CPDD.NumeroVentaProductoDevolucion
				INNER JOIN VentasProductos CP
				ON CP.NumeroAlmacen = CPD.NumeroAlmacen
				AND CP.NumeroVentaProducto = CPD.NumeroVentaProducto
				INNER JOIN Clientes C
				ON CP.CodigoCliente = C.CodigoCliente
				INNER JOIN Productos P
				ON CPDD.CodigoProducto = P.CodigoProducto'
	SET @W = 'WHERE CPD.NumeroAlmacenDevolucion = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND CPD.NumeroVentaProductoDevolucion LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(CPD.FechaHoraRegistro As Float)) As DateTime) BETWEEN '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' CPD.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN
		SET @W = @W + ' AND ( CPD.CodigoEstadoVentaDevolucion = ''' + @CodigoEstadoTranssacion + ''')'
	END
END

ELSE IF (@TipoTransaccion = 'T') -- SI ES PARA SOLICITUDES O COTIZACIONES
BEGIN
	SET @S = '	SELECT	DISTINCT SSA.NumeroSolicitudSalidaArticulo AS NumeroTransaccion, 
				'' '' AS NumeroComprobante, 
				SSA.FechaHoraSolicitud AS FechaHoraRegistro, 
				dbo.ObtenerNombreCompletoPersona(F.DIPersona) AS NombrePersonaTransaccion, 
				CAST(SSA.Observaciones AS VARCHAR(4000)) AS Observaciones,
				SSA.CodigoEstadoSolicitud  AS CodigoEstadoTransaccion,
				CASE SSA.CodigoEstadoSolicitud WHEN ''I'' THEN ''INICIADA'' WHEN ''F'' THEN ''FINALIZADA''
				WHEN ''A'' THEN ''ANULADA'' WHEN ''X'' THEN ''RECEPCION INCOMPLETA'' ELSE ''INDETERMINADO'' END AS EstadoTransaccion,
				0 as MontoTotal,
				SSA.FechaHoraSolicitud as FechaHoraCulminacion,
				U.NombreUsuario AS UsuarioResponsable '
	SET @F = '	FROM SolicitudSalidasArticulos SSA
				INNER JOIN SolicitudSalidasArticulosDetalle SSAD
				ON SSA.NumeroAlmacen = SSAD.NumeroAlmacen
				AND SSA.NumeroSolicitudSalidaArticulo = SSAD.NumeroSolicitudSalidaArticulo
				INNER JOIN Articulos A
				ON SSAD.CodigoArticulo = A.CodigoArticulo
				INNER JOIN Funcionarios F
				ON SSA.CodigoFuncionario = F.CodigoFuncionario
				INNER JOIN Usuarios U
				ON U.CodigoUsuario = SSA.CodigoUsuario'
	SET @W = 'WHERE SSA.NumeroAlmacen = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND SSA.NumeroSolicitudSalidaArticulo LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(SSA.FechaHoraSolicitud As Float)) As DateTime) BETWEEN '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' SSA.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN
		SET @W = @W + ' AND ( SSA.CodigoEstadoSolicitud = ''' + @CodigoEstadoTranssacion + ''')'
	END
END

ELSE IF (@TipoTransaccion = 'C') -- SI ES PARA SOLICITUDES DE COMPRA
BEGIN
	SET @S = '	SELECT	DISTINCT SSA.NumeroSolicitudIngresoArticulo AS NumeroTransaccion, 
				'' '' AS NumeroComprobante, 
				SSA.FechaHoraSolicitud AS FechaHoraRegistro, 
				dbo.ObtenerNombreCompletoPersona(F.DIPersona) AS NombrePersonaTransaccion, 
				CAST(SSA.Observaciones AS VARCHAR(4000)) AS Observaciones,
				SSA.CodigoEstadoSolicitud  AS CodigoEstadoTransaccion,
				CASE SSA.CodigoEstadoSolicitud WHEN ''I'' THEN ''INICIADA'' WHEN ''F'' THEN ''ENVIADA Y FINALIZADA''
				WHEN ''A'' THEN ''ANULADA'' WHEN ''T'' THEN ''AUTORIZADA'' ELSE ''INDETERMINADO'' END AS EstadoTransaccion,
				0 as MontoTotal,
				SSA.FechaHoraSolicitud as FechaHoraCulminacion,
				U.NombreUsuario AS UsuarioResponsable  '
	SET @F = '	FROM SolicitudIngresosArticulos SSA
				INNER JOIN SolicitudIngresosArticulosDetalle SSAD
				ON SSA.NumeroAlmacen = SSAD.NumeroAlmacen
				AND SSA.NumeroSolicitudIngresoArticulo = SSAD.NumeroSolicitudIngresoArticulo
				INNER JOIN Articulos A
				ON SSAD.CodigoArticulo = A.CodigoArticulo
				INNER JOIN Funcionarios F
				ON SSA.CodigoFuncionario = F.CodigoFuncionario
				INNER JOIN Usuarios U
				ON U.CodigoUsuario = SSA.CodigoUsuario'
	SET @W = 'WHERE SSA.NumeroAlmacen = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND SSA.NumeroSolicitudIngresoArticulo LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(SSA.FechaHoraSolicitud As Float)) As DateTime) BETWEEN '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' SSA.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN
		SET @W = @W + ' AND ( SSA.CodigoEstadoSolicitud = ''' + @CodigoEstadoTranssacion + ''')'
	END
END

ELSE IF (@TipoTransaccion = 'R') -- SI ES PARA DISTRIBUCION DE ARTICULOS FRACCIONABLES
BEGIN
	SET @S = '	SELECT DISTINCT DAF.NumeroDistribucionArticulosFraccionados AS NumeroTransaccion,
				DAF.NumeroComprobanteDistribucion AS NumeroComprobante, 
				DAF.FechaHoraRegistro AS FechaHoraRegistro, 
				dbo.ObtenerNombreCompletoPersona(F.DIPersona) AS NombrePersonaTransaccion, 
				CAST(DAF.Observaciones AS VARCHAR(4000)) AS Observaciones,
				DAF.CodigoEstadoDistribucion  AS CodigoEstadoTransaccion,
				CASE DAF.CodigoEstadoDistribucion WHEN ''I'' THEN ''INICIADA'' WHEN ''F'' THEN ''FINALIZADA''
				WHEN ''A'' THEN ''ANULADA'' WHEN ''X'' THEN ''RECEPCION INCOMPLETA'' ELSE ''INDETERMINADO'' END AS EstadoTransaccion,
				0 as MontoTotal,
				DAF.FechaHoraRegistro as FechaHoraCulminacion,
				U.NombreUsuario AS UsuarioResponsable  '
	SET @F = '	FROM DistribucionArticulosFraccionados DAF
				INNER JOIN DistribucionArticulosFraccionadosDetalle DAFD
				ON DAF.NumeroAlmacen = DAFD.NumeroAlmacen
				AND DAF.NumeroDistribucionArticulosFraccionados = DAFD.NumeroDistribucionArticulosFraccionados
				INNER JOIN Articulos A
				ON DAFD.CodigoArticulo = A.CodigoArticulo
				INNER JOIN Funcionarios F
				ON DAF.CodigoFuncionario = F.CodigoFuncionario
				INNER JOIN Usuarios U
				ON U.CodigoUsuario = DAF.CodigoUsuario '
	SET @W = 'WHERE DAF.NumeroAlmacen = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' AND DAF.NumeroDistribucionArticulosFraccionados LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(DAF.FechaHoraRegistro As Float)) As DateTime) BETWEEN  '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' DAF.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	
	BEGIN
		SET @W = @W + ' AND ( DAF.CodigoEstadoDistribucion = ''' + @CodigoEstadoTranssacion + ''')'
	END
END


ELSE IF (@TipoTransaccion = 'F') -- SI ES PARA TRANSFERENCIAS
BEGIN
	SET @S = 'SELECT DISTINCT TP.NumeroAlmacenEmisor AS NumeroAlmacen, TP.NumeroTransferenciaProducto AS NumeroTransaccion,
			  '' '' AS NumeroComprobante, TP.FechaHoraTransferencia as FechaHoraRegistro, 
			  A.NombreAlmacen as NombrePersonaTransaccion, ''EFECTIVO'' AS TipoTransaccion,
			  CASE TP.CodigoEstadoTransferencia WHEN ''I'' THEN ''INICIADA'' WHEN ''F'' THEN ''FINALIZADA''
				WHEN ''A'' THEN ''ANULADA'' ELSE ''INDETERMINADO'' END AS EstadoTransaccion,
				CAST(TP.Observaciones AS VARCHAR(4000)) AS Observaciones,
				TP.CodigoEstadoTransferencia AS CodigoEstadoTransaccion,
				''E'' AS CodigoTipoTransaccion,	TP.NumeroAlmacenRecepctor,			
				TP.MontoTotalTransferencia AS MontoTotalTransaccion, TP.MontoTotalTransferencia AS MontoCancelado,
				0 AS MontoSaldo, 0 AS NumeroCuentaCreditoTransaccion,				
				TP.FechaHoraTransferencia AS FechaHoraCulminacion,
				DBO.ObtenerNombreCompleto(TP.DIUsuario) AS UsuarioResponsable '
	SET @F = 'FROM TransferenciasProductos TP INNER JOIN TransferenciasProductosDetalle TPD ON TP.NumeroAlmacenEmisor = TPD.NumeroAlmacenEmisor AND TP.NumeroTransferenciaProducto = TPD.NumeroTransferenciaProducto
				INNER JOIN Productos P ON TPD.CodigoProducto = P.CodigoProducto
				INNER JOIN dbo.Almacenes A ON TP.NumeroAlmacenRecepctor = A.NumeroAlmacen'
	SET @W = 'WHERE (TP.NumeroAlmacenEmisor = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+' OR TP.NumeroAlmacenRecepctor = '+ RTRIM(LTRIM(CAST(@NumeroAlmacen AS CHAR(100))))+')
	AND TP.NumeroTransferenciaProducto LIKE '+ ISNULL(RTRIM(CAST(@NumeroTransaccion AS VARCHAR(8000))),'''%''')+' AND (Cast(Floor(Cast(FechaHoraTransferencia As Float)) As DateTime) BETWEEN '''+ CAST(@FechaInicio as CHAR(12))+''' AND '''+CAST(@FechaFin as CHAR(12)) +''') '
	SET @NombreCampo = ' TP.Observaciones '
	IF(@CodigoEstadoTranssacion IS NOT NULL)
	BEGIN
		SET @W = @W + ' AND ( TP.CodigoEstadoTransferencia = ''' + @CodigoEstadoTranssacion + ''')'
	END
END	
	

SET @AUX = ' '
--SET @NombreCampo = ''

--'0' -> NombrePersonaRealizaTransacción
--'1' -> NITPersonaRealizaTransacción
--'2' -> Nombre producto

IF @ExactamenteIgual = 1
BEGIN
	SET @OperadorComparacion = '='
	SET @TextoABuscarOptimizado = '''' + @TextoABuscar + ''''
END
ELSE
BEGIN
	SET @OperadorComparacion = 'LIKE'
	SET @TextoABuscarOptimizado = '''%' + @TextoABuscar + '%'''
END

IF (@TipoTransaccion = 'I') OR (@TipoTransaccion ='D') --OR (@TipoTransaccion ='D') OR (@TipoTransaccion ='S')
BEGIN
	IF @CodigoAmbitoBusqueda = '0' --corregir para nombre
		SET @Condicion = 'PR.NombreRazonSocial ' + @OperadorComparacion + @TextoABuscarOptimizado
	ELSE IF @CodigoAmbitoBusqueda = '1' 
		SET @Condicion = 'PR.NITProveedor ' + @OperadorComparacion + @TextoABuscarOptimizado
END
ELSE
BEGIN
	IF(@TipoTransaccion = 'F')
	BEGIN
		IF @CodigoAmbitoBusqueda = '0' 
		SET @Condicion = 'A.NombreAlmacen ' + @OperadorComparacion + @TextoABuscarOptimizado
	ELSE IF @CodigoAmbitoBusqueda = '1' 
		SET @Condicion = 'A.NombreAlmacen ' + @OperadorComparacion + @TextoABuscarOptimizado
	END
	ELSE
	BEGIN
		IF @CodigoAmbitoBusqueda = '0' 
			SET @Condicion = 'C.NombreCliente ' + @OperadorComparacion + @TextoABuscarOptimizado
		ELSE IF @CodigoAmbitoBusqueda = '1' 
			SET @Condicion = 'C.NITCliente ' + @OperadorComparacion + @TextoABuscarOptimizado
	END
	
END
IF (@CodigoAmbitoBusqueda = '2')
BEGIN
	SET @PosicionInicial = 0
	SET @PosicionActual = 0
	SET @PosicionFinal = 0
	SET @NombreCampo = ' P.NombreProducto '
	WHILE LEN(@TextoABuscar) >= @PosicionActual
	BEGIN
		SET @PosicionActual = @PosicionActual + 1
		IF (SUBSTRING(@TextoABuscar, @PosicionActual, 1) = ' ')
		BEGIN
			IF LEN(@AUX) > 1
				SET @AUX = @AUX + ' AND '			
			SET @AUX = @AUX + @NombreCampo + ' LIKE ' + '''%' + SUBSTRING(@TextoABuscar, @PosicionInicial, (@PosicionActual - @PosicionInicial)) + '%'''			
			SET @PosicionInicial = @PosicionActual + 1
		END
	END

	SET @Condicion =  LTRIM(RTRIM(@AUX))
END

IF (@CodigoAmbitoBusqueda = '3')
BEGIN
	SET @PosicionInicial = 0
	SET @PosicionActual = 0
	SET @PosicionFinal = 0	
	WHILE LEN(@TextoABuscar) >= @PosicionActual
	BEGIN
		SET @PosicionActual = @PosicionActual + 1
		IF (SUBSTRING(@TextoABuscar, @PosicionActual, 1) = ' ')
		BEGIN
			IF LEN(@AUX) > 1
				SET @AUX = @AUX + ' AND '			
			SET @AUX = @AUX + @NombreCampo + ' LIKE ' + '''%' + SUBSTRING(@TextoABuscar, @PosicionInicial, (@PosicionActual - @PosicionInicial)) + '%'''			
			SET @PosicionInicial = @PosicionActual + 1
		END
	END

	SET @Condicion =  LTRIM(RTRIM(@AUX))
END


SET @W = @W +' AND ('+RTRIM(@Condicion)+')';

SET @ScriptSQL  = LTRIM(RTRIM(LTRIM(RTRIM(@S)) + ' ' + LTRIM(RTRIM(@F)) + ' ' +  LTRIM(RTRIM(@W))))
PRINT @ScriptSQL

	--SELECT DISTINCT CP.NumeroAlmacen, CP.NumeroCompraProducto AS NumeroTransaccion, 
	--		CP.NumeroComprobante AS NumeroComprobante, 
	--		CP.FechaHoraRegistro AS FechaHoraRegistro, 
	--		PR.NombreRazonSocial AS NombrePersonaTransaccion,
	--		CASE(CodigoTipoCompra) WHEN 'E' THEN 'EFECTIVO' ELSE 'CREDITO' END AS TipoTransaccion, 
	--		CASE
	--		WHEN CodigoEstadoCompra = 'I' THEN 'INICIADA' 
	--		WHEN CodigoEstadoCompra = 'A' THEN 'ANULADA' 
	--		WHEN CodigoTipoCompra = 'E' AND CodigoEstadoCompra = 'P' THEN 'PAGADA EN TRANSITO' 
	--		WHEN CodigoTipoCompra = 'R' AND CodigoEstadoCompra = 'P' THEN 'A CREDITO EN TRANSITO' 
	--		WHEN CodigoEstadoCompra = 'D' THEN 'PENDIENTE'  
	--		WHEN CodigoEstadoCompra = 'F' THEN 'FINALIZADO Y RECIBIDO'  
	--		WHEN CodigoEstadoCompra = 'X' THEN 'FINALIZADO INCOMPLETO' END AS EstadoTransaccion, 
	--		CAST(CP.Observaciones AS NVARCHAR(4000)) AS Observaciones, 
	--		CP.CodigoEstadoCompra AS CodigoEstadoTransaccion, 
	--		CP.CodigoTipoCompra AS CodigoTipoTransaccion,
	--		MontoTotalCompra AS MontoTotalTransaccion,  
	--		--CASE WHEN CP.MontoDescuento IS NULL THEN 0 ELSE CP.MontoDescuento END AS MontoDescuento,
	--		CASE WHEN CodigoTipoCompra ='E' THEN DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, 'E') 
	--		ELSE DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, 'E') 
	--		+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, 'P')  END AS MontoCancelado,
	--		CASE WHEN CodigoTipoCompra ='E' THEN cp.MontoTotalCompra - DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, 'E') 
	--		ELSE cp.MontoTotalCompra -(DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto,'E') 
	--		+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, 'P'))  END AS MontoSaldo, 
	--		CP.NumeroCuentaPorPagar AS NumeroCuentaCreditoTransaccion,
	--		CP.FechaHoraRecepcion AS FechaHoraCulminacion,
	--		DBO.ObtenerNombreCompleto(CP.DIUsuario) AS UsuarioResponsable 
	--FROM ComprasProductos CP INNER JOIN ComprasProductosDetalle CPD ON CP.NumeroAlmacen = CPD.NumeroAlmacen AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto	
	--INNER JOIN Productos P ON CPD.CodigoProducto = P.CodigoProducto
	--INNER JOIN Proveedores PR ON PR.CodigoProveedor = CP.CodigoProveedor

EXEC(@ScriptSQL)
GO


--exec BuscarTransaccionGC 0, ' ',1,null, 'V', '01/01/2000','31/12/2012', 0, null
--exec BuscarTransaccionGC 3,'',1,1,'F','10/10/09','31/10/10',0
--exec BuscarTransaccionGC 0,'',1,null,'S','10/10/09','30/11/09',0

--select isnull(CAST(null as varchar(30)),'''%''')
--select isnull(CAST(342 as varchar(30)),'%')

--select * from VentasProductos
--where NumeroVentaProducto like %

--exec BuscarTransaccionGC 1,'', 1, null, 'S', '10/10/09','31/12/11', 0,NULL

--exec BuscarTransaccionGC 3, '',1,null, 'F', '10/10/09','31/10/12',0,null


--SELECT * FROM ComprasProductos















































--SELECT	DISTINCT SA.NumeroAlmacen, SA.NumeroVentaProducto AS NumeroTransaccion, 
--				SA.NumeroComprobante AS NumeroComprobante, 
--				SA.FechaHoraVenta AS FechaHoraRegistro, 
--				C.NombreCliente AS NombrePersonaTransaccion,
--				CASE(SA.CodigoTipoVenta) WHEN ''E'' THEN ''EFECTIVO'' ELSE ''CREDITO'' END AS TipoTransaccion,	
--				CASE SA.CodigoEstadoVenta WHEN ''I'' THEN ''INICIADA'' WHEN ''F'' THEN ''FINALIZADA''
--				WHEN ''A'' THEN ''ANULADA'' WHEN ''X'' THEN ''ENTREGA INCOMPLETA'' ELSE ''INDETERMINADO'' END AS EstadoTransaccion,					
--				CAST(SA.Observaciones AS VARCHAR(4000)) AS Observaciones,
--				SA.CodigoEstadoVenta AS CodigoEstadoTransaccion,
--				SA.CodigoTipoVenta AS CodigoTipoTransaccion,				
--				SA.MontoTotalVenta AS MontoTotalTransaccion,
--				CASE WHEN SA.CodigoTipoVenta =''E'' THEN DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto, ''V'') 
--				ELSE DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto, ''V'') 
--				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroCuentaPorCobrar, ''C'')  END AS MontoCancelado,
--				CASE WHEN CodigoTipoVenta =''E'' THEN SA.MontoTotalVenta - DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto, ''V'') 
--				ELSE SA.MontoTotalVenta -(DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroVentaProducto,''V'') 
--				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(SA.NumeroAlmacen, SA.NumeroCuentaPorCobrar, ''P''))  END AS MontoSaldo, 
--				SA.NumeroCuentaPorCobrar AS NumeroCuentaCreditoTransaccion,				
--				SA.FechaHoraEntrega AS FechaHoraCulminacion,
--				DBO.ObtenerNombreCompleto(SA.DIUsuario) AS UsuarioResponsable 
--				FROM VentasProductos SA
--				INNER JOIN VentasProductosDetalle SAD
--				ON SA.NumeroAlmacen = SAD.NumeroAlmacen
--				AND SA.NumeroVentaProducto = SAD.NumeroVentaProducto
--				INNER JOIN Productos P
--				ON SAD.CodigoProducto = P.CodigoProducto
--				INNER JOIN Clientes C
--				ON SA.CodigoCliente = C.CodigoCliente
				
				
				


--SELECT DISTINCT CP.NumeroAlmacen, CP.NumeroCompraProducto AS NumeroTransaccion, 
--				CP.NumeroComprobante AS NumeroComprobante, 
--				CP.FechaHoraRegistro AS FechaHoraRegistro, 
--				PR.NombreRazonSocial AS NombrePersonaTransaccion,
--				CASE(CodigoTipoCompra) WHEN ''E'' THEN ''EFECTIVO'' ELSE ''CREDITO'' END AS TipoTransaccion, 
--				CASE
--				WHEN CodigoEstadoCompra = ''I'' THEN ''INICIADA'' 
--				WHEN CodigoEstadoCompra = ''A'' THEN ''ANULADA'' 
--				WHEN CodigoTipoCompra = ''E'' AND CodigoEstadoCompra = ''P'' THEN ''PAGADA EN TRANSITO'' 
--				WHEN CodigoTipoCompra = ''R'' AND CodigoEstadoCompra = ''P'' THEN ''A CREDITO EN TRANSITO'' 
--				WHEN CodigoEstadoCompra = ''D'' THEN ''PENDIENTE''  
--				WHEN CodigoEstadoCompra = ''F'' THEN ''FINALIZADO Y RECIBIDO''  
--				WHEN CodigoEstadoCompra = ''X'' THEN ''FINALIZADO INCOMPLETO'' END AS EstadoTransaccion, 
--				CAST(CP.Observaciones AS NVARCHAR(4000)) AS Observaciones, 
--				CP.CodigoEstadoCompra AS CodigoEstadoTransaccion, 
--				CP.CodigoTipoCompra AS CodigoTipoTransaccion,
--				MontoTotalCompra AS MontoTotalTransaccion,  
--				--CASE WHEN CP.MontoDescuento IS NULL THEN 0 ELSE CP.MontoDescuento END AS MontoDescuento,
--				CASE WHEN CodigoTipoCompra =''E'' THEN DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, ''E'') 
--				ELSE DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, ''E'') 
--				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, ''P'')  END AS MontoCancelado,
--				CASE WHEN CodigoTipoCompra =''E'' THEN cp.MontoTotalCompra - DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto, ''E'') 
--				ELSE cp.MontoTotalCompra -(DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCompraProducto,''E'') 
--				+ DBO.ObtenerMontoTotalCuentasCobrosPagos(CP.NumeroAlmacen, CP.NumeroCuentaPorPagar, ''P''))  END AS MontoSaldo, 
--				CP.NumeroCuentaPorPagar AS NumeroCuentaCreditoTransaccion,
--				CP.FechaHoraRecepcion AS FechaHoraCulminacion,
--				DBO.ObtenerNombreCompleto(CP.DIUsuario) AS UsuarioResponsable 
--	FROM ComprasProductos CP 
--	INNER JOIN ComprasProductosDetalle CPD 
--	ON CP.NumeroAlmacen = CPD.NumeroAlmacen 
--	AND CP.NumeroCompraProducto = CPD.NumeroCompraProducto	
--	INNER JOIN Productos P 
--	ON CPD.CodigoProducto = P.CodigoProducto
--	INNER JOIN Proveedores PR 
--	ON PR.CodigoProveedor = CP.CodigoProveedor
