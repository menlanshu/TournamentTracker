using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        private static TournamentRoundModel CheckCurrentRound(TournamentModel tournament)
        {
            TournamentRoundModel currRound = null;

            foreach (TournamentRoundModel round in tournament.Rounds)
            {
                if (round.MatchUps.Any(m => m.Winner == null))
                {
                    currRound = round;
                    break;
                }
            }

            return currRound;
        }
        public static void CreateRound(TournamentModel model)
        {
            List<TeamModel> randomizeTeams = RandomizeTeamOrder(model.TournamentEntryModels.Select(te => te.Team).ToList());
            int rounds = FindNumberOfRounds(randomizeTeams.Count);
            int byes = NumberOfByes(rounds, randomizeTeams.Count);

            // model.Rounds.Add(CreateFirstRound(byes, randmizeTeams));

            // Create every rounds after the first
            TournamentRoundModel firstRound = CreateFirstRound(byes, randomizeTeams);
            firstRound.Tournament = model;
            model.Rounds.Add(firstRound);

            CreateOtherRounds(model, rounds);
        }
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;

            TournamentRoundModel previousRound = model.Rounds[0];
            TournamentRoundModel currRound = new TournamentRoundModel();

            MatchupModel currMatchup = new MatchupModel();

            while (round <= rounds)
            {
                currMatchup.Round = currRound;

                foreach (MatchupModel match in previousRound.MatchUps)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match, Matchup = currMatchup });

                    if (currMatchup.Entries.Count > 1)
                    {
                        currRound.MatchUps.Add(currMatchup);

                        currMatchup = new MatchupModel();
                    }
                }

                currRound.RoundNumber = round;
                currRound.Tournament = model;
                // Add current rounds to Tournament
                model.Rounds.Add(currRound);

                previousRound = currRound;
                currRound = new TournamentRoundModel();
                round++;
            }

        }
        private static TournamentRoundModel CreateFirstRound(int byes, List<TeamModel> teams)
        {
            TournamentRoundModel output = new TournamentRoundModel();
            List<MatchupModel> matchups = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();
            curr.Round = output;

            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team, Matchup = curr });

                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.RoundId = 1;

                    matchups.Add(curr);
                    curr = new MatchupModel();
                    curr.Round = output;

                    byes = byes > 0 ? byes - 1 : byes;
                }
            }

            output.RoundNumber = 1;
            output.MatchUps = matchups;
            return output;
        }
        public static void UpdateMatchup(TournamentModel tournament, MatchupModel currMatchup)
        {
            TournamentRoundModel startRound = CheckCurrentRound(tournament);
            
            // Assign actual winner of current matchup
            SetMatchupWinner(currMatchup);

            // Update child matchup TeamCompeting object after matchup finish
            UpdateMatchupChildMatchup(tournament, currMatchup);

            TournamentRoundModel endRound = CheckCurrentRound(tournament);

            if(startRound != endRound && endRound != null)
            {
                endRound.SendMailForCurrentRound();
            }

            if(endRound == null)
            {
                CompleteTournament(tournament);
            }
        }
        private static void CompleteTournament(TournamentModel model)
        {
            // Update tournament model status to 1
            // Get the first winner
            TeamModel winners = model.Rounds.Last().MatchUps.First().Winner;
            // Get the second one in final competetion
            TeamModel runnderUp = model.Rounds.Last().MatchUps.First().Entries.Where(e => e.TeamCompeting != winners).First().TeamCompeting;

            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;

            if(model.TournamentPrizeModels.Count > 0)
            {
                decimal totalIncome = model.TournamentEntryModels.Count * model.EntryFee;

                PrizeModel firstPlacePrize = model.TournamentPrizeModels.Where(x => x.Prize.PlaceNumber == 1).FirstOrDefault().Prize;
                PrizeModel secondPlacePrize = model.TournamentPrizeModels.Where(x => x.Prize.PlaceNumber == 2).FirstOrDefault().Prize;

                if(firstPlacePrize !=null)
                {
                    winnerPrize = firstPlacePrize.CalcualtePrizeModel(totalIncome);
                }

                if(secondPlacePrize !=null)
                {
                    runnerUpPrize = secondPlacePrize.CalcualtePrizeModel(totalIncome);
                }
            }

            // Send mail to all tournaments

            string subject = "";
            StringBuilder body = new StringBuilder();

            subject = $"In {model.TournamentName}, {winners.TeamName} has won!";

            body.AppendLine("<h1>We have a winner!</h1>");
            body.AppendLine("<p>Congratulations to our winner on a greate tournament.</p>");
            body.AppendLine("<br />");

            if(winnerPrize > 0)
            {
                body.AppendLine($"<p>{winners.TeamName} will receive ${winnerPrize}");
            }
            if(runnerUpPrize > 0)
            {
                body.AppendLine($"<p>{runnderUp.TeamName} will receive ${runnerUpPrize}");
            }

            body.AppendLine("Thanks for a great tournament everyone!");
            body.AppendLine("~Tournament Tracker");

            List<string> bcc = new List<string>();

            model.TournamentEntryModels.ToList().ForEach(te => te.Team.TeamMembers.ToList().ForEach(tm => bcc.Add(tm.PersonModel.EmailAddress)) );

            EmailLogic.SendEmail(new List<string>(), bcc, subject, body.ToString());

            //complete tournament
            model.CompleteTournament();
        }
        private static decimal CalcualtePrizeModel(this PrizeModel prize, decimal totalIncome)
        {
            decimal output = 0;

            if(prize.PrizeAmount > 0)
            {
                output = prize.PrizeAmount;
            }
            else if(prize.PrizePercentage > 0)
            {
                output = totalIncome * (decimal)(prize.PrizePercentage / 100);
            }

            return output;
        }
        public static void SendMailForCurrentRound(this TournamentRoundModel round)
        {
            foreach(var matchup in round.MatchUps)
            {
                foreach(var matchEntry in matchup.Entries)
                {
                    foreach (var teamMember in matchEntry.TeamCompeting.TeamMembers)
                    {
                        AlertPersonForMatchup(
                            teamMember.PersonModel, 
                            teamMember.TeamModel.TeamName,
                            matchup.Entries.Where(e => e.TeamCompeting != matchEntry.TeamCompeting).FirstOrDefault(),
                            matchup.DisplayName,
                            round.Tournament.TournamentName);
                    }
                }
            }
        }

        private static void AlertPersonForMatchup(PersonModel person, string teamName, MatchupEntryModel matchupEntryModel, string displayName, string tournamentName)
        {
            string toMailAddress = person.EmailAddress;
            string mailSubject = "";
            StringBuilder body = new StringBuilder();

            mailSubject = $"Tournament-{tournamentName}, Matchup {displayName} Email Remind";

            if(matchupEntryModel == null)
            {
                body.AppendLine($"<h1>Tournaament: {tournamentName}</h1>");
                body.AppendLine($"<h2>Matchup {displayName}</h2>");
                body.AppendLine($"You have a bye week this round, please enjoy.");
            }
            else
            {
                body.AppendLine($"<h1>Tournaament: {tournamentName}</h1>");
                body.AppendLine($"<h2>Matchup {displayName}</h2>");

                body.AppendLine($"You have will have competition with {matchupEntryModel.TeamCompeting.TeamName}.");
                body.AppendLine($"Please do preparation.");
            }

            EmailLogic.SendEmail(toMailAddress, mailSubject, body.ToString());
        }

        private static void UpdateMatchupChildMatchup(TournamentModel tournament, MatchupModel matchup)
        {
            foreach (TournamentRoundModel round in tournament.Rounds)
            {
                round.MatchUps.ForEach(m =>
                {
                    MatchupEntryModel currEntry = m.Entries
                    .Where(e => e.ParentMatchupId == matchup.Id)
                    .FirstOrDefault();
                    if(currEntry != null) currEntry.TeamCompeting = matchup.Winner;
                });
            }
        }
        private static void SetMatchupWinner(MatchupModel currMatchup)
        {
            if (currMatchup.Entries.Count == 1)
            {
                currMatchup.Winner = currMatchup.Entries[0].TeamCompeting;
            }
            else
            {
                // Check who is the winner
                if (currMatchup.Entries[0].Score > currMatchup.Entries[1].Score)
                {
                    currMatchup.Winner = currMatchup.Entries[0].TeamCompeting;
                }
                else if (currMatchup.Entries[0].Score < currMatchup.Entries[1].Score)
                {
                    currMatchup.Winner = currMatchup.Entries[1].TeamCompeting;
                }
                else
                {
                    throw new Exception("Matchup winner logic doestn't support tie score.");
                }
            }
        }
        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }
        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int multiply = 2;

            while (teamCount > multiply)
            {
                multiply *= 2;
                output++;
            }

            return output;
        }
        /// <summary>
        /// Return a randmized list according to the input team list
        /// </summary>
        /// <param name="teams"></param>
        /// <returns></returns>
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(t => Guid.NewGuid()).ToList();
        }

    }
}
