using Application.Features.JobDescriptions.Commands.CreateJobDescription;
using Application.Interfaces.Features.JobDescriptions;
using Moq;
using Shouldly;

namespace App.Core.Tests.Unit.Features.JobDescriptions.Commands.CreateJobDescription;

public class CreateJobDescriptionCommandValidatorTests
{
    private readonly Mock<IJobDescriptionRepository> _jdRepositoryMock;
    private readonly CreateJDCommandValidator _validator;

    public CreateJobDescriptionCommandValidatorTests()
    {
        _jdRepositoryMock = new Mock<IJobDescriptionRepository>();
        _validator = new CreateJDCommandValidator(_jdRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateJDValidator_WhenAllFieldsAreValid_ShouldPass()
    {
        var command = new CreateJDCommand
        {
            RequisitionCode = "600900",
            Title = "Software Engineer",
            PostingUrl = "https://example.com/job/software-engineer",
            KeyResponsabilities = "Develop and maintain software applications.",
            InternalNotes = "Internal notes here",
            RequiredSkills = ["C#", ".NET", "SQL"],
            DesiredSkills = ["Azure", "Docker"],
            BonusSkills = ["Kubernetes"]
        };

        var result = await _validator.ValidateAsync(command);

        result.Errors.ShouldNotContain(e => e.PropertyName == "RequisitionCode");
        result.Errors.ShouldNotContain(e => e.PropertyName == "Title");
        result.Errors.ShouldNotContain(e => e.PropertyName == "PostingUrl");
        result.Errors.ShouldNotContain(e => e.PropertyName == "KeyResponsabilities");
        result.Errors.ShouldNotContain(e => e.PropertyName == "RequiredSkills");
        result.Errors.ShouldNotContain(e => e.PropertyName == "DesiredSkills");
        result.Errors.ShouldNotContain(e => e.PropertyName == "BonusSkills");
    }
}