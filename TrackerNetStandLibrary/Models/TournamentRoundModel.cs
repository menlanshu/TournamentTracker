using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    [Table("TournamentRounds")]
    public class TournamentRoundModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int RoundNumber { get; set; }
        [ExcludeFromTextFile]
        public virtual TournamentModel Tournament { get; set; }
        [ExcludeFromTextFile]
        public virtual List<MatchupModel> MatchUps { get; set; } = new List<MatchupModel>();
    }
}
