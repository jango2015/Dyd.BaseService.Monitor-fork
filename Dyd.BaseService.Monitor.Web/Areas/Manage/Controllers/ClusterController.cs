using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Webdiyer.WebControls.Mvc;
using XXF.Db;

namespace Web.Areas.Manage.Controllers
{
    [AuthorityCheck(Roles = "Admin")]
    public class ClusterController : Controller
    {
        //
        // GET: /Manage/Manage/

        public ActionResult Index(string id,string keyword, string CStime, string CEtime, int pagesize = 10, int pageindex = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.CStime = CStime;
            ViewBag.CEtime = CEtime;
            ViewBag.pagesize = pagesize;
            ViewBag.pageindex = pageindex;
            ViewBag.SqlTimeNow = DateTime.Now;
            ViewBag.id = id;
            tb_cluster_dal dal = new tb_cluster_dal();
            PagedList<tb_cluster_model> pageList = null;
            int count = 0;
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                List<tb_cluster_model> List = dal.GetList(PubConn, id,keyword, CStime, CEtime, pagesize, pageindex, out count);
                using (DbConn MonitorConn = DbConfig.CreateConn(Config.ClusterConnectString))
                {
                    MonitorConn.Open();
                    tb_cluster_monitorinfo_dal monitordal = new tb_cluster_monitorinfo_dal();
                    for (int i = 0; i < List.Count; i++)
                    {
                        List[i].MonitorJson = monitordal.GetServerMonitorJson(MonitorConn, List[i].id);
                    }
                }
                pageList = new PagedList<tb_cluster_model>(List, pageindex, pagesize, count);
                ViewBag.SqlTimeNow = PubConn.GetServerDate();
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
        public ActionResult Add(tb_cluster_model model)
        {
            tb_cluster_dal dal = new tb_cluster_dal();
            try
            {
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    if (dal.CheckServerIp(PubConn, model.serverip))
                    {
                        CheckJsonFormat(model);
                        model.createtime = PubConn.GetServerDate();
                        model.lastupdatetime = PubConn.GetServerDate();
                        model.onlinetime = PubConn.GetServerDate().AddDays(-1);
                        dal.Add(PubConn, model);
                        return RedirectToAction("index");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "IP已存在");
                        return View(model);
                    }
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
                tb_cluster_model model = new tb_cluster_dal().Get(PubConn, id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Update(tb_cluster_model model)
        {
            try
            {
                tb_cluster_dal dal = new tb_cluster_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    if (dal.CheckServerIp(PubConn, model.serverip,model.id))
                    {
                        CheckJsonFormat(model);
                        model.onlinetime = PubConn.GetServerDate().AddDays(-1);
                        model.lastupdatetime = PubConn.GetServerDate();
                        dal.Update(PubConn, model);
                        return RedirectToAction("index");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "IP已存在");
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
                return View(model);
            }
        }

        private void CheckJsonFormat(tb_cluster_model model)
        {
            if (!string.IsNullOrWhiteSpace(model.monitorcollectconfigjson))
            {
                try {
                    new XXF.Serialization.JsonHelper().Deserialize<List<Dyd.BaseService.Monitor.Core.CollectConfig>>(model.monitorcollectconfigjson);
                }
                catch (Exception exp)
                {
                    throw new Exception("监控配置json格式不对");
                }
            }
            if (!string.IsNullOrWhiteSpace(model.performancecollectconfigjson))
            {
                try
                {
                    new XXF.Serialization.JsonHelper().Deserialize<List<Dyd.BaseService.Monitor.Core.CollectConfig>>(model.performancecollectconfigjson);
                }
                catch (Exception exp)
                {
                    throw new Exception("性能检测配置json格式不对");
                }
            }
        }

        public JsonResult Delete(int id)
        {
            try
            {
                tb_cluster_dal dal = new tb_cluster_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    var o = dal.Get(PubConn, id);
                    if ((PubConn.GetServerDate() - o.onlinetime) < TimeSpan.FromSeconds(10))
                    {
                        throw new Exception("请先关闭监控，并确认当前服务器的服务已停止！");
                    }
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
