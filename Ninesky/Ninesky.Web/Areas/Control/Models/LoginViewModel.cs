using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ninesky.Web.Areas.Control.Models
{
    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "必须输入{0}")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [DisplayName("账号")]
        public string Accounts { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "必须输入{0}")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }

    }
}