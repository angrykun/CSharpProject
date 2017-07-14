using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core;
using Ninesky.Core.General;
using Ninesky.Web.Areas.Control.Models;

namespace Ninesky.Web.Areas.Control.Controllers
{

    [AdminAuthorize]
    public class AdminController : Controller
    {
        private AdministratorManager adminManger = new AdministratorManager();

        #region 登录 Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string _password = Security.Sha256(loginViewModel.Password);
                var _response = adminManger.Verify(loginViewModel.Accounts, _password);
                if (_response.Code == 1)
                {
                    var _admin = adminManger.Find(loginViewModel.Accounts);
                    Session.Add("AdminID", _admin.AdministratorID);
                    Session.Add("Accounts", _admin.Accounts);
                    _admin.LoginTime = DateTime.Now;
                    _admin.LoginIP = Request.UserHostAddress;
                    adminManger.Update(_admin);
                    return RedirectToAction("Index");
                }
                else if (_response.Code == 2) ModelState.AddModelError("Accounts", _response.Message);
                else if (_response.Code == 3) ModelState.AddModelError("Password", _response.Message);
                else ModelState.AddModelError("", _response.Message);
            }

            return View(loginViewModel);
        }
        #endregion

        #region 注销 Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListJson()
        {
            return Json(adminManger.FindList());
        }
    }
}