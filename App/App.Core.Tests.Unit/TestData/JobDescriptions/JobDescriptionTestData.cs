using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Application.Features.JobDescriptions.Dtos.Response;
using Domain.Entities.JobDescriptions;

namespace App.Core.Tests.Unit.TestData.JobDescriptions;

public static class JobDescriptionTestData
{
    public static CreateJDCommand CreateJobDescriptionCommand() => new()
    {
        RequisitionCode = "600900",
        Title = "Software Engineer",
        PostingUrl = "",
        KeyResponsabilities = "Design, develop, and maintain software applications.",
        InternalNotes = "Bachelor's degree in Computer Science or related field.",
        RequiredSkills = ["C#", ".NET", "SQL", "Problem-solving skills"],
        DesiredSkills = ["Azure", "Docker", "Kubernetes"],
        BonusSkills = ["Machine", "Learning", "AI"],
    };

    public static JobDescription JobDescriptionEntity(CreateJDCommand command) => new()
    {
        Id = 1,
        ReqCode = command.RequisitionCode,
        Title = command.Title,
        PostingUrl = command.PostingUrl,
        KeyResponsabilities = command.KeyResponsabilities,
        InternalNotes = command.InternalNotes,
        RequiredSkills = command.RequiredSkills,
        DesiredSkills = command.DesiredSkills,
        BonusSkills = command.BonusSkills,
        ApprovalDate = null,
        Requisition = null!,
        LinkedCandidatesList = null!
    };

    public static CreateJobDescriptionResponseDto CreateJDDto(CreateJDCommand command) => new()
    {
        Id = 1,
        ReqCode = command.RequisitionCode,
        Title = command.Title,
        PostingUrl = command.PostingUrl,
        KeyResponsabilities = command.KeyResponsabilities,
        InternalNotes = command.InternalNotes,
        RequiredSkills = command.RequiredSkills,
        DesiredSkills = command.DesiredSkills,
        BonusSkills = command.BonusSkills,
        Status = "Active",
        LocationType = "Remote",
        CreatedBy = "test.user",
        CreatedAt = DateTime.UtcNow
    };
}