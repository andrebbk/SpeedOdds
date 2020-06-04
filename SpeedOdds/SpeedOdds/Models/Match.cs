using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Models
{
    public class Match
    {
        [Key]
        public int MatchId { get; set; }

        public int CompetitionId { get; set; }

        public int FixtureId { get; set; }

        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public decimal HomeOdd { get; set; }

        public decimal DrawOdd { get; set; }

        public decimal AwayOdd { get; set; }

        public int? HalfHomeGoals { get; set; }

        public int? HalfAwayGoals { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}
