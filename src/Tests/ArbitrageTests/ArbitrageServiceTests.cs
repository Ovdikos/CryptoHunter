using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using BLL.Services;
using Moq;

namespace Tests.ArbitrageTests
{
    public class ArbitrageServiceTests
    {
        private readonly Mock<IExchangeService> _c1 = new();
        private readonly Mock<IExchangeService> _c2 = new();

        private ArbitrageService CreateService()
            => new(new[] { _c1.Object, _c2.Object });

        [Fact]
        public async Task GetQuotes_ReturnsAllExchanges()
        {
            const string pair = "ABCUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 1.1m, Ask = 1.2m });

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 2.1m, Ask = 2.2m });

            var svc = CreateService();

            var quotes = (await svc.GetQuotes(pair)).ToList();
            Assert.Equal(2, quotes.Count);

            Assert.Contains(quotes, q =>
                q.Exchange == "Ex1" && q.Bid == 1.1m && q.Ask == 1.2m);

            Assert.Contains(quotes, q =>
                q.Exchange == "Ex2" && q.Bid == 2.1m && q.Ask == 2.2m);
        }

        [Fact]
        public async Task GetOpportunity_PicksBestBuyAndSell()
        {
            const string pair = "XYZUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 5.0m, Ask = 6.0m });

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 7.0m, Ask = 8.0m });

            var svc = CreateService();

            var opp = await svc.GetOpportunity(pair);

            Assert.Equal(pair, opp.Pair);
            Assert.Equal("Ex1", opp.Buy.Exchange);
            Assert.Equal(6.0m, opp.Buy.Price);
            Assert.Equal("Ex2", opp.Sell.Exchange);
            Assert.Equal(7.0m, opp.Sell.Price);
        }

        [Fact]
        public async Task GetOpportunity_IgnoresClientsWithExceptions()
        {
            const string pair = "ZZZUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new InvalidOperationException());

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 3m, Ask = 4m });

            var svc = CreateService();

            var opp = await svc.GetOpportunity(pair);

            Assert.Equal(pair, opp.Pair);
            Assert.Equal("Ex2", opp.Buy.Exchange);
            Assert.Equal(4m, opp.Buy.Price);
            Assert.Equal("Ex2", opp.Sell.Exchange);
            Assert.Equal(3m, opp.Sell.Price);
        }

        [Fact]
        public async Task GetOpportunity_Throws_WhenNoData()
        {
            const string pair = "NODATA";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception("fail1"));

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception("fail2"));

            var svc = CreateService();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => svc.GetOpportunity(pair));

            Assert.Contains("No data for pair", ex.Message);
        }
    }
}
