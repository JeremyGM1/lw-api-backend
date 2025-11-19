using Application.Common.Results;
using MediatR;

namespace Application.Features.JobDescriptions.Commands.DeleteJobDescription;
public class DeleteJDCommand : IRequest<Result>
{
    public required int Id { get; set; }
}