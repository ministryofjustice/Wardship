CREATE TABLE [dbo].[CAFCASSes] (
    [CAFCASSID]   INT            IDENTITY (1, 1) NOT NULL,
    [Detail]      NVARCHAR (10)  NULL,
    [Description] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([CAFCASSID] ASC)
);

