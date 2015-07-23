using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Dal
{
    public partial class tb_cluster_monitorinfo_snapshot_dal
    {
        public List<tb_cluster_monitorinfo_snapshot_model> GetList(DbConn PubConn, string keyword, string serverid, string timebegin, string timeend,string monitorjsonkeyword, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_cluster_monitorinfo_snapshot_model> model = new List<tb_cluster_monitorinfo_snapshot_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
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
                    sqlwhere += " and (M.monitorinfojson like '%'+@monitorjsonkeyword+'%')";
                }
                if (!string.IsNullOrWhiteSpace(timebegin))
                {
                    ps.Add("timebegin", timebegin);
                    sqlwhere += " and M.lastupdatetime >= @timebegin ";
                }
                if (!string.IsNullOrWhiteSpace(timeend))
                {
                    ps.Add("timeend", timeend);
                    sqlwhere += " and M.lastupdatetime <= @timeend ";
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by M.id desc) as rownum,M.*,C.serverip,C.servername from tb_cluster_monitorinfo_snapshot M with (nolock) left join [dyd_bs_monitor_platform_manage].[dbo].[tb_cluster] C with (nolock) on M.serverid=C.id ");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_cluster_monitorinfo_snapshot M with (nolock) left join [dyd_bs_monitor_platform_manage].[dbo].[tb_cluster] C with (nolock) on M.serverid=C.id " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_cluster_monitorinfo_snapshot_model m = CreateModel(dr);
                m.serverip = dr["serverip"].ToString();
                m.servername = dr["servername"].ToString();
                model.Add(m);
            }
            count = _count;
            return model;
        }
    }
}
