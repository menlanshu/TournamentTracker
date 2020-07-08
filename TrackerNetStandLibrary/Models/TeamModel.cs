using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent one team
    /// </summary>
    [Table("Teams")]
    public class TeamModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Team name of current team.
        /// </summary>
        public string TeamName { get; set; }
        [ExcludeFromTextFile]
        public virtual ICollection<TeamMemberModel> TeamMembers { get; set; }
        [ExcludeFromTextFile]
        public virtual ICollection<TournamentEntryModel> TournamentEntrys { get; set; }
        [ExcludeFromTextFile]
        [NotMapped]
        public virtual ICollection<MatchupModel> Matchups { get; set; }
        [ExcludeFromTextFile]
        [NotMapped]
        public virtual ICollection<MatchupEntryModel> MatchupEntrys { get; set; }
    }
}
