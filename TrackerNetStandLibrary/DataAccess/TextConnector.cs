using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;
using System.Collections.Immutable;

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

        public PersonModel CreatePerson(PersonModel model)
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

            return model;
        }

        /// <summary>
        /// Saves a new prize to a text
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize infomation, with unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
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

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
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
            foreach (var teamMember in model.TeamMemberModels)
            {
                teamMember.Id = currentTeamMemberId;
                teamMember.PersonId = teamMember.PersonModel.Id;
                teamMember.TeamId = currentTeamId;
                teamMember.TeamModel = model;

                currentTeamMemberId++;
            }

            // save to teammembers's file
            model.TeamMemberModels.ToList().SaveToFile(TeamMembersFile);

            // No need save person model, becasue they are got from people file

            return model;
        }

        public TournamentModel CreateTounament(TournamentModel tournament)
        {
            // Get list of tournament model from text file
            List<TournamentModel> tournamenetModels = TournamentFile.FullFilePath().LoadFile().ConvertToModel<TournamentModel>();

            // find the id of tournament models
            int currentTournamentId = (tournamenetModels.Count == 0) ? 1 : tournamenetModels.Max(x => x.Id) + 1;
            tournament.Id = currentTournamentId;

            // Save tournament to text file, create a tournament model
            tournamenetModels.Add(tournament);

            tournamenetModels.SaveToFile(TournamentFile);


            // Save to Tournament Entry Models' file
            SaveTournamentEntryModels(tournament);

            // Save to Tournament Prize Models's file
            SaveTournamentPrizeModels(tournament);

            // TODO - Capture round information

            return tournament;
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
            tournament.TournamentPrizeModels.ToList().SaveToFile(TournamentPrizeFile);
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
            tournament.TournamentEntryModels.ToList().SaveToFile(TournamentEntryFile);
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

        // TODO - Realize Actual logic of this part
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
                item.TeamMemberModels = teamMemberModels.Where(tm => tm.TeamId == item.Id).ToList() ;
            }

            return teamModels;
        }
    }
}
