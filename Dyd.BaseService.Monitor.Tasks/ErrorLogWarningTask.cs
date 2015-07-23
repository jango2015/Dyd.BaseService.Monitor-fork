using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.UnityLog.Dal;
using Dyd.BaseService.Monitor.Tasks.Tool;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Tasks
{
    /// <summary>
    /// 错误预警任务
    /// </summary>
    public class ErrorLogWarningTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        public override void TestRun()
        {
            this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            this.AppConfig.Add("MonitorPlatformManageConnectString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");
            this.AppConfig.Add("EveryGrowErrorNum", "20");

            string json = new XXF.Serialization.JsonHelper().Serializer(this.AppConfig);

            base.TestRun();
        }

        private string UnityLogConnectString = "";
        private DateTime lastscantime = DateTime.Now;
        public override void Run()
        {
            GlobalConfig.MonitorPlatformManageConnectString = AppConfig["MonitorPlatformManageConnectString"];
            int EveryGrowErrorNum = Convert.ToInt32( AppConfig["EveryGrowErrorNum"]);

            SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
            {
                tb_database_config_dal dal = new tb_database_config_dal();
                var list = dal.GetModelList(c);
                UnityLogConnectString = XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.GetDataBase(list, XXF.BaseService.Monitor.SystemRuntime.DataBaseType.UnityLog);
            });

            int errornum = 0;
            SqlHelper.ExcuteSql(UnityLogConnectString, (c) =>
            {
                tb_error_log_dal dal = new tb_error_log_dal();
                errornum = dal.GetGrawErrorNum(c, lastscantime);
            });
            if (errornum > EveryGrowErrorNum)
            {
                TaskLogHelper.Error("错误预警任务", string.Format("【{0}】-【{1}】期间平台错误日志增加{2}条",lastscantime,DateTime.Now,errornum), "错误预警任务");
            }
            lastscantime = DateTime.Now;
        }
    }
}
