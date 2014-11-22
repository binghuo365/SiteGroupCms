using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;

namespace SiteGroupCms.Entity
{
   public class Repost
    {
       private int id;

       public int Id
       {
           get { return id; }
           set { id = value; }
       }
       private int followid;

       public int Followid
       {
           get { return followid; }
           set { followid = value; }
       }
       private DateTime addtime;

       public DateTime Addtime
       {
           get { return addtime; }
           set { addtime = value; }
       }
       private string content;

       public string Content
       {
           get { return content; }
           set { content = value; }
       }
       private int type;

       public int Type
       {
           get { return type; }
           set { type = value; }
       }
    }
}
