CREATE TABLE [dbo].[ADGroups] (
    [ADGroupID]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (80) NOT NULL,
    [RoleStrength] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([ADGroupID] ASC),
    CONSTRAINT [ADGroup_Role] FOREIGN KEY ([RoleStrength]) REFERENCES [dbo].[Roles] ([strength]) ON DELETE CASCADE
);

