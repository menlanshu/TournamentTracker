﻿using System;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent matchup detail of a matchup?
    /// </summary>
    public class MatchupEntryModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Represents one team in the matchup.
        /// </summary>
        public TeamModel TeamCompeting { get; set; }
        /// <summary>
        /// Represents the score for this particular team.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Represents the matchup that this team came 
        /// from as a winner
        /// </summary>
        public MatchupEntryModel ParentMatchup { get; set; }
    }
}