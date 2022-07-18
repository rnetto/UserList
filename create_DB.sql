CREATE DATABASE  IF NOT EXISTS `db_userlist` ;
USE `db_userlist`;

DROP TABLE IF EXISTS `usuario`;

CREATE TABLE `usuario` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` text NOT NULL,
  `Apelido` text,
  `Endereco` text,
  `Telefone` text NOT NULL,
  `DataCadastro` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
;
