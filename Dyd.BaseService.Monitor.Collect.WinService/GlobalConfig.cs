using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Dyd.BaseService.Monitor.Collect.WinService.Tool;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.WinService
{
    public class GlobalConfig
    {
        public static TaskProvider TaskProvider = new TaskProvider();
        public static string ServerIP { get; set; }

        public static int VersionNum = 0;
        public static DateTime ClusterLastUpdateTime=DateTime.Now;

        /// <summary>
        /// 任务dll根目录
        /// </summary>
        public static string TaskDllDir = "采集任务dll根目录";
        /// <summary>
        /// 任务dll本地版本缓存
        /// </summary>
        public static string TaskDllCompressFileCacheDir = "采集任务dll版本缓存";

        public static string MonitorWebUrl { get { return System.Configuration.ConfigurationSettings.AppSettings["MonitorWebUrl"]; } }

        public static void LoadConfig()
        {
            foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (!string.IsNullOrWhiteSpace(address.ToString()))
                {
                    SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
                    {
                        tb_cluster_dal clusterdal = new tb_cluster_dal();
                        var clustermodel = clusterdal.GetNewByServerip(c, address.ToString(), DateTime.Parse("1900-01-01"));
                        if (clustermodel != null)
                        {
                            ServerIP = address.ToString();
                            return;
                        }
                    });
                }
            }
            if (ServerIP == null)
                throw new Exception("当前服务器ip未在统一监控平台中配置");
        }
    }
}
