using CMS1_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS1_Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorize : Attribute, IAuthorizationFilter
    {

        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            tblUser account = context.HttpContext.Session.GetObject<tblUser>(myCommon.hsSessionName);
            var lstRole = Roles.Split(",");
            if (account != null)
            {
                if (!lstRole.Any(r => r.Contains(account.Role)))
                {
                    context.HttpContext.Response.Redirect("/access-denied");
                }
            }
        }

        public static void SetLoggedUser(HttpContext context, tblUser user)
        {
            context.Session.SetObject(myCommon.hsSessionName, user);
            context.Response.Cookies.Append(myCommon.hsCookieName, user.Id.ToString(), new CookieOptions()
            {
                Expires = DateTime.Now.AddHours(8),
            });
        }

        public static tblUser GetLoggedUser(HttpContext context)
        {
            if (context.Session.GetObject<tblUser>(myCommon.hsSessionName) != null)
            {
                return context.Session.GetObject<tblUser>(myCommon.hsSessionName);
            }
            else
            {
                if (context.Request.Cookies[myCommon.hsCookieName] != null && context.Request.Cookies[myCommon.hsCookieName] != "")
                {
                    int userId = int.Parse(context.Request.Cookies[myCommon.hsCookieName]);
                    tblUser user = new myfunc().GetUserbyId(userId);
                    context.Session.SetObject(myCommon.hsSessionName, user);
                    return user;
                }
            }
            return null;
        }


        public static void LogOut(HttpContext context)
        {
            context.Session.Remove(myCommon.hsSessionName);
            context.Response.Cookies.Delete(myCommon.hsCookieName);
            context.Response.Redirect("/dang-nhap");
        }

    }
}
