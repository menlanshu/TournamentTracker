using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        /// <summary>
        /// We may have one or more place to save our data
        /// </summary>
        public static IDataConnection Connection { get; private set; }
        public static string ConnString { get; private set; }

        /// <summary>
        /// Initial Connections based on the input configuration
        /// </summary>
        /// <param name="database"></param>
        /// <param name="textFiles"></param>
        public static void InitializeConnections(DataBaseType db)
        {
            switch (db)
            {
                case DataBaseType.Sql:
                    SqlConnector sql = new SqlConnector();
                    Connection = sql;
                    GetConnString("Tournaments", db);
                    break;
                case DataBaseType.TextFile:
                    TextConnector text = new TextConnector();
                    Connection = text;
                    GetConnString("filePath", db);
                    break;
                default:
                    break;
            }
        }
        
        private static void GetConnString(string name, DataBaseType db)
        {
            switch (db)
            {
                case DataBaseType.Sql:
                    ConnString =  ConfigurationManager.ConnectionStrings[name].ConnectionString;
                    break;
                case DataBaseType.TextFile:
                    ConnString = ConfigurationManager.AppSettings[name];
                    break;
                default:
                    break;
            }
        }
    }
}
