namespace Knab.Exchangerates.Client
{
    public class ExchangeRatesResponse
    {
        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { set; get; }
        [JsonProperty("base")]
        public string BaseCurrency { set; get; }
    }
}
