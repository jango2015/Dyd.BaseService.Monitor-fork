using Dyd.BaseService.Monitor.Domain.Chart.Model;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
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
    public partial class tb_timewatchlog_dal
    {
        public List<tb_timewatchlog_model> GetList(DbConn PubConn, string timebegin, string timeend, double timewatchmin, double timewatchmax, string projectname, int logtype,string logtag, string fromip, string sqlip, string orderby,string url,int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_timewatchlog_model> model = new List<tb_timewatchlog_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                string zq = "";
                DateTime dtimebegin = DateTime.Now;
                if (DateTime.TryParse(timebegin, out dtimebegin))
                {
                    ps.Add("@dtimebegin", dtimebegin);
                    sqlwhere += " and sqlservercreatetime>=@dtimebegin";
                }
                DateTime dtimeend = DateTime.Now;
                if (DateTime.TryParse(timeend, out dtimeend))
                {
                    ps.Add("@dtimeend", dtimeend);
                    sqlwhere += " and sqlservercreatetime<=@dtimeend";
                }
                if (timewatchmin != -1)
                {
                    ps.Add("@timewatchmin", timewatchmin);
                    sqlwhere += " and time>=@timewatchmin";
                }
                if (timewatchmax != -1)
                {
                    ps.Add("@timewatchmax", timewatchmax);
                    sqlwhere += " and time<=@timewatchmax";
                }
                if (!string.IsNullOrWhiteSpace(projectname))
                {
                    ps.Add("@projectname", projectname);
                    sqlwhere += " and projectname<=@projectname";
                }
                if (!string.IsNullOrWhiteSpace(url))
                {
                    ps.Add("@url", url);
                    sqlwhere += " and url=@url";
                }
                if (logtype != -1)
                {
                    ps.Add("@logtype", logtype);
                    sqlwhere += " and logtype=@logtype";
                }
                if (!string.IsNullOrWhiteSpace(fromip))
                {
                    ps.Add("@fromip", fromip);
                    sqlwhere += " and fromip=@fromip";
                }
                if (!string.IsNullOrWhiteSpace(sqlip))
                {
                    ps.Add("@sqlip", sqlip);
                    sqlwhere += " and sqlip=@sqlip";
                } 
                if (!string.IsNullOrWhiteSpace(logtag))
                {
                    ps.Add("@logtag", logtag);
                    sqlwhere += " and logtag=@logtag";
                }
                string orderbywhere = "";
                if (string.IsNullOrWhiteSpace(orderby))
                    orderbywhere = "sqlservercreatetime desc";
                else
                    orderbywhere = orderby;
                zq = dtimeend.ToString("yyyyMMdd");
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by " + orderbywhere + ") as rownum,* from tb_timewatchlog" + zq + " with (nolock) " + sqlwhere);
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_timewatchlog" + zq + " with (nolock) " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_timewatchlog_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public List<tb_timewatchlog_model> GetChartJson(DbConn PubConn, string day, string stime, string etime, string keyword)
        {
            return SqlHelper.Visit(ps =>
            {
                List<tb_timewatchlog_model> model = new List<tb_timewatchlog_model>();
                string zq = "";
                DateTime dateNow = DateTime.Now;
                if (DateTime.TryParse(day, out dateNow))
                {
                    zq = dateNow.ToString("yyyyMMdd");
                }
                else
                {
                    zq = DateTime.Now.ToString("yyyyMMdd");
                }
                if (!string.IsNullOrWhiteSpace(stime))
                {
                    ps.Add("stime", dateNow.ToString("yyyy-MM-dd ") + stime);
                }
                else
                {
                    ps.Add("stime", dateNow.ToString("yyyy-MM-dd 00:00:00"));
                }
                if (!string.IsNullOrWhiteSpace(etime))
                {
                    ps.Add("etime", dateNow.ToString("yyyy-MM-dd ") + etime);
                }
                else
                {
                    ps.Add("etime", dateNow.ToString("yyyy-MM-dd 23:59:59"));
                }
                ps.Add("keyword", keyword);
                string sql = "select * from tb_timewatchlog" + zq + " where logtag=@keyword and logcreatetime between @stime and @etime";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_timewatchlog_model m = CreateModel(dr);
                    m.nodenotes = m.logcreatetime.ToString("HH:mm:ss fff");
                    model.Add(m);
                }
                return model;
            });
        }

        public Dictionary<string, List<ChartMainModel>> GetMainChartJson(DbConn PubConn, int num, string key, int type)
        {
            return SqlHelper.Visit(ps =>
            {
                Dictionary<string, List<ChartMainModel>> d = new Dictionary<string, List<ChartMainModel>>();
                DateTime date = DateTime.Now;
                string Stime = date.AddDays(num).ToString("yyyy-MM-dd");
                string Etime = date.ToString("yyyy-MM-dd");
                List<ChartMainModel> chartMainModel = new List<ChartMainModel>();
                ChartMainModel MainModel = new ChartMainModel();
                MainModel.Tkey = "avgtime";
                MainModel.Tvalue = 0;
                chartMainModel.Add(MainModel);
                MainModel = new ChartMainModel();
                MainModel.Tkey = "maxtime";
                MainModel.Tvalue = 0;
                chartMainModel.Add(MainModel);
                MainModel = new ChartMainModel();
                MainModel.Tkey = "mintime";
                MainModel.Tvalue = 0;
                chartMainModel.Add(MainModel);
                for (int i = num; i <= 0; i++)
                {
                    d.Add(date.AddDays(i).ToString("yyyy-MM-dd"), chartMainModel);
                }
                ps.Add("Stime", Stime);
                ps.Add("Etime", Etime);
                ps.Add("key", key);
                string sql = "";
                if (type == 1)
                {
                    sql = "select date,avgtime,maxtime,mintime from tb_timewatchlog_sql_dayreport where date>=@Stime and date<=@Etime and sqlhash=@key order by date";
                }
                else
                {
                    sql = "select date,avgtime,maxtime,mintime from tb_timewatchlog_api_dayreport where date>=@Stime and date<=@Etime and url=@key order by date";
                }
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                foreach (DataRow dr in dt.Rows)
                {
                    List<ChartMainModel> m = new List<ChartMainModel>();
                    foreach (DataColumn c in dr.Table.Columns)
                    {
                        if (c.ColumnName != "date")
                        {
                            ChartMainModel main = new ChartMainModel();
                            main.Tkey = c.ColumnName;
                            main.Tvalue = Convert.ToDecimal(dr[c.ColumnName]);
                            m.Add(main);
                        }
                    }
                    d[Convert.ToDateTime(dr["date"]).ToString("yyyy-MM-dd")] = m;
                }
                return d;
            });
        }

        public Dictionary<string, List<ChartSubModel>> GetSubChartJson(DbConn PubConn, List<string> day, string key, string columnname)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("key", key);
                Dictionary<string, List<ChartSubModel>> dic = new Dictionary<string, List<ChartSubModel>>();
                foreach (string d in day)
                {
                    string zq = Convert.ToDateTime(d).ToString("yyyyMMdd");
                    List<ChartSubModel> model = new List<ChartSubModel>();
                    string sql = "select id,ISNULL(avgtime,0) avgtime,ISNULL(maxtime,0) maxtime,ISNULL(mintime,0) mintime from tb_Hour H left join (select AVG(time) avgtime, Max(time) maxtime,Min(time) mintime,DATENAME(hour,logcreatetime) hour from tb_timewatchlog" + zq + " where logtag=@key group by DATENAME(hour,logcreatetime)) D on H.id=D.hour ";
                    DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                    foreach (DataRow dr in dt.Rows)
                    {
                        ChartSubModel m = new ChartSubModel();
                        m.Tkey = dr["id"].ToString();
                        m.Tvalue = Convert.ToDecimal(dr[columnname]);
                        model.Add(m);
                    }
                    dic.Add(d, model);
                }
                return dic;
            });
        }

        //public List<tb_timewatchlog_model> GetHourChartJson(DbConn PubConn, string day)
        //{
        //    return SqlHelper.Visit(ps =>
        //    {
        //        List<tb_timewatchlog_model> model = new List<tb_timewatchlog_model>();
        //        string zq = "";
        //        DateTime dateNow = DateTime.Now;
        //        ps.Add("keyword", keyword);
        //        string sql = "select * from tb_timewatchlog" + zq + " where logtag=@keyword and logcreatetime between @stime and @etime";
        //        DataSet ds = new DataSet();
        //        PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            tb_timewatchlog_model m = CreateModel(dr);
        //            m.nodenotes = m.logcreatetime.ToString("HH:mm:ss fff");
        //            model.Add(m);
        //        }
        //        return model;
        //    });
        //}

        public Dictionary<DateTime, double> GetTimeChart(DbConn PubConn, string column, DateTime date, string logtag,string datatype,string logtype)
        {
            return SqlHelper.Visit(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(logtag))
                {
                    ps.Add("logtag", logtag);
                    sqlwhere += " and p.logtag=@logtag";
                } 
                //ps.Add("logtype", logtype);
                //sqlwhere += " and p.logtype=@logtype";
                if (datatype == "count")
                    column = "id";
                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                string sql = string.Format(@"select CONVERT(varchar(30),p.sqlservercreatetime,100) as date,{3}(p.{2}) as avgnum from tb_timewatchlog{0} p with (nolock) {1} group by CONVERT(varchar(30),p.sqlservercreatetime,100)",
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
