using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Entity
{
  public  class Ftp
    {
      private string ftpserver;
      private string ftpport;
      private string ftpuser;
      private string ftppwd;
      private string ftpdir;

      public string Ftpserver
      {
          set { ftpserver = value; }
          get { return ftpserver; }
      }
      public string Ftpport
      {
          set { ftpport = value; }
          get { return ftpport; }
      }
      public string Ftpuser
      {
          set { ftpuser = value; }
          get { return ftpuser; }
      }
      public string Ftppwd
      {
          set { ftppwd = value; }
          get { return ftppwd; }
      }
      public string Ftpdir
      {
          set { ftpdir = value; }
          get { return ftpdir; }
      }

    }
}
