using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.Db;
using Webdiyer.WebControls.Mvc;
using Web.Models;

namespace Web.Areas.Manage.Controllers
{
    [AuthorityCheck(Roles = "Admin")]
    public class DataBaseConfigController : Controller
    {
        //
        // GET: /Manage/DataBaseConfig/

        public ActionResult Index(string keyword, int dbtype = -1, int pageindex = 1, int pagesize = 10)
        {
            ViewBag.keyword = keyword;
            ViewBag.dbtype = dbtype;
            ViewBag.pagesize = pagesize;
            ViewBag.pageindex = pageindex;
            tb_database_config_dal dal = new tb_database_config_dal();
            PagedList<tb_database_config_model> pageList = null;
            int count = 0;
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                List<tb_database_config_model> List = dal.GetList(PubConn, keyword, dbtype, pagesize, pageindex, out count);
                pageList = new PagedList<tb_database_config_model>(List, pageindex, pagesize, count);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("List", pageList);
            }
            return View(pageList);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(tb_database_config_model model)
        {
            tb_database_config_dal dal = new tb_database_config_dal();
            try
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    dal.Add(PubConn, model);
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(model);
            }
        }

        public ActionResult Update(int id)
        {
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                tb_database_config_model model = new tb_database_config_dal().Get(PubConn, id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Update(tb_database_config_model model)
        {
            tb_database_config_dal dal = new tb_database_config_dal();
            try
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    dal.Edit(PubConn, model);
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(model);
            }
        }

        public JsonResult Delete(int id)
        {
            try
            {
                tb_database_config_dal dal = new tb_database_config_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    dal.Delete(PubConn, id);
                    return Json(new { code = 1, msg = "Success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }
    }
}
