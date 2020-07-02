using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent a prize
    /// </summary>
    [Table("Prizes")]
    public class PrizeModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Place number for this prize
        /// </summary>
        public int PlaceNumber { get; set; }
        /// <summary>
        /// Place name for this prize
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// Prize amount for this prize
        /// </summary>
        public decimal PrizeAmount { get; set; }
        /// <summary>
        /// Prize percentaget for this prize
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;

            int.TryParse(placeNumber, out int placeNumberInt);
            PlaceNumber = placeNumberInt;

            decimal.TryParse(prizeAmount, out decimal prizeAmountDecimal);
            PrizeAmount = prizeAmountDecimal;

            double.TryParse(prizePercentage, out double prizePercentageDouble);
            PrizePercentage = prizePercentageDouble;
        }
    }
}