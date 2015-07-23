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
	public partial class tb_keyvalue_config_dal
    {
        public tb_keyvalue_config_model Get(DbConn PubConn,string key)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("@key",key);
                string cmd = "select * from tb_keyvalue_config where [key]=@key";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return CreateModel(ds.Tables[0].Rows[0]);
                }
                return null;
            });
        }

        public Dictionary<string,string> GetAll(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                Dictionary<string, string> configDic = new Dictionary<string, string>();
                string cmd = "select * from tb_keyvalue_config";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<tb_keyvalue_config_model> model = new List<tb_keyvalue_config_model>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        configDic.Add(dr["key"].ToString(), dr["value"].ToString());
                    }
                }
                return configDic;
            });
        }

        public List<tb_keyvalue_config_model> GetList(DbConn PubConn, string keyword, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_keyvalue_config_model> model = new List<tb_keyvalue_config_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by id desc) as rownum,* from tb_keyvalue_config ");
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and ( [key] like '%'+@keyword+'%' or remark like '%'+@keyword+'%' )";
                }
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_keyvalue_config " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_keyvalue_config_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }
    }
}