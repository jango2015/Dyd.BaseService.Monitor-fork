using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using XXF.Db;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using XXF.BaseService.Monitor.SystemRuntime;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;

namespace Web.Models
{
    public class TimeWatchConnVisit : Controller
    {
        public ActionResult ConnVisit(int connid ,Func<string, ActionResult> action)
        {
            Dictionary<int, timewatchConnModel> timeWatchConn = new Dictionary<int, timewatchConnModel>();
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                timeWatchConn = new tb_database_config_dal().GetDataBaseSqlConnDic(PubConn, (int)DataBaseType.Timewatch);
            }
            if (timeWatchConn.Count > 0)
            {
                ViewBag.TimeWatchConn = timeWatchConn;
                ViewBag.Connid = connid;
                string pointConn = connid == 0 ? timeWatchConn.First().Value.connect : timeWatchConn[connid].connect;
                return action.Invoke(pointConn);//.Replace("10.251.248.64", "121.40.29.71")
            }
            else
            {
                throw new Exception("没有找到耗时库连接！");
            }
        }
    }
}