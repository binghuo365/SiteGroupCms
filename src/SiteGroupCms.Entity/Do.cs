using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
  public  class Do
    {
      private int id;
      private string dotype;
      public int Id
      {
          set { id = value; }
          get { return id; }
      }
      public string Dotype
      {
          set { dotype = value; }
          get { return dotype; }
      }
    }
}
