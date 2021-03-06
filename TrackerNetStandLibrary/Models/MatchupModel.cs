﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerLibrary.Attributes;

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
        /// Winner team id
        /// </summary>
        public int? WinnerId { get; set; }
        /// <summary>
        /// And who is the winner of current matchup
        /// </summary>
        [ExcludeFromTextFile]
        public virtual TeamModel Winner { get; set; }
        /// <summary>
        /// The Round of current Matchup
        /// </summary>
        public int RoundId { get; set; }
        [ExcludeFromTextFile]
        public virtual TournamentRoundModel Round { get; set; }
        /// <summary>
        /// A list of match up entrys for current MatchupModel
        /// </summary>
        [ExcludeFromTextFile]
        public virtual List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        [ExcludeFromTextFile]
        [NotMapped]
        public string DisplayName {
            get
            {
                string name = "";
                if (Entries.Count == 1)
                {
                    name = $"{Entries[0].TeamCompeting?.TeamName}-vs-<Bye>";
                }else if(Entries.Count == 2)
                {
                    name = Entries[0].TeamCompeting == null ? "Not Deterimined" : Entries[0].TeamCompeting?.TeamName;
                    name = name + "-vs-" + (Entries[1].TeamCompeting == null ? "Not Deterimined" : Entries[1].TeamCompeting?.TeamName);
                }

                return name;
            } 
        }
    }
}