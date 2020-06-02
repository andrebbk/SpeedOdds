using SpeedOdds.Data;
using SpeedOdds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Services
{
    public class CompetitionService
    {
        public bool AlreadyExistsCompetition(string name, int seasonId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return db.Competitions.Where(c => c.Name == name.Trim() && c.SeasonId == seasonId).Any();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking competition -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return true;
            }
        }

        public bool CanDeleteById(int competitionId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return !db.Teams.Where(t => t.CompetitionId == competitionId).Any();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking competitions in teams from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public bool CreateCompetition(string name, int seasonId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    Competition comp = new Competition()
                    {
                        Name = name.Trim(),
                        SeasonId = seasonId,
                        CreateDate = DateTime.Now
                    };

                    db.Competitions.Add(comp);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating new competition -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        public IEnumerable<Competition> GetCompetitions()
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.Competitions.OrderBy(x => x.Name).ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Competitions from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public string GetCompetitionName(int competitionId)
        {
            string output = "";

            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var comp = db.Competitions.Where(x => x.CompetitionId == competitionId).FirstOrDefault();

                    if (comp != null)
                    {
                        output = comp.Name;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting competition name from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return output;
        }

        public string GetCompetitionSeasonName(int competitionId)
        {
            string output = "";

            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var season = (from c in db.Competitions
                               join s in db.Seasons on c.SeasonId equals s.SeasonId
                               where c.CompetitionId == competitionId
                               select new { s.Name }).FirstOrDefault();

                    if (season != null)
                    {
                        output = season.Name;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting competition season name from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return output;
        }

        public bool RemoveCompetition(int competitionId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var comp = db.Competitions.Where(x => x.CompetitionId == competitionId).FirstOrDefault();

                    if (comp != null)
                    {
                        db.Competitions.Remove(comp);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing competition from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
