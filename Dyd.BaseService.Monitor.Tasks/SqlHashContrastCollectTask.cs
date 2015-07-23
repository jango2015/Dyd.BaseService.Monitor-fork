using Dyd.BaseService.Monitor.Domain.TimeWatch.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Tasks
{
    public class SqlHashContrastCollectTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        public override void Run()
        {
            try
            {
                InitConfig();
                Do();
            }
            catch (Exception exception)
            {
                base.OpenOperator.Error("性能监控定时统计SQL性能失败", exception);
            }
        }

        public void InitConfig()
        {
            XXF.Common.XXFConfig.MonitorPlatformConnectionString = base.AppConfig["MonitorPlatformConnectionString"];
        }

        private void Do()
        {
            tb_sqlhash_consult_dal Dal = new tb_sqlhash_consult_dal();
            int SqlJudgeBase = Convert.ToInt32(base.AppConfig["SqlJudgeBase"]);
            bool c = Dal.SqlHashContrast(SqlJudgeBase);
            if (c)
            {
                base.OpenOperator.Log("性能监控定时统计SQL性能成功");
            }
        }

        public override void TestRun()
        {
            /*测试环境下任务的配置信息需要手工填写,正式环境下需要配置在任务配置中心里面*/
            base.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            base.AppConfig.Add("SqlJudgeBase", "5");
            base.AppConfig.Add("MonitorPlatformConnectionString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");
            base.TestRun();
        }
    }
}
