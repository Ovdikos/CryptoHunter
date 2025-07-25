CREATE TABLE Exchanges (
                           ExchangeId   INT           IDENTITY PRIMARY KEY,
                           Name         NVARCHAR(100) NOT NULL,
                           ApiBaseUrl   NVARCHAR(200) NOT NULL,
                           ApiKey       NVARCHAR(200) NULL,
                           IsActive     BIT           NOT NULL DEFAULT 1
);

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

CREATE TABLE ExchangeRates (
                               RateId       BIGINT        IDENTITY PRIMARY KEY,
                               ExchangeId   INT           NOT NULL FOREIGN KEY REFERENCES Exchanges(ExchangeId),
                               PairId       INT           NOT NULL FOREIGN KEY REFERENCES CurrencyPairs(PairId),
                               Rate         DECIMAL(28,8) NOT NULL,
                               RetrievedAt  DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE FetchSchedules (
                                ExchangeId   INT        PRIMARY KEY FOREIGN KEY REFERENCES Exchanges(ExchangeId),
                                IntervalSec  INT        NOT NULL DEFAULT 60
);
