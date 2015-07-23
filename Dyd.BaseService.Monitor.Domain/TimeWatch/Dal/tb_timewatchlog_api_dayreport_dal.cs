using Dyd.BaseService.Monitor.Domain.Chart.Dal;
using Dyd.BaseService.Monitor.Domain.Chart.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.Common;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Dal
{
    public partial class tb_timewatchlog_api_dayreport_dal
    {
        public List<tb_timewatchlog_api_dayreport_model> GetDayReport(DbConn PubConn, DateTime maxdate)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@date", maxdate);
                string cmd = "select * from tb_timewatchlog_api_dayreport where date>=@date";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                List<tb_timewatchlog_api_dayreport_model> rs = new List<tb_timewatchlog_api_dayreport_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        rs.Add(CreateModel(dr));
                }
                return rs;
            });
        }
        public List<tb_timewatchlog_api_dayreport_model> GetList(DbConn PubConn, string url, string timebegin, string timeend, string orderby, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_timewatchlog_api_dayreport_model> model = new List<tb_timewatchlog_api_dayreport_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                DateTime dtimebegin = DateTime.Now;
                if (DateTime.TryParse(timebegin, out dtimebegin))
                {
                    ps.Add("timebegin", dtimebegin);
                    sqlwhere += " and d.date>=@timebegin ";
                }
                DateTime dtimeend = DateTime.Now;
                if (DateTime.TryParse(timeend, out dtimeend))
                {
                    ps.Add("timeend", timeend);
                    sqlwhere += " and d.date<=@timeend ";
                }
                if (!string.IsNullOrWhiteSpace(url))
                {
                    ps.Add("url", url);
                    sqlwhere += " and d.url=@url ";
                }
                if (string.IsNullOrWhiteSpace(orderby))
                    orderby = "date";
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by "+orderby+" desc) as rownum,* from tb_timewatchlog_api_dayreport d with (nolock) " + sqlwhere);
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_timewatchlog_api_dayreport d with (nolock) " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_timewatchlog_api_dayreport_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public Dictionary<string, tb_timewatchlog_api_dayreport_model> GetOldDayReport(DbConn PubConn, string day)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("day", day);
                string sql = "select * from tb_timewatchlog_api_dayreport with (nolock) where date=@day";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                Dictionary<string, tb_timewatchlog_api_dayreport_model> dic = new Dictionary<string, tb_timewatchlog_api_dayreport_model>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_timewatchlog_api_dayreport_model m = CreateModel(dr);
                    dic.Add(m.url, m);
                }
                return dic;
            });
        }

        public int GetOldDayReportMaxID(DbConn PubConn, string day)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("day", day);
                string sql = "select ISNULL(MAX(lastmaxid),0) from tb_timewatchlog_api_dayreport where date=@day";
                int maxID = Convert.ToInt32(PubConn.ExecuteScalar(sql, ps.ToParameters()));
                return maxID;
            });
        }

        private List<tb_timewatchlog_api_dayreport_model> GetNewDayReport(DbConn PubConn, string day, int oldmaxid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", oldmaxid);
                ps.Add("day", day);
                string sql = "select url,AVG(time) avgtime,MAX(time) maxtime,MIN(time) mintime, count(1) count,MAX(id) lastmaxid from tb_timewatchlog_api" + day + " with (nolock) where id>@id Group By Url";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                List<tb_timewatchlog_api_dayreport_model> model = new List<tb_timewatchlog_api_dayreport_model>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_timewatchlog_api_dayreport_model m = CreateModel(dr);
                    model.Add(m);
                }
                return model;
            });
        }

        public bool StatisApiMinitor()
        {
            tb_timewatchlog_api_dayreport_model staticDayReport = new tb_timewatchlog_api_dayreport_model();
            List<string> sqlConnList = new List<string>();
            string day = "";
            string date = "";
            using (DbConn PubConn = DbConfig.CreateConn(XXFConfig.MonitorPlatformConnectionString))
            {
                PubConn.Open();
                DateTime dateNow = PubConn.GetServerDate();
                day = dateNow.ToString("yyyyMMdd");
                date = dateNow.ToString("yyyy-MM-dd");
                sqlConnList = new tb_database_config_dal().GetDataBaseSqlConnList(PubConn, (int)DataBaseType.Timewatch);
            }
            foreach (string conn in sqlConnList)
            {
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    int oldMaxID = GetOldDayReportMaxID(PubConn, date);
                    Dictionary<string, tb_timewatchlog_api_dayreport_model> oldDayReportList = GetOldDayReport(PubConn, date);
                    List<tb_timewatchlog_api_dayreport_model> newDayReportList = GetNewDayReport(PubConn, day, oldMaxID);
                    foreach (tb_timewatchlog_api_dayreport_model m in newDayReportList)
                    {
                        if (oldDayReportList.Keys.Contains(m.url))
                        {
                            tb_timewatchlog_api_dayreport_model oldStaticDayReport = oldDayReportList[m.url];
                            tb_timewatchlog_api_dayreport_model newStaticDayReport = new tb_timewatchlog_api_dayreport_model()
                            {
                                id = oldStaticDayReport.id,
                                avgtime = (oldStaticDayReport.avgtime * oldStaticDayReport.count + m.avgtime * m.count) / (oldStaticDayReport.count + m.count),
                                maxtime = oldStaticDayReport.maxtime > m.maxtime ? oldStaticDayReport.maxtime : m.maxtime,
                                mintime = oldStaticDayReport.mintime < m.mintime ? oldStaticDayReport.mintime : m.mintime,
                                lastmaxid = m.lastmaxid,
                                url = m.url,
                                count = oldStaticDayReport.count + m.count,
                                lastupdatetime = DateTime.Now
                            };
                            Update(PubConn, newStaticDayReport);
                        }
                        else
                        {
                            Insert(PubConn, m);
                        }
                    }
                }
            }
            return true;
        }

        public int Update(DbConn PubConn, tb_timewatchlog_api_dayreport_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", model.id);
                ps.Add("avgtime", model.avgtime);
                ps.Add("maxtime", model.maxtime);
                ps.Add("mintime", model.mintime);
                ps.Add("lastmaxid", model.lastmaxid);
                ps.Add("count", model.count);
                string sql = @"update tb_timewatchlog_api_dayreport set 
                avgtime=@avgtime,maxtime=@maxtime,mintime=@mintime,
                lastmaxid=@lastmaxid,lastupdatetime=getdate(),count=@count where id=@id";
                int rev = PubConn.ExecuteSql(sql, ps.ToParameters());
                return rev;
            });
        }

        public int Insert(DbConn PubConn, tb_timewatchlog_api_dayreport_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("url", model.url);
                ps.Add("avgtime", model.avgtime);
                ps.Add("maxtime", model.maxtime);
                ps.Add("mintime", model.mintime);
                ps.Add("lastmaxid", model.lastmaxid);
                ps.Add("count", model.count);
                string sql = @"insert into tb_timewatchlog_api_dayreport
                (url,date,avgtime,maxtime,mintime,lastmaxid,lastupdatetime,count)
                values
                (@url,getdate(),@avgtime,@maxtime,@mintime,@lastmaxid,getdate(),@count)";
                int rev = PubConn.ExecuteSql(sql, ps.ToParameters());
                return rev;
            });
        }

        #region Chart
        public List<Dictionary<string, object>> GetChartJson(DbConn PubConn, string Fday, string Nday, string key)
        {
            return SqlHelper.Visit(ps =>
            {
                List<Dictionary<string, object>> model = new List<Dictionary<string, object>>();
                ChartDal dal = new ChartDal();
                ps.Add("Fday", Fday);
                ps.Add("Nday", Nday);
                string sql = "select date,AVG(" + key + ") Cvalue from tb_timewatchlog_api_dayreport where date=@Fday or date=@Nday group by date";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Dictionary<string, object> m = new Dictionary<string, object>();
                    m.Add("Date", dr["date"]);
                    m.Add("Cvalue", dr["Cvalue"]);
                    model.Add(m);
                }
                return model;
            });
        }

        public List<ChartModel> GetKeyChartJson(DbConn PubConn, string Fday, string Nday, string key, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<ChartModel> Model = SqlHelper.Visit(ps =>
            {
                ps.Add("Fday", Fday);
                ps.Add("Nday", Nday);
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_timewatchlog_api_dayreport with (nolock) where date=@Fday", ps.ToParameters()));
                StringBuilder sb_Sql_Select = new StringBuilder();
                sb_Sql_Select.Append("select Tkey,Avalue,Bvalue,AsubB,rownum from (");
                sb_Sql_Select.Append("select ROW_NUMBER() over(order by ISNULL(B." + key + ",0)-ISNULL(A." + key + ",0) desc) as rownum,A.url Tkey,ISNULL(A." + key + ",0) Avalue,ISNULL(B." + key + ",0) Bvalue,ISNULL(B." + key + ",0)-ISNULL(A." + key + ",0) AS AsubB from ");
                sb_Sql_Select.Append("(select url," + key + " from tb_timewatchlog_api_dayreport with (nolock) where date=@Fday) A ");
                sb_Sql_Select.Append(" left join ");
                sb_Sql_Select.Append("(select url," + key + " from tb_timewatchlog_api_dayreport with (nolock) where date=@Nday) B ");
                sb_Sql_Select.Append(" on A.url=B.url) Temp ");
                sb_Sql_Select.Append(" where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex);
                DataTable dt = PubConn.SqlToDataTable(sb_Sql_Select.ToString(), ps.ToParameters());
                List<ChartModel> model = new List<ChartModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    model.Add(Chart.Dal.ChartDal.CreateChartModel(dr));
                }
                return model;
            });
            count = _count;
            return Model;
        }
        #endregion

        #region Chart
        public List<Dictionary<string, object>> GetChartJson(DbConn PubConn, string Sday, string Eday, string mainday, string secday, string ckey, string gkey)
        {
            return SqlHelper.Visit(ps =>
            {
                List<Dictionary<string, object>> model = new List<Dictionary<string, object>>();
                ChartDal dal = new ChartDal();
                string sqlwhere = "where ";
                if (!string.IsNullOrWhiteSpace(Sday))
                {
                    ps.Add("Sday", Sday);
                    ps.Add("Eday", Eday);
                    sqlwhere += " date>=@Sday and date<=@Eday ";
                }
                else
                {
                    ps.Add("Mday", mainday);
                    ps.Add("Sday", secday);
                    sqlwhere += " date in (@Mday,@Sday) ";
                }
                string sql = "select * from tb_timewatchlog_api_dayreport " + sqlwhere;
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    model.Add(dal.CreateDicModel(dr, ckey, gkey));
                }
                return model;
            });
        }

        public List<Dictionary<string, object>> GetAvgChartJson(DbConn PubConn, string Sday, string Eday, string mainday, string secday)
        {
            return SqlHelper.Visit<List<Dictionary<string, object>>>(ps =>
            {
                string sqlwhere = "where ";
                if (!string.IsNullOrWhiteSpace(Sday))
                {
                    ps.Add("Sday", Sday);
                    ps.Add("Eday", Eday);
                    sqlwhere += " date>=@Sday and date<=@Eday ";
                }
                else
                {
                    ps.Add("Mday", mainday);
                    ps.Add("Sday", secday);
                    sqlwhere += " date in (@Mday,@Sday) ";
                }
                string sql = "select date,AVG(avgtime) avgtime,AVG(maxtime) maxtime,AVG(mintime) mintime from tb_timewatchlog_api_dayreport " + sqlwhere + " group by date";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                List<Dictionary<string, object>> model = new List<Dictionary<string, object>>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    model.Add(Chart.Dal.ChartDal.CreateModel(dr));
                }
                return model;
            });
        }
        #endregion

        public Dictionary<DateTime, double> GetTimeChart(DbConn PubConn, string column, DateTime datebegin, DateTime dateend, string logtag, string datatype)
        {
            return SqlHelper.Visit(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(logtag))
                {
                    ps.Add("logtag", logtag);
                    sqlwhere += " and p.url=@logtag";
                }

                ps.Add("datebegin", datebegin);
                sqlwhere += " and p.date>=@datebegin";
                ps.Add("dateend", dateend);
                sqlwhere += " and p.date<=@dateend";
                if (datatype == "count")
                    column = "";
                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                string sql = string.Format(@"select p.date,p.{3}{2} as avgnum from tb_timewatchlog_api_dayreport p with (nolock) {1} ",
                    "", sqlwhere, column, datatype);
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dic.Add(Convert.ToDateTime(dr["date"]), Convert.ToDouble(dr["avgnum"]));
                    }
                }
                return dic;
            });
        }
    }
}
