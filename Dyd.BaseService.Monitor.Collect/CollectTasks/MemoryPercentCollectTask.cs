using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Collect.CollectTasks
{
    public class MemoryPercentCollectTask : BaseCollectTask, ICollect<MemoryInfo>
    {
        ComputerInfo cinf;
        public MemoryPercentCollectTask()
        {
            this.Name = "内存使用信息";
            this.cinf = new ComputerInfo();
        }
        public MemoryInfo Collect()
        {
            if (IsCollect == false)
                return null;
            try
            {
                var usedMem = this.cinf.TotalPhysicalMemory - this.cinf.AvailablePhysicalMemory;//总内存减去可用内存
                var per = Math.Round((double)(usedMem / Convert.ToDecimal(this.cinf.TotalPhysicalMemory) * 100),
                         2,
                         MidpointRounding.AwayFromZero);
                return new MemoryInfo { TotalPhysicalMemory=this.cinf.TotalPhysicalMemory, AvailablePhysicalMemory=this.cinf.AvailablePhysicalMemory, UserdPercent=per };
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("服务器[id:{0}] ", GlobalConfig.ServerIP.NullToEmpty()) + this.Name + "采集出错");
            }
        }
    }

    public class MemoryInfo
    {
        public ulong TotalPhysicalMemory { get; set; }
        public ulong AvailablePhysicalMemory { get; set; }
        public double UserdPercent { get; set; }
    }
}
