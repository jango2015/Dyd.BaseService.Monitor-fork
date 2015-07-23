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
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
	public partial class tb_timewatchlog_dal
    {
        public virtual bool Add(DbConn PubConn, tb_timewatchlog_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//���ݿⱾ�ش���ʱ��
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//��־����ʱ��
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//��ʱ
					new ProcedureParameter("@time",    model.time),
					//��Ŀ����
					new ProcedureParameter("@projectname",    model.projectname),
					//��ʱ��־���ͣ���ͨ��־=0��api�ӿ���־=1��sql��־=2
					new ProcedureParameter("@logtype",    model.logtype),
					//��־��ʶ,sql������Ϊsql��ϣ,api������Ϊurl
					new ProcedureParameter("@logtag",    model.logtag),
					//��ǰurl
					new ProcedureParameter("@url",    model.url),
					//��ǰ��Ϣ
					new ProcedureParameter("@msg",    model.msg),
					//��Դip(����ִ��ip)
					new ProcedureParameter("@fromip",    model.fromip),
					//sqlip��ַ
					new ProcedureParameter("@sqlip",    model.sqlip),
					//������¼�����Ϣ
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
                    
					//���ݿⱾ�ش���ʱ��
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//��־����ʱ��
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//��ʱ
					new ProcedureParameter("@time",    model.time),
					//��Ŀ����
					new ProcedureParameter("@projectname",    model.projectname),
					//��ʱ��־���ͣ���ͨ��־=0��api�ӿ���־=1��sql��־=2
					new ProcedureParameter("@logtype",    model.logtype),
					//��־��ʶ,sql������Ϊsql��ϣ,api������Ϊurl
					new ProcedureParameter("@logtag",    model.logtag),
					//��ǰurl
					new ProcedureParameter("@url",    model.url),
					//��ǰ��Ϣ
					new ProcedureParameter("@msg",    model.msg),
					//��Դip(����ִ��ip)
					new ProcedureParameter("@fromip",    model.fromip),
					//sqlip��ַ
					new ProcedureParameter("@sqlip",    model.sqlip),
					//������¼�����Ϣ
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
			//���ݿⱾ�ش���ʱ��
			if(dr.Table.Columns.Contains("sqlservercreatetime"))
			{
				o.sqlservercreatetime = dr["sqlservercreatetime"].ToDateTime();
			}
			//��־����ʱ��
			if(dr.Table.Columns.Contains("logcreatetime"))
			{
				o.logcreatetime = dr["logcreatetime"].ToDateTime();
			}
			//��ʱ
			if(dr.Table.Columns.Contains("time"))
			{
				o.time = dr["time"].Todouble();
			}
			//��Ŀ����
			if(dr.Table.Columns.Contains("projectname"))
			{
				o.projectname = dr["projectname"].Tostring();
			}
			//��ʱ��־���ͣ���ͨ��־=0��api�ӿ���־=1��sql��־=2
			if(dr.Table.Columns.Contains("logtype"))
			{
				o.logtype = dr["logtype"].ToByte();
			}
			//��־��ʶ,sql������Ϊsql��ϣ,api������Ϊurl
			if(dr.Table.Columns.Contains("logtag"))
			{
				o.logtag = dr["logtag"].Toint();
			}
			//��ǰurl
			if(dr.Table.Columns.Contains("url"))
			{
				o.url = dr["url"].Tostring();
			}
			//��ǰ��Ϣ
			if(dr.Table.Columns.Contains("msg"))
			{
				o.msg = dr["msg"].Tostring();
			}
			//��Դip(����ִ��ip)
			if(dr.Table.Columns.Contains("fromip"))
			{
				o.fromip = dr["fromip"].Tostring();
			}
			//sqlip��ַ
			if(dr.Table.Columns.Contains("sqlip"))
			{
				o.sqlip = dr["sqlip"].Tostring();
			}
			//������¼�����Ϣ
			if(dr.Table.Columns.Contains("remark"))
			{
				o.remark = dr["remark"].Tostring();
			}
			return o;
        }
    }
}