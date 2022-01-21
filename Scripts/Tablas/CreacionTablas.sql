USE AlvecoComercial10
GO
DROP TABLE TransferenciasProductosDetalle
GO
DROP TABLE TransferenciasProductos
GO
DROP TABLE VentasProductosDevolucionesDetalle
GO
DROP TABLE VentasProductosDevoluciones
GO
DROP TABLE VentasProductosDetalleEntrega
GO
DROP TABLE VentasProductosDetalle
GO
DROP TABLE VentasProductos
GO
DROP TABLE ComprasProductosDevolucionesDetalle
GO
DROP TABLE ComprasProductosDevoluciones
GO
DROP TABLE ComprasProductosDetalleEntrega
GO
DROP TABLE ComprasProductosDetalle
GO
DROP TABLE ComprasProductos
GO
DROP TABLE CuentasPorCobrarCobros
GO
DROP TABLE CuentasPorCobrar
GO
DROP TABLE CuentasPorPagarPagos
GO
DROP TABLE CuentasPorPagar
GO
DROP TABLE Conceptos
GO
DROP TABLE InventariosProductosCantidadesTransaccionesHistorial
GO
DROP TABLE InventariosProductos
GO
DROP TABLE Usuarios				
GO
DROP TABLE Personas
GO
DROP TABLE Proveedores
GO
DROP TABLE Movilidades
GO
DROP TABLE MovilidadesModelos
GO
DROP TABLE Productos
GO
DROP TABLE ProductosUnidades
GO
DROP TABLE ProductosMarcas
GO
DROP TABLE ProductosTipos
GO
DROP TABLE Clientes
GO
DROP TABLE Lugares
GO
DROP TABLE Provincias
GO
DROP TABLE Departamentos
GO
DROP TABLE Paises
GO
DROP TABLE Almacenes
GO
--DROP TABLE Bitacora
--GO



CREATE TABLE Almacenes
(
NumeroAlmacen			INT				NOT NULL IDENTITY(1,1) PRIMARY KEY,
NombreAlmacen			VARCHAR(250)	NOT NULL,
Descripcion				TEXT			NULL
)
GO


CREATE TABLE Paises
(
CodigoPais				CHAR(2)				NOT NULL PRIMARY KEY,
NombrePais				VARCHAR(250)		NOT NULL UNIQUE,
Nacionalidad			VARCHAR(250)		NULL
)
GO

CREATE TABLE Departamentos
(
CodigoPais				CHAR(2)				NOT NULL,
CodigoDepartamento		CHAR(2)				NOT NULL,
NombreDepartamento		VARCHAR(250)		NOT NULL,
PRIMARY KEY (CodigoPais, CodigoDepartamento),
FOREIGN KEY (CodigoPais) 
REFERENCES Paises(CodigoPais)
)
GO

CREATE TABLE Provincias
(
CodigoPais				CHAR(2)				NOT NULL,
CodigoDepartamento		CHAR(2)				NOT NULL,
CodigoProvincia			CHAR(2)				NOT NULL,
NombreProvincia			VARCHAR(250)		NOT NULL,
PRIMARY KEY (CodigoPais, CodigoDepartamento, CodigoProvincia),
FOREIGN KEY (CodigoPais, CodigoDepartamento) 
REFERENCES Departamentos(CodigoPais, CodigoDepartamento)
)

GO
CREATE TABLE Lugares
(
CodigoPais				CHAR(2)				NOT NULL,
CodigoDepartamento		CHAR(2)				NOT NULL,
CodigoProvincia			CHAR(2)				NOT NULL,
CodigoLugar				CHAR(3)				NOT NULL,
NombreLugar				VARCHAR(250)		NOT NULL,
PRIMARY KEY(CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar),
FOREIGN KEY (CodigoPais, CodigoDepartamento, CodigoProvincia) 
REFERENCES Provincias(CodigoPais, CodigoDepartamento, CodigoProvincia)
)
GO

CREATE TABLE Personas
(
DIPersona						CHAR(15)		NOT NULL PRIMARY KEY,
Paterno							VARCHAR(40)		NULL,
Materno							VARCHAR(40)		NULL,
Nombres							VARCHAR(80)		NOT NULL,
FechaNacimiento					DATETIME		NULL,
Sexo							CHAR(1)			NOT NULL DEFAULT 'M' CHECK(Sexo IN ('M', 'F')),
EstadoCivil						CHAR(1)			NOT NULL DEFAULT 'S' CHECK(EstadoCivil IN ('S', 'C', 'D', 'V', 'O')),
NombreCargo						VARCHAR(250)	NULL,
Celular							VARCHAR(30)		NULL,
Email							TEXT			NULL,
CodigoPaisD						CHAR(2)			NULL,
CodigoDepartamentoD				CHAR(2)			NULL,
CodigoProvinciaD				CHAR(2)			NULL,
CodigoLugarD					CHAR(3)			NULL,
DireccionD						VARCHAR(250)	NULL,
TelefonoD						VARCHAR(30)		NULL,
Observaciones					TEXT			NULL,
CONSTRAINT FK_PL02 FOREIGN KEY (CodigoPaisD, CodigoDepartamentoD, CodigoProvinciaD, CodigoLugarD)
REFERENCES Lugares(CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar)
)
GO

CREATE TABLE Usuarios					
(
--CodigoUsuario						INT				NOT NULL IDENTITY (1, 1),
DIUsuario							CHAR(15)		NOT NULL REFERENCES Personas(DIPersona),
NombreUsuario						CHAR(32)		NOT NULL UNIQUE,
Contrasena							VARBINARY(255) 	NOT NULL,
FechaRegistro						DATETIME		DEFAULT GETDATE(),
CodigoTipoUsuario					CHAR(1)			NOT NULL CHECK(CodigoTipoUsuario IN ('A','V','M','O')),--A->'ADMINISTRADOR', V->ENCARGADO DE VENTA, M->ALMCENES, O->OTROS
NumeroAlmacen						INT				NULL,
Observaciones						TEXT			NULL,
CONSTRAINT PK_Usuarios PRIMARY KEY(DIUsuario),
CONSTRAINT FK_UsuariosAlmacenes	FOREIGN KEY (NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen)
)
GO

CREATE TABLE Clientes
(
CodigoCliente				INT				NOT NULL IDENTITY(1, 1) PRIMARY KEY,
NombreCliente				VARCHAR(250)	NOT NULL,
NombreRepresentante			VARCHAR(250)	NOT NULL,
CodigoTipoCliente			CHAR(1)			NOT NULL DEFAULT 'P' CHECK(CodigoTipoCliente IN ('P','E')), --'P'->Persona; 'E'->Empresa
NITCliente					VARCHAR(30)		NULL,
CodigoPais					CHAR(2)			NULL,
CodigoDepartamento			CHAR(2)			NULL,
CodigoProvincia				CHAR(2)			NULL,
CodigoLugar					CHAR(3)			NULL,
Direccion					VARCHAR(250)	NULL,
Telefono					VARCHAR(50)		NULL,
Email						TEXT			NULL,
Observaciones				TEXT			NULL,
CodigoEstadoCliente			CHAR(1)			NOT NULL DEFAULT 'H' CHECK(CodigoEstadoCliente IN ('H', 'I', 'S')) --'H'->Habilitado, 'I'->Inhabilitado, 'S'->Supendido
UNIQUE (NombreCliente, NombreRepresentante),
FOREIGN KEY (CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar) 
REFERENCES Lugares(CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar)
)
GO

CREATE TABLE Proveedores
(
CodigoProveedor			INT					NOT NULL IDENTITY (1,1),
NombreRazonSocial		VARCHAR(250)		NOT NULL,
NombreRepresentante		VARCHAR(250)		NOT NULL,
CodigoTipoProveedor		CHAR(1)				NOT NULL DEFAULT 'P' CHECK(CodigoTipoProveedor IN ('P','E')), --'P'->Persona; 'E'->Empresa
Rubro					VARCHAR(250)		NULL,
NITProveedor			VARCHAR(30)			NULL,
CodigoPais				CHAR(2)				NULL,
CodigoDepartamento		CHAR(2)				NULL,
CodigoProvincia			CHAR(2)				NULL,
CodigoLugar				CHAR(3)				NULL,
Direccion				VARCHAR(250)		NULL,
Telefono				VARCHAR(50)			NULL,
Fax						VARCHAR(50)			NULL,
Casilla					CHAR(15)			NULL,
Email					TEXT				NULL,
PorcentajeGananciaVentaPorMayor	DECIMAL(10,2) NULL,
PorcentajeGananciaVentaPorMenor DECIMAL(10,2) NULL,
Observaciones			TEXT				NULL,
ProveedorActivo			BIT					NOT NULL DEFAULT 1,
UNIQUE (NombreRazonSocial, NombreRepresentante),
CONSTRAINT PK_Proveedores PRIMARY KEY(CodigoProveedor),
CONSTRAINT FK_Lugares FOREIGN KEY (CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar) 
REFERENCES Lugares(CodigoPais, CodigoDepartamento, CodigoProvincia, CodigoLugar),
)
GO

CREATE TABLE ProductosTipos
(
CodigoProductoTipo			CHAR(10)			NOT NULL PRIMARY KEY,
CodigoProductoTipoPadre		CHAR(10)			NULL,
NombreProductoTipo			VARCHAR(250)		NOT NULL,
DescripcionProductoTipo		TEXT				NULL
)
GO

CREATE TABLE ProductosMarcas
(
CodigoMarca				INT					NOT NULL IDENTITY (1,1) PRIMARY KEY,
NombreMarca				VARCHAR(250)		NOT NULL,
CodigoTipoMarca			CHAR(1)				NULL DEFAULT 'P' CHECK (CodigoTipoMarca IN ('P','M')) --'P'->PRODUCTOS, 'M'->MOVILIDADES
)
GO

CREATE TABLE ProductosUnidades
(
CodigoUnidad            INT					NOT NULL IDENTITY (1,1) PRIMARY KEY,
NombreUnidad            VARCHAR(250)		NOT NULL UNIQUE
)
GO

CREATE TABLE Productos
(
CodigoProducto				CHAR(15)		NOT NULL PRIMARY KEY,
CodigoProductoFabricante	CHAR(30)		NOT NULL UNIQUE,
NombreProducto				VARCHAR(250)	NOT NULL UNIQUE,
NombreProductoAlternativo	VARCHAR(250)	NULL,
CodigoProductoTipo			CHAR(10)		NOT NULL REFERENCES ProductosTipos(CodigoProductoTipo),
CodigoMarcaProducto			INT				NULL REFERENCES ProductosMarcas(CodigoMarca),
CodigoUnidadProducto		INT				NOT NULL REFERENCES ProductosUnidades(CodigoUnidad),
CodigoTipoCalculoInventario	CHAR(1)			NOT NULL DEFAULT 'O', --U'->UEPS, 'P'->PEPS, 'O'->Ponderado, 'B'-> Precio mas Bajo, 'A'->Precio mas alto, 'T'-> Ultimo Precio
ActualizarPrecioVenta		BIT				NULL DEFAULT 1,
CodigoProveedor				INT				NULL,
Descripcion					TEXT			NULL,
Observaciones				TEXT			NULL,
CONSTRAINT FOREIGN KEY (CodigoProveedor) REFERENCES Proveedores(CodigoProveedor) 
)
GO

CREATE TABLE MovilidadesModelos
(
CodigoModelo	INT	IDENTITY(1,1),
NombreModelo	VARCHAR(250)	NOT NULL UNIQUE,
CONSTRAINT PK_MovilidadesModelos PRIMARY KEY(CodigoModelo)
)
GO

CREATE TABLE Movilidades
(
CodigoMovilidad			VARCHAR(10)		NOT NULL,
NombreMovilidad			VARCHAR(200)	NOT NULL UNIQUE,
CodigoPlaca				CHAR(20)		NULL,
CodigoMarca				INT				NOT NULL,
CodigoModelo			INT				NOT NULL,
CodigoEstadoMovilidad	CHAR(1)	NOT NULL CHECK(CodigoEstadoMovilidad IN('A','B')),
Descripcion				TEXT			NULL,
CONSTRAINT PK_Movilidades PRIMARY KEY(CodigoMovilidad),
CONSTRAINT FK_Movilidades_Marcas  FOREIGN KEY(CodigoMarca) REFERENCES ProductosMarcas(CodigoMarca),
CONSTRAINT FK_Movilidades_Modelos FOREIGN KEY(CodigoModelo) REFERENCES MovilidadesModelos(CodigoModelo)
)
GO


CREATE TABLE InventariosProductos
(
NumeroAlmacen					INT				NOT NULL,
CodigoProducto					CHAR(15)		NOT NULL,
CantidadExistencia				INT				NOT NULL DEFAULT 0 CHECK (CantidadExistencia >= 0),
CantidadRequerida				INT				NOT NULL DEFAULT 0,
PrecioUnitarioCompra			DECIMAL(10,2)	NOT NULL DEFAULT 0,
PrecioValoradoTotal				DECIMAL(10,2)	DEFAULT 0,
PrecioUnitarioVentaPorMayor		DECIMAL(10,2)	NOT NULL DEFAULT 0,
PrecioUnitarioVentaPorMenor		DECIMAL(10,2)	NOT NULL DEFAULT 0,
PorcentajeGananciaVentaPorMayor	DECIMAL(10,2)	NOT NULL DEFAULT 0,
PorcentajeGananciaVentaPorMenor	DECIMAL(10,2)	NOT NULL DEFAULT 0,
TiempoGarantiaProducto			INT				NOT NULL DEFAULT 0,
StockMinimo						INT				NOT NULL DEFAULT 1,
FechaHoraIngresoInventario		DATETIME		DEFAULT GETDATE(),
CONSTRAINT PK_InventariosProductos PRIMARY KEY (NumeroAlmacen, CodigoProducto),
CONSTRAINT FK_InventariosProductos01 FOREIGN KEY (CodigoProducto) REFERENCES Productos (CodigoProducto)
)
GO


CREATE TABLE InventariosProductosCantidadesTransaccionesHistorial
(
NumeroAlmacen				INT				NOT NULL,
NumeroTransaccionProducto	INT				NOT NULL, --Esta columna hace referencia a cualquier transacción que sea un Compra a Inventarios (NumeroCompraProducto)
CodigoProducto				CHAR(15)		NOT NULL,
CodigoTipoTransaccion		CHAR(1)			NOT NULL CHECK(CodigoTipoTransaccion IN ('V','C','T')),-- 'V'->Venta, 'C'->Compra, 'T'->TRANSFERENCIA
FechaHoraCompra			DATETIME		NOT NULL,
CantidadExistente			INT				NOT NULL CHECK(CantidadExistente >= 0),	
PrecioUnitario				DECIMAL(10,2)	NOT NULL CHECK(PrecioUnitario >= 0)
CONSTRAINT PK_InVentarioProductosCantidadesTransaccionesHistorial PRIMARY KEY (NumeroAlmacen, NumeroTransaccionProducto, CodigoProducto, FechaHoraCompra)	
)
GO


CREATE TABLE Conceptos
(
	NumeroConcepto			INT				NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Concepto				VARCHAR(256)	NOT NULL
)
GO


CREATE TABLE CuentasPorPagar
(
	NumeroCuentaPorPagar	INT				IDENTITY(1,1) PRIMARY KEY,
	NumeroAlmacen			INT				NOT NULL,
	FechaHoraRegistro		DATETIME		NOT NULL,
	CodigoProveedor			INT				NULL,
	NumeroConcepto			INT				NULL,
	Monto					DECIMAL(10,2)	NOT NULL,
	FechaLimite				DATETIME		NULL,
	CodigoEstado			CHAR(1)			NOT NULL,
	Observaciones			TEXT			NULL,
	DIUsuario				CHAR(15)		NULL,
	FOREIGN KEY(NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen),
	FOREIGN KEY(NumeroConcepto) REFERENCES Conceptos(NumeroConcepto),
	FOREIGN KEY(CodigoProveedor) REFERENCES Proveedores(CodigoProveedor),
	CONSTRAINT CH_CodigoEstado CHECK(CodigoEstado IN ('P','D'))--'P'->PAGADO, 'D'->PENDIENTE
)
GO


CREATE TABLE CuentasPorPagarPagos
(	
	NumeroCuentaPorPagar	INT				NOT NULL,
	NumeroPago				INT				NOT NULL IDENTITY(1,1),
	FechaHoraPago			DATETIME		NOT NULL,
	Monto					DECIMAL(10,2)	NOT NULL,
	DIUsuario				CHAR(15)		NULL
	
	PRIMARY KEY(NumeroCuentaPorPagar, NumeroPago),
	FOREIGN KEY(NumeroCuentaPorPagar) REFERENCES CuentasPorPagar(NumeroCuentaPorPagar)
)
GO


CREATE TABLE CuentasPorCobrar
(
	NumeroCuentaPorCobrar	INT				IDENTITY(1,1) PRIMARY KEY,
	NumeroAlmacen			INT				NOT NULL,
	FechaHoraRegistro		DATETIME		NOT NULL,
	NumeroConcepto			INT				NULL,
	CodigoCliente			INT				NULL,
	Monto					DECIMAL(10,2)	NOT NULL,
	FechaLimite				DATETIME		NULL,
	CodigoEstado			CHAR(1)			NOT NULL,
	Observaciones			TEXT			NULL,
	DIUsuario				CHAR(15)		NULL,
	FOREIGN KEY(NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen),
	FOREIGN KEY(NumeroConcepto) REFERENCES Conceptos(NumeroConcepto),
	FOREIGN KEY(CodigoCliente) REFERENCES Clientes(CodigoCliente),
	CONSTRAINT CH_CodigoEstadoCuentasPorCobrar CHECK(CodigoEstado IN ('P','D'))--'P'->PAGADO, 'D'->PENDIENTE
)
GO


CREATE TABLE CuentasPorCobrarCobros
(	
	NumeroCuentaPorCobrar	INT				NOT NULL,
	NumeroCobro				INT				NOT NULL IDENTITY(1,1),
	FechaHoraCobro			DATETIME		NOT NULL,
	Monto					DECIMAL(10,2)	NOT NULL,
	DIUsuario				CHAR(15)		NULL,	
	PRIMARY KEY(NumeroCuentaPorCobrar, NumeroCobro),
	FOREIGN KEY(NumeroCuentaPorCobrar) REFERENCES CuentasPorCobrar(NumeroCuentaPorCobrar)
)
GO

CREATE TABLE ComprasProductos
(
NumeroAlmacen				INT				NOT NULL,
NumeroCompraProducto		INT				NOT NULL IDENTITY (1,1),
CodigoCompraProducto		CHAR(12)		NULL,
CodigoProveedor				INT				NOT NULL,
DIUsuario					CHAR(15)		NOT NULL,
FechaHoraRegistro			DATETIME		NOT NULL,
FechaHoraRecepcion			DATETIME		NULL,
CodigoTipoCompra			CHAR(1)			NOT NULL CHECK(CodigoTipoCompra IN ('E','R')), --E'-> Efectivo, 'R'->Credito
CodigoEstadoCompra			CHAR(1)			NOT NULL CHECK(CodigoEstadoCompra IN ('I', 'A', 'P', 'D', 'F','X' )), --I'->Iniciada, 'A'->Anulada, 'P'-> Pagada, 'D'->Pendiente, 'F'->Finalizada,'X' -> Finalizada pero Recepción incompleta 
NumeroFactura				VARCHAR(100)	NULL, 
NumeroComprobante			VARCHAR(100)	NULL, 
MontoTotalCompra			DECIMAL(10,2)	NOT NULL,
MontoTotalPagoEfectivo		DECIMAL(10,2)	NOT NULL,
NumeroCuentaPorPagar		INT				NULL,	 
Observaciones				TEXT			NULL,
CONSTRAINT PK_ComprasProductos PRIMARY KEY (NumeroAlmacen, NumeroCompraProducto),
CONSTRAINT FK_ComprasProductosAlmacenes			FOREIGN KEY (NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen),
CONSTRAINT FK_ComprasProductosProveedores		FOREIGN KEY (CodigoProveedor) REFERENCES Proveedores(CodigoProveedor),
CONSTRAINT FK_ComprasProductosUsuarios			FOREIGN KEY (DIUsuario) REFERENCES Usuarios(DIUsuario),
CONSTRAINT FK_ComprasProductosCuentasPorPagar	FOREIGN KEY (NumeroCuentaPorPagar) REFERENCES CuentasPorPagar(NumeroCuentaPorPagar)
--FOREIGN KEY (NumeroAlmacen,NumeroCuentaPorPagar) REFERENCES CuentasPorPagar(NumeroAlmacen, NumeroCuentaPorPagar)
)
GO


CREATE TABLE ComprasProductosDetalle
(
NumeroAlmacen				INT				NOT NULL,
NumeroCompraProducto		INT				NOT NULL,
CodigoProducto				CHAR(15)		NOT NULL,
CantidadCompra				INT				NOT NULL,
CantidadEntregada			INT				NOT NULL CHECK(CantidadEntregada >= 0),
PrecioUnitarioCompra		DECIMAL(10,2)	NOT NULL,
TiempoGarantiaCompra		INT				NULL,
CONSTRAINT PK_ComprasProductosDetalle PRIMARY KEY (NumeroAlmacen, NumeroCompraProducto,CodigoProducto),
CONSTRAINT FK_ComprasProductos FOREIGN KEY (NumeroAlmacen, NumeroCompraProducto) 
REFERENCES ComprasProductos(NumeroAlmacen, NumeroCompraProducto),
CONSTRAINT FK_ComprasProductosDetalleProductos FOREIGN KEY (CodigoProducto) REFERENCES Productos(CodigoProducto)
)
GO


CREATE TABLE ComprasProductosDetalleEntrega
(
NumeroAlmacen				INT				NOT NULL,
NumeroCompraProducto		INT				NOT NULL,
CodigoProducto				CHAR(15)		NOT NULL,
CantidadEntregada			INT				NOT NULL CHECK(CantidadEntregada > 0),
FechaHoraEntrega			DATETIME		NOT NULL
CONSTRAINT PK_ComprasProductosDetalleEntrega PRIMARY KEY (NumeroAlmacen, NumeroCompraProducto,CodigoProducto, FechaHoraEntrega),	
CONSTRAINT FK_ComprasProductosDetalle FOREIGN KEY (NumeroAlmacen, NumeroCompraProducto, CodigoProducto) 
REFERENCES ComprasProductosDetalle(NumeroAlmacen, NumeroCompraProducto, CodigoProducto)
)
GO

CREATE TABLE ComprasProductosDevoluciones
(
	NumeroAlmacenDevolucion				INT,
	NumeroCompraProductoDevolucion		INT				NOT NULL IDENTITY(1,1),
	CodigoDevolucionCompraProducto		CHAR(12)		NULL,	
	DIUsuario							CHAR(15)		NOT NULL,
	FechaHoraRegistro					DATETIME		NOT NULL,	
	CodigoEstadoCompraDevolucion		CHAR(1)			NOT NULL CHECK(CodigoEstadoCompraDevolucion IN ('I', 'A', 'F','X' )), --I'->Iniciada, 'A'->Anulada, 'P'-> Pagada, 'D'->Pendiente, 'F'->Finalizada,'X' -> Finalizada pero Recepción incompleta 
	MontoTotalCompraDevolucion			DECIMAL(10,2)	NOT NULL,
	MontoTotalPagoEfectivo				DECIMAL(10,2)	NOT NULL,
	NumeroCompraProducto				INT				NOT NULL,
	NumeroAlmacen						INT				NOT NULL,
	FechaHoraDevolucion					DATETIME		NULL,	
	Observaciones						TEXT			NULL,
	CONSTRAINT PK_ComprasProductosDevoluciones			PRIMARY KEY (NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion),
	CONSTRAINT FK_ComprasProductosDevolucionesAlmacenes	FOREIGN KEY (NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen),	
	CONSTRAINT FK_ComprasProductosDevolucionesUsuarios	FOREIGN KEY (DIUsuario) REFERENCES Usuarios(DIUsuario),
	CONSTRAINT FK_ComprasProductosDevoluciones			FOREIGN KEY (NumeroAlmacen, NumeroCompraProducto) REFERENCES ComprasProductos(NumeroAlmacen, NumeroCompraProducto)

)
GO

CREATE TABLE ComprasProductosDevolucionesDetalle
(
	NumeroAlmacenDevolucion				INT,
	NumeroCompraProductoDevolucion		INT,
	CodigoProducto						CHAR(15)		NOT NULL,
	CantidadCompraDevolucion			INT				NOT NULL CHECK(CantidadCompraDevolucion > 0),
	PrecioUnitarioDevolucion			DECIMAL(10,2)	NOT NULL,
	CONSTRAINT PK_ComprasProductosDevolucionesDetalle PRIMARY KEY (NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion,CodigoProducto),
	CONSTRAINT FK_ComprasProductosDevolucionesDetalle FOREIGN KEY (NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion) 
	REFERENCES ComprasProductosDevoluciones(NumeroAlmacenDevolucion, NumeroCompraProductoDevolucion),
	CONSTRAINT FK_ComprasProductosDevolucionesDetalleProductos FOREIGN KEY (CodigoProducto) REFERENCES Productos(CodigoProducto)
)
GO

--CREATE TABLE ComprasProductosEspecificos
--(
--NumeroAlmacen				INT				NOT NULL,
--NumeroCompraProducto		INT				NOT NULL,
--CodigoProducto				CHAR(15)		NOT NULL,
--CodigoProductoEspecifico	CHAR(30)		NOT NULL,
--TiempoGarantiaPE			INT				NULL,
--FechaHoraVencimientoPE		DATETIME		NULL,
--FechaHoraRecepcion			DATETIME		NOT NULL,
--CONSTRAINT PK_ComprasProductosEspecificos PRIMARY KEY (NumeroAlmacen, NumeroCompraProducto, CodigoProducto, CodigoProductoEspecifico),
--CONSTRAINT FK_ComprasProductosDetallePE FOREIGN KEY (NumeroAlmacen, NumeroCompraProducto, CodigoProducto) 
--REFERENCES ComprasProductosDetalle(NumeroAlmacen, NumeroCompraProducto, CodigoProducto)
--)
--GO



CREATE TABLE VentasProductos
(
NumeroAlmacen			INT				NOT NULL,
NumeroVentaProducto		INT				NOT NULL IDENTITY (1,1),
CodigoVentaProducto		CHAR(12)		NULL,
CodigoCliente			INT				NOT NULL,
DIUsuario				CHAR(15)		NOT NULL,
FechaHoraVenta			DATETIME		NOT NULL DEFAULT GETDATE(),
FechaHoraEntrega		DATETIME		NULL,
NumeroComprobante		VARCHAR(100)	NULL, 
CodigoEstadoVenta		CHAR(1)			NOT NULL CHECK(CodigoEstadoVenta IN ('I','P', 'F', 'A','T', 'C','E','D','X')), --I'->Iniciada, 'P'->Pagada, 'F'->Finalizada, 'A'->Anulada, 'T'->Venta a Insituticiones, 'C'->Entrega de Productos en Confianza, 'D'-> Distribucion [Pendiente (Venta Institucional)], 'E'->En Espera(Venta Normal)
CodigoTipoVenta			CHAR(1)			NOT NULL CHECK(CodigoTipoVenta IN ('E','R')),
CodigoMotivoVenta		CHAR(1)			NOT NULL DEFAULT 'N' CHECK(CodigoMotivoVenta IN ('N','D','P','V','O')),-- 'N'->NORMAL, 'D'->DAÑADOS, 'P'->PERDIDOS, 'V'->VENCIDOS, 'O'->OTROS
NumeroFactura			VARCHAR(100)	NULL, 
MontoTotalVenta			DECIMAL(10,2)	NOT NULL, --incluye los agregados en caso de que tengan precio
MontoTotalPagoEfectivo	DECIMAL(10,2)	NOT NULL,
DIPersonaDistribuidor	CHAR(15)		NULL,
VentaParaDistribuir		BIT				NULL DEFAULT 0, --si la Venta debe ser distribuida a lo clientes en su puesto
CodigoMovilidad			VARCHAR(10)		NULL,
MontoTotalDescuento		DECIMAL(5,2)	NULL,
NumeroCuentaPorCobrar	INTEGER			NULL,
EsVentaDistribuible		BIT				DEFAULT 0,
Observaciones			TEXT			NULL,
CONSTRAINT PK_VentasProductos				PRIMARY KEY (NumeroAlmacen, NumeroVentaProducto),
CONSTRAINT FK_VentasProductosAlmacenes		FOREIGN KEY (NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen),
CONSTRAINT FK_VentasProductosFuncionarios	FOREIGN KEY (CodigoCliente) REFERENCES Clientes(CodigoCliente),
CONSTRAINT FK_VentasProductosUsuarios		FOREIGN KEY (DIUsuario) REFERENCES Usuarios(DIUsuario),
CONSTRAINT FK_VentasProductoCuentasPorCobrar	FOREIGN KEY(NumeroCuentaPorCobrar) REFERENCES CuentasPorCobrar(NumeroCuentaPorCobrar),
CONSTRAINT FK_VentasProductoPersonas		FOREIGN KEY (DIPersonaDistribuidor) REFERENCES Personas(DIPersona),
CONSTRAINT FK_VentasProductosMovilidades	FOREIGN KEY (CodigoMovilidad) REFERENCES Movilidades(CodigoMovilidad)
)
GO

CREATE TABLE VentasProductosDetalle
(
NumeroAlmacen			INT				NOT NULL,
NumeroVentaProducto		INT				NOT NULL,
CodigoProducto			CHAR(15)		NOT NULL,
CantidadVenta			INT				NOT NULL DEFAULT 1,
CantidadEntregada		INT				NOT NULL DEFAULT 0,
PrecioUnitarioVenta		DECIMAL(10,2)	NOT NULL,
PorcentajeDescuento		DECIMAL(5,2)	NULL,
TiempoGarantiaVenta		INT				DEFAULT 0,
CONSTRAINT PK_VentasProductosDetalle PRIMARY KEY(NumeroAlmacen, NumeroVentaProducto, CodigoProducto),
CONSTRAINT FK_VPDProductos FOREIGN KEY(CodigoProducto) REFERENCES Productos(CodigoProducto),
CONSTRAINT FK_VPDVentasProductos FOREIGN KEY (NumeroAlmacen, NumeroVentaProducto)
REFERENCES VentasProductos(NumeroAlmacen, NumeroVentaProducto)
)
GO


CREATE TABLE VentasProductosDetalleEntrega
(
NumeroAlmacen					INT				NOT NULL,
NumeroVentaProducto				INT				NOT NULL,
CodigoProducto					CHAR(15)		NOT NULL,
FechaHoraEntrega				DATETIME		NOT NULL DEFAULT GETDATE(),
FechaHoraCompraInventario		DATETIME		DEFAULT GETDATE(),
CantidadEntregada				INT				NOT NULL DEFAULT 0,
PrecioUnitarioCompraInventario	DECIMAL(10,2)	NULL DEFAULT 0,
CONSTRAINT PK_VentasProductosDetalleEntrega PRIMARY KEY(NumeroAlmacen, NumeroVentaProducto, CodigoProducto, FechaHoraEntrega, FechaHoraCompraInventario),
CONSTRAINT FK_VentasProductosDetalle FOREIGN KEY (NumeroAlmacen, NumeroVentaProducto, CodigoProducto)
REFERENCES VentasProductosDetalle(NumeroAlmacen, NumeroVentaProducto, CodigoProducto)
--CONSTRAINT FK_Productos FOREIGN KEY (CodigoProducto) REFERENCES Productos(CodigoProducto)
)
GO


CREATE TABLE VentasProductosDevoluciones
(
	NumeroAlmacenDevolucion				INT,
	NumeroVentaProductoDevolucion		INT				NOT NULL IDENTITY(1,1),
	CodigoDevolucionVentaProducto		CHAR(12)		NULL,	
	DIUsuario							CHAR(15)		NOT NULL,
	FechaHoraRegistro					DATETIME		NOT NULL,	
	CodigoEstadoVentaDevolucion			CHAR(1)			NOT NULL CHECK(CodigoEstadoVentaDevolucion IN ('I', 'A', 'F','X' )), --I'->Iniciada, 'A'->Anulada, 'P'-> Pagada, 'D'->Pendiente, 'F'->Finalizada,'X' -> Finalizada pero Recepción incompleta 
	MontoTotalVentaDevolucion			DECIMAL(10,2)	NOT NULL,
	MontoTotalPagoEfectivo				DECIMAL(10,2)	NOT NULL,
	NumeroVentaProducto					INT				NOT NULL,
	NumeroAlmacen						INT				NOT NULL,
	FechaHoraDevolucion					DATETIME		NULL,	
	Observaciones						TEXT			NULL,
	CONSTRAINT PK_VentasProductosDevoluciones			PRIMARY KEY (NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion),
	CONSTRAINT FK_VentasProductosDevolucionesAlmacenes	FOREIGN KEY (NumeroAlmacen) REFERENCES Almacenes(NumeroAlmacen),	
	CONSTRAINT FK_VentasProductosDevolucionesUsuarios	FOREIGN KEY (DIUsuario) REFERENCES Usuarios(DIUsuario),
	CONSTRAINT FK_VentasProductosDevoluciones			FOREIGN KEY (NumeroAlmacen, NumeroVentaProducto) REFERENCES VentasProductos(NumeroAlmacen, NumeroVentaProducto)

)
GO

CREATE TABLE VentasProductosDevolucionesDetalle
(
	NumeroAlmacenDevolucion			INT,
	NumeroVentaProductoDevolucion	INT,
	CodigoProducto					CHAR(15)		NOT NULL,
	CantidadVentaDevolucion			INT				NOT NULL CHECK(CantidadVentaDevolucion > 0),
	PrecioUnitarioDevolucion		DECIMAL(10,2)	NOT NULL,
	CONSTRAINT PK_VentasProductosDevolucionesDetalle PRIMARY KEY (NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion,CodigoProducto),
	CONSTRAINT FK_VentasProductosDevolucionesDetalle FOREIGN KEY (NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion) 
	REFERENCES VentasProductosDevoluciones(NumeroAlmacenDevolucion, NumeroVentaProductoDevolucion),
	CONSTRAINT FK_VentasProductosDevolucionesDetalleProductos FOREIGN KEY (CodigoProducto) REFERENCES Productos(CodigoProducto)
)
GO


CREATE TABLE TransferenciasProductos
(
NumeroAlmacenEmisor				INT				NOT NULL,
NumeroTransferenciaProducto		INT				NOT NULL IDENTITY (1,1),
NumeroAlmacenRecepctor			INT				NOT NULL,
DIUsuario						CHAR(15)		NOT NULL,
FechaHoraTransferencia			DATETIME		NOT NULL DEFAULT GETDATE(),
CodigoEstadoTransferencia		CHAR(1)			NOT NULL,
MontoTotalTransferencia			DECIMAL(10,2)	NOT NULL, --incluye los Gastos de transferencia
Observaciones					TEXT			NULL,
CONSTRAINT PK_TransferenciasProductos PRIMARY KEY (NumeroAlmacenEmisor, NumeroTransferenciaProducto),
CONSTRAINT FK_TransferenciasProductos_Almacens01 FOREIGN KEY (NumeroAlmacenEmisor) REFERENCES Almacenes(NumeroAlmacen),
CONSTRAINT FK_TransferenciasProductos_Almacens02 FOREIGN KEY (NumeroAlmacenRecepctor) REFERENCES Almacenes(NumeroAlmacen),
CONSTRAINT FK_TransferenciasProductos_Usuarios FOREIGN KEY (DIUsuario) REFERENCES Usuarios(DIUsuario),
CONSTRAINT U_CodigoEstadoTransferencia  CHECK(CodigoEstadoTransferencia IN ('I','E', 'P','A', 'D', 'F', 'X')) --I'->Iniciada,'E' Enviada y Emitida,  'P'->Pagada y Confirmada su Envio, 'A'->Anulada, 'D'->Pendiente (Recepción por partes), 'F'->Finalizada, 'X'->Envio o Receipcion Incompleta 

)
GO

CREATE TABLE TransferenciasProductosDetalle
(
NumeroAlmacenEmisor					INT				NOT NULL,
NumeroTransferenciaProducto			INT				NOT NULL,
CodigoProducto						CHAR(15)		NOT NULL REFERENCES Productos(CodigoProducto),
CantidadTransferencia				INT				NOT NULL DEFAULT 0,
PrecioUnitarioTransferencia			DECIMAL(10,2)	NOT NULL,
CONSTRAINT PK_TransferenciasProductosDetalle PRIMARY KEY(NumeroAlmacenEmisor, NumeroTransferenciaProducto, CodigoProducto),
CONSTRAINT FK_TransferenciasProductosDetalle01 FOREIGN KEY (NumeroAlmacenEmisor, NumeroTransferenciaProducto)
REFERENCES TransferenciasProductos(NumeroAlmacenEmisor, NumeroTransferenciaProducto)
)
GO

--CREATE TABLE VentasProductosEspecificos
--(
--NumeroAlmacen					INT				NOT NULL,
--NumeroVentaProducto			INT				NOT NULL,
--CodigoProducto					CHAR(15)		NOT NULL,
--CodigoProductoEspecifico		CHAR(30)		NOT NULL,
--TiempoGarantiaPE				INT				NOT NULL,
--Entregado						BIT				NULL DEFAULT 1,
--FechaHoraEntrega				DATETIME		NULL,
--CONSTRAINT PK_VentasProductosEspecificos	PRIMARY KEY(NumeroAlmacen, NumeroVentaProducto,CodigoProducto,CodigoProductoEspecifico),
--CONSTRAINT FK_VentasProductosDetallePE		FOREIGN KEY (NumeroAlmacen, NumeroVentaProducto, CodigoProducto) 
--REFERENCES VentasProductosDetalle(NumeroAlmacen, NumeroVentaProducto, CodigoProducto),
--CONSTRAINT FK_InventariosProductosEspecificos FOREIGN KEY (NumeroAlmacen, CodigoProducto,CodigoProductoEspecifico) 
--REFERENCES InventariosProductosEspecificos(NumeroAlmacen, CodigoProducto,CodigoProductoEspecifico)
--)
--GO


--select CURRENT_USER