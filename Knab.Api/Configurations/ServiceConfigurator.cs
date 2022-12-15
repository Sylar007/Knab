namespace Knab.Api.Configurations
{ /// <summary>
  /// Extension and helper methods for configuring the services for the host.
  /// The extension method <see cref="IServiceCOllection.InjectDependencies" /> is called for your
  /// specific services to be injected.
  /// </summary>
    internal static class ServiceConfigurator
    {

        /// <summary>
        /// Extension method for configuring the services in the <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureServices(this IWebHostBuilder builder) =>
            builder.ConfigureServices(ConfigureServices);

        /// <summary>
        /// Helper method for adding services to the the <see cref="WebHostBuilderContext" />.
        /// </summary>
        /// <param name="hostBuilderContext"></param>
        /// <param name="serviceCollection"></param>
        private static void ConfigureServices(WebHostBuilderContext hostBuilderContext,
            IServiceCollection serviceCollection)
        {
            // Add services to the container.

            serviceCollection.AddOptions();
            var appConfigurations = hostBuilderContext.Configuration.GetSection("AppConfigurations");
            var appConfigs = hostBuilderContext.Configuration.GetSection("AppConfigurations").Get<AppConfigurations>();
            serviceCollection.Configure<AppConfigurations>(appConfigurations);
            serviceCollection.AddControllers();
            serviceCollection.AddEndpointsApiExplorer()
                .AddSwagger(hostBuilderContext);

            serviceCollection.AddSingleton<IProviderService, ProviderService>();

            // NOTE: In order to separate the dependency injection concerns for each solution project,
            // in our case each Api Client (CoinMarketCap/ExchangeRatesApi).
            // an extension methods are created to handle its dependecies
            // This concept will help us when introducing a new 3rd party exchange's api.
            serviceCollection.InitiateCoinMarketService(appConfigs);
            serviceCollection.InitiatExchangeRateService(appConfigs);
        }
    }
}
