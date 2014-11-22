using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class FavoriteList
    {
        private int id;
        private string title;
        private string content;
        private int isshow;
        private int userid;
        private string icon;
        private string url;
        private int rightid;
        private DateTime addtime;

        public int ID
        {
            set { id = value; }
            get { return id; }
        }
        public int Rightid
        {
            set { rightid = value; }
            get { return rightid; }
        }
        public string Title
        {
            set { title = value; }
            get { return title; }
        }
        public string Content
        {
            set { content = value; }
            get { return content; }
        }
        public int Isshow
        {
            set { isshow = value; }
            get { return isshow; }
        }
        public string Icon
        {
            set { icon = value; }
            get { return icon; }
        }
        public string Url
        {
            set { url = value; }
            get { return url; }
        }
        public int Userid
        {
            set { userid = value; }
            get { return userid; }
        }
        public DateTime AddTime
        {
            set { addtime = value; }
            get { return addtime; }
        }

    }
}
