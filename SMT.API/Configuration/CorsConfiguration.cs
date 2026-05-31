namespace SMT.API.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.
                    WithOrigins(configuration["Frontend:CorsOrigins"] ?? string.Empty).
                    AllowAnyHeader().
                    WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS").
                    AllowCredentials();
            });
        });
        
        return services;
    }
}