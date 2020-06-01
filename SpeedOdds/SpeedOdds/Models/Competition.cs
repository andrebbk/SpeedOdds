using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Models
{
    public class Competition
    {
        [Key]
        public int CompetitionId { get; set; }

        public string Name { get; set; }

        public int SeasonId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
