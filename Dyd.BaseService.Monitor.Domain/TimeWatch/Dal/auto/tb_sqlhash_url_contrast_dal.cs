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
	public partial class tb_sqlhash_url_contrast_dal
    {
        public virtual bool Add(DbConn PubConn, tb_sqlhash_url_contrast_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//
					new ProcedureParameter("@sqlhash",    model.sqlhash),
					//
					new ProcedureParameter("@url",    model.url)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_sqlhash_url_contrast(sqlhash,url)
										   values(@sqlhash,@url)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_sqlhash_url_contrast_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//
					new ProcedureParameter("@sqlhash",    model.sqlhash),
					//
					new ProcedureParameter("@url",    model.url)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_sqlhash_url_contrast set sqlhash=@sqlhash,url=@url where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_sqlhash_url_contrast where id=@id";
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

        public virtual tb_sqlhash_url_contrast_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_sqlhash_url_contrast s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_sqlhash_url_contrast_model CreateModel(DataRow dr)
        {
            var o = new tb_sqlhash_url_contrast_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("sqlhash"))
			{
				o.sqlhash = dr["sqlhash"].Toint();
			}
			//
			if(dr.Table.Columns.Contains("url"))
			{
				o.url = dr["url"].Tostring();
			}
			return o;
        }
    }
}