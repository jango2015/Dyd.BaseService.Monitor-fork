using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dyd.BaseService.Monitor.Core;
using Web.Models;

namespace Web.Areas.Manage.Controllers
{
    public class OpenApiController : Controller
    {
        //
        // GET: /Manage/OpenApi/

        public ActionResult GetConfig()
        {
            return Json(new { code = 1, msg = "", data = new { PlatformManageConnectString = StringDESHelper.EncryptDES(Models.Config.PlatformManageConnectString, "dyd888888") }, total = 0 }, JsonRequestBehavior.AllowGet);
            //return Json(new { PlatformManageConnectString = StringDESHelper.EncryptDES(Models.Config.PlatformManageConnectString,"dyd888888") });
        }

    }
}
