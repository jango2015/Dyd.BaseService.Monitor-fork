using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Collect.WinService.Tool;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.WinService.BackgroundTasks
{
    public class CollectUpdateBackgroundTask: BaseBackgroundTask
    {
        public override void Start()
        {
            this.TimeSleep = 5000;
            base.Start();
        }
        protected override void Run()
        {
            DateTime time = DateTime.Parse("1900-01-01");
            GlobalConfig.LoadConfig();
            SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
            {
                tb_cluster_dal dal = new tb_cluster_dal();
                time = dal.GetLastUpdateTime(c,GlobalConfig.ServerIP);
            });
            if (time > GlobalConfig.ClusterLastUpdateTime)
            {
               
                if (GlobalConfig.TaskProvider.IsStart())
                    GlobalConfig.TaskProvider.Stop();
                GlobalConfig.TaskProvider.Start();
                GlobalConfig.ClusterLastUpdateTime = time;
                Core.LogHelper.Log(string.Format("服务器ip【{0}】检测到服务器配置更新,时间【{1}】,服务dll重启更新完毕",GlobalConfig.ServerIP,time));
            }
           
        }
    
    }
}
