using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
   public class Role
    {
       private int id;
       private string title;
       private string rights;
       private string descriprion;
       private int sort;
       private string rightstitle;


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
       public string Righttitle
       {
           set { rightstitle = value; }
           get { return rightstitle; }
       }
       public string Rights
       {
           set { rights = value; }
           get { return rights; }
       }
       public string Description
       {
           set { descriprion = value; }
           get { return descriprion; }
       }
       public int Sort
       {
           set { sort = value; }
           get { return sort; }
       }
    }
}
