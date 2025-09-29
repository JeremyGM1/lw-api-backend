using Microsoft.EntityFrameworkCore;
using Domain.Common;
using Domain.Entities.Candidates;
using Domain.Entities.JobDescriptions;
using Domain.Entities.Requisitions;
using Domain.Entities.Users;

namespace Persistence;

public partial class AppDbContext : DbContext
{
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<User> Users { get; set; }
    public required DbSet<UserRole> UserRole { get; set; }
    public required DbSet<Requisition> Requisitions { get; set; }
    public required DbSet<JobDescription> JobDescriptions { get; set; }
    public required DbSet<Skill> Skills { get; set; }
    public required DbSet<Candidate> Candidates { get; set; }
    public required DbSet<RequisitionJob> RequisitionJobs { get; set; }
    public required DbSet<JobCandidate> JobCandidates { get; set; }
    public required DbSet<JobSkill> JobSkills { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in base.ChangeTracker.Entries<BaseEntity<object>>()
            .Where(q => q.State == EntityState.Added || q.State == EntityState.Modified))
        {
            entry.Entity.UpdatedAt = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}