using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using XXF.ProjectTool;
using XXF.Db;
using System.Data;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.BaseService.Monitor.SystemRuntime;
using XXF.Common;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Dal
{
    public partial class tb_sqlhash_consult_dal
    {
        public List<tb_sqlhash_consult_model> GetList(DbConn PubConn, string sqlhash, string sqlcontent, string orderby , int extime, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_sqlhash_consult_model> model = new List<tb_sqlhash_consult_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (extime != 0)
                {
                    ps.Add("extime", extime);
                    sqlwhere += " and count>@extime ";
                }
                if (!string.IsNullOrWhiteSpace(sqlhash))
                {
                    ps.Add("sqlhash", sqlhash);
                    sqlwhere += " and sqlhash = @sqlhash ";
                }
                if (!string.IsNullOrWhiteSpace(sqlcontent))
                {
                    ps.Add("sqlcontent", sqlcontent);
                    sqlwhere += " and (sql like '%'+@sqlcontent+'%') ";
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by " + orderby + ") as rownum,* from tb_sqlhash_consult with (nolock)");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_sqlhash_consult with (nolock) " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_sqlhash_consult_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public int DeleteAll(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "delete from tb_sqlhash_consult";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }

        public List<tb_sqlhash_consult_model> Contrast(DbConn PubConn, string day)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "select count(1) count,logtag as sqlhash,msg as sql from tb_timewatchlog" + day +" with (nolock) where logtype=3 group by logtag,msg";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, null);
                List<tb_sqlhash_consult_model> model = new List<tb_sqlhash_consult_model>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_sqlhash_consult_model m = CreateModel(dr);
                    model.Add(m);
                }
                return model;
            });
        }

        public bool SqlHashContrast(int sqljudgmentbase)
        {
            string day = "";
            tb_sqlhash_consult_dal condal = new tb_sqlhash_consult_dal();
            tb_sqlhash_noparam_dal nodal = new tb_sqlhash_noparam_dal();
            List<string> sqlConnList = new List<string>();
            using (DbConn PubConn = DbConfig.CreateConn(XXFConfig.MonitorPlatformConnectionString))
            {
                PubConn.Open();
                day = PubConn.GetServerDate().ToString("yyyyMMdd");
                sqlConnList = new tb_database_config_dal().GetDataBaseSqlConnList(PubConn, (int)DataBaseType.Timewatch);
            }
            foreach (string conn in sqlConnList)
            {
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    condal.DeleteAll(PubConn);
                    nodal.DeleteAll(PubConn);
                    List<tb_sqlhash_consult_model> sqlHashList = Contrast(PubConn, day);
                    foreach (tb_sqlhash_consult_model m in sqlHashList)
                    {
                        if (m.count > sqljudgmentbase)
                        {
                            condal.Add(PubConn, m);
                        }
                        else
                        {
                            tb_sqlhash_noparam_model nomodel = new tb_sqlhash_noparam_model()
                            {
                                sql = m.sql,
                                sqlhash = m.sqlhash,
                                count = m.count,
                            };
                            nodal.Add(PubConn, nomodel);
                        }
                    }
                }
            }
            return true;
        }

        public List<tb_sqlhash_consult_model> GetChartJson(DbConn PubConn, int count)
        {
            return SqlHelper.Visit(ps =>
            {
                List<tb_sqlhash_consult_model> model = new List<tb_sqlhash_consult_model>();
                ps.Add("count", count);
                string sql = "select * from tb_sqlhash_consult where count>@count";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_sqlhash_consult_model m = CreateModel(dr);
                    m.nodenotes = m.sqlhash.ToString();
                    model.Add(m);
                }
                return model;
            });
        }
    }
}
