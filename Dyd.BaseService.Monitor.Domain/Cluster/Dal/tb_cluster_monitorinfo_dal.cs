using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Dal
{
	public partial class tb_cluster_monitorinfo_dal
    {
        public void AddOrUpdate(DbConn PubConn, tb_cluster_monitorinfo_model model)
        {
            SqlHelper.Visit(ps =>
            {
                ps.Add("@monitorinfojson", model.monitorinfojson);
                ps.Add("@lastupdatetime", model.lastupdatetime);
                ps.Add("@serverid", model.serverid);

                string updatecmd = "update tb_cluster_monitorinfo set monitorinfojson=@monitorinfojson,lastupdatetime=@lastupdatetime where serverid=@serverid";
                string insertcmd = @"insert into tb_cluster_monitorinfo(serverid,monitorinfojson,lastupdatetime)
										   values(@serverid,@monitorinfojson,@lastupdatetime)";
                if (PubConn.ExecuteSql(updatecmd, ps.ToParameters()) <= 0)
                {
                    PubConn.ExecuteSql(insertcmd, ps.ToParameters());
                }
                return 1;
            });
        }

        public tb_cluster_monitorinfo_model GetByServerId(DbConn PubConn, int serverid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@serverid", serverid);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select s.* from tb_cluster_monitorinfo s where s.serverid=@serverid");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return CreateModel(ds.Tables[0].Rows[0]);
                }
                return null;
            });
        }

        public string GetServerMonitorJson(DbConn PubConn, int serverid)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@serverid", serverid);
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select monitorinfojson from tb_cluster_monitorinfo s where s.serverid=@serverid");
                string json = LibConvert.ObjToStr(PubConn.ExecuteScalar(stringSql.Tostring(), ps.ToParameters()));
                return json;
            });
        }

        public List<tb_cluster_monitorinfo_model> GetList(DbConn PubConn, string keyword, string serverid,string monitorjsonkeyword, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_cluster_monitorinfo_model> model = new List<tb_cluster_monitorinfo_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " and 1=1 ";
                DateTime date = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(serverid))
                {
                    ps.Add("serverid", serverid);
                    sqlwhere += " and M.serverid = @serverid ";
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and (C.serverip like '%'+@keyword+'%' or C.servername like '%'+@keyword+'%') ";
                } if (!string.IsNullOrWhiteSpace(monitorjsonkeyword))
                {
                    ps.Add("monitorjsonkeyword", monitorjsonkeyword);
                    sqlwhere += " and (M.monitorinfojson like '%'+@monitorjsonkeyword+'%') ";
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by M.id desc) as rownum,M.*,C.serverip,C.servername from tb_cluster_monitorinfo M with (nolock), [dyd_bs_monitor_platform_manage].[dbo].[tb_cluster] C with (nolock) where M.serverid=C.id ");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_cluster_monitorinfo M left join [dyd_bs_monitor_platform_manage].[dbo].[tb_cluster] C on M.serverid=C.id where 1=1 " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_cluster_monitorinfo_model m = CreateModel(dr);
                m.serverip = dr["serverip"].Tostring();
                m.servername = dr["servername"].Tostring();
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public int ReseverdServerMonitor(DbConn PubConn)
        {
            return SqlHelper.Visit(ps => 
            {
                string sql = @"insert into tb_cluster_monitorinfo_snapshot (serverid,monitorinfojson,lastupdatetime,createtime)
                            select M.serverid,M.monitorinfojson,M.lastupdatetime,getdate() createtime from tb_cluster_monitorinfo M with (nolock) left join [dyd_bs_monitor_platform_manage].[dbo].[tb_cluster] C with (nolock) on M.serverid=C.id where ifmonitor = 1";
                ps.Add("overtime",DateTime.Now.AddMonths(-1));
                string sqlDelete = "delete from tb_cluster_monitorinfo_snapshot where createtime<@overtime ";
                PubConn.ExecuteSql(sqlDelete, ps.ToParameters());
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }
    }
}