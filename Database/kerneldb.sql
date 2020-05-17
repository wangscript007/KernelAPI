/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : kerneldb

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2020-05-17 22:49:47
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for sysdict
-- ----------------------------
DROP TABLE IF EXISTS `sysdict`;
CREATE TABLE `sysdict` (
  `DictID` varchar(40) NOT NULL COMMENT '字典ID',
  `DictCode` varchar(50) DEFAULT NULL COMMENT '字典编码',
  `DictName` varchar(150) DEFAULT NULL COMMENT '字典名称',
  `IsEnabled` varchar(40) DEFAULT NULL COMMENT '是否启用',
  `CreateBy` varchar(40) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateBy` varchar(40) DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`DictID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='系统字典';

-- ----------------------------
-- Records of sysdict
-- ----------------------------

-- ----------------------------
-- Table structure for sysdictitem
-- ----------------------------
DROP TABLE IF EXISTS `sysdictitem`;
CREATE TABLE `sysdictitem` (
  `ItemID` varchar(40) NOT NULL COMMENT '字典ID',
  `DictID` varchar(40) DEFAULT NULL COMMENT '字典ID',
  `DictCode` varchar(50) DEFAULT NULL COMMENT '字典编码',
  `ItemCode` varchar(50) DEFAULT NULL COMMENT '字典项编码',
  `ItemName` varchar(150) DEFAULT NULL COMMENT '字典项名称',
  `ItemPID` varchar(40) DEFAULT NULL COMMENT '字典项PID',
  `CreateBy` varchar(40) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateBy` varchar(40) DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  `IsEnabled` varchar(40) DEFAULT NULL COMMENT '是否启用',
  PRIMARY KEY (`ItemID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='系统字典项';

-- ----------------------------
-- Records of sysdictitem
-- ----------------------------

-- ----------------------------
-- Table structure for sysmodule
-- ----------------------------
DROP TABLE IF EXISTS `sysmodule`;
CREATE TABLE `sysmodule` (
  `ModID` varchar(40) NOT NULL COMMENT '模块ID',
  `ModName` varchar(50) DEFAULT NULL COMMENT '模块名称',
  `NavUrl` varchar(100) DEFAULT NULL COMMENT '导航链接',
  `Image` varchar(100) DEFAULT NULL COMMENT '图标',
  `Target` varchar(50) DEFAULT NULL COMMENT '目标位置',
  `ModType` varchar(50) DEFAULT NULL COMMENT '模块分类（home、logo、menu）',
  `ModPID` varchar(40) DEFAULT NULL COMMENT '父模块ID',
  `CreateBy` varchar(40) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateBy` varchar(40) DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  `IsEnabled` varchar(40) DEFAULT NULL COMMENT '是否启用',
  `SortKey` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`ModID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='系统模块';

-- ----------------------------
-- Records of sysmodule
-- ----------------------------
INSERT INTO `sysmodule` VALUES ('3da527f2983a11eaa3ddb06ebfba413f', '首页', 'page/welcome-1.html?t=1', '', null, 'home', null, null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('cdb37e13983a11eaa3ddb06ebfba413f', 'KernelWeb', '', 'images/logo.png', null, 'logo', null, null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('0173d9a5983b11eaa3ddb06ebfba413f', '常规管理', null, 'fa fa-address-book', '_self', 'menu', null, null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('feceaf67983d11eaa3ddb06ebfba413f', '组件管理', null, 'fa fa-lemon-o', '_self', 'menu', null, null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('352e3e6b983e11eaa3ddb06ebfba413f', '其它管理', null, 'fa fa-slideshare', '_self', 'menu', null, null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('58c0d503983e11eaa3ddb06ebfba413f', '主页模板', null, 'fa fa-home', '_self', 'menu', '0173d9a5983b11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('75ee77f5983e11eaa3ddb06ebfba413f', '主页一', 'page/welcome-1.html', 'fa fa-tachometer', '_self', 'menu', '58c0d503983e11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('cd3c597d984811eaa3ddb06ebfba413f', '主页二', 'page/welcome-2.html', 'fa fa-tachometer', '_self', 'menu', '75ee77f5983e11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('861ae858984911eaa3ddb06ebfba413f', '主页三', 'page/welcome-3.html', 'fa fa-tachometer', '_self', 'menu', 'cd3c597d984811eaa3ddb06ebfba413f', null, null, null, null, '1', null);

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser` (
  `UserID` varchar(40) NOT NULL COMMENT '用户ID',
  `LoginID` varchar(50) DEFAULT NULL COMMENT '登录帐号',
  `UserName` varchar(50) DEFAULT NULL COMMENT '用户名',
  `UserNameFirstLetter` varchar(50) DEFAULT NULL COMMENT '拼音首字母',
  `LoginPwd` varchar(200) DEFAULT NULL COMMENT '密码',
  `IsLocked` char(1) DEFAULT '0' COMMENT '是否锁定：0、未锁定，1、锁定',
  `UserJob` varchar(50) DEFAULT NULL COMMENT '职务',
  `DictGender` varchar(40) DEFAULT NULL COMMENT '性别：M、男，F、女',
  `Birthday` datetime DEFAULT NULL COMMENT '生日',
  `TelPhone` varchar(50) DEFAULT NULL COMMENT '电话',
  `MobilePhone` varchar(50) DEFAULT NULL COMMENT '手机',
  `Email` varchar(100) DEFAULT NULL COMMENT '邮件',
  `Description` varchar(300) DEFAULT NULL COMMENT '备注',
  `UserPhoto` varchar(100) DEFAULT NULL COMMENT '头像',
  `DictIsActive` varchar(40) DEFAULT '1' COMMENT '是否启用，0、禁用，1、启用',
  `CreateBy` varchar(40) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateBy` varchar(40) DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`UserID`) USING BTREE,
  UNIQUE KEY `sysuser_LoginID_Unique` (`LoginID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf16 ROW_FORMAT=COMPACT COMMENT='用户信息';

-- ----------------------------
-- Records of sysuser
-- ----------------------------
INSERT INTO `sysuser` VALUES ('2164ab759f8e4d56bf9f7f479dcd0aa1', 'admin', '超级管理员', '', '4u3wBbH//4VCJwfuX/s1UA==', '0', '系统管理员', 'F', '1970-01-01 00:00:00', '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-17 01:39:38', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-17 01:39:38');

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

-- ----------------------------
-- Records of sys_files
-- ----------------------------
INSERT INTO `sys_files` VALUES ('070a3b234d0f46e0949733812bf668dc', null, '123', '新建文本文档', '.txt', '2020/05/16/123/', '2020/05/16/123/070a3b234d0f46e0949733812bf668dc.txt', null, '', '1', '1', '7895058', '2020-05-16', '', '2020-05-16', '');
INSERT INTO `sys_files` VALUES ('9960cbef0f80461ea5e9020c22f8bd8a', null, '123', 'tns', '.sql', '2020/05/16/123/', '2020/05/16/123/9960cbef0f80461ea5e9020c22f8bd8a.sql', null, '', '1', '1', '7895058', '2020-05-16', '', '2020-05-16', '');
INSERT INTO `sys_files` VALUES ('6efbc8b788254ad8a2472b03c306d0a2', null, '1122', '新建文本文档', '.txt', '2020/04/25/1122/', '2020/04/25/1122/6efbc8b788254ad8a2472b03c306d0a2.txt', null, '', '1', '1', '20', '2020-04-25', '', '2020-04-25', '');
INSERT INTO `sys_files` VALUES ('4cff6b6ba5eb45eea2197945b3b4bdc2', null, '1122', 'XtraReport1', '.pdf', '2020/04/25/1122/', '2020/04/25/1122/4cff6b6ba5eb45eea2197945b3b4bdc2.pdf', null, '', '1', '1', '1630378', '2020-04-25', '', '2020-04-25', '');
INSERT INTO `sys_files` VALUES ('b62d89a521054d4dbbcf1ed3e3d9f33f', null, '1122', 'FileZilla', '.rar', '2020/04/25/1122/', '2020/04/25/1122/b62d89a521054d4dbbcf1ed3e3d9f33f.rar', null, '', '1', '1', '11514505', '2020-04-25', '', '2020-04-25', '');
INSERT INTO `sys_files` VALUES ('bd53def4e6244337800864083d636af7', null, '1122', '新建 Microsoft Excel 工作表', '.xlsx', '2020/04/25/1122/', '2020/04/25/1122/bd53def4e6244337800864083d636af7.xlsx', null, '', '1', '1', '8692', '2020-04-25', '', '2020-04-25', '');

-- ----------------------------
-- View structure for v_guid
-- ----------------------------
DROP VIEW IF EXISTS `v_guid`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_guid` AS select replace(uuid(),'-','') AS `guid` ;
