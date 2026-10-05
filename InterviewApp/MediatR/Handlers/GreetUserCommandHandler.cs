using System.Threading;
using System.Threading.Tasks;
using InterviewApp.MediatR.Commands;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InterviewApp.MediatR.Handlers;

public class GreetUserCommandHandler(
    IGreetingService greetingService,
    ILogger<GreetUserCommandHandler> logger) : IRequestHandler<GreetUserCommand, string>
{
    public Task<string> Handle(GreetUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GreetUserCommand received.");
        return Task.FromResult(greetingService.BuildGreeting());
    }
}
