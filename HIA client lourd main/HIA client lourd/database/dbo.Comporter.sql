CREATE TABLE [dbo].[Comporter] (
    [id_service] INT NOT NULL,
    [id_chambre] INT NOT NULL,
    CONSTRAINT [FK_Comporter_Service] FOREIGN KEY ([id_service]) REFERENCES [dbo].[Service] ([id_service]),
    CONSTRAINT [FK_Comporter_Chambre] FOREIGN KEY ([id_chambre]) REFERENCES [dbo].[Chambre] ([id_chambre])
);

