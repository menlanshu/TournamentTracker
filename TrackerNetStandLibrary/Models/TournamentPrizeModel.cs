using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    [Table("TournamentPrizes")]
    public class TournamentPrizeModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int PrizeId { get; set; }
        [ExcludeFromTextFile]
        public virtual TournamentModel Tournament { get; set; }
        [ExcludeFromTextFile]
        public virtual PrizeModel Prize { get; set; }
    }
}
