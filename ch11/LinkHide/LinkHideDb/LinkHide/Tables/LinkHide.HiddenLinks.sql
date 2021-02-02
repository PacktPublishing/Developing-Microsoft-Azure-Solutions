CREATE TABLE [LinkHide].[HiddenLinks]
(
	[HiddenLinkId] int NOT NULL IDENTITY(1,1),
	[IsActive] bit NOT NULL CONSTRAINT [DF_HiddenLinks_IsActive] DEFAULT 1,
	[Expiration] datetimeoffset NULL CONSTRAINT [DF_HiddenLinks_Expiration] DEFAULT DATEADD(day, 1, GETUTCDATE()),
	[TokenHash] int NOT NULL,
	[Token] VARCHAR(50) NOT NULL,
	[LinkType] int NOT NULL,
	[Link] NVARCHAR(4000) NOT NULL,
	[Visits] int NOT NULL CONSTRAINT [DF_HiddenLinks_Visits] DEFAULT 0,
	[Success] int NOT NULL CONSTRAINT [DF_HiddenLinks_Success] DEFAULT 0,
    CONSTRAINT [PK_HiddenLinks] PRIMARY KEY CLUSTERED ([HiddenLinkId] ASC)
)
GO
CREATE NONCLUSTERED INDEX [IX_HiddenLinks_TokenHash_Token] ON [LinkHide].[HiddenLinks]
	([IsActive], [Expiration], [TokenHash], [Token]) INCLUDE ([Link], [LinkType], [Visits], [Success])
GO
CREATE NONCLUSTERED INDEX [IX_HiddenLinks_Token_TokenHash] ON [LinkHide].[HiddenLinks]
	([IsActive], [Expiration], [Token], [TokenHash]) INCLUDE ([Link], [LinkType], [Visits], [Success])
GO