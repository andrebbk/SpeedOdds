using SpeedOdds.Data;
using SpeedOdds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Services
{
    public class SeasonService
    {
        public bool AlreadyExistsSeason(int sy, int ey)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return db.Seasons.Where(s => s.StartYear == sy && s.EndYear == ey).Any();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking season -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return true;
            }
        }

        public bool CreateSeason(int sy, int ey)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {   
                    Season season = new Season()
                    {
                        Name = "Época " + sy.ToString() + "/ " + ey.ToString(),
                        StartYear = sy,
                        EndYear = ey,
                        CreateDate = DateTime.Now                        
                    };

                    db.Seasons.Add(season);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new season -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
