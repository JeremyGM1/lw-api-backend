using Domain.Common;

namespace Domain.Entities;
public class Skill : BaseEntity<int>
{
    public required string Name { get; set; }
    public required ICollection<JobSkill> JobSkills { get; set; } = [];
}
