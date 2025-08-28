using System;
using Domain;
using Xunit;

namespace Tests.Domain
{
    public class AuditTests
    {
        [Fact]
        public void CreatedAt_ShouldBeSet_ToCurrentDateTime_WhenObjectIsCreated()
        {
            var before = DateTime.Now;

            var audit = new Audit();
            var after = DateTime.Now;

            Assert.True(audit.CreatedAt >= before && audit.CreatedAt <= after);
        }

        [Fact]
        public void UpdatedAt_ShouldBeSet_ToCurrentDateTime_WhenObjectIsCreated()
        {
            var before = DateTime.Now;

            var audit = new Audit();
            var after = DateTime.Now;

            Assert.NotNull(audit.UpdatedAt);
            Assert.True(audit.UpdatedAt >= before && audit.UpdatedAt <= after);
        }

        [Fact]
        public void ShouldBeAbleToUpdate_CreatedBy_And_UpdatedBy()
        {
            var audit = new Audit();

            audit.CreatedBy = "system";
            audit.UpdatedBy = "admin";

            Assert.Equal("system", audit.CreatedBy);
            Assert.Equal("admin", audit.UpdatedBy);
        }

        [Fact]
        public void ShouldBeAbleToUpdate_UpdatedAt()
        {
            var audit = new Audit();
            var newTime = DateTime.UtcNow.AddHours(1);

            audit.UpdatedAt = newTime;

            Assert.Equal(newTime, audit.UpdatedAt);
        }
    }
}
