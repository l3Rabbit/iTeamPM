/*
Navicat SQL Server Data Transfer

Source Server         : iTeamDB
Source Server Version : 120000
Source Host           : iteamspu.database.windows.net:1433
Source Database       : iTeamDB
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 120000
File Encoding         : 65001

Date: 2018-05-15 09:21:27
*/


-- ----------------------------
-- Table structure for iteam_comment
-- ----------------------------
DROP TABLE [dbo].[iteam_comment]
GO
CREATE TABLE [dbo].[iteam_comment] (
[task_id] int NOT NULL ,
[itemno] int NOT NULL ,
[comment] nvarchar(MAX) NULL ,
[add_user] int NOT NULL ,
[edit_user] int NULL ,
[add_dt] datetime NULL ,
[edit_dt] datetime NULL ,
PRIMARY KEY ([itemno], [task_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_comment
-- ----------------------------

-- ----------------------------
-- Records of iteam_comment
-- ----------------------------

-- ----------------------------
-- Table structure for iteam_contact
-- ----------------------------
DROP TABLE [dbo].[iteam_contact]
GO
CREATE TABLE [dbo].[iteam_contact] (
[contact_id] int NOT NULL IDENTITY(1,1) ,
[user_name] nvarchar(MAX) NULL ,
[phone] varchar(10) NULL ,
[email] varchar(50) NULL ,
[des] nvarchar(MAX) NULL ,
[add_user] int NULL ,
[add_date] datetime NULL ,
[status] varchar(1) NULL ,
[read] varchar(1) NULL ,
PRIMARY KEY ([contact_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_contact
-- ----------------------------

-- ----------------------------
-- Records of iteam_contact
-- ----------------------------
SET IDENTITY_INSERT [dbo].[iteam_contact] ON
GO
INSERT INTO [dbo].[iteam_contact] ([contact_id], [user_name], [phone], [email], [des], [add_user], [add_date], [status], [read]) VALUES (N'1', N'นาย สมจิตร จงจองหอง', N'0924465578', N'illusion_mist@hotmail.com', N'แก้ข้อมูล', N'3', N'2018-05-07 17:59:36.557', N'Y', N'Y')
GO
GO
INSERT INTO [dbo].[iteam_contact] ([contact_id], [user_name], [phone], [email], [des], [add_user], [add_date], [status], [read]) VALUES (N'2', N'นาย ข ไข่ อยู่ในเล้า', N'0856678854', N'opajao@hotmail.com', N'ตรวจสอบหน้า member list โชว์ข้อมูลไม่ถูกต้อง', N'3', N'2018-05-07 18:04:16.497', N'N', N'Y')
GO
GO
INSERT INTO [dbo].[iteam_contact] ([contact_id], [user_name], [phone], [email], [des], [add_user], [add_date], [status], [read]) VALUES (N'3', N'นาย ขขวดไม่ใช่ขวดเปล่า', N'0987764582', N'Ojamba@hotmail.com', N'แก้ไขข้อมูลใน project list ล้มเหลว', N'3', N'2018-05-07 18:11:56.607', N'N', N'N')
GO
GO
SET IDENTITY_INSERT [dbo].[iteam_contact] OFF
GO

-- ----------------------------
-- Table structure for iteam_group
-- ----------------------------
DROP TABLE [dbo].[iteam_group]
GO
CREATE TABLE [dbo].[iteam_group] (
[group_id] int NOT NULL IDENTITY(1,1) ,
[group_name] nvarchar(MAX) NULL ,
[group_description] nvarchar(MAX) NULL ,
[add_user] int NULL ,
[add_dt] datetime NULL ,
[edit_user] int NULL ,
[edit_dt] datetime NULL ,
PRIMARY KEY ([group_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_group
-- ----------------------------

-- ----------------------------
-- Records of iteam_group
-- ----------------------------
SET IDENTITY_INSERT [dbo].[iteam_group] ON
GO
INSERT INTO [dbo].[iteam_group] ([group_id], [group_name], [group_description], [add_user], [add_dt], [edit_user], [edit_dt]) VALUES (N'1', N'Team 1', null, N'3', N'2018-05-07 16:46:21.807', null, null)
GO
GO
SET IDENTITY_INSERT [dbo].[iteam_group] OFF
GO

-- ----------------------------
-- Table structure for iteam_group_user
-- ----------------------------
DROP TABLE [dbo].[iteam_group_user]
GO
CREATE TABLE [dbo].[iteam_group_user] (
[group_id] int NOT NULL ,
[user_id] int NOT NULL ,
PRIMARY KEY ([group_id], [user_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_group_user
-- ----------------------------

-- ----------------------------
-- Records of iteam_group_user
-- ----------------------------
INSERT INTO [dbo].[iteam_group_user] ([group_id], [user_id]) VALUES (N'1', N'1')
GO
GO
INSERT INTO [dbo].[iteam_group_user] ([group_id], [user_id]) VALUES (N'1', N'2')
GO
GO
INSERT INTO [dbo].[iteam_group_user] ([group_id], [user_id]) VALUES (N'1', N'3')
GO
GO

-- ----------------------------
-- Table structure for iteam_history_project
-- ----------------------------
DROP TABLE [dbo].[iteam_history_project]
GO
CREATE TABLE [dbo].[iteam_history_project] (
[project_id] int NOT NULL ,
[user_id] int NOT NULL ,
[read_notify] varchar(1) NULL ,
[add_user] int NULL ,
[active] varchar(1) NULL ,
[project_name] nvarchar(MAX) NULL ,
PRIMARY KEY ([project_id], [user_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_history_project
-- ----------------------------

-- ----------------------------
-- Records of iteam_history_project
-- ----------------------------
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'1', N'1', N'N', N'3', N'Y', N'Project 1')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'1', N'2', N'N', N'3', N'Y', N'Project 1')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'1', N'3', N'N', N'3', N'Y', N'Project 1')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'2', N'1', N'N', N'3', N'Y', N'Project2')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'2', N'2', N'N', N'3', N'Y', N'Project2')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'2', N'3', N'N', N'3', N'Y', N'Project2')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'3', N'1', N'N', N'4', N'Y', N'Test Add Project 01')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'3', N'2', N'N', N'4', N'Y', N'Test Add Project 01')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'3', N'3', N'N', N'4', N'Y', N'Test Add Project 01')
GO
GO
INSERT INTO [dbo].[iteam_history_project] ([project_id], [user_id], [read_notify], [add_user], [active], [project_name]) VALUES (N'3', N'4', N'N', N'4', N'Y', N'Test Add Project 01')
GO
GO

-- ----------------------------
-- Table structure for iteam_news
-- ----------------------------
DROP TABLE [dbo].[iteam_news]
GO
CREATE TABLE [dbo].[iteam_news] (
[news_id] int NOT NULL IDENTITY(1,1) ,
[news_name] nvarchar(MAX) NULL ,
[news_des] nvarchar(MAX) NULL ,
[add_date] datetime NULL ,
[add_user] int NULL ,
[color] varchar(20) NULL ,
PRIMARY KEY ([news_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_news
-- ----------------------------

-- ----------------------------
-- Records of iteam_news
-- ----------------------------
SET IDENTITY_INSERT [dbo].[iteam_news] ON
GO
SET IDENTITY_INSERT [dbo].[iteam_news] OFF
GO

-- ----------------------------
-- Table structure for iteam_project
-- ----------------------------
DROP TABLE [dbo].[iteam_project]
GO
CREATE TABLE [dbo].[iteam_project] (
[project_id] int NOT NULL IDENTITY(1,1) ,
[project_name] nvarchar(MAX) NULL ,
[project_des] nvarchar(MAX) NULL ,
[start_project] date NULL ,
[end_project] date NULL ,
[add_dt] datetime NULL ,
[add_user] int NULL ,
[edit_dt] datetime NULL ,
[tasks_complete] int NULL ,
[tasks_count] int NULL ,
[status_project] varchar(1) NULL ,
PRIMARY KEY ([project_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_project
-- ----------------------------

-- ----------------------------
-- Records of iteam_project
-- ----------------------------
SET IDENTITY_INSERT [dbo].[iteam_project] ON
GO
INSERT INTO [dbo].[iteam_project] ([project_id], [project_name], [project_des], [start_project], [end_project], [add_dt], [add_user], [edit_dt], [tasks_complete], [tasks_count], [status_project]) VALUES (N'3', N'Test Add Project 01', N'Test', N'2018-05-10', N'2018-05-18', N'2018-05-07 17:08:55.610', N'4', null, N'0', N'1', N'N')
GO
GO
SET IDENTITY_INSERT [dbo].[iteam_project] OFF
GO

-- ----------------------------
-- Table structure for iteam_project_user
-- ----------------------------
DROP TABLE [dbo].[iteam_project_user]
GO
CREATE TABLE [dbo].[iteam_project_user] (
[project_id] int NOT NULL ,
[user_id] int NOT NULL ,
PRIMARY KEY ([project_id], [user_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_project_user
-- ----------------------------

-- ----------------------------
-- Records of iteam_project_user
-- ----------------------------
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'1', N'1')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'1', N'3')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'2', N'1')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'2', N'2')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'2', N'3')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'3', N'1')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'3', N'2')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'3', N'3')
GO
GO
INSERT INTO [dbo].[iteam_project_user] ([project_id], [user_id]) VALUES (N'3', N'4')
GO
GO

-- ----------------------------
-- Table structure for iteam_task
-- ----------------------------
DROP TABLE [dbo].[iteam_task]
GO
CREATE TABLE [dbo].[iteam_task] (
[task_id] int NOT NULL IDENTITY(1,1) ,
[project_id] int NOT NULL ,
[tasks_name] nvarchar(MAX) NULL ,
[tasks_description] nvarchar(MAX) NULL ,
[date_start] datetime NULL ,
[date_end] datetime NULL ,
[tasks_prioity] nvarchar(MAX) NULL ,
[add_dt] datetime NULL ,
[path_file] nvarchar(MAX) NULL ,
[add_user] int NULL ,
[status] varchar(1) NULL ,
PRIMARY KEY ([task_id], [project_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_task
-- ----------------------------

-- ----------------------------
-- Records of iteam_task
-- ----------------------------
SET IDENTITY_INSERT [dbo].[iteam_task] ON
GO
INSERT INTO [dbo].[iteam_task] ([task_id], [project_id], [tasks_name], [tasks_description], [date_start], [date_end], [tasks_prioity], [add_dt], [path_file], [add_user], [status]) VALUES (N'4', N'3', N'Test Add Task 01', N'Test', N'2018-05-11 00:00:00.000', N'2018-05-12 17:00:00.000', N'2', N'2018-05-07 17:11:02.740', null, N'4', N'N')
GO
GO
SET IDENTITY_INSERT [dbo].[iteam_task] OFF
GO

-- ----------------------------
-- Table structure for iteam_task_lists
-- ----------------------------
DROP TABLE [dbo].[iteam_task_lists]
GO
CREATE TABLE [dbo].[iteam_task_lists] (
[task_id] int NOT NULL ,
[itemno] int NOT NULL ,
[name_task_list] nvarchar(MAX) NULL ,
[remark_list] nvarchar(MAX) NULL ,
[save_user] int NULL ,
[add_dt] datetime NULL ,
[active] varchar(1) NULL ,
[edit_user] int NULL ,
[edit_date] datetime NULL ,
[status] varchar(1) NULL ,
PRIMARY KEY ([task_id], [itemno])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_task_lists
-- ----------------------------

-- ----------------------------
-- Records of iteam_task_lists
-- ----------------------------
INSERT INTO [dbo].[iteam_task_lists] ([task_id], [itemno], [name_task_list], [remark_list], [save_user], [add_dt], [active], [edit_user], [edit_date], [status]) VALUES (N'2', N'1', N'detail1', null, N'3', N'2018-05-07 16:25:21.137', N'Y', N'3', N'2018-05-07 16:25:27.870', N'3')
GO
GO
INSERT INTO [dbo].[iteam_task_lists] ([task_id], [itemno], [name_task_list], [remark_list], [save_user], [add_dt], [active], [edit_user], [edit_date], [status]) VALUES (N'2', N'2', N'detail 2', null, N'3', N'2018-05-07 16:25:21.137', N'Y', N'3', N'2018-05-07 16:25:27.137', N'3')
GO
GO

-- ----------------------------
-- Table structure for iteam_task_user
-- ----------------------------
DROP TABLE [dbo].[iteam_task_user]
GO
CREATE TABLE [dbo].[iteam_task_user] (
[task_id] int NOT NULL ,
[user_id] int NOT NULL ,
PRIMARY KEY ([task_id], [user_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_task_user
-- ----------------------------

-- ----------------------------
-- Records of iteam_task_user
-- ----------------------------
INSERT INTO [dbo].[iteam_task_user] ([task_id], [user_id]) VALUES (N'1', N'3')
GO
GO
INSERT INTO [dbo].[iteam_task_user] ([task_id], [user_id]) VALUES (N'2', N'3')
GO
GO
INSERT INTO [dbo].[iteam_task_user] ([task_id], [user_id]) VALUES (N'4', N'1')
GO
GO
INSERT INTO [dbo].[iteam_task_user] ([task_id], [user_id]) VALUES (N'4', N'4')
GO
GO

-- ----------------------------
-- Table structure for iteam_upload
-- ----------------------------
DROP TABLE [dbo].[iteam_upload]
GO
CREATE TABLE [dbo].[iteam_upload] (
[task_id] int NOT NULL ,
[path_file] nvarchar(MAX) NULL ,
[file_name] nvarchar(MAX) NULL ,
[add_user] int NULL ,
[add_dt] datetime NULL ,
[itemno] int NOT NULL ,
PRIMARY KEY ([task_id], [itemno])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_upload
-- ----------------------------

-- ----------------------------
-- Records of iteam_upload
-- ----------------------------

-- ----------------------------
-- Table structure for iteam_upload_pic
-- ----------------------------
DROP TABLE [dbo].[iteam_upload_pic]
GO
CREATE TABLE [dbo].[iteam_upload_pic] (
[news_id] int NOT NULL ,
[path_file] nvarchar(MAX) NULL ,
[file_name] nvarchar(MAX) NULL ,
[add_user] int NULL ,
[add_dt] datetime NULL ,
[itemno] int NOT NULL ,
PRIMARY KEY ([news_id], [itemno])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_upload_pic
-- ----------------------------

-- ----------------------------
-- Records of iteam_upload_pic
-- ----------------------------

-- ----------------------------
-- Table structure for iteam_user
-- ----------------------------
DROP TABLE [dbo].[iteam_user]
GO
CREATE TABLE [dbo].[iteam_user] (
[user_id] int NOT NULL IDENTITY(1,1) ,
[username] varchar(100) NOT NULL ,
[password] varchar(100) NOT NULL ,
[name_th] nvarchar(MAX) NULL ,
[postion] nvarchar(MAX) NULL ,
[path_image] text NULL ,
[description] nvarchar(MAX) NULL ,
[phone] varchar(10) NULL ,
[line_id] varchar(50) NULL ,
[email] varchar(200) NULL ,
[is_admin] varchar(1) NULL ,
PRIMARY KEY ([user_id])
)


GO

-- ----------------------------
-- Indexes structure for table iteam_user
-- ----------------------------

-- ----------------------------
-- Records of iteam_user
-- ----------------------------
SET IDENTITY_INSERT [dbo].[iteam_user] ON
GO
INSERT INTO [dbo].[iteam_user] ([user_id], [username], [password], [name_th], [postion], [path_image], [description], [phone], [line_id], [email], [is_admin]) VALUES (N'1', N'kitaponjit', N'1234', N'Kitapon Jit', N'6', N'https://westdulwichosteopaths.com/wp-content/uploads/2017/05/yonetici-icon-300x300.png', null, null, null, N'kitapon.jit@gmail.com', N'N')
GO
GO
INSERT INTO [dbo].[iteam_user] ([user_id], [username], [password], [name_th], [postion], [path_image], [description], [phone], [line_id], [email], [is_admin]) VALUES (N'2', N'pp', N'1234', N'PP', N'6', N'https://westdulwichosteopaths.com/wp-content/uploads/2017/05/yonetici-icon-300x300.png', null, null, null, N'peerasin_pee@hotmail.com', N'N')
GO
GO
INSERT INTO [dbo].[iteam_user] ([user_id], [username], [password], [name_th], [postion], [path_image], [description], [phone], [line_id], [email], [is_admin]) VALUES (N'3', N'bhumipas', N'1234', N'bhum', N'3', N'https://westdulwichosteopaths.com/wp-content/uploads/2017/05/yonetici-icon-300x300.png', N'Skill: Html,CSS Angular.js Vue.js', N'0939901758', N'l3Rabbit', N'illusion_mist@hotmail.com', N'Y')
GO
GO
INSERT INTO [dbo].[iteam_user] ([user_id], [username], [password], [name_th], [postion], [path_image], [description], [phone], [line_id], [email], [is_admin]) VALUES (N'4', N'mospichet', N'4628', N'Pichet Boonwong', N'6', N'https://scontent.fbkk1-4.fna.fbcdn.net/v/t31.0-8/27993559_1810480662357673_2927971824617012533_o.jpg?_nc_cat=0&oh=de2e274823c8b38b6922dcabd51e0fcc&oe=5B97E422', null, N'0982850832', N'pichetboonwong', N'ipmospichet@hotmail.co.th', N'N')
GO
GO
SET IDENTITY_INSERT [dbo].[iteam_user] OFF
GO
