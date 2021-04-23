CREATE TABLE [Wicked].[Favorite]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(80) NOT NULL,
	[CategoryId] INT NOT NULL,
	[LogoUrl] NVARCHAR(4000) NULL,
	[ImageUrl] NVARCHAR(4000) NULL,
	[Url] NVARCHAR(4000) NULL,
	[Votes] INT NOT NULL CONSTRAINT [DF_Favorite_Votes] DEFAULT 0,
	[CreatedOn] DATETIME CONSTRAINT [DF_Favorite_CreatedOn] DEFAULT GETUTCDATE(),
	CONSTRAINT [PK_Favorite] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Wicked].[Category]([Id])
)
GO
CREATE NONCLUSTERED INDEX IX_Favorite_Name
	ON [Wicked].[Favorite]([Name])
GO
