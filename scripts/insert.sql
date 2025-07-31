DECLARE @Pairs TABLE (PairName VARCHAR(10) NOT NULL);
INSERT INTO @Pairs (PairName) VALUES
                                  ('ACHUSDT'), ('ADAUSDT'), ('AGLDUSDT'), ('APEUSDT'), ('APTUSDT'),
                                  ('ATOMUSDT'), ('AVAXUSDT'), ('AXSUSDT'), ('BTCUSDT'), ('CHZUSDT'),
                                  ('DOGEUSDT'), ('DOTUSDT'), ('ENJUSDT'), ('ENSUSDT'), ('ETHUSDT'),
                                  ('FETUSDT'), ('FIDAUSDT'), ('FLOWUSDT'), ('HBARUSDT'), ('HFTUSDT'),
                                  ('ICPUSDT'), ('IMXUSDT'), ('JASMYUSDT'), ('KSMUSDT'), ('LINKUSDT'),
                                  ('LRCUSDT'), ('MASKUSDT'), ('MINAUSDT'), ('NEARUSDT'), ('OPUSDT'),
                                  ('PERPUSDT'), ('QNTUSDT'), ('ROSEUSDT'), ('SANDUSDT'), ('SHIBUSDT'),
                                  ('SOLUSDT'), ('STGUSDT'), ('STXUSDT'), ('XLMUSDT'), ('XRPUSDT');

INSERT INTO Currencies (Symbol, Name)
SELECT U.Symbol, U.Symbol AS Name
FROM (
         SELECT DISTINCT LEFT(PairName, LEN(PairName) - 4) AS Symbol
         FROM @Pairs
         UNION
         SELECT DISTINCT RIGHT(PairName, 4) AS Symbol
         FROM @Pairs
     ) AS U
WHERE NOT EXISTS (
    SELECT 1
    FROM Currencies C
    WHERE C.Symbol = U.Symbol
);

INSERT INTO CurrencyPairs (FromCurrency, ToCurrency)
SELECT
    C1.CurrencyId,
    C2.CurrencyId
FROM @Pairs P
         JOIN Currencies C1
              ON C1.Symbol = LEFT(P.PairName, LEN(P.PairName) - 4)
    JOIN Currencies C2
ON C2.Symbol = RIGHT(P.PairName, 4)
WHERE NOT EXISTS (
    SELECT 1
    FROM CurrencyPairs CP
    WHERE CP.FromCurrency = C1.CurrencyId
  AND CP.ToCurrency   = C2.CurrencyId
    );