using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class RoleManagement
    {
        public int RoleManagementId { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserData User { get; set; }
    }
}
