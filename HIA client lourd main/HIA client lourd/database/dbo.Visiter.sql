CREATE TABLE [dbo].[Visiter] (
    [id_visiteur]     INT            NOT NULL,
    [id_patient]      INT            NOT NULL,
    [num_bon_visite]  NVARCHAR (MAX) NULL,
    [date_deb_visite] DATETIME       NULL,
    [date_fin_visite] DATETIME       NULL,
    [status_demande] INT NOT NULL, 
    CONSTRAINT [FK_Visiter_Visiteur] FOREIGN KEY ([id_visiteur]) REFERENCES [dbo].[Visiteur] ([id_visiteur]),
    CONSTRAINT [FK_Visiter_Patient] FOREIGN KEY ([id_patient]) REFERENCES [dbo].[Patient] ([id_patient])
);

