
namespace Knab.Core
{
    public class AppConfigurations
    {
        public CoinmarketcapApi CoinmarketcapApi { get; set; }
        public ExchangeratesApi ExchangeratesApi { get; set; }
        public LoggingSinks LoggingSinks { get; set; }
        public Logging Logging { get; set; }
    }
    public class CoinmarketcapApi
    {
        public bool Enabled { get; set; }
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string QuotesEndpoint { get; set; }
        public string APIKeyName { get; set; }
        public string APIKeyValue { get; set; }
        public string ExchangeName { get; set; }
        public List<string> TargetCurrencies { get; set; }
    }

    public class ExchangeratesApi
    {
        public bool Enabled { get; set; }
        public string ServiceBaseUrl { get; set; }
        public string ExchangeRateEndpoint { get; set; }
        public List<string> TargetedCurrencies { get; set; }
        public string Version { get; set; }
        public string AccessKey { get; set; }
        public string ExchangeName { get; set; }
    }

    public class Console
    {
        public bool Enabled { get; set; }
        public string OutputTemplate { get; set; }
        public LogEventLevel LogLevel { get; set; }
    }

    public class RollingFile
    {
        public bool Enabled { get; set; }
        public string Location { get; set; }
        public string Extension { get; set; }
        public string OutputTemplate { get; set; }
        public RollingInterval RollingInterval { get; set; }
        public LogEventLevel MimimumLevel { get; set; }
    }

    public class LoggingSinks
    {
        public Console Console { get; set; }
        public RollingFile RollingFile { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
        public LogEventLevel MimimumLevel { get; set; }

    }
}
