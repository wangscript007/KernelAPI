/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50553
Source Host           : localhost:3306
Source Database       : kerneldb

Target Server Type    : MYSQL
Target Server Version : 50553
File Encoding         : 65001

Date: 2020-05-26 22:57:41
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
INSERT INTO `sysdict` VALUES ('a5609c3b99db11eab025b06ebfba413f', 'DictIsActive', '状态', '1', null, null, null, null);

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
  `SortKey` decimal(10,2) DEFAULT NULL COMMENT '排序码',
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
INSERT INTO `sysdictitem` VALUES ('ca27881399db11eab025b06ebfba413f', 'a5609c3b99db11eab025b06ebfba413f', 'DictIsActive', '0', '作废', null, null, null, null, null, null, '1');
INSERT INTO `sysdictitem` VALUES ('97df6c0b99dc11eab025b06ebfba413f', 'a5609c3b99db11eab025b06ebfba413f', 'DictIsActive', '1', '在用', null, null, null, null, null, null, '1');

-- ----------------------------
-- Table structure for sysfunc
-- ----------------------------
DROP TABLE IF EXISTS `sysfunc`;
CREATE TABLE `sysfunc` (
  `FuncID` varchar(40) NOT NULL COMMENT '功能ID',
  `ModID` varchar(40) NOT NULL COMMENT '菜单ID',
  `FuncCode` varchar(50) DEFAULT NULL COMMENT '功能编码',
  `FuncName` varchar(50) DEFAULT NULL COMMENT '功能名称',
  `ApiName` varchar(255) DEFAULT NULL COMMENT 'api名称',
  PRIMARY KEY (`FuncID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='系统功能';

-- ----------------------------
-- Records of sysfunc
-- ----------------------------
INSERT INTO `sysfunc` VALUES ('fb4206799f5811eaab5cb06ebfba413f', '3da527f2983a11eaa3ddb06ebfba413f', 'PermNav', '查询导航菜单', 'WebAPI.Areas.System.Controllers.SysModuleController.GetSysModuleInit_V1_0 (WebAPI)');
INSERT INTO `sysfunc` VALUES ('0750d0089f5e11eaab5cb06ebfba413f', '3da527f2983a11eaa3ddb06ebfba413f', 'PermTree', '查询授权信息', 'WebAPI.Areas.System.Controllers.SysModuleController.GetPermTree_V1_0 (WebAPI)');

-- ----------------------------
-- Table structure for sysfuncperm
-- ----------------------------
DROP TABLE IF EXISTS `sysfuncperm`;
CREATE TABLE `sysfuncperm` (
  `RoleID` varchar(40) NOT NULL COMMENT '角色',
  `ModID` varchar(40) NOT NULL COMMENT '菜单ID',
  `FuncID` varchar(40) NOT NULL COMMENT '功能ID',
  `FuncCode` varchar(50) DEFAULT NULL COMMENT '功能编码',
  `FuncName` varchar(50) DEFAULT NULL COMMENT '功能名称',
  `ApiName` varchar(255) DEFAULT NULL COMMENT 'api名称',
  `CreateBy` varchar(40) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`RoleID`,`ModID`,`FuncID`),
  KEY `ApiName_Normal` (`ApiName`(250))
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='系统功能权限';

-- ----------------------------
-- Records of sysfuncperm
-- ----------------------------
INSERT INTO `sysfuncperm` VALUES ('e897ff4f9d9511eaa242b06ebfba413f', '3da527f2983a11eaa3ddb06ebfba413f', 'fb4206799f5811eaab5cb06ebfba413f', 'PermNav', '查询导航菜单', 'WebAPI.Areas.System.Controllers.SysModuleController.GetSysModuleInit_V1_0 (WebAPI)', null, null);
INSERT INTO `sysfuncperm` VALUES ('e897ff4f9d9511eaa242b06ebfba413f', '3da527f2983a11eaa3ddb06ebfba413f', '0750d0089f5e11eaab5cb06ebfba413f', 'PermTree', '查询授权信息', 'WebAPI.Areas.System.Controllers.SysModuleController.GetPermTree_V1_0 (WebAPI)', '', '0000-00-00 00:00:00');

-- ----------------------------
-- Table structure for sysmenuperm
-- ----------------------------
DROP TABLE IF EXISTS `sysmenuperm`;
CREATE TABLE `sysmenuperm` (
  `RoleID` varchar(40) NOT NULL,
  `ModID` varchar(40) DEFAULT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16;

-- ----------------------------
-- Records of sysmenuperm
-- ----------------------------
INSERT INTO `sysmenuperm` VALUES ('e897ff4f9d9511eaa242b06ebfba413f', '3da527f2983a11eaa3ddb06ebfba413f');

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
  `IsEnabled` varchar(40) DEFAULT '1' COMMENT '是否启用',
  `SortKey` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`ModID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='系统模块';

-- ----------------------------
-- Records of sysmodule
-- ----------------------------
INSERT INTO `sysmodule` VALUES ('3da527f2983a11eaa3ddb06ebfba413f', '首页', 'page/welcome-1.html?t=1', '', null, 'home', '-1', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('cdb37e13983a11eaa3ddb06ebfba413f', 'KernelWeb', '', 'images/logo.png', null, 'logo', '-1', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('0173d9a5983b11eaa3ddb06ebfba413f', '常规管理', null, 'fa fa-address-book', '_self', 'menu', '-1', null, null, null, null, '1', '20.00');
INSERT INTO `sysmodule` VALUES ('feceaf67983d11eaa3ddb06ebfba413f', '系统管理', null, 'fa fa-lemon-o', '_self', 'menu', '-1', null, null, null, null, '1', '30.00');
INSERT INTO `sysmodule` VALUES ('352e3e6b983e11eaa3ddb06ebfba413f', '其它管理', null, 'fa fa-slideshare', '_self', 'menu', '-1', null, null, null, null, '1', '40.00');
INSERT INTO `sysmodule` VALUES ('58c0d503983e11eaa3ddb06ebfba413f', '主页模板', null, 'fa fa-home', '_self', 'menu', '0173d9a5983b11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('75ee77f5983e11eaa3ddb06ebfba413f', '主页一', 'page/welcome-1.html', 'fa fa-tachometer', '_self', 'menu', '58c0d503983e11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('cd3c597d984811eaa3ddb06ebfba413f', '主页二', 'page/welcome-2.html', 'fa fa-tachometer', '_self', 'menu', '75ee77f5983e11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('861ae858984911eaa3ddb06ebfba413f', '主页三', 'page/welcome-3.html', 'fa fa-tachometer', '_self', 'menu', 'cd3c597d984811eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('8714f687991311eaa311b06ebfba413f', '用户管理', 'pages/user-manage.html', 'fa fa-tachometer', '_self', 'menu', 'feceaf67983d11eaa3ddb06ebfba413f', null, null, null, null, '1', null);
INSERT INTO `sysmodule` VALUES ('5b0bdf449dc711eaa242b06ebfba413f', '功能授权', 'pages/func-permissions.html', 'fa fa-tachometer', null, 'menu', 'feceaf67983d11eaa3ddb06ebfba413f', null, null, null, null, '1', null);

-- ----------------------------
-- Table structure for sysrole
-- ----------------------------
DROP TABLE IF EXISTS `sysrole`;
CREATE TABLE `sysrole` (
  `RoleID` varchar(40) NOT NULL COMMENT '角色ID',
  `RoleName` varchar(50) DEFAULT NULL COMMENT '角色名称',
  `DictIsActive` varchar(40) DEFAULT '1' COMMENT '状态，0、作废，1、在用',
  `Description` varchar(300) DEFAULT NULL COMMENT '备注',
  `CreateBy` varchar(40) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UpdateBy` varchar(40) DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`RoleID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='角色信息';

-- ----------------------------
-- Records of sysrole
-- ----------------------------
INSERT INTO `sysrole` VALUES ('e897ff4f9d9511eaa242b06ebfba413f', '系统管理员', '1', null, null, null, null, null);
INSERT INTO `sysrole` VALUES ('d11e08bf9da111eaa242b06ebfba413f', '运维人员', '1', null, null, null, null, null);

-- ----------------------------
-- Table structure for sysrolerelation
-- ----------------------------
DROP TABLE IF EXISTS `sysrolerelation`;
CREATE TABLE `sysrolerelation` (
  `UserID` varchar(40) NOT NULL COMMENT '用户ID',
  `RoleID` varchar(40) NOT NULL COMMENT '角色ID',
  PRIMARY KEY (`UserID`,`RoleID`)
) ENGINE=MyISAM DEFAULT CHARSET=utf16 COMMENT='用户角色关联';

-- ----------------------------
-- Records of sysrolerelation
-- ----------------------------
INSERT INTO `sysrolerelation` VALUES ('2164ab759f8e4d56bf9f7f479dcd0aa1', 'd11e08bf9da111eaa242b06ebfba413f');
INSERT INTO `sysrolerelation` VALUES ('2164ab759f8e4d56bf9f7f479dcd0aa1', 'e897ff4f9d9511eaa242b06ebfba413f');

-- ----------------------------
-- Table structure for sysuser
-- ----------------------------
DROP TABLE IF EXISTS `sysuser`;
CREATE TABLE `sysuser` (
  `UserID` varchar(40) NOT NULL COMMENT '用户ID',
  `LoginID` varchar(50) DEFAULT NULL COMMENT '登录帐号',
  `UserName` varchar(50) DEFAULT NULL COMMENT '姓名',
  `UserNameFirstLetter` varchar(50) DEFAULT NULL COMMENT '拼音首字母',
  `LoginPwd` varchar(200) DEFAULT NULL COMMENT '密码',
  `IsLocked` char(1) DEFAULT '0' COMMENT '是否锁定：0、未锁定，1、锁定',
  `UserJob` varchar(50) DEFAULT NULL COMMENT '职务',
  `JobNumber` varchar(50) DEFAULT NULL COMMENT '工号',
  `Birthday` datetime DEFAULT NULL COMMENT '生日',
  `TelPhone` varchar(50) DEFAULT NULL COMMENT '电话',
  `MobilePhone` varchar(50) DEFAULT NULL COMMENT '手机',
  `Email` varchar(100) DEFAULT NULL COMMENT '邮件',
  `Description` varchar(300) DEFAULT NULL COMMENT '备注',
  `UserPhoto` varchar(100) DEFAULT NULL COMMENT '头像',
  `DictIsActive` varchar(40) DEFAULT '1' COMMENT '状态，0、作废，1、在用',
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
INSERT INTO `sysuser` VALUES ('001aa72dba3f4e1786809ae4d4eb7e8a', 'ssss', '22222', '', '', '', '', '', null, '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:07:32', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:07:32');
INSERT INTO `sysuser` VALUES ('1ca239cd4dc34c98919b2c5728b30f41', 'dfdsf', 'dfdfd', '', '', '', '', '', '2020-05-14 00:00:00', '', '13423652565', '111@a.com', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 21:01:00', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 22:28:37');
INSERT INTO `sysuser` VALUES ('2164ab759f8e4d56bf9f7f479dcd0aa1', 'admin', '超级管理员', '', '4u3wBbH//4VCJwfuX/s1UA==', '0', '系统管理员', '12334', '1970-01-01 00:00:00', '', '15623522258', '', '备注一下', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-17 01:39:38', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-17 01:39:38');
INSERT INTO `sysuser` VALUES ('24addb5cc77949ad8d815bd6d1e95f46', 'sda', 'dsdsa', '', '', '', '', '', null, '', '', '', '', '', '0', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:44:46', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:44:46');
INSERT INTO `sysuser` VALUES ('5b0acc658b094f9f8260859e902d510f', '65765', '67676', '', '', '', '', '', null, '', '12545874589', '', '', '', '0', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:45:05', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 18:09:37');
INSERT INTO `sysuser` VALUES ('5ede822b09834aa483f9f39dcfdf1c55', 'ffff', 'ggg', '', '', '', '', '', null, '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:46:32', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:46:32');
INSERT INTO `sysuser` VALUES ('6bf841830832480b834678054b1b7b22', 'dfsdf', 'fdsfsf', '', '', '', '', '', null, '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:07:18', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:07:18');
INSERT INTO `sysuser` VALUES ('6f538463d14a42e1a2da5e83942e0dad', '222', '3333', '', '', '', '', '', null, '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 21:00:15', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 21:04:48');
INSERT INTO `sysuser` VALUES ('734bf62385a34ab2ab0200df6afbf14e', '555', '5656', '', '', '', '', '', null, '', '', '', '', '', '0', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:44:58', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:44:58');
INSERT INTO `sysuser` VALUES ('a2acc14f0c0b4b438333e3eec01634ce', 'fgfdg', 'hfhghgh', '', '', '', '', '', null, '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 15:45:11', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 22:28:31');
INSERT INTO `sysuser` VALUES ('aeb3ac399a4c4255a2ecc8c2d44136c7', 'fgdfg', 'dffgfg', '', '', '', '', '', null, '', '', '', '', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:00:45', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:00:45');
INSERT INTO `sysuser` VALUES ('af9686b01f31497abf4fd04065cbf58c', '888', '888', '', '', '', '9999', '', '2020-05-07 00:00:00', '123666', '12545896589', '', '12345454545\n4\n3222222222222222222222222222222222\n4\n32', '', '1', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-21 22:58:12', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 14:31:21');
INSERT INTO `sysuser` VALUES ('d54256d831ce468b929370849904dfc6', 'gfgffg', 'dfsdfsf', '', '', '', '', '', null, '', '', '', '', '', '0', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 16:07:22', '2164ab759f8e4d56bf9f7f479dcd0aa1', '2020-05-23 22:20:37');

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

-- ----------------------------
-- View structure for v_roleuser
-- ----------------------------
DROP VIEW IF EXISTS `v_roleuser`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_roleuser` AS select `a`.`RoleID` AS `RoleID`,`a`.`RoleName` AS `RoleName`,`c`.`UserID` AS `UserID`,`c`.`UserName` AS `UserName` from ((`sysrole` `a` join `sysrolerelation` `b` on((`a`.`RoleID` = `b`.`RoleID`))) join `sysuser` `c` on((`b`.`UserID` = `c`.`UserID`))) ;
