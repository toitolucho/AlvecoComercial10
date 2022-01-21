USE AlvecoComercial10
GO

DROP PROCEDURE CrearBackup
GO

CREATE PROCEDURE CrearBackup @Path VARCHAR(4000)
AS
BEGIN
	DECLARE @FechaHoy DATETIME
	SET @FechaHoy = GETDATE()
	DECLARE @FormatoFecha CHAR(13) 
	SET @FormatoFecha = RIGHT('0'+RTRIM(CAST(DATEPART("DAY" ,@FechaHoy) AS CHAR(2))),2)
	+RIGHT('0'+RTRIM(CAST(DATEPART("MONTH",@FechaHoy) AS CHAR(2))),2)
	+CAST(DATEPART("YEAR" ,@FechaHoy) AS CHAR(4)) +'_'
	+RIGHT('0'+RTRIM(CAST(DATEPART("HH",@FechaHoy) AS CHAR(2))),2)
	+RIGHT('0'+RTRIM(CAST(DATEPART("MINUTE",@FechaHoy) AS CHAR(2))),2)
	DECLARE @RutaBackup VARCHAR(300) 
	SET @RutaBackup = @Path + '\AlvecoComercial10_'+ @FormatoFecha +'.bak'
	
	BACKUP DATABASE [AlvecoComercial10] TO  DISK = @rutabackup
END

