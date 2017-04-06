IF NOT EXISTS (	SELECT *	FROM sys.databases	WHERE NAME = 'Snmp_Client_DB'	)
BEGIN
	CREATE DATABASE Snmp_Client_DB
END
GO

USE Snmp_Client_DB
Go

CREATE TABLE [dbo].[Log](
	[LogID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[OIDKey] [int]  NULL,
	[Value] [nvarchar](50) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[WMIID] [int]  NULL,
	[IPAddress] [nvarchar](15) NULL)

CREATE TABLE [dbo].[OIDs](
	[OIDKey] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Item] [nvarchar](30) NOT NULL,
	[OID] [nvarchar](40) NOT NULL)

CREATE TABLE [dbo].[WMI](
	[WMIID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Description] [nvarchar](30) NULL)


GO
SET IDENTITY_INSERT [dbo].[OIDs] ON 

GO
INSERT [dbo].[OIDs] ([OIDKey], [Item], [OID]) VALUES (1, N'sysName', N'.1.3.6.1.2.1.1.5.0')
GO
INSERT [dbo].[OIDs] ([OIDKey], [Item], [OID]) VALUES (2, N'sysLocation', N'.1.3.6.1.2.1.1.6.0')
GO
INSERT [dbo].[OIDs] ([OIDKey], [Item], [OID]) VALUES (3, N'sysUptime', N'.1.3.6.1.2.1.1.3.0')
GO
INSERT [dbo].[OIDs] ([OIDKey], [Item], [OID]) VALUES (4, N'Interface Type', N'.1.3.6.1.2.1.2.2.1.3.1')
GO
INSERT [dbo].[OIDs] ([OIDKey], [Item], [OID]) VALUES (5, N'sysServices', N'.1.3.6.1.2.1.1.7.0')
GO
SET IDENTITY_INSERT [dbo].[OIDs] OFF
GO
SET IDENTITY_INSERT [dbo].[WMI] ON 

GO
INSERT [dbo].[WMI] ([WMIID], [Description]) VALUES (1, N'CPU')
GO
INSERT [dbo].[WMI] ([WMIID], [Description]) VALUES (2, N'Memory')
GO
INSERT [dbo].[WMI] ([WMIID], [Description]) VALUES (3, 'Disk Disk reads / sec')
GO
INSERT [dbo].[WMI] ([WMIID], [Description]) VALUES (4, 'Disk writes / sec')
GO
INSERT [dbo].[WMI] ([WMIID], [Description]) VALUES (5, 'Disk transfers / sec')
GO
SET IDENTITY_INSERT [dbo].[WMI] OFF
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_OIDs] FOREIGN KEY([OIDKey])
REFERENCES [dbo].[OIDs] ([OIDKey])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_OIDs]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_WMI] FOREIGN KEY([WMIID])
REFERENCES [dbo].[WMI] ([WMIID])
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_WMI]
GO


CREATE PROCEDURE [dbo].[uspGetLog]
@SearchString nvarchar(50) = null
AS
BEGIN
	SET NOCOUNT ON;
SELECT [Item]
	,[Description]
	,[Value]
	,[DateTime]
	,[IPAddress]
FROM [log] lg
LEFT OUTER JOIN OIDs oi ON lg.[OIDKey] = oi.[OIDKey]
LEFT OUTER JOIN [WMI] wmi ON wmi.[WMIID] = lg.[WMIID]
WHERE (
		([value]  like '%'+ @SearchString+'%')
		OR ([IPAddress] like '%'+ @SearchString+'%')
		--OR ([DateTime] = @SearchString)
		OR ([Item] like '%'+@SearchString+'%')
		OR ([Description] like '%'+@SearchString+'%')
		or (@SearchString = null)
		)
		order by [DateTime],[IPAddress] 
END


--exec [uspGetLog] ' '