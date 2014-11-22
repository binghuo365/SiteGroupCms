using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SiteGroupCms
{
    public partial class guest : SiteGroupCms.Ui.BasicPage
    {
        int page = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["page"] != null)
                page = Str2Int(Request.QueryString["page"], 1);
            if (!IsPostBack)
            {
                binddata();

            }
        }
        /// <summary>
        /// 绑定留言数据
        /// </summary>
        private void binddata()
        {
            SiteGroupCms.Dal.GuestDal guestdal = new Dal.GuestDal();
            int recordcount = guestdal.GetCount("audit=1");
            Pager2.RecordCount = recordcount;
            Repeater1.DataSource = guestdal.GetDT(page, 8, " audit=1 ", " id desc");
            Repeater1.DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //判断里层repeater处于外层repeater的哪个位置（ AlternatingItemTemplate，FooterTemplate，
            //HeaderTemplate，，ItemTemplate，SeparatorTemplate）
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rep = e.Item.FindControl("Repeater2") as Repeater;//找到里层的repeater对象
                DataRowView rowv = (DataRowView)e.Item.DataItem;//找到分类Repeater关联的数据项 
                int followid = Convert.ToInt32(rowv["id"]); //获取填充子类的id 
                rep.DataSource = new SiteGroupCms.Dal.RepostDal().GetList(followid);
                rep.DataBind();
            }
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)//提交
        {
            SiteGroupCms.Entity.Guest guest = new Entity.Guest();
            guest.Username = UserName.Text;
            guest.Title = Title.Text;
            guest.Content = Content.Text;
            guest.Audit = 0;
            if (Session["ValidateCode"].ToString() != yz.Text)
            {
                Response.Write("<script>alert('验证码错误！');</script>");
                return;
            }
            if (new SiteGroupCms.Dal.GuestDal().insertguest(guest) >= 0)
                Response.Write("<script>alert('留言成功，审核通过将在此显示。谢谢你对云作坊的关注');location.href='guest.aspx';</script>");
            else
                Response.Write("<script>alert('留言失败');location.href='guest.aspx';</script>");
        }
    }
}