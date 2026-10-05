using InterviewApp.MediatR.Commands;
using InterviewApp.MediatR.Handlers;
using InterviewApp.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace InterviewApp.Tests;

public class GreetUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsGreetingFromService()
    {
        var greetingService = new Mock<IGreetingService>();
        greetingService.Setup(s => s.BuildGreeting()).Returns("Good morning! Hello, Welcome!");
        var handler = new GreetUserCommandHandler(greetingService.Object, Mock.Of<ILogger<GreetUserCommandHandler>>());

        var result = await handler.Handle(new GreetUserCommand(), default);

        Assert.Equal("Good morning! Hello, Welcome!", result);
        greetingService.Verify(s => s.BuildGreeting(), Times.Once);
    }
}
