using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            // load text file
            // convert test to a List<PrizeModel>
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToModel<PersonModel>();

            // find the id
            int currentId = (people.Count == 0 ) ? 1 : people.Max(x => x.Id) + 1;
            model.Id = currentId;

            // add the new record with the new id (max+1)
            people.Add(model);

            // convert the prizes to a List<string>
            // save the List<string> to the text file
            people.SaveToFile(PeopleFile);

            return model;
        }

        /// <summary>
        /// Saves a new prize to a text
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize infomation, with unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // load text file
            // convert test to a List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToModel<PrizeModel>();

            // find the id
            int currentId = (prizes.Count == 0 ) ? 1 : prizes.Max(x => x.Id) + 1;
            model.Id = currentId;

            // add the new record with the new id (max+1)
            prizes.Add(model);

            // convert the prizes to a List<string>
            // save the List<string> to the text file
            prizes.SaveToFile(PrizesFile);

            return model;
        }

    }
}
