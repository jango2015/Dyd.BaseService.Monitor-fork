using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Dal
{
    public partial class tb_cluster_dal
    {
        public tb_cluster_model Get(DbConn PubConn, string serverip)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@serverip", serverip);
                string cmd = "select top 1 * from tb_cluster where serverip=@serverip";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return CreateModel(ds.Tables[0].Rows[0]);
                }
                return null;
            });
        }

        public DateTime GetLastUpdateTime(DbConn PubConn, string serverip)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@serverip", serverip);
                string cmd = "select top 1 lastupdatetime from tb_cluster where serverip=@serverip";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToDateTime(ds.Tables[0].Rows[0][0]);
                }
                return DateTime.Parse("1900-01-01");
            });
        }
        public tb_cluster_model GetNewByServerip(DbConn PubConn, string serverip, DateTime lastupdatetime)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@serverip", serverip);
                ps.Add("@lastupdatetime", lastupdatetime);
                string cmd = "select top 1 * from tb_cluster where serverip=@serverip and lastupdatetime>@lastupdatetime";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return CreateModel(ds.Tables[0].Rows[0]);
                }
                return null;
            });
        }

        public List<tb_cluster_model> GetAllList(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "select * from tb_cluster";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                var rs = new List<tb_cluster_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        rs.Add(CreateModel(dr));
                }
                return rs;
            });
        }

        public List<tb_cluster_model> GetListInIds(DbConn PubConn, List<int> ids)
        {
            return SqlHelper.Visit(ps =>
            {
                if (ids.Count == 0)
                    return new List<tb_cluster_model>();
                string cmd = string.Format("select * from tb_cluster where id in ({0})",string.Join(",",ids.ToArray()).TrimEnd(','));
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                var rs = new List<tb_cluster_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        rs.Add(CreateModel(dr));
                }
                return rs;
            });
        }

        public List<tb_cluster_model> GetList(DbConn PubConn,string id, string keyword, string CStime, string CEtime, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_cluster_model> model = new List<tb_cluster_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by id desc) as rownum,* from tb_cluster ");
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and ( servername like '%'+@keyword+'%' or serverip like '%'+@keyword+'%' )";
                }
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ps.Add("id", id);
                    sqlwhere += " and ( id=@id )";
                }
                DateTime d = DateTime.Now;
                if (DateTime.TryParse(CStime, out d))
                {
                    ps.Add("CStime", Convert.ToDateTime(CStime));
                    sqlwhere += " and createtime>=@CStime";
                }
                if (DateTime.TryParse(CEtime, out d))
                {
                    ps.Add("CEtime", Convert.ToDateTime(CEtime));
                    sqlwhere += " and createtime<=@CEtime";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_cluster " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_cluster_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public string GetServerName(DbConn PubConn, string serverip, string serverid)
        {
            return SqlHelper.Visit(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(serverip))
                {
                    ps.Add("serverip", serverip);
                    sqlwhere += " and serverip=@serverip ";
                }
                else
                {
                    ps.Add("serverid", serverid);
                    sqlwhere += " and id=@serverid ";
                }
                string sql = "select servername from tb_cluster " + sqlwhere;
                return PubConn.ExecuteScalar(sql, ps.ToParameters()).Tostring();
            });
        }

        public bool CheckServerIp(DbConn PubConn, string serverip)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("serverip", serverip);
                string sql = "select count(1) from tb_cluster where serverip=@serverip";
                int count = Convert.ToInt32(PubConn.ExecuteScalar(sql, ps.ToParameters()));
                return count == 0;
            });
        }

        public bool CheckServerIp(DbConn PubConn, string serverip,int id)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("serverip", serverip);
                ps.Add("id", id);
                string sql = "select count(1) from tb_cluster where serverip=@serverip and id!=@id";
                int count = Convert.ToInt32(PubConn.ExecuteScalar(sql, ps.ToParameters()));
                return count == 0;
            });
        }
        public int Update(DbConn PubConn, tb_cluster_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", model.id);
                ps.Add("servername", model.servername);
                ps.Add("serverip", model.serverip);
                ps.Add("ifmonitor", model.ifmonitor);
                ps.Add("performancecollectconfigjson", model.performancecollectconfigjson);
                ps.Add("monitorcollectconfigjson", model.monitorcollectconfigjson);
                ps.Add("onlinetime", model.onlinetime);
                string sql = "update tb_cluster set servername=@servername,serverip=@serverip,ifmonitor=@ifmonitor,performancecollectconfigjson=@performancecollectconfigjson,";
                sql += "monitorcollectconfigjson=@monitorcollectconfigjson,lastupdatetime=getdate(),onlinetime=@onlinetime where id=@id";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }

        public int UpdateOnLineTime(DbConn PubConn, string serverip)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("serverip", serverip);
                string sql = "update tb_cluster set onlinetime=getdate() where serverip=@serverip";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }
    }
}