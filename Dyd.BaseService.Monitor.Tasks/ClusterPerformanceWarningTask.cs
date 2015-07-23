using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using Dyd.BaseService.Monitor.Tasks.Tool;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Tasks
{
    /// <summary>
    /// 集群性能预警任务
    /// </summary>
    public class ClusterPerformanceWarningTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        public override void TestRun()
        {
            this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            this.AppConfig.Add("MonitorPlatformManageConnectString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");

            base.TestRun();
        }

        private string MonitorClusterConnectString = "";
        private DateTime lastclusterruntime = DateTime.Now.AddDays(-1);

        public override void Run()
        {
            GlobalConfig.MonitorPlatformManageConnectString = AppConfig["MonitorPlatformManageConnectString"];
            SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
            {
                tb_database_config_dal dal = new tb_database_config_dal();
                var list = dal.GetModelList(c);
                MonitorClusterConnectString = XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.GetDataBase(list, XXF.BaseService.Monitor.SystemRuntime.DataBaseType.Cluster);
            });
            CheckMonitorInfoWarning();
            if (lastclusterruntime.Date!=DateTime.Now.Date&&DateTime.Now.Hour>18)//每天18点后运行，每天运行一次足矣。
            { 
                CheckPerformanceOfCluster();
                lastclusterruntime = DateTime.Now;
            }
        }

        /// <summary>
        /// 检查监控信息的预警
        /// </summary>
        private void CheckMonitorInfoWarning()
        {
            try
            {

                string waringmsg = "";
                List<tb_cluster_model> clusters = new List<tb_cluster_model>();
                SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
                {
                    tb_cluster_dal dal = new tb_cluster_dal();
                    clusters = dal.GetAllList(c);

                });
                clusters.ForEach(o =>
                {
                    if (o.ifmonitor == true)
                    {
                        tb_cluster_monitorinfo_model monitorinfo = null;
                        SqlHelper.ExcuteSql(MonitorClusterConnectString, (c) =>
                        {
                            tb_cluster_monitorinfo_dal dal = new tb_cluster_monitorinfo_dal();
                            monitorinfo = dal.GetByServerId(c, o.id);

                        });
                        if (monitorinfo != null)
                        {
                            var collectconfigs = new XXF.Serialization.JsonHelper().Deserialize<List<CollectConfig>>(o.monitorcollectconfigjson);
                            foreach (var config in collectconfigs)
                            {
                                waringmsg += GetWarnningInfo(config, monitorinfo,o);
                            }
                        }
                    }
                });

                if (waringmsg != "")
                {
                    TaskLogHelper.Error("检查监控信息的预警", "服务器性能检查(预警值检查)"+"\r\n"+waringmsg, "集群性能预警任务");
                }
            }
            catch (Exception exp)
            {
                this.OpenOperator.Error("【集群性能预警任务】检查监控信息的预警", exp);
            }
        }

        private string GetWarnningInfo(CollectConfig config, tb_cluster_monitorinfo_model model,tb_cluster_model cluster)
        {
            string msg = "";

            var monitorinfos = new XXF.Serialization.JsonHelper().Deserialize<List<ClusterMonitorInfo>>(model.monitorinfojson);
            foreach (var info in monitorinfos)
            {
                if (info.Name == config.CollectName)
                {
                    if (config.EqualWarningValue != null && config.EqualWarningValue.IsWarning == true && config.EqualWarningValue.Value == info.MonitorValue)
                        msg += string.Format("【id:{2},{4}】检测项【{0}】等于预警值{3},当前值为{1}\r\n", config.CollectName, info.MonitorValue, model.serverid, config.EqualWarningValue.Value, cluster.servername);
                    if (config.LessThanWarningValue != null && config.LessThanWarningValue.IsWarning == true && config.LessThanWarningValue.Value > info.MonitorValue)
                        msg += string.Format("【id:{2},{4}】检测项【{0}】小于预警值{3},当前值为{1}\r\n", config.CollectName, info.MonitorValue, model.serverid, config.LessThanWarningValue.Value, cluster.servername);
                    if (config.MoreThanWarningValue != null && config.MoreThanWarningValue.IsWarning == true && config.MoreThanWarningValue.Value < info.MonitorValue)
                        msg += string.Format("【id:{2},{4}】检测项【{0}】大于预警值{3},当前值为{1}\r\n", config.CollectName, info.MonitorValue, model.serverid, config.MoreThanWarningValue.Value, cluster.servername);
                }
            }
            return msg;
        }

        private double PerformanceComparisonPer(double nowvalue, double yestodayvalue)
        {
            return (yestodayvalue == 0 ? 0 : (nowvalue - yestodayvalue) / yestodayvalue);
        }

        /// <summary>
        /// 检查Cluster的接口性能
        /// </summary>
        private void CheckPerformanceOfCluster()
        {
            try
            {
                Dictionary<int,tb_cluster_model> clusters = new Dictionary<int,tb_cluster_model>();
                SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
                {
                    tb_cluster_dal dal = new tb_cluster_dal();
                    var cs = dal.GetAllList(c);
                    foreach(var o in cs)
                    {
                        clusters.Add(o.id,o);
                    }
                });

                DateTime dtime = DateTime.Now;
                string msg = "";
                Dictionary<string, tb_performance_dayreport_model> dic = new Dictionary<string, tb_performance_dayreport_model>();
                List<int> serverids = new List<int>();
                SqlHelper.ExcuteSql(MonitorClusterConnectString, (c) =>
                {
                    tb_performance_dayreport_dal dal = new tb_performance_dayreport_dal();
                    var models = dal.GetDayReport(c, dtime.Date.AddDays(-1));
                    models.ForEach(o =>
                    {
                        string key = o.date.Date.ToString("yyyyMMdd") + o.serverid;
                        if (!dic.Keys.Contains(key))
                            dic.Add(key, o);
                        if (!serverids.Contains(o.serverid))
                        {
                            serverids.Add(o.serverid);
                        }
                    });
                });
                foreach (var u in serverids)
                {
                    if (!clusters.ContainsKey(u)||!clusters[u].ifmonitor)
                        continue;
                    string keynow = dtime.Date.ToString("yyyyMMdd") + u;
                    string keyyestoday = dtime.Date.AddDays(-1).ToString("yyyyMMdd") + u;
                    if (dic.ContainsKey(keynow) && dic.ContainsKey(keyyestoday))
                    {
                        #region 性能对比

                        var per = PerformanceComparisonPer((double)dic[keynow].avgcpu, (double)dic[keyyestoday].avgcpu);
                        if (per > 0.2)
                        {
                            msg += string.Format("【id:{1},{2}】【avgcpu】上升{0}%", (per * 100).ToString("f2"), u, clusters[u].servername) + "\r\n";
                        }

                        per = PerformanceComparisonPer((double)dic[keynow].avgioread, (double)dic[keyyestoday].avgioread);
                        if (per > 0.2)
                        {
                            msg += string.Format("【id:{1},{2}】【avgioread】上升{0}%", (per * 100).ToString("f2"), u, clusters[u].servername) + "\r\n";
                        }

                        per = PerformanceComparisonPer((double)dic[keynow].avgiowrite, (double)dic[keyyestoday].avgiowrite);
                        if (per > 0.2)
                        {
                            msg += string.Format("【id:{1},{2}】【avgiowrite】上升{0}%", (per * 100).ToString("f2"), u, clusters[u].servername) + "\r\n";
                        }

                        per = PerformanceComparisonPer((double)dic[keyyestoday].avgmemory, (double)dic[keynow].avgmemory);//这里的内存是可用内存 所以是昨天的与今天对比
                        if (per > 0.2)
                        {
                            msg += string.Format("【id:{1},{2}】【avgmemory】(可用内存)下降{0}%", (per * 100).ToString("f2"), u, clusters[u].servername) + "\r\n";
                        }

                        per = PerformanceComparisonPer((double)dic[keynow].avgnetworkdownload, (double)dic[keyyestoday].avgnetworkdownload);
                        if (per > 0.2)
                        {
                            msg += string.Format("【id:{1},{2}】【avgnetworkdownload】上升{0}%", (per * 100).ToString("f2"), u, clusters[u].servername) + "\r\n";
                        }

                        per = PerformanceComparisonPer((double)dic[keynow].avgnetworkupload, (double)dic[keyyestoday].avgnetworkupload);
                        if (per > 0.2)
                        {
                            msg += string.Format("【id:{1},{2}】【avgnetworkupload】上升{0}%", (per * 100).ToString("f2"), u, clusters[u].servername) + "\r\n";
                        }
                        #endregion
                    }
                }
                if (msg != "")
                    TaskLogHelper.Error("检查Cluster性能", "服务器性能检查(性能与昨日对比性能)"+"\r\n" + msg, "集群性能预警任务");
            }
            catch (Exception exp)
            {
                this.OpenOperator.Error("【集群性能预警任务】检查Cluster性能", exp);
            }
        }
    }
}
