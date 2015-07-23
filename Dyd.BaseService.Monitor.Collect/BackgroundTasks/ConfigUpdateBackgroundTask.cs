using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;

namespace Dyd.BaseService.Monitor.Collect.BackgroundTasks
{
    public class ConfigUpdateBackgroundTask:BaseBackgroundTask
    {

        public override void Start()
        {
            this.TimeSleep = GlobalConfig.ConfigUpdateBackgroundTaskSleepTime * 1000;
            base.Start();
        }

        protected override void Run()
        {
            GlobalConfig.LoadClusterConfig();
        }
    }
}
