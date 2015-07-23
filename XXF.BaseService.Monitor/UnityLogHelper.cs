using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXF.BaseService.Monitor.Dal;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.BaseService.Monitor.SystemRuntime.BatchQueues;
using XXF.ProjectTool;
using XXF.Extensions;

namespace XXF.BaseService.Monitor
{
    public class UnityLogHelper
    {
        static LogBatchQueue logbatchqueue;
        static UnityLogHelper()
        {
            logbatchqueue = new LogBatchQueue();
        }

        public static void AddCommonLog(CommonLogInfo log)
        {
            if (XXF.Common.XXFConfig.IsWriteCommonLog && XXF.Common.XXFConfig.IsWriteCommonLogToMonitorPlatform)
            {
                try
                {
                    logbatchqueue.Add(log);
                }
                catch (Exception exp)
                {
                    XXF.Log.ErrorLog.Write("常用日志出错", exp);
                }
            }
        }

        public static void AddErrorLog(ErrorLogInfo log)
        {
            if (XXF.Common.XXFConfig.IsWriteErrorLog && XXF.Common.XXFConfig.IsWriteErrorLogToMonitorPlatform)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(Config.UnityLogConnectString))
                    {
                        SqlHelper.ExcuteSql(Config.UnityLogConnectString, (c) =>
                        {
                            tb_error_log_dal errorlogdal = new tb_error_log_dal();
                            errorlogdal.Add(c, log);
                        });
                        logbatchqueue.Add(new CommonLogInfo() { logcreatetime = log.logcreatetime, logtag = log.logtag, logtype = log.logtype, msg = log.msg.SubString2(900).NullToEmpty(), projectname = log.projectname });
                    }
                }
                catch (Exception exp)
                {
                    
                }
            }
        }
    }
}
