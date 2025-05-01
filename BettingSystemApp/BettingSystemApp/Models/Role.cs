﻿using System.Collections.Generic;

namespace BettingSystemApp.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
