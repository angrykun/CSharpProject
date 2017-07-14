using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninesky.Core.Types
{
    /// <summary>
    /// 查询分页数据时使用的类，包含当前页，每页记录数，总记录数，当前页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paging<T>
    {
        /// <summary>
        /// 当前页，从1计数
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页记录数，默认20
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalNumber { get; set; }
        /// <summary>
        /// 当前页记录列表
        /// </summary>
        public List<T> Items { get; set; }

        public Paging()
        {
            PageIndex = 1;
            PageSize = 20;
        }
    }
}
