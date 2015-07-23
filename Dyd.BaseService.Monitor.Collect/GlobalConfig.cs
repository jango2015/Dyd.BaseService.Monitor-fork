using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.ProjectTool;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Collect
{
    public class GlobalConfig
    {
        public static tb_cluster_model ClusterModel { get; set; }

       
        public static string ServerIP { get; set; }
        public static int MonitorCollectBackgroundTaskSleepTime = 10;
        public static int PerformanceCollectBackgroundTaskSleepTime = 10;
        public static int ConfigUpdateBackgroundTaskSleepTime = 10;
        public static int OnLineTimeBackgroundTaskSleepTime = 10;
        public static List<tb_database_config_model> DataBaseConfigModels { get; set; }

        public static void LoadBaseConfig()
        {
            try
            {
                DataBaseConfigModels = new List<tb_database_config_model>();
                SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
                {
                    tb_database_config_dal databasedal = new tb_database_config_dal();
                    var modellist = databasedal.GetModelList(c);
                    if (modellist != null)
                    {
                        DataBaseConfigModels = modellist;
                    }
                });
            }
            catch (Exception exp)
            {
                LogHelper.Error(string.Format("服务器[id:{0}] ", GlobalConfig.ServerIP.NullToEmpty()) + "LoadBaseConfig", exp);
            }

        }

        private static DateTime LoadClusterConfigTime = DateTime.Parse("1900-01-01");
        public static void LoadClusterConfig()
        {
            try
            {
                SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
                {
                    tb_cluster_dal clusterdal = new tb_cluster_dal();
                    var clustermodel = clusterdal.GetNewByServerip(c, ServerIP, LoadClusterConfigTime);
                    if (clustermodel != null)
                    {
                        LoadClusterConfigTime = c.GetServerDate();
                        ClusterModel = clustermodel;
                    }

                    tb_keyvalue_config_dal keyvalueconfigdal = new tb_keyvalue_config_dal();
                    MonitorCollectBackgroundTaskSleepTime = Convert.ToInt32(keyvalueconfigdal.Get(c, "MonitorCollectBackgroundTaskSleepTime").value);
                    PerformanceCollectBackgroundTaskSleepTime = Convert.ToInt32(keyvalueconfigdal.Get(c, "PerformanceCollectBackgroundTaskSleepTime").value);
                    ConfigUpdateBackgroundTaskSleepTime = Convert.ToInt32(keyvalueconfigdal.Get(c, "ConfigUpdateBackgroundTaskSleepTime").value);
                    OnLineTimeBackgroundTaskSleepTime = Convert.ToInt32(keyvalueconfigdal.Get(c, "OnLineTimeBackgroundTaskSleepTime").value);
                });
            }
            catch (Exception exp)
            {
                LogHelper.Error(string.Format("服务器[id:{0}] ", GlobalConfig.ServerIP.NullToEmpty()) + "LoadClusterConfig", exp);
            }
        }
    }
}
