using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        // TODO - Make the CreatePrize method actually save to the text file
        /// <summary>
        /// Saves a new prize to a text
        /// </summary>
        /// <param name="model">The prize information</param>
        /// <returns>The prize infomation, with unique identifier</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using(IDbConnection connection = new Microsoft.Data.Sqlite.SqliteConnection(GlobalConfig.ConnString("Tournaments")))
            {

            }
            model.Id = 2;

            return model;
        }

    }
}
