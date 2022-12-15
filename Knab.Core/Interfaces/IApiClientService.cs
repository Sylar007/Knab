namespace Knab.Core
{
    public interface IApiClientService
    {
        Task<ExchangeRatesList> GetExchangeRatesList(string baseCurrencySymbol, List<string> targetCurrencies);
        string Name { get; }
    }
}
