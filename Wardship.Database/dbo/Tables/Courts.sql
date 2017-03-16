CREATE TABLE [dbo].[Courts] (
    [CourtID]       INT            IDENTITY (1, 1) NOT NULL,
    [CourtName]     NVARCHAR (100) NOT NULL,
    [AddressLine1]  NVARCHAR (50)  NULL,
    [AddressLine2]  NVARCHAR (50)  NULL,
    [AddressLine3]  NVARCHAR (50)  NULL,
    [AddressLine4]  NVARCHAR (50)  NULL,
    [Town]          NVARCHAR (30)  NULL,
    [County]        NVARCHAR (30)  NULL,
    [Country]       NVARCHAR (20)  NULL,
    [Postcode]      NVARCHAR (8)   NULL,
    [DX]            NVARCHAR (60)  NULL,
    [active]        BIT            NOT NULL,
    [deactivated]   DATETIME       NULL,
    [deactivatedBy] NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([CourtID] ASC)
);

