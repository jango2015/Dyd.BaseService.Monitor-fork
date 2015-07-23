using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;
using XXF.Extensions;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Dal
{
    public partial class tb_timewatchlog_api_dal
    {
        //public string nodenotes { get; set; }

        public Dictionary<DateTime, double> GetTimeChart(DbConn PubConn, string column, DateTime date, string logtag, string datatype, string logtype)
        {
            return SqlHelper.Visit(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(logtag))
                {
                    ps.Add("logtag", logtag);
                    sqlwhere += " and p.url=@logtag";
                }
                //ps.Add("logtype", logtype);
                //sqlwhere += " and p.logtype=@logtype";
                if (datatype == "count")
                    column = "id";
                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                string sql = string.Format(@"select CONVERT(varchar(30),p.sqlservercreatetime,100) as date,{3}(p.{2}) as avgnum from tb_timewatchlog_api{0} p with (nolock) {1} group by CONVERT(varchar(30),p.sqlservercreatetime,100)",
                    date.Date.ToString("yyyyMMdd"), sqlwhere, column, datatype);
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dic.Add(dr["date"].ToDateTime(), dr["avgnum"].Todouble());
                    }
                }
                return dic;
            });
        }
    }
}
