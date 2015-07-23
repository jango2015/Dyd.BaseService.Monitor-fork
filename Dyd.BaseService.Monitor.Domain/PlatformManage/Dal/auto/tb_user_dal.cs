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
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
	public partial class tb_user_dal
    {
        public virtual bool Add(DbConn PubConn, tb_user_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//�û�����
					new ProcedureParameter("@userstaffno",    model.userstaffno),
					//�û���
					new ProcedureParameter("@username",    model.username),
					//�û���ɫ
					new ProcedureParameter("@userrole",    model.userrole),
					//�û�����ʱ��
					new ProcedureParameter("@usercreatetime",    model.usercreatetime),
					//�û��ֻ�����
					new ProcedureParameter("@usertel",    model.usertel),
					//�û�email
					new ProcedureParameter("@useremail",    model.useremail),
					//��һ�δ�����ʱ��
					new ProcedureParameter("@lastsenderrortime",    model.lastsenderrortime),
					//������ʱ��������λ:���ӣ�
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
                    
					//�û�����
					new ProcedureParameter("@userstaffno",    model.userstaffno),
					//�û���
					new ProcedureParameter("@username",    model.username),
					//�û���ɫ
					new ProcedureParameter("@userrole",    model.userrole),
					//�û�����ʱ��
					new ProcedureParameter("@usercreatetime",    model.usercreatetime),
					//�û��ֻ�����
					new ProcedureParameter("@usertel",    model.usertel),
					//�û�email
					new ProcedureParameter("@useremail",    model.useremail),
					//��һ�δ�����ʱ��
					new ProcedureParameter("@lastsenderrortime",    model.lastsenderrortime),
					//������ʱ��������λ:���ӣ�
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
			//�û�����
			if(dr.Table.Columns.Contains("userstaffno"))
			{
				o.userstaffno = dr["userstaffno"].Tostring();
			}
			//�û���
			if(dr.Table.Columns.Contains("username"))
			{
				o.username = dr["username"].Tostring();
			}
			//�û���ɫ
			if(dr.Table.Columns.Contains("userrole"))
			{
				o.userrole = dr["userrole"].ToByte();
			}
			//�û�����ʱ��
			if(dr.Table.Columns.Contains("usercreatetime"))
			{
				o.usercreatetime = dr["usercreatetime"].ToDateTime();
			}
			//�û��ֻ�����
			if(dr.Table.Columns.Contains("usertel"))
			{
				o.usertel = dr["usertel"].Tostring();
			}
			//�û�email
			if(dr.Table.Columns.Contains("useremail"))
			{
				o.useremail = dr["useremail"].Tostring();
			}
			//��һ�δ�����ʱ��
			if(dr.Table.Columns.Contains("lastsenderrortime"))
			{
				o.lastsenderrortime = dr["lastsenderrortime"].ToDateTime();
			}
			//������ʱ��������λ:���ӣ�
			if(dr.Table.Columns.Contains("errorsendintervaltime"))
			{
				o.errorsendintervaltime = dr["errorsendintervaltime"].Toint();
			}
			return o;
        }
    }
}