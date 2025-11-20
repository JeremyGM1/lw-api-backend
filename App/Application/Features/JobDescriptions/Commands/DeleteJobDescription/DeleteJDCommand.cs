using MediatR;
using Application.Common.Results;

namespace Application.Features.JobDescriptions.Commands.DeleteJobDescription;

public class DeleteJDCommand : IRequest<Result>
{
    public required int Id { get; set; }
}