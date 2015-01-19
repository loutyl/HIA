CREATE TABLE [dbo].[Patient]
(
	[id_patient] INT NOT NULL PRIMARY KEY IDENTITY, 
    [nom_patient] NCHAR(50) NOT NULL, 
    [prenom_patient] NCHAR(50) NOT NULL, 
    [age_patient] INT NOT NULL, 
    [service] NCHAR(50) NOT NULL, 
    [etage] INT NOT NULL, 
    [chambre] INT NOT NULL, 
    [lit] NCHAR(50) NOT NULL
)
