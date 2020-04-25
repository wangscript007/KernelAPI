/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : kerneldb

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2020-04-25 13:00:18
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for sys_files
-- ----------------------------
DROP TABLE IF EXISTS `sys_files`;
CREATE TABLE `sys_files` (
  `ATTACH_ID` varchar(50) NOT NULL,
  `ATTACH_REMARKS` varchar(255) DEFAULT NULL,
  `ATTACH_BIZ_ID` varchar(50) DEFAULT NULL,
  `ATTACH_FILENAME` varchar(255) DEFAULT NULL,
  `ATTACH_FILETYPE` varchar(50) DEFAULT NULL,
  `ATTACH_FILEPATH` varchar(255) DEFAULT NULL,
  `ATTACH_PHYADDRESS` varchar(255) DEFAULT NULL,
  `ATTACH_BIZTABLE` varchar(50) DEFAULT NULL,
  `ATTACH_BIZNAME` varchar(50) DEFAULT NULL,
  `DICT_IS_ENABLED` varchar(50) DEFAULT '1',
  `DICT_INPUT_INTERFACE` varchar(50) DEFAULT '1',
  `ATTACH_FILESIZE` decimal(12,0) DEFAULT NULL,
  `OP_CREATE_DATE` date DEFAULT NULL,
  `OP_CREATE_USER` varchar(255) DEFAULT NULL,
  `OP_MODIFY_DATE` date DEFAULT NULL,
  `OP_MODIFY_USER` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ATTACH_ID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16;
