USE AlvecoComercial10
GO

DROP PROCEDURE ListarBitacoraReporte
GO

CREATE PROCEDURE ListarBitacoraReporte
	@NumeroAlmacen		INT,
	@DIUsuario			CHAR(15),	
	@FechaHoraInicio	DATETIME,
	@FechaHoraFin		DATETIME,
	@NombreBaseDatos	VARCHAR(100)	
AS
BEGIN

	
	SET @FechaHoraInicio = DBO.FormatearFechaInicioFin(@FechaHoraInicio,1)
	SET @FechaHoraFin = DBO.FormatearFechaInicioFin(@FechaHoraFin,0)
	
	DECLARE @NombreUsuario CHAR(32)
	
	SELECT @NombreUsuario = NombreUsuario
	FROM Usuarios
	WHERE DIUsuario = @DIUsuario
	
	SELECT BitacoraID, CASE WHEN EventType = 'RPC Event' THEN 'Ejecución desde aplicacion' 
			WHEN EventType = 'Language Event' THEN 'Ejecución dentro del gestor de base de datos' 
			ELSE 'Ejecucion desconocida' END AS TipoEjecucionEvento,
			CASE WHEN Status = 0 THEN 'Ejecucion Satisfactoria' ELSE 'Ejecucion Incorrecta' END AS StatusEjecucion,
			CASE WHEN SUBSTRING(EventInfo,0, LEN(@NombreBaseDatos) +1) = @NombreBaseDatos THEN
				SUBSTRING(EventInfo, LEN(@NombreBaseDatos + '.dbo.') +1 , LEN(EventInfo) - LEN(@NombreBaseDatos + '.dbo.') +1 -4)
				ELSE EventInfo END AS InformacionEvento,
			Usuario, Fecha, ValoresInsertados, ValoresEliminados
	FROM Bitacora B
	WHERE Fecha BETWEEN @FechaHoraInicio AND @FechaHoraFin
	AND Usuario LIKE (CASE WHEN @NombreUsuario IS NULL THEN '%%' ELSE rtrim(@NombreUsuario) END)
	ORDER BY (CASE WHEN @NombreUsuario IS NULL THEN 6 ELSE 5 END)
END
GO
