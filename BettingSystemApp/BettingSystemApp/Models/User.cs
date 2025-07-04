using System;
using System.Collections.Generic;

namespace BettingSystemApp.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Balance { get; set; }
        public int? BetsCount { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<UserBet> UserBets { get; set; }
    }
}
