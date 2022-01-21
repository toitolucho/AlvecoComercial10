USE AlvecoComercial10
GO


DROP FUNCTION GenerarCodigoPais
GO

CREATE FUNCTION GenerarCodigoPais()	
	RETURNS CHAR(2)
BEGIN
	DECLARE @CodigoPais CHAR(2)
	SELECT @CodigoPais = CAST(COUNT(*) + 1 AS CHAR(2))
	FROM Paises	
	
	
	
	RETURN ISNULL( RIGHT( '0' + @CodigoPais,2) ,'01')
END
GO




DROP FUNCTION GenerarCodigoDepartamento
GO

CREATE FUNCTION GenerarCodigoDepartamento(@CodigoPais CHAR(2))	
	RETURNS CHAR(2)
BEGIN
	DECLARE @CodigoDepartamento CHAR(2)
	SELECT TOP 1  @CodigoDepartamento = RIGHT( '0'+ RTRIM(CAST(CAST(CodigoDepartamento AS INT) +1  AS CHAR(2))) ,2)
	FROM Departamentos
	WHERE CodigoPais = @CodigoPais
	ORDER BY 1 DESC
	
	RETURN ISNULL(@CodigoDepartamento, '01')
END
GO




DROP FUNCTION GenerarCodigoProvincia
GO

CREATE FUNCTION GenerarCodigoProvincia(@CodigoPais CHAR(2), @CodigoDepartamento CHAR(2))	
	RETURNS CHAR(2)
BEGIN
	DECLARE @CodigoProvincia CHAR(2)
	SELECT TOP 1 @CodigoProvincia = RIGHT( '0'+ RTRIM(CAST(CAST(CodigoProvincia AS INT) +1  AS CHAR(2))) ,2)
	FROM Provincias
	WHERE CodigoPais = @CodigoPais
	AND CodigoDepartamento = @CodigoDepartamento
	ORDER BY 1 DESC
	
	RETURN ISNULL(@CodigoProvincia, '01')
END
GO



DROP FUNCTION GenerarCodigoLugar
GO

CREATE FUNCTION GenerarCodigoLugar(@CodigoPais CHAR(2), @CodigoDepartamento CHAR(2), @CodigoProvincia CHAR(2))	
	RETURNS CHAR(2)
BEGIN
	DECLARE @CodigoLugar CHAR(2)
	SELECT TOP 1  @CodigoLugar = RIGHT( '0'+ RTRIM(CAST(CAST(CodigoLugar AS INT) +1  AS CHAR(2))) ,2)
	FROM Lugares
	WHERE CodigoPais = @CodigoPais
	AND CodigoDepartamento = @CodigoDepartamento
	AND CodigoProvincia = @CodigoProvincia
	ORDER BY 1 DESC
	
	RETURN ISNULL(@CodigoLugar, '01')
END
GO

--SELECT DBO.GenerarCodigoLugar('BO','11','01')

--SELECT TOP 1   CAST(CodigoDepartamento AS INT),  RIGHT( '0'+ RTRIM(CAST(CAST(CodigoDepartamento AS INT) +2  AS CHAR(2))) ,2)FROM Departamentos
--WHERE CodigoPais = 'BO'
--ORDER BY 1 DESC

--SELECT TOP 1   CAST(CodigoProvincia AS INT),  RIGHT( '0'+ RTRIM(CAST(CAST(CodigoProvincia AS INT) +1  AS CHAR(2))) ,2)
--FROM Provincias
--WHERE CodigoPais = 'BO'
--AND CodigoDepartamento = '01'
--ORDER BY 1 DESC


--SELECT TOP 1  RIGHT( '0'+ RTRIM(CAST(CAST(CodigoLugar AS INT) +1  AS CHAR(2))) ,2)
--FROM Lugares
--WHERE CodigoPais = 'BO'
--AND CodigoDepartamento = '01'
--AND CodigoProvincia = '01'
--ORDER BY 1 DESC


--SELECT * FROM Provincias
--WHERE CodigoPais = 'BO'
--AND CodigoDepartamento = '01'
--ORDER BY 3 DESC


--SELECT RIGHT('00555',4)

