CREATE TABLE [LinkHide].[Quotas]
(
	[QuotasId] int NOT NULL IDENTITY(1,1),
	[Date] datetimeoffset NOT NULL CONSTRAINT [DF_Quotas_Date] DEFAULT GETUTCDATE(),
	[Client] nvarchar(1024) NOT NULL,
    CONSTRAINT [PK_Quotas] PRIMARY KEY CLUSTERED ([QuotasId] ASC)
)
GO

