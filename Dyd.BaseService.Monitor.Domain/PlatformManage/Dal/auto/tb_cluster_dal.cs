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
	public partial class tb_cluster_dal
    {
        public virtual bool Add(DbConn PubConn, tb_cluster_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//��������
					new ProcedureParameter("@servername",    model.servername),
					//������ip
					new ProcedureParameter("@serverip",    model.serverip),
					//�Ƿ������������
					new ProcedureParameter("@ifmonitor",    model.ifmonitor),
					//
					new ProcedureParameter("@performancecollectconfigjson",    model.performancecollectconfigjson),
					//��ص�����json
					new ProcedureParameter("@monitorcollectconfigjson",    model.monitorcollectconfigjson),
					//����ʱ��
					new ProcedureParameter("@createtime",    model.createtime),
					//�ϴθ���ʱ��
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime),
					//
					new ProcedureParameter("@onlinetime",    model.onlinetime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_cluster(servername,serverip,ifmonitor,performancecollectconfigjson,monitorcollectconfigjson,createtime,lastupdatetime,onlinetime)
										   values(@servername,@serverip,@ifmonitor,@performancecollectconfigjson,@monitorcollectconfigjson,@createtime,@lastupdatetime,@onlinetime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_cluster_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//��������
					new ProcedureParameter("@servername",    model.servername),
					//������ip
					new ProcedureParameter("@serverip",    model.serverip),
					//�Ƿ������������
					new ProcedureParameter("@ifmonitor",    model.ifmonitor),
					//
					new ProcedureParameter("@performancecollectconfigjson",    model.performancecollectconfigjson),
					//��ص�����json
					new ProcedureParameter("@monitorcollectconfigjson",    model.monitorcollectconfigjson),
					//����ʱ��
					new ProcedureParameter("@createtime",    model.createtime),
					//�ϴθ���ʱ��
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime),
					//
					new ProcedureParameter("@onlinetime",    model.onlinetime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_cluster set servername=@servername,serverip=@serverip,ifmonitor=@ifmonitor,performancecollectconfigjson=@performancecollectconfigjson,monitorcollectconfigjson=@monitorcollectconfigjson,createtime=@createtime,lastupdatetime=@lastupdatetime,onlinetime=@onlinetime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_cluster where id=@id";
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

        public virtual tb_cluster_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_cluster s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_cluster_model CreateModel(DataRow dr)
        {
            var o = new tb_cluster_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//��������
			if(dr.Table.Columns.Contains("servername"))
			{
				o.servername = dr["servername"].Tostring();
			}
			//������ip
			if(dr.Table.Columns.Contains("serverip"))
			{
				o.serverip = dr["serverip"].Tostring();
			}
			//�Ƿ������������
			if(dr.Table.Columns.Contains("ifmonitor"))
			{
				o.ifmonitor = dr["ifmonitor"].Tobool();
			}
			//
			if(dr.Table.Columns.Contains("performancecollectconfigjson"))
			{
				o.performancecollectconfigjson = dr["performancecollectconfigjson"].Tostring();
			}
			//��ص�����json
			if(dr.Table.Columns.Contains("monitorcollectconfigjson"))
			{
				o.monitorcollectconfigjson = dr["monitorcollectconfigjson"].Tostring();
			}
			//����ʱ��
			if(dr.Table.Columns.Contains("createtime"))
			{
				o.createtime = dr["createtime"].ToDateTime();
			}
			//�ϴθ���ʱ��
			if(dr.Table.Columns.Contains("lastupdatetime"))
			{
				o.lastupdatetime = dr["lastupdatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("onlinetime"))
			{
				o.onlinetime = dr["onlinetime"].ToDateTime();
			}
			return o;
        }
    }
}