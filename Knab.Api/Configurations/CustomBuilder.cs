namespace Knab.Api.Configurations
{
    /// <summary>
    /// A default provided class that is being used for building the WebApplication.
    /// </summary>
    public static class CustomBuilder
    {
        private static AppConfigurations _appConfigurations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static async Task<WebApplication> ExecuteAsync(this WebApplicationBuilder builder,
            CancellationToken cancellationToken = default)
        {
            var webHost = builder.WebHost;

            // Configurations
            webHost
                .ConfigureAppConfiguration()
                .ConfigureServices()
                .ConfigureLogging();

            // Configuration File
            _appConfigurations = builder
                .Configuration
                .GetSection("AppConfigurations")
                .Get<AppConfigurations>();

            // App initiations
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Mappings
            app.MapControllers();

            // Swagger
            app.UseSwaggerGen(true, _appConfigurations);

            // Run
            await app
                .RunAsync(cancellationToken);
            return app;
        }
    }
}
