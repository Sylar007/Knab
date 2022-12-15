namespace Knab.Core
{
    public interface IProviderService
    {
        Task<ExchangeRatesList> GetExchangeRatesAsync(string BaseCryptocurrencySymbol);
    }
}
