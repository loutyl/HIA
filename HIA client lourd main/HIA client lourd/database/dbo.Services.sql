CREATE TABLE [dbo].[Services] (
    [id_service]   INT            IDENTITY (1, 1) NOT NULL,
    [nom_service]  NVARCHAR (MAX) NOT NULL,
    [zone]         INT            NOT NULL,
    [commentaires] TEXT           NULL,
    PRIMARY KEY CLUSTERED ([id_service] ASC)
);

