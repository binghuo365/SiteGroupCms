using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace SiteGroupCms
{
    public partial class roleedit : System.Web.UI.Page
    {
        public string rihgts = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SiteGroupCms.Dal.RightDal rightdal = new SiteGroupCms.Dal.RightDal();
            DataTable dt = rightdal.GetDT("1=1");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if(i==0)
                    rihgts += " {display:'" + dt.Rows[i]["title"].ToString() + "',name:'q" + dt.Rows[i]["id"].ToString() + "',newline:true,validate:{required:true,maxlength:60},labelWidth:200,width:200,space:30,type:'checkbox',group:'权限信息', groupicon:groupicon},";
                else if(i<dt.Rows.Count-1)
                    rihgts += " {display:'" + dt.Rows[i]["title"].ToString() + "',name:'q" + dt.Rows[i]["id"].ToString() + "',newline:true,labelWidth:200,width:200,space:30,type:'checkbox', groupicon:groupicon},";
                else
                    rihgts += " {display:'" + dt.Rows[i]["title"].ToString() + "',name:'q" + dt.Rows[i]["id"].ToString() + "',newline:true,labelWidth:200,width:200,space:30,type:'checkbox', groupicon:groupicon}";
            }
           
        }
    }
}
