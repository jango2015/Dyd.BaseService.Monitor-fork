using Dyd.BaseService.Monitor.Domain.CreateTable.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.Monitor.Tasks
{
    public class CreateTask : XXF.BaseService.TaskManager.BaseDllTask
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
                base.OpenOperator.Error("定时创表执行失败", exception);
            }
        }

        public void InitConfig()
        {
            XXF.Common.XXFConfig.MonitorPlatformConnectionString = base.AppConfig["MonitorPlatformConnectionString"];
        }

        private void Do()
        {
            Dictionary<string, Exception> errordic = new Dictionary<string, Exception>();
            //string sqltxturl = base.AppConfig["TableCreateSqlTxtUrl"].ToString();
            //string type = base.AppConfig["TableCreateType"].ToString();
            //string key = base.AppConfig["TableCreateConfigKey"].ToString();
            bool c = new CreateDal().CreateTable(out errordic);
            if (c)
            {
                base.OpenOperator.Log("定时创表执行成功");
            }
            else
            {
                base.OpenOperator.Log("定时创表执行失败");
            }
            if (errordic.Count > 0)
            { 
                foreach(KeyValuePair<string,Exception> k in errordic)
                {
                    base.OpenOperator.Error(k.Key, k.Value);
                }
            }
        }

        public override void TestRun()
        {
            /*测试环境下任务的配置信息需要手工填写,正式环境下需要配置在任务配置中心里面*/
            base.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            //base.AppConfig.Add("TableCreateSqlTxtUrl", "E:\\CreateSQL\\Day");
            //base.AppConfig.Add("TableCreateType", "Month");
            //base.AppConfig.Add("TableCreateConfigKey", "CreateMonthTable");
            base.AppConfig.Add("MonitorPlatformConnectionString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");
            base.TestRun();
        }
    }
}
