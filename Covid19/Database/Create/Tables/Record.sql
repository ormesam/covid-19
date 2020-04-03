CREATE TABLE [Record] (
    [RecordId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,

    [CountryId] int NOT NULL CONSTRAINT [FK_Record_Country] REFERENCES [Country],
    [Date] datetime NOT NULL,
    [AccumulatedConfirmed] int NOT NULL,
    [AccumulatedRecovered] int NOT NULL,
    [AccumulatedDeaths] int NOT NULL,
    [NewConfirmed] int NOT NULL,
    [NewRecovered] int NOT NULL,
    [NewDeaths] int NOT NULL,
)