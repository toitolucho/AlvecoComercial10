DECLARE @FechaHoy DATETIME = GETDATE()
DECLARE @FormatoFecha CHAR(13) = RIGHT('0'+RTRIM(CAST(DATEPART("DAY" ,@FechaHoy) AS CHAR(2))),2)
+RIGHT('0'+RTRIM(CAST(DATEPART("MONTH",@FechaHoy) AS CHAR(2))),2)
+CAST(DATEPART("YEAR" ,@FechaHoy) AS CHAR(4)) +'_'
+RIGHT('0'+RTRIM(CAST(DATEPART("HH",@FechaHoy) AS CHAR(2))),2)
+RIGHT('0'+RTRIM(CAST(DATEPART("MINUTE",@FechaHoy) AS CHAR(2))),2)
DECLARE @RutaBackup VARCHAR(300) = 'd:\Proyectos\AlvecoComercial10\BD_Backups\AlvecoComercial10_'+ @FormatoFecha +'.bak'
print @FormatoFecha + ' ruta ' + @rutabackup
BACKUP DATABASE AlvecoComercial10 TO  DISK = @rutabackup
GO
