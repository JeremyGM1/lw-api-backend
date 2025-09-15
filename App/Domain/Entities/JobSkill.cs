namespace Domain.Entities;
public class JobSkill
{
    public int JobId { get; set; }
    public int SkillId { set; get; }
    public virtual JobDescription JobDescription { get; set; } = null!;
    public virtual Skill Skill { get; set; } = null!;
}