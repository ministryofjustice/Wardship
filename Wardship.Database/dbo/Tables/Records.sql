CREATE TABLE [dbo].[Records] (
    [RecordID]    INT            IDENTITY (1, 1) NOT NULL,
    [Detail]      NVARCHAR (5)   NULL,
    [Description] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([RecordID] ASC)
);

