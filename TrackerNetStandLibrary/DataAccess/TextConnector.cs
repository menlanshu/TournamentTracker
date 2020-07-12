using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";
        private const string TeamsFile = "TeamModels.csv";
        private const string TeamMembersFile = "TeamMemberModels.csv";
        private const string TournamentFile = "TournamentModels.csv";
        private const string TournamentEntryFile = "TournamentEntryModels.csv";
        private const string TournamentPrizeFile = "TournamentPrizeModels.csv";
        private const string TournamentRoundFile = "TournamentRoundModels.csv";
        private const string TournamentMatchupFile = "TournamentMatchupModels.csv";
        private const string TournamentMatchupEntryFile = "TournamentMatchupEntryModels.csv";

        public void CreatePerson(PersonModel model)
        {
            // load text file
            // convert test to a List<PrizeModel>
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToModel<PersonModel>();

            // find the id
            int currentId = (people.Count == 0) ? 1 : people.Max(x => x.Id) + 1;
            model.Id = currentId;

            // add the new record with the new id (max+1)
            people.Add(model);

            // convert the prizes to a List<string>
            // save the List<string> to the text file
            people.SaveToFile(PeopleFile);

        }

        /// <summary>
        /// Saves a new prize to a text
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize infomation, with unique identifier</returns>
        public void CreatePrize(PrizeModel model)
        {
            // load text file
            // convert test to a List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToModel<PrizeModel>();

            // find the id
            int currentId = (prizes.Count == 0) ? 1 : prizes.Max(x => x.Id) + 1;
            model.Id = currentId;

            // add the new record with the new id (max+1)
            prizes.Add(model);

            // convert the prizes to a List<string>
            // save the List<string> to the text file
            prizes.SaveToFile(PrizesFile);

        }

        public void CreateTeam(TeamModel model)
        {
            // Get list of team model from text file
            List<TeamModel> teamModels = TeamsFile.FullFilePath().LoadFile().ConvertToModel<TeamModel>();

            // find the id of team models
            int currentTeamId = (teamModels.Count == 0) ? 1 : teamModels.Max(x => x.Id) + 1;
            model.Id = currentTeamId;

            // Save team to text file, create a team model
            teamModels.Add(model);

            teamModels.SaveToFile(TeamsFile);

            // Save link to team members text file

            // Get list of teammember model from text file
            List<TeamMemberModel> teamMemberModels = TeamMembersFile.FullFilePath().LoadFile().ConvertToModel<TeamMemberModel>();

            // find the id of teammember models
            int currentTeamMemberId = (teamMemberModels.Count == 0) ? 1 : teamMemberModels.Max(x => x.Id) + 1;
            foreach (var teamMember in model.TeamMembers)
            {
                teamMember.Id = currentTeamMemberId;
                teamMember.PersonId = teamMember.PersonModel.Id;
                teamMember.TeamId = currentTeamId;
                teamMember.TeamModel = model;

                currentTeamMemberId++;
            }

            // save to teammembers's file
            teamMemberModels.AddRange(model.TeamMembers);
            teamMemberModels.ToList().SaveToFile(TeamMembersFile);

            // No need save person model, becasue they are got from people file

        }

        public void CreateTounament(TournamentModel tournament)
        {
            // Get list of tournament model from text file
            List<TournamentModel> tournamenetModels = TournamentFile.FullFilePath().LoadFile().ConvertToModel<TournamentModel>();

            // find the id of tournament models
            int currentTournamentId = (tournamenetModels.Count == 0) ? 1 : tournamenetModels.Max(x => x.Id) + 1;
            tournament.Id = currentTournamentId;

            // Add current tournament model to tournament list
            tournamenetModels.Add(tournament);

            // Save to Tournament Entry Models' file
            SaveTournamentEntryModels(tournament);

            // Save to Tournament Prize Models's file
            SaveTournamentPrizeModels(tournament);

            // Capture round information
            SaveTournamentRounds(tournament);

            // Save tornament to file
            tournamenetModels.SaveToFile(TournamentFile);

        }

        private void SaveTournamentRounds(TournamentModel tournament)
        {
            // Get List from Tournament Rounds file
            List<TournamentRoundModel> tournamentRounds = TournamentRoundFile.FullFilePath().LoadFile().ConvertToModel<TournamentRoundModel>();

            // Get the max id
            // Generate id for current new record
            int currentRoundId = (tournamentRounds.Count == 0) ? 1 : tournamentRounds.Max(x => x.Id) + 1;

            // Loop each round
            foreach (TournamentRoundModel round in tournament.Rounds)
            {
                round.Id = currentRoundId;
                round.TournamentId = tournament.Id;
                round.Tournament = tournament;

                // Add current round to list
                tournamentRounds.Add(round);

                // Save all math ups belong to current round to database
                SaveRoundMatchups(round);

                currentRoundId++;
            }

            // Save current round to text file
            tournamentRounds.SaveToFile(TournamentRoundFile);
        }

        private void SaveRoundMatchups(TournamentRoundModel round)
        {
            // Get All match up records from text file
            List<MatchupModel> matchups = TournamentMatchupFile.FullFilePath().LoadFile().ConvertToModel<MatchupModel>();

            // Get current match up id
            int currMatchupId = (matchups.Count == 0) ? 1 : matchups.Max(x => x.Id) + 1;

            // Loop each match up
            foreach (MatchupModel matchup in round.MatchUps)
            {
                // Assign value that need to be assigned in mathup model
                matchup.Id = currMatchupId;
                matchup.WinnerId = matchup.Winner?.Id;
                matchup.RoundId = matchup.Round.Id;

                // Add current mathup model into match up list
                matchups.Add(matchup);

                // Save all match up entries to file
                SaveMatchupEntrys(matchup);

                currMatchupId++;
            }

            // Save to matchup models's file
            matchups.SaveToFile(TournamentMatchupFile);
        }

        private void SaveMatchupEntrys(MatchupModel matchup)
        {
            // Get all match up entries from text file
            List<MatchupEntryModel> matchupEntries = TournamentMatchupEntryFile.FullFilePath().LoadFile().ConvertToModel<MatchupEntryModel>();

            // Get current match up entry id
            int currMatchupEntryId = (matchupEntries.Count == 0) ? 1 : matchupEntries.Max(x => x.Id) + 1;

            // Loop each match up entry in matchup
            foreach (MatchupEntryModel matchupEntry in matchup.Entries)
            {
                matchupEntry.Id = currMatchupEntryId;
                matchupEntry.MatchupId = matchup.Id;
                matchupEntry.TeamCompetingId = matchupEntry.TeamCompeting?.Id;
                matchupEntry.ParentMatchupId = matchupEntry.ParentMatchup?.Id;

                matchupEntries.Add(matchupEntry);

                currMatchupEntryId++;
            }


            matchupEntries.SaveToFile(TournamentMatchupEntryFile);
        }

        private void SaveTournamentPrizeModels(TournamentModel tournament)
        {
            // Get list of tournamentprize model from text file
            List<TournamentPrizeModel> tournamentPrizeModels = TournamentPrizeFile.FullFilePath().LoadFile().ConvertToModel<TournamentPrizeModel>();

            // find the id of Tournament Prize models
            int currentTournamentPrizeId = (tournamentPrizeModels.Count == 0) ? 1 : tournamentPrizeModels.Max(x => x.Id) + 1;
            foreach (var tournamentPrize in tournament.TournamentPrizeModels)
            {
                tournamentPrize.Id = currentTournamentPrizeId;
                tournamentPrize.TournamentId = tournament.Id;
                tournamentPrize.Tournament = tournament;

                currentTournamentPrizeId++;
            }

            // save to TournamentPrizeModel's file
            tournamentPrizeModels.AddRange(tournament.TournamentPrizeModels);
            tournamentPrizeModels.ToList().SaveToFile(TournamentPrizeFile);
        }

        private void SaveTournamentEntryModels(TournamentModel tournament)
        {
            // Save link to tournament entry text file

            // Get list of tournamententry model from text file
            List<TournamentEntryModel> tournamentEntryModels = TournamentEntryFile.FullFilePath().LoadFile().ConvertToModel<TournamentEntryModel>();

            // find the id of TournamentEntry models
            int currentTournamentEntryId = (tournamentEntryModels.Count == 0) ? 1 : tournamentEntryModels.Max(x => x.Id) + 1;
            foreach (var tournamentEntryModel in tournament.TournamentEntryModels)
            {
                tournamentEntryModel.Id = currentTournamentEntryId;
                tournamentEntryModel.TournamentId = tournament.Id;
                tournamentEntryModel.Tournament = tournament;

                currentTournamentEntryId++;
            }

            // save to TournamentEntry's file
            tournamentEntryModels.AddRange(tournament.TournamentEntryModels);
            tournamentEntryModels.ToList().SaveToFile(TournamentEntryFile);
        }

        /// <summary>
        /// Get person list from people text file
        /// </summary>
        /// <returns>A list of person model</returns>
        public List<PersonModel> GetPerson_All()
        {
            // Get List of person from text file
            List<PersonModel> people = new List<PersonModel>();

            people = PeopleFile.FullFilePath().LoadFile().ConvertToModel<PersonModel>();

            return people;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> teamModels = TeamsFile.FullFilePath().LoadFile().ConvertToModel<TeamModel>();
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToModel<PersonModel>();
            List<TeamMemberModel> teamMemberModels = TeamMembersFile.FullFilePath().LoadFile().ConvertToModel<TeamMemberModel>();

            foreach (var item in teamModels)
            {
                teamMemberModels.Where(tm => tm.TeamId == item.Id)
                    .ToList()
                    .ForEach(tm =>
                    {
                        tm.PersonModel = people.Where(p => p.Id == tm.PersonId).First();
                        tm.TeamModel = item;
                    }
                    );
                item.TeamMembers = teamMemberModels.Where(tm => tm.TeamId == item.Id).ToList();
            }

            return teamModels;
        }

        public List<TournamentModel> GetTournament_All()
        {
            // TODO - Implement actual logic here

            // Get list of Tournament
            List<TournamentModel> tournaments = TournamentFile.FullFilePath().LoadFile().ConvertToModel<TournamentModel>();
            // Get list of Rounds
            List<TournamentRoundModel> tournamentRounds = TournamentRoundFile.FullFilePath().LoadFile().ConvertToModel<TournamentRoundModel>();
            // Get list of Matchup
            List<MatchupModel> matchups = TournamentMatchupFile.FullFilePath().LoadFile().ConvertToModel<MatchupModel>();
            // Get list of Matchup Entries
            List<MatchupEntryModel> matchupEntries = TournamentMatchupEntryFile.FullFilePath().LoadFile().ConvertToModel<MatchupEntryModel>();

            // Get team list
            List<TeamModel> teams = TeamsFile.FullFilePath().LoadFile().ConvertToModel<TeamModel>();
            List<TournamentEntryModel> tournamentEntries = TournamentEntryFile.FullFilePath().LoadFile().ConvertToModel<TournamentEntryModel>();
            // Get prize list
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToModel<PrizeModel>();
            List<TournamentPrizeModel> tournamentPrizes = TournamentPrizeFile.FullFilePath().LoadFile().ConvertToModel<TournamentPrizeModel>();

            // Handle match up entries model
            matchupEntries.ForEach(me =>
            {
                me.Matchup = matchups.Where(m => m.Id == me.MatchupId).FirstOrDefault();
                me.ParentMatchup = matchups.Where(m => m.Id == me.ParentMatchupId).FirstOrDefault();
                me.TeamCompeting = teams.Where(t => t.Id == me.TeamCompetingId).FirstOrDefault();
            });

            // Assign Mathupentries to matchup model
            matchups.ForEach(m =>
                {
                    m.Winner = teams.Where(t => t.Id == m.WinnerId).FirstOrDefault();
                    m.Round = tournamentRounds.Where(tr => tr.Id == m.RoundId).FirstOrDefault();
                    m.Entries = matchupEntries.Where(me => me.MatchupId == m.Id).ToList();
                }
            );

            // Assign round model value
            tournamentRounds.ForEach(tr =>
            {
                tr.MatchUps = matchups.Where(me => me.RoundId == tr.Id).ToList();
                tr.Tournament = tournaments.Where(t => t.Id == tr.TournamentId).FirstOrDefault();
            });

            // Assign tournament entries
            tournamentEntries.ForEach(te =>
            {
                te.Tournament = tournaments.Where(t => t.Id == te.TournamentId).FirstOrDefault();
                te.Team = teams.Where(t => t.Id == te.TeamId).FirstOrDefault();
            });

            // Assign tournament prizes
            tournamentPrizes.ForEach(tp =>
            {
                tp.Prize = prizes.Where(p => p.Id == tp.PrizeId).FirstOrDefault();
                tp.Tournament = tournaments.Where(t => t.Id == tp.TournamentId).FirstOrDefault();
            });

            // Assign tournament model
            tournaments.ForEach(t =>
            {
               t.TournamentEntryModels = tournamentEntries
                                            .Where(te => te.TournamentId == t.Id)
                                            .ToList();

               t.TournamentPrizeModels = tournamentPrizes
                                            .Where(tp => tp.TournamentId == t.Id)
                                            .ToList();

               t.Rounds = tournamentRounds
                            .Where(tr => tr.TournamentId == t.Id)
                            .ToList();
            });

            return tournaments;
        }

        public void UpdateTournament(TournamentModel tournament)
        {
            // Get list of Matchup
            List<MatchupModel> matchups = TournamentMatchupFile.FullFilePath().LoadFile().ConvertToModel<MatchupModel>();
            // Get list of Matchup Entries
            List<MatchupEntryModel> matchupEntries = TournamentMatchupEntryFile.FullFilePath().LoadFile().ConvertToModel<MatchupEntryModel>();

            List<MatchupModel> tournamentMatchups = new List<MatchupModel>();

            tournament.Rounds.ForEach(r => tournamentMatchups.AddRange(r.MatchUps));


            List<MatchupEntryModel> tournamentMatchupEntries = new List<MatchupEntryModel>();

            tournament.Rounds.ForEach(r =>
                r.MatchUps.ForEach(m => tournamentMatchupEntries.AddRange(m.Entries)));

            // update data in original list
            tournamentMatchups.ForEach(tm => 
                { 
                    MatchupModel currentMatchup = matchups.Where(m => m.Id == tm.Id).FirstOrDefault();
                    currentMatchup.Winner = tm.Winner;
                    currentMatchup.WinnerId = tm.Winner?.Id;
                });


            tournamentMatchupEntries.ForEach(tme =>
                {
                    MatchupEntryModel currentMatchupEntry = matchupEntries.Where(me => me.Id == tme.Id).FirstOrDefault();
                    currentMatchupEntry.ParentMatchup = tme.ParentMatchup;
                    currentMatchupEntry.ParentMatchupId = tme.ParentMatchup?.Id;

                    currentMatchupEntry.Score = tme.Score;
                    currentMatchupEntry.TeamCompeting = tme.TeamCompeting;
                    currentMatchupEntry.TeamCompetingId = tme.TeamCompeting?.Id;
                });

            matchups.SaveToFile(TournamentEntryFile);
            matchupEntries.SaveToFile(TournamentMatchupEntryFile);
        }
    }
}
