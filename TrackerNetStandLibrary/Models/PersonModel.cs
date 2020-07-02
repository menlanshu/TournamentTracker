using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using TrackerLibrary.Attributes;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent a person
    /// </summary>
    [Table("People")]
    public class PersonModel
    {
        public int Id { get; set; }
        /// <summary>
        /// First name of this person
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last name of this person
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Email Address of this person
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Cellphone number of this person
        /// </summary>
        public string CellphoneNumber { get; set; }

        // TODO - Find a better way to exclude this item from covert file
        [ExcludeFromTextFileAttribute]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        [ExcludeFromTextFileAttribute]
        public virtual ICollection<TeamMemberModel> TeamMemberModels { get; set; }

        public PersonModel()
        {

        }
        public PersonModel(string firstName, string lastName, string emailAddress, string cellPhoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            CellphoneNumber = cellPhoneNumber;
        }
    }
}