CREATE TABLE [dbo].[tbl_User] (
    [UserID]       INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]    NVARCHAR (50) NOT NULL,
    [LastName]     NVARCHAR (50) NOT NULL,
    [Gender]       NVARCHAR (50) NOT NULL,
    [Age]          NVARCHAR (50) NOT NULL,
    [EmailAddress] NVARCHAR (50) NOT NULL,
    [Username]     NVARCHAR (50) NOT NULL,
    [Password]     NVARCHAR (50) NOT NULL,
    [State]        NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);
