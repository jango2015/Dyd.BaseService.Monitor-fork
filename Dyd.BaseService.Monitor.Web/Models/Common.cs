using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XXF.Db;

namespace Web.Models
{
    public class Common
    {
        public static tb_user_model GetUserName(string userstaffno)
        {
            using (DbConn PubConn = DbConfig.CreateConn(Config.PlatformManageConnectString))
            {
                PubConn.Open();
                tb_user_dal dal = new tb_user_dal();
                return dal.GetUserName(PubConn, userstaffno);
            }
        }
    }
}