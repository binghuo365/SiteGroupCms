using System;
using System.Web;
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// ffmpeg.exe调用
    /// </summary>
    public static class ffmpegHelp
    {
        /// <summary>
        /// 视频格式转为Flv
        /// </summary>
        /// <param name="vFileName">原视频文件地址</param>
        /// <param name="WidthAndHeight">宽和高参数，如：480*360</param>
        /// <param name="ExportName">生成后的FLV文件地址</param>
        /// <returns></returns>
        public static bool Convert2Flv(string vFileName, string WidthAndHeight, string ExportName)
        {
            try
            {
                vFileName = HttpContext.Current.Server.MapPath(vFileName);
                ExportName = HttpContext.Current.Server.MapPath(ExportName);
                string Command = " -i \"" + vFileName + "\" -y -ab 32 -ar 22050 -b 800000 -s " + WidthAndHeight + " \"" + ExportName + "\""; //Flv格式   
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"ffmpeg.exe";
                p.StartInfo.Arguments = Command;
                p.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/bin/tools/");
                #region 方法一
                //p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序 启动 线程
                //p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用 StandardOutput是捕获不到任何消息的...
                //p.StartInfo.CreateNoWindow = false;//不创建进程窗口
                //p.Start();//启动线程
                //p.BeginErrorReadLine();//开始异步读取
                //p.WaitForExit();//等待完成
                //p.Close();//关闭进程
                //p.Dispose();//释放资源
                #endregion
                #region 方法二
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                p.Start();//启动线程
                p.WaitForExit();//等待完成
                p.Close();//关闭进程
                p.Dispose();//释放资源
                #endregion
            }
            catch (System.Exception e)
            {
                throw e;
            }
            return true;
        }
        /// <summary>
        /// 生成FLV视频的缩略图
        /// </summary>
        /// <param name="vFileName">视频文件地址</param>
        /// <param name="FlvImgSize">宽和高参数，如：240*180</param>
        /// <returns></returns>
        public static string CatchImg(string vFileName, string FlvImgSize, string Second)
        {
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(vFileName)))
                return "";
            try
            {
                string flv_img_p = vFileName.Substring(0, vFileName.Length - 4) + "_thumbs.jpg";
                string Command = " -i \"" + HttpContext.Current.Server.MapPath(vFileName) + "\" -y -f image2 -ss " + Second + " -t 0.1 -s " + FlvImgSize + " \"" + HttpContext.Current.Server.MapPath(flv_img_p) + "\"";
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = @"ffmpeg.exe";
                p.StartInfo.Arguments = Command;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                p.StartInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/bin/tools/");
                //不创建进程窗口
                p.StartInfo.CreateNoWindow = true;
                p.Start();//启动线程
                p.WaitForExit();//等待完成
                p.Close();//关闭进程
                p.Dispose();//释放资源
                System.Threading.Thread.Sleep(4000);
                //注意:图片截取成功后,数据由内存缓存写到磁盘需要时间较长,大概在3,4秒甚至更长;
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(flv_img_p)))
                {
                    return flv_img_p;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
