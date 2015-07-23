using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XXF.Db;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Dal
{
    public partial class tb_user_dal
    {
        public List<tb_user_model> GetAllUsers(DbConn PubConn)
        {
            return SqlHelper.Visit(ps =>
            {
                StringBuilder stringSql = new StringBuilder();
                stringSql.Append(@"select * from tb_user s order by id desc");
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, stringSql.ToString(), ps.ToParameters());
                List<tb_user_model> rs = new List<tb_user_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        rs.Add(CreateModel(dr));
                    }
                }
                return rs;
            });
        }

        public tb_user_model GetUserName(DbConn PubConn, string userstaffno)
        {
            return SqlHelper.Visit(ps =>
            {
                ps.Add("userstaffno", userstaffno);
                string sql = "select id,username,userrole from tb_user where userstaffno=@userstaffno";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, sql, ps.ToParameters());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tb_user_model m = CreateModel(ds.Tables[0].Rows[0]);
                    return m;
                }
                else
                    return null;
            });
        }

        public void UpdateLastErrorSendTime(DbConn PubConn, int userid,DateTime lasterrorsendtime)
        {
             SqlHelper.Visit(ps =>
            {
                ps.Add("@lasterrorsendtime", lasterrorsendtime); ps.Add("@id", userid);
                string sql = "update  tb_user set lastsenderrortime=@lasterrorsendtime where id=@id";
                return PubConn.ExecuteSql(sql, ps.ToParameters());
            });
        }
    }
}
