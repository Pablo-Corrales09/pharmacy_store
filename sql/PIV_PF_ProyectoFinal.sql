CREATE DATABASE PIV_PF_ProyectoFinal
GO

USE PIV_PF_ProyectoFinal
GO



-- Tabla "usuario" con validaciones de correo, tipo y estado.
-- Las constraints tienen nombres descriptivos para facilitar mantenimiento y depuración de errores.
CREATE Table usuario (
	id_usuario INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
	cedula VARCHAR(200) NOT NULL UNIQUE,
	nombre_completo NVARCHAR(200) NOT NULL,
	correo_electronico VARCHAR(200) NOT NULL UNIQUE,
	CONSTRAINT check_correoUsuario CHECK
	( 
	correo_electronico LIKE '%_@__%.__%'
	AND correo_electronico NOT LIKE'%@%@%'
	AND correo_electronico NOT LIKE '%..%'
	AND correo_electronico NOT LIKE '%.@%'
	AND correo_electronico NOT LIKE '%@.%'
	),
	tipo_usuario VARCHAR(20) NOT NULL, 
	CONSTRAINT check_tipoUsuario CHECK (tipo_usuario IN ('Administrador','Vendedor','Contabilidad')),
	estado VARCHAR(20) NOT NULL DEFAULT 'Activo', 
	CONSTRAINT check_estadoUsuario CHECK (estado IN('Activo', 'Inactivo')) 
);
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Crreación de a taba para el manejo de credenciales para el Login

CREATE TABLE UsuarioRegistrado(
	[id_usuarioRegistrado] [int] IDENTITY(1,1) NOT NULL,
	[user] [varchar](150) NOT NULL,
	[password] [varchar](150) NOT NULL,
	[rol] [varchar](150),
	[id_usuario] [int] NOT NULL,

    CONSTRAINT [PK_UsuarioRegistrado] PRIMARY KEY CLUSTERED 
    (
        [id_usuarioRegistrado] ASC
    ),

    CONSTRAINT [UQ_UsuarioRegistrado_id_usuario] UNIQUE ([id_usuario]),
    CONSTRAINT [FK_UsuarioRegistrado_Usuario] FOREIGN KEY ([id_usuario])
    REFERENCES usuario (id_usuario)
    ON DELETE CASCADE
) ON [PRIMARY]
GO


SET IDENTITY_INSERT [dbo].[UsuarioRegistrado] ON 
INSERT [dbo].[UsuarioRegistrado] ([id_usuarioRegistrado], [user], [password], [rol], [id_usuario]) VALUES (1, N'PCORRALES', N'ADMIN123', N'ADMINISTRADOR', 1)
SET IDENTITY_INSERT [dbo].[usuarioRegistrado] OFF
GO

SELECT * FROM UsuarioRegistrado;
GO


-- Tabla "cliente" con validación en el tipo de correo.
-- La constraint tiene nombres descriptivo para facilitar mantenimiento y depuración de errores.
CREATE Table cliente(
	id_cliente INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
	cedula VARCHAR(200) NOT NULL UNIQUE,
	nombre_completo VARCHAR(250) NOT NULL,
	correo_electronico VARCHAR(200) NOT NULL UNIQUE,
	CONSTRAINT check_correoCliente CHECK
	( 
	correo_electronico LIKE '%_@__%.__%'
	AND correo_electronico NOT LIKE'%@%@%'
	AND correo_electronico NOT LIKE '%..%'
	AND correo_electronico NOT LIKE '%.@%'
	AND correo_electronico NOT LIKE '%@.%'
	)
);
GO

--Tabla tipoProducto, continene información relacionable con cualquier producto.
CREATE TABLE tipoProducto(
	id_tipoProducto INT IDENTITY(1,1) PRIMARY KEY,
	codigo_tipo VARCHAR(200) NOT NULL UNIQUE,
	descripcion NVARCHAR(250) NOT NULL
);
GO


--Creación de la tabla producto relacionada con el tipo de producto y con las validaciones necesarias para asegurar que el ingreso de los datos sea correcto.
CREATE TABLE producto(
	id_producto INT IDENTITY (1,1) PRIMARY KEY,
	codigo_producto VARCHAR(200) NOT NULL UNIQUE,
	descripcion NVARCHAR(250) NOT NULL,
	precio DECIMAL(10,2) NOT NULL CHECK (precio>=0),
	estado VARCHAR(20) NOT NULL DEFAULT 'En existencia',
	CONSTRAINT check_estadoProducto CHECK(estado IN ('Agotado','En existencia')),
	id_tipoProducto INT NOT NULL,
	CONSTRAINT id_tipoProducto FOREIGN KEY (id_tipoProducto) REFERENCES tipoProducto(id_tipoProducto)
);
GO

-- Tabla "factura": registra las compras realizadas por los clientes,
-- incluyendo fecha, método de pago, subtotal y referencia al cliente.
CREATE TABLE factura(
	id_factura INT IDENTITY(1,1) PRIMARY KEY,
	codigo_factura VARCHAR(200) UNIQUE NOT NULL,
	fecha_compra DATETIME NOT NULL,
	id_cliente INT NOT NULL,
	metodo_pago VARCHAR(20) NOT NULL, 
	CONSTRAINT check_metodoPagoFactura CHECK(metodo_pago IN ('Efectivo','Tarjeta')) ,
	sub_total DECIMAL(10,2) NOT NULL,
	CONSTRAINT check_subTotalFactura CHECK (sub_total>=0),
	CONSTRAINT fk_factura_cliente FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente)
);
GO


CREATE TABLE detalleFactura(
	num_consecutivo INT IDENTITY(1,1) PRIMARY KEY,
	id_factura INT NOT NULL,
	codigo_producto VARCHAR(200) NOT NULL,
	cantidad INT NOT NULL,
	CONSTRAINT check_cantidadDetalleFactura CHECK (cantidad>=0),
	valor_unitario DECIMAL(10,2) NOT NULL,
	CONSTRAINT check_valorUnitarioDetalleFactura CHECK(valor_unitario>=0),
	total_linea DECIMAL(10,2) NOT NULL,
	CONSTRAINT check_totalLineaDetalleFactura CHECK(total_linea>=0),
	CONSTRAINT fk_idFacturaDetalleFactura FOREIGN KEY(id_factura) REFERENCES factura(id_factura),
	CONSTRAINT fk_codigoProductoDetalleFactura FOREIGN KEY(codigo_producto) REFERENCES producto(codigo_producto)
);
GO

