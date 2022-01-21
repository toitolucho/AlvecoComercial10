USE master
GO

DROP PROCEDURE RestaurarBaseDatos
GO

CREATE PROCEDURE RestaurarBaseDatos
	@NombreArchivoRestaurar VARCHAR(100), 
	@DirectorioBackup		VARCHAR(100),
	@NombreBaseDatos		VARCHAR(100)
AS
BEGIN
	
	--declare @NombreArchivoRestaurar varchar(100), @DirectorioBackup varchar(100),
	DECLARE @databaseDataFilename	VARCHAR(200), 
			@databaseLogFilename	VARCHAR(200),
			@databaseDataFile		VARCHAR(200), 
			@databaseLogFile		VARCHAR(200),
			@execSql				NVARCHAR(2000)

	-- Set the name of the database to restore
	--set @NombreBaseDatos = 'AlvecoComercial10'
	---- Set the path to the directory containing the database backup
	--set @DirectorioBackup = 'K:\Proyectos\AlvecoComercial10\BD_Backups\' -- such as 'c:\temp\'

	---- Create the backup file name based on the restore directory, the database name and today's date
	--set @NombreArchivoRestaurar = @DirectorioBackup + @NombreBaseDatos + '-' + replace(convert(varchar, getdate(), 110), '-', '.') + '.bak'

	
	exec('use master')
	-- obtenermos el archivo path y su dirrección fisica
	SELECT @databaseDataFile = rtrim([Name]),@databaseDataFilename = rtrim([Filename])
	FROM master.dbo.sysaltfiles as files
	INNER JOIN master.dbo.sysfilegroups as groups
	ON files.groupID = groups.groupID
	WHERE DBID = (
	  SELECT DBID
	  FROM master.dbo.sysdatabases
	  WHERE [Name] = @NombreBaseDatos
	)
	
	-- Obtenemos el Archivo log  y su directorio
	SELECT @databaseLogFile = rtrim([Name]), @databaseLogFilename = rtrim([Filename])
	FROM master.dbo.sysaltfiles as files
	WHERE DBID = (
	  SELECT DBID
	  FROM master.dbo.sysdatabases
	  WHERE [Name] = @NombreBaseDatos
	)
	AND groupID = 0

	print 'Eliminando las conecciones establecidas con la base de datos "' + @NombreBaseDatos 

	-- Creamos el comando que eliminar las conecciones actuales
	set @execSql = ''
	
	SELECT @execSql = @execSql + 'kill ' + convert(char(10), spid) + ' '
	FROM master.dbo.sysprocesses
	WHERE db_name(dbid) = @NombreBaseDatos
	AND DBID <> 0
	AND spid <> @@spid
	

	SET @execSql = 'USE MASTER ' + @execSql
	exec (@execSql)

	print 'Restaurando la Base de datos "' + @NombreBaseDatos + '" Desde el Directorio "' + @NombreArchivoRestaurar + '" con '
	print '  el Archivo de Datos : "' + @databaseDataFile + '" ubicado en el directorio "' + @databaseDataFilename + '"'
	print '  asi tambien el archivo de Logs "' + @databaseLogFile + '" ubicado en el directorio "' + @databaseLogFilename + '"'

	set @execSql = '
	restore database [' + @NombreBaseDatos + ']
	from disk = ''' +@DirectorioBackup +'\'+ @NombreArchivoRestaurar + '''
	with
	  file = 1,
	  move ''' + @databaseDataFile + ''' to ' + '''' + @databaseDataFilename + ''',
	  move ''' + @databaseLogFile + ''' to ' + '''' + @databaseLogFilename + ''',
	  norewind,
	  nounload,
	  replace'
	SET @execSql = 'USE MASTER  ' + @execSql
	exec sp_executesql @execSql
	
	
	exec('use ' + @NombreBaseDatos)


END
GO