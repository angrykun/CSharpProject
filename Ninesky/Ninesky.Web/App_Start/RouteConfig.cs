using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ninesky.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "Login", id = UrlParameter.Optional },
              namespaces: new string[] { "Ninesky.Web.Controllers" } //命名空间
            ).DataTokens.Add("Area", "Control"); //设置Area下的Control区域为默认首页
        }
    }
}
