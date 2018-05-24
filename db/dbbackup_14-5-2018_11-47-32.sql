-- MySqlBackup.NET 2.0.9.2
-- Dump Time: 2018-05-14 11:47:32
-- --------------------------------------
-- Server version 5.7.21-log MySQL Community Server (GPL)


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
  `font_size` int(11) NOT NULL DEFAULT '20',
  `para_handroll` tinyint(1) DEFAULT '0',
  `es_vegetal` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table agregados
-- 

/*!40000 ALTER TABLE `agregados` DISABLE KEYS */;
INSERT INTO `agregados`(`id`,`nombre`,`cobro_extra`,`font_size`,`para_handroll`,`es_vegetal`) VALUES
(9,'SALMON',0,16,1,0),
(10,'POLLO',0,16,1,0),
(11,'CHAMPIÑON',0,16,1,0),
(12,'CIBOULETTE',0,16,1,1),
(13,'PALMITO',0,16,1,1),
(19,'CEBOLLIN',0,16,1,1),
(20,'PALTA',0,16,1,1),
(21,'KANIKAMA',0,16,1,0),
(22,'CAMARON',0,16,1,0),
(26,'PIMENTON',0,16,1,1),
(28,'CHOCLO',0,16,1,1),
(29,'AJI ORO',0,16,1,1),
(30,'ACEITUNAS',0,16,1,1),
(31,'ESPARRAGOS',0,16,1,1),
(32,'PEPINO',0,16,1,1);
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table bodega_stock
-- 

/*!40000 ALTER TABLE `bodega_stock` DISABLE KEYS */;
INSERT INTO `bodega_stock`(`id`,`producto_id`,`stock`) VALUES
(1,250,30.00),
(2,251,10.00);
/*!40000 ALTER TABLE `bodega_stock` ENABLE KEYS */;

-- 
-- Definition of boleta_mediopago
-- 

DROP TABLE IF EXISTS `boleta_mediopago`;
CREATE TABLE IF NOT EXISTS `boleta_mediopago` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `boleta_id` int(11) DEFAULT NULL,
  `mediopago_id` int(11) DEFAULT NULL,
  `ingreso` int(11) NOT NULL DEFAULT '0',
  `vendedor_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `boletamediopago_fk_idx` (`boleta_id`),
  KEY `mediopago_fk_idx` (`mediopago_id`),
  KEY `usuario_fk_idx` (`vendedor_id`),
  CONSTRAINT `boletamediopago_fk` FOREIGN KEY (`boleta_id`) REFERENCES `boletas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `mediopago_fk` FOREIGN KEY (`mediopago_id`) REFERENCES `medio_pago` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `usuario_fk` FOREIGN KEY (`vendedor_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=228 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table boleta_mediopago
-- 

/*!40000 ALTER TABLE `boleta_mediopago` DISABLE KEYS */;
INSERT INTO `boleta_mediopago`(`id`,`boleta_id`,`mediopago_id`,`ingreso`,`vendedor_id`) VALUES
(165,479,1,600,22),
(166,479,2,3000,22),
(167,480,1,2200,22),
(168,481,1,2200,22),
(169,482,1,2200,22),
(170,483,1,9680,47),
(171,484,1,9790,22),
(172,485,1,2200,22),
(173,486,1,2200,22),
(174,487,1,2200,22),
(175,488,1,2200,22),
(176,489,1,2200,22),
(177,490,1,2200,22),
(178,491,1,2200,22),
(179,492,1,2200,22),
(180,493,1,2200,22),
(181,494,1,2200,22),
(182,495,1,2200,22),
(183,496,1,2200,22),
(184,497,1,7150,22),
(185,498,1,2200,22),
(186,499,1,2200,22),
(187,500,1,2200,22),
(188,501,1,2200,22),
(189,502,1,2200,22),
(190,503,1,2200,22),
(191,504,1,2200,22),
(192,505,1,2000,22),
(193,506,2,2200,22),
(194,507,2,2000,22),
(195,508,1,300,22),
(196,509,1,750,22),
(197,510,3,605,22),
(198,511,1,550,22),
(199,512,1,500,47),
(200,513,2,500,47),
(201,514,1,500,47),
(202,515,1,550,22),
(203,516,1,550,22),
(204,517,1,550,22),
(205,518,1,600,22),
(206,519,1,550,22),
(207,520,1,550,22),
(208,521,1,550,22),
(209,522,1,1500,22),
(210,523,1,1500,22),
(211,524,1,550,22),
(212,525,2,300,22),
(213,526,1,550,22),
(214,527,1,500,22),
(215,528,2,1550,22),
(216,529,2,2550,22),
(217,530,2,2550,22),
(218,531,1,2000,22),
(219,532,1,8900,22),
(220,533,2,4400,22),
(221,534,1,1500,22),
(222,535,1,750,22),
(223,536,1,750,22),
(224,537,1,2000,22),
(225,538,1,750,22),
(226,539,1,750,22),
(227,540,2,750,22);
/*!40000 ALTER TABLE `boleta_mediopago` ENABLE KEYS */;

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
  `propina` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `boletas_ibfk_1` (`cliente_id`),
  KEY `boletas_ibfk_2` (`usuario_id`),
  CONSTRAINT `boletas_ibfk_1` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `boletas_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=541 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table boletas
-- 

/*!40000 ALTER TABLE `boletas` DISABLE KEYS */;
INSERT INTO `boletas`(`id`,`numero_boleta`,`fecha`,`puntos_cantidad`,`total`,`cliente_id`,`usuario_id`,`propina`) VALUES
(479,NULL,'2018-04-24 17:08:45',0,3100,NULL,22,500),
(480,NULL,'2018-04-24 17:10:04',0,2000,NULL,22,200),
(481,NULL,'2018-04-24 17:10:49',0,2000,NULL,22,200),
(482,NULL,'2018-04-24 17:12:44',0,2000,NULL,22,200),
(483,NULL,'2018-04-24 17:28:07',0,8800,NULL,47,880),
(484,NULL,'2018-04-24 17:39:17',0,8900,NULL,22,890),
(485,NULL,'2018-04-24 19:39:57',0,2000,NULL,22,200),
(486,NULL,'2018-04-24 19:43:19',0,2000,NULL,22,200),
(487,NULL,'2018-04-24 19:46:11',0,2000,NULL,22,200),
(488,NULL,'2018-04-24 19:49:12',0,2000,NULL,22,200),
(489,NULL,'2018-04-24 19:50:37',0,2000,NULL,22,200),
(490,NULL,'2018-04-24 19:54:57',0,2000,NULL,22,200),
(491,NULL,'2018-04-24 20:04:11',0,2000,NULL,22,200),
(492,NULL,'2018-04-24 20:05:30',0,2000,NULL,22,200),
(493,NULL,'2018-04-24 20:08:06',0,2000,NULL,22,200),
(494,NULL,'2018-04-24 20:25:08',0,2000,NULL,22,200),
(495,NULL,'2018-04-24 20:28:09',0,2000,NULL,22,200),
(496,NULL,'2018-04-24 20:30:09',0,2000,NULL,22,200),
(497,NULL,'2018-04-24 20:44:31',0,6500,NULL,22,650),
(498,NULL,'2018-04-25 14:18:34',0,2000,NULL,22,200),
(499,NULL,'2018-04-25 14:27:49',0,2000,NULL,22,200),
(500,NULL,'2018-04-25 14:29:42',0,2000,NULL,22,200),
(501,NULL,'2018-04-25 15:19:45',0,2000,NULL,22,200),
(502,NULL,'2018-04-25 15:51:54',0,2000,NULL,22,200),
(503,NULL,'2018-04-25 15:52:32',0,2000,NULL,22,200),
(504,NULL,'2018-04-25 15:53:06',0,2000,NULL,22,200),
(505,NULL,'2018-04-25 16:06:16',0,2000,NULL,22,0),
(506,NULL,'2018-04-25 16:07:09',0,2000,NULL,22,200),
(507,NULL,'2018-04-26 17:53:04',0,2000,NULL,22,0),
(508,NULL,'2018-04-26 18:01:17',0,300,NULL,22,0),
(509,NULL,'2018-04-26 18:28:26',0,750,NULL,22,0),
(510,NULL,'2018-04-26 18:32:03',0,550,NULL,22,55),
(511,NULL,'2018-04-26 18:38:19',0,550,NULL,22,0),
(512,NULL,'2018-04-26 18:40:32',0,500,NULL,47,0),
(513,NULL,'2018-04-26 18:40:49',0,500,NULL,47,0),
(514,NULL,'2018-04-26 18:43:44',0,500,NULL,47,0),
(515,1,'2018-05-02 18:02:09',0,550,NULL,22,0),
(516,2,'2018-05-02 18:02:31',0,550,NULL,22,0),
(517,3,'2018-05-02 18:02:44',0,550,NULL,22,0),
(518,0,'2018-05-02 18:03:14',0,600,NULL,22,0),
(519,5,'2018-05-02 18:04:02',0,550,NULL,22,0),
(520,6,'2018-05-02 18:05:41',0,550,NULL,22,0),
(521,7,'2018-05-02 18:06:42',0,550,NULL,22,0),
(522,0,'2018-05-02 18:11:13',0,1500,NULL,22,0),
(523,1,'2018-05-02 18:11:51',0,1500,NULL,22,0),
(524,0,'2018-05-02 18:26:11',0,550,NULL,22,0),
(525,1,'2018-05-02 18:28:02',0,300,NULL,22,0),
(526,2,'2018-05-02 18:28:17',0,550,NULL,22,0),
(527,3,'2018-05-02 21:01:50',0,500,NULL,22,0),
(528,4,'2018-05-02 21:02:56',0,550,NULL,22,1000),
(529,5,'2018-05-02 21:04:24',0,550,NULL,22,2000),
(530,6,'2018-05-02 21:04:48',0,550,NULL,22,2000),
(531,7,'2018-05-12 19:35:41',0,2000,NULL,22,0),
(532,8,'2018-05-12 19:39:00',0,8900,NULL,22,0),
(533,9,'2018-05-12 19:40:09',0,4000,NULL,22,400),
(534,10,'2018-05-12 19:41:53',0,1500,NULL,22,0),
(535,11,'2018-05-12 19:49:28',0,750,NULL,22,0),
(536,12,'2018-05-12 20:22:46',0,750,NULL,22,0),
(537,13,'2018-05-13 13:42:13',0,2000,NULL,22,0),
(538,14,'2018-05-13 15:22:32',0,750,NULL,22,0),
(539,15,'2018-05-13 15:23:17',0,750,NULL,22,0),
(540,16,'2018-05-13 15:42:05',0,750,NULL,22,0);
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
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table categoria_sector
-- 

/*!40000 ALTER TABLE `categoria_sector` DISABLE KEYS */;
INSERT INTO `categoria_sector`(`id`,`categoria_id`,`sector_id`) VALUES
(51,42,40),
(52,43,42),
(53,44,43),
(54,46,45),
(55,47,46);
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
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table categoria_subcategoria
-- 

/*!40000 ALTER TABLE `categoria_subcategoria` DISABLE KEYS */;
INSERT INTO `categoria_subcategoria`(`id`,`categoria_id`,`subcategoria_id`) VALUES
(41,42,25),
(49,43,50),
(50,44,51),
(51,46,53),
(52,47,54);
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
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table categorias
-- 

/*!40000 ALTER TABLE `categorias` DISABLE KEYS */;
INSERT INTO `categorias`(`id`,`nombre`,`seleccionable`) VALUES
(42,'MENU',0),
(43,'TABLA',0),
(44,'PROMO',0),
(45,'OTRO',0),
(46,'PLATOS',0),
(47,'BEBIDA',0);
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table compras
-- 

/*!40000 ALTER TABLE `compras` DISABLE KEYS */;
INSERT INTO `compras`(`id`,`fecha`,`usuario_id`) VALUES
(1,'2018-05-12 19:41:33',22),
(2,'2018-05-13 13:41:51',22);
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
  `teclado_tactil_integrado` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table datos_negocio
-- 

/*!40000 ALTER TABLE `datos_negocio` DISABLE KEYS */;
INSERT INTO `datos_negocio`(`id`,`nombre`,`mensaje`,`direccion`,`logo`,`iva`,`inicio_jornada_dia`,`termino_jornada_dia`,`inicio_jornada_hora`,`termino_jornada_hora`,`modo`,`correo_primario`,`correo_secundario`,`teclado_tactil_integrado`) VALUES
(1,'SUSHI','','','',19,'A','A','12:00:00','23:59:00','SUSHI','crsk@mail.com','',0);
/*!40000 ALTER TABLE `datos_negocio` ENABLE KEYS */;

-- 
-- Definition of delivery_item
-- 

DROP TABLE IF EXISTS `delivery_item`;
CREATE TABLE IF NOT EXISTS `delivery_item` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `direccion` varchar(60) DEFAULT NULL,
  `nombre_cliente` varchar(30) DEFAULT NULL,
  `cliente_id` int(11) DEFAULT NULL,
  `comentario` varchar(60) DEFAULT NULL,
  `incluye` varchar(60) DEFAULT NULL,
  `fecha_entrega` datetime DEFAULT NULL,
  `boleta_id` int(11) DEFAULT NULL,
  `servir` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `cliente_fk_idx` (`cliente_id`),
  KEY `boleta_fk_idx` (`boleta_id`),
  CONSTRAINT `boleta_fk` FOREIGN KEY (`boleta_id`) REFERENCES `boletas` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `cliente_fk` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=170 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table delivery_item
-- 

/*!40000 ALTER TABLE `delivery_item` DISABLE KEYS */;
INSERT INTO `delivery_item`(`id`,`direccion`,`nombre_cliente`,`cliente_id`,`comentario`,`incluye`,`fecha_entrega`,`boleta_id`,`servir`) VALUES
(111,'','',NULL,'','SOYA x2\nUNAGI x0\n','2018-05-13 15:29:25',479,1),
(112,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:27',480,1),
(113,'','',NULL,'','SOYA x1\nUNAGI x0\n','2018-05-13 15:29:30',481,1),
(114,'','',NULL,'','SOYA x0\nUNAGI x1\n','2018-05-13 15:29:32',482,1),
(115,'','',NULL,'','SOYA x2\nUNAGI x1\n','2018-05-13 15:29:35',483,1),
(116,'','pedro',NULL,'','SOYA x2\nUNAGI x2\n','2018-04-24 17:40:30',484,1),
(117,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:37',485,1),
(118,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:39',486,1),
(119,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:41',487,1),
(120,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:43',488,1),
(121,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:46',489,1),
(122,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:49',491,1),
(123,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:51',492,1),
(124,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:53',493,1),
(125,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:55',494,1),
(126,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:57',495,1),
(127,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:29:59',497,1),
(128,'','',NULL,'','SOYA x0\nUNAGI x0\n',NULL,498,1),
(129,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:54',499,1),
(130,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:47',501,1),
(131,'','cliente',NULL,'','SOYA x1\nUNAGI x1\n','2018-05-13 15:30:41',502,1),
(132,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:03',503,1),
(133,'','asd',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:04',504,1),
(134,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:30',505,1),
(135,'','juana',NULL,'','SOYA x0\nUNAGI x1\n','2018-05-13 15:30:06',506,1),
(136,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:08',507,1),
(137,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:18',508,1),
(138,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:10',509,1),
(139,'','CHRISTOPHER',NULL,'','SOYA x1\nUNAGI x2\n','2018-05-13 15:30:12',510,1),
(140,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:14',511,1),
(141,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:16',512,1),
(142,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:20',513,1),
(143,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:22',514,1),
(144,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:24',515,1),
(145,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:26',516,1),
(146,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:28',517,1),
(147,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:32',518,1),
(148,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:34',519,1),
(149,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:37',520,1),
(150,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:39',521,1),
(151,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:43',522,1),
(152,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:45',523,1),
(153,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:49',524,1),
(154,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:51',525,1),
(155,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:31:09',526,1),
(156,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:57',527,1),
(157,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:30:59',528,1),
(158,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:31:01',529,1),
(159,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:31:03',530,1),
(160,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:31:11',531,1),
(161,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:31:07',532,1),
(162,'','',NULL,'','SOYA x0\nUNAGI x1\n',NULL,533,1),
(163,'','',NULL,'','SOYA x0\nUNAGI x0\n','2018-05-13 15:38:58',534,1),
(164,'','',NULL,'','SOYA x0\nUNAGI x0\n',NULL,535,1),
(165,'','',NULL,'','SOYA x0\nUNAGI x0\n',NULL,536,1),
(166,'','',NULL,'','SOYA x0\nUNAGI x0\n',NULL,537,0),
(167,'','marcelo',NULL,'','SOYA x0\nUNAGI x0\n',NULL,538,0),
(168,'','claudia',NULL,'','SOYA x0\nUNAGI x1\n','2018-05-13 15:29:21',539,1),
(169,'','yop',NULL,'','SOYA x0\nUNAGI x2\n',NULL,540,0);
/*!40000 ALTER TABLE `delivery_item` ENABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=1749 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table detalle_boleta
-- 

/*!40000 ALTER TABLE `detalle_boleta` DISABLE KEYS */;
INSERT INTO `detalle_boleta`(`id`,`monto`,`cantidad`,`descuento`,`producto_id`,`boleta_id`,`promocion_id`) VALUES
(1699,2000,1,0,234,479,NULL),
(1701,2000,1,0,234,480,NULL),
(1702,2000,1,0,234,481,NULL),
(1703,2000,1,0,234,482,NULL),
(1704,8500,1,0,236,483,NULL),
(1706,8900,1,0,242,484,NULL),
(1707,2000,1,0,234,485,NULL),
(1708,2000,1,0,234,486,NULL),
(1709,2000,1,0,234,487,NULL),
(1710,2000,1,0,234,488,NULL),
(1711,2000,1,0,234,489,NULL),
(1712,2000,1,0,234,490,NULL),
(1713,2000,1,0,234,491,NULL),
(1714,2000,1,0,234,492,NULL),
(1715,2000,1,0,234,493,NULL),
(1716,2000,1,0,234,494,NULL),
(1717,2000,1,0,234,495,NULL),
(1718,2000,1,0,234,496,NULL),
(1719,2000,1,0,234,497,NULL),
(1720,4500,1,0,231,497,NULL),
(1721,2000,1,0,234,498,NULL),
(1722,2000,1,0,234,499,NULL),
(1723,2000,1,0,234,500,NULL),
(1724,2000,1,0,234,501,NULL),
(1725,2000,1,0,234,502,NULL),
(1726,2000,1,0,234,503,NULL),
(1727,2000,1,0,234,504,NULL),
(1728,2000,1,0,234,505,NULL),
(1729,2000,1,0,234,506,NULL),
(1735,750,1,0,250,509,NULL),
(1736,2000,1,0,234,531,NULL),
(1737,5900,1,0,235,532,NULL),
(1738,3000,1,0,232,532,NULL),
(1739,4000,1,0,231,533,NULL),
(1740,750,1,0,250,534,NULL),
(1741,750,1,0,250,534,NULL),
(1742,750,1,0,250,535,NULL),
(1743,750,1,0,250,536,NULL),
(1744,1000,1,0,251,537,NULL),
(1745,1000,1,0,251,537,NULL),
(1746,750,1,0,250,538,NULL),
(1747,750,1,0,250,539,NULL),
(1748,750,1,0,250,540,NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table devolucion
-- 

/*!40000 ALTER TABLE `devolucion` DISABLE KEYS */;
INSERT INTO `devolucion`(`id`,`producto_id`,`cantidad`,`fecha`,`monto`) VALUES
(1,250,1.00,'2018-05-12 19:42:09',750.00),
(2,250,1.00,'2018-05-12 19:45:43',750.00),
(3,250,1.00,'2018-05-12 20:06:00',750.00),
(4,250,1.00,'2018-05-12 20:12:07',750.00),
(5,250,1.00,'2018-05-12 20:12:08',750.00),
(6,250,1.00,'2018-05-12 20:17:49',750.00),
(7,250,1.00,'2018-05-12 20:20:34',750.00),
(8,250,1.00,'2018-05-12 20:20:50',750.00),
(9,250,1.00,'2018-05-12 20:23:15',750.00),
(10,251,1.00,'2018-05-13 13:42:46',1000.00);
/*!40000 ALTER TABLE `devolucion` ENABLE KEYS */;

-- 
-- Definition of envolturas
-- 

DROP TABLE IF EXISTS `envolturas`;
CREATE TABLE IF NOT EXISTS `envolturas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  `costo_adicional` int(11) DEFAULT NULL,
  `producto_id` int(11) DEFAULT NULL,
  `para_handroll` tinyint(1) DEFAULT '0',
  `para_superhandroll` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `envoltura_producto_ibfk_1_idx` (`producto_id`),
  CONSTRAINT `envoltura_producto_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table envolturas
-- 

/*!40000 ALTER TABLE `envolturas` DISABLE KEYS */;
INSERT INTO `envolturas`(`id`,`nombre`,`costo_adicional`,`producto_id`,`para_handroll`,`para_superhandroll`) VALUES
(1,'NORI',0,NULL,1,1),
(2,'PALTA',0,NULL,0,0),
(3,'QUESO CREMA',0,NULL,0,0),
(4,'SIN NORI',0,NULL,1,0),
(5,'FURAY',0,NULL,1,1),
(7,'CALIFORNIA',0,NULL,1,1),
(8,'CIBOULETTE',0,NULL,0,0);
/*!40000 ALTER TABLE `envolturas` ENABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table gastos
-- 

/*!40000 ALTER TABLE `gastos` DISABLE KEYS */;
INSERT INTO `gastos`(`id`,`detalle`,`monto`,`fecha`) VALUES
(41,'gas',12990.00,'2018-04-24 17:48:41'),
(42,'BOLSA BASURA',1000.00,'2018-04-26 18:27:03');
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
) ENGINE=InnoDB AUTO_INCREMENT=164 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table jornadas
-- 

/*!40000 ALTER TABLE `jornadas` DISABLE KEYS */;
INSERT INTO `jornadas`(`id`,`fecha_apertura`,`fecha_cierre`,`especial`,`mensaje`,`ingresos`,`usuario_id`,`caja_inicial_efectivo`) VALUES
(156,'2018-04-23 23:30:46','2018-04-24 16:42:17',0,'',0,22,20000),
(157,'2018-04-24 17:18:36','2018-04-26 17:55:18',0,'',0,47,0),
(158,'2018-04-26 17:56:04','2018-04-26 18:40:13',0,'',0,47,0),
(159,'2018-04-26 18:40:23','2018-05-02 18:02:57',0,'',0,47,20000),
(160,'2018-05-02 18:03:56','2018-05-02 18:05:54',0,'',0,22,0),
(161,'2018-05-02 18:06:26','2018-05-02 18:11:25',0,'',0,22,0),
(162,'2018-05-02 18:11:44','2018-05-02 18:27:21',0,'',0,22,0),
(163,'2018-05-02 18:27:39',NULL,0,'',0,22,0);
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
-- Definition of medio_pago
-- 

DROP TABLE IF EXISTS `medio_pago`;
CREATE TABLE IF NOT EXISTS `medio_pago` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table medio_pago
-- 

/*!40000 ALTER TABLE `medio_pago` DISABLE KEYS */;
INSERT INTO `medio_pago`(`id`,`nombre`) VALUES
(1,'Efectivo'),
(2,'Trans Bank'),
(3,'Junaeb'),
(4,'Otro');
/*!40000 ALTER TABLE `medio_pago` ENABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table mermas
-- 

/*!40000 ALTER TABLE `mermas` DISABLE KEYS */;
INSERT INTO `mermas`(`id`,`producto_id`,`cantidad`,`fecha`) VALUES
(1,250,1.00,'2018-05-12 19:47:19'),
(2,250,2.00,'2018-05-12 19:48:02'),
(3,250,1.00,'2018-05-12 20:21:25');
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
(2,'1',0,1,NULL,NULL),
(3,'2',1,1,NULL,NULL),
(4,'3',0,1,NULL,NULL),
(5,'4',1,1,NULL,NULL),
(6,'5',1,1,NULL,NULL),
(7,'6',1,1,NULL,NULL),
(8,'7',1,1,NULL,NULL),
(9,'8',0,1,NULL,NULL),
(10,'9',1,1,NULL,NULL),
(11,'10',1,1,NULL,NULL),
(12,'11',0,2,NULL,NULL),
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
  CONSTRAINT `pedidos_ibfk_2` FOREIGN KEY (`mesa_id`) REFERENCES `mesas` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table pedidos
-- 

/*!40000 ALTER TABLE `pedidos` DISABLE KEYS */;
INSERT INTO `pedidos`(`id`,`fecha`,`mensaje`,`usuario_id`,`mesa_id`,`pagado`) VALUES
(22,'2018-04-05 15:03:57','Actualizado',NULL,2,NULL),
(23,'2018-04-05 15:05:53','mensaje',NULL,4,NULL),
(24,'2018-04-11 00:37:23','mensaje',NULL,2,NULL),
(25,'2018-04-11 14:27:11','mensaje',NULL,2,NULL),
(27,'2018-04-11 14:31:52','mensaje',NULL,12,NULL),
(28,'2018-04-11 15:45:50','mensaje',NULL,9,NULL);
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
  `precio` int(11) NOT NULL DEFAULT '0',
  `nota` varchar(50) DEFAULT NULL,
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
  `font_size` int(11) NOT NULL DEFAULT '20',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table preparaciones
-- 

/*!40000 ALTER TABLE `preparaciones` DISABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table producto_compra
-- 

/*!40000 ALTER TABLE `producto_compra` DISABLE KEYS */;
INSERT INTO `producto_compra`(`id`,`compra_id`,`producto_id`,`cantidad_disponible`,`costo_unitario`,`cantidad_compra`,`activa`) VALUES
(1,1,250,8.00,2000.00,10.00,1),
(2,2,251,9.00,600.00,10.00,1),
(3,2,250,20.00,200.00,20.00,1);
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
  `precio` int(11) NOT NULL DEFAULT '0',
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
  `sector_impresion_id` int(11) DEFAULT NULL,
  `tipo_itemventa_id` int(11) DEFAULT NULL,
  `z-index` int(11) NOT NULL DEFAULT '0',
  `es_handroll` tinyint(1) DEFAULT '0',
  `es_shawarma` tinyint(1) DEFAULT '0',
  `es_coccion` tinyint(1) DEFAULT '0',
  `es_multiple_tamano` tinyint(1) DEFAULT '0',
  `es_envoltura` tinyint(1) DEFAULT '0',
  `es_tabla` tinyint(1) DEFAULT NULL,
  `cantidad_rollos_tabla` int(11) DEFAULT NULL,
  `es_superhandroll` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `nombre_UNIQUE` (`nombre`),
  KEY `productos_ibfk_1_idx` (`sub_categoria_id`),
  KEY `productos_ibkf_2_idx` (`proveedor_id`),
  KEY `productos_ibkf_3_idx` (`sector_impresion_id`),
  KEY `productos_ibfk_4_idx` (`tipo_itemventa_id`),
  CONSTRAINT `productos_ibfk_1` FOREIGN KEY (`sub_categoria_id`) REFERENCES `subcategorias` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `productos_ibfk_4` FOREIGN KEY (`tipo_itemventa_id`) REFERENCES `tipo_itemventa` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `productos_ibkf_2` FOREIGN KEY (`proveedor_id`) REFERENCES `proveedores` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `productos_ibkf_3` FOREIGN KEY (`sector_impresion_id`) REFERENCES `sector_impresion` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=253 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table productos
-- 

/*!40000 ALTER TABLE `productos` DISABLE KEYS */;
INSERT INTO `productos`(`id`,`nombre`,`codigo_barras`,`precio`,`retornable`,`favorito`,`puntos_cantidad`,`imagen`,`sub_categoria_id`,`proveedor_id`,`contiene_agregado`,`solo_venta`,`solo_compra`,`preparado_especial`,`sector_impresion_id`,`tipo_itemventa_id`,`z-index`,`es_handroll`,`es_shawarma`,`es_coccion`,`es_multiple_tamano`,`es_envoltura`,`es_tabla`,`cantidad_rollos_tabla`,`es_superhandroll`) VALUES
(231,'ROLLO CARTA','',4000,0,1,0,'ROLLO CARTA.png',25,NULL,0,1,0,0,2,3,1,0,0,0,1,1,NULL,NULL,0),
(232,'SUPER HANDROLL','',3000,0,1,0,'HANDROLL_2.png',25,NULL,0,1,0,0,2,3,1,0,0,0,1,0,NULL,NULL,1),
(234,'HANDROLL','',2000,0,1,0,'HANDROLL_2.png',25,NULL,0,1,0,0,2,3,1,1,0,0,1,0,NULL,NULL,0),
(235,'TABLA x 20','',5900,0,1,0,'tabla-20.jpg',50,NULL,0,1,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,1,2,0),
(236,'TABLA X 30','',8500,0,1,0,'tabla-30.jpg',50,NULL,0,1,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,1,3,0),
(237,'TABLA X 40','',10900,0,1,0,'tabla-40.jpg',50,NULL,0,1,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,1,4,0),
(238,'TABLA X 50','',12500,0,1,0,'tabla-50.jpg',50,NULL,0,1,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,1,5,0),
(239,'TABLA X 60','',13900,0,1,0,'tabla-60.jpg',50,NULL,0,1,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,1,6,0),
(240,'TABLA X 100','',25000,0,1,0,'tabla-100.jpg',50,NULL,0,1,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,1,10,0),
(241,'PROMO X 20','',4900,0,1,0,'tabla-20-promo.jpg',51,NULL,0,1,0,0,2,3,1,0,0,0,0,0,0,2,0),
(242,'PROMO X 40','',8900,0,1,0,'tabla-40-promo.jpg',51,NULL,0,1,0,0,2,3,1,0,0,0,0,0,0,4,0),
(243,'PROMO x 60','',12000,0,1,0,'tabla-60-promo.jpg',51,NULL,0,1,0,0,2,3,1,0,0,0,0,0,0,6,0),
(250,'AGUA MINERAL','',750,0,1,0,'',54,NULL,0,0,0,0,2,3,1,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL),
(251,'test','',1000,0,0,0,'',25,NULL,0,0,0,0,4,3,0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
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
-- Definition of salsa
-- 

DROP TABLE IF EXISTS `salsa`;
CREATE TABLE IF NOT EXISTS `salsa` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table salsa
-- 

/*!40000 ALTER TABLE `salsa` DISABLE KEYS */;
INSERT INTO `salsa`(`id`,`nombre`) VALUES
(1,'SOYA'),
(2,'UNAGI');
/*!40000 ALTER TABLE `salsa` ENABLE KEYS */;

-- 
-- Definition of sector_impresion
-- 

DROP TABLE IF EXISTS `sector_impresion`;
CREATE TABLE IF NOT EXISTS `sector_impresion` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `impresora` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table sector_impresion
-- 

/*!40000 ALTER TABLE `sector_impresion` DISABLE KEYS */;
INSERT INTO `sector_impresion`(`id`,`nombre`,`impresora`) VALUES
(1,'BAR','IMPRIMIR EN TXT'),
(2,'COCINA','IMPRIMIR EN TXT'),
(4,'CAJA','IMPRIMIR EN TXT');
/*!40000 ALTER TABLE `sector_impresion` ENABLE KEYS */;

-- 
-- Definition of sectores
-- 

DROP TABLE IF EXISTS `sectores`;
CREATE TABLE IF NOT EXISTS `sectores` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  `seleccionable` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table sectores
-- 

/*!40000 ALTER TABLE `sectores` DISABLE KEYS */;
INSERT INTO `sectores`(`id`,`nombre`,`seleccionable`) VALUES
(40,'MENU',1),
(42,'TABLA',1),
(43,'PROMO',1),
(45,'COCIN',1),
(46,'BEBIDA',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=118 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table stock_pr
-- 

/*!40000 ALTER TABLE `stock_pr` DISABLE KEYS */;
INSERT INTO `stock_pr`(`id`,`producto_id`,`entrada`,`salida`,`ajuste`) VALUES
(107,234,0.00,28.00,0.00),
(108,231,0.00,3.00,0.00),
(109,235,0.00,2.00,0.00),
(111,236,0.00,1.00,0.00),
(113,242,0.00,1.00,0.00),
(115,250,30.00,9.00,0.00),
(116,232,0.00,1.00,0.00),
(117,251,10.00,1.00,-4.00);
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
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table subcategorias
-- 

/*!40000 ALTER TABLE `subcategorias` DISABLE KEYS */;
INSERT INTO `subcategorias`(`id`,`nombre`) VALUES
(54,'BEBIDA'),
(53,'ENTRADA'),
(25,'MENU'),
(52,'OTRO'),
(51,'PROMO'),
(50,'TABLA');
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
-- Definition of tipo_itemventa
-- 

DROP TABLE IF EXISTS `tipo_itemventa`;
CREATE TABLE IF NOT EXISTS `tipo_itemventa` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table tipo_itemventa
-- 

/*!40000 ALTER TABLE `tipo_itemventa` DISABLE KEYS */;
INSERT INTO `tipo_itemventa`(`id`,`nombre`) VALUES
(3,'plato fondo'),
(5,'otro');
/*!40000 ALTER TABLE `tipo_itemventa` ENABLE KEYS */;

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
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table usuarios
-- 

/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios`(`id`,`nombre`,`nombre_usuario`,`pass`,`tipo`,`config_id`,`imagen`,`favorito`) VALUES
(22,'Chris','','1','A',1,'',1),
(47,'Caja','Caja','12','C',1,'',1);
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
  `cobro_extra` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `producto_id` (`producto_id`),
  KEY `jornada_id` (`jornada_id`),
  KEY `ventas_jornada_ibfk_2` (`promo_id`),
  CONSTRAINT `ventas_jornada_ibfk_1` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ventas_jornada_ibfk_3` FOREIGN KEY (`jornada_id`) REFERENCES `jornadas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `ventas_jornada_ibfk_4` FOREIGN KEY (`promo_id`) REFERENCES `promociones` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=265 DEFAULT CHARSET=utf8;

-- 
-- Dumping data for table ventas_jornada
-- 

/*!40000 ALTER TABLE `ventas_jornada` DISABLE KEYS */;
INSERT INTO `ventas_jornada`(`id`,`producto_id`,`promo_id`,`jornada_id`,`cantidad`,`cobro_extra`) VALUES
(247,234,NULL,156,5.00,0),
(248,231,NULL,156,1.00,0),
(249,235,NULL,156,1.00,0),
(251,236,NULL,157,1.00,0),
(253,242,NULL,157,1.00,0),
(254,234,NULL,157,22.00,0),
(255,231,NULL,157,1.00,0),
(258,250,NULL,158,1.00,0),
(259,234,NULL,163,1.00,0),
(260,235,NULL,163,1.00,0),
(261,232,NULL,163,1.00,0),
(262,231,NULL,163,1.00,0),
(263,250,NULL,163,7.00,0),
(264,251,NULL,163,2.00,0);
/*!40000 ALTER TABLE `ventas_jornada` ENABLE KEYS */;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;


-- Dump completed on 2018-05-14 11:47:32
-- Total time: 0:0:0:0:153 (d:h:m:s:ms)
