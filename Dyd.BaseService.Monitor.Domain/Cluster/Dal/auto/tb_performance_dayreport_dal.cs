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
	public partial class tb_performance_dayreport_dal
    {
        public virtual bool Add(DbConn PubConn, tb_performance_dayreport_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//日期
					new ProcedureParameter("@date",    model.date),
					//服务器id
					new ProcedureParameter("@serverid",    model.serverid),
					//平均cpu
					new ProcedureParameter("@avgcpu",    model.avgcpu),
					//最大cpu
					new ProcedureParameter("@maxcpu",    model.maxcpu),
					//
					new ProcedureParameter("@mincpu",    model.mincpu),
					//
					new ProcedureParameter("@avgmemory",    model.avgmemory),
					//
					new ProcedureParameter("@maxmemory",    model.maxmemory),
					//
					new ProcedureParameter("@minmemory",    model.minmemory),
					//
					new ProcedureParameter("@avgnetworkupload",    model.avgnetworkupload),
					//
					new ProcedureParameter("@maxnetworkupload",    model.maxnetworkupload),
					//
					new ProcedureParameter("@minnetworkupload",    model.minnetworkupload),
					//
					new ProcedureParameter("@avgnetworkdownload",    model.avgnetworkdownload),
					//
					new ProcedureParameter("@maxnetworkdownload",    model.maxnetworkdownload),
					//
					new ProcedureParameter("@minnetworkdownload",    model.minnetworkdownload),
					//
					new ProcedureParameter("@avgioread",    model.avgioread),
					//
					new ProcedureParameter("@maxioread",    model.maxioread),
					//
					new ProcedureParameter("@minioread",    model.minioread),
					//
					new ProcedureParameter("@avgiowrite",    model.avgiowrite),
					//
					new ProcedureParameter("@maxiowrite",    model.maxiowrite),
					//写磁盘
					new ProcedureParameter("@miniowrite",    model.miniowrite),
					//
					new ProcedureParameter("@avgiisrequest",    model.avgiisrequest),
					//
					new ProcedureParameter("@maxiisrequest",    model.maxiisrequest),
					//
					new ProcedureParameter("@miniisrequest",    model.miniisrequest),
					//
					new ProcedureParameter("@count",    model.count),
					//上次扫描最大id
					new ProcedureParameter("@lastmaxid",    model.lastmaxid),
					//上次扫描时间
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_performance_dayreport(date,serverid,avgcpu,maxcpu,mincpu,avgmemory,maxmemory,minmemory,avgnetworkupload,maxnetworkupload,minnetworkupload,avgnetworkdownload,maxnetworkdownload,minnetworkdownload,avgioread,maxioread,minioread,avgiowrite,maxiowrite,miniowrite,avgiisrequest,maxiisrequest,miniisrequest,count,lastmaxid,lastupdatetime)
										   values(@date,@serverid,@avgcpu,@maxcpu,@mincpu,@avgmemory,@maxmemory,@minmemory,@avgnetworkupload,@maxnetworkupload,@minnetworkupload,@avgnetworkdownload,@maxnetworkdownload,@minnetworkdownload,@avgioread,@maxioread,@minioread,@avgiowrite,@maxiowrite,@miniowrite,@avgiisrequest,@maxiisrequest,@miniisrequest,@count,@lastmaxid,@lastupdatetime)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_performance_dayreport_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//日期
					new ProcedureParameter("@date",    model.date),
					//服务器id
					new ProcedureParameter("@serverid",    model.serverid),
					//平均cpu
					new ProcedureParameter("@avgcpu",    model.avgcpu),
					//最大cpu
					new ProcedureParameter("@maxcpu",    model.maxcpu),
					//
					new ProcedureParameter("@mincpu",    model.mincpu),
					//
					new ProcedureParameter("@avgmemory",    model.avgmemory),
					//
					new ProcedureParameter("@maxmemory",    model.maxmemory),
					//
					new ProcedureParameter("@minmemory",    model.minmemory),
					//
					new ProcedureParameter("@avgnetworkupload",    model.avgnetworkupload),
					//
					new ProcedureParameter("@maxnetworkupload",    model.maxnetworkupload),
					//
					new ProcedureParameter("@minnetworkupload",    model.minnetworkupload),
					//
					new ProcedureParameter("@avgnetworkdownload",    model.avgnetworkdownload),
					//
					new ProcedureParameter("@maxnetworkdownload",    model.maxnetworkdownload),
					//
					new ProcedureParameter("@minnetworkdownload",    model.minnetworkdownload),
					//
					new ProcedureParameter("@avgioread",    model.avgioread),
					//
					new ProcedureParameter("@maxioread",    model.maxioread),
					//
					new ProcedureParameter("@minioread",    model.minioread),
					//
					new ProcedureParameter("@avgiowrite",    model.avgiowrite),
					//
					new ProcedureParameter("@maxiowrite",    model.maxiowrite),
					//写磁盘
					new ProcedureParameter("@miniowrite",    model.miniowrite),
					//
					new ProcedureParameter("@avgiisrequest",    model.avgiisrequest),
					//
					new ProcedureParameter("@maxiisrequest",    model.maxiisrequest),
					//
					new ProcedureParameter("@miniisrequest",    model.miniisrequest),
					//
					new ProcedureParameter("@count",    model.count),
					//上次扫描最大id
					new ProcedureParameter("@lastmaxid",    model.lastmaxid),
					//上次扫描时间
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_performance_dayreport set date=@date,serverid=@serverid,avgcpu=@avgcpu,maxcpu=@maxcpu,mincpu=@mincpu,avgmemory=@avgmemory,maxmemory=@maxmemory,minmemory=@minmemory,avgnetworkupload=@avgnetworkupload,maxnetworkupload=@maxnetworkupload,minnetworkupload=@minnetworkupload,avgnetworkdownload=@avgnetworkdownload,maxnetworkdownload=@maxnetworkdownload,minnetworkdownload=@minnetworkdownload,avgioread=@avgioread,maxioread=@maxioread,minioread=@minioread,avgiowrite=@avgiowrite,maxiowrite=@maxiowrite,miniowrite=@miniowrite,avgiisrequest=@avgiisrequest,maxiisrequest=@maxiisrequest,miniisrequest=@miniisrequest,count=@count,lastmaxid=@lastmaxid,lastupdatetime=@lastupdatetime where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_performance_dayreport where id=@id";
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

        public virtual tb_performance_dayreport_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_performance_dayreport s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_performance_dayreport_model CreateModel(DataRow dr)
        {
            var o = new tb_performance_dayreport_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//日期
			if(dr.Table.Columns.Contains("date"))
			{
				o.date = dr["date"].ToDateTime();
			}
			//服务器id
			if(dr.Table.Columns.Contains("serverid"))
			{
				o.serverid = dr["serverid"].Toint();
			}
			//平均cpu
			if(dr.Table.Columns.Contains("avgcpu"))
			{
				o.avgcpu = dr["avgcpu"].Todecimal();
			}
			//最大cpu
			if(dr.Table.Columns.Contains("maxcpu"))
			{
				o.maxcpu = dr["maxcpu"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("mincpu"))
			{
				o.mincpu = dr["mincpu"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("avgmemory"))
			{
				o.avgmemory = dr["avgmemory"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("maxmemory"))
			{
				o.maxmemory = dr["maxmemory"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("minmemory"))
			{
				o.minmemory = dr["minmemory"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("avgnetworkupload"))
			{
				o.avgnetworkupload = dr["avgnetworkupload"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("maxnetworkupload"))
			{
				o.maxnetworkupload = dr["maxnetworkupload"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("minnetworkupload"))
			{
				o.minnetworkupload = dr["minnetworkupload"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("avgnetworkdownload"))
			{
				o.avgnetworkdownload = dr["avgnetworkdownload"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("maxnetworkdownload"))
			{
				o.maxnetworkdownload = dr["maxnetworkdownload"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("minnetworkdownload"))
			{
				o.minnetworkdownload = dr["minnetworkdownload"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("avgioread"))
			{
				o.avgioread = dr["avgioread"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("maxioread"))
			{
				o.maxioread = dr["maxioread"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("minioread"))
			{
				o.minioread = dr["minioread"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("avgiowrite"))
			{
				o.avgiowrite = dr["avgiowrite"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("maxiowrite"))
			{
				o.maxiowrite = dr["maxiowrite"].Todecimal();
			}
			//写磁盘
			if(dr.Table.Columns.Contains("miniowrite"))
			{
				o.miniowrite = dr["miniowrite"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("avgiisrequest"))
			{
				o.avgiisrequest = dr["avgiisrequest"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("maxiisrequest"))
			{
				o.maxiisrequest = dr["maxiisrequest"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("miniisrequest"))
			{
				o.miniisrequest = dr["miniisrequest"].Todecimal();
			}
			//
			if(dr.Table.Columns.Contains("count"))
			{
				o.count = dr["count"].Toint();
			}
			//上次扫描最大id
			if(dr.Table.Columns.Contains("lastmaxid"))
			{
				o.lastmaxid = dr["lastmaxid"].Toint();
			}
			//上次扫描时间
			if(dr.Table.Columns.Contains("lastupdatetime"))
			{
				o.lastupdatetime = dr["lastupdatetime"].ToDateTime();
			}
          
			return o;
        }
    }
}