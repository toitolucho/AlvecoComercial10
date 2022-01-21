USE alvecocomercial10
GO

DROP FUNCTION ObtenerMontoNumerico_EnTexto
GO

CREATE FUNCTION ObtenerMontoNumerico_EnTexto(@monto NUMERIC(14,2), @moneda CHAR(10))
RETURNS CHAR(255)	
AS
BEGIN
DECLARE @unidades		CHAR(255),
		@decenAS		CHAR(255),
		@centenAS		CHAR(255),
		@especiales		CHAR(108),
		@decimales		CHAR(25),
		@valor_entero	CHAR(9),
		@longitud		INT,
		@caracteres		CHAR(3), 
		@contador		INT,
		@posicion		INT,
		@flag			INT,
		@decimal		INT,
		@letrAS CHAR(255)

--SET NOCOUNT ON
	SELECT @unidades = 'un    dos   tres  cuatro' +'cinco seis  siete ocho  ' + 'nueve '
	SELECT @especiales = 'once        doce        trece       '  +    'catorce     quince      diez y seis '  +    'diez y sietediez y ocho diez y nueve'
	SELECT @decenAS = 'diez     veINTe   treINTa  cuarenta ' +    'cincuentASesenta  setenta  ochenta  ' +    'noventa  '
	SELECT @centenAS = 'ciento       doscientos   trescientos  cuatrocientos' +    'quinientos   seiscientos  setecientos  ochocientos  ' +    'novecientos  '
	SELECT @decimal = (@monto - CAST(@monto AS INT)) * 100
	SELECT @monto  = round(@monto,0,1)
	SELECT @longitud  = LEN( RTRIM(CAST(CAST(@monto AS INT) AS char)))
	SELECT @valor_entero = RTRIM(CAST(CAST(@monto AS INT) AS char))
	SELECT @valor_entero = REPLICATE('0',9-@longitud)+ SUBSTRING(@valor_entero,1,@longitud)
	
	SELECT @contador = 1,@letrAS  = REPLICATE(' ',255)
	WHILE @contador < 8
	 BEGIN /* 0 */
	 SELECT @caracteres = SUBSTRING(@valor_entero,@contador,3)
	 IF @caracteres <> '000'
	  BEGIN /* 1 */
	  IF SUBSTRING(@caracteres,1,1) <> '0'
	   -- CENTENAS
	   BEGIN /* 2 */
	   SELECT @posicion = CAST(SUBSTRING(@caracteres,1,1) AS INT)
	   IF  @posicion = '1' and
		CAST(SUBSTRING(@caracteres,2,2) AS INT) = 0
		BEGIN /* 3 */
	    
		SELECT @letrAS = RTRIM(@letrAS) + ' Cien '
		END /* 3 */
	   ELSE
		BEGIN /* 4 */
		SELECT @letrAS = RTRIM(@letrAS)      +       ' '       +
		   SUBSTRING(@centenAS, 13 * (@posicion - 1) + 1,13)
		END /* 4 */
	   END /* 2 */
	  SELECT @flag = 0
	  IF CAST(SUBSTRING(@caracteres,2,2) AS INT) > 10 and
	   CAST(SUBSTRING(@caracteres,2,2) AS INT) < 20
	   -- ESPECIALES
	   BEGIN /* 5 */
	   SELECT @posicion = CAST(SUBSTRING(@caracteres,3,1) AS INT)
	   SELECT @letrAS = RTRIM(@letrAS)      +
		  ' '       +
		  SUBSTRING(@especiales, 12 * (@posicion - 1) + 1,12)
	   SELECT @flag = 1
	   END /* 5 */
	  IF @flag = 0
	   -- DECENAS
	   BEGIN /* 6 */
	   IF SUBSTRING(@caracteres,2,1) <> '0'
		BEGIN /* 7 */
		SELECT @posicion = CAST(SUBSTRING(@caracteres,2,1) AS INT)
		IF @posicion <> 2 or
		 SUBSTRING(@caracteres,3,1) = '0'
		 BEGIN /* 8 */
		 SELECT @letrAS = RTRIM(@letrAS)      +
			' '       +
			SUBSTRING(@decenAS, 9 * (@posicion - 1) + 1,9)
	     
		 END /* 8 */
		ELSE
		 BEGIN /* 9 */
	     
		 SELECT @letrAS = RTRIM(@letrAS)      +        ' VEINTI'
		 END /* 9 */
	    
		END /* 7 */
	   
	   IF SUBSTRING(@caracteres,3,1) <> '0'
	 
		-- UNIDADES
		BEGIN /* 10 */
		SELECT @posicion = CAST(SUBSTRING(@caracteres,3,1) AS INT)
		IF SUBSTRING(@caracteres,2,1) <> '0' and
		 SUBSTRING(@caracteres,3,1) <> '0'
		 BEGIN /* 11 */
		 IF SUBSTRING(@caracteres,2,1) = '2'
		  SELECT @letrAS = RTRIM(@letrAS)     +
			 SUBSTRING(@unidades, 6 * (@posicion - 1) + 1,6)
		 ELSE
		  SELECT @letrAS = RTRIM(@letrAS)     +
			 ' y '      +
			 SUBSTRING(@unidades, 6 * (@posicion - 1) + 1,6)
		 END /* 11 */
	    
		ELSE
		 SELECT @letrAS = RTRIM(@letrAS)      +
			' '       +
			SUBSTRING(@unidades, 6 * (@posicion - 1) + 1,6)
		END /* 10 */
	 
	   END /* 6 */
	  IF @contador = 1
	   BEGIN /* 12 */ 
	   IF @posicion   = 1 and
		SUBSTRING(@caracteres,1,2) = '00'
		BEGIN /* 13 */
		SELECT @letrAS = RTRIM(@letrAS) +       ' millón '
		END /* 13 */
	   ELSE
	 
	--    IF @posicion = 1
		 BEGIN /* 14 */
	 
		 SELECT @letrAS = RTRIM(@letrAS) +        ' millones '
		 END /* 14 */
	   END /* 12 */
	  ELSE
	   BEGIN /* 15 */
	   IF @contador = 4
		BEGIN /* 16 */
		SELECT @letrAS = RTRIM(@letrAS) +       ' mil '
		END /* 16 */
	   END /* 15 */
	  END /* 1 */
	 SELECT @contador = @contador + 3
	  
	 END /* 0 */
	-- CIENTOS
	IF right(RTRIM(@letrAS),6) = 'ciento'
	 BEGIN /* 17 */
	 SELECT @letrAS = SUBSTRING(@letrAS,1,LEN(RTRIM(@letrAS))-6) +    'cien '
	 END /* 17 */
	-- DECIMALES
	IF @decimal > 0
	 SELECT @decimales = '  ' + @moneda + ' con '        +     REPLICATE ('0' ,2 - LEN(RTRIM(LTRIM(CAST (@decimal AS char(2)))))) +
		 LTRIM(RTRIM(CAST (@decimal AS char(2))))   +     '/100'
	ELSE
	 SELECT @decimales = '  ' + @moneda + ' exactos'
	-- FINAL

	--SELECT @letrAS  = '** '    +    RTRIM(SUBSTRING(@letrAS,1,255)) +    RTRIM(@decimales)  +    ' **'
	SELECT @letrAS  = UPPER(LTRIM(RTRIM( RTRIM(SUBSTRING(@letrAS,1,255)) +    RTRIM(@decimales))))
	--SELECT 'letrAS' = UPPER(@letrAS)
	RETURN @letrAS
END
GO

--SELECT dbo.ObtenerMontoNumerico_EnTexto('999000508.00 ','Bolivianos')