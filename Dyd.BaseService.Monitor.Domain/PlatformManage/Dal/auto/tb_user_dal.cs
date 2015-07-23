using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;

namespace Dyd.BaseService.Monitor.Domain.PlatformManage.Dal
{
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
	public partial class tb_user_dal
    {
        public virtual bool Add(DbConn PubConn, tb_user_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//用户工号
					new ProcedureParameter("@userstaffno",    model.userstaffno),
					//用户名
					new ProcedureParameter("@username",    model.username),
					//用户角色
					new ProcedureParameter("@userrole",    model.userrole),
					//用户创建时间
					new ProcedureParameter("@usercreatetime",    model.usercreatetime),
					//用户手机号码
					new ProcedureParameter("@usertel",    model.usertel),
					//用户email
					new ProcedureParameter("@useremail",    model.useremail),
					//上一次错误发送时间
					new ProcedureParameter("@lastsenderrortime",    model.lastsenderrortime),
					//错误发送时间间隔（单位:分钟）
					new ProcedureParameter("@errorsendintervaltime",    model.errorsendintervaltime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_user(userstaffno,username,userrole,usercreatetime,usertel,useremail,lastsenderrortime,errorsendintervaltime)
										   values(@userstaffno,@username,@userrole,@usercreatetime,@usertel,@useremail,@lastsenderrortime,@errorsendintervaltime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_user_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//用户工号
					new ProcedureParameter("@userstaffno",    model.userstaffno),
					//用户名
					new ProcedureParameter("@username",    model.username),
					//用户角色
					new ProcedureParameter("@userrole",    model.userrole),
					//用户创建时间
					new ProcedureParameter("@usercreatetime",    model.usercreatetime),
					//用户手机号码
					new ProcedureParameter("@usertel",    model.usertel),
					//用户email
					new ProcedureParameter("@useremail",    model.useremail),
					//上一次错误发送时间
					new ProcedureParameter("@lastsenderrortime",    model.lastsenderrortime),
					//错误发送时间间隔（单位:分钟）
					new ProcedureParameter("@errorsendintervaltime",    model.errorsendintervaltime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_user set userstaffno=@userstaffno,username=@username,userrole=@userrole,usercreatetime=@usercreatetime,usertel=@usertel,useremail=@useremail,lastsenderrortime=@lastsenderrortime,errorsendintervaltime=@errorsendintervaltime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_user where id=@id";
            int rev = PubConn.ExecuteSql(Sql, Par);
            if (rev == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual tb_user_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_user s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_user_model CreateModel(DataRow dr)
        {
            var o = new tb_user_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//用户工号
			if(dr.Table.Columns.Contains("userstaffno"))
			{
				o.userstaffno = dr["userstaffno"].Tostring();
			}
			//用户名
			if(dr.Table.Columns.Contains("username"))
			{
				o.username = dr["username"].Tostring();
			}
			//用户角色
			if(dr.Table.Columns.Contains("userrole"))
			{
				o.userrole = dr["userrole"].ToByte();
			}
			//用户创建时间
			if(dr.Table.Columns.Contains("usercreatetime"))
			{
				o.usercreatetime = dr["usercreatetime"].ToDateTime();
			}
			//用户手机号码
			if(dr.Table.Columns.Contains("usertel"))
			{
				o.usertel = dr["usertel"].Tostring();
			}
			//用户email
			if(dr.Table.Columns.Contains("useremail"))
			{
				o.useremail = dr["useremail"].Tostring();
			}
			//上一次错误发送时间
			if(dr.Table.Columns.Contains("lastsenderrortime"))
			{
				o.lastsenderrortime = dr["lastsenderrortime"].ToDateTime();
			}
			//错误发送时间间隔（单位:分钟）
			if(dr.Table.Columns.Contains("errorsendintervaltime"))
			{
				o.errorsendintervaltime = dr["errorsendintervaltime"].Toint();
			}
			return o;
        }
    }
}