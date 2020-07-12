using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerUI.Interface;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, ITeamRequester, IPrizeRequester
    {
        private List<TeamModel> _availableTeams = GlobalConfig.Connection.GetTeam_All();
        private List<TeamModel> _selectedTeams = new List<TeamModel>();
        private List<PrizeModel> _selctedPrizes = new List<PrizeModel>();

        private ICreateTournamentRequester _createTournamentRequester;

        public CreateTournamentForm(ICreateTournamentRequester createTournamentRequester)
        {
            InitializeComponent();

            _createTournamentRequester = createTournamentRequester;

            WireUpList();
        }

        private void WireUpList()
        {
            selectTeamDropDown.DataSource = null;
            tournamentPlayersListBox.DataSource = null;
            prizesListBox.DataSource = null;

            selectTeamDropDown.DataSource = _availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            //List<PersonModel> personModels = new List<PersonModel>();
            //((TeamModel)selectTeamDropDown.SelectedItem).TeamMemberModels.ToList().ForEach(p => personModels.Add(p.PersonModel));

            //tournamentPlayersListBox.DataSource = personModels;
            //tournamentPlayersListBox.DisplayMember = "FullName"; 

            tournamentPlayersListBox.DataSource = _selectedTeams;
            tournamentPlayersListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = _selctedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel teamModel = (TeamModel)selectTeamDropDown.SelectedItem;

            if(teamModel != null)
            {
                _availableTeams.Remove(teamModel);
                _selectedTeams.Add(teamModel);

                WireUpList();
            }
        }

        public void PrizeComplete(PrizeModel prizeModel)
        {
            if(prizeModel !=null)
            {
                _selctedPrizes.Add(prizeModel);

                WireUpList();
            }
        }

        public void TeamComplete(TeamModel teamModel)
        {
            if(teamModel != null)
            {
                _selectedTeams.Add(teamModel);

                WireUpList();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // Create prize form
            CreatePrizeForm frm = new CreatePrizeForm(this);

            // Show team form
            frm.Show();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Create team form
            CreateTeamForm frm = new CreateTeamForm(this);

            // Show team form
            frm.Show();
        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            // Get select item in team member lsit
            TeamModel p = (TeamModel)tournamentPlayersListBox.SelectedItem;

            if (p != null)
            {
                // Remove team from selected team list
                _selectedTeams.Remove(p);

                // Add team to available team list
                _availableTeams.Add(p);

                // Refresh control
                WireUpList();

            }
        }

        private void deleteSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;

            if(p != null)
            {
                _selctedPrizes.Remove(p);


                WireUpList();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate input in this form
            (bool validInput, string errorDesc) = ValidateTournamentFormControl();

            if (validInput)
            {
                // Create Tournament entry
                TournamentModel tm = new TournamentModel();

                tm.TournamentName = tournamentNameText.Text;
                // Thie statement rely on the result of ValidateTournamentFormControl
                // Otherwise if any abnormal with entry fee text control, error happen
                tm.EntryFee = decimal.Parse(entryFeeText.Text);

                List<TournamentPrizeModel> tournamentPrizeModels = new List<TournamentPrizeModel>();
                // Create Tournament prizes entries
                foreach(PrizeModel prize in _selctedPrizes)
                {
                    TournamentPrizeModel tournamentPrizeModel = new TournamentPrizeModel();

                    tournamentPrizeModel.Tournament = tm;
                    tournamentPrizeModel.Prize = prize;

                    tournamentPrizeModels.Add(tournamentPrizeModel);
                }

                List<TournamentEntryModel> tournamentEntryModels = new List<TournamentEntryModel>();
                // Create Tournament entries
                foreach(TeamModel team in _selectedTeams)
                {
                    TournamentEntryModel tournamentEntryModel = new TournamentEntryModel();

                    tournamentEntryModel.Tournament = tm;
                    tournamentEntryModel.Team = team;

                    tournamentEntryModels.Add(tournamentEntryModel);
                }

                tm.TournamentPrizeModels = tournamentPrizeModels;
                tm.TournamentEntryModels = tournamentEntryModels;
                tm.Active = 1;

                // Create our matchs
                // Wire up matchups

                // Create Round for current Tournament
                TournamentLogic.CreateRound(tm);

                // Create Tournament model
                GlobalConfig.Connection.CreateTounament(tm);


                TournamentViewerForm frm = new TournamentViewerForm(tm);
                frm.Show();

                _createTournamentRequester.CreateTournamentComplete(tm);
                this.Close();
            }
            else
            {
                MessageBox.Show(errorDesc);
            }
        }

        private (bool validInput, string errorDesc) ValidateTournamentFormControl()
        {
            bool output = true;
            string errorDesc = "";

            if (tournamentNameText.Text.Length == 0)
            {
                output = false;
                errorDesc += "Tournament name can not be empty\n";
            }

            if(!decimal.TryParse(entryFeeText.Text, out decimal decResult))
            {
                output = false;
                errorDesc += "Entry fee is not right for decimal";
            }

            if (tournamentPlayersListBox.Items.Count == 0)
            {
                output = false;
                errorDesc += "You must add some team to this tournament";
            }

            return (output, errorDesc);

        }
    }
}
