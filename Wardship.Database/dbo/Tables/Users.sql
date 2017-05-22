CREATE TABLE [dbo].[Users] (
    [UserID]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (150) NOT NULL,
    [DisplayName]  NVARCHAR (30) NULL,
    [LastActive]   DATETIME      NULL,
    [RoleStrength] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [User_Role] FOREIGN KEY ([RoleStrength]) REFERENCES [dbo].[Roles] ([strength]) ON DELETE CASCADE
);

