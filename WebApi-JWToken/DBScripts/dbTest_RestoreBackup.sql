USE [dbTest]
GO
/****** Object:  Table [dbo].[HistorialRefreshToken]    Script Date: 28/09/2023 03:56:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HistorialRefreshToken]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HistorialRefreshToken](
	[IdHistorialToken] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Token] [varchar](500) NULL,
	[RefreshToken] [varchar](200) NULL,
	[CreationDate] [datetime] NULL,
	[ExpirationDate] [datetime] NULL,
	[IsActive]  AS (case when [ExpirationDate]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end),
PRIMARY KEY CLUSTERED 
(
	[IdHistorialToken] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 28/09/2023 03:56:04 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[HistorialRefreshToken] ON 

INSERT [dbo].[HistorialRefreshToken] ([IdHistorialToken], [UserId], [Token], [RefreshToken], [CreationDate], [ExpirationDate]) VALUES (1, 1, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJBZG1pbiIsIm5iZiI6MTY5NTkzNDI0MywiZXhwIjoxNjk1OTM0MzAzLCJpYXQiOjE2OTU5MzQyNDN9.uxFRvNuTsHdVVgG32I0ZxcDn0On2IeSp240NBzPFwSc', N'QeaMZQw7uxBzxNvrAluit7o6Big3HaD7TC8RPyw9YlKfYKjp68nTLtOEQ+xONg+EVpOxURLgVUUfVEmqSPKTcQ==', CAST(N'2023-09-28T20:50:43.133' AS DateTime), CAST(N'2023-09-28T20:52:43.133' AS DateTime))
SET IDENTITY_INSERT [dbo].[HistorialRefreshToken] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [Username], [Password]) VALUES (1, N'Admin', N'admin123')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Historial__UserI__29572725]') AND parent_object_id = OBJECT_ID(N'[dbo].[HistorialRefreshToken]'))
ALTER TABLE [dbo].[HistorialRefreshToken]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
