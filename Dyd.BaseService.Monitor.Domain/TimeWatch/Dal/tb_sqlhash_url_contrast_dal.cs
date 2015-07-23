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
    public partial class tb_sqlhash_url_contrast_dal
    {
        public List<tb_sqlhash_url_contrast_model> GetList(DbConn PubConn, string sqlhash, string sqlcontent, string url, string orderby, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_sqlhash_url_contrast_model> model = new List<tb_sqlhash_url_contrast_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(sqlhash))
                {
                    ps.Add("sqlhash", sqlhash);
                    sqlwhere += " and C.sqlhash = @sqlhash ";
                }
                if (!string.IsNullOrWhiteSpace(url))
                {
                    ps.Add("url", url);
                    sqlwhere += " and C.url=@url";
                }
                if (!string.IsNullOrWhiteSpace(sqlcontent))
                {
                    ps.Add("sqlcontent", sqlcontent);
                    sqlwhere += " and (E.sql like '%'+@sqlcontent+'%') ";
                }
                if (string.IsNullOrWhiteSpace(orderby))
                    orderby = "C.url";
                string sql = @"select ROW_NUMBER() over(order by " + orderby + @" desc) as rownum,C.*,ISNULL(D.avgtime,0) avgtime,ISNULL(D.maxtime,0) maxtime,ISNULL(D.mintime,0) mintime,ISNULL(D.date,getdate()) date,E.sql,ISNULL(D.count,0) count from tb_sqlhash_url_contrast C with (nolock) 
                left hash join 
                (select * from tb_timewatchlog_sql_dayreport with (nolock)
                where id in
                (select MAX(id) from tb_timewatchlog_sql_dayreport with (nolock) group by sqlhash)) D 
                on C.sqlhash=D.sqlhash
                LEFT HASH JOIN
                tb_sqlhash_consult E
                on C.sqlhash=E.sqlhash ";
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from (" + sql + sqlwhere + ") A", ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_sqlhash_url_contrast_model m = CreateModel(dr);
                m.avgtime = Convert.ToDecimal(dr["avgtime"]);
                m.maxtime = Convert.ToDecimal(dr["maxtime"]);
                m.mintime = Convert.ToDecimal(dr["mintime"]);
                m.date = Convert.ToDateTime(dr["date"]);
                m.sql = dr["sql"].ToString();
                m.count = Convert.ToInt32(dr["count"]);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public bool GenerationTask()
        {
            string day = "";
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
                    DeleteAll(PubConn);
                    Generation(PubConn, day);
                }
            }
            return true;
        }

        public int DeleteAll(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                return PubConn.ExecuteSql("delete from tb_sqlhash_url_contrast", ps.ToParameters());
            });
        }

        public int Generation(DbConn PubConn,string day)
        {
            return SqlHelper.Visit(ps =>
            {
                string sql = "insert into tb_sqlhash_url_contrast select logtag as sqlhash,url from tb_timewatchlog" + day + " where logtype=3 group by logtag,url";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }
    }
}
