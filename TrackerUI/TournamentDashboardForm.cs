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
    public partial class TournamentDashboardForm : Form, ICreateTournamentRequester
    {
        List<TournamentModel> _tournamentList = GlobalConfig.Connection.GetTournament_All();
        public TournamentDashboardForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            loadExistingTournamentDropDown.DataSource = null;

            loadExistingTournamentDropDown.DataSource = _tournamentList;
            loadExistingTournamentDropDown.DisplayMember = "TournamentName";
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm frm = new CreateTournamentForm(this);
            frm.Show();
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel currTournament = (TournamentModel)loadExistingTournamentDropDown.SelectedItem;

            if (currTournament == null)
            {
                MessageBox.Show($"Please select a tournament to load");
            }
            else
            {
                TournamentViewerForm frm = new TournamentViewerForm(currTournament);
                frm.Show();
            }
        }

        public void CreateTournamentComplete(TournamentModel tournament)
        {
            if(!_tournamentList.Contains(tournament))
            {
                _tournamentList.Add(tournament);

                WireUpLists();
            }
        }
    }
}
