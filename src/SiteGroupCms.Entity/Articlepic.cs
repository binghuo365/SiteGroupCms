using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
  public  class Articlepic
    {
        private int id;
        private int artid;
        private string url;
        private string type;
        private int size;
        private int istop;
        private string title;

        /// <summary>
        /// 编号
        /// </summary>
        public int ID
        {
            set { id = value; }
            get { return id; }
        }
        /// <summary>
        /// 文章编号
        /// </summary>
        public int Artid
        {
            set { artid = value; }
            get { return artid; }
        }
        /// <summary>
        /// 图片链接
        /// </summary>
        public string Url
        {
            set { url = value; }
            get { return url; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            set { type = value; }
            get { return type; }
        }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size
        {
            set { size = value; }
            get { return size; }
        }
        /// <summary>
        /// 是否置顶ID
        /// </summary>
        public int Istop
        {
            set { istop = value; }
            get { return istop; }
        } /// <summary>
        ///标题说明
        /// </summary>
        public string Title
        {
            set { title = value; }
            get { return title; }
        }

    }
}
