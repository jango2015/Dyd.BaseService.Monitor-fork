﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Model;
using Dyd.BaseService.Monitor.Domain.TimeWatch.Dal;
using Webdiyer.WebControls.Mvc;
using XXF.Db;
using Web.Models;

namespace Web.Areas.TimeWatchCustomer.Controllers
{
    [AuthorityCheck]
    public class SqlHashNoParamController : TimeWatchConnVisit
    {
        //
        // GET: /TimeWatchCustomer/SqlHashNoParam/

        public ActionResult Index(string sqlhash, string sql, string orderby = "count desc", int connid = 0, int pagesize = 15, int pageindex = 1)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                ViewBag.sqlhash = sqlhash; ViewBag.sql = sql; ViewBag.orderby = orderby;
                tb_sqlhash_noparam_dal dal = new tb_sqlhash_noparam_dal();
                PagedList<tb_sqlhash_noparam_model> pageList = null;
                int count = 0;
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    List<tb_sqlhash_noparam_model> List = dal.GetList(PubConn, sqlhash, sql, orderby,pagesize, pageindex, out count);
                    pageList = new PagedList<tb_sqlhash_noparam_model>(List, pageindex, pagesize, count);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("List", pageList);
                }
                return View(pageList);
            });
        }

        public ActionResult Chart(int connid = 0)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                return View();
            });
        }

        public ActionResult ChartJson(int connid, int count = 0)
        {
            return this.ConnVisit(connid, (conn) =>
            {
                tb_sqlhash_noparam_dal dal = new tb_sqlhash_noparam_dal();
                List<tb_sqlhash_noparam_model> model = new List<tb_sqlhash_noparam_model>();
                using (DbConn PubConn = DbConfig.CreateConn(conn))
                {
                    PubConn.Open();
                    model = dal.GetChartJson(PubConn, count);
                }
                return Json(new { data = model });
            });
        }
    }
}
