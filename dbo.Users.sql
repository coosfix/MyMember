USE [Northwind]
GO

/****** 物件: Table [dbo].[Users] 指令碼日期: 2019/4/24 下午 10:08:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users] (
    [Id]       INT            IDENTITY (1, 1) PRIMARY KEY,
    [Username] NVARCHAR (16)  NOT NULL UNIQUE,
    [Password] VARBINARY (32) NOT NULL,
	[Email] NVARCHAR (64) NOT NULL UNIQUE,
    [RanNum]   NVARCHAR (32)  NOT NULL,
    [Date]     DATETIME       NULL
);


