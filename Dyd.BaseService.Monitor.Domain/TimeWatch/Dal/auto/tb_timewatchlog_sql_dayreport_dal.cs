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
	public partial class tb_timewatchlog_sql_dayreport_dal
    {
        public virtual bool Add(DbConn PubConn, tb_timewatchlog_sql_dayreport_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//sql ��ϣ
					new ProcedureParameter("@sqlhash",    model.sqlhash),
					//����
					new ProcedureParameter("@date",    model.date),
					//ƽ����ʱ
					new ProcedureParameter("@avgtime",    model.avgtime),
					//����ʱ
					new ProcedureParameter("@maxtime",    model.maxtime),
					//��С��ʱ
					new ProcedureParameter("@mintime",    model.mintime),
					//�ϴ�ɨ�����id
					new ProcedureParameter("@lastmaxid",    model.lastmaxid),
					//�ϴ�ɨ�����ʱ��
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime),
					//���ô���
					new ProcedureParameter("@count",    model.count)   
                };
            int rev = PubConn.ExecuteSql(@"insert into tb_timewatchlog_sql_dayreport(sqlhash,date,avgtime,maxtime,mintime,lastmaxid,lastupdatetime,count)
										   values(@sqlhash,@date,@avgtime,@maxtime,@mintime,@lastmaxid,@lastupdatetime,@count)", Par);
            return rev == 1;

        }

        public virtual bool Edit(DbConn PubConn, tb_timewatchlog_sql_dayreport_model model)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                    
					//sql ��ϣ
					new ProcedureParameter("@sqlhash",    model.sqlhash),
					//����
					new ProcedureParameter("@date",    model.date),
					//ƽ����ʱ
					new ProcedureParameter("@avgtime",    model.avgtime),
					//����ʱ
					new ProcedureParameter("@maxtime",    model.maxtime),
					//��С��ʱ
					new ProcedureParameter("@mintime",    model.mintime),
					//�ϴ�ɨ�����id
					new ProcedureParameter("@lastmaxid",    model.lastmaxid),
					//�ϴ�ɨ�����ʱ��
					new ProcedureParameter("@lastupdatetime",    model.lastupdatetime),
					//���ô���
					new ProcedureParameter("@count",    model.count)
            };
			Par.Add(new ProcedureParameter("@id",  model.id));

            int rev = PubConn.ExecuteSql("update tb_timewatchlog_sql_dayreport set sqlhash=@sqlhash,date=@date,avgtime=@avgtime,maxtime=@maxtime,mintime=@mintime,lastmaxid=@lastmaxid,lastupdatetime=@lastupdatetime,count=@count where id=@id", Par);
            return rev == 1;

        }

        public virtual bool Delete(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id",  id));

            string Sql = "delete from tb_timewatchlog_sql_dayreport where id=@id";
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

        public virtual tb_timewatchlog_sql_dayreport_model Get(DbConn PubConn, int id)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>();
            Par.Add(new ProcedureParameter("@id", id));
            StringBuilder stringSql = new StringBuilder();
            stringSql.Append(@"select s.* from tb_timewatchlog_sql_dayreport s where s.id=@id");
            DataSet ds = new DataSet();
            PubConn.SqlToDataSet(ds, stringSql.ToString(), Par);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
				return CreateModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

		public virtual tb_timewatchlog_sql_dayreport_model CreateModel(DataRow dr)
        {
            var o = new tb_timewatchlog_sql_dayreport_model();
			
			//
			if(dr.Table.Columns.Contains("id"))
			{
				o.id = dr["id"].Toint();
			}
			//sql ��ϣ
			if(dr.Table.Columns.Contains("sqlhash"))
			{
				o.sqlhash = dr["sqlhash"].Tostring();
			}
			//����
			if(dr.Table.Columns.Contains("date"))
			{
				o.date = dr["date"].ToDateTime();
			}
			//ƽ����ʱ
			if(dr.Table.Columns.Contains("avgtime"))
			{
				o.avgtime = dr["avgtime"].Todouble();
			}
			//����ʱ
			if(dr.Table.Columns.Contains("maxtime"))
			{
				o.maxtime = dr["maxtime"].Todouble();
			}
			//��С��ʱ
			if(dr.Table.Columns.Contains("mintime"))
			{
				o.mintime = dr["mintime"].Todouble();
			}
			//�ϴ�ɨ�����id
			if(dr.Table.Columns.Contains("lastmaxid"))
			{
				o.lastmaxid = dr["lastmaxid"].Toint();
			}
			//�ϴ�ɨ�����ʱ��
			if(dr.Table.Columns.Contains("lastupdatetime"))
			{
				o.lastupdatetime = dr["lastupdatetime"].ToDateTime();
			}
			//���ô���
			if(dr.Table.Columns.Contains("count"))
			{
				o.count = dr["count"].Toint();
			}
            //���ô���
            if (dr.Table.Columns.Contains("sql"))
            {
                o.sql = dr["sql"].Tostring();
            }
			return o;
        }
    }
}