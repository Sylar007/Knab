namespace Knab.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoCurrencyExchangeController : ControllerBase
    {
        private readonly IProviderService _exchangeProvider;

        public CryptoCurrencyExchangeController(IProviderService exchangeProvider)
        {
            _exchangeProvider = exchangeProvider;
        }

        [HttpGet("{symbol}")]
        [ProducesResponseType(typeof(ExchangeRatesList), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<ActionResult> GetCurrencyQuotesAsync(string symbol)
        {
            try
            {
                var exchangeRatesResult = await _exchangeProvider.GetExchangeRatesAsync(symbol);
                if (exchangeRatesResult != null)
                {
                    return this.Ok(exchangeRatesResult);
                }
                return this.NotFound();
            }
            catch (Exception ex)
            {
                return Problem($"Internal server error, there is a problem interacting with the clients. {ex.Message.ToString()}");
            }
            catch
            {
                return Problem($"Unable to get fiat currency Quotes for base currency:  {symbol}.");
            }
        }
    }
}
