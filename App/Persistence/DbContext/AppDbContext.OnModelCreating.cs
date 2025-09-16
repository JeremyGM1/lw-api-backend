using Domain.Entities.Candidates;
using Domain.Entities.JobDescriptions;
using Domain.Entities.Requisitions;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public partial class AppDbContext : DbContext
{
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

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
