using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TrackerLibrary.Models
{
    public class TeamMemberModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PersonId { get; set; }
        //[ForeignKey("TeamId")]
        //public virtual TeamModel TeamModel { get; set; }
        //[ForeignKey("PersonId")]
        //public virtual PersonModel PersonModel { get; set; }
    }
}
