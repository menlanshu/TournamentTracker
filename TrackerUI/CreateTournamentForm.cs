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

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form
    {
        private List<TeamModel> _availableTeams = GlobalConfig.Connection.GetTeam_All();
        private List<TeamModel> _selectedTeams = new List<TeamModel>();
        private List<PrizeModel> _selctedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

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
    }
}
