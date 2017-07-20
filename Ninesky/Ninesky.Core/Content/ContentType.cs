using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ninesky.Core.Content
{
    /// <summary>
    /// 内容类型
    /// </summary>
    public class ContentType
    {
        [Key]
        public int ContentTypeID { get; set; }
        ///// <summary>
        ///// 栏目ID
        ///// </summary>
        //[Required()]
        //[DisplayName("栏目ID")]
        //public int CategoryID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required()]
        [DisplayName("名称")]
        public string Name { get; set; }
        /// <summary>
        ///  控制器名称
        /// </summary>
        [Required()]
        [DisplayName("控制器名称")]
        public string Controller { get; set; }
        [StringLength(1000, ErrorMessage = "必须少于{1}个字符")]
        [DisplayName("说明")]
        public string Description { get; set; }
    }
}
