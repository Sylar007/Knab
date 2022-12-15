namespace Knab.Api.Configurations
{
    internal static class SwaggerConfigurator
    {
        #region IServiceCollection

        public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection,
            WebHostBuilderContext webHostBuilderContext)
        {
            var applicationConfiguration = webHostBuilderContext
                .Configuration
                .GetSection("AppConfigurations")
                .Get<AppConfigurations>();

            serviceCollection.AddSwaggerGen();

            return serviceCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="isDevelopment"></param>
        /// <param name="serviceConfigurations"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerGen(this IApplicationBuilder applicationBuilder,
            bool isDevelopment,
            AppConfigurations appConfigurations)
        {
            applicationBuilder.UseSwagger(appConfigurations);

            return applicationBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="serviceConfigurations"></param>
        /// <returns></returns>
        private static IApplicationBuilder UseSwagger(this IApplicationBuilder applicationBuilder,
            AppConfigurations appConfigurations)
        {
            applicationBuilder
                .UseDeveloperExceptionPage()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Knab Swagger API");
                });

            return applicationBuilder;
        }

        #endregion
    }
}
