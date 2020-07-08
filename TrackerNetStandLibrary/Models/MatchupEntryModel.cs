using System;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent matchup detail of a matchup?
    /// </summary>
    [Table("MatchUpEntries")]
    public class MatchupEntryModel
    {
        public int Id { get; set; }
        public int MatchupId { get; set; }
        [ExcludeFromTextFile]
        public virtual MatchupModel Matchup { get; set; }
        public int? Score { get; set; }
        public int? TeamCompetingId { get; set; }
        /// <summary>
        /// Represents one team in the matchup.
        /// </summary>
        [ExcludeFromTextFile]
        public TeamModel TeamCompeting { get; set; }
        /// <summary>
        /// Represents the score for this particular team.
        /// </summary>
        public int? ParentMatchupId { get; set; }
        /// <summary>
        /// Represents the matchup that this team came 
        /// from as a winner
        /// </summary>
        [ExcludeFromTextFile]
        public virtual MatchupModel ParentMatchup { get; set; }
    }
}