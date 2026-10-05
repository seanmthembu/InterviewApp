using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InterviewApp.Extensions;
using InterviewApp.MediatR.Commands;

class Program
{
    static async Task<int> Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddApplicationServices(context.Configuration);
            })
            .UseConsoleLifetime(o => o.SuppressStatusMessages = true)
            .Build();

        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        try
        {
            await host.StartAsync();

            var mediator = host.Services.GetRequiredService<IMediator>();
            var result = await mediator.Send(new GreetUserCommand());
            Console.WriteLine(result);

            return 0;
        }
        catch (OptionsValidationException ex)
        {
            logger.LogError("Configuration validation failed: {Errors}", string.Join(", ", ex.Failures));
            return 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred.");
            return 1;
        }
        finally
        {
            await host.StopAsync();
        }
    }
}
