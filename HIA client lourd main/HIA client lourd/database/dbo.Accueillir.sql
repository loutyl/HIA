CREATE TABLE [dbo].[Accueillir] (
    [id_patient] INT NOT NULL,
    [id_accueil] INT NOT NULL,
    CONSTRAINT [FK_Table_Patient] FOREIGN KEY ([id_patient]) REFERENCES [dbo].[Patient] ([id_patient]),
    CONSTRAINT [FK_Table_Accueil] FOREIGN KEY ([id_accueil]) REFERENCES [dbo].[Accueil] ([id_accueil])
);

