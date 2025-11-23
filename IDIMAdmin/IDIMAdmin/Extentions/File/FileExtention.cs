using System.Net;
using System.Web;

namespace IDIMAdmin.Extentions.File
{
    public static class FileExtention
    {
        /// <summary>
        /// Create folder from path if not exists
        /// </summary>
        /// <param name="path">file path</param>
        public static void ToFolder(this string path)
        {
            if(string.IsNullOrEmpty(path))
                return;

            var exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
        }

        /// <summary>
        /// Check file exists in http path
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsHttpFileExists(this string value)
        {
            try
            {
                //var request = (HttpWebRequest)WebRequest.Create(value);
                //request.Method = "HEAD";

                //var response = (HttpWebResponse)request.GetResponse();
                //response.Close();
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Get full path of thumbnail from file name
        /// </summary>
        /// <param name="value">file name with extention</param>
        /// <returns></returns>
        public static string ToThumbnail(this string value)
        {
            var thumb = DefaultData.DefaultAvatar;

            if (!string.IsNullOrEmpty(value))
            {
                thumb = IsHttpFileExists($"{DefaultData.HttpAvatar}{value}") ? $"{DefaultData.HttpAvatar}{value}" : thumb;
            }

            return thumb;
        }
    }
}