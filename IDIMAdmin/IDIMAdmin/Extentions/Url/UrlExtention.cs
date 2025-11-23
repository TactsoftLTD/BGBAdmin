using System.Web;

namespace IDIMAdmin.Extentions.Url
{
    public static class UrlExtention
    {
        public static string GetAvatar(string pictureName)
        {
            var url = DefaultData.DefaultAvatar;

            if (string.IsNullOrEmpty(pictureName))
            {
                return url;
            }

            var path = string.Concat(DefaultData.AvatarLocation, "/", pictureName);
            var file = HttpContext.Current.Server.MapPath(path);

            if (System.IO.File.Exists(file))
            {
                return path;
            }

            return url;
        }
    }
}