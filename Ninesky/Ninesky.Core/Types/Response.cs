using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninesky.Core.Types
{
    /// <summary>
    /// 返回数据类型，包含返回代码，返回消息，返回数据3个属性
    /// </summary>
    public class Response
    {
        /// <summary>
        /// 返回代码 0-失败，1-成功，其他-见具体返回值说明
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; set; }

        public Response()
        {
            Code = 0;
        }
    }
}
