using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 模板-------表映射实体
    /// </summary>
    public class Normal_Template
    {
        public Normal_Template()
        {
        
        }

        private int _id;
        private string _title;
        private int _siteid;
        private int _type;
        private string _imagedirname;
        private DateTime _addtime;
        private string _source;
        private string _filename;
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        public string FileName
        {
            set { _filename = value; }
            get { return _filename; }
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
        public int Siteid
        {
            set { _siteid = value; }
            get { return _siteid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Imagedirname
        {
            set { _imagedirname = value; }
            get { return _imagedirname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
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
