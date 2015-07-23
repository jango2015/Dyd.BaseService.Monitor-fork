using Dyd.BaseService.Monitor.Domain.UnityLog.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.UnityLog.Dal
{
    public partial class tb_log_dal
    {
        public List<tb_log_model> GetList(DbConn PubConn,string id, string keyword, DateTime timebegin,DateTime timeend,int logtype,string projectname, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_log_model> model = new List<tb_log_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ps.Add("id", id);
                    sqlwhere += " and (id=@id) ";
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and (msg like '%'+@keyword+'%') ";
                }
                if (!string.IsNullOrWhiteSpace(projectname))
                {
                    ps.Add("projectname", projectname);
                    sqlwhere += " and (projectname=@projectname) ";
                }
                if (true)
                {
                    ps.Add("timebegin", timebegin); ps.Add("timeend", timeend);
                    sqlwhere += " and  sqlservercreatetime>=@timebegin and sqlservercreatetime<=@timeend";
                }
                if (logtype >= 0)
                {
                    ps.Add("logtype", logtype);
                    sqlwhere += " and  logtype=@logtype";
                }
                string zq = "";
                DateTime dateNow=DateTime.Now;
                if (DateTime.TryParse(timeend.ToString("yyyyMM"), out dateNow))
                {
                    zq = timeend.ToString("yyyyMM");
                }
                else
                {
                    zq = DateTime.Now.ToString("yyyyMM");
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by id desc) as rownum,* from tb_log" + zq + " with (nolock) ");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_log" + zq + " with (nolock) " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_log_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }
    }
}
