using Dyd.BaseService.Monitor.Domain.TimeWatch.Dal;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Webdiyer.WebControls.Mvc;
using XXF.Db;

namespace Web.Areas.TimeWatchCustomer.Controllers
{
    public class SqlHashContrastUrlController : TimeWatchConnVisit
    {
        //
        // GET: /TimeWatchCustomer/SqlHashContrastUrl/

        public ActionResult Index(string sqlhash, string sql, string url, string orderby = "", int connid = 0, int pagesize = 10, int pageindex = 1)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                ViewBag.sqlhash = sqlhash; ViewBag.sql = sql; ViewBag.url = url; ViewBag.orderby = orderby;
                tb_sqlhash_url_contrast_dal dal = new tb_sqlhash_url_contrast_dal();
                PagedList<tb_sqlhash_url_contrast_model> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    List<tb_sqlhash_url_contrast_model> List = dal.GetList(PubConn, sqlhash, sql, url, orderby, pagesize, pageindex, out count);
                    pageList = new PagedList<tb_sqlhash_url_contrast_model>(List, pageindex, pagesize, count);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("List", pageList);
                }
                return View(pageList);
            });
        }

    }
}
