using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 文章图片实体类
    /// </summary>

   public class Articleatts
    {
        private int id;
        private int artid;
        private string url;
        private string type;
        private int size;
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
        ///标题说明
        /// </summary>
        public string Title
        {
            set { title = value; }
            get { return title; }
        }

    }
}
