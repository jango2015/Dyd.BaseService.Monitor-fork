using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.UnityLog.Model;

namespace Dyd.BaseService.Monitor.Domain.UnityLog.Dal
{
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
	public partial class tb_error_log_dal
    {
        public virtual bool Add(DbConn PubConn, tb_error_log_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//数据库创建时间
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//日志项目中创建时间
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//日志类型:一般非正常错误,系统级严重错误
					new ProcedureParameter("@logtype",    model.logtype),
					//项目名称
					new ProcedureParameter("@projectname",    model.projectname),
					//日志唯一标示(简短的方法名或者url,便于归类)
					new ProcedureParameter("@logtag",    model.logtag),
					//错误信息
					new ProcedureParameter("@msg",    model.msg),
					//堆栈跟踪
					new ProcedureParameter("@tracestack",    model.tracestack),
					//其他备注信息
					new ProcedureParameter("@remark",    model.remark),
					//相关开发人员
					new ProcedureParameter("@developer",    model.developer)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_error_log(sqlservercreatetime,logcreatetime,logtype,projectname,logtag,msg,tracestack,remark,developer)
										   values(@sqlservercreatetime,@logcreatetime,@logtype,@projectname,@logtag,@msg,@tracestack,@remark,@developer)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_error_log_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//数据库创建时间
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//日志项目中创建时间
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//日志类型:一般非正常错误,系统级严重错误
					new ProcedureParameter("@logtype",    model.logtype),
					//项目名称
					new ProcedureParameter("@projectname",    model.projectname),
					//日志唯一标示(简短的方法名或者url,便于归类)
					new ProcedureParameter("@logtag",    model.logtag),
					//错误信息
					new ProcedureParameter("@msg",    model.msg),
					//堆栈跟踪
					new ProcedureParameter("@tracestack",    model.tracestack),
					//其他备注信息
					new ProcedureParameter("@remark",    model.remark),
					//相关开发人员
					new ProcedureParameter("@developer",    model.developer)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_error_log set sqlservercreatetime=@sqlservercreatetime,logcreatetime=@logcreatetime,logtype=@logtype,projectname=@projectname,logtag=@logtag,msg=@msg,tracestack=@tracestack,remark=@remark,developer=@developer where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_error_log where id=@id";
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

        public virtual tb_error_log_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_error_log s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_error_log_model CreateModel(DataRow dr)
        {
            var o = new tb_error_log_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//数据库创建时间
			if(dr.Table.Columns.Contains("sqlservercreatetime"))
			{
				o.sqlservercreatetime = dr["sqlservercreatetime"].ToDateTime();
			}
			//日志项目中创建时间
			if(dr.Table.Columns.Contains("logcreatetime"))
			{
				o.logcreatetime = dr["logcreatetime"].ToDateTime();
			}
			//日志类型:一般非正常错误,系统级严重错误
			if(dr.Table.Columns.Contains("logtype"))
			{
				o.logtype = dr["logtype"].ToByte();
			}
			//项目名称
			if(dr.Table.Columns.Contains("projectname"))
			{
				o.projectname = dr["projectname"].Tostring();
			}
			//日志唯一标示(简短的方法名或者url,便于归类)
			if(dr.Table.Columns.Contains("logtag"))
			{
				o.logtag = dr["logtag"].Tostring();
			}
			//错误信息
			if(dr.Table.Columns.Contains("msg"))
			{
				o.msg = dr["msg"].Tostring();
			}
			//堆栈跟踪
			if(dr.Table.Columns.Contains("tracestack"))
			{
				o.tracestack = dr["tracestack"].Tostring();
			}
			//其他备注信息
			if(dr.Table.Columns.Contains("remark"))
			{
				o.remark = dr["remark"].Tostring();
			}
			//相关开发人员
			if(dr.Table.Columns.Contains("developer"))
			{
				o.developer = dr["developer"].Tostring();
			}
			return o;
        }
    }
}