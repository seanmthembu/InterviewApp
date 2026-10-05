using System;

namespace InterviewApp.Services;

public class TimeGreetingService(TimeProvider timeProvider) : ITimeGreetingService
{
    public string GetGreeting()
    {
        var hour = timeProvider.GetLocalNow().Hour;

        return hour switch
        {
            >= 0 and < 12  => "Good morning",
            >= 12 and < 18 => "Good afternoon",
            _              => "Good evening"
        };
    }
}
