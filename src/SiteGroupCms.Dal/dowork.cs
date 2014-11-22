using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SiteGroupCms.Dal
{
   public class work 
 { 
  public int State=0;//0-没有开始,1-正在运行,2-成功结束,3-失败结束 
  public DateTime StartTime; 
  public DateTime FinishTime; 
  public DateTime ErrorTime;

  public void runwork() 
  { 
   lock(this)//确保临界区被一个Thread所占用 
   { 
    if(State!=1) 
    { 
     State=1;
     StartTime=DateTime.Now;
    // HttpContext.Current.Session["publishprocess"] = new Random().Next();
     System.Threading.Thread thread=new System.Threading.Thread(new System.Threading.ThreadStart(dowork)); 
     thread.Start();                         
    } 
   } 
  }

  private void dowork() 
  { 
   try 
   {
       for (int i = 0; i < 10; i++)
       { 

           System.Web.HttpContext.Current.Application.Lock();
           System.Web.HttpContext.Current.Application.Set("publishprocess", new Random().Next());
           System.Web.HttpContext.Current.Application.UnLock();
       } 
    State=2; 
   } 
   catch 
   { 
    ErrorTime=DateTime.Now; 
    State=3; 
   } 
   finally 
   { 
    FinishTime=DateTime.Now; 
   } 
  } 
 } 
}
