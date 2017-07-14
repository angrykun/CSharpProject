using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ninesky.Web.Areas.Control.Controllers
{
    /// <summary>
    /// 主控制器
    /// </summary>
    [AdminAuthorize]
    public class HomeController : Controller
    {
        // GET: Control/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}