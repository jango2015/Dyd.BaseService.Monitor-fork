using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXF.BaseService.Monitor.Model;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.BaseService.Monitor.SystemRuntime.BatchQueues;

namespace XXF.BaseService.Monitor
{
    public class TimeWatchHelper
    {
        static TimeWatchLogBatchQueue timewatchlogbatchqueue;
        static TimeWatchLogApiBatchQueue timewatchlogapibatchqueue;

        static TimeWatchHelper()
        {
            timewatchlogbatchqueue = new TimeWatchLogBatchQueue();
            timewatchlogapibatchqueue = new TimeWatchLogApiBatchQueue();
        }

        public static void AddTimeWatchLog(TimeWatchLogInfo log)
        {
            if (XXF.Common.XXFConfig.IsWriteTimeWatchLog && XXF.Common.XXFConfig.IsWriteTimeWatchLogToMonitorPlatform)
            { 
                try
                {
                    timewatchlogbatchqueue.Add(log);
                }
                catch (Exception exp)
                {
                    XXF.Log.ErrorLog.Write("耗时日志出错",exp);
                }
            }
        }

        public static void AddTimeWatchApiLog(TimeWatchLogApiInfo log)
        {
            if (XXF.Common.XXFConfig.IsWriteTimeWatchLog && XXF.Common.XXFConfig.IsWriteTimeWatchLogToMonitorPlatform)
            {
                try
                {
                    timewatchlogapibatchqueue.Add(log);
                    //timewatchlogbatchqueue.Add(new TimeWatchLogInfo()
                    //{
                    //    fromip = log.fromip,
                    //    logcreatetime = log.logcreatetime,
                    //    logtag = log.url.GetHashCode(),
                    //    url=log.url,
                    //    time = log.time,
                    //    sqlip = "",
                    //    remark = log.tag,
                    //    projectname = log.projectname,
                    //    msg = log.msg,
                    //    logtype = (int)EnumTimeWatchLogType.ApiUrl
                    //});
                }
                catch (Exception exp)
                {
                    XXF.Log.ErrorLog.Write("耗时日志api出错", exp);
                }
            }
        }
    }
}
