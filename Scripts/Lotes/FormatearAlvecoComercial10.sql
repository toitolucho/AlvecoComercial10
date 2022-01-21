--------------------------------------------------------------------------------------
--Valores iniciales
USE AlvecoComercial10
GO

DELETE FROM dbo.TransferenciasProductosDetalle
DELETE FROM dbo.TransferenciasProductos
DBCC checkident ('AlvecoComercial10.dbo.TransferenciasProductos', reseed, 0) 


DELETE FROM dbo.VentasProductosDevolucionesDetalle
DELETE FROM dbo.VentasProductosDevoluciones
DELETE FROM dbo.VentasProductosDetalleEntrega
DELETE FROM dbo.VentasProductosDetalle
DELETE FROM dbo.VentasProductos
DBCC checkident ('AlvecoComercial10.dbo.VentasProductos', reseed, 0) 
DBCC checkident ('AlvecoComercial10.dbo.VentasProductosDevoluciones', reseed, 0) 

DELETE FROM CuentasPorCobrarCobros
DELETE FROM CuentasPorCobrar
DBCC checkident ('AlvecoComercial10.dbo.CuentasPorCobrar', reseed, 0) 



DELETE FROM Clientes
DBCC checkident ('AlvecoComercial10.dbo.Clientes', reseed, 0) 


DELETE FROM dbo.ComprasProductosDevolucionesDetalle
DELETE FROM dbo.ComprasProductosDevoluciones
DELETE FROM dbo.ComprasProductosDetalleEntrega
DELETE FROM dbo.ComprasProductosDetalle
DELETE FROM dbo.ComprasProductos
DBCC checkident ('AlvecoComercial10.dbo.ComprasProductos', reseed, 0) 
DBCC checkident ('AlvecoComercial10.dbo.ComprasProductosDevoluciones', reseed, 0) 

DELETE FROM CuentasPorPagarPagos
DELETE FROM CuentasPorPagar
DBCC checkident ('AlvecoComercial10.dbo.CuentasPorPagar', reseed, 0) 

DBCC checkident ('AlvecoComercial10.dbo.Almacenes', reseed, 1) 

DELETE FROM Proveedores
DBCC checkident ('AlvecoComercial10.dbo.Proveedores', reseed, 0) 

DELETE FROM InventariosProductosCantidadesTransaccionesHistorial
UPDATE dbo.InventariosProductos
	SET CantidadExistencia = 0,
		CantidadRequerida = 1,
		StockMinimo = 1,
		PrecioValoradoTotal = 0,
		PrecioUnitarioCompra = 0,
		PrecioUnitarioVentaPorMayor = 0,
		PrecioUnitarioVentaPorMenor = 0,
		FechaHoraIngresoInventario = GETDATE()

--DELETE FROM LUGARES
--DELETE FROM PROVINCIAS
--DELETE FROM DEPARTAMENTOS
--DELETE FROM PAISES

--DELETE FROM dbo.InventariosProductos
--DELETE FROM PRODUCTOS


DELETE FROM Conceptos
DBCC checkident ('AlvecoComercial10.dbo.Conceptos', reseed, 0) 

INSERT INTO Conceptos(Concepto) VALUES ('COMPRA PRODUCTOS')
INSERT INTO Conceptos(Concepto) VALUES ('VENTA PRODUCTOS')



--SELECT * FROM Conceptos


----------------------------------------------------------------
-- HABILITAR EN CASO DE QUERER BORRAR TODOS LOS PRODUCTOS Y SUS DEPENDENCIAS
-------------------------------------------------------------------
--DELETE FROM InventariosProductosCantidadesTransaccionesHistorial
--DELETE FROM InventariosProductos


--DELETE FROM Productos

--DELETE FROM Movilidades


--DELETE FROM ProductosUnidades
--DBCC checkident ('AlvecoComercial10.dbo.ProductosUnidades', reseed, 0) 

--DELETE FROM dbo.MovilidadesModelos
--DBCC checkident ('AlvecoComercial10.dbo.MovilidadesModelos', reseed, 0) 


--DELETE FROM ProductosMarcas
--DBCC checkident ('AlvecoComercial10.dbo.ProductosMarcas', reseed, 0) 

--DELETE FROM ProductosTIPOS

-------------------------------------------------------------------------





--SELECT * FROM Personas
--SELECT * FROM Usuarios

--DELETE FROM Usuarios 
--WHERE DIUsuario NOT IN('0000000000', '5058785')

--DELETE FROM Personas
--WHERE DIPersona NOT IN('0000000000', '5058785')
DELETE FROM BITACORA
