using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Dal
{
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
	public partial class tb_timewatchlog_dal
    {
        public virtual bool Add(DbConn PubConn, tb_timewatchlog_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//数据库本地创建时间
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//日志创建时间
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//耗时
					new ProcedureParameter("@time",    model.time),
					//项目名称
					new ProcedureParameter("@projectname",    model.projectname),
					//耗时日志类型：普通日志=0，api接口日志=1，sql日志=2
					new ProcedureParameter("@logtype",    model.logtype),
					//日志标识,sql类型则为sql哈希,api类型则为url
					new ProcedureParameter("@logtag",    model.logtag),
					//当前url
					new ProcedureParameter("@url",    model.url),
					//当前信息
					new ProcedureParameter("@msg",    model.msg),
					//来源ip(代码执行ip)
					new ProcedureParameter("@fromip",    model.fromip),
					//sqlip地址
					new ProcedureParameter("@sqlip",    model.sqlip),
					//其他记录标记信息
					new ProcedureParameter("@remark",    model.remark)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_timewatchlog(sqlservercreatetime,logcreatetime,time,projectname,logtype,logtag,url,msg,fromip,sqlip,remark)
										   values(@sqlservercreatetime,@logcreatetime,@time,@projectname,@logtype,@logtag,@url,@msg,@fromip,@sqlip,@remark)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_timewatchlog_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//数据库本地创建时间
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//日志创建时间
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//耗时
					new ProcedureParameter("@time",    model.time),
					//项目名称
					new ProcedureParameter("@projectname",    model.projectname),
					//耗时日志类型：普通日志=0，api接口日志=1，sql日志=2
					new ProcedureParameter("@logtype",    model.logtype),
					//日志标识,sql类型则为sql哈希,api类型则为url
					new ProcedureParameter("@logtag",    model.logtag),
					//当前url
					new ProcedureParameter("@url",    model.url),
					//当前信息
					new ProcedureParameter("@msg",    model.msg),
					//来源ip(代码执行ip)
					new ProcedureParameter("@fromip",    model.fromip),
					//sqlip地址
					new ProcedureParameter("@sqlip",    model.sqlip),
					//其他记录标记信息
					new ProcedureParameter("@remark",    model.remark)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_timewatchlog set sqlservercreatetime=@sqlservercreatetime,logcreatetime=@logcreatetime,time=@time,projectname=@projectname,logtype=@logtype,logtag=@logtag,url=@url,msg=@msg,fromip=@fromip,sqlip=@sqlip,remark=@remark where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_timewatchlog where id=@id";
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

        public virtual tb_timewatchlog_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_timewatchlog s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_timewatchlog_model CreateModel(DataRow dr)
        {
            var o = new tb_timewatchlog_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//数据库本地创建时间
			if(dr.Table.Columns.Contains("sqlservercreatetime"))
			{
				o.sqlservercreatetime = dr["sqlservercreatetime"].ToDateTime();
			}
			//日志创建时间
			if(dr.Table.Columns.Contains("logcreatetime"))
			{
				o.logcreatetime = dr["logcreatetime"].ToDateTime();
			}
			//耗时
			if(dr.Table.Columns.Contains("time"))
			{
				o.time = dr["time"].Todouble();
			}
			//项目名称
			if(dr.Table.Columns.Contains("projectname"))
			{
				o.projectname = dr["projectname"].Tostring();
			}
			//耗时日志类型：普通日志=0，api接口日志=1，sql日志=2
			if(dr.Table.Columns.Contains("logtype"))
			{
				o.logtype = dr["logtype"].ToByte();
			}
			//日志标识,sql类型则为sql哈希,api类型则为url
			if(dr.Table.Columns.Contains("logtag"))
			{
				o.logtag = dr["logtag"].Toint();
			}
			//当前url
			if(dr.Table.Columns.Contains("url"))
			{
				o.url = dr["url"].Tostring();
			}
			//当前信息
			if(dr.Table.Columns.Contains("msg"))
			{
				o.msg = dr["msg"].Tostring();
			}
			//来源ip(代码执行ip)
			if(dr.Table.Columns.Contains("fromip"))
			{
				o.fromip = dr["fromip"].Tostring();
			}
			//sqlip地址
			if(dr.Table.Columns.Contains("sqlip"))
			{
				o.sqlip = dr["sqlip"].Tostring();
			}
			//其他记录标记信息
			if(dr.Table.Columns.Contains("remark"))
			{
				o.remark = dr["remark"].Tostring();
			}
			return o;
        }
    }
}