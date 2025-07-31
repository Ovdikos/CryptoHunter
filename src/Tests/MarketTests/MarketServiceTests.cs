using BLL.DTOs._24hStat;
using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using Moq;

namespace Tests.MarketTests
{
    public class MarketServiceTests
    {
        private readonly Mock<IExchangeService> _ex1 = new();
        private readonly Mock<IExchangeService> _ex2 = new();
        private readonly Mock<ICurrencyPairRepository> _repo = new();

        private MarketService CreateService()
            => new(new[] { _ex1.Object, _ex2.Object }, _repo.Object);

        [Fact]
        public async Task Get24hSummary_AggregatesCorrectly()
        {
            const string pair = "AAAUSDT";

            SetupStats(_ex1, "Ex1", 1m,3m,1m,2m,10m,1.5m);
            SetupStats(_ex2, "Ex2",2m,5m,2m,4m,20m,3.5m);

            var svc = CreateService();

            var summary = await svc.Get24hSummary(pair);

            Assert.Equal(2, summary.PerExchange.Count);
            Assert.Contains(summary.PerExchange, x => x.Exchange=="Ex1" && x.Volume==10m);
            Assert.Contains(summary.PerExchange, x => x.Exchange=="Ex2" && x.Volume==20m);

            Assert.Equal(pair, summary.Pair);
            Assert.Equal(Math.Round((1m*10m+2m*20m)/30m,6), Math.Round(summary.Aggregated.Open,6));
            Assert.Equal(5m, summary.Aggregated.High);
            Assert.Equal(1m, summary.Aggregated.Low);
            Assert.Equal(Math.Round((2m*10m+4m*20m)/30m,6), Math.Round(summary.Aggregated.Close,6));
            var wo = (1m*10m+2m*20m)/30m;
            var wc = (2m*10m+4m*20m)/30m;
            var pct = Math.Round((wc - wo)/wo*100m, 6);
            Assert.Equal(pct, Math.Round(summary.Aggregated.PriceChangePct,6));
            Assert.Equal(Math.Round((1.5m*10m+3.5m*20m)/30m,6),
                         Math.Round(summary.Aggregated.WeightedAvgPrice,6));
        }

        private void SetupStats(
            Mock<IExchangeService> mock, string name,
            decimal open, decimal high, decimal low,
            decimal close, decimal volume, decimal weightedAvg)
        {
            mock.SetupGet(x => x.ExchangeName).Returns(name);
            mock.Setup(x => x.Get24hStats(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Exchange24hDto {
                    Exchange = name,
                    Pair     = "IGNORED",
                    Open     = open,
                    High     = high,
                    Low      = low,
                    Close    = close,
                    Volume   = volume,
                    PriceChangePct   = 0,
                    WeightedAvgPrice = weightedAvg
                });
        }

        [Fact]
        public async Task Get24hSummary_Throws_WhenNoData()
        {
            _ex1.Setup(x => x.Get24hStats(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            _ex2.Setup(x => x.Get24hStats(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => svc.Get24hSummary("X"));
        }
        
        
        [Fact]
        public async Task Convert_CalculatesCorrectly()
        {
            _ex1.SetupGet(x => x.ExchangeName).Returns("Ex1");
            _ex1.Setup(x => x.GetTicker("XUSDT", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TickerResponseDto { Bid = 2m, Ask = 4m });
            _ex2.SetupGet(x => x.ExchangeName).Returns("Ex2");
            _ex2.Setup(x => x.GetTicker("XUSDT", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TickerResponseDto { Bid = 3m, Ask = 5m });
            _ex1.SetupGet(x => x.ExchangeName).Returns("Ex1");
            _ex1.Setup(x => x.GetTicker("YUSDT", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TickerResponseDto { Bid = 0m, Ask = 1m });
            _ex2.SetupGet(x => x.ExchangeName).Returns("Ex2");
            _ex2.Setup(x => x.GetTicker("YUSDT", It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TickerResponseDto { Bid = 0m, Ask = 2m });

            var svc = CreateService();
            var result = await svc.Convert("XUSDT","YUSDT", 10m);

            Assert.Equal(30m, result.UsdtAmount);
            Assert.Equal(30m, result.ToAmount);
            Assert.Equal("Ex2", result.BidExchange);
            Assert.Equal("Ex1", result.AskExchange);
        }
    }
}
