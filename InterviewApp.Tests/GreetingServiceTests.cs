using System.Collections.Generic;
using InterviewApp.Models;
using InterviewApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Microsoft.Extensions.Options;
using Moq;

namespace InterviewApp.Tests;

public class GreetingServiceTests
{
    private static (GreetingService Service, FakeLogger<GreetingService> Logger) BuildService(
        string? language, string? message, string timeGreeting = "Good morning")
    {
        var options = Options.Create(new GreetingOptions { Language = language!, Message = message! });
        var fakeLogger = new FakeLogger<GreetingService>();
        var timeMock = new Mock<ITimeGreetingService>();
        timeMock.Setup(t => t.GetGreeting()).Returns(timeGreeting);
        return (new GreetingService(options, fakeLogger, timeMock.Object), fakeLogger);
    }

    [Theory]
    [InlineData("English",   "Hello")]
    [InlineData("Afrikaans", "Hallo")]
    [InlineData("Zulu",      "Sawubona")]
    [InlineData("Xhosa",     "Molo")]
    [InlineData("Sotho",     "Dumela")]
    [InlineData("Tswana",    "Dumela")]
    [InlineData("Pedi",      "Dumela")]
    [InlineData("Tsonga",    "Avuxeni")]
    [InlineData("Venda",     "Ndaa")]
    [InlineData("Swati",     "Sawubona")]
    [InlineData("Ndebele",   "Lotjhani")]
    public void BuildGreeting_AllSupportedLanguages_ReturnsExactMessage(string language, string expectedGreeting)
    {
        var (service, _) = BuildService(language, "Welcome!");
        var result = service.BuildGreeting();
        Assert.Equal($"Good morning! {expectedGreeting}, Welcome!", result);
    }

    [Fact]
    public void BuildGreeting_UnsupportedLanguage_FallsBackToEnglish()
    {
        var (service, _) = BuildService("Klingon", "Welcome!");
        var result = service.BuildGreeting();
        Assert.Equal("Good morning! Hello, Welcome!", result);
    }

    [Fact]
    public void BuildGreeting_UnsupportedLanguage_LogsWarningWithBothLanguages()
    {
        var (service, fakeLogger) = BuildService("Klingon", "Welcome!");
        service.BuildGreeting();

        var warning = fakeLogger.Collector.GetSnapshot()
            .Single(l => l.Level == LogLevel.Warning);

        Assert.Contains("Klingon", warning.Message);
        Assert.Contains("English", warning.Message);
    }

    [Fact]
    public void BuildGreeting_LanguageLookupIsCaseInsensitive()
    {
        var (service, _) = BuildService("afrikaans", "Welcome!");
        var result = service.BuildGreeting();
        Assert.Equal("Good morning! Hallo, Welcome!", result);
    }

    [Fact]
    public void BuildGreeting_TimeGreetingCombinedInResult()
    {
        var (service, _) = BuildService("English", "Welcome!", "Good evening");
        var result = service.BuildGreeting();
        Assert.Equal("Good evening! Hello, Welcome!", result);
    }

    [Fact]
    public async Task ValidateOnStart_EmptyMessage_ThrowsOptionsValidationException()
    {
        var inMemoryConfig = new Dictionary<string, string?>
        {
            ["Greeting:Message"] = "",
            ["Greeting:Language"] = "English"
        };

        using var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config => config.AddInMemoryCollection(inMemoryConfig))
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<GreetingOptions>()
                    .Bind(context.Configuration.GetSection("Greeting"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
            })
            .UseConsoleLifetime(o => o.SuppressStatusMessages = true)
            .Build();

        await Assert.ThrowsAsync<OptionsValidationException>(() => host.StartAsync());
    }

    [Fact]
    public async Task ValidateOnStart_EmptyLanguage_ThrowsOptionsValidationException()
    {
        var inMemoryConfig = new Dictionary<string, string?>
        {
            ["Greeting:Message"] = "Welcome!",
            ["Greeting:Language"] = ""
        };

        using var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config => config.AddInMemoryCollection(inMemoryConfig))
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<GreetingOptions>()
                    .Bind(context.Configuration.GetSection("Greeting"))
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
            })
            .UseConsoleLifetime(o => o.SuppressStatusMessages = true)
            .Build();

        await Assert.ThrowsAsync<OptionsValidationException>(() => host.StartAsync());
    }
}
