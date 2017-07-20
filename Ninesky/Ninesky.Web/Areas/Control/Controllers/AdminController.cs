using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core;
using Ninesky.Core.General;
using Ninesky.Web.Areas.Control.Models;
using Ninesky.Core.Types;

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
            var accounts = GetCookie("Accounts");
            if (accounts != "")
            {
                var _admin = adminManger.Find(accounts);
                Session.Add("AdminID", _admin.AdministratorID);
                Session.Add("Accounts", _admin.Accounts);
                _admin.LoginTime = DateTime.Now;
                _admin.LoginIP = Request.UserHostAddress;
                adminManger.Update(_admin);
                return RedirectToAction("Index");
            }
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
                    SetCookie("Accounts", _admin.Accounts);
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
        #region Cookie 操作
        /// <summary>
        /// cookie 存储
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private void SetCookie(string name, string value)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.HttpOnly = true;
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddMinutes(5);
            HttpContext.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 读取cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetCookie(string name)
        {
            var cookie = HttpContext.Request.Cookies[name];
            if (cookie != null)
            {
                return cookie.Value;
            }
            return "";
        }
        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="name"></param>
        private void ClearCookie(string name)
        {
            var cookie = HttpContext.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Response.Cookies.Add(cookie); //将Cookie写回响应流中
            }
        }
        #endregion

        #endregion

        #region 注销 Logout
        public ActionResult Logout()
        {
            Session.Clear();
            ClearCookie("Accounts");
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

        #region 添加
        public PartialViewResult AddPartialView()
        {
            return PartialView();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("AddPartialView")]
        public JsonResult AddJson(AddAdminViewModel addMin)
        {
            Response _res = new Core.Types.Response();
            if (ModelState.IsValid)
            {
                if (adminManger.HasAccounts(addMin.Accounts))
                {
                    _res.Code = 0;
                    _res.Message = "账号已存在";
                }
                else
                {
                    Administrator _admin = new Administrator()
                    {
                        Accounts = addMin.Accounts,
                        CreateTime = DateTime.Now,
                        Password = Security.Sha256(addMin.Password)
                    };
                    _res = adminManger.Add(_admin);
                }
            }
            else
            {
                _res.Code = 0;
                _res.Message = General.GetModelErrorString(ModelState);
            }
            return Json(_res);
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult DeleteJson(List<int> ids)
        {
            int _total = ids.Count();
            Response _res = new Response();
            int _currentAdminID = int.Parse(Session["AdminID"].ToString());
            if (ids.Contains(_currentAdminID))
            {
                ids.Remove(_currentAdminID);
            }
            _res = adminManger.Delete(ids);
            if (_res.Code == 1 && _res.Data < _total)
            {
                _res.Code = 2;
                _res.Message = "共提交删除" + _total + "名管理员，实际删除" + _res.Data + "名管理员。\n原因：不能删除当前登录的账号";
            }
            else if (_res.Code == 2)
            {
                _res.Message = "共提交删除" + _total + "名管理员，实际删除" + _res.Data + "名管理员。";
            }
            return Json(_res);
        }
        #endregion

        #region 重置密码
        public JsonResult ResetPassword(int id)
        {
            string _password = "angrykun";
            Response _res = adminManger.ChangePassword(id, _password);
            if (_res.Code == 1) _res.Message = "密码重置为" + _password;
            return Json(_res);
        }
        #endregion

        #region 我的资料
        public ActionResult MyInfo()
        {
            return View(adminManger.Find(Session["Accounts"].ToString()));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyInfo(FormCollection form)
        {
            var _admin = adminManger.Find(Session["Accounts"].ToString());
            if (_admin.Password != form["Password"])
            {
                _admin.Password = Security.Sha256(form["Password"].ToString());
                var _resp = adminManger.ChangePassword(_admin.AdministratorID, _admin.Password);
                if (_resp.Code == 1)
                {
                    ViewBag.Message = "<div class=\"alert alert-success\" role=\"alert\"><span class=\"glyphicon glyphicon-ok\"></span>修改密码成功！</div>";
                }
                else ViewBag.Message = "<div class=\"alert alert-danger\" role=\"alert\"><span class=\"glyphicon glyphicon-remove\"></span>修改密码失败！</div>";
            }
            return View(_admin);
        }
        #endregion
    }
}