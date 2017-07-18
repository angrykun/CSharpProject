using Ninesky.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninesky.Core.Category
{
    /// <summary>
    /// 常规栏目管理
    /// </summary>
    public class CategoryGeneralManager : BaseManager<CategoryGeneral>
    {
        /// <summary>
        /// 删除常规栏目 -根据栏目ID
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public Response DeleteByCategoryID(int categoryID)
        {
            return base.Delete(categoryID);
        }
    }
}
