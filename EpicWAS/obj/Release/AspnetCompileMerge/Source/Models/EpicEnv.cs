using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicWAS.Models
{
    public class EpicEnv
    {
        public string Env_ID { get; set; }
        public string Env_Description { get; set; }
        public bool Env_IsActive { get; set; }
        public string Env_AppServer { get; set; }
        public string Env_AppEpicor { get; set; }
        public string Env_AppUserId { get; set; }
        public string Env_AppPassKey { get; set; }
        public string Env_SQLServer { get; set; }
        public string Env_SQLDB { get; set; }
        public string Env_SQLUserId { get; set;}
        public string Env_SQLPassKey { get; set; }
        public string Env_BarCodeSeperator { get; set; }
        public string Env_BarCodeSeperator2 { get; set; }
        public bool Env_UsedEpicLogin { get; set; }


        public string Env_RESTAPIURL { get; set; }
        public string Env_RESTAPIKEY { get; set; }
    }
}