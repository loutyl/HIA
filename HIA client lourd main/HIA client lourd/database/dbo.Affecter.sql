CREATE TABLE [dbo].[Affecter] (
    [id_etage]   INT NOT NULL,
    [id_service] INT NULL,
    CONSTRAINT [FK_Affecter_Etage] FOREIGN KEY ([id_etage]) REFERENCES [dbo].[Etage] ([id_etage]),
    CONSTRAINT [FK_Affecter_Service] FOREIGN KEY ([id_service]) REFERENCES [dbo].[Service] ([id_service])
);

