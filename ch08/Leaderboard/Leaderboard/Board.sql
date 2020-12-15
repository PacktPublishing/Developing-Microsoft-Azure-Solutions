CREATE TABLE [dbo].[Board]
(
  [Id] INT IDENTITY(1,1) NOT NULL,
  [NumOfWorkouts] BIGINT NOT NULL,
  [UserID] INT NOT NULL,
  CONSTRAINT [PK_Board] PRIMARY KEY CLUSTERED ([Id]),
  FOREIGN KEY ([UserId]) REFERENCES BoardUser([UserID])
)
GO
CREATE INDEX [IX_BoardWorkouts]
ON [dbo].[Board] ([NumOfWorkouts])
GO