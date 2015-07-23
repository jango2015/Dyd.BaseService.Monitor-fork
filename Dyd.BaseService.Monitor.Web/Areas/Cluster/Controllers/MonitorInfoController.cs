using Dyd.BaseService.Monitor.Domain.Cluster.Dal;
using Dyd.BaseService.Monitor.Domain.Cluster.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using Webdiyer.WebControls.Mvc;
using XXF.Db;
using XXF.ProjectTool;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;

namespace Web.Areas.Cluster.Controllers
{
    [AuthorityCheck]
    public class MonitorInfoController : Controller
    {
        //
        // GET: /Cluster/MonitorInfo/

        public ActionResult Index(string keyword,string serverid, string monitorjsonkeyword,string ifrefreash, int pageindex = 1, int pagesize = 10)
        {
            ViewBag.keyword = keyword; ViewBag.monitorjsonkeyword = monitorjsonkeyword; ViewBag.ifrefreash = ifrefreash;
            
            PagedList<tb_cluster_monitorinfo_model> pageList = null;
            int count = 0;
            List<tb_cluster_monitorinfo_model> List = new List<tb_cluster_monitorinfo_model>();
            using (DbConn PubConn = DbConfig.CreateConn(Config.ClusterConnectString))
            {
                PubConn.Open();
                ViewBag.SqlTimeNow = PubConn.GetServerDate();
                tb_cluster_monitorinfo_dal dal = new tb_cluster_monitorinfo_dal();
                List = dal.GetList(PubConn, keyword,serverid,monitorjsonkeyword, pagesize, pageindex, out count);
            }
            pageList = new PagedList<tb_cluster_monitorinfo_model>(List, pageindex, pagesize, count);
            Dictionary<int,List<Dyd.BaseService.Monitor.Core.CollectConfig>> ServerConfigDic = new Dictionary<int,List<Dyd.BaseService.Monitor.Core.CollectConfig>>();
            SqlHelper.ExcuteSql(Config.PlatformManageConnectString, (c) =>
                {
                    tb_cluster_dal dal = new tb_cluster_dal();
                    var list = dal.GetListInIds(c,List.Select(o=>o.serverid).ToList());
                    foreach(var o in list)
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
