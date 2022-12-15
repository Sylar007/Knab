
namespace Knab.Exchangerates.Client.UnitTest
{
    /// <summary>
    /// Naming Convention
    /// The name of the test consists of three parts: 
    /// 1. The name of the method being tested. 
    /// 2. The scenario under which it's being tested. 
    /// 3. The expected behavior when the scenario is invoked.
    /// </summary>

    [TestClass]
    public class ExchangeRatesServiceUnitTest
    {
        private readonly string _exchangeApiUrl = "https://api.apilayer.com/exchangerates_data/";
        private readonly string _accessKey = "NTkQd5YVKIgsbPKh75ZyysbekUSgpXCX";
        
        private const decimal BTCVALUE = 40000.1M;


        [TestMethod]
        [DataRow("BTC", "USD")]
        public void GetExchangeRatesList_invokeApiEndpoint_ShouldReturnResult(string baseCurrency, string targetCurrencies)
        {
            //Arrange

            var listOfTargetCurrencies = new List<string>();
            listOfTargetCurrencies.Add(targetCurrencies);
            var httpClientStub = new Mock<IHttpClientFactory>();
            var logger = Mock.Of<ILogger<ExchangeRatesService>>();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            string jsonResponse = @" { 'base': '" + baseCurrency + "' } ";
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // use real http client with mocked handler here
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(_exchangeApiUrl),
            };
            httpClientStub.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            //mockconfiguration
            var appConfigurations = new AppConfigurations();
            appConfigurations.ExchangeratesApi = new ExchangeratesApi()
            {
                ServiceBaseUrl = _exchangeApiUrl,
                AccessKey = _accessKey,
                ExchangeRateEndpoint = "/latest",
                TargetedCurrencies = listOfTargetCurrencies
            };

            // We need to set the Value of IOptions to be the ServiceConfigurations Class
            var configStub = new Mock<IOptions<AppConfigurations>>();
            configStub.Setup(ap => ap.Value).Returns(appConfigurations);
            ExchangeRatesService exchangeRatesService = new ExchangeRatesService(logger, configStub.Object, httpClientStub.Object);

            //Act
            var result = exchangeRatesService.GetExchangeRatesList(baseCurrency, listOfTargetCurrencies).Result;

            //Assert
            Assert.AreEqual(baseCurrency, result.BaseCurrencySymbol);
        }
    }
}
