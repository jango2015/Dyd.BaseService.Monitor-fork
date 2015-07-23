using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXF.BaseService.Monitor.Dal;
using XXF.ProjectTool;

namespace XXF.BaseService.Monitor.SystemRuntime
{
    public class Config
    {
        static Config()
        {
            try
            {
                SqlHelper.ExcuteSql(XXF.Common.XXFConfig.MonitorPlatformConnectionString, (c) => {
                    tb_database_config_dal configdal = new tb_database_config_dal();
                    UnityLogConnectString = DbShardingHelper.GetDataBase(configdal.GetModelList(c), DataBaseType.UnityLog);
                });
            }
            catch 
            {
                
            }
        }
        public static string UnityLogConnectString = "";

        public static int TimeWatchLogBatchCommitCount = Convert.ToInt32(XXF.Common.XXFConfig.Get("TimeWatchLogBatchCommitCount", "10000"));
    }
}
