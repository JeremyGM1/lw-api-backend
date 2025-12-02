using Domain.Common;

namespace Domain.Entities.JobDescriptions;

public class JobDescriptionLog : BaseEntityLog
{
    public required string ReqCode { get; set; }
    public required string Title { get; set; }
}