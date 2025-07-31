using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using BLL.Services;
using Core.Config;
using DAL.Context;
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
    .AddHttpClient<BinanceService>(client =>
    {
        client.BaseAddress = new Uri(binanceConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeService>(sp =>
    sp.GetRequiredService<BinanceService>());

// BYBIT
builder.Services
    .AddHttpClient<BybitService>(client =>
    {
        client.BaseAddress = new Uri(bybitConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeService>(sp =>
    sp.GetRequiredService<BybitService>());


// BINGX
builder.Services
    .AddHttpClient<BingxService>(client =>
    {
        client.BaseAddress = new Uri(bingxConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeService>(sp =>
    sp.GetRequiredService<BingxService>());


// MEXC
builder.Services
    .AddHttpClient<MexcService>(client =>
    {
        client.BaseAddress = new Uri(mexcConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeService>(sp =>
    sp.GetRequiredService<MexcService>());


// COINBASE
builder.Services
    .AddHttpClient<CoinbaseService>(client =>
    {
        client.BaseAddress = new Uri(coinBaseConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeService>(sp =>
    sp.GetRequiredService<CoinbaseService>());


// KuCoin
builder.Services
    .AddHttpClient<KuCoinService>(client =>
    {
        client.BaseAddress = new Uri(kuCoinConfig.BaseUrl);
    });
builder.Services.AddTransient<IExchangeService>(sp =>
    sp.GetRequiredService<KuCoinService>());

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