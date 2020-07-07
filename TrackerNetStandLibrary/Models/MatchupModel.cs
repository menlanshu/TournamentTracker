using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent actual mathup
    /// </summary>
    [Table("MatchUps")]
    public class MatchupModel
    {
        public int Id { get; set; }
        /// <summary>
        /// The Round of current Matchup
        /// </summary>
        public int MatchupRound { get; set; }
        /// <summary>
        /// And who is the winner of current matchup
        /// </summary>
        public virtual TeamModel Winner { get; set; }
        /// <summary>
        /// A list of match up entrys for current MatchupModel
        /// </summary>
        public virtual List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
    }
}