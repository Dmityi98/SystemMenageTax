using MassTransit;
using SMT.Application.Consumers;

namespace SMT.API.Configuration;

public static class MassTransitConfiguration
{
    
    public static IServiceCollection AddMassTransitServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PaymentConsumer>();
    
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitHost = configuration.GetValue<string>("RabbitMQ:Host") ?? "rabbitmq://localhost:5672";
                cfg.Host(rabbitHost, h =>
                {
                    h.Username(configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest");
                    h.Password(configuration.GetValue<string>("RabbitMQ:Password") ?? "guest");
                });
                
                cfg.ConfigureEndpoints(context);
        
            });
        });

        return services;
    }
}