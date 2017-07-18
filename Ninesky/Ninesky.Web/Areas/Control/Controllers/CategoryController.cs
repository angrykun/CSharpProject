using Ninesky.Core.Category;
using Ninesky.DataLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
            return View();
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
            var _categories = categoryManager.FindList(0, null, new OrderParam[] { new OrderParam() { Method = OrderMethod.ASC, PropertyName = "ParentPath" }, new OrderParam() { Method = OrderMethod.ASC, PropertyName = "Order" } });
            return View();
        }
        #endregion
    }
}