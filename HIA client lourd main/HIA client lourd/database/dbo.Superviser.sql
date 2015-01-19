CREATE TABLE [dbo].[Superviser] (
    [id_personnel] INT NOT NULL,
    [id_service]   INT NULL,
    CONSTRAINT [FK_Superviser_Personnel] FOREIGN KEY ([id_personnel]) REFERENCES [dbo].[Personnel] ([id_personnel]),
    CONSTRAINT [FK_Superviser_Service] FOREIGN KEY ([id_service]) REFERENCES [dbo].[Service] ([id_service])
);

