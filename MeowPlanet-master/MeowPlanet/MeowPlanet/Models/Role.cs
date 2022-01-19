using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleManagements = new HashSet<RoleManagement>();
        }

        public int RoleId { get; set; }
        public string RoleDiscription { get; set; }

        public virtual ICollection<RoleManagement> RoleManagements { get; set; }
    }
}
