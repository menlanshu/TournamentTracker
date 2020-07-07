using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    [Table("TournamentEntries")]
    public class TournamentEntryModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }
        [ExcludeFromTextFile]
        public virtual TournamentModel Tournament { get; set; }
        [ExcludeFromTextFile]
        public virtual TeamModel Team { get; set; }
    }
}
