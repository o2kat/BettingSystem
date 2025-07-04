using System;
using System.Collections.Generic;

namespace BettingSystemApp.Models
{
    public class Bet
    {
        public int BetID { get; set; }
        public string Team1 { get; set; }
        public string Team2 { get; set; }
        public DateTime MatchTime { get; set; }
        public string Sport { get; set; }
        public string Description { get; set; }
        public decimal Team1Win { get; set; }
        public decimal Team2Win { get; set; }
        public decimal Draw { get; set; }

        public virtual ICollection<UserBet> UserBets { get; set; }
    }
}
