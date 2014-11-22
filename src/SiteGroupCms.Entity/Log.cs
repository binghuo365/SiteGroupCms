using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class Log
    {
       private int id;
       private DateTime dotime;
       private string userid;
       private string doip;
       private string dotype;
       public int Id
       {
           set { id = value; }
           get { return id; }
       }
       public DateTime Dotime
       {
           set { dotime = value; }
           get { return dotime; }
       }
       public string UserId
       {
           set { userid = value; }
           get { return  userid; }
       }
       public string  Doip
       {
           set { doip = value; }
           get { return doip; }
       }
       public string Dotype
       {
           set { dotype = value; }
           get { return dotype; }
       }
    }

}
