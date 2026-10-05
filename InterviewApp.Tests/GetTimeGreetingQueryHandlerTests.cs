using System;
using InterviewApp.MediatR.Handlers;
using InterviewApp.MediatR.Queries;
using InterviewApp.Services;
using Microsoft.Extensions.Time.Testing;

namespace InterviewApp.Tests;

public class GetTimeGreetingQueryHandlerTests
{
    private static GetTimeGreetingQueryHandler BuildHandler(int hour)
    {
        var fakeTime = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, hour, 0, 0, TimeSpan.Zero));
        return new GetTimeGreetingQueryHandler(new TimeGreetingService(fakeTime));
    }

    [Fact]
    public async Task Handle_MorningHour_ReturnsGoodMorning()
    {
        var result = await BuildHandler(9).Handle(new GetTimeGreetingQuery(), default);
        Assert.Equal("Good morning", result);
    }

    [Fact]
    public async Task Handle_AfternoonHour_ReturnsGoodAfternoon()
    {
        var result = await BuildHandler(14).Handle(new GetTimeGreetingQuery(), default);
        Assert.Equal("Good afternoon", result);
    }

    [Fact]
    public async Task Handle_EveningHour_ReturnsGoodEvening()
    {
        var result = await BuildHandler(20).Handle(new GetTimeGreetingQuery(), default);
        Assert.Equal("Good evening", result);
    }
}
