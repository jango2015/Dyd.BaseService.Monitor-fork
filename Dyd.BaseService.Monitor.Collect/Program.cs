using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dyd.BaseService.Monitor.Collect.CollectTasks;

namespace Dyd.BaseService.Monitor.Collect
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //List<PerformanceCounterCollectTask> pcounts = new List<PerformanceCounterCollectTask>();
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("cpu", "Processor", "% Processor Time", "_Total")));
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("内存", "Memory", "Available MBytes", "")));
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("网络发送/s", "Network Interface", "Bytes Sent/sec", "本地连接* 14")));
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("网络下载/s", "Network Interface", "Bytes Received/sec", "本地连接* 14")));
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("物理磁盘读字节/s", "PhysicalDisk", "Disk Read Bytes/sec", "_Total")));
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("物理磁盘写字节/s", "PhysicalDisk", "Disk Write Bytes/sec", "_Total")));
            //pcounts.Add(new PerformanceCounterCollectTask(new PerformanceCounterConfig("IIS请求/s", "Web Service", "Current Connections", "_Total")));

            //DiskInfoCollectTask dct = new DiskInfoCollectTask();
            //MemoryPercentCollectTask mct = new MemoryPercentCollectTask();
            //while (true)
            //{
            //    //foreach (var p in pcounts)
            //    //{
            //    //    var c = p.Collect();
            //    //    Console.WriteLine(p.Name + ":" + c);
            //    //}
            //    foreach (var d in dct.Collect())
            //    {
            //        Console.WriteLine(d.Name + string.Format(":[空闲{0}][总的{1}]",d.FreeSpace,d.TotalSpace));
            //    }
            //    Console.WriteLine(mct.Name + string.Format(":{0}%", mct.Collect().UserdPercent));
            //    System.Threading.Thread.Sleep(2000);
            //}

            CollectMonitorDll cmd = new CollectMonitorDll();
            cmd.PlatformManageConnectString="server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;";
            GlobalConfig.ServerIP = "192.168.17.54";
            cmd.Start();
            while (true)
            {
             
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
