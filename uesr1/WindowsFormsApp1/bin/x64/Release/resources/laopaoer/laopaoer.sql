/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MySQL
 Source Server Version : 80031
 Source Host           : localhost:3306
 Source Schema         : laopaoer

 Target Server Type    : MySQL
 Target Server Version : 80031
 File Encoding         : 65001

 Date: 05/12/2022 01:33:38
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for object
-- ----------------------------
DROP TABLE IF EXISTS `object`;
CREATE TABLE `object`  (
  `objectname` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '物品名',
  `objectid` char(3) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '物品ID',
  `type` tinyint NOT NULL COMMENT '类型',
  `place` tinyint NOT NULL DEFAULT 0 COMMENT '位置',
  `state` tinyint NOT NULL COMMENT '状态',
  `lasttime` timestamp NOT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '最后处理时间',
  PRIMARY KEY (`objectid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '物品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of object
-- ----------------------------
INSERT INTO `object` VALUES ('测试', '000', 1, 0, 0, '2022-12-05 00:53:57');
INSERT INTO `object` VALUES ('显卡', '001', 1, 2, 0, '2022-12-05 00:32:03');
INSERT INTO `object` VALUES ('声卡', '002', 1, 11, 1, '2022-12-05 00:14:43');
INSERT INTO `object` VALUES ('CPU', '003', 0, 12, 1, '2022-12-05 00:14:43');
INSERT INTO `object` VALUES ('主板', '004', 0, 3, 1, '2022-12-05 00:14:43');
INSERT INTO `object` VALUES ('CPU', '005', 1, 4, 1, '2022-03-10 06:30:47');
INSERT INTO `object` VALUES ('内存', '006', 0, 5, 1, '2022-01-28 12:35:17');
INSERT INTO `object` VALUES ('硬盘', '007', 1, 6, 1, '2022-06-23 12:05:30');
INSERT INTO `object` VALUES ('内存', '008', 0, 7, 1, '2022-03-08 13:48:51');
INSERT INTO `object` VALUES ('硬盘', '009', 0, 8, 1, '2022-07-02 06:51:16');
INSERT INTO `object` VALUES ('内存', '010', 0, 9, 1, '2022-07-30 02:39:57');
INSERT INTO `object` VALUES ('鼠标', '011', 1, 10, 1, '2022-08-30 12:18:50');
INSERT INTO `object` VALUES ('网卡', '012', 1, 1, 0, '2022-12-05 00:25:08');
INSERT INTO `object` VALUES ('测试2', '015', 1, 7, 1, '2022-12-05 00:36:00');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user`  (
  `username` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '用户名',
  `userid` char(11) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '用户ID',
  `email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '邮箱',
  `power` tinyint NOT NULL DEFAULT 0 COMMENT '权限',
  `password` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT '密码',
  PRIMARY KEY (`userid`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci COMMENT = '用户' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('操作员', '11111111111', '11111111111@qq.com', 1, '123456');
INSERT INTO `user` VALUES ('顾立轩', '15817400084', 'louis.damore@yahoo.com', 0, '8iiu');
INSERT INTO `user` VALUES ('杨懿轩', '15836060911', 'krysten.mraz@hotmail.com', 0, 'Qh34S');
INSERT INTO `user` VALUES ('严弘文', '15879148696', 'ivory.blanda@yahoo.com', 0, '2K');
INSERT INTO `user` VALUES ('姚昊然', '17041957830', 'cathi.casper@hotmail.com', 0, 'M1VEI');
INSERT INTO `user` VALUES ('冯绍齐', '17074079619', 'linwood.batz@hotmail.com', 0, '1G1');
INSERT INTO `user` VALUES ('孔博文', '17085552715', 'marcos.erdman@yahoo.com', 0, 'XZtF');
INSERT INTO `user` VALUES ('曹胤祥', '17101016832', 'jimmie.dach@yahoo.com', 0, 'amWr');
INSERT INTO `user` VALUES ('程鹏飞', '17139877049', 'jenelle.schaden@gmail.com', 0, 'yF');
INSERT INTO `user` VALUES ('白笑愚', '17166278619', 'cherryl.heller@yahoo.com', 0, 'cW45n');
INSERT INTO `user` VALUES ('徐炫明', '17242207109', 'lesley.ledner@yahoo.com', 0, 'O7');
INSERT INTO `user` VALUES ('洪弘文', '17390309679', 'conrad.heidenreich@hotmail.com', 0, 'fDZA');
INSERT INTO `user` VALUES ('严伟泽', '17640896527', 'caleb.emard@hotmail.com', 0, 'Ho6L8');
INSERT INTO `user` VALUES ('姜明杰', '17701561702', 'joshua.ferry@gmail.com', 0, 'zVgj');
INSERT INTO `user` VALUES ('姜烨伟', '17789218868', 'herschel.lind@hotmail.com', 0, '7ExEb');
INSERT INTO `user` VALUES ('金博文', '17896394082', 'merlene.kessler@hotmail.com', 0, 'KR');
INSERT INTO `user` VALUES ('杨志泽', '18159057848', 'margarett.baumbach@yahoo.com', 0, 'sf');
INSERT INTO `user` VALUES ('沈峻熙', '18631314284', 'barrett.funk@gmail.com', 0, '72M');
INSERT INTO `user` VALUES ('管理员', '22222222222', '22222222222@qq.com', 1, '123456');
INSERT INTO `user` VALUES ('工程师', '33333333333', '33333333333@qq.com', 2, '123456');

SET FOREIGN_KEY_CHECKS = 1;
