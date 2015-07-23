using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XXF.BaseService.Monitor.Dal;
using XXF.BaseService.Monitor.Model;
using XXF.Log;
using XXF.ProjectTool;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Core
{
    public class LogHelper
    {
        private static string UnityLogConnectString = "";

        private static void LoadConnectString()
        {
            if (string.IsNullOrWhiteSpace(UnityLogConnectString))
            {
                SqlHelper.ExcuteSql(CoreGlobalConfig.PlatformManageConnectString, (c) =>
                {
                    tb_database_config_dal dal = new tb_database_config_dal();
                    var list = dal.GetModelList(c);
                    UnityLogConnectString = XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.GetDataBase(list, XXF.BaseService.Monitor.SystemRuntime.DataBaseType.UnityLog);
                });
            }
        }

        public static void Error(string msg,Exception exp)
        {
            try
            {
                LoadConnectString();
                if (exp == null)
                    exp = new Exception();
                SqlHelper.ExcuteSql(UnityLogConnectString, (c) =>
                {
                    tb_error_log_dal dal = new tb_error_log_dal();
                    dal.Add(c, new tb_error_log_model()
                    {
                        logcreatetime = DateTime.Now,
                        developer = "车江毅",
                        logtag = "统一监控平台系统异常",
                        logtype = (int)XXF.BaseService.Monitor.SystemRuntime.EnumErrorLogType.SystemError,
                        msg = msg + "【原始信息】" + exp.Message.NullToEmpty(),
                        projectname = "统一监控平台",
                        remark = "",
                        tracestack = exp.StackTrace
                    });
                });
                Debug.WriteLine(msg + exp.Message);
                XXF.Log.ErrorLog.Write(msg, exp);
            }
            catch (Exception e)
            {
                XXF.Log.ErrorLog.Write(msg, e);
            }
        }

        public static void Log(string msg)
        {
            try
            {
                LoadConnectString();
                SqlHelper.ExcuteSql(UnityLogConnectString, (c) =>
                {
                    tb_log_dal dal = new tb_log_dal();
                    dal.Add(c, new tb_log_model()
                    {
                        logcreatetime = DateTime.Now,
                        logtag = "统一监控平台",
                        logtype = (int)XXF.BaseService.Monitor.SystemRuntime.EnumCommonLogType.SystemLog,
                        msg = msg,
                        projectname = "统一监控平台"
                    });
                });
                Debug.WriteLine(msg);
                CommLog.Write(msg);
            }
            catch (Exception e)
            {
                XXF.Log.ErrorLog.Write(msg, e);
            }

        }
    }
}
