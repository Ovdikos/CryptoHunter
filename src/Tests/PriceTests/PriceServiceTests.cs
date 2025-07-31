using BLL.DTOs.Arbitrage;
using BLL.Interfaces;
using Moq;

namespace Tests.PriceTests
{
    public class PriceServiceTests
    {
        private readonly Mock<IExchangeService> _c1 = new();
        private readonly Mock<IExchangeService> _c2 = new();

        private PriceService CreateService() =>
            new(new[] { _c1.Object, _c2.Object });

        [Fact]
        public async Task GetHighestBid_ReturnsClientWithMaxAsk()
        {
            const string pair = "FOOUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 1m, Ask = 2m });

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 3m, Ask = 5m });

            var svc = CreateService();
            var result = await svc.GetHighestBid(pair);

            Assert.Equal(pair, result.Pair);
            Assert.Equal("Ex2", result.Exchange);
            Assert.Equal(5m,    result.Price);
        }

        [Fact]
        public async Task GetHighestBid_IgnoresFailingClients()
        {
            const string pair = "BARUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception("oops"));

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 2m, Ask = 4m });

            var svc = CreateService();
            var result = await svc.GetHighestBid(pair);

            Assert.Equal(pair,  result.Pair);
            Assert.Equal("Ex2", result.Exchange);
            Assert.Equal(4m,    result.Price);
        }

        [Fact]
        public async Task GetLowestAsk_ReturnsClientWithMinAsk()
        {
            const string pair = "BAZUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 6m, Ask = 7m });

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 2m, Ask = 3m });

            var svc = CreateService();
            var result = await svc.GetLowestAsk(pair);

            Assert.Equal(pair,  result.Pair);
            Assert.Equal("Ex2", result.Exchange);
            Assert.Equal(3m,    result.Price);
        }

        [Fact]
        public async Task GetLowestAsk_IgnoresFailingClients()
        {
            const string pair = "QUXUSDT";

            _c1.SetupGet(c => c.ExchangeName).Returns("Ex1");
            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception("fail"));

            _c2.SetupGet(c => c.ExchangeName).Returns("Ex2");
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new TickerResponseDto { Bid = 5m, Ask = 8m });

            var svc = CreateService();
            var result = await svc.GetLowestAsk(pair);

            Assert.Equal(pair,  result.Pair);
            Assert.Equal("Ex2", result.Exchange);
            Assert.Equal(8m,    result.Price);
        }

        [Fact]
        public async Task GetLowestAsk_Throws_WhenAllClientsFail()
        {
            const string pair = "NONEUSDT";

            _c1.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception());
            _c2.Setup(c => c.GetTicker(pair, It.IsAny<CancellationToken>()))
               .ThrowsAsync(new Exception());

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => svc.GetLowestAsk(pair));
        }
    }
}
