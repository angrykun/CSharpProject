using Ninesky.Core.Category;
using Ninesky.DataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninesky.Core.Types;
using Ninesky.Core.Content;

namespace Ninesky.Web.Areas.Control.Controllers
{
    /// <summary>
    /// 栏目控制器
    /// </summary>
    [AdminAuthorize]
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager;

        public CategoryController()
        {
            categoryManager = new CategoryManager();
        }

        // GET: Control/Category
        public ActionResult Index()
        {
            return View("Add");
        }

        #region 展示左侧树形列表
        /// <summary>
        /// 树形节点数据
        /// </summary>
        /// <param name="showIcon"></param>
        /// <returns></returns>
        public ActionResult Tree(bool showIcon = false)
        {
            List<TreeNode> _nodes = new List<TreeNode>();
            //获取栏目并进行排序 使最深的节点排在最前面
            var _categories = categoryManager.FindList(0, null, new OrderParam[] { new OrderParam() { Method = OrderMethod.ASC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            TreeNode _node;
            //遍历常规栏目
            foreach (var _category in _categories)
            {
                _node = new TreeNode()
                {
                    pId = _category.ParentID,
                    id = _category.CategoryID,
                    name = _category.Name,
                    url = Url.Action("Modify", "Category", new { id = _category.CategoryID })
                };
                if (showIcon)
                {
                    switch (_category.Type)
                    {
                        case CategoryType.General:
                            _node.icon = Url.Content("~/Content/img/metro.png");
                            break;
                        case CategoryType.Page:
                            _node.icon = Url.Content("~/Content/img/metro.png");
                            break;
                        case CategoryType.Link:
                            _node.icon = Url.Content("~/Content/img/metro.png");
                            break;

                    }
                }
                _nodes.Add(_node);
            }

            return Json(_nodes);
        }
        #endregion
        #region 组合树
        /// <summary>
        /// 组合树
        /// </summary>
        /// <returns></returns>
        //public ActionResult DropdownTreeList()
        //{
        //    List<TreeNode> _nodes = new List<TreeNode>();
        //    //栏目并进行排序使最深的节点排在最前
        //    var _categories = categoryManager.FindList(0, CategoryType.General, new OrderParam[] { new OrderParam() { Method = OrderMethod.DESC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
        //    TreeNode _node;
        //    //遍历常规栏目
        //    foreach (var _category in _categories)
        //    {
        //        _node = new TreeNode() { parentid = _category.ParentID, value = _category.CategoryID, id = "node_" + _category.CategoryID, label = _category.Name, html = Url.Action("Detials", "Category", new { @id = _category.CategoryID }) };
        //        if (_nodes.Exists(n => n.parentid == _category.CategoryID))
        //        {
        //            var _children = _nodes.Where(n => n.parentid == _category.CategoryID).ToList();
        //            _nodes.RemoveAll(n => n.parentid == _category.CategoryID);
        //            _node.items = _children;
        //            _node.expanded = false;
        //            _nodes.Add(_node);
        //        }
        //        else _nodes.Add(_node);
        //    }
        //    _nodes.Insert(0, new TreeNode() { id = "node_0", value = 0, label = "无" });
        //    return Json(_nodes);
        //}

        //public ActionResult zTree(bool showIcon = false)
        //{
        //    List<TreeNode> _nodes = new List<TreeNode>();
        //    //栏目并进行排序使最深的节点排在最前
        //    var _categories = categoryManager.FindList(0, null, new OrderParam[] { new OrderParam() { Method = OrderMethod.DESC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
        //    TreeNode _node;
        //    //遍历常规栏目
        //    foreach (var _category in _categories)
        //    {
        //        _node = new TreeNode() { parentid = _category.ParentID, value = _category.CategoryID, id = "node_" + _category.CategoryID, label = _category.Name, html = Url.Action("Detials", "Category", new { @id = _category.CategoryID }) };
        //        if (showIcon)
        //        {
        //            switch (_category.Type)
        //            {
        //                case CategoryType.General:
        //                    _node.icon = Url.Content("~/Content/Images/folder.png");
        //                    break;
        //                case CategoryType.Page:
        //                    _node.icon = Url.Content("~/Content/Images/page.png");
        //                    break;
        //                case CategoryType.Link:
        //                    _node.icon = Url.Content("~/Content/Images/link.png");
        //                    break;
        //            }
        //        }
        //        _nodes.Add(_node);
        //    }

        //    return Json(_nodes);
        //}
        #endregion
        #region 下拉树【常规栏目】
        public ActionResult DropdownTree()
        {
            //获取栏目并进行排序 使最深的节点排在最前面
            var _categories = categoryManager.FindList(0, null, new OrderParam[] { new OrderParam() { Method = OrderMethod.ASC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            List<TreeNode> _nodes = new List<TreeNode>();
            //遍历常规栏目
            foreach (var _category in _categories)
            {
                _nodes.Add(new TreeNode()
                {
                    pId = _category.ParentID,
                    id = _category.CategoryID,
                    name = _category.Name
                });
            }
            return Json(_nodes);
        }
        #endregion

        #region 添加
        public ActionResult Add(int? id)
        {
            Category _category = new Category() { ParentID = 0 };

            if (id != null && id > 0)
            {
                var _parent = categoryManager.Find((int)id);
                if (_parent != null && _parent.Type == CategoryType.General)
                {
                    _category.ParentID = (int)id;
                }
            }
            return View(_category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add()
        {
            Category _category = new Category();
            TryUpdateModel(_category, new string[] { "Type", "ParentID", "Name", "Description", "Order", "Target" });
            if (ModelState.IsValid)
            {
                //检查父栏目
                if (_category.ParentID > 0)
                {
                    var _parentCategory = categoryManager.Find(_category.ParentID);
                    if (_parentCategory == null) ModelState.AddModelError("ParentID", "父栏目不存在，请刷新后重新添加");
                    else if (_parentCategory.Type != CategoryType.General) ModelState.AddModelError("ParentID", "父栏目不允许添加子栏目");
                    else
                    {
                        _category.ParentPath = _parentCategory.ParentPath + "," + _parentCategory.CategoryID;
                        _category.Depth = _parentCategory.Depth + 1;
                    }
                }
                else
                {
                    _category.ParentPath = "0";
                    _category.Depth = 0;
                }
                //栏目基本信息保存
                Response _response = new Response() { Code = 0, Message = "初始失败信息" };
                //根据栏目类型进行处理
                switch (_category.Type)
                {
                    case CategoryType.General:
                        var _general = new CategoryGeneral();
                        TryUpdateModel(_general);
                        _response = categoryManager.Add(_category, _general);
                        break;
                    case CategoryType.Page:
                        var _page = new CategoryPage();
                        TryUpdateModel(_page);
                        _response = categoryManager.Add(_category, _page);
                        break;
                    case CategoryType.Link:
                        var _link = new CategoryLink();
                        TryUpdateModel(_link);
                        _response = categoryManager.Add(_category, _link);
                        break;
                }
                if (_response.Code == 1) return View("Prompt", new Ninesky.Web.Models.Prompt() { Title = "添加栏目成功", Message = "添加栏目【" + _category.Name + "】成功" });
                else return View("Prompt", new Ninesky.Web.Models.Prompt() { Title = "添加失败", Message = "添加栏目【" + _category.Name + "】时发生系统错误，未能保存到数据库，请重试" });
            }

            return View(_category);
        }
        /// <summary>
        /// 常规栏目信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddGeneral()
        {
            var _general = new CategoryGeneral() { ContentView = "Index", View = "Index" };
            List<SelectListItem> _contentTypeItems = new List<SelectListItem>();
            ContentTypeManager _contentTypeManager = new ContentTypeManager();
            var _contentTypes = _contentTypeManager.FindList();
            foreach (var contentType in _contentTypes)
            {
                _contentTypeItems.Add(new SelectListItem() { Value = contentType.ContentTypeID.ToString(), Text = contentType.Name });
            }
            ViewBag.ContentTypeItems = _contentTypeItems;
            return PartialView(_general);
        }

        /// <summary>
        /// 添加单页栏目
        /// </summary>
        /// <returns></returns>
        public ActionResult AddPage()
        {
            var _page = new CategoryPage() { View = "Index" };
            return PartialView(_page);
        }
        /// <summary>
        /// 添加外部链接
        /// </summary>
        /// <returns></returns>
        public ActionResult AddLink()
        {
            var _link = new CategoryLink() { Url = "http://" };
            return PartialView(_link);
        }
        #endregion
    }
}