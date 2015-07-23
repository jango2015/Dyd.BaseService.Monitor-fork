using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Core;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Collect.BackgroundTasks
{
    public class OnLineTimeBackgroundTask : BaseBackgroundTask
    {

        public override void Start()
        {
            this.TimeSleep = GlobalConfig.OnLineTimeBackgroundTaskSleepTime * 1000;
            base.Start();
        }

        protected override void Run()
        {
            SqlHelper.ExcuteSql(DbShardingHelper.GetDataBase(GlobalConfig.DataBaseConfigModels, DataBaseType.PlatformManage), (c) =>
            {
                tb_cluster_dal dal = new tb_cluster_dal();
                dal.UpdateOnLineTime(c,GlobalConfig.ServerIP);
            });
        }
    }
}
