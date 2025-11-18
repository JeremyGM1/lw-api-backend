namespace Application.Common.Results.JobDescriptionErrors;
public class JobDescriptionErros
{
    public static Error ExistingJobDescription(int id) => new(
        "JobDescriptions.Existing",
        $"A Job Description with this code '{id}' already exists");
}