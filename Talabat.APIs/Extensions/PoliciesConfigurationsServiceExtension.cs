namespace Talabat.APIs.Extensions
{
    public static class PoliciesConfigurationsServiceExtension
    {
        public static IServiceCollection AddPolicesConfigurations(this IServiceCollection Services, IConfiguration config)
        {
            Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(config["FrontBaseUrl"])
                        .AllowCredentials();
                });
            });

            return Services;
        }
    }
}
