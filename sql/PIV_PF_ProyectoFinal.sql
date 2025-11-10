CREATE DATABASE PIV_PF_ProyectoFinal
GO

USE PIV_PF_ProyectoFinal
GO



-- Tabla "usuario" con validaciones de correo, tipo y estado.
-- Las constraints tienen nombres descriptivos para facilitar mantenimiento y depuración de errores.
CREATE Table usuario (
	id_usuario VARCHAR(200) PRIMARY KEY,
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

-- Tabla "cliente" con validación en el tipo de correo.
-- La constraint tiene nombres descriptivo para facilitar mantenimiento y depuración de errores.
CREATE Table cliente(
	id_cliente VARCHAR(200) PRIMARY KEY,
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
	codigo_tipo VARCHAR(200) PRIMARY KEY,
	descripcion NVARCHAR(250) NOT NULL
);
GO


--Creación de la tabla producto relacionada con el tipo de producto y con las validaciones necesarias para asegurar que el ingreso de los datos sea correcto.
CREATE TABLE producto(
	codigo_producto VARCHAR(200) PRIMARY KEY,
	descripcion NVARCHAR(250) NOT NULL,
	precio DECIMAL(10,2) NOT NULL CHECK (precio>=0),
	estado VARCHAR(20) NOT NULL DEFAULT 'En existencia',
	CONSTRAINT check_estadoProducto CHECK(estado IN ('Agotado','En existencia')),
	codigo_tipo VARCHAR(200) NOT NULL,
	CONSTRAINT fk_codigotipo FOREIGN KEY (codigo_tipo) REFERENCES tipoProducto(codigo_tipo)
);
GO

-- Tabla "factura": registra las compras realizadas por los clientes,
-- incluyendo fecha, método de pago, subtotal y referencia al cliente.
CREATE TABLE factura(
	codigo_factura VARCHAR(200) PRIMARY KEY,
	fecha_compra DATETIME NOT NULL,
	id_cliente VARCHAR(200) NOT NULL,
	metodo_pago VARCHAR(20) NOT NULL, 
	CONSTRAINT check_metodoPagoFactura CHECK(metodo_pago IN ('Efectivo','Tarjeta')) ,
	sub_total DECIMAL(10,2) NOT NULL,
	CONSTRAINT check_subTotalFactura CHECK (sub_total>=0),
	CONSTRAINT fk_factura_cliente FOREIGN KEY (id_cliente) REFERENCES cliente(id_cliente)
);
GO


CREATE TABLE detalleFactura(
	num_consecutivo INT IDENTITY(1,1) PRIMARY KEY,
	codigo_factura VARCHAR(200) NOT NULL,
	codigo_producto VARCHAR(200) NOT NULL,
	cantidad INT NOT NULL,
	CONSTRAINT check_cantidadDetalleFactura CHECK (cantidad>=0),
	valor_unitario DECIMAL(10,2) NOT NULL,
	CONSTRAINT check_valorUnitarioDetalleFactura CHECK(valor_unitario>=0),
	total_linea DECIMAL(10,2) NOT NULL,
	CONSTRAINT check_totalLineaDetalleFactura CHECK(total_linea>=0),
	CONSTRAINT fk_codigoFacturaDetalleFactura FOREIGN KEY(codigo_factura) REFERENCES factura(codigo_factura),
	CONSTRAINT fk_codigoProductoDetalleFactura FOREIGN KEY(codigo_producto) REFERENCES producto(codigo_producto)
);
GO

