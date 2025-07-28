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

var binanceConfig = builder.Configuration
    .GetSection("Exchanges")
    .Get<List<ExchangeConfig>>()
    .First(x => x.Name == "Binance");

builder.Services
    .AddHttpClient<IExchangeApiClient, BinanceApiClient>(client =>
    {
        client.BaseAddress = new Uri(binanceConfig.BaseUrl);
    });

var app = builder.Build();

// 6. Пайплайн
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();