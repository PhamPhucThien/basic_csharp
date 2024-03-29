USE [BookStore]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 11/17/2023 10:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[id] [uniqueidentifier] NOT NULL,
	[code] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](250) NOT NULL,
	[username] [nvarchar](250) NOT NULL,
	[password] [nvarchar](250) NOT NULL,
	[role] [nvarchar](250) NOT NULL,
	[is_available] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 11/17/2023 10:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[id] [uniqueidentifier] NOT NULL,
	[account_id] [uniqueidentifier] NOT NULL,
	[cart_record] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/17/2023 10:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[id] [uniqueidentifier] NOT NULL,
	[account_id] [uniqueidentifier] NOT NULL,
	[order_record] [nvarchar](500) NULL,
	[total_price] [bigint] NOT NULL,
	[create_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/17/2023 10:56:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[id] [uniqueidentifier] NOT NULL,
	[code] [int] IDENTITY(1,1) NOT NULL,
	[product_name] [nvarchar](250) NOT NULL,
	[price] [bigint] NOT NULL,
	[quantity] [int] NOT NULL,
	[is_available] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([id], [code], [name], [username], [password], [role], [is_available]) VALUES (N'b6b86a63-4ebe-44ed-ae16-42755fcf8989', 2, N'User', N'User', N'12345', N'User', 1)
INSERT [dbo].[Account] ([id], [code], [name], [username], [password], [role], [is_available]) VALUES (N'95868ee8-4263-430b-a674-a8083e02c2c5', 1, N'Thien', N'Thien', N'12345', N'Admin', 1)
INSERT [dbo].[Account] ([id], [code], [name], [username], [password], [role], [is_available]) VALUES (N'968d68e2-4d2b-4552-8d64-f5ebe207d9f5', 4, N'yenvy', N'yenvy', N'1234', N'User', 1)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
INSERT [dbo].[Cart] ([id], [account_id], [cart_record]) VALUES (N'39864e34-5498-4865-95f6-55ffc5f6ea5d', N'b6b86a63-4ebe-44ed-ae16-42755fcf8989', N'')
INSERT [dbo].[Cart] ([id], [account_id], [cart_record]) VALUES (N'fa26b715-09e1-4038-9eea-ff7d7b2d939a', N'968d68e2-4d2b-4552-8d64-f5ebe207d9f5', N'')
GO
INSERT [dbo].[Order] ([id], [account_id], [order_record], [total_price], [create_date]) VALUES (N'ac71688f-0342-44f4-85c4-f0a194418e60', N'968d68e2-4d2b-4552-8d64-f5ebe207d9f5', N'3&Bleach&17&6/', 102, CAST(N'2023-11-17T22:09:31.247' AS DateTime))
INSERT [dbo].[Order] ([id], [account_id], [order_record], [total_price], [create_date]) VALUES (N'8a66cac0-1e9d-45ca-b6a8-f28ba0a43930', N'968d68e2-4d2b-4552-8d64-f5ebe207d9f5', N'5&Dragon Ball Z&10&5/7&Reverse 1999&100&2/', 250, CAST(N'2023-11-17T22:09:49.587' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'38325cfe-4d64-4262-a432-00417fd03479', 3, N'Bleach', 17, 100, 1)
INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'857b6d73-1067-4577-a8e3-3159bc2af77e', 4, N'One Piece', 23, 100, 1)
INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'90b879fa-342d-4942-ad4c-7164e83f2cd0', 7, N'Reverse 1999', 100, 10, 1)
INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'fe83b8e7-9fc3-46f4-9b6d-d8d3f37be2ff', 1, N'Hyouka', 30, 1, 1)
INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'a49a041d-04c2-4eef-93aa-ead9e7e2b223', 2, N'Naruto', 20, 100, 1)
INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'e53e9ca1-a8a7-46a0-8bac-f1bd2803c26b', 5, N'Dragon Ball Z', 10, 100, 1)
INSERT [dbo].[Product] ([id], [code], [product_name], [price], [quantity], [is_available]) VALUES (N'd231e666-6323-49ea-9c2e-f70763787c04', 6, N'Doraemon', 15, 100, 1)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Account__F3DBC572B5547391]    Script Date: 11/17/2023 10:56:48 PM ******/
ALTER TABLE [dbo].[Account] ADD UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Cart__46A222CC40170256]    Script Date: 11/17/2023 10:56:48 PM ******/
ALTER TABLE [dbo].[Cart] ADD UNIQUE NONCLUSTERED 
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ((1)) FOR [is_available]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT ('') FOR [cart_record]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((1)) FOR [is_available]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [role_check] CHECK  (([role]='User' OR [role]='Admin'))
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [role_check]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [quantity_check] CHECK  (([quantity]>=(0)))
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [quantity_check]
GO
