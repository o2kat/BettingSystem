using System;

namespace BettingSystemApp.Models
{
    public class Journal
    {
        public int JournalID { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public int? UserID { get; set; }

        public virtual User User { get; set; }
    }
}
