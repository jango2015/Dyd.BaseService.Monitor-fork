using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Dal;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using Dyd.BaseService.Monitor.Domain.UnityLog.Dal;
using Dyd.BaseService.Monitor.Domain.UnityLog.Model;
using Dyd.BaseService.Monitor.Tasks.Tool;
using XXF.BaseService.Monitor.Model;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Tasks
{
    /// <summary>
    /// 耗时性能预警任务
    /// </summary>
    public class TimeWatchPerformanceWarningTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        public override void TestRun()
        {
            this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            this.AppConfig.Add("MonitorPlatformManageConnectString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");

            base.TestRun();
        }
        public override void Run()
        {
            GlobalConfig.MonitorPlatformManageConnectString = AppConfig["MonitorPlatformManageConnectString"];
            SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
           {
               tb_database_config_dal dal = new tb_database_config_dal();
               var list = dal.GetModelList(c);
               list.ForEach(o =>
               {
                   if (o.dbtype == (byte)XXF.BaseService.Monitor.SystemRuntime.DataBaseType.Timewatch)
                   {
                       var connectstring = XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.GetDataBase(o);
                       CheckPerformanceOfApi(connectstring,o);
                       CheckPerformanceOfSql(connectstring,o);
                   }
               });
           });

        }

        /// <summary>
        /// 检查api的接口性能
        /// </summary>
        private void CheckPerformanceOfApi(string connectstring,Domain.PlatformManage.Model.tb_database_config_model configmodel)
        {
            try
            {
                DateTime dtime = DateTime.Now;
                string msg = "";
                Dictionary<string, tb_timewatchlog_api_dayreport_model> dic = new Dictionary<string, tb_timewatchlog_api_dayreport_model>();
                List<string> urls = new List<string>();
                SqlHelper.ExcuteSql(connectstring, (c) =>
                {
                    tb_timewatchlog_api_dayreport_dal dal = new tb_timewatchlog_api_dayreport_dal();
                    var models = dal.GetDayReport(c, dtime.Date.AddDays(-1));
                    models.ForEach(o =>
                    {
                        string key = o.date.Date.ToString("yyyyMMdd") + o.url.ToLower();
                        if (!dic.Keys.Contains(key))
                            dic.Add(key, o);
                        if (!urls.Contains(o.url.ToLower()))
                        {
                            urls.Add(o.url.ToLower());
                        }
                    });
                });
                foreach (var u in urls)
                {
                    string keynow = dtime.Date.ToString("yyyyMMdd") + u.ToLower();
                    string keyyestoday = dtime.Date.AddDays(-1).ToString("yyyyMMdd") + u.ToLower();
                    if (dic.ContainsKey(keynow) && dic.ContainsKey(keyyestoday))
                    {
                        var per = (dic[keyyestoday].avgtime == 0 ? 0 : ((dic[keynow].avgtime - dic[keyyestoday].avgtime) / dic[keyyestoday].avgtime));
                        if (per > 0.2)
                        {
                            msg += string.Format("【{1}】上升{0}%({2}s,{3}s)\r\n", (per * 100).ToString("f2"), u, dic[keynow].avgtime.ToString("f2"), dic[keyyestoday].avgtime.ToString("f2"));
                        }
                    }
                }
                if(msg !="")
                    TaskLogHelper.Error("检查api的接口性能", "【" + configmodel.dblocalname + "】Api接口性能检查(与昨日对比平均耗时)" + "\r\n" + msg, "耗时性能预警任务");
            }
            catch (Exception exp)
            {
                this.OpenOperator.Error("【耗时性能预警任务】检查api的接口性能", exp);
            }
        }
        /// <summary>
        /// 检查sql的性能
        /// </summary>
        private void CheckPerformanceOfSql(string connectstring, Domain.PlatformManage.Model.tb_database_config_model configmodel)
        {
            try
            {
                DateTime dtime = DateTime.Now;
                string msg = "";
                Dictionary<string, tb_timewatchlog_sql_dayreport_model> dic = new Dictionary<string, tb_timewatchlog_sql_dayreport_model>();
                List<string> hashs = new List<string>();
                SqlHelper.ExcuteSql(connectstring, (c) =>
                {
                    tb_timewatchlog_sql_dayreport_dal dal = new tb_timewatchlog_sql_dayreport_dal();
                    var models = dal.GetDayReport(c, dtime.Date.AddDays(-1));
                    models.ForEach(o =>
                    {
                        string key = o.date.Date.ToString("yyyyMMdd") + o.sqlhash.ToLower();
                        if (!dic.Keys.Contains(key))
                            dic.Add(key, o);
                        if (!hashs.Contains(o.sqlhash.ToLower()))
                        {
                            hashs.Add(o.sqlhash.ToLower());
                        }
                    });
                });
                foreach (var u in hashs)
                {
                    string keynow = dtime.Date.ToString("yyyyMMdd") + u.ToLower();
                    string keyyestoday = dtime.Date.AddDays(-1).ToString("yyyyMMdd") + u.ToLower();
                    if (dic.ContainsKey(keynow) && dic.ContainsKey(keyyestoday))
                    {
                        var per = (dic[keyyestoday].avgtime == 0 ? 0 : ((dic[keynow].avgtime - dic[keyyestoday].avgtime) / dic[keyyestoday].avgtime));
                        if (per > 0.2)
                        {
                            msg += string.Format("【sqlhash:{1}】上升{0}%({2}s,{3}s)\r\n", (per * 100).ToString("f2"), u, dic[keynow].avgtime.ToString("f2"), dic[keyyestoday].avgtime.ToString("f2"));
                        }
                    }
                }
                if (msg != "")
                    TaskLogHelper.Error("检查sql的性能", "【" + configmodel.dblocalname + "】Sql性能检查(与昨日对比平均耗时)" + "\r\n" + msg, "耗时性能预警任务");
            }
            catch (Exception exp)
            {
                this.OpenOperator.Error("【耗时性能预警任务】检查sql的性能", exp);
            }
        }
    }
}
