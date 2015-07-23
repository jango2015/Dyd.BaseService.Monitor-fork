using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Dal
{
    public partial class tb_sqlhash_noparam_dal
    {
        public List<tb_sqlhash_noparam_model> GetList(DbConn PubConn, string sqlhash, string sqlcontent, string orderby ,int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_sqlhash_noparam_model> model = new List<tb_sqlhash_noparam_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
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
                sql.Append("select ROW_NUMBER() over(order by " + orderby + ") as rownum,* from tb_sqlhash_noparam with (nolock) " + sqlwhere);
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_sqlhash_noparam with (nolock) " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_sqlhash_noparam_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public int DeleteAll(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "delete from tb_sqlhash_noparam";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }

        public List<tb_sqlhash_noparam_model> GetChartJson(DbConn PubConn, int count)
        {
            return SqlHelper.Visit(ps =>
            {
                List<tb_sqlhash_noparam_model> model = new List<tb_sqlhash_noparam_model>();
                ps.Add("count", count);
                string sql = "select * from tb_sqlhash_noparam where count=@count";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tb_sqlhash_noparam_model m = CreateModel(dr);
                    m.nodenotes = m.sqlhash.ToString();
                    model.Add(m);
                }
                return model;
            });
        }
    }
}
