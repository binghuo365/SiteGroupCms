using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 模板方案-------表映射实体
    /// </summary>
   public class Normal_TemplateProject
    {
        public Normal_TemplateProject()
        { }

        private string _id;
        private string _title;
        private string _info;
        private string _dir;
        private int _isdefault;
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Info
        {
            set { _info = value; }
            get { return _info; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Dir
        {
            set { _dir = value; }
            get { return _dir; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsDefault
        {
            set { _isdefault = value; }
            get { return _isdefault; }
        }

    }
}
