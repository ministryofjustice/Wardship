CREATE TABLE [dbo].[Types] (
    [TypeID]      INT            IDENTITY (1, 1) NOT NULL,
    [Detail]      NVARCHAR (20)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([TypeID] ASC)
);

