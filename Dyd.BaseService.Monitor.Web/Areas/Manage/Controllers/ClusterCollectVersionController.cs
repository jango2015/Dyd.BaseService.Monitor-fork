using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Webdiyer.WebControls.Mvc;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using XXF.Db;
using Web.Models;
using System.IO;

namespace Web.Areas.Manage.Controllers
{
    [AuthorityCheck(Roles="Admin")]
    public class ClusterCollectVersionController : Controller
    {
        //
        // GET: /Manage/ClusterCollectVersion/
        public ActionResult Index(int pagesize = 10, int pageindex = 1)
        {
            ViewBag.pagesize = pagesize;
            ViewBag.pageindex = pageindex;
            tb_cluster_collect_version_dal dal = new tb_cluster_collect_version_dal();
            PagedList<tb_cluster_collect_version_model> pageList = null;
            int count = 0;
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                List<tb_cluster_collect_version_model> List = dal.GetList(PubConn, pagesize, pageindex, out count);
                pageList = new PagedList<tb_cluster_collect_version_model>(List, pageindex, pagesize, count);
                ViewBag.MaxVersion = dal.GetMaxVersionNumber(PubConn);
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
        public ActionResult Add(HttpPostedFileBase filedll)
        {
            if (filedll == null)
            {
                ModelState.AddModelError("Error", "请选择上传文件");
                return View();
            }
            Stream dll = filedll.InputStream;
            byte[] dllbyte = new byte[dll.Length];
            dll.Read(dllbyte, 0, Convert.ToInt32(dll.Length));
            try
            {
                tb_cluster_collect_version_dal dal = new tb_cluster_collect_version_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    tb_cluster_collect_version_model model = new tb_cluster_collect_version_model()
                    {
                        versionnum = dal.GetMaxVersion(PubConn),
                        versioncreatetime = DateTime.Now,
                        zipfile = dllbyte,
                        zipfilename = filedll.FileName
                    };
                    dal.Add(PubConn, model);
                    return RedirectToAction("index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View();
            }
        }

        //public ActionResult Update(int id)
        //{
        //    using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
        //    {
        //        PubConn.Open();
        //        tb_cluster_collect_version_model model = new tb_cluster_collect_version_dal().Get(PubConn, id);
        //        return View(model);
        //    }
        //}

        //[HttpPost]
        //public ActionResult Update(tb_cluster_collect_version_model model,httppo)
        //{
        //    tb_cluster_collect_version_dal dal = new tb_cluster_collect_version_dal();
        //    try
        //    {
        //        using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
        //        {
        //            PubConn.Open();
        //            dal.Update(PubConn, model);
        //            return RedirectToAction("index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("Error", ex.Message);
        //        return View(model);
        //    }
        //}

        public JsonResult Delete(int id)
        {
            try
            {
                tb_cluster_collect_version_dal dal = new tb_cluster_collect_version_dal();
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

        public JsonResult EnableID(int id)
        {
            try
            {
                tb_cluster_collect_version_dal dal = new tb_cluster_collect_version_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    dal.ChangePerformVersion(PubConn, id);
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
