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
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrize : Form
    {
        public CreatePrize()
        {
            InitializeComponent();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            (bool Valid, string errorDesc) = ValidateForm();
            if(Valid)
            {
                PrizeModel prize = new PrizeModel(
                    placeNameText.Text,
                    placeNumberText.Text,
                    prizeAmountText.Text,
                    prizePercentageText.Text);

                GlobalConfig.Connection.CreatePrize(prize);

                InitializeTextBoxText();
            }
            else
            {
                MessageBox.Show(errorDesc);
            }
        }

        private void InitializeTextBoxText()
        {
            placeNameText.Text = "";
            placeNumberText.Text = "";
            prizeAmountText.Text = "0";
            prizePercentageText.Text = "0";
        }

        /// <summary>
        /// Validate all input in those text box
        /// </summary>
        /// <returns></returns>
        private (bool validInput, string errorDesc) ValidateForm()
        {
            bool output = true;
            string errorDesc = "";

            if (!int.TryParse(placeNumberText.Text, out int placeNumber))
            {
                output = false;
                errorDesc = "Place Number is not valid number" ;
            }

            if(placeNumber < 1)
            {
                output = false;
                errorDesc += "\nPlace Number can not be small then 1";
            }

            if(placeNameText.Text.Length == 0)
            {
                output = false;
                errorDesc += "\nPlace Name can not be empty";
            }

            //validate Prize amount which should be a decimal
            if (!decimal.TryParse(prizeAmountText.Text, out decimal prizeAmount))
            {
                output = false;
                errorDesc += "\nPrize Amount input invalid for parse to number";
            }
            if (!double.TryParse(prizePercentageText.Text, out double prizePercentage))
            {
                output = false;
                errorDesc += "\nPrize Percentage input invalid for parse to number";
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false;
                errorDesc += "\nPrize Amount and PrizePercentage can not be both smaller than 0";
            }

            if(prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
                errorDesc += "\nPrize Percentage can not be smaller than 0 and can not be larger than 100";
            }

            return (output, errorDesc);
        }
    }
}
