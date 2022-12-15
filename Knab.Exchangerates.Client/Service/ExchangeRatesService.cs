
namespace Knab.Exchangerates.Client
{
    public class ExchangeRatesService : IApiClientService
    {

        protected readonly AppConfigurations _appConfigs;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ExchangeRatesService> _logger;

        public string Name { get { return "ExchangeRatesService"; } }

        public ExchangeRatesService(ILogger<ExchangeRatesService> logger, IOptions<AppConfigurations> appConfigs, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _appConfigs = appConfigs.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ExchangeRatesList> GetExchangeRatesList(string baseCurrencySymbol, List<string> targetCurrencies)
        {

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var query = HttpUtility.ParseQueryString(String.Empty);
                query["base"] = baseCurrencySymbol;
                query["symbols"] = string.Join(",", targetCurrencies.Select(e => e.ToUpper()).ToArray());
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_appConfigs.ExchangeratesApi.ExchangeRateEndpoint}?{query}");
                var httpclient = _httpClientFactory.CreateClient("ExchangeRatesApi");
                response = await httpclient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var exchangeRateApiResponse = JsonConvert.DeserializeObject<ExchangeRatesResponse>(responseString);
                if (response.IsSuccessStatusCode)
                {

                    return new ExchangeRatesList()
                    {
                        BaseCurrencySymbol = exchangeRateApiResponse.BaseCurrency,
                        CurrenciesRates = exchangeRateApiResponse.Rates
                    };
                }
            }
            catch (Exception ex)
            {
                //throw will have a stack trace with it.
                _logger.LogError(String.Format("ExchangeRatesApi Status Code returned {0}", response.StatusCode));
                _logger.LogError("Error ->", ex.Message.ToString());
                throw;
            }
            return new ExchangeRatesList();
        }
    }




}
