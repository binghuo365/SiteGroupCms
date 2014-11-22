using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SiteGroupCms.Utils;

namespace SiteGroupCms
{
    public partial class 水印测试 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {                                   


            System.Drawing.Image img = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);//用上传控件加载图片
            //System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("~/bb.jpg"));//用加字件加载图片

            string fileName = Server.MapPath("/aa.jpg");
            string waterName = Server.MapPath("/lib/images/water.jpg");

            //图片水印
           // ImageWaterMark.AddImageSignPic(img, fileName, waterName, 9, 80, 6);
            //文字水印
            ImageWaterMark.AddImageSignText(img, fileName, "yunstudio", 9, 100, "微软雅黑", 50);
            Image1.ImageUrl = fileName;
        }
    }
}