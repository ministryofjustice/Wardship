CREATE TABLE [dbo].[WordTemplates] (
    [templateID]    INT            IDENTITY (1, 1) NOT NULL,
    [templateName]  NVARCHAR (80)  NOT NULL,
    [templateXML]   NVARCHAR (MAX) NOT NULL,
    [active]        BIT            NOT NULL,
    [deactivated]   DATETIME       NULL,
    [deactivatedBy] NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([templateID] ASC)
);

