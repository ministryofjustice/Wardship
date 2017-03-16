CREATE TABLE [dbo].[AuditEvents] (
    [idAuditEvent]                                  INT            IDENTITY (1, 1) NOT NULL,
    [EventDate]                                     DATETIME       NOT NULL,
    [UserID]                                        NVARCHAR (40)  NOT NULL,
    [idAuditEventDescription]                       NVARCHAR (MAX) NOT NULL,
    [ChildSurname]                                  NVARCHAR (100) NULL,
    [ChildForenames]                                NVARCHAR (100) NULL,
    [ChildDateofBirth]                              DATETIME       NULL,
    [AuditEventDescription_idAuditEventDescription] INT            NULL,
    PRIMARY KEY CLUSTERED ([idAuditEvent] ASC),
    CONSTRAINT [AuditEventDescription_AuditEvents] FOREIGN KEY ([AuditEventDescription_idAuditEventDescription]) REFERENCES [dbo].[AuditEventDescriptions] ([idAuditEventDescription])
);

