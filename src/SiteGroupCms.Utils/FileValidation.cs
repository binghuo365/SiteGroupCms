/*
 * 程序中文名称: 站群内容管理系统
 * 
 * 程序英文名称: SiteGroupCms
 * 
 * 程序版本: 5.2.X
 * 
 * 程序作者: 高伟 ( 合作请联系：254860396#qq.com)
 * 
 * 
 * 
 * 
 * 
 */

using System;
using System.Web;
using System.IO;
namespace SiteGroupCms.Utils
{
    /// <summary>
    /// 文件编号
    /// </summary>
    public enum FileExtension
    {
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        //RAR = 8297
        // 255216 jpg;    
        // 7173 gif;    
        // 6677 bmp,    
        // 13780 png;    
        // 7790 exe dll,    
        // 8297 rar    
        // 6063 xml    
        // 6033 html    
        // 239187 aspx    

        // 117115 cs    
        // 119105 js    
        // 210187 txt    
        //255254 sql    
    }
    public static class FileValidation
    {

        /// <summary>
        /// 是否允许
        /// </summary>
        /// <param name="oFile"></param>
        /// <param name="fileEx"></param>
        /// <returns></returns>
        public static bool IsAllowedExtension(HttpPostedFile oFile, FileExtension[] fileEx)
        {
            int fileLen = oFile.ContentLength;

            byte[] imgArray = new byte[fileLen];
            oFile.InputStream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch { }
            br.Close();
            ms.Close();
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 上传前的图片是否可靠
        /// </summary>
        /// <param name="oFile"></param>
        /// <returns></returns>
        public static bool IsSecureUploadPhoto(HttpPostedFile oFile)
        {
            bool isPhoto = false;
            string fileExtension = System.IO.Path.GetExtension(oFile.FileName).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }
            if (!isPhoto) return true;//不是图片，既然允许上传，那就不检测了
            FileExtension[] fe = {FileExtension.BMP,
                                     FileExtension.GIF,
                                     FileExtension.JPG,
                                     FileExtension.PNG
                                 };
            if (IsAllowedExtension(oFile, fe))
                return true;
            else
                return false;
        }
        /// <summary>
        /// 上传后的图片是否安全
        /// </summary>
        /// <param name="photoFile">物理地址</param>
        /// <returns></returns>
        public static bool IsSecureUpfilePhoto(string photoFile)
        {
            bool isPhoto = false;
            string Img = "Yes";
            string fileExtension = System.IO.Path.GetExtension(photoFile).ToLower();
            string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".bmp" };
            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (fileExtension == allowedExtensions[i])
                {
                    isPhoto = true;
                    break;
                }
            }
            if (!isPhoto) return true;//不是图片，既然允许上传，那就不检测了
            StreamReader sr = new StreamReader(photoFile, System.Text.Encoding.Default);
            string strContent = sr.ReadToEnd();
            sr.Close();
            string str = "request|<script|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
            foreach (string s in str.Split('|'))
                if (strContent.ToLower().IndexOf(s) != -1)
                {
                    File.Delete(photoFile);
                    Img = "No";
                    break;
                }
            return (Img == "Yes");
        }
    }
}
