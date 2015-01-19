CREATE TABLE [dbo].[PreListe] (
    [id_preliste]     INT            IDENTITY (1, 1) NOT NULL,
    [nom_visiteur]    NVARCHAR (MAX) NOT NULL,
    [prenom_visiteur] NVARCHAR (MAX) NOT NULL,
    [tel_visiteur]    NVARCHAR (MAX) NULL,
    [email_visiteur]  NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id_preliste] ASC)
);

