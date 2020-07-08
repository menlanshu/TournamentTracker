using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent a tournament
    /// </summary>
    [Table("Tournaments")]
    public class TournamentModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Name of this Tournament
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        /// Entry fee of this Trounament
        /// </summary>
        public decimal EntryFee { get; set; }
        public int Active { get; set; }
        /// <summary>
        /// Link Prize with Tounament through tournament Prize models
        /// </summary>
        [ExcludeFromTextFile]
        public virtual ICollection<TournamentPrizeModel> TournamentPrizeModels { get; set; }
        /// <summary>
        /// Link Team with Tournament through tournament Entry models
        /// </summary>
        [ExcludeFromTextFile]
        public virtual ICollection<TournamentEntryModel> TournamentEntryModels { get; set; }
        /// <summary>
        /// List for all Matchups in this Tournament
        /// </summary>
        [ExcludeFromTextFile]
        public virtual List<TournamentRoundModel> Rounds { get; set; } = new List<TournamentRoundModel>();
    }
}
