using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Models.Shared
{
    public class MatchSolverItem
    {
        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public int MargemCasa {
            get
            {
                return HomeGoals - AwayGoals;
            }
        }
    }
}
