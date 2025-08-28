using Domain.AccessControl;
using Xunit;

namespace Domain.Tests.AccessControl
{
    internal class TestEntity : BaseEntity
    {

    }

    public class BaseEntityTests
    {
        [Fact]
        public void Constructor_WhenCalled_CreatesUniqueIdForEachInstance()
        {
            var entity1 = new TestEntity();
            var entity2 = new TestEntity();

            Assert.NotEqual(entity1.Id, entity2.Id);
        }

        [Fact]
        public void Constructor_WhenCalled_InitializesIsActiveAsTrue()
        {
            var entity = new TestEntity();

            Assert.True(entity.IsActive);
        }

        [Fact]
        public void ToggleActive_WhenCalledTwice_ReturnsToOriginalState()
        {
            var entity = new TestEntity();
            var initialStatus = entity.IsActive;

            entity.ToggleActive();
            entity.ToggleActive();

            Assert.Equal(initialStatus, entity.IsActive);
        }

        [Fact]
        public void ToggleActive_WhenCalled_ChangesIsActiveStatus()
        {
            var entity = new TestEntity();
            var initialStatus = entity.IsActive;

            entity.ToggleActive();
          
            Assert.NotEqual(initialStatus, entity.IsActive);
        }

    }
}