using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Collect.WinService.Tool;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.WinService
{
    public class CollectVersionUpdateBackgroundTask : BaseBackgroundTask
    {
       
        public override void Start()
        {
            this.TimeSleep = 5000;
            base.Start();
        }
        protected override void Run()
        {
            int maxversion = -1; DateTime sqltime = DateTime.Now;
            SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
            {
                tb_cluster_collect_version_dal versiondal = new tb_cluster_collect_version_dal();
                maxversion = versiondal.GetMaxVersionNumber(c);
                sqltime = c.GetServerDate();
            });
            if (maxversion > GlobalConfig.VersionNum)
            {
                GlobalConfig.LoadConfig();
                if (GlobalConfig.TaskProvider.IsStart())
                    GlobalConfig.TaskProvider.Stop();
                GlobalConfig.TaskProvider.Start();
                GlobalConfig.VersionNum = maxversion;
                GlobalConfig.ClusterLastUpdateTime = sqltime;
                Core.LogHelper.Log(string.Format("服务器ip【{0}】采集任务dll新版本【" + maxversion+"】更新完毕",GlobalConfig.ServerIP));
            }
           
        }
    }
}
