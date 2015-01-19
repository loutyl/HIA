CREATE TABLE [dbo].[Chambre] (
    [id_chambre]  INT IDENTITY (1, 1) NOT NULL,
    [num_chambre] INT NOT NULL,
    [nb_lits_max] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([id_chambre] ASC)
);

