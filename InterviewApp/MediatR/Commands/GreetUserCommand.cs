using MediatR;

namespace InterviewApp.MediatR.Commands;

public record GreetUserCommand : IRequest<string>;
