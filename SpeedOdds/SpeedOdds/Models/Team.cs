using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        public string Name { get; set; }

        public int CompetitionId { get; set; }

        public bool IsFavorite { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
