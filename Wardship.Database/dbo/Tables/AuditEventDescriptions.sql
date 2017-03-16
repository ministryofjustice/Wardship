CREATE TABLE [dbo].[AuditEventDescriptions] (
    [idAuditEventDescription] INT           IDENTITY (1, 1) NOT NULL,
    [AuditDescription]        NVARCHAR (40) NOT NULL,
    PRIMARY KEY CLUSTERED ([idAuditEventDescription] ASC)
);

