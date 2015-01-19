CREATE TABLE [dbo].[Occuper] (
    [id_accueil] INT NOT NULL,
    [id_lit]     INT NOT NULL,
    CONSTRAINT [FK_Occuper_Accueil] FOREIGN KEY ([id_accueil]) REFERENCES [dbo].[Accueil] ([id_accueil]),
    CONSTRAINT [FK_Occuper_Lit] FOREIGN KEY ([id_lit]) REFERENCES [dbo].[Lit] ([id_lit])
);

