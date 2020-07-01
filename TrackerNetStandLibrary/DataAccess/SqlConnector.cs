using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        /// <summary>
        /// Save a new Person to the database
        /// </summary>
        /// <param name="model">A new person information</param>
        /// <returns>The person information, include the unique identifier</returns>
        public PersonModel CreatePerson(PersonModel model)
        {
            using(var context = new SQLiteContext(GlobalConfig.ConnString))
            {
                context.People.Add(model);
                context.SaveChanges();

                return model;
            }
        }

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using(var context = new SQLiteContext(GlobalConfig.ConnString))
            {
                context.Prizes.Add(model);
                context.SaveChanges();

                return model;
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            using (var context = new SQLiteContext(GlobalConfig.ConnString))
            {
                return context.People.ToList();
            }
        }
    }
}
