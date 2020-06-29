namespace TrackerLibrary
{
    /// <summary>
    /// Represent a prize
    /// </summary>
    public class PrizeModel
    {
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
    }
}