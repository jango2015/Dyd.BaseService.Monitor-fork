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
    public partial class tb_cluster_collect_version_dal
    {
        public tb_cluster_collect_version_model GetCurrentVersion(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "  select top 1 * from tb_cluster_collect_version where versionnum=(select max(versionnum) from tb_cluster_collect_version)";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return CreateModel(ds.Tables[0].Rows[0]);
                }
                return null;
            });
        }

        public int GetMaxVersionNumber(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "select max(versionnum) as maxversionnum from tb_cluster_collect_version";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                return 0;
            });
        }

        public List<tb_cluster_collect_version_model> GetList(DbConn PubConn, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_cluster_collect_version_model> model = new List<tb_cluster_collect_version_model>();
            try
            {
                DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
                {
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select ROW_NUMBER() over(order by id desc) as rownum,* from tb_cluster_collect_version ");
                    _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_cluster_collect_version ", ps.ToParameters()));
                    DataSet ds = new DataSet();
                    string sqlSel = "select * from (" + sql + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                    PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                    return ds;
                });
                foreach (DataRow dr in dsList.Tables[0].Rows)
                {
                    tb_cluster_collect_version_model m = CreateModel(dr);
                    model.Add(m);
                }
            }
            catch (Exception exp)
            { }
            count = _count;
            return model;
        }

        public int Update(DbConn PubConn, tb_cluster_collect_version_model model)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", model.id);
                ps.Add("versionnum", model.versionnum);
                ps.Add("zipfilename", model.zipfilename);
                ps.Add("zipfile", model.zipfile);
                string sql = "update tb_cluster_collect_version set versionnum=@versionnum,versioncreatetime=getdate(),zipfilename=@zipfilename,zipfile=@zipfile where id=@id";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }

        public int GetMaxVersion(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "select ISNULL(max(versionnum),0)+1 from tb_cluster_collect_version ";
                int maxVersion = Convert.ToInt32(PubConn.ExecuteScalar(sql, ps.ToParameters()));
                return maxVersion;
            });
        }

        public int ChangePerformVersion(DbConn PubConn, int id)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("id", id);
                string sql = "update tb_cluster_collect_version set versionnum=(select max(versionnum)+1 from tb_cluster_collect_version) where id=@id";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }
    }
}