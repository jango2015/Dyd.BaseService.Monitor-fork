using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Config
    {
        public static string PlatformManageConnectString = System.Configuration.ConfigurationManager.AppSettings["PlatformManageConnectString"].ToString();
        public static string TimeWatchConnectString = System.Configuration.ConfigurationManager.AppSettings["TimeWatchConnectString"].ToString();
        public static string ClusterConnectString = System.Configuration.ConfigurationManager.AppSettings["ClusterConnectString"].ToString();
        public static string UnityLogConnectString = System.Configuration.ConfigurationManager.AppSettings["UnityLogConnectString"].ToString();
    }
}