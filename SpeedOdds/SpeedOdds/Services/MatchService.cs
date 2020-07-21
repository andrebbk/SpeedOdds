using SpeedOdds.Commons;
using SpeedOdds.Commons.Enums;
using SpeedOdds.Commons.Helpers;
using SpeedOdds.Data;
using SpeedOdds.Models;
using SpeedOdds.UserControls.Matches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedOdds.Services
{
    public class MatchService
    {
        public Tuple<int, OperationTypeValues> CreateOrUpdateMatch(MatchesModel model)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    Match jogo = null;

                    if (model.MatchId.HasValue)
                    {
                        //EDIT
                        jogo = db.Matches.Where(x => x.MatchId == model.MatchId).FirstOrDefault();
                        if(jogo != null)
                        {
                            jogo.Order = model.Order;
                            jogo.HomeTeamId = model.HomeTeamId;
                            jogo.AwayTeamId = model.AwayTeamId;
                            jogo.HomeGoals = model.HomeGoals;
                            jogo.AwayGoals = model.AwayGoals;
                            jogo.HomeOdd = model.OddsHome;
                            jogo.DrawOdd = model.OddsDraw;
                            jogo.AwayOdd = model.OddsAway;
                            jogo.UpdatedDate = DateTime.Now;

                            db.SaveChanges();

                            return new Tuple<int, OperationTypeValues>(jogo.MatchId, OperationTypeValues.Edit);
                        }

                        return null;
                    }
                    else
                    {
                        //CREATE
                        jogo = new Match()
                        {
                            CompetitionId = model.CompetitionId,
                            FixtureId = model.FixtureId,
                            Order = model.Order,
                            HomeTeamId = model.HomeTeamId,
                            AwayTeamId = model.AwayTeamId,
                            HomeGoals = model.HomeGoals,
                            AwayGoals = model.AwayGoals,
                            HomeOdd = model.OddsHome,
                            DrawOdd = model.OddsDraw,
                            AwayOdd = model.OddsAway,
                            CreatedDate = DateTime.Now
                        };

                        db.Matches.Add(jogo);
                        db.SaveChanges();

                        return new Tuple<int, OperationTypeValues>(jogo.MatchId, OperationTypeValues.Create);
                    }                                        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating/updating match -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public OperationTypeValues UpdateMatchHalfTime(int matchId, int homeGoals, int awayGoals)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    Match jogo = db.Matches.Where(x => x.MatchId == matchId).FirstOrDefault();

                    if (jogo != null)
                    {
                        bool createdNow = jogo.HalfHomeGoals.HasValue ? false : true;

                        jogo.HalfHomeGoals = homeGoals;
                        jogo.HalfAwayGoals = awayGoals;
                        jogo.UpdatedDate = DateTime.Now;
                        db.SaveChanges();

                        if (createdNow)
                            return OperationTypeValues.Create;
                        else
                            return OperationTypeValues.Edit;
                    }

                    return OperationTypeValues.Error;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating match half time -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return OperationTypeValues.Error;
            }
        }

        public bool HasInitialMatchesForCompetitionFixture(int compId, int fixId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    return db.Matches.Where(x => x.CompetitionId == compId && x.FixtureId == fixId)
                        .Any();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking matches by competition and fixture from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public IEnumerable<Match> GetMatchesByCompetitionFixture(int compId, int fixId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.Matches.Where(x => x.CompetitionId == compId && x.FixtureId == fixId)
                        .OrderBy(x => x.MatchId)
                        .ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Matches by competition and fixture from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public IEnumerable<Match> GetMatchesFiltered(int compId, int? fixId, int? tmId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var query = db.Matches.Where(x => x.CompetitionId == compId)
                        .OrderBy(x => x.MatchId)
                        .ToList();

                    if(fixId.HasValue)
                        query = query.Where(x => x.FixtureId == fixId.Value)
                             .OrderBy(x => x.MatchId)
                             .ToList();

                    if (tmId.HasValue)
                        query = query.Where(x => x.HomeTeamId == tmId.Value || x.AwayTeamId == tmId.Value)
                             .OrderBy(x => x.MatchId)
                             .ToList();

                    if (query != null)
                        return query;
                    else
                        return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting Matches filtered from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public int GetMatchesNumber()
        {
            int nMatches = 0;

            try
            {
                using (var db = new SpeedOddsContext())
                {
                    nMatches = db.Matches.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error counting matches -> " + ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return nMatches;
        }

        public bool RemoveMatch(int matchId)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var match = db.Matches.Where(x => x.MatchId == matchId).FirstOrDefault();

                    if (match != null)
                    {
                        db.Matches.Remove(match);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing match from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);

                return false;
            }
        }


        //ODDS OPERATIONS
        public Tuple<int, int, int, int> GetTeamResults(int competitionId, int teamId, bool IsHomeTeam)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    if (IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int vics = 0, draws = 0, defts = 0;

                            foreach(var match in matches)
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    vics++;
                                else if (match.HomeGoals < match.AwayGoals)
                                    defts++;
                                else
                                    draws++;
                            }

                            return new Tuple<int, int, int, int>(vics, draws, defts, matches.Count());
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int vics = 0, draws = 0, defts = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    defts++;
                                else if (match.HomeGoals < match.AwayGoals)
                                    vics++;
                                else
                                    draws++;
                            }

                            return new Tuple<int, int, int, int>(vics, draws, defts, matches.Count());
                        }
                    }

                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team results from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public int GetTeamForma(int competitionId, int teamId, bool IsHomeTeam)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    /*if (IsHomeTeam)
                    {
                        var matches = db.Matches
                            .Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId)
                            .OrderByDescending(x => x.FixtureId)
                            .Take(5);

                        if (matches != null && matches.Count() > 0)
                        {
                            int forma = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    forma += 1;
                                else if (match.HomeGoals < match.AwayGoals)
                                    forma -= 1;
                            }

                            return forma;
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches
                            .Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId)
                            .OrderByDescending(x => x.FixtureId)
                            .Take(5);

                        if (matches != null && matches.Count() > 0)
                        {
                            int forma = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    forma -= 1;
                                else if (match.HomeGoals < match.AwayGoals)
                                    forma += 1;
                            }

                            return forma;
                        }
                    }*/

                    var matches = db.Matches
                            .Where(x => x.CompetitionId == competitionId && (x.HomeTeamId == teamId || x.AwayTeamId == teamId))
                            .OrderByDescending(x => x.FixtureId)
                            .Take(5);

                    if (matches != null && matches.Count() > 0)
                    {
                        int forma = 0;

                        foreach (var match in matches)
                        {
                            if(match.AwayTeamId == teamId)
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    forma -= 1;
                                else if (match.HomeGoals < match.AwayGoals)
                                    forma += 1;
                            }
                            else
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    forma += 1;
                                else if (match.HomeGoals < match.AwayGoals)
                                    forma -= 1;
                            }
                            
                        }

                        return forma;
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team forma from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public Tuple<int, int> GetTeamGoals(int competitionId, int teamId, bool IsHomeTeam)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    if (IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int goalsScored = 0, goalsSuffered = 0;

                            foreach (var match in matches)
                            {
                                goalsScored += match.HomeGoals;
                                goalsSuffered += match.AwayGoals;
                            }

                            return new Tuple<int, int>(goalsScored, goalsSuffered);
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int goalsScored = 0, goalsSuffered = 0;

                            foreach (var match in matches)
                            {
                                goalsScored += match.AwayGoals;
                                goalsSuffered += match.HomeGoals;
                            }

                            return new Tuple<int, int>(goalsScored, goalsSuffered);
                        }
                    }

                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team goals from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public int GetTeamOver15(int competitionId, int teamId, bool IsHomeTeam)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    if (IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if ((match.HomeGoals + match.AwayGoals) > (decimal)1.5)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if ((match.HomeGoals + match.AwayGoals) > (decimal)1.5)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team over 1.5 from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int GetTeamOver25(int competitionId, int teamId, bool IsHomeTeam)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    if (IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if ((match.HomeGoals + match.AwayGoals) > (decimal)2.5)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if ((match.HomeGoals + match.AwayGoals) > (decimal)2.5)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team over 2.5 from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int GetTeamBTTS(int competitionId, int teamId, bool IsHomeTeam)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    if (IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals > (decimal)0.5 && match.AwayGoals > (decimal)0.5)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals > (decimal)0.5 && match.AwayGoals > (decimal)0.5)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team BTTS from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int GetTeamResultsOccurrences(int competitionId, int teamId, bool IsHomeTeam, int homeGoals, int awayGoals)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    if (IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.HomeTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals == homeGoals && match.AwayGoals == awayGoals)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    if (!IsHomeTeam)
                    {
                        var matches = db.Matches.Where(x => x.CompetitionId == competitionId && x.AwayTeamId == teamId);

                        if (matches != null && matches.Count() > 0)
                        {
                            int occ = 0;

                            foreach (var match in matches)
                            {
                                if (match.HomeGoals == homeGoals && match.AwayGoals == awayGoals)
                                    occ++;
                            }

                            return occ;
                        }
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting team result occurences from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        //ODDS OPERATIONS MATCH TYPES
        public Tuple<int, int, int, int> GetMatchTypeResults(int competitionId, MatchTypeValues tp)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var matches = db.Matches.Where(x => x.CompetitionId == competitionId);

                    if (matches != null && matches.Count() > 0)
                    {
                        int vics = 0, draws = 0, defts = 0, mCount = 0;
                        
                        foreach (var match in matches)
                        {
                            if(UtilsOddOperations.GetMatchType(match.HomeOdd, match.AwayOdd) == tp)
                            {
                                if (match.HomeGoals > match.AwayGoals)
                                    vics++;
                                else if (match.HomeGoals < match.AwayGoals)
                                    defts++;
                                else
                                    draws++;

                                mCount++;
                            }                            
                        }

                        return new Tuple<int, int, int, int>(vics, draws, defts, mCount);
                    }

                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting match type results from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public int GetMatchTypeOver15(int competitionId, MatchTypeValues tp)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var matches = db.Matches.Where(x => x.CompetitionId == competitionId);

                    if (matches != null && matches.Count() > 0)
                    {
                        int occ = 0;

                        foreach (var match in matches)
                        {
                            if (UtilsOddOperations.GetMatchType(match.HomeOdd, match.AwayOdd) == tp)
                            {
                                if ((match.HomeGoals + match.AwayGoals) > (decimal)1.5)
                                    occ++;
                            }                                
                        }

                        return occ;
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting match type over 1.5 from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int GetMatchTypeOver25(int competitionId, MatchTypeValues tp)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var matches = db.Matches.Where(x => x.CompetitionId == competitionId);

                    if (matches != null && matches.Count() > 0)
                    {
                        int occ = 0;

                        foreach (var match in matches)
                        {
                            if (UtilsOddOperations.GetMatchType(match.HomeOdd, match.AwayOdd) == tp)
                            {
                                if ((match.HomeGoals + match.AwayGoals) > (decimal)2.5)
                                    occ++;
                            }                                
                        }

                        return occ;
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting match type over 2.5 from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public int GetMatchTypeBTTS(int competitionId, MatchTypeValues tp)
        {
            try
            {
                using (var db = new SpeedOddsContext())
                {
                    var matches = db.Matches.Where(x => x.CompetitionId == competitionId);

                    if (matches != null && matches.Count() > 0)
                    {
                        int occ = 0;

                        foreach (var match in matches)
                        {
                            if (UtilsOddOperations.GetMatchType(match.HomeOdd, match.AwayOdd) == tp)
                            {
                                if (match.HomeGoals > (decimal)0.5 && match.AwayGoals > (decimal)0.5)
                                    occ++;
                            }                                
                        }

                        return occ;
                    }

                    return 0;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting match type BTTS from BD -> " + ex.ToString());
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
    }
}
