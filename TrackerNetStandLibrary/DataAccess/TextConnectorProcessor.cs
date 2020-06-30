using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        /// <summary>
        /// Get full path of file name of current data model
        /// </summary>
        /// <param name="fileName">Data Model csv file name</param>
        /// <returns></returns>
        public static string FullFilePath(this string fileName) 
        {
            return $"{ GlobalConfig.ConnString }\\{fileName}";
        }

        /// <summary>
        /// Load text in file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string> LoadFile(this string file)
        {
            if(!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }


        public static List<PrizeModel> ConvertToPrizeModel(this List<string> lines)
        {
            //initialize a output List<PrizeModel>
            List<PrizeModel> output = new List<PrizeModel>();

            //Convert each line to PrizeModel and add it to output list
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);

                output.Add(p);
            }

            //return output parameter
            return output;
        }

        public static void SaveToPrizesFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (var model in models)
            {
                lines.Add($"{model.Id},{model.PlaceNumber},{model.PlaceName},{model.PrizeAmount},{model.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
    }
}
