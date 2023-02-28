using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

namespace EpicWAS.Models
{
    public class SpecialCharHandler
    {
        public string replaceSpecialChar(string strToReplace)
        {
            string strReturnString = string.Empty;

            if (strToReplace.IndexOf("\''") != -1)
            { strReturnString = strToReplace.Replace("\''", @"''''"); }
            else if (strToReplace.IndexOf("\'") != -1)
            { strReturnString = strToReplace.Replace("\'", "''"); }
            else if (strToReplace.IndexOf("\"") != -1)
            { strReturnString = strToReplace.Replace("\"", @""""); }
            else
            { strReturnString = strToReplace; }

            /*
            string strMSSQL = string.Empty;

            strMSSQL = ConfigurationManager.AppSettings["MSSQL"].ToString();

            switch (strMSSQL)
            {

                case "2012":
                    strReturnString = strToReplace.Replace(@"\""", "''''");
                    break;
                case "2016": case "2019":
                    strReturnString = strToReplace.Replace("\'", "''");
                    break;
                default:
                    strReturnString = strToReplace.Replace("\"", "''''");
                    break;
            }
            */

            return strReturnString;

        }

        public string replaceSpecialCharCSharp(string strToReplace)
        {
            string strReturnString = strToReplace;

            if (strReturnString.IndexOf("\''") != -1)
                { strReturnString = (@strReturnString).Replace("\"", "''"); }
            else if (strReturnString.IndexOf("\'") != -1)
                { strReturnString = (@strReturnString).Replace("\"", "'"); }
            else if (strToReplace.IndexOf("\"") != -1)
            { strReturnString = strToReplace.Replace("\"", @""""); }
            else
            { strReturnString = strToReplace; }

            return strReturnString;
        }

    }
}