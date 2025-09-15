using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class AppDbContext : DbContext
{
    public required DbSet<Role> Roles { get; set; }
    public required DbSet<Requisition> Requisitions { get; set; }
    public required DbSet<JobDescription> JobDescriptions { get; set; }
    public required DbSet<Skill> Skills { get; set; }
    public required DbSet<Candidate> Candidates { get; set; }
    public required DbSet<RequisitionJob> RequisitionJobs { get; set; }
    public required DbSet<JobCandidate> JobCandidates { get; set; }
    public required DbSet<JobSkill> JobSkills { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}