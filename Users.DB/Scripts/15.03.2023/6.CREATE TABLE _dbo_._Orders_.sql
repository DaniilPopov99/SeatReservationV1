SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[PersonCount] [int] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_CreateDate]  DEFAULT (getutcdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Restaurants] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Restaurants]
GO

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO