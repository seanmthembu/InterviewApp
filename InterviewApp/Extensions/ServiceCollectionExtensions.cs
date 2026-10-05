using System;
using InterviewApp.MediatR.Handlers;
using InterviewApp.Models;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<GreetingOptions>()
                .Bind(configuration.GetSection("Greeting"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IGreetingService, GreetingService>();
        services.AddTransient<ITimeGreetingService, TimeGreetingService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GreetUserCommandHandler).Assembly));

        return services;
    }
}
