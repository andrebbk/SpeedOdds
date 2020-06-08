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

        public bool CanDeleteById(int seasonId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return !db.Competitions.Where(c => c.SeasonId == seasonId).Any();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking seasons in competitions from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
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

        public IEnumerable<Season> GetSeasons()
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.Seasons.OrderByDescending(x => x.EndYear).ThenByDescending(x => x.StartYear).ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Seasons from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public string GetSeasonName(int seasonId)
        {
            string output = "";

            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var season = db.Seasons.Where(x => x.SeasonId == seasonId).FirstOrDefault();

                    if (season != null)
                    {
                        output = season.Name;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting season name from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);                
            }

            return output;
        }

        public int GetSeasonsNumber()
        {
            int nSeasons = 0;

            try
            {
                using (var db = new SpeedOddsContext())
                {
                    nSeasons = db.Seasons.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error counting season -> " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return nSeasons;
        }

        public bool RemoveSeason(int seasonId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var season = db.Seasons.Where(x => x.SeasonId == seasonId).FirstOrDefault();

                    if (season != null)
                    {
                        db.Seasons.Remove(season);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing season from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
