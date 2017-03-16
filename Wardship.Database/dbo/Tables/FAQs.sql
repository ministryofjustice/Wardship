CREATE TABLE [dbo].[FAQs] (
    [faqID]        INT             IDENTITY (1, 1) NOT NULL,
    [loggedInUser] BIT             NOT NULL,
    [question]     NVARCHAR (150)  NOT NULL,
    [answer]       NVARCHAR (4000) NOT NULL,
    PRIMARY KEY CLUSTERED ([faqID] ASC)
);

