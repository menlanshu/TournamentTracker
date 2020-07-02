using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    [Table("TeamMembers")]
    public class TeamMemberModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PersonId { get; set; }
        [ExcludeFromTextFileAttribute]
        public virtual TeamModel TeamModel { get; set; }
        [ExcludeFromTextFileAttribute]
        public virtual PersonModel PersonModel { get; set; }
    }
}
