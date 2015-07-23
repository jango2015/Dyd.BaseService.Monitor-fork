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
using XXF.ProjectTool;

namespace Web.Areas.Cluster.Controllers
{
    public class MonitorinfoSnapshotController : Controller
    {
        //
        // GET: /Cluster/MonitorinfoSnapshot/

        public ActionResult Index(string keyword, string serverid, string timebegin, string timeend, string monitorjsonkeyword, int pageindex = 1, int pagesize = 10)
        {
            if (string.IsNullOrWhiteSpace(timebegin))
                timebegin = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrWhiteSpace(timeend))
                timeend = DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.keyword = keyword; ViewBag.serverid = serverid; ViewBag.timebegin = timebegin; ViewBag.timeend = timeend; ViewBag.monitorjsonkeyword = monitorjsonkeyword;

            PagedList<tb_cluster_monitorinfo_snapshot_model> pageList = null;
            int count = 0;
            List<tb_cluster_monitorinfo_snapshot_model> List = new List<tb_cluster_monitorinfo_snapshot_model>();
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
            {
                PubConn.Open();
                ViewBag.SqlTimeNow = PubConn.GetServerDate();
                tb_cluster_monitorinfo_snapshot_dal dal = new tb_cluster_monitorinfo_snapshot_dal();
                List = dal.GetList(PubConn, keyword, serverid, timebegin,timeend,monitorjsonkeyword, pagesize, pageindex, out count);
            }
            pageList = new PagedList<tb_cluster_monitorinfo_snapshot_model>(List, pageindex, pagesize, count);
            Dictionary<int, List<Dyd.BaseService.Monitor.Core.CollectConfig>> ServerConfigDic = new Dictionary<int, List<Dyd.BaseService.Monitor.Core.CollectConfig>>();
            SqlHelper.ExcuteSql(Config.PlatformManageConnectString, (c) =>
            {
                tb_cluster_dal dal = new tb_cluster_dal();
                var list = dal.GetListInIds(c, List.Select(o => o.serverid).ToList());
                foreach (var o in list)
                {
                    var config = new XXF.Serialization.JsonHelper().Deserialize<List<Dyd.BaseService.Monitor.Core.CollectConfig>>(o.monitorcollectconfigjson);
                    ServerConfigDic.Add(o.id, config);
                }
            });
            ViewBag.ServerConfigDic = ServerConfigDic;
            if (Request.IsAjaxRequest())
            {
                return PartialView("List", pageList);
            }
            return View(pageList);
        }

    }
}
