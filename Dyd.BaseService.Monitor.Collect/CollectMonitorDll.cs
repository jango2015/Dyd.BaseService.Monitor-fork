using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Collect.BackgroundTasks;
using XXF.BaseService.Monitor;
using Dyd.BaseService.Monitor.Core;
using XXF.BaseService.Monitor.SystemRuntime;

namespace Dyd.BaseService.Monitor.Collect
{
    public class CollectMonitorDll:BaseCollectMonitorDll
    {
        public List<BaseBackgroundTask> BackgroundTasks = new List<BaseBackgroundTask>();

        public override void Start()
        {
            CoreGlobalConfig.PlatformManageConnectString = this.PlatformManageConnectString;
            if (string.IsNullOrWhiteSpace(GlobalConfig.ServerIP))
                GlobalConfig.ServerIP = this.ServerIP;
            GlobalConfig.LoadBaseConfig();
            GlobalConfig.LoadClusterConfig();
            if (BackgroundTasks.Count == 0)
            { 
                BackgroundTasks.Add(new MonitorCollectBackgroundTask());
                BackgroundTasks.Add(new PerformanceCollectBackgroundTask());
                //BackgroundTasks.Add(new ConfigUpdateBackgroundTask());
                BackgroundTasks.Add(new OnLineTimeBackgroundTask());
                foreach (var t in BackgroundTasks)
                {
                    t.Start();
                }
            }
        }

    }
}
