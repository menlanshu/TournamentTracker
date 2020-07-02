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
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        public CreateTeamForm()
        {
            InitializeComponent();

            //CreateSampleData();

            WireUpLists();
        }

        /// <summary>
        /// For drop down and list box test sample data
        /// </summary>
        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Fu", LastName = "Long" });
            availableTeamMembers.Add(new PersonModel { FirstName = "Kuang", LastName = "DaLi" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Hua", LastName = "TingTing" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Cai", LastName = "Qingjing" });

        }

        /// <summary>
        /// Wrapup dropdown list and list box
        /// </summary>
        private void WireUpLists()
        {
            // TODO - Whether there is a better to update data source
            selectMemberDropDown.DataSource = null;
            teamMembersListBox.DataSource = null;

            //assign data source of available member drop down
            selectMemberDropDown.DataSource = availableTeamMembers;
            selectMemberDropDown.DisplayMember = "FullName";

            //assign data source of selected team member list
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            (bool Valid, string errorDesc) = ValidateMemberGroupControl();
            if (Valid)
            {
                PersonModel person = new PersonModel(
                    firstNameText.Text,
                    lastNameText.Text,
                    emailText.Text,
                    cellphoneText.Text);

                GlobalConfig.Connection.CreatePerson(person);

                // Add person to selected team memeber list
                selectedTeamMembers.Add(person);

                // Update control info
                WireUpLists();

                InitializeMemberTextBox();
            }
            else
            {
                MessageBox.Show(errorDesc);
            }
        }

        private void InitializeMemberTextBox()
        {
            firstNameText.Text = "";
            lastNameText.Text = "";
            emailText.Text = "";
            cellphoneText.Text = "";
        }

        private void InitialTeamFormControl()
        {
            teamNameText.Text = "";

            availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
            selectedTeamMembers = null;

            WireUpLists();
        }

        /// <summary>
        /// Validate all input for team member text box
        /// </summary>
        /// <returns></returns>
        private (bool validInput, string errorDesc) ValidateMemberGroupControl()
        {
            bool output = true;
            string errorDesc = "";

            if (firstNameText.Text.Length == 0)
            {
                output = false;
                errorDesc += "First Name can not be empty\n";
            }

            if (lastNameText.Text.Length == 0)
            {
                output = false;
                errorDesc += "Last Name can not be empty\n";
            }

            if (emailText.Text.Length == 0)
            {
                output = false;
                errorDesc += "Email Address can not be empty\n";
            }

            if (cellphoneText.Text.Length == 0)
            {
                output = false;
                errorDesc += "Cellphone number can not be empty\n";
            }

            return (output, errorDesc);
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            // Take person out of available members list
            PersonModel p = (PersonModel)selectMemberDropDown.SelectedItem;

            if (p != null)
            {
                availableTeamMembers.Remove(p);

                // Add person into list box
                selectedTeamMembers.Add(p);

                // Refresh dropdown list and list box after change
                WireUpLists();

            }
        }

        private void deleteSelectedTeamMember_Click(object sender, EventArgs e)
        {
            // Get select item in team member lsit
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                // Remove person from selectedTeamMember
                selectedTeamMembers.Remove(p);

                // Add person to available member list
                availableTeamMembers.Add(p);

                // Refresh control
                WireUpLists();

            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            // Validate control status
            // Check team member list right of not

            (bool Valid, string errorDesc) = ValidateTeamFormControl();

            if(Valid)
            {
                List<TeamMemberModel> teamMemberModels = new List<TeamMemberModel>();

                // Create Current Team
                TeamModel teamModel = new TeamModel
                {
                    TeamName = teamNameText.Text
                };

                // Save team to data base?
                foreach(PersonModel member in teamMembersListBox.Items)
                {
                    TeamMemberModel teamMemberModel = new TeamMemberModel();
                    teamMemberModel.PersonModel = member;
                    teamMemberModel.TeamModel = teamModel;

                    teamMemberModels.Add(teamMemberModel);
                }

                teamModel.TeamMemberModels = teamMemberModels;

                GlobalConfig.Connection.CreateTeam(teamModel);

                InitialTeamFormControl();
            }
            else
            {
                MessageBox.Show(errorDesc);
            }

        }
        private (bool validInput, string errorDesc) ValidateTeamFormControl()
        {
            bool output = true;
            string errorDesc = "";

            if(teamNameText.Text.Length == 0)
            {
                output = false;
                errorDesc += "Team name can not be empty\n";
            }

            if(teamMembersListBox.Items.Count == 0)
            {
                output = false;
                errorDesc += "You cann't create a team with no member\n";
            }

            return (output, errorDesc);

        }
    }
}
