namespace Application.Common.Results.JobDescriptionErrors;
public class JobDescriptionErros
{
    public static Error ExistingJobDescription(int id) => new(
        "JobDescriptions.Existing",
        $"A Job Description with this code '{id}' already exists");

    public static Error NotExistingJobDescription(int id) => new(
        "JobDescriptions.Existing",
        $"A Job Description with this code '{id}' does not exists");
}