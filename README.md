# CryptoHunter

## Project Overview

CryptoHunter is a mini crypto-exchange aggregator project written in C#. Its main purpose is to provide unified access to real-time cryptocurrency trading data from multiple popular exchanges, including Binance, Bybit, BingX, MEXC, Coinbase, and KuCoin. The application fetches ticker information and 24-hour statistics for supported trading pairs, making it useful for price comparisons, market analytics, or as a starting point for building more advanced crypto trading or arbitrage tools.

The project is still in an early stage and can be expanded significantly, for example by adding more exchanges, supporting trading operations, providing user authentication, or implementing arbitrage strategies.

## Architecture and Main Components

- **Exchange Clients:** Each supported exchange (Binance, Bybit, BingX, MEXC, Coinbase, KuCoin) has its own API client implementing the `IExchangeApiClient` interface. These clients fetch ticker data and 24h statistics from their respective APIs.
- **Repository Layer:** Used for storing and retrieving supported currency pairs from the database.
- **Service Layer:** Provides business logic and data aggregation for exchange data.
- **Web Layer:** ASP.NET Core Web API, with controllers exposing endpoints for client applications.

## API Endpoints

Below is a description of all available endpoints in the CryptoHunter mini crypto-exchange project:

---

### 1. GET `/maxbid/{coin}`

**Description:**  
Returns the maximum bid price for the specified cryptocurrency (symbol: `{coin}`) among all supported exchanges.  
**Parameters:**  
- `coin` (path): The symbol of the cryptocurrency (e.g., `BTC`, `ETH`).

**Response:**  
- On success: Returns the exchange with the maximum bid price for the given coin.
- On failure: Returns a not found response if no data is available.

---

### 2. GET `/minbid/{coin}`

**Description:**  
Returns the minimum bid price for the specified cryptocurrency (symbol: `{coin}`) among all supported exchanges.  
**Parameters:**  
- `coin` (path): The symbol of the cryptocurrency.

**Response:**  
- On success: Returns the exchange with the minimum bid price for the given coin.
- On failure: Returns a not found response if no data is available.

---

### 3. GET `/opportunity/{coin}`

**Description:**  
Retrieves an arbitrage opportunity for the specified cryptocurrency against USDT.  
**Parameters:**  
- `coin` (path): The symbol of the cryptocurrency (e.g., `BTC`).

**Response:**  
- On success: Returns arbitrage data for the specified pair (e.g., `BTCUSDT`), including data required to identify an opportunity.
- On failure: Returns a not found response if no data is found for the given pair.

---

### 4. GET `/database/{pair}`

**Description:**  
Fetches all available quotes for a trading pair from the database, calculates the best buy and sell exchanges, and provides the spread information.  
**Parameters:**  
- `pair` (path): The trading pair symbol (e.g., `BTCUSDT`).

**Response:**  
- On success:  
  - `pair`: The trading pair.
  - `bestBuy`: Exchange and price with the lowest ask.
  - `bestSell`: Exchange and price with the highest bid.
  - `spread`: Both absolute and percentage values.
  - `allQuotes`: List of all quotes for this pair.
- On failure: Returns not found if no quotes exist for the pair.

---

### 5. GET `/toppairs?type={type}&limit={limit}`

**Description:**  
Returns a list of top trading pairs by gainers or losers, depending on the `type` parameter.  
**Parameters:**  
- `type` (query): Type of pairs to return; must be either `gainers` or `losers`.
- `limit` (query, optional): The number of pairs to return (default is 10).

**Response:**  
- On success: Returns a list of top pairs, sorted by the requested type.
- On failure: Returns a bad request if parameters are invalid.

---

### 6. GET `/converter?from={from}&to={to}&amount={amount}`

**Description:**  
Converts a specified amount from one cryptocurrency to another using current exchange rates.  
**Parameters:**  
- `from` (query): Source cryptocurrency symbol.
- `to` (query): Target cryptocurrency symbol.
- `amount` (query): Amount to convert (must be positive).

**Response:**  
- On success: Returns the conversion result including the calculated amount.
- On failure: Returns a bad request if parameters are missing or invalid.

## How to Run the Project

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Ovdikos/CryptoHunter.git
   cd CryptoHunter
   ```

2. **Configure the database:**
   - Update the `DefaultConnection` in your configuration file with your SQL Server credentials.

 ```bash
   {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "input here your connection string"
  },
  "Exchanges" : [
    {
      "Name" : "Binance",
      "BaseUrl" : "https://api.binance.com",
      "ApiKey" : "for future"
    },
    {
      "Name" : "Bybit",
      "BaseUrl" : "https://api.bybit.com",
      "ApiKey" : "for future"
    },
    {
      "Name" : "BingX",
      "BaseUrl" : "https://open-api.bingx.com",
      "ApiKey" : "for future"
    },
    {
      "Name" : "MEXC",
      "BaseUrl" : "https://api.mexc.com",
      "ApiKey" : "for future"
    },
    {
      "Name" : "Coinbase",
      "BaseUrl" : "https://api.exchange.coinbase.com",
      "ApiKey" : "for future"
    },
    {
      "Name" : "KuCoin",
      "BaseUrl" : "https://api.kucoin.com",
      "ApiKey" : "for future"
    }
  ]
}

   ```

3. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

4. **Build and run the project:**
   ```bash
   dotnet run --project src/Web/
   ```

## Extensibility & Future Improvements

CryptoHunter is not a production-ready exchange but a foundation for more complex crypto tools. There is a lot of space for future enhancements, such as:

- Adding more exchanges and advanced data aggregation.
- Supporting trading actions (buy, sell, order book, etc.).
- Implementing user authentication and authorization.
- Adding more analytics, dashboards, and arbitrage opportunities.
- Improving error handling and resilience for exchange API failures.
- Deploying to cloud or Docker for production use.

---

CryptoHunter is a flexible starting point for building crypto market data tools. Contributions and extensions are encouraged!
