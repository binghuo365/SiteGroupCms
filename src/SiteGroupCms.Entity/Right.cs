using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class Right
    {
       private int id;
       private string title;
       private string description;
       private int sort;


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
       public string Description
       {
           set { description = value; }
           get { return description; }
       }
       public int Sort
       {
           set { sort = value; }
           get { return sort; }
       }
    }
}
