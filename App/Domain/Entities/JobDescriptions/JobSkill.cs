namespace Domain.Entities.JobDescriptions;

public class JobSkill
{
    public int JobId { get; set; }
    public int SkillId { set; get; }
    public virtual JobDescription JobDescription { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
    public JobSkillType JobSkillType { get; set; }
}

public enum JobSkillType
{
    Required,
    Desired,
    Bonus
}