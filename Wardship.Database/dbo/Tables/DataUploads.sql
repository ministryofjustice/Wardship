CREATE TABLE [dbo].[DataUploads] (
    [DataUploadID]    INT            IDENTITY (1, 1) NOT NULL,
    [UploadStarted]   DATETIME       NOT NULL,
    [UploadedBy]      NVARCHAR (MAX) NULL,
    [FileName]        NVARCHAR (MAX) NULL,
    [FullPathandName] NVARCHAR (MAX) NULL,
    [FileSize]        INT            NOT NULL,
    [UploadCompleted] DATETIME       NULL,
    [NumberofRows]    INT            NOT NULL,
    [NumberOfErrs]    INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([DataUploadID] ASC)
);

