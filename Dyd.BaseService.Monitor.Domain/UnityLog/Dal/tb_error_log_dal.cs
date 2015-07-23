using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.UnityLog.Model;
using XXF.Db;
using System.Data;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Domain.UnityLog.Dal
{
    public partial class tb_error_log_dal
    {
        public virtual bool AddError(DbConn PubConn, tb_error_log_model model)
        {

            List<ProcedureParameter> Par = new List<ProcedureParameter>()
                {
					
					//数据库创建时间
					//new ProcedureParameter("@sqlservercreatetime",    model.sqlservercreatetime),
					//日志项目中创建时间
					new ProcedureParameter("@logcreatetime",    model.logcreatetime),
					//日志类型:一般非正常错误,系统级严重错误
					new ProcedureParameter("@logtype",    model.logtype),
					//项目名称
					new ProcedureParameter("@projectname",    model.projectname),
					//日志唯一标示(简短的方法名或者url,便于归类)
					new ProcedureParameter("@logtag",    model.logtag),
					//错误信息
					new ProcedureParameter("@msg",    model.msg),
					//堆栈跟踪
					new ProcedureParameter("@tracestack",    model.tracestack),
					//其他备注信息
					new ProcedureParameter("@remark",    model.remark),
					//相关开发人员
					new ProcedureParameter("@developer",    model.developer)   
                };
            int rev = PubConn.ExecuteSql(string.Format(@"insert into tb_error_log{0}(sqlservercreatetime,logcreatetime,logtype,projectname,logtag,msg,tracestack,remark,developer)
										   values(getdate(),@logcreatetime,@logtype,@projectname,@logtag,@msg,@tracestack,@remark,@developer)", XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.MonthRule(DateTime.Now)), Par);
            return rev == 1;

        }

        public virtual int GetGrawErrorNum(DbConn PubConn, DateTime lasttime)
        {

            return SqlHelper.Visit(ps =>
            {
                ps.Add("@lasttime", lasttime);
                string cmd = string.Format("select count(0) from tb_error_log{0} with (nolock) where sqlservercreatetime>=@lasttime", XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.MonthRule(DateTime.Now));
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                return 0;
            });

        }
        public virtual List<tb_error_log_model> GetUserErrors(DbConn PubConn, DateTime lasttime, string username, string userstaffno,int topcount)
        {

            return SqlHelper.Visit(ps =>
            {
                ps.Add("@lasttime", lasttime);
                string cmd = string.Format("select top {0} * from tb_error_log{1} with (nolock) where sqlservercreatetime>=@lasttime", topcount, XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.MonthRule(DateTime.Now));
                if (username != "" || userstaffno != "")
                {
                    string developersql = "1=0";
                    if (username != "")
                    {
                        developersql += " or developer=@username";
                        ps.Add("@username",username);
                    }
                    if (userstaffno != "")
                    {
                        developersql += " or developer=@userstaffno";
                        ps.Add("@userstaffno", userstaffno);
                    }
                    cmd += string.Format(" and ({0})",developersql);
                }
                cmd += " order by id desc";
                DataSet ds = new DataSet();
                PubConn.SqlToDataSet(ds, cmd, ps.ToParameters());
                List<tb_error_log_model> rs = new List<tb_error_log_model>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        tb_error_log_model m = CreateModel(dr);
                        rs.Add(m);
                    }
                }
                return rs;
            });

        }
        public List<tb_error_log_model> GetList(DbConn PubConn,string id, string keyword, DateTime timebegin, DateTime timeend, int logtype, string projectname, int pagesize, int pageindex, out int count)
        {
            int _count = 0;
            List<tb_error_log_model> model = new List<tb_error_log_model>();
            DataSet dsList = SqlHelper.Visit<DataSet>(ps =>
            {
                string sqlwhere = " where 1=1 ";
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ps.Add("id",id);
                    sqlwhere += " and (id=@id) ";
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    ps.Add("keyword", keyword);
                    sqlwhere += " and (remark like '%'+@keyword+'%' or projectname like '%'+@keyword+'%' or logtag like '%'+@keyword+'%' or msg like '%'+@keyword+'%' ) ";
                }
                if (!string.IsNullOrWhiteSpace(projectname))
                {
                    ps.Add("projectname", projectname);
                    sqlwhere += " and (projectname=@projectname) ";
                }
                if (true)
                {
                    ps.Add("timebegin", timebegin); ps.Add("timeend", timeend);
                    sqlwhere += " and  sqlservercreatetime>=@timebegin and sqlservercreatetime<=@timeend";
                }
                if (logtype >= 0)
                {
                    ps.Add("logtype", logtype);
                    sqlwhere += " and  logtype=@logtype";
                }
                string zq = "";
                DateTime dateNow = DateTime.Now;
                if (DateTime.TryParse(timeend.ToString("yyyyMM"), out dateNow))
                {
                    zq = dateNow.ToString("yyyyMM");
                }
                else
                {
                    zq = DateTime.Now.ToString("yyyyMM");
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("select ROW_NUMBER() over(order by id desc) as rownum,* from tb_error_log" + zq + " with (nolock) ");
                _count = Convert.ToInt32(PubConn.ExecuteScalar("select count(1) from tb_error_log" + zq + " with (nolock) " + sqlwhere, ps.ToParameters()));
                DataSet ds = new DataSet();
                string sqlSel = "select * from (" + sql + sqlwhere + ") A where rownum between " + ((pageindex - 1) * pagesize + 1) + " and " + pagesize * pageindex;
                PubConn.SqlToDataSet(ds, sqlSel, ps.ToParameters());
                return ds;
            });
            foreach (DataRow dr in dsList.Tables[0].Rows)
            {
                tb_error_log_model m = CreateModel(dr);
                model.Add(m);
            }
            count = _count;
            return model;
        }
    }
}
