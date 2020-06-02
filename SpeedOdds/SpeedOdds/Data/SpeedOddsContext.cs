using SpeedOdds.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Data
{
    class SpeedOddsContext: DbContext
    {
        public DbSet<Season> Seasons { get; set; }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<Team> Teams { get; set; }
    }
}
