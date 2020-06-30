using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<T> ConvertToModel<T>(this List<string> lines)
        {
            //initialize a output List<T>
            List<T> output = new List<T>();

            //Convert each line to T and add it to output list
            foreach (string line in lines)
            {
                var temp = Activator.CreateInstance(typeof(T));
                string[] cols = line.Split(',');
                int currentCount = 0;

                foreach (var propInfo in typeof(T).GetProperties())
                {
                    if (propInfo.PropertyType == typeof(int))
                    {
                        propInfo.SetValue(temp, int.Parse(cols[currentCount]));
                    }
                    else if(propInfo.PropertyType == typeof(double))
                    {
                        propInfo.SetValue(temp, double.Parse(cols[currentCount]));
                    }
                    else if (propInfo.PropertyType == typeof(decimal))
                    {
                        propInfo.SetValue(temp, decimal.Parse(cols[currentCount]));
                    }
                    else
                    {
                        propInfo.SetValue(temp, cols[currentCount]);
                    }
                    currentCount++;
                }

                output.Add((T)temp);
            }

            //return output parameter
            return output;
        }

        /// <summary>
        /// Save list of object to text file
        /// </summary>
        /// <typeparam name="T">Model classes such as PrizeModel, PerosnModel, etc.</typeparam>
        /// <param name="models">A instance of an actual model</param>
        /// <param name="fileName">The file name of corresponding mode</param>
        public static void SaveToFile<T>(this List<T> models, string fileName)
        {
            List<string> lines = new List<string>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var model in models)
            {
                StringBuilder currentLine = new StringBuilder();
                foreach (PropertyInfo property in properties)
                {
                    currentLine.Append(property.GetValue(model)).Append(",");
                }
                lines.Add(currentLine.Remove(currentLine.Length - 1, 1).ToString());
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

    }
}
