using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ninesky.Core.Category
{
    /// <summary>
    /// 栏目类型
    /// </summary>
    public enum CategoryType
    {
        /// <summary>
        /// 常规内容
        /// </summary>
        [Display(Name = "常规栏目", Description = "可以添加内容，不能添加子栏目")]
        General,
        /// <summary>
        /// 单页栏目
        /// </summary>
        [Display(Name = "单页栏目", Description = "仅有一个页面的栏目")]
        Page,
        /// <summary>
        /// 外部链接
        /// </summary>
        [Display(Name = "外部链接", Description = "点击跳转到指定链接")]
        Link
    }
}
