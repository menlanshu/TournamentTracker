using System.Collections.Generic;

namespace TrackerLibrary
{
    /// <summary>
    /// Represent actual mathup
    /// </summary>
    public class MatchupModel
    {
        /// <summary>
        /// A list of match up entrys for current MatchupModel
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        /// <summary>
        /// And who is the winner of current matchup
        /// </summary>
        public TeamModel Winner { get; set; }
        /// <summary>
        /// The Round of current Matchup
        /// </summary>
        public int MatchupRound { get; set; }
    }
}