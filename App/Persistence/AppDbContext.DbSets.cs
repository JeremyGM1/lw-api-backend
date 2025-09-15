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
        modelBuilder.Entity<RequisitionJob>()
            .HasKey(rj => new { rj.ReqId, rj.JobDescriptionId });

        modelBuilder.Entity<RequisitionJob>()
            .HasOne(rj => rj.Requisition)
            .WithMany(rj => rj.LinkedPostingList)
            .HasForeignKey(rj => rj.ReqId);

        modelBuilder.Entity<RequisitionJob>()
            .HasOne(rj => rj.JobDescription)
            .WithMany(rj => rj.LinkedPostingList)
            .HasForeignKey(rj => rj.JobDescriptionId);

        modelBuilder.Entity<JobCandidate>()
            .HasKey(jc => new { jc.JobDescriptionId, jc.CandidateId });

        modelBuilder.Entity<JobCandidate>()
            .HasOne(jc => jc.JobDescription)
            .WithMany(jc => jc.LinkedCandidatesList)
            .HasForeignKey(jc => jc.JobDescriptionId);

        modelBuilder.Entity<JobCandidate>()
            .HasOne(jc => jc.Candidate)
            .WithMany(jc => jc.JobCandidates)
            .HasForeignKey(jc => jc.CandidateId);

        modelBuilder.Entity<JobSkill>()
            .HasKey(js => new { js.JobId, js.SkillId });

        modelBuilder.Entity<JobSkill>()
            .HasOne(js => js.JobDescription)
            .WithMany(js => js.JobSkills)
            .HasForeignKey(js => js.JobId);

        modelBuilder.Entity<JobSkill>()
            .HasOne(js => js.Skill)
            .WithMany(js => js.JobSkills)
            .HasForeignKey(js => js.SkillId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}