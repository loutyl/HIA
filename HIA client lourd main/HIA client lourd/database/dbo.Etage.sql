CREATE TABLE [dbo].[Etage] (
    [id_etage]    INT  IDENTITY (1, 1) NOT NULL,
    [etage]       INT  NOT NULL,
    [description] TEXT NULL,
    PRIMARY KEY CLUSTERED ([id_etage] ASC)
);

