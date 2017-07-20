using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninesky.Core.Types
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class TreeNode
    {
        public int pId { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
    }
}
