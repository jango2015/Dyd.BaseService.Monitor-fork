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
    public class TimeWatchLogController : TimeWatchConnVisit
    {
        //
        // GET: /TimeWatchCustomer/TimeWatchLog/

        public ActionResult Index(string timebegin, string timeend, string projectname, string fromip, string sqlip, string url, string logtag,string orderby, int logtype = -1, double timewatchmin = -1, double timewatchmax = -1, int connid = 0, int pagesize = 10, int pageindex = 1)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                if (string.IsNullOrWhiteSpace(timebegin))
                    timebegin = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
                if (string.IsNullOrWhiteSpace(timeend))
                    timeend = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.timebegin = timebegin; ViewBag.timeend = timeend; ViewBag.timewatchmin = (timewatchmin == -1 ? "" : timewatchmin + ""); ViewBag.timewatchmax = (timewatchmax == -1 ? "" : timewatchmax + ""); ViewBag.projectname = projectname; ViewBag.logtype = logtype;
                ViewBag.fromip = fromip; ViewBag.sqlip = sqlip; ViewBag.logtag = logtag; ViewBag.orderby = orderby; ViewBag.url = url;
                tb_timewatchlog_dal dal = new tb_timewatchlog_dal();
                PagedList<tb_timewatchlog_model> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    List<tb_timewatchlog_model> List = dal.GetList(PubConn, timebegin, timeend, timewatchmin, timewatchmax, projectname, logtype, logtag, fromip, sqlip, orderby, url,pagesize, pageindex, out count);
                    pageList = new PagedList<tb_timewatchlog_model>(List, pageindex, pagesize, count);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("List", pageList);
                }
                return View(pageList);
            });
        }

        //public ActionResult Chart(string key, int type = 1, int connid = 0)
        //{
        //    return this.ConnVisit(connid, (conn) =>
        //    {
        //        ViewBag.Key = key;
        //        ViewBag.Type = type;
        //        return View();
        //    });
        //}

        public ActionResult ChartLoad(string key, int num = -3, int connid = 0, int type = 1)
        { 
            return this.ConnVisit(connid, (conn) =>
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return Json(new { code = -1, msg = "参数丢失" });
                }
                tb_timewatchlog_dal dal = new tb_timewatchlog_dal();
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    Dictionary<string, List<ChartMainModel>> model = dal.GetMainChartJson(PubConn, num, key, type);
                    return Json(new { code = 1, data = model });
                }
            });
        }

        public ActionResult ChartJson(int connid, string Fday,string Nday, string key, string columnname)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    return Json(new { code = -1, msg = "参数丢失" });
                }
                if (string.IsNullOrWhiteSpace(columnname))
                {
                    return Json(new { code = -1, msg = "请选择比较的参数" });
                }
                try
                {
                    List<string> day = new List<string>();
                    day.Add(Fday);
                    day.Add(Nday);
                    tb_timewatchlog_dal dal = new tb_timewatchlog_dal();
                    Dictionary<string, List<ChartSubModel>> model = new Dictionary<string, List<ChartSubModel>>();
                    using (DbConn PubConn = DbConfig.CreateConn(conn))
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
            });
        }

        public ActionResult Chart(string logtag, string date, XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType type= XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType.ApiUrl, string datatype = "avg", int connid = 0, string columns="time")
        {
            return this.ConnVisit(connid, (conn) =>
          {
              if (string.IsNullOrWhiteSpace(date))
                  date = DateTime.Now.Date.ToString("yyyy-MM-dd");
              ViewBag.type = type; ViewBag.date = date; ViewBag.datatype = datatype; ViewBag.logtag = logtag; ViewBag.connid = connid; 
              List<TimeChartModel> ms = new List<TimeChartModel>();
              if (string.IsNullOrWhiteSpace(logtag))
              {
                  return View(ms);
              } 
              int a=0;
              if(!int.TryParse(logtag,out a))
              {
                  type = XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType.ApiUrl;
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
                  model.Title = "耗时";
                  model.UnitText = "秒";
                  if (model.DataType == TimeChartDataType.count)
                      model.UnitText = "次";

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
                  using (DbConn PubConn = DbConfig.CreateConn(conn))
                    {
                        if (model.Type == TimeChartType.Day)
                        {
                            if (type == XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType.ApiUrl)
                            {
                                tb_timewatchlog_api_dal dal = new tb_timewatchlog_api_dal();
                                dic = dal.GetTimeChart(PubConn, c, model.StartDate, logtag, model.DataType.ToString(), (int)type + "");
                            }
                            else if (type == XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType.SqlCmd)
                            { 
                                tb_timewatchlog_dal dal = new tb_timewatchlog_dal();
                                dic = dal.GetTimeChart(PubConn, c, model.StartDate,logtag, model.DataType.ToString(),(int)type+"");
                            }
                        }
                        else
                        {
                            if (type == XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType.ApiUrl)
                            {
                                tb_timewatchlog_api_dayreport_dal dal = new tb_timewatchlog_api_dayreport_dal();
                                dic = dal.GetTimeChart(PubConn, c, model.StartDate, model.EndDate, logtag, model.DataType.ToString());
                            }
                            else if(type == XXF.BaseService.Monitor.SystemRuntime.EnumTimeWatchLogType.SqlCmd)
                            {
                                tb_timewatchlog_sql_dayreport_dal dal = new tb_timewatchlog_sql_dayreport_dal();
                                dic = dal.GetTimeChart(PubConn, c, model.StartDate, model.EndDate, logtag, model.DataType.ToString());
                            }
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
              return View(ms);
          });
        }
    }
}
