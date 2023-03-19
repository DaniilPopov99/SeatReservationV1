SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Restaurants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[House] [nvarchar](50) NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[PhoneNumber] [numeric](11, 0) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Restaurants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Restaurants] ADD  CONSTRAINT [DF_Restaurants_CreateDate]  DEFAULT (getutcdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Restaurants] ADD  DEFAULT ((1)) FOR [IsActive]
GO