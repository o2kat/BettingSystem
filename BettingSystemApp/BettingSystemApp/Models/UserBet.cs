using System;

namespace BettingSystemApp.Models
{
    public class UserBet
    {
        public int UserBetID { get; set; }
        public int UserID { get; set; }
        public int BetID { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePlaced { get; set; }
        public string Status { get; set; } // Active, Won, Lost

        public virtual User User { get; set; }
        public virtual Bet Bet { get; set; }
    }
} 