using Domain.Common;
using System;
using System.Collections.Generic;
namespace Domain.Entities.Users;
    public class UserPermission : BaseEntity<Guid>
    {
        public string UserId { get; set; } = default!;
        public Guid PermissionId { get; set; } = default!;
        public bool IsGranted { get; set; } = true;

        public Permission Permission { get; set; } = default!;
    }

