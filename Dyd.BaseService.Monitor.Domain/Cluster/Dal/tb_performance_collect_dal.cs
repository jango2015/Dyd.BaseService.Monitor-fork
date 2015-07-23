using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using XXF.ProjectTool;
using Dyd.BaseService.Monitor.Core;
using XXF.BaseService.Monitor.SystemRuntime;
using Dyd.BaseService.Monitor.Domain.Chart.Model;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Dal
{
    public partial class tb_performance_collect_dal
    {
        public void AddCollect(DbConn PubConn, tb_performance_collect_model model)
        {
            SqlHelper.Visit(ps =>
            {
                //服务器id
                ps.Add("@serverid", model.serverid);
                //cpu信息
                ps.Add("@cpu", model.cpu);
                //内存字节
                ps.Add("@memory", model.memory);
                //网络上传字节
                ps.Add("@networkupload", model.networkupload);
                //网络下载字节
                ps.Add("@networkdownload", model.networkdownload);
                //io读字节
                ps.Add("@ioread", model.ioread);
                //io写字节
                ps.Add("@iowrite", model.iowrite);
                //
                ps.Add("@iisrequest", model.iisrequest);
                //创建时间
                ps.Add("@createtime", model.createtime);
                return  PubConn.ExecuteSql(string.Format(@"insert into tb_performance_collect{0}(serverid,cpu,memory,networkupload,networkdownload,ioread,iowrite,iisrequest,createtime)
										   values(@serverid,@cpu,@memory,@networkupload,@networkdownload,@ioread,@iowrite,@iisrequest,@createtime)", DbShardingHelper.DayRule(DateTime.Now)), ps.ToParameters());
            });
        }

        public List<tb_performance_collect_model> GetList(DbConn PubConn, string serverid,string serverip, string timebegin,string timeend,string orderby, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            string connday = "";
            if (!string.IsNullOrWhiteSpace(timeend))
            {
                connday = DateTime.Parse(timeend).ToString("yyyyMMdd");
            }
            List<tb_performance_collect_model> model = new List<tb_performance_collect_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                DateTime date = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(serverid))
                {
                    ps.Add("serverid", serverid);
                    sqlwhere += " and p.serverid =@serverid ";
                }
                if (!string.IsNullOrWhiteSpace(serverip))
                {
                    ps.Add("serverip", serverip);
                    sqlwhere += " and c.serverip =@serverip ";
                }
                if (!string.IsNullOrWhiteSpace(timebegin))
                {
                    ps.Add("timebegin", timebegin);
                    sqlwhere += " and p.createtime >=@timebegin ";
                }
                if (!string.IsNullOrWhiteSpace(timeend))
                {
                    ps.Add("timeend", timeend);
                    sqlwhere += " and p.createtime <=@timeend ";
                }
                string orderbywhere = "p.id";
                if (!string.IsNullOrWhiteSpace(orderby))
                {

                    orderbywhere = " p."+orderby;
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by " + orderbywhere + " desc) as rownum,p.*,c.serverip,c.servername from tb_performance_collect" + connday + " p with (nolock) right join dyd_bs_monitor_platform_manage.dbo.tb_cluster c with (nolock) on p.serverid=c.id");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_performance_collect" + connday + " p with (nolock) right join dyd_bs_monitor_platform_manage.dbo.tb_cluster c with (nolock) on p.serverid=c.id" + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_performance_collect_model m = CreateModel(dr);
                //创建时间
                if (dr.Table.Columns.Contains("serverip"))
                {
                    m.serverip = dr["serverip"].Tostring();
                }
                if (dr.Table.Columns.Contains("servername"))
                {
                    m.servername = dr["servername"].Tostring();
                }
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public List<tb_performance_collect_model> GetChartJson(DbConn PubConn, string day, string stime, string etime, string keyword)
        {
            return SqlHelper.Visit(ps =>
            {
                List<tb_performance_collect_model> model = new List<tb_performance_collect_model>();
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
                string sql = "select * from tb_performance_collect" + zq + " where serverid=@keyword and createtime between @stime and @etime";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_performance_collect_model m = CreateModel(dr);
                    m.nodenotes = m.createtime.ToString("HH:mm:ss fff");
                    model.Add(m);
                }
                return model;
            });
        }

        public Dictionary<string, List<ChartMainModel>> GetMainChartJson(DbConn PubConn, int num, string key)
        {
            return SqlHelper.Visit(ps =>
            {
                Dictionary<string, List<ChartMainModel>> d = new Dictionary<string, List<ChartMainModel>>();
                DateTime date = DateTime.Now;
                string Stime = date.AddDays(num).ToString("yyyy-MM-dd");
                string Etime = date.ToString("yyyy-MM-dd");
                ps.Add("Stime", Stime);
                ps.Add("Etime", Etime);
                ps.Add("key", key);
                string sql = @"select date,avgcpu,maxcpu,mincpu,avgmemory,maxmemory,minmemory,
                avgnetworkupload,maxnetworkupload,minnetworkupload,avgnetworkdownload,
                maxnetworkdownload,miniowrite,avgiisrequest,maxiisrequest,miniisrequest,
                minnetworkdownload,avgioread,maxioread,minioread,avgiowrite,maxiowrite
                from tb_performance_dayreport where date>=@Stime and date<=@Etime and serverid=@key order by date";
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                List<ChartMainModel> chartMainModel = new List<ChartMainModel>();
                foreach (DataColumn c in dt.Columns)
                {
                    if (c.ColumnName != "date")
                    {
                        ChartMainModel MainModel = new ChartMainModel();
                        MainModel.Tkey = c.ColumnName;
                        MainModel.Tvalue = 0;
                        chartMainModel.Add(MainModel);
                    }
                }
                for (int i = num; i <= 0; i++)
                {
                    d.Add(date.AddDays(i).ToString("yyyy-MM-dd"), chartMainModel);
                }
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
                    string sqlColumnName = "";
                    if (columnname.IndexOf("avg") == 0)
                    {
                        string sqlDayTableKey = columnname.Replace("avg", "");
                        sqlColumnName = "AVG(" + sqlDayTableKey + ") " + columnname;
                    }
                    else if (columnname.IndexOf("max") == 0)
                    {
                        string sqlDayTableKey = columnname.Replace("max", "");
                        sqlColumnName = "MAX(" + sqlDayTableKey + ") " + columnname;
                    }
                    else
                    {
                        string sqlDayTableKey = columnname.Replace("min", "");
                        sqlColumnName = "MIN(" + sqlDayTableKey + ") " + columnname;
                    }
                    string sql = @"select id,ISNULL(" + columnname + ",0) "+columnname+" from tb_hour H left join (select DATENAME(hour,createtime) hour," + sqlColumnName + " from tb_performance_collect" + zq + " where serverid=@key group by DATENAME(hour,createtime)) D on H.id=D.hour ";
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

        public Dictionary<DateTime, double> GetTimeChart(DbConn PubConn,string column,DateTime date,string serverip,string serverid,string datatype)
        {
            return SqlHelper.Visit(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(serverip))
                {
                    ps.Add("serverip",serverip);
                    sqlwhere += " and c.serverip=@serverip";
                }
                if (!string.IsNullOrWhiteSpace(serverid))
                {
                    ps.Add("serverid", serverid);
                    sqlwhere += " and p.serverid=@serverid";
                }
                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                string sql = string.Format(@"select CONVERT(varchar(30),p.createtime,100) as date,{3}(p.{2}) as avgnum from tb_performance_collect{0} p with (nolock) right join [dyd_bs_monitor_platform_manage].dbo.tb_cluster c with (nolock)
                                             on c.id=p.serverid {1} group by CONVERT(varchar(30),p.createtime,100)",
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