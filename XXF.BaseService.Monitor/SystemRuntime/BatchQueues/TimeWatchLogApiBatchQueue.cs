using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XXF.BaseService.Monitor.Model;
using XXF.Db;
using XXF.ProjectTool;

namespace XXF.BaseService.Monitor.SystemRuntime.BatchQueues
{
    public class TimeWatchLogApiBatchQueue : BaseBatchQueue<tb_timewatchlog_api_model>
    {
        public override int MaxQueueCount { get { return 10000; } }

        public override int MaxSleepTime { get { return 5000 * 60; } }//5分钟

        public override int SleepTime { get { return 5000; } }

        public override string BatchTable { get { return "tb_timewatchlog_api"; } }

         protected override void BatchCommit()
        {
            if (!string.IsNullOrEmpty(XXF.Common.XXFConfig.TimeWatchConnectionString))
            {
                var timewatchinfoTable = DataTableHelper.ConvertToDataTable<tb_timewatchlog_api_model>(TempQueue);
                var dict = new Dictionary<string, string>
                            {
                                //{"sqlservercreatetime", "sqlservercreatetime"},
                                {"logcreatetime","logcreatetime"},
                                {"time", "time"},
                                {"projectname", "projectname"},
                                {"url", "url"},
                                {"tag", "tag"},
                                {"msg", "msg"},
                                {"fromip", "fromip"}
                            };
                using (var c = Db.DbConfig.CreateConn(Db.DbType.SQLSERVER, XXF.Common.XXFConfig.TimeWatchConnectionString))
                {
                    c.Open();
                    c.BeginTransaction();
                    try
                    {
                        c.SqlBulkCopy(timewatchinfoTable, BatchTable + DateTime.Now.ToString("yyyyMMdd"), "", new List<ProcedureParameter>(), dict, 0);
                        c.Commit();
                    }
                    catch (Exception exp)
                    {
                        c.Rollback();
                    }
                }
            }

        }
    }
}
