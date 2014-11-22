using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using SiteGroupCms.Utils;
using SiteGroupCms.DBUtility;


namespace SiteGroupCms.Dal
{
    /// <summary>
    /// 网站参数
    /// </summary>
    public class SiteDal: Common
    {
        public SiteDal()
        { }
        /// <summary>
        /// 获得网站参数
        /// </summary>
        /// <returns></returns>
        public SiteGroupCms.Entity.Site GetEntity(int _siteid)
        {
            SiteGroupCms.Entity.Site eSite = new SiteGroupCms.Entity.Site();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT * FROM [yy_siteinfo] WHERE [id]=" + _siteid;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    eSite.ID = _siteid;
                    eSite.Title = dt.Rows[0]["title"].ToString();
                    eSite.Location = dt.Rows[0]["location"].ToString(); 
                    eSite.WebTitle = dt.Rows[0]["webtitle"].ToString();
                    eSite.Description = dt.Rows[0]["description"].ToString();
                    eSite.Meta_Description = dt.Rows[0]["meta_description"].ToString();
                    eSite.Keyword = dt.Rows[0]["meta_keywords"].ToString();
                    eSite.UploadSize =Str2Int(dt.Rows[0]["uploadsize"].ToString()); 
                    eSite.UploadType = dt.Rows[0]["uploadtype"].ToString();
                    eSite.EmailServer = dt.Rows[0]["emailserver"].ToString();
                    eSite.EmailUser = dt.Rows[0]["emailuser"].ToString();
                    eSite.EmailPwd = dt.Rows[0]["emailpwd"].ToString();
                    eSite.FtpServer = dt.Rows[0]["ftpserver"].ToString(); 
                    eSite.FtpUser = dt.Rows[0]["ftpuser"].ToString();
                    eSite.FtpPort =Str2Int(dt.Rows[0]["ftpport"].ToString());
                    eSite.FtpDir = dt.Rows[0]["ftpdir"].ToString();
                    eSite.FtpPwd = dt.Rows[0]["ftppwd"].ToString();
                    eSite.Domain = dt.Rows[0]["domain"].ToString();
                    eSite.IsWork =Str2Int(dt.Rows[0]["iswork"].ToString());
                    eSite.Type =Str2Int(dt.Rows[0]["type"].ToString());
                    eSite.Indextemplate = Str2Int(dt.Rows[0]["indextemplate"].ToString());
                    eSite.Listtemplate = Str2Int(dt.Rows[0]["listtemplate"].ToString());
                    eSite.Contenttemplate = Str2Int(dt.Rows[0]["contenttemplate"].ToString());
                   
                }
               

        }
            return eSite;
        }
        /// <summary>
        /// 获得主网站参数  site表必须存在一个type=1的主网站
        /// </summary>
        /// <returns></returns>
        public SiteGroupCms.Entity.Site GetBaseEntity()
        {
            SiteGroupCms.Entity.Site eSite = new SiteGroupCms.Entity.Site();
            using (DbOperHandler _doh = new Common().Doh())
            {
                _doh.Reset();
                _doh.SqlCmd = "SELECT top 1 * FROM [yy_siteinfo] WHERE [type]=1" ;
                DataTable dt = _doh.GetDataTable();
                if (dt.Rows.Count > 0)
                {
                    eSite.ID =Str2Int(dt.Rows[0]["id"].ToString());
                    eSite.Title = dt.Rows[0]["title"].ToString();
                    eSite.Location = dt.Rows[0]["location"].ToString();
                    eSite.WebTitle = dt.Rows[0]["webtitle"].ToString();
                    eSite.Description = dt.Rows[0]["description"].ToString();
                    eSite.Meta_Description = dt.Rows[0]["meta_description"].ToString();
                    eSite.Keyword = dt.Rows[0]["meta_keywords"].ToString();
                    eSite.UploadSize = Str2Int(dt.Rows[0]["uploadsize"].ToString());
                    eSite.UploadType = dt.Rows[0]["uploadtype"].ToString();
                    eSite.EmailServer = dt.Rows[0]["emailserver"].ToString();
                    eSite.EmailUser = dt.Rows[0]["emailuser"].ToString();
                    eSite.EmailPwd = dt.Rows[0]["emailpwd"].ToString();
                    eSite.FtpServer = dt.Rows[0]["ftpserver"].ToString();
                    eSite.FtpUser = dt.Rows[0]["ftpuser"].ToString();
                    eSite.FtpPort = Str2Int(dt.Rows[0]["ftpport"].ToString());
                    eSite.FtpDir = dt.Rows[0]["ftpdir"].ToString();
                    eSite.FtpPwd = dt.Rows[0]["ftppwd"].ToString();
                    eSite.Domain = dt.Rows[0]["domain"].ToString();
                    eSite.IsWork = Str2Int(dt.Rows[0]["iswork"].ToString());
                    eSite.Type = Str2Int(dt.Rows[0]["type"].ToString());
                    eSite.Indextemplate = Str2Int(dt.Rows[0]["indextemplate"].ToString());
                    eSite.Listtemplate = Str2Int(dt.Rows[0]["listtemplate"].ToString());
                    eSite.Contenttemplate = Str2Int(dt.Rows[0]["contenttemplate"].ToString());

                }

                return eSite;
            }
        }
      public bool updateftp(SiteGroupCms.Entity.Ftp obj)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "type=@type";
              _doh.AddConditionParameter("@type", 1);
              _doh.AddFieldItem("ftpserver", obj.Ftpserver);
              _doh.AddFieldItem("ftpuser", obj.Ftpuser);
              _doh.AddFieldItem("ftpport", obj.Ftpport);
              _doh.AddFieldItem("ftppwd", obj.Ftppwd);
              _doh.AddFieldItem("ftpdir", obj.Ftpdir);
              int _update = _doh.Update("yy_siteinfo");
              return (_update == 1);
          }
      }
      public bool updatesite(SiteGroupCms.Entity.Site obj)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = "id=@id";
              _doh.AddConditionParameter("@id", obj.ID);
              _doh.AddFieldItem("title", obj.Title);
              _doh.AddFieldItem("webtitle", obj.WebTitle);
            //  _doh.AddFieldItem("location", obj.Location); 

              _doh.AddFieldItem("description", obj.Description);
              _doh.AddFieldItem("meta_keywords", obj.Keyword);
              _doh.AddFieldItem("iswork", obj.IsWork);
              _doh.AddFieldItem("domain", obj.Domain);


              _doh.AddFieldItem("indextemplate", obj.Indextemplate);
              _doh.AddFieldItem("listtemplate", obj.Listtemplate);
              _doh.AddFieldItem("contenttemplate", obj.Contenttemplate);

              _doh.AddFieldItem("emailserver", obj.EmailServer);
              _doh.AddFieldItem("emailuser", obj.EmailUser);
              _doh.AddFieldItem("emailpwd", obj.EmailPwd);

              _doh.AddFieldItem("ftpserver", obj.FtpServer);
              _doh.AddFieldItem("ftpuser", obj.FtpUser);
              _doh.AddFieldItem("ftpport", obj.FtpPort);
              _doh.AddFieldItem("ftppwd", obj.FtpPwd);
              _doh.AddFieldItem("ftpdir", obj.FtpDir);
              int _update = _doh.Update("yy_siteinfo");
              return (_update == 1);
          }
      }
      public DataTable GetDT(string where)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.SqlCmd = "SELECT * FROM [yy_siteinfo] WHERE " + where;
              DataTable dt = _doh.GetDataTable();
              if (dt.Rows.Count > 0)
                  return dt;
              else
                  return null;
          }
      }
      public void GetListJSON(int _thispage, int _pagesize, string _wherestr, ref string _jsonstr, string ordercol, string ordertype)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              string sqlStr = "";
              sqlStr = SiteGroupCms.Utils.SqlHelp.GetSql("*", "yy_siteinfo", ordercol, _pagesize, _thispage, ordertype, _wherestr);
              _doh.Reset();
              _doh.SqlCmd = sqlStr;
              DataTable dt = _doh.GetDataTable();
              int _countnum = dt.Rows.Count;
              DataTable dt2 = new DataTable();
              DataColumn col = new DataColumn("id", System.Type.GetType("System.String"));
              DataColumn col2 = new DataColumn("type", System.Type.GetType("System.String"));
              DataColumn col3 = new DataColumn("title", System.Type.GetType("System.String"));
              DataColumn col4 = new DataColumn("location", System.Type.GetType("System.String"));
              DataColumn col5 = new DataColumn("description", System.Type.GetType("System.String"));
              DataColumn col6 = new DataColumn("domain", System.Type.GetType("System.String"));
              DataColumn col7 = new DataColumn("addtime", System.Type.GetType("System.String"));
              dt2.Columns.Add(col);
              dt2.Columns.Add(col2);
              dt2.Columns.Add(col3);
              dt2.Columns.Add(col4);
              dt2.Columns.Add(col5);
              dt2.Columns.Add(col6);
              dt2.Columns.Add(col7);
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                  DataRow dr = dt2.NewRow();
                  dr["id"] = dt.Rows[i]["id"];
                  dr["type"] = dt.Rows[i]["type"].ToString() =="1" ? "系统站" : "普通站";
                  dr["title"] = dt.Rows[i]["title"];
                  dr["location"] = dt.Rows[i]["location"].ToString();
                  dr["description"] = dt.Rows[i]["description"].ToString();
                  dr["domain"] = dt.Rows[i]["domain"].ToString();
                  dr["addtime"] = String.Format("{0:g}", SiteGroupCms.Utils.Validator.StrToDate(dt.Rows[i]["addtime"].ToString(), DateTime.Now));
                  dt2.Rows.Add(dr);
              }
              _jsonstr = SiteGroupCms.Utils.dtHelp.DT2JSON(dt2, _countnum);
              dt.Clear();
              dt.Dispose();
          }
      }
      public int addsite(SiteGroupCms.Entity.Site obj)
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.AddFieldItem("title", obj.Title);
              _doh.AddFieldItem("webtitle", obj.WebTitle);
              _doh.AddFieldItem("location", obj.Location);

              _doh.AddFieldItem("description", obj.Description);
              _doh.AddFieldItem("meta_keywords", obj.Keyword);
              _doh.AddFieldItem("iswork", obj.IsWork);
              _doh.AddFieldItem("domain", obj.Domain);
              _doh.AddFieldItem("type", 0);//一般添加普通站点


              _doh.AddFieldItem("indextemplate", obj.Indextemplate);
              _doh.AddFieldItem("listtemplate", obj.Listtemplate);
              _doh.AddFieldItem("contenttemplate", obj.Contenttemplate);

              _doh.AddFieldItem("emailserver", obj.EmailServer);
              _doh.AddFieldItem("emailuser", obj.EmailUser);
              _doh.AddFieldItem("emailpwd", obj.EmailPwd);

              _doh.AddFieldItem("ftpserver", obj.FtpServer);
              _doh.AddFieldItem("ftpuser", obj.FtpUser);
              _doh.AddFieldItem("ftpport", obj.FtpPort);
              _doh.AddFieldItem("ftppwd", obj.FtpPwd);
              _doh.AddFieldItem("ftpdir", obj.FtpDir);


              return _doh.Insert("yy_siteinfo");
                     

          }
      }

      public int addsite(SiteGroupCms.Entity.Site obj,int type)//站点类型  type 1,系统站点，2，普通站点  ，系统站点整个系统只有一个
      {
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.AddFieldItem("title", obj.Title);
              _doh.AddFieldItem("webtitle", obj.WebTitle);
              _doh.AddFieldItem("location", obj.Location);

              _doh.AddFieldItem("meta_description", obj.Description);
              _doh.AddFieldItem("meta_keywords", obj.Keyword);
              _doh.AddFieldItem("iswork", obj.IsWork);
              _doh.AddFieldItem("domain", obj.Domain);
              _doh.AddFieldItem("type",type);//一般添加普通站点


              _doh.AddFieldItem("indextemplate", obj.Indextemplate);
              _doh.AddFieldItem("listtemplate", obj.Listtemplate);
              _doh.AddFieldItem("contenttemplate", obj.Contenttemplate);

              _doh.AddFieldItem("emailserver", obj.EmailServer);
              _doh.AddFieldItem("emailuser", obj.EmailUser);
              _doh.AddFieldItem("emailpwd", obj.EmailPwd);

              _doh.AddFieldItem("ftpserver", obj.FtpServer);
              _doh.AddFieldItem("ftpuser", obj.FtpUser);
              _doh.AddFieldItem("ftpport", obj.FtpPort);
              _doh.AddFieldItem("ftppwd", obj.FtpPwd);
              _doh.AddFieldItem("ftpdir", obj.FtpDir);


              return _doh.Insert("yy_siteinfo");


          }
      }
      public bool deleteByid(string id)
      {
          //先判断是否为系统站点，如果是系统站点则不能够删除。
          //对站点删除  要删除站点下的所有栏目，删除站点下的所有文章，删除站点下的所有用户。包括从硬盘
          //删除文件，删除ftp上面的所有文件
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.SqlCmd = "select * from yy_siteinfo where id=" + id;
              DataTable dt = _doh.GetDataTable();
                  SiteGroupCms.Utils.DirFile.DeleteDir("/sites/"+dt.Rows[0]["location"].ToString());
              _doh.Reset();
              _doh.SqlCmd="delete from yy_siteinfo where id="+id+" and type!=1";
              int _del = _doh.ExecuteSqlNonQuery();

              if (_del==1)//删除栏目，删除文章，删除模板,暂时不删除用户
              {
                  _doh.Reset();
                  _doh.SqlCmd = "delete from yy_cataloginfo where siteid="+id;
                  _doh.ExecuteSqlNonQuery();
                  _doh.Reset();
                  _doh.SqlCmd = "delete from yy_articleinfo where siteid="+id;
                  _doh.ExecuteSqlNonQuery();
                  _doh.Reset();
                  _doh.SqlCmd = "delete from yy_templateinfo where siteid="+id;
                  _doh.ExecuteSqlNonQuery();
              }

              return (_del == 1);
          }


      }
      /// <summary>
      /// 是否存在记录
      /// </summary>
      /// <param name="_wherestr">条件</param>
      /// <returns></returns>
      public bool Exists(string _wherestr)
      {
          int _ext = 0;
          using (DbOperHandler _doh = new Common().Doh())
          {
              _doh.Reset();
              _doh.ConditionExpress = _wherestr;
              if (_doh.Exist("yy_siteinfo"))
                  _ext = 1;
              return (_ext == 1);
          }

      }
    }
}
