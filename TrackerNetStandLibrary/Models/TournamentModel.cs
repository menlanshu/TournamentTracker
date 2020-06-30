using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent a tournament
    /// </summary>
    public class TournamentModel
    {
        /// <summary>
        /// Name of this Tournament
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        /// Entry fee of this Trounament
        /// </summary>
        public decimal EntryFee { get; set; }
        /// <summary>
        /// Teams that will participate in this Tournament
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        /// <summary>
        /// Prizes for winners of this Tournament
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        /// <summary>
        /// List for all Matchups in this Tournament
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    }
}
