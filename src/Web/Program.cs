using DAL.Data;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using BLL.Services;
using Core.Config;
using DAL.Interfaces;
using DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration
                           .GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string not found");


builder.Services.AddDbContext<CryptoContext>(opts =>
    opts.UseSqlServer(connectionString));

builder.Services.AddScoped<ICurrencyPairRepository, CurrencyPairRepository>();



var exchangeConfigs = builder.Configuration
    .GetSection("Exchanges")
    .Get<List<ExchangeConfig>>();

var binanceConfig = exchangeConfigs
    .First(x => x.Name.Equals("Binance", StringComparison.OrdinalIgnoreCase));
var bybitConfig   = exchangeConfigs
    .First(x => x.Name.Equals("Bybit",   StringComparison.OrdinalIgnoreCase));
var bingxConfig   = exchangeConfigs
    .First(x => x.Name.Equals("BingX",   StringComparison.OrdinalIgnoreCase));
var mexcConfig   = exchangeConfigs
    .First(x => x.Name.Equals("MEXC",   StringComparison.OrdinalIgnoreCase));
var coinBaseConfig   = exchangeConfigs
    .First(x => x.Name.Equals("Coinbase",   StringComparison.OrdinalIgnoreCase));
var kuCoinConfig   = exchangeConfigs
    .First(x => x.Name.Equals("KuCoin",   StringComparison.OrdinalIgnoreCase));

// BINANCE
builder.Services
    .AddHttpClient<BinanceApiClient>(client =>
    {
        client.BaseAddress = new Uri(binanceConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeApiClient>(sp =>
    sp.GetRequiredService<BinanceApiClient>());

// BYBIT
builder.Services
    .AddHttpClient<BybitApiClient>(client =>
    {
        client.BaseAddress = new Uri(bybitConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeApiClient>(sp =>
    sp.GetRequiredService<BybitApiClient>());


// BINGX
builder.Services
    .AddHttpClient<BingxApiClient>(client =>
    {
        client.BaseAddress = new Uri(bingxConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeApiClient>(sp =>
    sp.GetRequiredService<BingxApiClient>());


// MEXC
builder.Services
    .AddHttpClient<MexcApiClient>(client =>
    {
        client.BaseAddress = new Uri(mexcConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeApiClient>(sp =>
    sp.GetRequiredService<MexcApiClient>());


// COINBASE
builder.Services
    .AddHttpClient<CoinbaseApiClient>(client =>
    {
        client.BaseAddress = new Uri(coinBaseConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeApiClient>(sp =>
    sp.GetRequiredService<CoinbaseApiClient>());


// KuCoin
builder.Services
    .AddHttpClient<KuCoinApiClient>(client =>
    {
        client.BaseAddress = new Uri(kuCoinConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeApiClient>(sp =>
    sp.GetRequiredService<KuCoinApiClient>());

builder.Services.AddTransient<IArbitrageService, ArbitrageService>();
builder.Services.AddTransient<IPriceService, PriceService>();
builder.Services.AddTransient<IMarketService, MarketService>();





var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();