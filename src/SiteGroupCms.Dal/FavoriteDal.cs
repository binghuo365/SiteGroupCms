using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;

namespace SiteGroupCms.Dal
{
  public  class FavoriteDal:Common
    {
      public FavoriteDal()
        {
            base.SetupSystemDate();
        }
      /// <summary>
      /// 是否存在记录
      /// </summary>
      /// <param name="_wherestr">条件</param>
      /// <returns></returns>
      public bool Exists(string _wherestr)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              int _ext = 0;
              _doh.Reset();
              _doh.ConditionExpress = _wherestr;
              if (_doh.Exist("yy_favoriteinfo"))
                  _ext = 1;
              return (_ext == 1);
          }
      }
     
   
      /// <summary>
      /// 彻底删除一条数据
      /// </summary>
      public bool updateishow(string _id,int type)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "id=@id";
              _doh.AddConditionParameter("@id", _id);
              _doh.AddFieldItem("isshow", type);
              int _update = _doh.Update("yy_favoriteinfo");
              return (_update >= 1);
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
                  fav.ID =Convert.ToInt32(_id);
                  fav.Title = dt.Rows[0]["title"].ToString();
                  fav.Content = dt.Rows[0]["content"].ToString();
                  fav.Icon = dt.Rows[0]["icon"].ToString();
                  fav.Isshow = dt.Rows[0]["isshow"].ToString() == "0" ? 0 : 1;
                  fav.Rightid =Convert.ToInt32(dt.Rows[0]["rightid"].ToString());
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
              _doh.SqlCmd = "SELECT * FROM [yy_favoriteinfo] WHERE " + where;
              DataTable dt = _doh.GetDataTable();
              if (dt.Rows.Count > 0)
                  return dt;
              else
                  return null;
          }
      }

      public bool addfavorite(SiteGroupCms.Entity.Favorite obj)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              SiteGroupCms.Entity.Admin _admin=(SiteGroupCms.Entity.Admin)HttpContext.Current.Session["admin"];
              _doh.Reset();
              _doh.AddFieldItem("title", obj.Title);
              _doh.AddFieldItem("content", obj.Content);
              _doh.AddFieldItem("url", obj.Content);
              _doh.AddFieldItem("content", obj.Content);
              _doh.AddFieldItem("description", obj.Content);
              int _insert = _doh.Insert("yy_roleinfo");
              return _insert>=1;
          }
      }
    
    }
}
