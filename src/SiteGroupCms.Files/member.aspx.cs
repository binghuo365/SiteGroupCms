using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiteGroupCms
{
    public partial class member : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loaddata();
            }
        }
        public void loaddata()
        {
            SiteGroupCms.Dal.AdminDal admindal = new Dal.AdminDal();
            Repeater1.DataSource = admindal.GetpersonDT("job like '%总顾问%'"," sort asc, id asc");
            Repeater1.DataBind();
            Repeater2.DataSource = admindal.GetpersonDT("job like '%主管%'", "sort asc, id asc");
            Repeater2.DataBind();
            Repeater3.DataSource = admindal.Getdepartdt("商务组", "sort asc, id asc");
            Repeater3.DataBind();
            Repeater4.DataSource = admindal.Getdepartdt("美工组", "sort asc, id asc");
            Repeater4.DataBind();
            Repeater5.DataSource = admindal.Getdepartdt("程序组", "sort asc, id asc");
            Repeater5.DataBind();
            Repeater6.DataSource = admindal.Getdepartdt("往期成员", "sort asc, id asc");
            Repeater6.DataBind();
        }

        public string getimg(object imgs)
        { 
          if(imgs.ToString()=="")
              return "/lib/images/noface.jpg";
          else
              return imgs.ToString().Remove(imgs.ToString().Length-1);
        }
    }
}