CREATE TABLE [dbo].[Appartenir] (
    [id_lit]     INT      NOT NULL,
    [id_chambre] INT      NOT NULL,
    [date_debut] DATETIME NULL,
    [date_fin ]  DATETIME NULL,
    CONSTRAINT [FK_Appartenir_Lit] FOREIGN KEY ([id_lit]) REFERENCES [dbo].[Lit] ([id_lit]),
    CONSTRAINT [FK_Appartenir_Chambre] FOREIGN KEY ([id_chambre]) REFERENCES [dbo].[Chambre] ([id_chambre])
);

