CREATE TABLE [dbo].[Personnel] (
    [id_personnel]     INT            IDENTITY (1, 1) NOT NULL,
    [nom_personnel]    NVARCHAR (MAX) NULL,
    [prenom_personnel] NVARCHAR (MAX) NULL,
    [login]            NVARCHAR (MAX) NULL,
    [password]         NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id_personnel] ASC)
);

