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
	/*代码自动生成工具自动生成,不要在这里写自己的代码，否则会被自动覆盖哦 - 车毅*/
	public partial class tb_cluster_monitorinfo_dal
    {
        public virtual bool Add(DbConn PubConn, tb_cluster_monitorinfo_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//服务器id
					new ProcedureParameter("@serverid",    model.serverid),
					//服务器监控得到的各项指标信息
					new ProcedureParameter("@monitorinfojson",    model.monitorinfojson),
					//上次服务器更新时间
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_cluster_monitorinfo(serverid,monitorinfojson,lastupdatetime)
										   values(@serverid,@monitorinfojson,@lastupdatetime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_cluster_monitorinfo_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//服务器id
					new ProcedureParameter("@serverid",    model.serverid),
					//服务器监控得到的各项指标信息
					new ProcedureParameter("@monitorinfojson",    model.monitorinfojson),
					//上次服务器更新时间
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_cluster_monitorinfo set serverid=@serverid,monitorinfojson=@monitorinfojson,lastupdatetime=@lastupdatetime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_cluster_monitorinfo where id=@id";
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

        public virtual tb_cluster_monitorinfo_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_cluster_monitorinfo s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_cluster_monitorinfo_model CreateModel(DataRow dr)
        {
            var o = new tb_cluster_monitorinfo_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//服务器id
			if(dr.Table.Columns.Contains("serverid"))
			{
				o.serverid = dr["serverid"].Toint();
			}
			//服务器监控得到的各项指标信息
			if(dr.Table.Columns.Contains("monitorinfojson"))
			{
				o.monitorinfojson = dr["monitorinfojson"].Tostring();
			}
			//上次服务器更新时间
			if(dr.Table.Columns.Contains("lastupdatetime"))
			{
				o.lastupdatetime = dr["lastupdatetime"].ToDateTime();
			}
			return o;
        }
    }
}