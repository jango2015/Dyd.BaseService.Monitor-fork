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
	public partial class tb_database_config_dal
    {
        public virtual bool Add(DbConn PubConn, tb_database_config_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//数据库本地昵称
					new ProcedureParameter("@dblocalname",    model.dblocalname),
					//数据库服务器地址
					new ProcedureParameter("@dbserver",    model.dbserver),
					//数据库名称
					new ProcedureParameter("@dbname",    model.dbname),
					//数据库用户名
					new ProcedureParameter("@dbuser",    model.dbuser),
					//数据库密码
					new ProcedureParameter("@dbpass",    model.dbpass),
					//数据库类型
					new ProcedureParameter("@dbtype",    model.dbtype)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_database_config(dblocalname,dbserver,dbname,dbuser,dbpass,dbtype)
										   values(@dblocalname,@dbserver,@dbname,@dbuser,@dbpass,@dbtype)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_database_config_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//数据库本地昵称
					new ProcedureParameter("@dblocalname",    model.dblocalname),
					//数据库服务器地址
					new ProcedureParameter("@dbserver",    model.dbserver),
					//数据库名称
					new ProcedureParameter("@dbname",    model.dbname),
					//数据库用户名
					new ProcedureParameter("@dbuser",    model.dbuser),
					//数据库密码
					new ProcedureParameter("@dbpass",    model.dbpass),
					//数据库类型
					new ProcedureParameter("@dbtype",    model.dbtype)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_database_config set dblocalname=@dblocalname,dbserver=@dbserver,dbname=@dbname,dbuser=@dbuser,dbpass=@dbpass,dbtype=@dbtype where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_database_config where id=@id";
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

        public virtual tb_database_config_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_database_config s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_database_config_model CreateModel(DataRow dr)
        {
            var o = new tb_database_config_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//数据库本地昵称
			if(dr.Table.Columns.Contains("dblocalname"))
			{
				o.dblocalname = dr["dblocalname"].Tostring();
			}
			//数据库服务器地址
			if(dr.Table.Columns.Contains("dbserver"))
			{
				o.dbserver = dr["dbserver"].Tostring();
			}
			//数据库名称
			if(dr.Table.Columns.Contains("dbname"))
			{
				o.dbname = dr["dbname"].Tostring();
			}
			//数据库用户名
			if(dr.Table.Columns.Contains("dbuser"))
			{
				o.dbuser = dr["dbuser"].Tostring();
			}
			//数据库密码
			if(dr.Table.Columns.Contains("dbpass"))
			{
				o.dbpass = dr["dbpass"].Tostring();
			}
			//数据库类型
			if(dr.Table.Columns.Contains("dbtype"))
			{
				o.dbtype = dr["dbtype"].ToByte();
			}
			return o;
        }
    }
}