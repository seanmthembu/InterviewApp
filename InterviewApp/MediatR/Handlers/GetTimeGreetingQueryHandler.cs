using System.Threading;
using System.Threading.Tasks;
using InterviewApp.MediatR.Queries;
using InterviewApp.Services;
using MediatR;

namespace InterviewApp.MediatR.Handlers;

public class GetTimeGreetingQueryHandler(ITimeGreetingService timeGreetingService)
    : IRequestHandler<GetTimeGreetingQuery, string>
{
    public Task<string> Handle(GetTimeGreetingQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(timeGreetingService.GetGreeting());
    }
}
