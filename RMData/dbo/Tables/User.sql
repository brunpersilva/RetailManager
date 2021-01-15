CREATE TABLE [dbo].[User]
(
	[Id] nvarchar(128) not null PRIMARY KEY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAdress] NVARCHAR(256) NOT NULL, 
    [CreateDate] DATETIME2 NOT NULL DEFAULT getutcdate(),
)
