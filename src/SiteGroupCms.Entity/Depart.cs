using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class Depart
    {
       private int id;
       private string name;
       private string description;
       private int totalusers;
       public int Id
       {
           set { id = value; }
           get { return id; }
       }
       public string Name
       {
           set { name = value; }
           get { return name; }
       }
       public string Description
       {
           set { description = value; }
           get { return description; }
       }
       public int TotalNum
       {
           set { totalusers=value;}
           get { return totalusers; }
       }
    }
}
