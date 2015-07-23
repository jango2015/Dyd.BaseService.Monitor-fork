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
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
	public partial class tb_error_log_dal
    {
        public virtual bool Add(DbConn PubConn, tb_error_log_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//���ݿⴴ��ʱ��
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//��־��Ŀ�д���ʱ��
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//��־����:һ�����������,ϵͳ�����ش���
					new ProcedureParameter("@logtype",    model.logtype),
					//��Ŀ����
					new ProcedureParameter("@projectname",    model.projectname),
					//��־Ψһ��ʾ(��̵ķ���������url,���ڹ���)
					new ProcedureParameter("@logtag",    model.logtag),
					//������Ϣ
					new ProcedureParameter("@msg",    model.msg),
					//��ջ����
					new ProcedureParameter("@tracestack",    model.tracestack),
					//������ע��Ϣ
					new ProcedureParameter("@remark",    model.remark),
					//��ؿ�����Ա
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
                    
					//���ݿⴴ��ʱ��
					new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//��־��Ŀ�д���ʱ��
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//��־����:һ�����������,ϵͳ�����ش���
					new ProcedureParameter("@logtype",    model.logtype),
					//��Ŀ����
					new ProcedureParameter("@projectname",    model.projectname),
					//��־Ψһ��ʾ(��̵ķ���������url,���ڹ���)
					new ProcedureParameter("@logtag",    model.logtag),
					//������Ϣ
					new ProcedureParameter("@msg",    model.msg),
					//��ջ����
					new ProcedureParameter("@tracestack",    model.tracestack),
					//������ע��Ϣ
					new ProcedureParameter("@remark",    model.remark),
					//��ؿ�����Ա
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
			//���ݿⴴ��ʱ��
			if(dr.Table.Columns.Contains("sqlservercreatetime"))
			{
				o.sqlservercreatetime = dr["sqlservercreatetime"].ToDateTime();
			}
			//��־��Ŀ�д���ʱ��
			if(dr.Table.Columns.Contains("logcreatetime"))
			{
				o.logcreatetime = dr["logcreatetime"].ToDateTime();
			}
			//��־����:һ�����������,ϵͳ�����ش���
			if(dr.Table.Columns.Contains("logtype"))
			{
				o.logtype = dr["logtype"].ToByte();
			}
			//��Ŀ����
			if(dr.Table.Columns.Contains("projectname"))
			{
				o.projectname = dr["projectname"].Tostring();
			}
			//��־Ψһ��ʾ(��̵ķ���������url,���ڹ���)
			if(dr.Table.Columns.Contains("logtag"))
			{
				o.logtag = dr["logtag"].Tostring();
			}
			//������Ϣ
			if(dr.Table.Columns.Contains("msg"))
			{
				o.msg = dr["msg"].Tostring();
			}
			//��ջ����
			if(dr.Table.Columns.Contains("tracestack"))
			{
				o.tracestack = dr["tracestack"].Tostring();
			}
			//������ע��Ϣ
			if(dr.Table.Columns.Contains("remark"))
			{
				o.remark = dr["remark"].Tostring();
			}
			//��ؿ�����Ա
			if(dr.Table.Columns.Contains("developer"))
			{
				o.developer = dr["developer"].Tostring();
			}
			return o;
        }
    }
}