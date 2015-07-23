using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Domain.UnityLog.Dal;
using Dyd.BaseService.Monitor.Domain.UnityLog.Model;
using XXF.Db;
using Webdiyer.WebControls.Mvc;
using Web.Models;

namespace Web.Areas.UnityLog.Controllers
{
    [AuthorityCheck]
    public class ErrorLogController : Controller
    {
        //
        // GET: /UnityLog/ErrorLog/

        public ActionResult Index(string id,string keyword, string month, string timebegin, string timeend, string projectname, int logtype = -1, int pageindex = 1, int pagesize = 10)
        {
            ViewBag.keyword = keyword; ViewBag.logtype = logtype; ViewBag.projectname = projectname;
            ViewBag.id = id;
            if (!string.IsNullOrWhiteSpace(timebegin))
                ViewBag.timebegin = DateTime.Parse(timebegin);
            else
                ViewBag.timebegin = DateTime.Now.Date;
            if (!string.IsNullOrWhiteSpace(timeend))
                ViewBag.timeend = DateTime.Parse(timeend);
            else
                ViewBag.timeend = DateTime.Now.Date.AddDays(1);
            tb_error_log_dal dal = new tb_error_log_dal();
            PagedList<tb_error_log_model> pageList = null;
            int count = 0;
            using (DbConn PubConn = DbConfig.CreateConn(Config.UnityLogConnectString))
            {
                PubConn.Open();
                List<tb_error_log_model> List=new List<tb_error_log_model>();
                try
                {
                    List = dal.GetList(PubConn, id,keyword, ViewBag.timebegin, ViewBag.timeend, logtype, projectname, pagesize, pageindex, out count);
                }
                catch
                {
                    List = new List<tb_error_log_model>();
                }
                pageList = new PagedList<tb_error_log_model>(List, pageindex, pagesize, count);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("List", pageList);
            }
            return View(pageList);
        }

    }
}
