using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ninesky.Web.Areas.Control
{
    /// <summary>
    /// 管理员身份验证类
    /// AdminAuthorizeAttribute 继承自AuthorizeAttribute
    /// 重写AuthorizeCore方法，通过Session["AdminID"]判断管理员是否已经登录
    /// 重写HandleUnahthorizeRequest方法来处理未登录时的页面跳转
    /// </summary>
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写自定义授权检查
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["AdminID"] == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 重写未授权的HTTP请求处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Control/Admin/Login");
        }
    }
}