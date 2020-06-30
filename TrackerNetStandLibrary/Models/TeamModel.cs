using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    /// <summary>
    /// Represent one team
    /// </summary>
    public class TeamModel
    {
        /// <summary>
        /// Team name of current team.
        /// </summary>
        public string TeamName { get; set; }
        /// <summary>
        /// A list of persons that belong to this team
        /// </summary>
        public List<PersonModel> TeamMember { get; set; } = new List<PersonModel>();
    }
}
