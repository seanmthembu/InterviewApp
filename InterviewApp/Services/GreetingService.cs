using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InterviewApp.Models;

namespace InterviewApp.Services;

public class GreetingService(
    IOptions<GreetingOptions> options,
    ILogger<GreetingService> logger,
    ITimeGreetingService timeGreetingService) : IGreetingService
{
    private readonly GreetingOptions _options = options.Value;
    private const string FallbackLanguage = "English";

    private static readonly Dictionary<string, string> Greetings = new(StringComparer.OrdinalIgnoreCase)
    {
        { "English",   "Hello" },
        { "Afrikaans", "Hallo" },
        { "Zulu",      "Sawubona" },
        { "Xhosa",     "Molo" },
        { "Sotho",     "Dumela" },
        { "Tswana",    "Dumela" },
        { "Pedi",      "Dumela" },
        { "Tsonga",    "Avuxeni" },
        { "Venda",     "Ndaa" },
        { "Swati",     "Sawubona" },
        { "Ndebele",   "Lotjhani" }
    };

    public string BuildGreeting()
    {
        logger.LogInformation("GreetingService started. Language: {Language}", _options.Language);

        if (!Greetings.TryGetValue(_options.Language, out var greeting))
        {
            greeting = Greetings[FallbackLanguage];
            logger.LogWarning(
                "Unsupported language '{RequestedLanguage}'. Falling back to '{FallbackLanguage}'.",
                _options.Language, FallbackLanguage);
        }

        var timeGreeting = timeGreetingService.GetGreeting();
        var message = $"{timeGreeting}! {greeting}, {_options.Message}";
        logger.LogInformation("Displaying message: {Message}", message);
        return message;
    }
}
