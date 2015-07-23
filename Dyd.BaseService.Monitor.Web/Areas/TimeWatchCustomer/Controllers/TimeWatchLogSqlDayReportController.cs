using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Dal;
using Webdiyer.WebControls.Mvc;
using XXF.Db;
using Web.Models;
using Dyd.BaseService.Monitor.Domain.Chart.Model;

namespace Web.Areas.TimeWatchCustomer.Controllers
{
    [AuthorityCheck]
    public class TimeWatchLogSqlDayReportController : TimeWatchConnVisit
    {
        //
        // GET: /TimeWatchCustomer/TimeWatchLogSqlDayReport/

        public ActionResult Index( string sqlhash,string sql,string timebegin,string timeend,string orderby="", int connid = 0, int pagesize = 15, int pageindex = 1)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                if (string.IsNullOrWhiteSpace(timebegin))
                {
                    timebegin = DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrWhiteSpace(timeend))
                {
                    timeend = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
                ViewBag.sqlhash = sqlhash; ViewBag.sql = sql; ViewBag.timebegin = timebegin; ViewBag.timeend = timeend; ViewBag.orderby = orderby; 
                tb_timewatchlog_sql_dayreport_dal dal = new tb_timewatchlog_sql_dayreport_dal();
                PagedList<tb_timewatchlog_sql_dayreport_model> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    List<tb_timewatchlog_sql_dayreport_model> List = dal.GetList(PubConn, sqlhash,sql,timebegin,timeend,orderby, pagesize, pageindex, out count);
                    pageList = new PagedList<tb_timewatchlog_sql_dayreport_model>(List, pageindex, pagesize, count);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("List", pageList);
                }
                return View(pageList);
            });
        }

        public ActionResult Chart(string Fday, string Nday, string key, int pageindex = 1,int connid = 0)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                if (string.IsNullOrWhiteSpace(Fday))
                {
                    Fday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrWhiteSpace(Nday))
                {
                    Nday = DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrWhiteSpace(key))
                {
                    key = "avgtime";
                }
                ViewBag.Fday = Fday;
                ViewBag.Nday = Nday;
                ViewBag.Key = key;
                List<ChartModel> model = new List<ChartModel>();
                PagedList<ChartModel> pageList = null;
                int pagesize = 10;
                int count = 0;
                tb_timewatchlog_sql_dayreport_dal dal = new tb_timewatchlog_sql_dayreport_dal();
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    model = dal.GetKeyChartJson(PubConn, Fday, Nday, key, pagesize, pageindex, out count);
                    pageList = new PagedList<ChartModel>(model, pageindex, pagesize, count);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("List", pageList);
                }
                return View(pageList);
            });
        }

        public ActionResult ChartJson(int connid, string Sday, string Eday, string key)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
                tb_timewatchlog_sql_dayreport_dal dal = new tb_timewatchlog_sql_dayreport_dal();
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    dic = dal.GetChartJson(PubConn, Sday, Eday, key);
                }
                return Json(new { data = dic });
            });
        }
    }
}
