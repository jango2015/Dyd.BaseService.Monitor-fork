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
	public partial class tb_timewatchlog_api_dal
    {
        public virtual bool Add(DbConn PubConn, tb_timewatchlog_api_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//
					new ProcedureParameter("@time",    model.time),
					//
					new ProcedureParameter("@projectname",    model.projectname),
					//
					new ProcedureParameter("@url",    model.url),
					//
					new ProcedureParameter("@msg",    model.msg),
					//
					new ProcedureParameter("@fromip",    model.fromip),
					//
					new ProcedureParameter("@tag",    model.tag)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_timewatchlog_api(sqlservercreatetime,logcreatetime,time,projectname,url,msg,fromip,tag)
										   values(@sqlservercreatetime,@logcreatetime,@time,@projectname,@url,@msg,@fromip,@tag)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_timewatchlog_api_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//
					new ProcedureParameter("@time",    model.time),
					//
					new ProcedureParameter("@projectname",    model.projectname),
					//
					new ProcedureParameter("@url",    model.url),
					//
					new ProcedureParameter("@msg",    model.msg),
					//
					new ProcedureParameter("@fromip",    model.fromip),
					//
					new ProcedureParameter("@tag",    model.tag)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_timewatchlog_api set sqlservercreatetime=@sqlservercreatetime,logcreatetime=@logcreatetime,time=@time,projectname=@projectname,url=@url,msg=@msg,fromip=@fromip,tag=@tag where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_timewatchlog_api where id=@id";
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

        public virtual tb_timewatchlog_api_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_timewatchlog_api s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_timewatchlog_api_model CreateModel(DataRow dr)
        {
            var o = new tb_timewatchlog_api_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("sqlservercreatetime"))
			{
				o.sqlservercreatetime = dr["sqlservercreatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("logcreatetime"))
			{
				o.logcreatetime = dr["logcreatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("time"))
			{
				o.time = dr["time"].Todouble();
			}
			//
			if(dr.Table.Columns.Contains("projectname"))
			{
				o.projectname = dr["projectname"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("url"))
			{
				o.url = dr["url"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("msg"))
			{
				o.msg = dr["msg"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("fromip"))
			{
				o.fromip = dr["fromip"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("tag"))
			{
				o.tag = dr["tag"].Tostring();
			}
			return o;
        }
    }
}