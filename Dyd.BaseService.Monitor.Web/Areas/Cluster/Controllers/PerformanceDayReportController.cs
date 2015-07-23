using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using Webdiyer.WebControls.Mvc;
using XXF.Db;
using Web.Models;
using Dyd.BaseService.Monitor.Domain.Chart.Model;

namespace Web.Areas.Cluster.Controllers
{
    [AuthorityCheck]
    public class PerformanceDayReportController : Controller
    {
        //
        // GET: /Cluster/PerformanceDayReport/

        public ActionResult Index(string serverip,string serverid,string timebegin, string timeend,string orderby="", string datatype="", int pageindex = 1, int pagesize = 15)
        {
            if (string.IsNullOrWhiteSpace(timebegin))
                timebegin = DateTime.Now.Date.AddDays(-10).ToString("yyyy-MM-dd");
            ViewBag.timebegin = timebegin;
            if (string.IsNullOrWhiteSpace(timeend))
                timeend = DateTime.Now.Date.ToString("yyyy-MM-dd");
            ViewBag.timeend = timeend;
            ViewBag.orderby = orderby; ViewBag.datatype = datatype; ViewBag.serverip = serverip; ViewBag.serverid = serverid;
            tb_performance_dayreport_dal dal = new tb_performance_dayreport_dal();
            PagedList<tb_performance_dayreport_model> pageList = null;
            int count = 0;
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
            {
                PubConn.Open();
                List<tb_performance_dayreport_model> List = dal.GetList(PubConn,serverip,serverid, timebegin,timeend,orderby,datatype, pagesize, pageindex, out count);
                pageList = new PagedList<tb_performance_dayreport_model>(List, pageindex, pagesize, count);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("List", pageList);
            }
            return View(pageList);
        }

        public ActionResult Chart(string Fday, string Nday, string key, int pageindex = 1)
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
                key = "avgcpu";
            }
            ViewBag.Fday = Fday;
            ViewBag.Nday = Nday;
            ViewBag.Key = key;
            List<ChartModel> model = new List<ChartModel>();
            PagedList<ChartModel> pageList = null;
            int pagesize = 10;
            int count = 0;
            tb_performance_dayreport_dal dal = new tb_performance_dayreport_dal();
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
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
        }

        public ActionResult ChartJson(string Sday, string Eday, string key)
        {
            List<Dictionary<string, object>> dic = new List<Dictionary<string, object>>();
            tb_performance_dayreport_dal dal = new tb_performance_dayreport_dal();
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
            {
                PubConn.Open();
                dic = dal.GetChartJson(PubConn, Sday, Eday, key);
            }
            return Json(new { data = dic });
        }
    }
}