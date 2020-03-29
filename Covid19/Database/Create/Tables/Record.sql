CREATE TABLE [Record] (
    [RecordId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,

    [CountryId] int NOT NULL CONSTRAINT [FK_Record_Country] REFERENCES [Country],
    [Date] datetime NOT NULL,
    [Confirmed] int NOT NULL,
    [Recovered] int NOT NULL,
    [Deaths] int NOT NULL,
)