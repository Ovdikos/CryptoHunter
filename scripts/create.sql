CREATE TABLE Currencies (
                            CurrencyId   INT           IDENTITY PRIMARY KEY,
                            Symbol       NVARCHAR(10)  NOT NULL UNIQUE,  
                            Name         NVARCHAR(50)  NOT NULL
);

CREATE TABLE CurrencyPairs (
                               PairId       INT           IDENTITY PRIMARY KEY,
                               FromCurrency INT           NOT NULL FOREIGN KEY REFERENCES Currencies(CurrencyId),
                               ToCurrency   INT           NOT NULL FOREIGN KEY REFERENCES Currencies(CurrencyId),
                               UNIQUE(FromCurrency, ToCurrency)
);