using SpeedOdds.Commons;
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
    }
}
