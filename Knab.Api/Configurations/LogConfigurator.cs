namespace Knab.Api.Configurations
{
    internal static class LogConfigurator
    {
        private static readonly object _syncLock = new();
        private static Logger _logger;



        /// <summary>
        /// Extension method for setting up logging configuration for an instance of <see cref="IWebHostBuilder" />.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureLogging(this IWebHostBuilder builder) =>
            builder.ConfigureLogging(ConfigureLogging);

        /// <summary>
        /// Sets up logging configuration for the <see cref="WebHostBuilderContext" />.
        /// </summary>
        /// <param name="builderContext"></param>
        /// <param name="loggingBuilder"></param>
        private static void ConfigureLogging(WebHostBuilderContext builderContext,
            ILoggingBuilder loggingBuilder)
        {
            var appConfiguration = builderContext
                .Configuration
                .GetSection("AppConfigurations")
                .Get<AppConfigurations>();

            ConfigureLogging(loggingBuilder, appConfiguration);
        }

        /// <summary>
        /// Sets up logging configuration for the <see cref="ILoggingBuilder" />.
        /// </summary>
        /// <param name="loggingBuilder"></param>
        /// <param name="serviceConfiguration"></param>
        private static void ConfigureLogging(ILoggingBuilder loggingBuilder,
            AppConfigurations appConfiguration)
        {
            // This is to ensure that both the Host and WebHost are using the same logger configuration

            if (_logger == null)
            {
                lock (_syncLock)
                {
                    var loggerConfiguration = CreateLoggerConfiguration(appConfiguration);

                    ConfigureConsole(loggerConfiguration, appConfiguration);
                    ConfigureRollingFile(loggerConfiguration, appConfiguration);

                    _logger = loggerConfiguration.CreateLogger();
                }
            }

            loggingBuilder.AddSerilog(_logger, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationConfiguration"></param>
        /// <returns></returns>
        private static LoggerConfiguration CreateLoggerConfiguration(AppConfigurations appConfiguration)
        {
            var levelSwitch = new LoggingLevelSwitch()
            {
                MinimumLevel = appConfiguration.Logging.MimimumLevel
            };

            return new LoggerConfiguration()
               .MinimumLevel.ControlledBy(levelSwitch)
               .MinimumLevel.Override("Microsoft", levelSwitch.MinimumLevel)
               .Enrich.FromLogContext()
               .Destructure.ToMaximumDepth(10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="applicationConfiguration"></param>
        private static void ConfigureConsole(LoggerConfiguration loggerConfiguration,
            AppConfigurations appConfiguration)
        {
            if (appConfiguration.LoggingSinks.Console.Enabled)
            {
                loggerConfiguration
                    .WriteTo
                    .Console(outputTemplate: appConfiguration.LoggingSinks.Console.OutputTemplate,
                        restrictedToMinimumLevel: appConfiguration.LoggingSinks.Console.LogLevel,
                        theme: SystemConsoleTheme.Literate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <param name="applicationConfiguration"></param>
        private static void ConfigureRollingFile(LoggerConfiguration loggerConfiguration,
            AppConfigurations appConfiguration)
        {
            if (appConfiguration.LoggingSinks.RollingFile.Enabled)
            {
                EnsureDirectory(appConfiguration.LoggingSinks.RollingFile.Location);

                var fileName = $"Knab.{appConfiguration.LoggingSinks.RollingFile.Extension}";
                var filePath = Path.Combine(appConfiguration.LoggingSinks.RollingFile.Location, fileName);
                loggerConfiguration
                    .WriteTo
                    .File(filePath,
                        rollingInterval: appConfiguration.LoggingSinks.RollingFile.RollingInterval,
                        restrictedToMinimumLevel: appConfiguration.LoggingSinks.RollingFile.MimimumLevel,
                        outputTemplate: appConfiguration.LoggingSinks.RollingFile.OutputTemplate);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sinkFolder"></param>
        private static void EnsureDirectory(string sinkFolder)
        {
            if (!Directory.Exists(sinkFolder))
            {
                Directory.CreateDirectory(sinkFolder);
            }
        }
    }
}
