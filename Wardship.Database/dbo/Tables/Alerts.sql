CREATE TABLE [dbo].[Alerts] (
    [AlertID]     INT            IDENTITY (1, 1) NOT NULL,
    [Live]        BIT            NOT NULL,
    [EventStart]  DATETIME       NOT NULL,
    [RaisedHours] INT            NOT NULL,
    [WarnStart]   DATETIME       NOT NULL,
    [Message]     NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([AlertID] ASC)
);

