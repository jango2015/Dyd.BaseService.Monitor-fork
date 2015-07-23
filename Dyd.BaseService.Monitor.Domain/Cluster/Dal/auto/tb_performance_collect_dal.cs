using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
using XXF.Extensions;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;

namespace Dyd.BaseService.Monitor.Domain.Cluster.Dal
{
	/*�����Զ����ɹ����Զ�����,��Ҫ������д�Լ��Ĵ��룬����ᱻ�Զ�����Ŷ - ����*/
	public partial class tb_performance_collect_dal
    {
        public virtual bool Add(DbConn PubConn, tb_performance_collect_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//������id
					new ProcedureParameter("@serverid",    model.serverid),
					//cpu��Ϣ
					new ProcedureParameter("@cpu",    model.cpu),
					//�ڴ��ֽ�
					new ProcedureParameter("@memory",    model.memory),
					//�����ϴ��ֽ�
					new ProcedureParameter("@networkupload",    model.networkupload),
					//���������ֽ�
					new ProcedureParameter("@networkdownload",    model.networkdownload),
					//io���ֽ�
					new ProcedureParameter("@ioread",    model.ioread),
					//ioд�ֽ�
					new ProcedureParameter("@iowrite",    model.iowrite),
					//
					new ProcedureParameter("@iisrequest",    model.iisrequest),
					//����ʱ��
					new ProcedureParameter("@createtime",    model.createtime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_performance_collect(serverid,cpu,memory,networkupload,networkdownload,ioread,iowrite,iisrequest,createtime)
										   values(@serverid,@cpu,@memory,@networkupload,@networkdownload,@ioread,@iowrite,@iisrequest,@createtime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_performance_collect_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//������id
					new ProcedureParameter("@serverid",    model.serverid),
					//cpu��Ϣ
					new ProcedureParameter("@cpu",    model.cpu),
					//�ڴ��ֽ�
					new ProcedureParameter("@memory",    model.memory),
					//�����ϴ��ֽ�
					new ProcedureParameter("@networkupload",    model.networkupload),
					//���������ֽ�
					new ProcedureParameter("@networkdownload",    model.networkdownload),
					//io���ֽ�
					new ProcedureParameter("@ioread",    model.ioread),
					//ioд�ֽ�
					new ProcedureParameter("@iowrite",    model.iowrite),
					//
					new ProcedureParameter("@iisrequest",    model.iisrequest),
					//����ʱ��
					new ProcedureParameter("@createtime",    model.createtime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_performance_collect set serverid=@serverid,cpu=@cpu,memory=@memory,networkupload=@networkupload,networkdownload=@networkdownload,ioread=@ioread,iowrite=@iowrite,iisrequest=@iisrequest,createtime=@createtime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_performance_collect where id=@id";
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

        public virtual tb_performance_collect_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_performance_collect s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_performance_collect_model CreateModel(DataRow dr)
        {
            var o = new tb_performance_collect_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//������id
			if(dr.Table.Columns.Contains("serverid"))
			{
				o.serverid = dr["serverid"].Toint();
			}
			//cpu��Ϣ
			if(dr.Table.Columns.Contains("cpu"))
			{
				o.cpu = dr["cpu"].Todecimal();
			}
			//�ڴ��ֽ�
			if(dr.Table.Columns.Contains("memory"))
			{
				o.memory = dr["memory"].Todecimal();
			}
			//�����ϴ��ֽ�
			if(dr.Table.Columns.Contains("networkupload"))
			{
				o.networkupload = dr["networkupload"].Todecimal();
			}
			//���������ֽ�
			if(dr.Table.Columns.Contains("networkdownload"))
			{
				o.networkdownload = dr["networkdownload"].Todecimal();
			}
			//io���ֽ�
			if(dr.Table.Columns.Contains("ioread"))
			{
				o.ioread = dr["ioread"].Todecimal();
			}
			//ioд�ֽ�
			if(dr.Table.Columns.Contains("iowrite"))
			{
				o.iowrite = dr["iowrite"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("iisrequest"))
			{
				o.iisrequest = dr["iisrequest"].Todecimal();
			}
			//����ʱ��
			if(dr.Table.Columns.Contains("createtime"))
			{
				o.createtime = dr["createtime"].ToDateTime();
			}
           
			return o;
        }
    }
}