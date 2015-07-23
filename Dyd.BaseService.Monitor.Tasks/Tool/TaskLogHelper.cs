using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.UnityLog.Dal;
using Dyd.BaseService.Monitor.Domain.UnityLog.Model;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Tasks.Tool
{
    public class TaskLogHelper
    {
        private static string UnityLogConnectString = "";

        private static void LoadConnectString()
        {
            if (string.IsNullOrWhiteSpace(UnityLogConnectString))
            {
                SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
                    {
                        tb_database_config_dal dal = new tb_database_config_dal();
                        var list = dal.GetModelList(c);
                        UnityLogConnectString = XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.GetDataBase(list, XXF.BaseService.Monitor.SystemRuntime.DataBaseType.UnityLog);
                    });
            }
        }
        public static void Error(string logtag, string msg, string projectname)
        {
            try
            {
                LoadConnectString();
                SqlHelper.ExcuteSql(UnityLogConnectString, (c) =>
                {
                    tb_error_log_dal dal = new tb_error_log_dal();
                    dal.AddError(c, new tb_error_log_model()
                    {
                        logcreatetime = DateTime.Now,
                        developer = "",
                        logtag = logtag,
                        logtype = (int)XXF.BaseService.Monitor.SystemRuntime.EnumErrorLogType.SystemError,
                        msg = msg,
                        projectname = projectname + "【监控平台】",
                        remark = "",
                        tracestack = ""
                    });
                });
            }
            catch (Exception exp)
            {
                XXF.Log.ErrorLog.Write("LogHelper.Error", exp);
            }
        }
    }
}
