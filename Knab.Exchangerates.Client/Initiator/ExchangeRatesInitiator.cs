
namespace Knab.Exchangerates.Client
{
    public static class ExchangeRatesInitiator
    {
        public static void InitiatExchangeRateService(this IServiceCollection services, AppConfigurations appConfigurations)
        {
            // Register CointMarketcap's dependencies
            services.AddHttpClient("ExchangeRatesApi", httpclient =>
            {
                httpclient.BaseAddress = new Uri(appConfigurations.ExchangeratesApi.ServiceBaseUrl);
                httpclient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpclient.DefaultRequestHeaders.Add("apikey", appConfigurations.ExchangeratesApi.AccessKey);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Reuse the handlers within 5 minutes lifetime
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddSingleton<IApiClientService, ExchangeRatesService>();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            //retry when other than success result is returned.
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != System.Net.HttpStatusCode.OK)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            // Break the request circuit after 5 retries for 30 seconds 
            return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
