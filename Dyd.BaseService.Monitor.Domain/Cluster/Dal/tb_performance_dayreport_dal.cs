using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using XXF.ProjectTool;
using Dyd.BaseService.Monitor.Domain.Chart.Dal;
using Dyd.BaseService.Monitor.Domain.Chart.Model;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Dal
{
	public partial class tb_performance_dayreport_dal
    {
        public List<tb_performance_dayreport_model> GetDayReport(DbConn PubConn, DateTime maxdate)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@date", maxdate);
                string cmd = "select * from tb_performance_dayreport where date>=@date";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                List<tb_performance_dayreport_model> rs = new List<tb_performance_dayreport_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        rs.Add(CreateModel(dr));
                }
                return rs;
            });
        }

        public List<tb_performance_dayreport_model> GetList(DbConn PubConn,string serverip,string serverid, string timebegin, string timeend, string orderby, string datatype, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_performance_dayreport_model> model = new List<tb_performance_dayreport_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(serverip))
                {
                    ps.Add("serverip", serverip);
                    sqlwhere += " and c.serverip=@serverip ";
                }
                if (!string.IsNullOrWhiteSpace(serverid))
                {
                    ps.Add("serverid", serverid);
                    sqlwhere += " and d.serverid=@serverid ";
                }
                DateTime dtimebegin = DateTime.Now;
                if (DateTime.TryParse(timebegin, out dtimebegin))
                {
                    ps.Add("dtimebegin", dtimebegin.ToString("yyyy-MM-dd"));
                    sqlwhere += " and d.date>=@dtimebegin ";
                }
                DateTime dtimeend = DateTime.Now;
                if (DateTime.TryParse(timeend, out dtimeend))
                {
                    ps.Add("timeend", dtimeend.ToString("yyyy-MM-dd"));
                    sqlwhere += " and d.date<=@timeend ";
                }
                string orderbywhere = "";
                if (!string.IsNullOrWhiteSpace(orderby))
                {
                    orderbywhere = "d." + datatype + orderby;
                }
                else
                    orderbywhere = "d.date";
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by " + orderbywhere + " desc) as rownum,d.*,c.serverip,c.servername from tb_performance_dayreport " + "d with (nolock) right join dyd_bs_monitor_platform_manage.dbo.tb_cluster c with (nolock) on d.serverid=c.id ");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_performance_dayreport " + "d with (nolock) right join dyd_bs_monitor_platform_manage.dbo.tb_cluster c with (nolock) on d.serverid=c.id " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_performance_dayreport_model m = CreateModel(dr);
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

        public Dictionary<int, tb_performance_dayreport_model> GetAutoTaskList(DbConn PubConn, string day)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("day", day);
                string sql = "select * from tb_performance_dayreport with (nolock) where date=@day";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                Dictionary<int, tb_performance_dayreport_model> dic = new Dictionary<int, tb_performance_dayreport_model>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_performance_dayreport_model m = CreateModel(dr);
                    dic.Add(m.serverid, m);
                }
                return dic;
            });
        }

        public tb_performance_dayreport_model GetStaticMonitor(DbConn PubConn, string day, int serverid, int startid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("serverid", serverid);
                ps.Add("startid", startid);
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("SELECT serverid ");
                sbSelect.Append(",AVG(cpu) avgcpu,MAX(cpu) maxcpu,MIN(cpu) mincpu ");
                sbSelect.Append(",AVG(memory) avgmemory,MAX(memory) maxmemory,MIN(memory) minmemory ");
                sbSelect.Append(",AVG(networkupload) avgnetworkupload,MAX(networkupload) maxnetworkupload,MIN(networkupload) minnetworkupload ");
                sbSelect.Append(",AVG(networkdownload) avgnetworkdownload,MAX(networkdownload) maxnetworkdownload,MIN(networkdownload) minnetworkdownload ");
                sbSelect.Append(",AVG(ioread) avgioread,MAX(ioread) maxioread,MIN(ioread) minioread ");
                sbSelect.Append(",AVG(iowrite) avgiowrite,MAX(iowrite) maxiowrite,MIN(iowrite) miniowrite ");
                sbSelect.Append(",AVG(iisrequest) avgiisrequest,MAX(iisrequest) maxiisrequest,MIN(iisrequest) miniisrequest ");
                sbSelect.Append(",MAX(ID) lastmaxid,count(1) count ");
                sbSelect.AppendFormat("FROM tb_performance_collect{0} with (nolock) ", day);
                sbSelect.AppendFormat("where serverid=@serverid and id>@startid ");
                sbSelect.Append("group by serverid ");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sbSelect.ToString(), ps.ToParameters());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tb_performance_dayreport_model model = CreateModel(ds.Tables[0].Rows[0]);
                    return model;
                }
                else
                {
                    return null;
                }
            });
        }

        public int Update(DbConn PubConn, tb_performance_dayreport_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", model.id);
                ps.Add("serverid", model.serverid);
                ps.Add("avgcpu", model.avgcpu);
                ps.Add("maxcpu", model.maxcpu);
                ps.Add("mincpu", model.mincpu);
                ps.Add("avgmemory", model.avgmemory);
                ps.Add("maxmemory", model.maxmemory);
                ps.Add("minmemory", model.minmemory);
                ps.Add("avgnetworkupload", model.avgnetworkupload);
                ps.Add("maxnetworkupload", model.maxnetworkupload);
                ps.Add("minnetworkupload", model.minnetworkupload);
                ps.Add("avgnetworkdownload", model.avgnetworkdownload);
                ps.Add("maxnetworkdownload", model.maxnetworkdownload);
                ps.Add("minnetworkdownload", model.minnetworkdownload);
                ps.Add("avgioread", model.avgioread);
                ps.Add("maxioread", model.maxioread);
                ps.Add("minioread", model.minioread);
                ps.Add("avgiowrite", model.avgiowrite);
                ps.Add("maxiowrite", model.maxiowrite);
                ps.Add("miniowrite", model.miniowrite);
                ps.Add("avgiisrequest", model.avgiisrequest);
                ps.Add("maxiisrequest", model.maxiisrequest);
                ps.Add("miniisrequest", model.miniisrequest);
                ps.Add("lastmaxid", model.lastmaxid);
                ps.Add("lastupdatetime", model.lastupdatetime);
                ps.Add("count", model.count);
                string sql = @"update tb_performance_dayreport set 
                serverid=@serverid,avgcpu=@avgcpu,maxcpu=@maxcpu,
                mincpu=@mincpu,avgmemory=@avgmemory,maxmemory=@maxmemory,
                minmemory=@minmemory,avgnetworkupload=@avgnetworkupload,
                maxnetworkupload=@maxnetworkupload,minnetworkupload=@minnetworkupload,
                avgnetworkdownload=@avgnetworkdownload,maxnetworkdownload=@maxnetworkdownload,
                minnetworkdownload=@minnetworkdownload,avgioread=@avgioread,maxioread=@maxioread,
                minioread=@minioread,avgiowrite=@avgiowrite,maxiowrite=@maxiowrite,miniowrite=@miniowrite,
                lastmaxid=@lastmaxid,lastupdatetime=@lastupdatetime,avgiisrequest=@avgiisrequest,
                maxiisrequest=@maxiisrequest,miniisrequest=@miniisrequest,count=@count where id=@id";
                int rev = PubConn.ExecuteSql(sql, ps.ToParameters());
                return rev;
            });
        }

        public Dictionary<DateTime, double> GetTimeChart(DbConn PubConn, string column, DateTime datebegin, DateTime dateend, string serverip, string serverid, string datatype)
        {
            return SqlHelper.Visit(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(serverip))
                {
                    ps.Add("serverip", serverip);
                    sqlwhere += " and c.serverip=@serverip";
                }
                if (!string.IsNullOrWhiteSpace(serverid))
                {
                    ps.Add("serverid", serverid);
                    sqlwhere += " and p.serverid=@serverid";
                }
                ps.Add("datebegin", datebegin);
                sqlwhere += " and p.date>=@datebegin";
                ps.Add("dateend", dateend);
                sqlwhere += " and p.date<=@dateend";
                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                string sql = string.Format(@"select p.date,p.{3}{2} as avgnum from tb_performance_dayreport p with (nolock) right join [dyd_bs_monitor_platform_manage].dbo.tb_cluster c with (nolock)
                                             on c.id=p.serverid {1} ",
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


        public virtual bool AddStatis(DbConn PubConn, tb_performance_dayreport_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					//服务器id
					new ProcedureParameter("@serverid",    model.serverid),
					//平均cpu
					new ProcedureParameter("@avgcpu",    model.avgcpu),
					//最大cpu
					new ProcedureParameter("@maxcpu",    model.maxcpu),
					//
					new ProcedureParameter("@mincpu",    model.mincpu),
					//
					new ProcedureParameter("@avgmemory",    model.avgmemory),
					//
					new ProcedureParameter("@maxmemory",    model.maxmemory),
					//
					new ProcedureParameter("@minmemory",    model.minmemory),
					//
					new ProcedureParameter("@avgnetworkupload",    model.avgnetworkupload),
					//
					new ProcedureParameter("@maxnetworkupload",    model.maxnetworkupload),
					//
					new ProcedureParameter("@minnetworkupload",    model.minnetworkupload),
					//
					new ProcedureParameter("@avgnetworkdownload",    model.avgnetworkdownload),
					//
					new ProcedureParameter("@maxnetworkdownload",    model.maxnetworkdownload),
					//
					new ProcedureParameter("@minnetworkdownload",    model.minnetworkdownload),
					//
					new ProcedureParameter("@avgioread",    model.avgioread),
					//
					new ProcedureParameter("@maxioread",    model.maxioread),
					//
					new ProcedureParameter("@minioread",    model.minioread),
					//
					new ProcedureParameter("@avgiowrite",    model.avgiowrite),
					//
					new ProcedureParameter("@maxiowrite",    model.maxiowrite),
					//
					new ProcedureParameter("@miniowrite",    model.miniowrite),
                    //
					new ProcedureParameter("@avgiisrequest",    model.avgiisrequest),
					//
					new ProcedureParameter("@maxiisrequest",    model.maxiisrequest),
					//
					new ProcedureParameter("@miniisrequest",    model.miniisrequest),
					//上次扫描最大id
					new ProcedureParameter("@lastmaxid",    model.lastmaxid),
                    //
                    new ProcedureParameter("@count", model.count)
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_performance_dayreport(date,serverid,avgcpu,maxcpu,mincpu,avgmemory,maxmemory,minmemory,avgnetworkupload,maxnetworkupload,minnetworkupload,avgnetworkdownload,maxnetworkdownload,minnetworkdownload,avgioread,maxioread,minioread,avgiowrite,maxiowrite,miniowrite,lastmaxid,lastupdatetime,avgiisrequest,maxiisrequest,miniisrequest,count)
										   values(getdate(),@serverid,@avgcpu,@maxcpu,@mincpu,@avgmemory,@maxmemory,@minmemory,@avgnetworkupload,@maxnetworkupload,@minnetworkupload,@avgnetworkdownload,@maxnetworkdownload,@minnetworkdownload,@avgioread,@maxioread,@minioread,@avgiowrite,@maxiowrite,@miniowrite,@lastmaxid,getdate(),@avgiisrequest,@maxiisrequest,@miniisrequest,@count)", Par);
            return rev == 1;
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
                string sql = "select date,AVG(" + key + ") Cvalue from tb_performance_dayreport where date=@Fday or date=@Nday group by date";
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
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_performance_dayreport with (nolock) where date=@Fday", ps.ToParameters()));
                StringBuilder sb_Sql_Select = new StringBuilder();
                sb_Sql_Select.Append("select Tkey,Avalue,Bvalue,AsubB,rownum from (");
                sb_Sql_Select.Append("select ROW_NUMBER() over(order by ISNULL(B." + key + ",0)-ISNULL(A." + key + ",0) desc) as rownum,A.serverid Tkey,ISNULL(A." + key + ",0) Avalue,ISNULL(B." + key + ",0) Bvalue,ISNULL(B." + key + ",0)-ISNULL(A." + key + ",0) AS AsubB from ");
                sb_Sql_Select.Append("(select serverid," + key + " from tb_performance_dayreport with (nolock) where date=@Fday) A ");
                sb_Sql_Select.Append(" left join ");
                sb_Sql_Select.Append("(select serverid," + key + " from tb_performance_dayreport with (nolock) where date=@Nday) B ");
                sb_Sql_Select.Append(" on A.serverid=B.serverid) Temp ");
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
    }
}