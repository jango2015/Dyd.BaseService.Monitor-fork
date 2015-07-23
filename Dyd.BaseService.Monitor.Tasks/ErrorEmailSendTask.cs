using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Dal;
using Dyd.BaseService.Monitor.Domain.PlatformManage.Model;
using Dyd.BaseService.Monitor.Domain.UnityLog.Dal;
using Dyd.BaseService.Monitor.Domain.UnityLog.Model;
using Dyd.BaseService.Monitor.Tasks.Tool;
using XXF.ProjectTool;

namespace Dyd.BaseService.Monitor.Tasks
{
    /// <summary>
    /// 错误邮件发送任务
    /// </summary>
    public class ErrorEmailSendTask : XXF.BaseService.TaskManager.BaseDllTask
    {
        public override void TestRun()
        {
            this.AppConfig = new XXF.BaseService.TaskManager.SystemRuntime.TaskAppConfigInfo();
            this.AppConfig.Add("MonitorPlatformManageConnectString", "server=192.168.17.201;Initial Catalog=dyd_bs_monitor_platform_manage;User ID=sa;Password=Xx~!@#;");
            this.AppConfig.Add("sendmailhost", "smtp.163.com");
            this.AppConfig.Add("sendmailname", "fengyeguigui@163.com");
            this.AppConfig.Add("password", "472790378@");
            this.AppConfig.Add("moreEmailSubject", "测试环境");

            string json = new XXF.Serialization.JsonHelper().Serializer(this.AppConfig);

            base.TestRun();
        }

        private string UnityLogConnectString = "";
        private DateTime lastscantime = DateTime.Now;
        public override void Run()
        {
            GlobalConfig.MonitorPlatformManageConnectString = AppConfig["MonitorPlatformManageConnectString"];

            SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
            {
                tb_database_config_dal dal = new tb_database_config_dal();
                var list = dal.GetModelList(c);
                UnityLogConnectString = XXF.BaseService.Monitor.SystemRuntime.DbShardingHelper.GetDataBase(list, XXF.BaseService.Monitor.SystemRuntime.DataBaseType.UnityLog);
            });

            List<tb_user_model> users = new List<tb_user_model>();
            SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
            {
                tb_user_dal dal = new tb_user_dal();
                users = dal.GetAllUsers(c);

            });
            foreach (var u in users)
            {
                if (!string.IsNullOrWhiteSpace(u.useremail))
                {
                    if (u.lastsenderrortime.AddMinutes(u.errorsendintervaltime) < DateTime.Now)
                    {
                        List<tb_error_log_model> errors = new List<tb_error_log_model>();
                        SqlHelper.ExcuteSql(UnityLogConnectString, (c) =>
                        {
                            tb_error_log_dal dal = new tb_error_log_dal();
                            if (u.userrole == (byte)XXF.BaseService.Monitor.SystemRuntime.EnumUserRole.Admin)
                                errors = dal.GetUserErrors(c, u.lastsenderrortime, "", "", 50);
                            else
                                errors = dal.GetUserErrors(c, u.lastsenderrortime, u.username.Trim(), u.userstaffno.Trim(), 50);
                        });
                        SendEmail(errors, u.useremail);
                        SqlHelper.ExcuteSql(GlobalConfig.MonitorPlatformManageConnectString, (c) =>
                        {
                            tb_user_dal dal = new tb_user_dal();
                            dal.UpdateLastErrorSendTime(c, u.id, DateTime.Now);
                        });
                    }
                }
            }

        }

        public void SendEmail(List<tb_error_log_model> errors, string email)
        {
            StringBuilder sb = new StringBuilder();
            if (errors != null)
                foreach (var e in errors)
                {
                    sb.AppendLine(string.Format("【错误id】{0}【项目】{1}【标识信息】{2}【错误内容】{3}【开发人员】{4}【创建时间】{5}<br/><br/>",
                        e.id, e.projectname, e.logtag, e.msg, e.developer, e.logcreatetime));
                }
            string content = sb.ToString();
            if (string.IsNullOrEmpty(content))
                return;
            content += "\r\n详情请查看错误信息表!";

            EmailHelper emailhelper = new EmailHelper();
            emailhelper.mailFrom = this.AppConfig["sendmailname"];
            emailhelper.mailPwd = this.AppConfig["password"];
            emailhelper.mailSubject = string.Format("监控平台之错误日报" + DateTime.Now.ToString("yyyyMMddHHmmss") + "【系统邮件】【{0}】", this.AppConfig["moreEmailSubject"]);
            emailhelper.mailBody = content;
            emailhelper.isbodyHtml = true;    //是否是HTML
            emailhelper.host = this.AppConfig["sendmailhost"];//如果是QQ邮箱则：smtp:qq.com,依次类推
            emailhelper.mailToArray = new string[] { email };//接收者邮件集合
            emailhelper.mailCcArray = new string[] { };//抄送者邮件集合
            try
            {
                emailhelper.Send();
            }
            catch (Exception exp)
            {
                OpenOperator.Error("发送错误邮件错误", exp);
            }
        }


    }
}
