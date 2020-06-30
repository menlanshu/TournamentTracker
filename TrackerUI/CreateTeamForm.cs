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
        public CreateTeamForm()
        {
            InitializeComponent();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            (bool Valid, string errorDesc) = ValidateForm();
            if (Valid)
            {
                PersonModel person = new PersonModel(
                    firstNameText.Text,
                    lastNameText.Text,
                    emailText.Text,
                    cellphoneText.Text);

                GlobalConfig.Connection.CreatePerson(person);

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

        /// <summary>
        /// Validate all input for team member text box
        /// </summary>
        /// <returns></returns>
        private (bool validInput, string errorDesc) ValidateForm()
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
    }
}
