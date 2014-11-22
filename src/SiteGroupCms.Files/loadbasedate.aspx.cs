using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
    public partial class loadbasedate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string localstring=SiteGroupCms.Dal.Const.ConnectionString;
            string ycstring="Data Source=210.43.192.28;Initial Catalog=SiteGroupCms;User ID=baoliao;Password=123456";
            try
            {
                SqlConnection conn = new SqlConnection(localstring);
                SqlConnection conn2 = new SqlConnection(ycstring);
               // conn.Open();
                conn2.Open();
                string sql = "select * from yy_rightinfo";
                SqlCommand cmd = new SqlCommand(sql,conn2);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) 
                {
                   // string sql2 = "insert into yy_rightinfo (title,description) values ('"+dr["title"]+"','"+dr["description"]+"')";
                   // SqlCommand cmdtemp = new SqlCommand(sql2, conn2);
                   // cmdtemp.ExecuteNonQuery();
                    Response.Write(dr["title"].ToString());
                }
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }
    }
}
