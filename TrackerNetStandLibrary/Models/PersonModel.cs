using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace TrackerLibrary.Models
{
    /// <summary>
    /// Represent a person
    /// </summary>
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
        [DataMember]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

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