SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ImagesToRestaurants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantId] [int] NOT NULL,
	[ImageId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ImagesToRestaurants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ImagesToRestaurants] ADD  CONSTRAINT [DF_ImagesToRestaurants_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[ImagesToRestaurants]  WITH CHECK ADD  CONSTRAINT [FK_ImagesToRestaurants_Images] FOREIGN KEY([ImageId])
REFERENCES [dbo].[Images] ([Id])
GO

ALTER TABLE [dbo].[ImagesToRestaurants] CHECK CONSTRAINT [FK_ImagesToRestaurants_Images]
GO

ALTER TABLE [dbo].[ImagesToRestaurants]  WITH CHECK ADD  CONSTRAINT [FK_ImagesToRestaurants_Restaurants] FOREIGN KEY([RestaurantId])
REFERENCES [dbo].[Restaurants] ([Id])
GO

ALTER TABLE [dbo].[ImagesToRestaurants] CHECK CONSTRAINT [FK_ImagesToRestaurants_Restaurants]
GO