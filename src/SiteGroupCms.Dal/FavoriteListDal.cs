using System;
using System.Collections.Generic;
using System.Text;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;
using System.Data;
using System.Web;

namespace SiteGroupCms.Dal
{
   public class FavoriteListDal
    {
        
        /// <summary>
        /// 彻底删除一条数据
        /// </summary>
        public bool delete(string _id)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.ConditionExpress = "id=@id";
                _doh.AddConditionParameter("@id", _id);
                int _del = _doh.Delete("yy_favoritelistinfo");
                return (_del >= 1);
            }
        }

        public bool Exists(string _wherestr)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                int _ext = 0;
                _doh.Reset();
                _doh.ConditionExpress = _wherestr;
                if (_doh.Exist("yy_favoritelistinfo"))
                    _ext = 1;
                return (_ext == 1);
            }
        }

        /// <summary>
        /// 获得单页内容的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public SiteGroupCms.Entity.Favorite GetEntity(string _id)
        {
            SiteGroupCms.Entity.Favorite fav = new SiteGroupCms.Entity.Favorite();

            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_favoriteinfo] WHERE [Id]=" + _id;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    fav.ID = Convert.ToInt32(_id);
                    fav.Title = dt.Rows[0]["title"].ToString();
                    fav.Content = dt.Rows[0]["content"].ToString();
                    fav.Icon = dt.Rows[0]["icon"].ToString();
                    fav.Isshow = dt.Rows[0]["isshow"].ToString() == "0" ? 0 : 1;
                    fav.Rightid = Convert.ToInt32(dt.Rows[0]["rightid"].ToString());
                    fav.Url = dt.Rows[0]["url"].ToString();
                }
                return fav;
            }
        }

        /// <summary>
        /// 获得单页内容的单条记录实体
        /// </summary>
        /// <param name="_id"></param>
        public DataTable GetDT(string where)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = @"SELECT     yy_favoriteinfo.id, yy_favoriteinfo.title, yy_favoriteinfo.[content], yy_favoriteinfo.url, yy_favoriteinfo.Icon, yy_favoriteinfo.isshow, yy_favoriteinfo.rightid, 
                      yy_favoritelistinfo.id AS listid, yy_favoritelistinfo.Addtime, yy_favoritelistinfo.userid, yy_favoritelistinfo.favoriteid
FROM         yy_favoriteinfo INNER JOIN
                      yy_favoritelistinfo ON yy_favoriteinfo.id = yy_favoritelistinfo.favoriteid and " + where;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                    return dt;
                else
                    return null;
            }
        }

        public bool addfavorite(string favoriteid)
        {
            using (DbOperHandler _doh = new Common().Doh())
            {
                SiteGroupCms.Entity.Admin _admin = (SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
                _doh.Reset();

                _doh.AddFieldItem("favoriteid", favoriteid);
                _doh.AddFieldItem("userid", _admin.Id);
                int _insert = _doh.Insert("yy_favoritelistinfo");
                return _insert >= 1;
            }
        }
    
    }
}
