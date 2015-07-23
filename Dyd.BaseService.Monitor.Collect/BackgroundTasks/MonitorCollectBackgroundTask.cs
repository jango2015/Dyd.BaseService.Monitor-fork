using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Collect.CollectTasks;
using Dyd.BaseService.Monitor.Collect.Tool;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.ProjectTool;
using XXF.Serialization;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Collect.BackgroundTasks
{
    public class MonitorCollectBackgroundTask : BaseBackgroundTask
    {
        List<PerformanceCounterCollectTask> pcounts = new List<PerformanceCounterCollectTask>();
        DiskInfoCollectTask dct = null;
        MemoryPercentCollectTask mct = null;
        List<BaseCollectTask> customtasks = new List<BaseCollectTask>();

        public override void Start()
        {
            try
            {
                this.TimeSleep = GlobalConfig.MonitorCollectBackgroundTaskSleepTime * 1000;
                dct = new DiskInfoCollectTask();
                mct = new MemoryPercentCollectTask();
                customtasks.Add(dct); customtasks.Add(mct);
                if (!string.IsNullOrWhiteSpace(GlobalConfig.ClusterModel.monitorcollectconfigjson))
                {
                    var collectconfigs = new XXF.Serialization.JsonHelper().Deserialize<List<Core.CollectConfig>>(GlobalConfig.ClusterModel.monitorcollectconfigjson);
                    foreach (var config in collectconfigs)
                    {
                        if (config.CollectType == CollectType.System)
                            pcounts.Add(new PerformanceCounterCollectTask(config));
                        else
                        {
                            foreach (var ct in customtasks)
                            {
                                if (config.CollectName == ct.Name)
                                {
                                    ct.IsCollect = true;
                                }
                            }
                        }
                    }
                }
                base.Start();
            }
            catch (Exception exp)
            {
                LogHelper.Error(string.Format("服务器[id:{0}] MonitorCollectBackgroundTask 初始化错误",GlobalConfig.ServerIP.NullToEmpty()), exp);
            }
        }
        protected override void Run()
        {
            if (GlobalConfig.ClusterModel.ifmonitor == false)
                return;
            List<ClusterMonitorInfo> cmis = new List<ClusterMonitorInfo>();
            foreach (var p in pcounts)
            {
                var c = p.Collect();
                cmis.Add(new ClusterMonitorInfo(p.Name, c.ToString("F2"), (double)c));
            }

            if (dct.IsCollect == true)
            {
                string msg = ""; double minfreespace = 1000000;
                foreach (var d in dct.Collect())
                {
                    minfreespace = Math.Min(minfreespace, d.FreeSpace);
                    msg += string.Format("[{0}]:{1}(G)/{2}(G)\r\n", d.Name, d.FreeSpace.ToString("F2"),d.TotalSpace.ToString("F2"));
                }
                cmis.Add(new ClusterMonitorInfo(dct.Name, msg, minfreespace));
            }

            if (mct.IsCollect == true)
            {
                var mper = mct.Collect();
                cmis.Add(new ClusterMonitorInfo(mct.Name, string.Format("剩余/总量:{0}(M)/{1}(M) 已用:{2}%",ConvertHelper.ByteToM(mper.AvailablePhysicalMemory).ToString("F2"),ConvertHelper.ByteToM(mper.TotalPhysicalMemory).ToString("F2"), mper.UserdPercent), mper.UserdPercent));
            }

            SqlHelper.ExcuteSql(DbShardingHelper.GetDataBase(GlobalConfig.DataBaseConfigModels.Select(c1 => (dynamic)c1).ToList(), DataBaseType.Cluster), (c) =>
            {

                tb_cluster_monitorinfo_dal dal = new tb_cluster_monitorinfo_dal();
                dal.AddOrUpdate(c, new tb_cluster_monitorinfo_model() { lastupdatetime = c.GetServerDate(), serverid = GlobalConfig.ClusterModel.id, monitorinfojson = new JsonHelper().Serializer(cmis) });
            });
        }
    }
}
