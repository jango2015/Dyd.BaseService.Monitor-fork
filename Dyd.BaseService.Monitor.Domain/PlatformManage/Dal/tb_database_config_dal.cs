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
	public partial class tb_database_config_dal
    {
        public List<tb_database_config_model> GetModelList(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                string cmd = "select * from tb_database_config";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                List<tb_database_config_model> rs = new List<tb_database_config_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        rs.Add(CreateModel(dr));
                }
                return rs;
            });
        }

        public List<tb_database_config_model> GetList(DbConn PubConn, string keyword, int dbtype, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_database_config_model> model = new List<tb_database_config_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by id desc) as rownum,* from tb_database_config ");
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and ( dblocalname like '%'+@keyword+'%' or dbname like '%'+@keyword+'%' or dbserver like '%'+@keyword+'%' )";
                }
                if (dbtype != -1)
                {
                    ps.Add("dbtype", dbtype);
                    sqlwhere += " and dbtype=@dbtype ";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_database_config " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_database_config_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }

        public List<string> GetDataBaseSqlConnList(DbConn PubConn, int dbtype)
        {
            return SqlHelper.Visit(ps =>
            {
                List<string> connLisg = new List<string>();
                ps.Add("dbtype", dbtype);
                string sql = "select * from tb_database_config where dbtype=@dbtype";
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        tb_database_config_model m = CreateModel(dr);
                        string sqlconn = ReplaceConnectStringTemplate(m);
                        connLisg.Add(sqlconn);
                    }
                    return connLisg;
                }
                else
                {
                    return connLisg;
                }
            });
        }

        public Dictionary<int,timewatchConnModel> GetDataBaseSqlConnDic(DbConn PubConn, int dbtype)
        {
            return SqlHelper.Visit(ps =>
            {
                Dictionary<int, timewatchConnModel> connDic = new Dictionary<int, timewatchConnModel>();
                ps.Add("dbtype", dbtype);
                string sql = "select * from tb_database_config where dbtype=@dbtype";
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        tb_database_config_model m = CreateModel(dr);
                        string sqlconn = ReplaceConnectStringTemplate(m);
                        timewatchConnModel model = new timewatchConnModel()
                        {
                            name = m.dblocalname,
                            connect = sqlconn
                        };
                        connDic.Add(m.id, model);
                    }
                    return connDic;
                }
                else
                {
                    return connDic;
                }
            });
        }

        public string GetDataBaseSqlConn(DbConn PubConn, int dbtype)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("dbtype", dbtype);
                string sql = "select * from tb_database_config where dbtype=@dbtype";
                DataTable dt = PubConn.SqlToDataTable(sql, ps.ToParameters());
                if (dt.Rows.Count > 0)
                {
                    tb_database_config_model m = CreateModel(dt.Rows[0]);
                    string sqlconn = ReplaceConnectStringTemplate(m);
                    return sqlconn;
                }
                else
                {
                    return "";
                }
            });
        }

        /// <summary>
        /// 根据模板替换连接字符串
        /// </summary>
        /// <param name="connectstringTemplate"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public string ReplaceConnectStringTemplate(tb_database_config_model o)
        {
            if (o == null)
                throw new Exception("找不到数据库连接分库配置");
            string connectstringTempLate = "server={dbserver};Initial Catalog={dbname};User ID={dbuser};Password={dbpass};";
            return connectstringTempLate.Replace("{dbserver}", o.dbserver).Replace("{dbname}", o.dbname)
                            .Replace("{dbuser}", o.dbuser).Replace("{dbpass}", o.dbpass);
        }
    }
}