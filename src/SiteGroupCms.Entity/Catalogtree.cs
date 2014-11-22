using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{    /// <summary>
    /// 栏目树实体
    /// </summary>
   public class Catalogtree
    {
        private string _id;
        private string _name = string.Empty;
        private bool _haschild = false;
        private List<Catalogtree> _subchild;
        /// <summary>
        /// 栏目编号
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        public bool HasChild
        {
            set { _haschild = value; }
            get { return _haschild; }
        }
        /// <summary>
        /// 子树
        /// </summary>
        public List<Catalogtree> SubChild
        {
            set { _subchild = value; }
            get { return _subchild; }
        }
    }
}
