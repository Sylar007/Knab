namespace Knab.CoinMarketCap.Client
{
    public class CoinMarketService : IApiClientService
    {
        protected readonly AppConfigurations _appConfigs;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CoinMarketService> _logger;

        public string Name { get { return "CoinMarketService"; } }

        public CoinMarketService(ILogger<CoinMarketService> logger, IOptions<AppConfigurations> appConfigs, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _appConfigs = appConfigs.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ExchangeRatesList> GetExchangeRatesList(string BaseCurrencySymbol, List<string> targetCurrencies)
        {
            BaseCurrencySymbol = BaseCurrencySymbol.ToUpper();
            var currencies = targetCurrencies.Select(e => e.ToUpper()).ToArray();

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var query = HttpUtility.ParseQueryString(String.Empty);
                query["symbol"] = BaseCurrencySymbol;
                query["convert"] = string.Join(",", currencies);
                query["aux"] = "is_active"; 
                var request = new HttpRequestMessage(HttpMethod.Get, $"/{_appConfigs.CoinmarketcapApi.Version}/{_appConfigs.CoinmarketcapApi.QuotesEndpoint}?{query}");
                request.Headers.Add(_appConfigs.CoinmarketcapApi.APIKeyName, _appConfigs.CoinmarketcapApi.APIKeyValue);
                var httpclient = _httpClientFactory.CreateClient("CoinmarketcapApi");
                response = await httpclient.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                var coinmarketResponse = JsonConvert.DeserializeObject<QuoteResponse>(responseString);
                if (response.IsSuccessStatusCode)
                {
                    if (coinmarketResponse?.Status?.ErrorCode == 0
                        && coinmarketResponse.Status.ErrorMessage == null
                        && coinmarketResponse.Status.CreditCount > 0
                        && coinmarketResponse.Data?.Count > 0
                        && coinmarketResponse.Data.Count > 0
                        && coinmarketResponse.Data.ContainsKey(BaseCurrencySymbol))
                    {
                        return new ExchangeRatesList()
                        {
                            BaseCurrencySymbol = coinmarketResponse.Data[BaseCurrencySymbol][0].Symbol,
                            CurrenciesRates = coinmarketResponse.Data[BaseCurrencySymbol][0].Quote.Where(e => currencies.Contains(e.Key)).ToDictionary(key => key.Key, val => val.Value.Price)
                        };
                    }

                }
                return new ExchangeRatesList();
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("CoinMarketApi Status Code returned {0}", response.StatusCode));
                _logger.LogError("Error ->", ex.Message.ToString());
                throw;
            }
            return new ExchangeRatesList();
        }
    }
}
