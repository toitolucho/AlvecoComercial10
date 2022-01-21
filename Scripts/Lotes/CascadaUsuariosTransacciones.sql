BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComprasProductos
	DROP CONSTRAINT FK_ComprasProductosUsuarios
GO
ALTER TABLE dbo.ComprasProductosDevoluciones
	DROP CONSTRAINT FK_ComprasProductosDevolucionesUsuarios
GO
ALTER TABLE dbo.VentasProductos
	DROP CONSTRAINT FK_VentasProductosUsuarios
GO
ALTER TABLE dbo.VentasProductosDevoluciones
	DROP CONSTRAINT FK_VentasProductosDevolucionesUsuarios
GO
ALTER TABLE dbo.Usuarios SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.VentasProductosDevoluciones ADD CONSTRAINT
	FK_VentasProductosDevolucionesUsuarios FOREIGN KEY
	(
	DIUsuario
	) REFERENCES dbo.Usuarios
	(
	DIUsuario
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.VentasProductosDevoluciones SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.VentasProductos ADD CONSTRAINT
	FK_VentasProductosUsuarios FOREIGN KEY
	(
	DIUsuario
	) REFERENCES dbo.Usuarios
	(
	DIUsuario
	) ON UPDATE  CASCADE 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.VentasProductos SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComprasProductosDevoluciones ADD CONSTRAINT
	FK_ComprasProductosDevolucionesUsuarios FOREIGN KEY
	(
	DIUsuario
	) REFERENCES dbo.Usuarios
	(
	DIUsuario
	) ON UPDATE  CASCADE 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComprasProductosDevoluciones SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComprasProductos ADD CONSTRAINT
	FK_ComprasProductosUsuarios FOREIGN KEY
	(
	DIUsuario
	) REFERENCES dbo.Usuarios
	(
	DIUsuario
	) ON UPDATE  CASCADE 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComprasProductos SET (LOCK_ESCALATION = TABLE)
GO
COMMIT