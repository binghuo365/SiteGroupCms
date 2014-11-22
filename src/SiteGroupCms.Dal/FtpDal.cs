using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Dal
{
   public class FtpDal
    {
       public SiteGroupCms.Utils.Ftp Ftp = new SiteGroupCms.Utils.Ftp(new System.Uri("ftp://127.0.0.1:21"),"ftp","123456");
       public void UploadFile(string dfiles)
       {
           //对路径进行分析，若存在则上传，若不存在则创建
           //示例 ~/sites/demo/pub/gnxw/tyxw/1.htm
           string[] strs = dfiles.Split('/');
           for (int i = 2; i < strs.Length-1; i++)
           {
               if(!Ftp.DirectoryExist(strs[i]))
               Ftp.MakeDirectory(strs[i]);
               Ftp.GotoDirectory(strs[i]);
           }
           Ftp.UploadFile(System.Web.HttpContext.Current.Server.MapPath("..") + dfiles.Substring(1,dfiles.Length-1),true);  
       }

    }
}
