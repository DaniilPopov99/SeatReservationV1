SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FavoritesRestaurants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_FavoritesRestaurants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FavoritesRestaurants] ADD  CONSTRAINT [DF_FavoritesRestaurants_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[FavoritesRestaurants]  WITH CHECK ADD  CONSTRAINT [FK_FavoritesRestaurants_Restaurants] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
GO

ALTER TABLE [dbo].[FavoritesRestaurants] CHECK CONSTRAINT [FK_FavoritesRestaurants_Restaurants]
GO

ALTER TABLE [dbo].[FavoritesRestaurants]  WITH CHECK ADD  CONSTRAINT [FK_FavoritesRestaurants_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[FavoritesRestaurants] CHECK CONSTRAINT [FK_FavoritesRestaurants_Users]
GO