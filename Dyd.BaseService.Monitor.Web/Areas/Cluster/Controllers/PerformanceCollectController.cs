using Dyd.BaseService.Monitor.Domain.Chart.Model;
using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Webdiyer.WebControls.Mvc;
using XXF.Db;

namespace Web.Areas.Cluster.Controllers
{
    [AuthorityCheck]
    public class PerformanceCollectController : Controller
    {
        //
        // GET: /Cluster/PerformanceCollect/

        public ActionResult Index(string serverid,string serverip, string timebegin,string timeend,string orderby="", int pageindex = 1, int pagesize = 15)
        {
            ViewBag.serverid = serverid;
            ViewBag.serverip = serverip;
            if (string.IsNullOrWhiteSpace(timebegin))
            {
                timebegin = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm");
            }
            ViewBag.timebegin = timebegin;
            if (string.IsNullOrWhiteSpace(timeend))
            {
                timeend = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm");
            }
            ViewBag.timeend = timeend;

            ViewBag.orderby = orderby;
            tb_performance_collect_dal dal = new tb_performance_collect_dal();
            PagedList<tb_performance_collect_model> pageList = null;
            int count = 0;
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
            {
                PubConn.Open();
                List<tb_performance_collect_model> List = dal.GetList(PubConn, serverid, serverip, timebegin,timeend,orderby, pagesize, pageindex, out count);
                pageList = new PagedList<tb_performance_collect_model>(List, pageindex, pagesize, count);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("List", pageList);
            }
            return View(pageList);
        }

        public ActionResult Chart(string serverip, string serverid, string date, string columns = "cpu,memory,ioread,iowrite,networkupload,networkdownload", string datatype = "avg")
        {
            if (string.IsNullOrWhiteSpace(date))
                date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            ViewBag.columns = columns; ViewBag.serverip = serverip; ViewBag.serverid = serverid; ViewBag.date = date; ViewBag.datatype = datatype;
            List<TimeChartModel> ms = new List<TimeChartModel>();
            if (string.IsNullOrWhiteSpace(serverip) && string.IsNullOrWhiteSpace(serverid))
            {
                return View(ms);
            }
            TimeChartDataType chartdatatype = (TimeChartDataType)Enum.Parse(typeof(TimeChartDataType), datatype);
            TimeChartType charttype = TimeChartType.Day;

            if (date.Split('-').Length == 3)
                charttype = TimeChartType.Day;
            else if (date.Split('-').Length == 2)
                charttype = TimeChartType.Month;
            else if (date.Split('-').Length == 1)
            {
                charttype = TimeChartType.Year;
                date = new DateTime(Convert.ToInt32(date), 1, 1, 0, 0, 0).ToString("yyyy-MM-dd");
            }

            var startDate = DateTime.Parse(date).Date;
            string[] cs = columns.TrimEnd(',').Split(',');

            foreach (var c in cs)
            {
                if (c == "")
                    continue;
                var model = new TimeChartModel();
                model.DataType = chartdatatype;
                model.Type = charttype;
                model.StartDate = startDate;
                model.Key = c;
                if (c == "cpu")
                {
                    model.Title = "CPU";
                    model.UnitText = "%";
                }
                if (c == "memory")
                {
                    model.Title = "可用内存";
                    model.UnitText = "M";
                }
                if (c == "networkupload")
                {
                    model.Title = "网络上传";
                    model.UnitText = "Byte";
                }
                if (c == "networkdownload")
                {
                    model.Title = "网络下载";
                    model.UnitText = "Byte";
                }
                if (c == "ioread")
                {
                    model.Title = "磁盘读";
                    model.UnitText = "Byte";
                }
                if (c == "iowrite")
                {
                    model.Title = "磁盘写";
                    model.UnitText = "Byte";
                }
                if (c == "iisrequest")
                {
                    model.Title = "iis请求";
                    model.UnitText = "次/s";
                }
                model.SubTitle = model.Title + "【" + model.DataType.ToString() + "】值统计图";
                if (model.Type == TimeChartType.Day)
                {
                    model.PointInterval = "60 * 1000";//一分钟
                    model.EndDate = model.StartDate.AddDays(1);
                    model.MaxZoom = "60 * 60 * 1000";
                    model.SubTitle += ",采集单位【分钟】";
                }
                else if (model.Type == TimeChartType.Month)
                {
                    model.PointInterval = "24 * 60 * 60 * 1000";//一天
                    model.EndDate = model.StartDate.AddMonths(1);
                    model.MaxZoom = "10 * 24 * 60 * 60 * 1000";
                    model.SubTitle += ",采集单位【天】";
                }
                else if (model.Type == TimeChartType.Year)
                {
                    model.PointInterval = "24 * 60 * 60 * 1000";//一天
                    model.EndDate = model.StartDate.AddYears(1);
                    model.MaxZoom = "10 * 24 * 60 * 60 * 1000";
                    model.SubTitle += ",采集单位【天】";
                }


                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
                {
                    PubConn.Open();
                    if (model.Type == TimeChartType.Day)
                    {
                        tb_performance_collect_dal dal = new tb_performance_collect_dal();
                        dic = dal.GetTimeChart(PubConn, c, model.StartDate, serverip, serverid, model.DataType.ToString());
                    }
                    else
                    {
                        tb_performance_dayreport_dal dal = new tb_performance_dayreport_dal();
                        dic = dal.GetTimeChart(PubConn, c, model.StartDate, model.EndDate, serverip, serverid, model.DataType.ToString());
                    }
                }

                DateTime dtbegin = model.StartDate; DateTime dtend = model.EndDate;
                while (true)
                {
                    if (!dic.ContainsKey(dtbegin))
                    {
                        dic.Add(dtbegin, 0);
                    }
                    if (model.Type == TimeChartType.Day)
                        dtbegin = dtbegin.AddMinutes(1);
                    else
                        dtbegin = dtbegin.AddDays(1);
                    if (dtbegin >= dtend)
                        break;
                }
                model.DataJson = new XXF.Serialization.JsonHelper().Serializer(dic.OrderBy(c1 => c1.Key).Select(c1 => c1.Value));
                ms.Add(model);
            }
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                tb_cluster_dal dal = new tb_cluster_dal();
                ViewBag.ServerName = dal.GetServerName(PubConn, serverip, serverid);
            }
            return View(ms);
        }

        public ActionResult ChartLoad(string key, int num = -3)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Json(new { code = -1, msg = "参数丢失" });
            }
            tb_performance_collect_dal dal = new tb_performance_collect_dal();
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
            {
                PubConn.Open();
                Dictionary<string, List<ChartMainModel>> model = dal.GetMainChartJson(PubConn, num, key);
                return Json(new { code = 1, data = model });
            }
        }

        public ActionResult ChartJson(string Fday, string Nday, string key, string columnname)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Json(new { code = -1, msg = "参数丢失" });
            }
            if (string.IsNullOrWhiteSpace(columnname))
            {
                return Json(new { code = -1, msg = "请选择比较的参数" });
            }
            if (Fday == Nday)
            {
                return Json(new { code = -1, msg = "时间不能一样" });
            }
            try
            {
                List<string> day = new List<string>();
                day.Add(Fday);
                day.Add(Nday);
                tb_performance_collect_dal dal = new tb_performance_collect_dal();
                Dictionary<string, List<ChartSubModel>> model = new Dictionary<string, List<ChartSubModel>>();
                using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
                {
                    PubConn.Open();
                    model = dal.GetSubChartJson(PubConn, day, key, columnname);
                }
                return Json(new { code = 1, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = "日表不存在无法比较" });
            }
        }
    }
}
