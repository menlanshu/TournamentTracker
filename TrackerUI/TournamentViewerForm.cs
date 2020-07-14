using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private TournamentModel _tournament;
        public TournamentViewerForm(TournamentModel tournament)
        {
            InitializeComponent();

            _tournament = tournament;
            _tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

            LoadFormData();
            LoadRounds();
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            this.Close();
        }

        private void LoadFormData()
        {
            tournamentName.Text = _tournament.TournamentName;

        }

        private void LoadRounds()
        {
            roundDropDown.DataSource = null;

            roundDropDown.DataSource = _tournament.Rounds;
            roundDropDown.DisplayMember = "RoundNumber";
        }

        private void LoadMathupList(TournamentRoundModel round, bool onlyeLoadUnPlayed)
        {
            if (round != null)
            {
                matchupListBox.DataSource = null;

                matchupListBox.DataSource = onlyeLoadUnPlayed ? round.MatchUps.Where(m => (m.Winner == null)).ToList() : round.MatchUps;
                matchupListBox.DisplayMember = "DisplayName";

                SetComparePaneVisible(matchupListBox.Items.Count > 0);
            }
        }


        private void SetComparePaneVisible(bool isVisible)
        {
            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreText.Visible = isVisible;
            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreText.Visible = isVisible;

            scoreButton.Visible = isVisible;
            versusLabel.Visible = isVisible;
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            TournamentRoundModel selectRound = (TournamentRoundModel)roundDropDown.SelectedItem;

            LoadMathupList(selectRound, unPlayedOnlyCheckBox.Checked);
           
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MatchupModel matchup = (MatchupModel)matchupListBox.SelectedItem;

            LoadMatchupEntry(matchup);

        }

        private void LoadMatchupEntry(MatchupModel matchup)
        {
            if (matchup != null)
            {
                teamTwoScoreText.Enabled = true;
                if (matchup.Entries.Count == 1)
                {
                    teamOneName.Text = matchup.Entries[0].TeamCompeting?.TeamName;
                    teamOneScoreText.Text = matchup.Entries[0].Score.ToString();

                    teamTwoName.Text = "<Bye>";
                    teamTwoScoreText.Text = "";
                    teamTwoScoreText.Enabled = false;
                }
                else if (matchup.Entries.Count == 2)
                {
                    teamOneName.Text = matchup.Entries[0].TeamCompeting == null ? "Not Determined Yet" : matchup.Entries[0].TeamCompeting.TeamName;
                    teamOneScoreText.Text = matchup.Entries[0].Score.ToString();

                    teamTwoName.Text = matchup.Entries[1].TeamCompeting == null ? "Not Determined Yet" : matchup.Entries[1].TeamCompeting.TeamName;
                    teamTwoScoreText.Text = matchup.Entries[1].Score.ToString();
                }
            }
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            // Validate score input right or not
            (bool isValid, string errorDesc, int teamOneScore, int teamTwoScore) = ValidateScoreInput();
            MatchupModel currMatchup = (MatchupModel)matchupListBox.SelectedItem;

            if (isValid)
            {
                // Set Score
                if (currMatchup.Entries.Count == 1)
                {
                    currMatchup.Entries[0].Score = teamOneScore;
                }
                else
                {
                    currMatchup.Entries[0].Score = teamOneScore;
                    currMatchup.Entries[1].Score = teamTwoScore;
                }

                try
                {
                    TournamentLogic.UpdateMatchup(_tournament, currMatchup); ;

                }
                catch (Exception  err)
                {
                    MessageBox.Show($"Errod happen when update current Matchup: {err.Message}");
                    return;
                }

                GlobalConfig.Connection.UpdateTournament(_tournament);

                LoadMathupList((TournamentRoundModel)roundDropDown.SelectedItem, unPlayedOnlyCheckBox.Checked);

            }
            else
            {
                MessageBox.Show(errorDesc);
            }
        }


        private (bool isValid, string errorDesc, int teamOneScore, int teamTwoScore) ValidateScoreInput()
        {
            bool isValid = true;
            string errorDesc = "";

            int teamOneScore = 0, teamTwoScore = 0;

            MatchupModel currMatchup = (MatchupModel)matchupListBox.SelectedItem;

            if (currMatchup.Entries.Count == 2)
            {
                foreach (MatchupEntryModel entry in currMatchup.Entries)
                {
                    if (entry.TeamCompeting == null)
                    {
                        isValid = false;
                        errorDesc = "Not all team parent match up finished\n";
                    }
                }
            }

            if (!int.TryParse(teamOneScoreText.Text, out teamOneScore))
            {
                isValid = false;
                errorDesc = "Team One Score is not an available number\n";
            }

            if (teamTwoScoreText.Enabled)
            {
                if (!int.TryParse(teamTwoScoreText.Text, out teamTwoScore))
                {
                    isValid = false;
                    errorDesc += "Team Two Score is not an available number\n";
                }

                if (teamOneScore == teamTwoScore)
                {
                    isValid = false;
                    errorDesc += "Two teams do not support tie score!\n";

                }
            }

            return (isValid, errorDesc, teamOneScore, teamTwoScore);
        }

        private void unPlayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMathupList((TournamentRoundModel)roundDropDown.SelectedItem, unPlayedOnlyCheckBox.Checked);
        }
    }
}
