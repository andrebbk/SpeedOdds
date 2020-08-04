using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Models
{
    public class TimeGoal
    {
        [Key]
        public int TimeGoalsId { get; set; }

        public int CompetitionId { get; set; }

        public int TeamId { get; set; }

        public int? Goal15 { get; set; }

        public int? Goal30 { get; set; }

        public int? Goal45 { get; set; }

        public int? Goal60 { get; set; }

        public int? Goal75 { get; set; }

        public int? Goal90 { get; set; }
    }
}
