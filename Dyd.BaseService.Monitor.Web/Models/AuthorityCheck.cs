using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using XXF.BaseService.Monitor.SystemRuntime;

namespace Web.Models
{
    public class AuthorityCheck : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                string url = httpContext.Request.Path;
                url = url.Substring(0, url.IndexOf("?") > 1 ? url.IndexOf("?") : url.Length);
                HttpCookie authcookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authcookie.Value);
                    string userid = ticket.Name.Split(' ').FirstOrDefault();
                    if (this.Roles == "Admin")
                    {
                        if (Convert.ToInt32(ticket.Name.Split(' ').LastOrDefault()) == (int)EnumUserRole.Admin)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch (Exception exp)
            {
                return false;
            }
        }
    }
}