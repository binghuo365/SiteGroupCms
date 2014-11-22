using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 片段代码块(模板标签)-------表映射实体
    /// </summary>
  public  class Normal_TemplateLabel
    {
        public Normal_TemplateLabel()
        { }

        private string _id;
        private string _title;
        private string _info;
        private int _pid;
        private int _sort;
        private string _source;
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
        public int PId
        {
            set { _pid = value; }
            get { return _pid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sort
        {
            set { _sort = value; }
            get { return _sort; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Source
        {
            set { _source = value; }
            get { return _source; }
        }

    }
}
