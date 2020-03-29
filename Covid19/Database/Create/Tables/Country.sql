CREATE TABLE [Country] (
    [CountryId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,

    [Name] nvarchar(255) NOT NULL,
    [CurrentConfirmed] int NOT NULL,
    [CurrentRecovered] int NOT NULL,
    [CurrentDeaths] int NOT NULL,
)