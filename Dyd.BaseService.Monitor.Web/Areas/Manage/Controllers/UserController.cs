using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using XXF.Db;
using XXF.Extensions;

namespace Web.Areas.Manage.Controllers
{
    [AuthorityCheck(Roles = "Admin")]
    public class UserController : Controller
    {
        //
        // GET: /Manage/User/

        public ActionResult Index()
        {
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                List<tb_user_model> Model = new tb_user_dal().GetAllUsers(PubConn);
                return View(Model);
            }
        }

        public ActionResult Add(int? userid)
        {
            if (userid == null)
                return View();
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                tb_user_dal dal = new tb_user_dal();

                var model = dal.Get(PubConn, userid.Value);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Add(tb_user_model model)
        {
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                tb_user_dal dal = new tb_user_dal();
                model.usercreatetime = DateTime.Now;
                model.lastsenderrortime = DateTime.Now;
                model.errorsendintervaltime =( model.errorsendintervaltime==0?1:model.errorsendintervaltime);
                model.usertel = model.usertel.NullToEmpty();
                model.useremail = model.useremail.NullToEmpty();
                if (model.id == 0)
                    dal.Add(PubConn, model);
                else
                    dal.Edit(PubConn, model);
            }
            return RedirectToAction("index");
        }

        public JsonResult Delete(int id)
        {
            try
            {
                tb_user_dal dal = new tb_user_dal();
                using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
                {
                    PubConn.Open();
                    bool state = dal.Delete(PubConn, id);
                    return Json(new { code = 1, state = state });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, msg = ex.Message });
            }
        }
    }
}
