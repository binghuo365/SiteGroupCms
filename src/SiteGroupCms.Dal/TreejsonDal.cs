using System;
using System.Collections.Generic;
using System.Text;

namespace SiteGroupCms.Dal
{
   public class Treejson
    {
       public static string tree2json2(SiteGroupCms.Entity.Catalogtree tree, bool isfirst)
       {
           string respons = String.Empty;
           if (tree != null)
           {
                   respons += "{\"catalogid\":" + tree.Id + ",\"id\":" + tree.Id + ",\"text\":" +SiteGroupCms.Utils.fastJSON.JSON.WriteString(tree.Name) ;//站点不输出url即不输出连接
                   
               if (tree.HasChild)
               {
                   respons += ",\"children\":[";
                   for (int i = 0; i < tree.SubChild.Count; i++)
                   {
                       respons += tree2json2(tree.SubChild[i], false);
                       if (i != tree.SubChild.Count - 1)
                           respons += ",";
                   }
                   respons += "]";
               }
               else
               {
                   respons += "";
               }
               respons += "}";

           }
           return respons;
       
       }


       public static string tree2json(SiteGroupCms.Entity.Catalogtree tree,bool isfirst)
   {
       string respons = String.Empty;
       if (tree != null)
       {
           if(isfirst)
               respons += "{text:'" + tree.Name + "',id:'site" + tree.Id + "'";//站点不输出url即不输出连接
           else
           respons += "{text:'" + tree.Name + "',isexpand:'false',id:'" + tree.Id + "',url:'articlelist.aspx?type=catalog&catalogid=" + tree.Id + "'";
           if (tree.HasChild)
           {
               respons += ",children:[";
               for (int i = 0; i < tree.SubChild.Count; i++)
               {
                  respons+= tree2json(tree.SubChild[i],false);
                  if (i != tree.SubChild.Count - 1)
                      respons += ",";
               }
               respons += "]";
           }
           else
           {
               respons += "";
           }
           respons += "}";

       }
       return respons;
      
   }
    }
}
