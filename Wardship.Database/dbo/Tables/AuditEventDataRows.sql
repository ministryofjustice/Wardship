CREATE TABLE [dbo].[AuditEventDataRows] (
    [idAuditData]  INT            IDENTITY (1, 1) NOT NULL,
    [idAuditEvent] INT            NOT NULL,
    [ColumnName]   NVARCHAR (200) NOT NULL,
    [Was]          NVARCHAR (200) NOT NULL,
    [Now]          NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([idAuditData] ASC),
    CONSTRAINT [AuditEventDataRow_auditEvent] FOREIGN KEY ([idAuditEvent]) REFERENCES [dbo].[AuditEvents] ([idAuditEvent]) ON DELETE CASCADE
);

