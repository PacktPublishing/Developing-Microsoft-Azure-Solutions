CREATE TABLE [Wicked].[Category]
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(80) NOT NULL,
	CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
)
GO
CREATE NONCLUSTERED INDEX [IX_Category_Name]
	ON [Wicked].[Category]([Name])
GO
