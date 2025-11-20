namespace Application.Common.Results.JobDescriptionErrors;
public class JobDescriptionErros
{
    public static Error ExistingJobDescription(int id) => new(
        "JobDescriptions.Existing",
        $"A Job Description with this id '{id}' already exists");

    public static Error NotExistingJobDescriptionById(int id) => new(
        "JobDescriptions.Existing",
        $"A Job Description with this id '{id}' does not exists");

    public static Error NotExistingJobDescriptionByRequisitionCode(string reqCode) => new(
        "JobDescriptions.Existing",
        $"A Job Description with this requisition code '{reqCode}' does not exists");
}