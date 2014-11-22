using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class Message
    {
       private int id;
       private string title;
       private int senduserid;
       private string content;
       private int type;
       private int deptid;
       private int userid;
       private string senderuser;
       private int isread;
       private int sort;
       private DateTime sendtime;
       public int Id
       {
           set { id = value; }
           get { return id; }
       }
       public string Title
       {
           set { title = value; }
           get { return title; }
       }
       public int Senduserid
       {
           set { senduserid = value; }
           get { return senduserid; }
       }
       public string Content
       {
           set { content = value; }
           get { return content; }
       }
       public int Type
       {
           set { type = value; }
           get { return type; }
       }
       public int DeptId
       {
           set { deptid = value; }
           get { return deptid; }
       }
       public int Userid
       {
           set { userid = value; }
           get { return userid; }
       }
       public int Isread
       {
           set { isread = value; }
           get { return isread; }
       }
       public int Sort
       {
           set { sort = value; }
           get { return sort; }
       }
       public DateTime Sendtime
       {
           set { sendtime = value; }
           get { return sendtime; }
       }
    }
}
