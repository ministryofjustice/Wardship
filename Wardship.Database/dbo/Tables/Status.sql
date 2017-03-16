CREATE TABLE [dbo].[Status] (
    [StatusID] INT           IDENTITY (1, 1) NOT NULL,
    [Detail]   NVARCHAR (15) NULL,
    PRIMARY KEY CLUSTERED ([StatusID] ASC)
);

