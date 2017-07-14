using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Ninesky.Web
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class General
    {
        public static string GetModelErrorString(ModelStateDictionary modelState)
        {
            StringBuilder _sb = new StringBuilder();
            var _ErrorModelState = modelState.Where(m => m.Value.Errors.Count() > 0);
            foreach (var item in _ErrorModelState)
            {
                foreach (var modelError in item.Value.Errors)
                {
                    _sb.AppendLine(modelError.ErrorMessage);
                }
            }
            return _sb.ToString();
        }
    }
}