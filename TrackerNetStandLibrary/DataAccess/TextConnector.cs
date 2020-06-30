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

        // TODO - Make the CreatePrize method actually save to the text file
        /// <summary>
        /// Saves a new prize to a text
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize infomation, with unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // load text file
            // convert test to a List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModel();

            // find the id
            int currentId = (prizes.Count == 0 ) ? 1 : prizes.Max(x => x.Id) + 1;
            model.Id = currentId;

            // add the new record with the new id (max+1)
            prizes.Add(model);

            // convert the prizes to a List<string>
            // save the List<string> to the text file
            prizes.SaveToPrizesFile(PrizesFile);

            return model;
        }

    }
}
