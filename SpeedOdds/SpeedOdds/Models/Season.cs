using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Models
{
    public class Season
    {
        [Key]
        public int SeasonId { get; set; }

        public string Name { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
