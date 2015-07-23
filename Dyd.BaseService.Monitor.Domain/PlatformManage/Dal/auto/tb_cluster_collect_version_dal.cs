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
	public partial class tb_cluster_collect_version_dal
    {
        public virtual bool Add(DbConn PubConn, tb_cluster_collect_version_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@versionnum",    model.versionnum),
					//
					new ProcedureParameter("@versioncreatetime",    model.versioncreatetime),
					//
					new ProcedureParameter("@zipfilename",    model.zipfilename),
					//
					new ProcedureParameter("@zipfile",    model.zipfile)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_cluster_collect_version(versionnum,versioncreatetime,zipfilename,zipfile)
										   values(@versionnum,@versioncreatetime,@zipfilename,@zipfile)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_cluster_collect_version_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@versionnum",    model.versionnum),
					//
					new ProcedureParameter("@versioncreatetime",    model.versioncreatetime),
					//
					new ProcedureParameter("@zipfilename",    model.zipfilename),
					//
					new ProcedureParameter("@zipfile",    model.zipfile)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_cluster_collect_version set versionnum=@versionnum,versioncreatetime=@versioncreatetime,zipfilename=@zipfilename,zipfile=@zipfile where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_cluster_collect_version where id=@id";
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

        public virtual tb_cluster_collect_version_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_cluster_collect_version s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_cluster_collect_version_model CreateModel(DataRow dr)
        {
            var o = new tb_cluster_collect_version_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("versionnum"))
			{
				o.versionnum = dr["versionnum"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("versioncreatetime"))
			{
				o.versioncreatetime = dr["versioncreatetime"].ToDateTime();
			}
			//
			if(dr.Table.Columns.Contains("zipfilename"))
			{
				o.zipfilename = dr["zipfilename"].Tostring();
			}
			//
			if(dr.Table.Columns.Contains("zipfile"))
			{
				o.zipfile = dr["zipfile"].ToBytes();
			}
			return o;
        }
    }
}