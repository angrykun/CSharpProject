using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core;
using Ninesky.Core.Types;
using Ninesky.Web.Areas.Control.Models;
using Ninesky.Web.Models;
using Ninesky.Core.General;

namespace Ninesky.Web.Areas.Control.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [AdminAuthorize]
    public class UserController : Controller
    {
        private UserManager userManager = new UserManager();

        // GET: Control/User
        public ActionResult Index()
        {
            return View();
        }
        #region 分页列表【json】
        /// <summary>
        /// 分页列表【json】
        /// </summary>
        /// <param name="roleID">角色ID</param>
        /// <param name="username">用户名</param>
        /// <param name="name">名称</param>
        /// <param name="sex">性别</param>
        /// <param name="email">Email</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="order">排序</param>
        /// <returns>Json</returns>
        public ActionResult PageListJson(int? roleID, string username, string name, int? sex, string email, int? pageNumber, int? pageSize, int? order)
        {
            Paging<User> _pagingUser = new Paging<Core.User>();
            if (pageNumber != null && pageNumber > 0) _pagingUser.PageIndex = (int)pageNumber;
            if (pageSize != null & pageSize > 0) _pagingUser.PageSize = (int)pageSize;
            var _paging = userManager.FindPageList(_pagingUser, roleID, username, name, sex, email, null);
            return Json(new { total = _pagingUser.TotalNumber, rows = _pagingUser.Items });
        }
        #endregion

        #region 验证是否可用
        /// <summary>
        /// 用户名是否可用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CanUserName(string userName)
        {
            return Json(!userManager.HasUserName(userName));
        }
        /// <summary>
        /// 邮箱是否可用
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CanEmail(string email)
        {
            return Json(!userManager.HasEmail(email));
        }
        #endregion

        #region Add
        public ActionResult Add()
        {
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var role in _roles)
            {
                _listItems.Add(new SelectListItem()
                {
                    Text = role.Name,
                    Value = role.RoleID.ToString()
                });
            }
            ViewBag.Roles = _listItems;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUserViewModel userViewModel)
        {
            if (userManager.HasUserName(userViewModel.UserName)) ModelState.AddModelError("UserName", "用户名已存在");
            if (userManager.HasEmail(userViewModel.Email)) ModelState.AddModelError("Email", "Email已存在");
            if (ModelState.IsValid)
            {
                User _user = new Core.User()
                {
                    RoleID = userViewModel.RoleID,
                    UserName = userViewModel.UserName,
                    Name = userViewModel.Name,
                    Sex = userViewModel.Sex,
                    Password = Core.General.Security.Sha256(userViewModel.Password),
                    Email = userViewModel.Email,
                    RegTime = DateTime.Now
                };

                var _response = userManager.Add(_user);
                if (_response.Code == 1) return View("Prompt", new Prompt()
                {
                    Title = "添加用户成功",
                    Message = "您已成功添加了用户【" + _response.Data.UserName + "（" + _response.Data.Name + "）】",
                    Buttons = new List<string> {"<a href=\"" + Url.Action("Index", "User") + "\" class=\"btn btn-default\">用户管理</a>",
                 "<a href=\"" + Url.Action("Details", "User",new { id= _response.Data.UserID }) + "\" class=\"btn btn-default\">查看用户</a>",
                 "<a href=\"" + Url.Action("Add", "User") + "\" class=\"btn btn-default\">继续添加</a>"}
                });
                else ModelState.AddModelError("", _response.Message);
            }
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束

            return View(userViewModel);

        }
        #endregion

        #region Modify
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>      
        public ActionResult Modify(int id)
        {
            //角色列表
            var _roles = new RoleManager().FindList();
            List<SelectListItem> _listItems = new List<SelectListItem>(_roles.Count());
            foreach (var _role in _roles)
            {
                _listItems.Add(new SelectListItem() { Text = _role.Name, Value = _role.RoleID.ToString() });
            }
            ViewBag.Roles = _listItems;
            //角色列表结束
            return PartialView(userManager.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify(int id, FormCollection form)
        {
            Response _resp = new Core.Types.Response();
            var _user = userManager.Find(id);
            if (TryUpdateModel(_user, new string[] { "RoleID", "Name", "Sex", "Email" }))
            {
                if (_user == null)
                {
                    _resp.Code = 0;
                    _resp.Message = "用户不存在，可能已被删除，请刷新后重试";
                }
                else
                {
                    if (_user.Password != form["Password"].ToString())
                    {
                        _user.Password = Security.Sha256(form["Password"].ToString());
                    }
                    _resp = userManager.Update(_user);
                }
            }
            else
            {
                _resp.Code = 0;
                _resp.Message = General.GetModelErrorString(ModelState);
            }
            return Json(_resp);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            return Json(userManager.Delete(id));
        }
        #endregion
    }
}