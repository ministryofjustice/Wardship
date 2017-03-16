CREATE TABLE [dbo].[CaseTypes] (
    [CaseTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [Detail]      NVARCHAR (2)   NULL,
    [Description] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([CaseTypeID] ASC)
);

