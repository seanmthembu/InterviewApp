using System;
using InterviewApp.Services;
using Microsoft.Extensions.Time.Testing;

namespace InterviewApp.Tests;

public class TimeGreetingServiceTests
{
    private static TimeGreetingService BuildService(int hour, int minute = 0)
    {
        var fakeTime = new FakeTimeProvider(new DateTimeOffset(2024, 1, 1, hour, minute, 0, TimeSpan.Zero));
        return new TimeGreetingService(fakeTime);
    }

    [Theory]
    [InlineData(0,  0,  "Good morning")]
    [InlineData(6,  0,  "Good morning")]
    [InlineData(11, 59, "Good morning")]
    [InlineData(12, 0,  "Good afternoon")]
    [InlineData(15, 0,  "Good afternoon")]
    [InlineData(17, 59, "Good afternoon")]
    [InlineData(18, 0,  "Good evening")]
    [InlineData(21, 0,  "Good evening")]
    [InlineData(23, 0,  "Good evening")]
    public void GetGreeting_ReturnsCorrectGreetingForTime(int hour, int minute, string expected)
    {
        Assert.Equal(expected, BuildService(hour, minute).GetGreeting());
    }
}
