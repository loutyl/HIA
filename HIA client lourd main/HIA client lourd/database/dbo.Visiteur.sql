CREATE TABLE [dbo].[Visiteur]
(
	[id_visiteur] INT NOT NULL PRIMARY KEY IDENTITY, 
    [nom_visiteur] NVARCHAR(MAX) NULL, 
    [prenom_visiteur] NVARCHAR(MAX) NULL, 
    [tel_visiteur] NVARCHAR(MAX) NOT NULL, 
    [email_visiteur] NVARCHAR(MAX) NOT NULL
)
