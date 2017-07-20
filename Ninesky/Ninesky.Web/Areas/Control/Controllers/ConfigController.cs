using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core.Config;
using Ninesky.Web.Models;

namespace Ninesky.Web.Areas.Control.Controllers
{
    /// <summary>
    /// 站点设置
    /// </summary>
    public class ConfigController : Controller
    {
        // GET: Control/Config
        public ActionResult SiteConfig()
        {
            SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as Ninesky.Core.Config.SiteConfig;
            return View(_siteConfig);
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SiteConfig(FormCollection form)
        {
            SiteConfig _siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as Ninesky.Core.Config.SiteConfig;
            if (TryUpdateModel<SiteConfig>(_siteConfig))
            {
                _siteConfig.CurrentConfiguration.Save();
                return View("Prompt", new Prompt() { Title = "修改成功", Message = "成功修改了网站设置", Buttons = new List<string> { "<a href='" + Url.Action("SiteConfig") + "' class='btn btn-default'>返回</a>" } });
            }
            else return View(_siteConfig);
        }
    }
}