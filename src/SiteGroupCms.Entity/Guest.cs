using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class Guest
    {
       private int id;

       public int Id
       {
           get { return id; }
           set { id = value; }
       }
       private string username;

       public string Username
       {
           get { return username; }
           set { username = value; }
       }
       private string title;

       public string Title
       {
           get { return title; }
           set { title = value; }
       }
       private string content;

       public string Content
       {
           get { return content; }
           set { content = value; }
       }
       private DateTime addtime;

       public DateTime Addtime
       {
           get { return addtime; }
           set { addtime = value; }
       }
       private string userip;

       public string Userip
       {
           get { return userip; }
           set { userip = value; }
       }
       private int audit;

       public int Audit
       {
           get { return audit; }
           set { audit = value; }
       }
     

    }
}
