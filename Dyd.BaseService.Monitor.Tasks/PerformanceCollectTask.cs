using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.Monitor.Tasks
{
    public class PerformanceCollectTask : XXF.BaseService.TaskManager.BaseDllTask
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
                base.OpenOperator.Error("性能监控定时统计失败", exception);
            }
        }

        public void InitConfig()
        {
            XXF.Common.XXFConfig.MonitorPlatformConnectionString = base.AppConfig["MonitorPlatformConnectionString"];
        }

        private void Do()
        {
            PerformanceCollectDal Dal = new PerformanceCollectDal();
            bool c = Dal.StatisServerMinitor();
            if (c)
            {
                base.OpenOperator.Log("性能监控定时统计成功");
            }
        }

        public override void TestRun()
        {
            /*测试环境下任务的配置信息需要手工填写,正式环境下需要配置在任务配置中心里面*/
            base.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            base.AppConfig.Add("MonitorPlatformConnectionString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");
            base.TestRun();
        }
    }
}
