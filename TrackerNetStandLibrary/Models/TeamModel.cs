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
        [ExcludeFromTextFileAttribute]
        public virtual ICollection<TeamMemberModel> TeamMemberModels { get; set; }
        [ExcludeFromTextFileAttribute]
        public virtual ICollection<TournamentEntryModel> TournamentEntryModels { get; set; }
    }
}
