using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Dyd.BaseService.Monitor.Collect.WinService.BackgroundTasks;
using Dyd.BaseService.Monitor.Core;
using XXF.Api;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.WinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);
            //string url = GlobalConfig.MonitorWebUrl.TrimEnd('/') + "/OpenApi/GetConfig/";
            //ClientResult r = ApiHelper.Get(url, new
            //{

            //});
            //if (r.success == false)
            //{
            //    string msg = "请求" + url + "失败,请检查配置中“统一监控平台站点url”配置项";
            //    Core.LogHelper.Error(msg, null);
            //    throw new Exception(msg);
            //}

            //var appconfiginfo = ApiHelper.Data(r);
            //string connectstring = appconfiginfo.PlatformManageConnectString;
            //Core.CoreGlobalConfig.PlatformManageConnectString = StringDESHelper.DecryptDES(connectstring, "dyd88888888");

            //Core.CoreGlobalConfig.PlatformManageConnectString = "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;";
            //var task = new CollectVersionUpdateBackgroundTask();
            //task.Start();
            //System.Threading.Thread.Sleep(1000);
            //var task2 = new CollectUpdateBackgroundTask();
            //task2.Start();
            //while (true)
            //{

            //    System.Threading.Thread.Sleep(2000);
            //}
        }
    }
}
