using Flurl.Http;
using Hash.Products.Domain.Commands;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Hash.Products.IntegratedTests
{
    public class ProductControllerTests
    {
        private readonly IFlurlClient _client;

        public ProductControllerTests()
        {
            var server = new ServerFixture();
            _client = new FlurlClient(server.Client);
        }

        [Theory]
        [AutoDataSubstitute]
        public async Task Shoud_Be_Success_When_Endpoint_Requested(BuyProductsCommand command)
        {
            var response = await _client.Request("/api/v1/products")
                                        .PostJsonAsync(command);

            Assert.True(response.StatusCode == (int)HttpStatusCode.OK);
        }
    }
}
