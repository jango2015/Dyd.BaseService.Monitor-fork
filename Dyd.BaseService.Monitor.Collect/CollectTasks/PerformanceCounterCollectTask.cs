using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Collect.CollectTasks
{
    public class PerformanceCounterCollectTask :BaseCollectTask, ICollect<float>
    {
        private PerformanceCounter counter = null;
        public PerformanceCounterCollectTask(CollectConfig config)
        {
            this.Name = config.CollectName;
            counter = new PerformanceCounter();
            counter.CategoryName = config.CategoryName;
            counter.CounterName = config.CounterName;
            counter.InstanceName = config.InstanceName;
            counter.MachineName = ".";
            try
            {
                Collect();//开启采集
                System.Threading.Thread.Sleep(100);
            }
            catch { }
        }

        public float Collect()
        {
            try
            {
                return counter.NextValue();
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("服务器[id:{3}] {0}-{1}-{2} 性能计数器出错:",counter.CategoryName,counter.CounterName,counter.InstanceName,GlobalConfig.ServerIP.NullToEmpty()));
            }
        }
    }

   
}
