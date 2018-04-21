-- MySqlBackup.NET 2.0.9.2
-- Dump Time: 2017-12-05 05:52:32
-- --------------------------------------
-- Server version 5.7.19-log MySQL Community Server (GPL)


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- 
-- Definition of agregados
-- 

DROP TABLE IF EXISTS `agregados`;
CREATE TABLE IF NOT EXISTS `agregados` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(25) DEFAULT NULL,
  `cobro_extra` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table agregados
-- 

/*!40000 ALTER TABLE `agregados` DISABLE KEYS */;
INSERT INTO `agregados`(`id`,`nombre`,`cobro_extra`) VALUES
(1,'ARROZ',0),
(2,'PURE',0),
(3,'PAPAS FRITAS',500),
(4,'ENSALADA',0);
/*!40000 ALTER TABLE `agregados` ENABLE KEYS */;

-- 
-- Definition of bodega_movimiento
-- 

DROP TABLE IF EXISTS `bodega_movimiento`;
CREATE TABLE IF NOT EXISTS `bodega_movimiento` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  `jornada_id` int(11) DEFAULT NULL,
  `entrada` decimal(12,2) DEFAULT NULL,
  `salida` decimal(12,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  KEY `jornada_id` (`jornada_id`),
  CONSTRAINT `bodega_movimiento_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `bodega_movimiento_ibfk_2` FOREIGN KEY (`jornada_id`) REFERENCES `jornadas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table bodega_movimiento
-- 

/*!40000 ALTER TABLE `bodega_movimiento` DISABLE KEYS */;

/*!40000 ALTER TABLE `bodega_movimiento` ENABLE KEYS */;

-- 
-- Definition of bodega_stock
-- 

DROP TABLE IF EXISTS `bodega_stock`;
CREATE TABLE IF NOT EXISTS `bodega_stock` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `stock` decimal(12,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `bodega_stock_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table bodega_stock
-- 

/*!40000 ALTER TABLE `bodega_stock` DISABLE KEYS */;

/*!40000 ALTER TABLE `bodega_stock` ENABLE KEYS */;

-- 
-- Definition of boletas
-- 

DROP TABLE IF EXISTS `boletas`;
CREATE TABLE IF NOT EXISTS `boletas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `numero_boleta` int(11) DEFAULT NULL,
  `fecha` datetime NOT NULL,
  `puntos_cantidad` int(11) DEFAULT NULL,
  `total` int(11) DEFAULT NULL,
  `cliente_id` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `boletas_ibfk_1` (`cliente_id`),
  KEY `boletas_ibfk_2` (`usuario_id`),
  CONSTRAINT `boletas_ibfk_1` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `boletas_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table boletas
-- 

/*!40000 ALTER TABLE `boletas` DISABLE KEYS */;

/*!40000 ALTER TABLE `boletas` ENABLE KEYS */;

-- 
-- Definition of cantidadesvendidas
-- 

DROP TABLE IF EXISTS `cantidadesvendidas`;
CREATE TABLE IF NOT EXISTS `cantidadesvendidas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `itemventa` varchar(30) NOT NULL,
  `cantidad` decimal(9,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table cantidadesvendidas
-- 

/*!40000 ALTER TABLE `cantidadesvendidas` DISABLE KEYS */;

/*!40000 ALTER TABLE `cantidadesvendidas` ENABLE KEYS */;

-- 
-- Definition of cantidadesvendidas_jornada
-- 

DROP TABLE IF EXISTS `cantidadesvendidas_jornada`;
CREATE TABLE IF NOT EXISTS `cantidadesvendidas_jornada` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cantidadesvendidas_id` int(11) DEFAULT NULL,
  `jornada_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `cantidadesvendidas_id` (`cantidadesvendidas_id`),
  KEY `jornada_id` (`jornada_id`),
  CONSTRAINT `cantidadesvendidas_jornada_ibfk_1` FOREIGN KEY (`cantidadesvendidas_id`) REFERENCES `cantidadesvendidas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `cantidadesvendidas_jornada_ibfk_2` FOREIGN KEY (`jornada_id`) REFERENCES `jornadas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table cantidadesvendidas_jornada
-- 

/*!40000 ALTER TABLE `cantidadesvendidas_jornada` DISABLE KEYS */;

/*!40000 ALTER TABLE `cantidadesvendidas_jornada` ENABLE KEYS */;

-- 
-- Definition of categoria_sector
-- 

DROP TABLE IF EXISTS `categoria_sector`;
CREATE TABLE IF NOT EXISTS `categoria_sector` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `categoria_id` int(11) DEFAULT NULL,
  `sector_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `categoria_sector_ibfk_1` (`categoria_id`),
  KEY `categoria_sector_ibfk_2` (`sector_id`),
  CONSTRAINT `categoria_sector_ibfk_1` FOREIGN KEY (`categoria_id`) REFERENCES `categorias` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `categoria_sector_ibfk_2` FOREIGN KEY (`sector_id`) REFERENCES `sectores` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table categoria_sector
-- 

/*!40000 ALTER TABLE `categoria_sector` DISABLE KEYS */;

/*!40000 ALTER TABLE `categoria_sector` ENABLE KEYS */;

-- 
-- Definition of categoria_subcategoria
-- 

DROP TABLE IF EXISTS `categoria_subcategoria`;
CREATE TABLE IF NOT EXISTS `categoria_subcategoria` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `categoria_id` int(11) DEFAULT NULL,
  `subcategoria_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `categoria_subcategoria_ibfk_1` (`categoria_id`),
  KEY `categoria_subcategoria_ibfk_2` (`subcategoria_id`),
  CONSTRAINT `categoria_subcategoria_ibfk_1` FOREIGN KEY (`categoria_id`) REFERENCES `categorias` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `categoria_subcategoria_ibfk_2` FOREIGN KEY (`subcategoria_id`) REFERENCES `subcategorias` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table categoria_subcategoria
-- 

/*!40000 ALTER TABLE `categoria_subcategoria` DISABLE KEYS */;

/*!40000 ALTER TABLE `categoria_subcategoria` ENABLE KEYS */;

-- 
-- Definition of categorias
-- 

DROP TABLE IF EXISTS `categorias`;
CREATE TABLE IF NOT EXISTS `categorias` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `seleccionable` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `nombre_UNIQUE` (`nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table categorias
-- 

/*!40000 ALTER TABLE `categorias` DISABLE KEYS */;

/*!40000 ALTER TABLE `categorias` ENABLE KEYS */;

-- 
-- Definition of clientes
-- 

DROP TABLE IF EXISTS `clientes`;
CREATE TABLE IF NOT EXISTS `clientes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `rut` varchar(12) DEFAULT NULL,
  `nombre` varchar(50) DEFAULT NULL,
  `pass` varchar(30) NOT NULL DEFAULT '123',
  `contacto` varchar(20) DEFAULT NULL,
  `favorito` tinyint(1) DEFAULT NULL,
  `puntos_id` int(11) NOT NULL,
  `comentario` varchar(200) DEFAULT NULL,
  `imagen` varchar(1024) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `puntos_id` (`puntos_id`),
  CONSTRAINT `clientes_ibfk_1` FOREIGN KEY (`puntos_id`) REFERENCES `puntos` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table clientes
-- 

/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;

/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;

-- 
-- Definition of compras
-- 

DROP TABLE IF EXISTS `compras`;
CREATE TABLE IF NOT EXISTS `compras` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` datetime DEFAULT NULL,
  `usuario_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `usuario_id` (`usuario_id`),
  CONSTRAINT `compras_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table compras
-- 

/*!40000 ALTER TABLE `compras` DISABLE KEYS */;

/*!40000 ALTER TABLE `compras` ENABLE KEYS */;

-- 
-- Definition of configs
-- 

DROP TABLE IF EXISTS `configs`;
CREATE TABLE IF NOT EXISTS `configs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `font_size` char(1) DEFAULT NULL,
  `theme` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table configs
-- 

/*!40000 ALTER TABLE `configs` DISABLE KEYS */;
INSERT INTO `configs`(`id`,`font_size`,`theme`) VALUES
(1,'m','dark');
/*!40000 ALTER TABLE `configs` ENABLE KEYS */;

-- 
-- Definition of datos_negocio
-- 

DROP TABLE IF EXISTS `datos_negocio`;
CREATE TABLE IF NOT EXISTS `datos_negocio` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `mensaje` varchar(50) DEFAULT NULL,
  `direccion` varchar(30) DEFAULT NULL,
  `logo` varchar(2048) DEFAULT NULL,
  `iva` int(11) DEFAULT NULL,
  `inicio_jornada_dia` char(1) DEFAULT NULL,
  `termino_jornada_dia` char(1) DEFAULT NULL,
  `inicio_jornada_hora` time DEFAULT NULL,
  `termino_jornada_hora` time DEFAULT NULL,
  `modo` varchar(25) DEFAULT NULL,
  `correo_primario` varchar(45) DEFAULT NULL,
  `correo_secundario` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table datos_negocio
-- 

/*!40000 ALTER TABLE `datos_negocio` DISABLE KEYS */;
INSERT INTO `datos_negocio`(`id`,`nombre`,`mensaje`,`direccion`,`logo`,`iva`,`inicio_jornada_dia`,`termino_jornada_dia`,`inicio_jornada_hora`,`termino_jornada_hora`,`modo`,`correo_primario`,`correo_secundario`) VALUES
(1,'BUENOS AIRES RESTAURANT','','','',19,'A','B','12:00:00','05:00:00','RESTAURANT','crsk@mail.com','');
/*!40000 ALTER TABLE `datos_negocio` ENABLE KEYS */;

-- 
-- Definition of detalle_boleta
-- 

DROP TABLE IF EXISTS `detalle_boleta`;
CREATE TABLE IF NOT EXISTS `detalle_boleta` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `monto` int(11) DEFAULT NULL,
  `cantidad` decimal(10,0) NOT NULL,
  `descuento` decimal(10,0) NOT NULL DEFAULT '0',
  `producto_id` int(11) DEFAULT NULL,
  `boleta_id` int(11) NOT NULL,
  `promocion_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `boleta_id` (`boleta_id`),
  KEY `producto_id` (`producto_id`),
  KEY `detalle_boleta_ibfk_3_idx` (`promocion_id`),
  CONSTRAINT `detalle_boleta_ibfk_1` FOREIGN KEY (`boleta_id`) REFERENCES `boletas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `detalle_boleta_ibfk_2` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `detalle_boleta_ibfk_3` FOREIGN KEY (`promocion_id`) REFERENCES `promociones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table detalle_boleta
-- 

/*!40000 ALTER TABLE `detalle_boleta` DISABLE KEYS */;

/*!40000 ALTER TABLE `detalle_boleta` ENABLE KEYS */;

-- 
-- Definition of deudas
-- 

DROP TABLE IF EXISTS `deudas`;
CREATE TABLE IF NOT EXISTS `deudas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cantidad` int(11) DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  `saldada` tinyint(1) DEFAULT NULL,
  `producto_id` int(11) DEFAULT NULL,
  `cliente_id` int(11) DEFAULT NULL,
  `comentario` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  KEY `cliente_id` (`cliente_id`),
  CONSTRAINT `deudas_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `deudas_ibfk_2` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table deudas
-- 

/*!40000 ALTER TABLE `deudas` DISABLE KEYS */;

/*!40000 ALTER TABLE `deudas` ENABLE KEYS */;

-- 
-- Definition of devolucion
-- 

DROP TABLE IF EXISTS `devolucion`;
CREATE TABLE IF NOT EXISTS `devolucion` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) NOT NULL,
  `cantidad` decimal(12,2) NOT NULL,
  `fecha` datetime DEFAULT NULL,
  `monto` decimal(12,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `devolucion_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table devolucion
-- 

/*!40000 ALTER TABLE `devolucion` DISABLE KEYS */;

/*!40000 ALTER TABLE `devolucion` ENABLE KEYS */;

-- 
-- Definition of fiados
-- 

DROP TABLE IF EXISTS `fiados`;
CREATE TABLE IF NOT EXISTS `fiados` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` datetime DEFAULT NULL,
  `total` int(11) DEFAULT NULL,
  `cliente_id` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `cliente_id` (`cliente_id`),
  KEY `usuario_id` (`usuario_id`),
  CONSTRAINT `fiados_ibfk_1` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fiados_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table fiados
-- 

/*!40000 ALTER TABLE `fiados` DISABLE KEYS */;

/*!40000 ALTER TABLE `fiados` ENABLE KEYS */;

-- 
-- Definition of gastos
-- 

DROP TABLE IF EXISTS `gastos`;
CREATE TABLE IF NOT EXISTS `gastos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `detalle` varchar(50) DEFAULT NULL,
  `monto` decimal(12,2) DEFAULT NULL,
  `fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table gastos
-- 

/*!40000 ALTER TABLE `gastos` DISABLE KEYS */;

/*!40000 ALTER TABLE `gastos` ENABLE KEYS */;

-- 
-- Definition of ingresos
-- 

DROP TABLE IF EXISTS `ingresos`;
CREATE TABLE IF NOT EXISTS `ingresos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cantidad` int(11) NOT NULL,
  `fecha_desde` datetime DEFAULT NULL,
  `fecha_hasta` datetime DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `caja_inicial_efectivo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `usuario_id` (`usuario_id`),
  CONSTRAINT `ingresos_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table ingresos
-- 

/*!40000 ALTER TABLE `ingresos` DISABLE KEYS */;

/*!40000 ALTER TABLE `ingresos` ENABLE KEYS */;

-- 
-- Definition of jornadas
-- 

DROP TABLE IF EXISTS `jornadas`;
CREATE TABLE IF NOT EXISTS `jornadas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_apertura` datetime DEFAULT NULL,
  `fecha_cierre` datetime DEFAULT NULL,
  `especial` tinyint(1) DEFAULT NULL,
  `mensaje` varchar(50) DEFAULT NULL,
  `ingresos` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `caja_inicial_efectivo` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `usuario_id` (`usuario_id`),
  CONSTRAINT `jornadas_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=132 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table jornadas
-- 

/*!40000 ALTER TABLE `jornadas` DISABLE KEYS */;
INSERT INTO `jornadas`(`id`,`fecha_apertura`,`fecha_cierre`,`especial`,`mensaje`,`ingresos`,`usuario_id`,`caja_inicial_efectivo`) VALUES
(131,'2017-01-01 00:00:00','2017-01-01 00:01:00',0,'',0,22,0);
/*!40000 ALTER TABLE `jornadas` ENABLE KEYS */;

-- 
-- Definition of lineas_fiado
-- 

DROP TABLE IF EXISTS `lineas_fiado`;
CREATE TABLE IF NOT EXISTS `lineas_fiado` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `monto` int(11) DEFAULT NULL,
  `cantidad` decimal(10,0) NOT NULL,
  `descuento` decimal(10,0) NOT NULL DEFAULT '0',
  `producto_id` int(11) DEFAULT NULL,
  `fiado_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  KEY `fiado_id` (`fiado_id`),
  CONSTRAINT `lineas_fiado_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `lineas_fiado_ibfk_2` FOREIGN KEY (`fiado_id`) REFERENCES `fiados` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table lineas_fiado
-- 

/*!40000 ALTER TABLE `lineas_fiado` DISABLE KEYS */;

/*!40000 ALTER TABLE `lineas_fiado` ENABLE KEYS */;

-- 
-- Definition of materiasprimas
-- 

DROP TABLE IF EXISTS `materiasprimas`;
CREATE TABLE IF NOT EXISTS `materiasprimas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `unidad_medida_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `nombre_UNIQUE` (`nombre`),
  KEY `materiasprimas_ibfk_1_idx` (`unidad_medida_id`),
  CONSTRAINT `materiasprimas_ibfk_1` FOREIGN KEY (`unidad_medida_id`) REFERENCES `unidades_medida` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table materiasprimas
-- 

/*!40000 ALTER TABLE `materiasprimas` DISABLE KEYS */;

/*!40000 ALTER TABLE `materiasprimas` ENABLE KEYS */;

-- 
-- Definition of mermas
-- 

DROP TABLE IF EXISTS `mermas`;
CREATE TABLE IF NOT EXISTS `mermas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) NOT NULL,
  `cantidad` decimal(12,2) NOT NULL,
  `fecha` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `mermas_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table mermas
-- 

/*!40000 ALTER TABLE `mermas` DISABLE KEYS */;

/*!40000 ALTER TABLE `mermas` ENABLE KEYS */;

-- 
-- Definition of mesas
-- 

DROP TABLE IF EXISTS `mesas`;
CREATE TABLE IF NOT EXISTS `mesas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `codigo` varchar(20) DEFAULT NULL,
  `libre` tinyint(1) DEFAULT NULL,
  `sector_id` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `items` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `mesas_ibfk_1_idx` (`sector_id`),
  KEY `usuarios_ibfk_2_idx` (`usuario_id`),
  CONSTRAINT `mesas_ibfk_1` FOREIGN KEY (`sector_id`) REFERENCES `sectormesas` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `usuarios_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table mesas
-- 

/*!40000 ALTER TABLE `mesas` DISABLE KEYS */;
INSERT INTO `mesas`(`id`,`codigo`,`libre`,`sector_id`,`usuario_id`,`items`) VALUES
(2,'1',1,1,NULL,NULL),
(3,'2',1,1,NULL,NULL),
(4,'3',1,1,NULL,NULL),
(5,'4',1,1,NULL,NULL),
(6,'5',1,1,NULL,NULL),
(7,'6',1,1,NULL,NULL),
(8,'7',1,1,NULL,NULL),
(9,'8',1,1,NULL,NULL),
(10,'9',1,1,NULL,NULL),
(11,'10',1,1,NULL,NULL),
(12,'11',1,2,NULL,NULL),
(13,'12',1,2,NULL,NULL),
(14,'13',1,2,NULL,NULL),
(15,'14',1,2,NULL,NULL),
(16,'15',1,3,NULL,NULL),
(17,'16',1,3,NULL,NULL),
(18,'17',1,3,NULL,NULL),
(19,'18',1,3,NULL,NULL),
(20,'19',1,3,NULL,NULL),
(21,'20',1,3,NULL,NULL);
/*!40000 ALTER TABLE `mesas` ENABLE KEYS */;

-- 
-- Definition of pedidos
-- 

DROP TABLE IF EXISTS `pedidos`;
CREATE TABLE IF NOT EXISTS `pedidos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` datetime DEFAULT NULL,
  `mensaje` varchar(60) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `mesa_id` int(11) DEFAULT NULL,
  `pagado` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `usuario_id` (`usuario_id`),
  KEY `mesa_id` (`mesa_id`),
  CONSTRAINT `pedidos_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `pedidos_ibfk_2` FOREIGN KEY (`mesa_id`) REFERENCES `mesas` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pedidos
-- 

/*!40000 ALTER TABLE `pedidos` DISABLE KEYS */;

/*!40000 ALTER TABLE `pedidos` ENABLE KEYS */;

-- 
-- Definition of pedidos_agregados
-- 

DROP TABLE IF EXISTS `pedidos_agregados`;
CREATE TABLE IF NOT EXISTS `pedidos_agregados` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pedidos_productos_id` int(11) DEFAULT NULL,
  `agregado_uno_id` int(11) DEFAULT NULL,
  `agregado_dos_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pedidos_productos_id` (`pedidos_productos_id`),
  KEY `pedidos_agregados_ibfk_2_idx` (`agregado_uno_id`),
  KEY `pedidos_agregados_ibfk_3_idx` (`agregado_dos_id`),
  CONSTRAINT `pedidos_agregados_ibfk_1` FOREIGN KEY (`pedidos_productos_id`) REFERENCES `pedidos_productos` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `pedidos_agregados_ibfk_2` FOREIGN KEY (`agregado_uno_id`) REFERENCES `agregados` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `pedidos_agregados_ibfk_3` FOREIGN KEY (`agregado_dos_id`) REFERENCES `agregados` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pedidos_agregados
-- 

/*!40000 ALTER TABLE `pedidos_agregados` DISABLE KEYS */;

/*!40000 ALTER TABLE `pedidos_agregados` ENABLE KEYS */;

-- 
-- Definition of pedidos_preparaciones
-- 

DROP TABLE IF EXISTS `pedidos_preparaciones`;
CREATE TABLE IF NOT EXISTS `pedidos_preparaciones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `pedidos_productos_id` int(11) DEFAULT NULL,
  `preparacion_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pedidos_productos_id` (`pedidos_productos_id`),
  KEY `preparacion_id` (`preparacion_id`),
  CONSTRAINT `pedidos_preparaciones_ibfk_1` FOREIGN KEY (`pedidos_productos_id`) REFERENCES `pedidos_productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `pedidos_preparaciones_ibfk_2` FOREIGN KEY (`preparacion_id`) REFERENCES `preparaciones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pedidos_preparaciones
-- 

/*!40000 ALTER TABLE `pedidos_preparaciones` DISABLE KEYS */;

/*!40000 ALTER TABLE `pedidos_preparaciones` ENABLE KEYS */;

-- 
-- Definition of pedidos_productos
-- 

DROP TABLE IF EXISTS `pedidos_productos`;
CREATE TABLE IF NOT EXISTS `pedidos_productos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cantidad` decimal(12,2) DEFAULT NULL,
  `pedido_id` int(11) DEFAULT NULL,
  `producto_id` int(11) DEFAULT NULL,
  `impreso` tinyint(1) DEFAULT NULL,
  `impreso_cantidad` decimal(12,2) DEFAULT NULL,
  `promo_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pedido_id` (`pedido_id`),
  KEY `producto_id` (`producto_id`),
  KEY `pedidos_productos_ibfk_3_idx` (`promo_id`),
  CONSTRAINT `pedidos_productos_ibfk_1` FOREIGN KEY (`pedido_id`) REFERENCES `pedidos` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `pedidos_productos_ibfk_2` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `pedidos_productos_ibfk_3` FOREIGN KEY (`promo_id`) REFERENCES `promociones` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pedidos_productos
-- 

/*!40000 ALTER TABLE `pedidos_productos` DISABLE KEYS */;

/*!40000 ALTER TABLE `pedidos_productos` ENABLE KEYS */;

-- 
-- Definition of pendientes
-- 

DROP TABLE IF EXISTS `pendientes`;
CREATE TABLE IF NOT EXISTS `pendientes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `fecha` datetime NOT NULL,
  `archivado` tinyint(1) DEFAULT NULL,
  `promocion_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `pendientes_ibfk_1` (`producto_id`),
  KEY `pendientes_ibfk_2_idx` (`usuario_id`),
  KEY `pendientes_ibfk_3_idx` (`promocion_id`),
  CONSTRAINT `pendientes_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `pendientes_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `pendientes_ibfk_3` FOREIGN KEY (`promocion_id`) REFERENCES `promociones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pendientes
-- 

/*!40000 ALTER TABLE `pendientes` DISABLE KEYS */;

/*!40000 ALTER TABLE `pendientes` ENABLE KEYS */;

-- 
-- Definition of preparaciones
-- 

DROP TABLE IF EXISTS `preparaciones`;
CREATE TABLE IF NOT EXISTS `preparaciones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table preparaciones
-- 

/*!40000 ALTER TABLE `preparaciones` DISABLE KEYS */;
INSERT INTO `preparaciones`(`id`,`nombre`) VALUES
(1,'A LA PLANCHA'),
(2,'A LA MANTEQUILLA'),
(3,'FRITO');
/*!40000 ALTER TABLE `preparaciones` ENABLE KEYS */;

-- 
-- Definition of prestamo_envases
-- 

DROP TABLE IF EXISTS `prestamo_envases`;
CREATE TABLE IF NOT EXISTS `prestamo_envases` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `envases` int(11) NOT NULL DEFAULT '0',
  `fecha` datetime NOT NULL,
  `cliente_id` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `cliente_id` (`cliente_id`),
  CONSTRAINT `prestamo_envases_ibfk_1` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table prestamo_envases
-- 

/*!40000 ALTER TABLE `prestamo_envases` DISABLE KEYS */;

/*!40000 ALTER TABLE `prestamo_envases` ENABLE KEYS */;

-- 
-- Definition of producto_compra
-- 

DROP TABLE IF EXISTS `producto_compra`;
CREATE TABLE IF NOT EXISTS `producto_compra` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `compra_id` int(11) NOT NULL,
  `producto_id` int(11) NOT NULL,
  `cantidad_disponible` decimal(12,2) NOT NULL,
  `costo_unitario` decimal(12,2) DEFAULT NULL,
  `cantidad_compra` decimal(12,2) DEFAULT NULL,
  `activa` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `compras_producto_ibfk_1` (`producto_id`),
  KEY `compras_producto_ibfk_2` (`compra_id`),
  CONSTRAINT `producto_compra_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `producto_compra_ibfk_2` FOREIGN KEY (`compra_id`) REFERENCES `compras` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table producto_compra
-- 

/*!40000 ALTER TABLE `producto_compra` DISABLE KEYS */;

/*!40000 ALTER TABLE `producto_compra` ENABLE KEYS */;

-- 
-- Definition of producto_materiaprima
-- 

DROP TABLE IF EXISTS `producto_materiaprima`;
CREATE TABLE IF NOT EXISTS `producto_materiaprima` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `materiaprima_id` int(11) DEFAULT NULL,
  `producto_id` int(11) DEFAULT NULL,
  `cantidad` decimal(12,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `materiaprima_id` (`materiaprima_id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `producto_materiaprima_ibfk_1` FOREIGN KEY (`materiaprima_id`) REFERENCES `materiasprimas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `producto_materiaprima_ibfk_2` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table producto_materiaprima
-- 

/*!40000 ALTER TABLE `producto_materiaprima` DISABLE KEYS */;

/*!40000 ALTER TABLE `producto_materiaprima` ENABLE KEYS */;

-- 
-- Definition of producto_preparacion
-- 

DROP TABLE IF EXISTS `producto_preparacion`;
CREATE TABLE IF NOT EXISTS `producto_preparacion` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `preparacion_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `preparacion_id` (`preparacion_id`),
  KEY `producto_preparacion_ibfk_3` (`producto_id`),
  CONSTRAINT `producto_preparacion_ibfk_2` FOREIGN KEY (`preparacion_id`) REFERENCES `preparaciones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `producto_preparacion_ibfk_3` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table producto_preparacion
-- 

/*!40000 ALTER TABLE `producto_preparacion` DISABLE KEYS */;

/*!40000 ALTER TABLE `producto_preparacion` ENABLE KEYS */;

-- 
-- Definition of producto_productocomplejo
-- 

DROP TABLE IF EXISTS `producto_productocomplejo`;
CREATE TABLE IF NOT EXISTS `producto_productocomplejo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `productocomplejo_id` int(11) DEFAULT NULL,
  `producto_id` int(11) DEFAULT NULL,
  `cantidad` decimal(12,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `productocomplejo_id` (`productocomplejo_id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `producto_productocomplejo_ibfk_1` FOREIGN KEY (`productocomplejo_id`) REFERENCES `productoscomplejos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `producto_productocomplejo_ibfk_2` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table producto_productocomplejo
-- 

/*!40000 ALTER TABLE `producto_productocomplejo` DISABLE KEYS */;

/*!40000 ALTER TABLE `producto_productocomplejo` ENABLE KEYS */;

-- 
-- Definition of producto_promocion
-- 

DROP TABLE IF EXISTS `producto_promocion`;
CREATE TABLE IF NOT EXISTS `producto_promocion` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `promocion_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  KEY `promocion_id` (`promocion_id`),
  CONSTRAINT `producto_promocion_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `producto_promocion_ibfk_2` FOREIGN KEY (`promocion_id`) REFERENCES `promociones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table producto_promocion
-- 

/*!40000 ALTER TABLE `producto_promocion` DISABLE KEYS */;

/*!40000 ALTER TABLE `producto_promocion` ENABLE KEYS */;

-- 
-- Definition of productos
-- 

DROP TABLE IF EXISTS `productos`;
CREATE TABLE IF NOT EXISTS `productos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `codigo_barras` varchar(100) DEFAULT NULL,
  `precio` int(11) DEFAULT NULL,
  `retornable` tinyint(1) DEFAULT NULL,
  `favorito` tinyint(1) DEFAULT NULL,
  `puntos_cantidad` int(11) DEFAULT NULL,
  `imagen` varchar(1024) DEFAULT NULL,
  `sub_categoria_id` int(11) DEFAULT NULL,
  `proveedor_id` int(11) DEFAULT NULL,
  `contiene_agregado` tinyint(1) DEFAULT NULL,
  `solo_venta` tinyint(1) DEFAULT NULL,
  `solo_compra` tinyint(1) DEFAULT NULL,
  `preparado_especial` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `nombre_UNIQUE` (`nombre`),
  KEY `productos_ibfk_3` (`proveedor_id`),
  KEY `producto_ibfk_5_idx` (`sub_categoria_id`),
  CONSTRAINT `productos_ibfk_3` FOREIGN KEY (`proveedor_id`) REFERENCES `proveedores` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `productos_ibfk_5` FOREIGN KEY (`sub_categoria_id`) REFERENCES `subcategorias` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table productos
-- 

/*!40000 ALTER TABLE `productos` DISABLE KEYS */;
INSERT INTO `productos`(`id`,`nombre`,`codigo_barras`,`precio`,`retornable`,`favorito`,`puntos_cantidad`,`imagen`,`sub_categoria_id`,`proveedor_id`,`contiene_agregado`,`solo_venta`,`solo_compra`,`preparado_especial`) VALUES
(71,'COCACOLA 350 ml','',19000,0,1,90,'COCACOLA 350 ml.jpg',NULL,NULL,0,0,NULL,NULL),
(72,'SPRITE 350 ml','',1900,0,1,90,'SPRITE 350 ml.jpg',NULL,NULL,0,0,NULL,NULL),
(73,'FANTA 350 ml','',900,0,1,90,'FANTA 350 ml.jpg',NULL,NULL,0,0,NULL,NULL),
(74,'NECT DURAZ 350 ml','',1000,0,1,100,'NECT DURAZ 350 ml.jpg',NULL,NULL,0,0,NULL,NULL),
(75,'CACHANTUN 500 ml','',900,0,1,90,'CACHANTUN 500 ml.jpg',NULL,NULL,0,0,NULL,NULL),
(91,'merluza','',3500,0,1,0,'merluza.jpg',NULL,NULL,1,1,0,1);
/*!40000 ALTER TABLE `productos` ENABLE KEYS */;

-- 
-- Definition of productoscomplejos
-- 

DROP TABLE IF EXISTS `productoscomplejos`;
CREATE TABLE IF NOT EXISTS `productoscomplejos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `productoscomplejos_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table productoscomplejos
-- 

/*!40000 ALTER TABLE `productoscomplejos` DISABLE KEYS */;

/*!40000 ALTER TABLE `productoscomplejos` ENABLE KEYS */;

-- 
-- Definition of promociones
-- 

DROP TABLE IF EXISTS `promociones`;
CREATE TABLE IF NOT EXISTS `promociones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `precio` int(11) DEFAULT NULL,
  `imagen` varchar(1024) DEFAULT NULL,
  `subcategoria_id` int(11) DEFAULT NULL,
  `favorito` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `promociones_ibfk_1_idx` (`subcategoria_id`),
  CONSTRAINT `promociones_ibfk_1` FOREIGN KEY (`subcategoria_id`) REFERENCES `subcategorias` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table promociones
-- 

/*!40000 ALTER TABLE `promociones` DISABLE KEYS */;

/*!40000 ALTER TABLE `promociones` ENABLE KEYS */;

-- 
-- Definition of proveedores
-- 

DROP TABLE IF EXISTS `proveedores`;
CREATE TABLE IF NOT EXISTS `proveedores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  `representante` varchar(45) DEFAULT NULL,
  `contacto` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `proveedor_contacto_id` (`representante`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table proveedores
-- 

/*!40000 ALTER TABLE `proveedores` DISABLE KEYS */;

/*!40000 ALTER TABLE `proveedores` ENABLE KEYS */;

-- 
-- Definition of puntos
-- 

DROP TABLE IF EXISTS `puntos`;
CREATE TABLE IF NOT EXISTS `puntos` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `puntos_activos` int(11) DEFAULT NULL,
  `puntos_expirados` int(11) DEFAULT NULL,
  `fecha_expiracion` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table puntos
-- 

/*!40000 ALTER TABLE `puntos` DISABLE KEYS */;

/*!40000 ALTER TABLE `puntos` ENABLE KEYS */;

-- 
-- Definition of registros
-- 

DROP TABLE IF EXISTS `registros`;
CREATE TABLE IF NOT EXISTS `registros` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` date DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  `tipo` varchar(20) DEFAULT NULL,
  `detalle` varchar(80) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `usuario_id` (`usuario_id`),
  CONSTRAINT `registros_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table registros
-- 

/*!40000 ALTER TABLE `registros` DISABLE KEYS */;

/*!40000 ALTER TABLE `registros` ENABLE KEYS */;

-- 
-- Definition of reservas
-- 

DROP TABLE IF EXISTS `reservas`;
CREATE TABLE IF NOT EXISTS `reservas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` datetime DEFAULT NULL,
  `mesa_id` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `mesa_id` (`mesa_id`),
  KEY `usuario_id` (`usuario_id`),
  CONSTRAINT `reservas_ibfk_1` FOREIGN KEY (`mesa_id`) REFERENCES `mesas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `reservas_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table reservas
-- 

/*!40000 ALTER TABLE `reservas` DISABLE KEYS */;

/*!40000 ALTER TABLE `reservas` ENABLE KEYS */;

-- 
-- Definition of sectores
-- 

DROP TABLE IF EXISTS `sectores`;
CREATE TABLE IF NOT EXISTS `sectores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table sectores
-- 

/*!40000 ALTER TABLE `sectores` DISABLE KEYS */;
INSERT INTO `sectores`(`id`,`nombre`) VALUES
(25,'BAR'),
(26,'COCINA');
/*!40000 ALTER TABLE `sectores` ENABLE KEYS */;

-- 
-- Definition of sectormesas
-- 

DROP TABLE IF EXISTS `sectormesas`;
CREATE TABLE IF NOT EXISTS `sectormesas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table sectormesas
-- 

/*!40000 ALTER TABLE `sectormesas` DISABLE KEYS */;
INSERT INTO `sectormesas`(`id`,`nombre`) VALUES
(1,'terraza'),
(2,'uno'),
(3,'dos');
/*!40000 ALTER TABLE `sectormesas` ENABLE KEYS */;

-- 
-- Definition of stock_mp
-- 

DROP TABLE IF EXISTS `stock_mp`;
CREATE TABLE IF NOT EXISTS `stock_mp` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `materiaprima_id` int(11) DEFAULT NULL,
  `entrada` decimal(12,2) NOT NULL DEFAULT '0.00',
  `salida` decimal(12,2) NOT NULL DEFAULT '0.00',
  `ajuste` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `materiaprima_id` (`materiaprima_id`),
  CONSTRAINT `stock_mp_ibfk_1` FOREIGN KEY (`materiaprima_id`) REFERENCES `materiasprimas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table stock_mp
-- 

/*!40000 ALTER TABLE `stock_mp` DISABLE KEYS */;

/*!40000 ALTER TABLE `stock_mp` ENABLE KEYS */;

-- 
-- Definition of stock_pr
-- 

DROP TABLE IF EXISTS `stock_pr`;
CREATE TABLE IF NOT EXISTS `stock_pr` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `entrada` decimal(12,2) NOT NULL DEFAULT '0.00',
  `salida` decimal(12,2) NOT NULL DEFAULT '0.00',
  `ajuste` decimal(12,2) NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  CONSTRAINT `stock_pr_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table stock_pr
-- 

/*!40000 ALTER TABLE `stock_pr` DISABLE KEYS */;

/*!40000 ALTER TABLE `stock_pr` ENABLE KEYS */;

-- 
-- Definition of subcategorias
-- 

DROP TABLE IF EXISTS `subcategorias`;
CREATE TABLE IF NOT EXISTS `subcategorias` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `nombre_UNIQUE` (`nombre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table subcategorias
-- 

/*!40000 ALTER TABLE `subcategorias` DISABLE KEYS */;

/*!40000 ALTER TABLE `subcategorias` ENABLE KEYS */;

-- 
-- Definition of syncs
-- 

DROP TABLE IF EXISTS `syncs`;
CREATE TABLE IF NOT EXISTS `syncs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `sync_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table syncs
-- 

/*!40000 ALTER TABLE `syncs` DISABLE KEYS */;

/*!40000 ALTER TABLE `syncs` ENABLE KEYS */;

-- 
-- Definition of unidades_medida
-- 

DROP TABLE IF EXISTS `unidades_medida`;
CREATE TABLE IF NOT EXISTS `unidades_medida` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table unidades_medida
-- 

/*!40000 ALTER TABLE `unidades_medida` DISABLE KEYS */;
INSERT INTO `unidades_medida`(`id`,`nombre`) VALUES
(4,'ml'),
(5,'gr'),
(6,'lt'),
(7,'un');
/*!40000 ALTER TABLE `unidades_medida` ENABLE KEYS */;

-- 
-- Definition of usuarios
-- 

DROP TABLE IF EXISTS `usuarios`;
CREATE TABLE IF NOT EXISTS `usuarios` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(25) DEFAULT NULL,
  `nombre_usuario` varchar(20) DEFAULT NULL,
  `pass` varchar(256) DEFAULT NULL,
  `tipo` char(1) DEFAULT NULL,
  `config_id` int(11) NOT NULL DEFAULT '1',
  `imagen` varchar(2048) DEFAULT NULL,
  `favorito` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `config_id` (`config_id`),
  CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`config_id`) REFERENCES `configs` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table usuarios
-- 

/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios`(`id`,`nombre`,`nombre_usuario`,`pass`,`tipo`,`config_id`,`imagen`,`favorito`) VALUES
(22,'Admin','','1144','A',1,'admin.jpg',1),
(30,'cajero','','12','C',1,'default.jpg',1);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;

-- 
-- Definition of ventas_jornada
-- 

DROP TABLE IF EXISTS `ventas_jornada`;
CREATE TABLE IF NOT EXISTS `ventas_jornada` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `producto_id` int(11) DEFAULT NULL,
  `promo_id` int(11) DEFAULT NULL,
  `jornada_id` int(11) DEFAULT NULL,
  `cantidad` decimal(12,2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  KEY `jornada_id` (`jornada_id`),
  KEY `ventas_jornada_ibfk_2` (`promo_id`),
  CONSTRAINT `ventas_jornada_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ventas_jornada_ibfk_3` FOREIGN KEY (`jornada_id`) REFERENCES `jornadas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ventas_jornada_ibfk_4` FOREIGN KEY (`promo_id`) REFERENCES `promociones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table ventas_jornada
-- 

/*!40000 ALTER TABLE `ventas_jornada` DISABLE KEYS */;

/*!40000 ALTER TABLE `ventas_jornada` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2017-12-05 05:52:32
-- Total time: 0:0:0:0:353 (d:h:m:s:ms)
