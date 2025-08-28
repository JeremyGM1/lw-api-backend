using System;

namespace Domain
{
    public class Role : Audit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string RoleCategoryId { get; set; }
        public bool IsActive { get; set; } = true;

    }
}