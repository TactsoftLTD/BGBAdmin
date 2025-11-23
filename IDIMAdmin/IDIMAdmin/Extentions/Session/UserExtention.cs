using System.Web;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Extentions.Session
{
    public static class UserExtention
    {
        public static T Get<T>(string key)
        {
            object sessionObject = HttpContext.Current.Session[key];
            if (sessionObject == null)
            {
                return default(T);
            }
            return (T)HttpContext.Current.Session[key];

        }

        public static T Get<T>(string key, T defaultValue)
        {
            object sessionObject = HttpContext.Current.Session[key];
            if (sessionObject == null)
            {
                HttpContext.Current.Session[key] = defaultValue;
            }

            return (T)HttpContext.Current.Session[key];
        }

        public static void Save<T>(string key, T entity)
        {
            HttpContext.Current.Session[key] = entity;
            HttpContext.Current.Session.Timeout = HttpContext.Current.Session.Timeout; 
        }

        public static void Save(UserInformation userInformation)
        {
            Save(nameof(UserInformation), userInformation);
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public static int GetUserId()
        {
            return Get<UserInformation>(nameof(UserInformation)).UserId;
        }

        public static int? GetApplicationId()
        {
            return Get<UserInformation>(nameof(UserInformation)).ApplicationId;
        }

        public static int? GetUnitId()
        {
            return Get<UserInformation>(nameof(UserInformation)).UnitId;
        }

    }
}