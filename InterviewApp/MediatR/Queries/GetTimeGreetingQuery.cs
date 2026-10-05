using MediatR;

namespace InterviewApp.MediatR.Queries;

public record GetTimeGreetingQuery : IRequest<string>;
