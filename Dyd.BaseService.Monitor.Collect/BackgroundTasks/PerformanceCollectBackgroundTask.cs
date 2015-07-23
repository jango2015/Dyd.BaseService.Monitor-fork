using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Collect.CollectTasks;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.BackgroundTasks
{
    public class PerformanceCollectBackgroundTask : BaseBackgroundTask
    {
        List<PerformanceCounterCollectTask> pcounts = new List<PerformanceCounterCollectTask>();
        public override void Start()
        {
            #region test
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("cpu", "Processor", "% Processor Time", "_Total")));
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("内存", "Memory", "Available MBytes", "")));
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("网络发送/s", "Network Interface", "Bytes Sent/sec", "本地连接* 14")));
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("网络下载/s", "Network Interface", "Bytes Received/sec", "本地连接* 14")));
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("物理磁盘读字节/s", "PhysicalDisk", "Disk Read Bytes/sec", "_Total")));
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("物理磁盘写字节/s", "PhysicalDisk", "Disk Write Bytes/sec", "_Total")));
            //pcounts.Add(new PerformanceCounterCollectTask(new CollectConfig("IIS请求/s", "Web Service", "Current Connections", "_Total")));
            #endregion

            try
            {
                this.TimeSleep = GlobalConfig.PerformanceCollectBackgroundTaskSleepTime * 1000;
                if (!string.IsNullOrWhiteSpace(GlobalConfig.ClusterModel.performancecollectconfigjson))
                {
                    var collectconfigs = new XXF.Serialization.JsonHelper().Deserialize<List<Core.CollectConfig>>(GlobalConfig.ClusterModel.performancecollectconfigjson);
                    foreach (var config in collectconfigs)
                    {
                        pcounts.Add(new PerformanceCounterCollectTask(config));
                    }
                }
                base.Start();
            }
            catch (Exception exp)
            {
                LogHelper.Error("PerformanceCollectBackgroundTask 初始化错误", exp);
            }
        }
        protected override void Run()
        {
            if (GlobalConfig.ClusterModel.ifmonitor == false)
                return;
            tb_performance_collect_model model = new tb_performance_collect_model();
            foreach (var p in pcounts)
            {
                var c = p.Collect();
                //Console.WriteLine(p.Name + ":" + c);
                if (p.Name.Contains("cpu"))
                {
                    model.cpu = (decimal)c;
                }
                else if (p.Name.Contains("内存"))
                {
                    model.memory = (decimal)c;
                }
                else if (p.Name.Contains("网络发送"))
                {
                    model.networkupload = (decimal)c;
                }
                else if (p.Name.Contains("网络下载"))
                {
                    model.networkdownload = (decimal)c;
                }
                else if (p.Name.Contains("物理磁盘读"))
                {
                    model.ioread = (decimal)c;
                }
                else if (p.Name.Contains("物理磁盘写"))
                {
                    model.iowrite = (decimal)c;
                }
                else if (p.Name.Contains("IIS请求"))
                {
                    model.iisrequest = (decimal)c;
                }
            }
            model.serverid = GlobalConfig.ClusterModel.id;
            model.createtime = DateTime.Now;
            SqlHelper.ExcuteSql(DbShardingHelper.GetDataBase(GlobalConfig.DataBaseConfigModels, DataBaseType.Cluster), (c) =>
            {
                model.createtime = c.GetServerDate();
                tb_performance_collect_dal performancecollectdal = new tb_performance_collect_dal();
                performancecollectdal.AddCollect(c,model);
            });
        }
    }
}
