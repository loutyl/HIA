CREATE TABLE [dbo].[Patient] (
    [id_patient]     INT            IDENTITY (1, 1) NOT NULL,
    [nom_patient]    NVARCHAR (MAX) NOT NULL,
    [prenom_patient] NVARCHAR (MAX) NOT NULL,
    [age_patient]    INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([id_patient] ASC)
);

